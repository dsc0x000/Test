using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public event Action<ItemID> ItemStored;
    
    [SerializeField] private LayerMask grabbableMask;
    [SerializeField] private Basket basket;
    [SerializeField] private Transform grabIKTarget;
    [SerializeField] private Transform placementIKTarget;
    [SerializeField] private Transform grabbedItemParent;

    [SerializeField] private Vector3 grabbableZoneSize;
    [SerializeField] private Vector3 grabbableZoneOffset;

    private Camera mainCamera;
    private Animator animator;

    private int animatorIdleHash;
    private int animatorGrabHash;
    private int animatorDanceHash;
    private int animatorFailHash;

    private Item pickedItem = null;
    private bool isFollowingItem;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        animatorIdleHash = Animator.StringToHash("Idle");
        animatorGrabHash = Animator.StringToHash("Grab");
        animatorDanceHash = Animator.StringToHash("Dance");
        animatorFailHash = Animator.StringToHash("Fail");
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (Input.touchCount > 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out var hitInfo, 100f, grabbableMask, QueryTriggerInteraction.Ignore))
            {
                if (hitInfo.transform.TryGetComponent<Item>(out var itemToPick) == false)
                    return;

                if (CanPickItem(itemToPick))
                    PickItem(itemToPick);
            }
        }
    }

    private bool CanPickItem(Item itemToCollect)
    {
        if (pickedItem != null)
            return false;

        Vector3 grabbableZonePosition = transform.position + transform.right * grabbableZoneOffset.x +
            transform.up * grabbableZoneOffset.y + transform.forward * grabbableZoneOffset.z;

        Collider[] hits = Physics.OverlapBox(grabbableZonePosition, grabbableZoneSize / 2f, 
            Quaternion.identity, grabbableMask, QueryTriggerInteraction.Ignore);

        foreach (var collider in hits)
        {
            if (itemToCollect.transform == collider.transform)
                return true;
        }

        return false;
    }

    private void PickItem(Item itemToPick)
    {
        pickedItem = itemToPick;

        StartCoroutine(FollowItemRoutine(grabIKTarget, itemToPick.transform));

        animator.SetTrigger(animatorGrabHash);
        placementIKTarget.position = basket.GetFreeSlotPosition();
    }

    private void Animator_OnItemPicked()
    {
        pickedItem.Pick();
        pickedItem.transform.SetParent(grabbedItemParent);
        pickedItem.transform.localPosition = Vector3.zero;
    }

    private void Animator_OnItemPlaced()
    {
        basket.StoreItem(pickedItem);
        isFollowingItem = false;

        pickedItem = null;
    }

    private IEnumerator FollowItemRoutine(Transform obj, Transform target)
    {
        isFollowingItem = true;

        while (isFollowingItem)
        {
            obj.position = target.position;
            yield return null;
        }  
    }

    private void Basket_OnItemStored(ItemID itemID)
    {
        ItemStored?.Invoke(itemID);
    }

    private void GameManager_OnNewGameStarted()
    {
        animator.SetTrigger(animatorIdleHash);
        basket.Clear();
    }

    private void GameManager_OnGameOver(GameOverType type)
    {
        if (type == GameOverType.LevelComplete)
            animator.SetTrigger(animatorDanceHash);
        else
            animator.SetTrigger(animatorFailHash);
    }

    private void OnEnable()
    {
        GameManager.Instance.GameOver += GameManager_OnGameOver;
        GameManager.Instance.NewGameStarted += GameManager_OnNewGameStarted;
        basket.ItemStored += Basket_OnItemStored;
    }

    private void OnDisable()
    {
        GameManager.Instance.GameOver -= GameManager_OnGameOver;
        GameManager.Instance.NewGameStarted -= GameManager_OnNewGameStarted;
        basket.ItemStored -= Basket_OnItemStored;
    }

    private void OnDrawGizmos()
    {
        Vector3 grabbableZonePosition = transform.position + transform.right * grabbableZoneOffset.x +
            transform.up * grabbableZoneOffset.y + transform.forward * grabbableZoneOffset.z;

        Gizmos.color = new Color(0f, 1f, 0f, .25f);
        Gizmos.DrawCube(grabbableZonePosition, grabbableZoneSize);
    }
}