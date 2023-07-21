using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemID id;

    private Rigidbody rb;
    public ItemID ID => id;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();   
    }

    public void Pick()
    {
        rb.isKinematic = true;
    }

    public static ItemID GetRandomID()
    {
        ItemID[] itemIDs = (ItemID[])Enum.GetValues(typeof(ItemID));
        int index = UnityEngine.Random.Range(0, itemIDs.Length);
        return itemIDs[index];
    }
}
