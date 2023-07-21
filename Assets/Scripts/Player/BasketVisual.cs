using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Basket))]
public class BasketVisual : MonoBehaviour
{
    [SerializeField] private TextMeshPro popUpTextPrefab;
    [SerializeField] private float popUpTime;
    private Basket basket;

    private void Awake()
    {
        basket = GetComponent<Basket>();
    }

    private void SpawnPlusOnePopUpText() => StartCoroutine(PopUpRoutine("+1", Color.green));
    private void SpawnFailPopUpText() => StartCoroutine(PopUpRoutine("Fail", Color.red));

    private IEnumerator PopUpRoutine(string text, Color textColor)
    {
        TextMeshPro popUpText = Instantiate(popUpTextPrefab, basket.transform.position, Quaternion.identity);

        popUpText.alpha = 0f;
        popUpText.text = text;
        popUpText.color = textColor;

        float popUpTimer = popUpTime;

        while (popUpTimer > 0f)
        {
            float process = 1f - popUpTimer / popUpTime;

            popUpText.alpha = Mathf.Sin(Mathf.PI * process);
            popUpText.transform.Translate(Vector3.up * Time.deltaTime);
            
            popUpTimer -= Time.deltaTime;
            yield return null;
        }

        Destroy(popUpText.gameObject);
    }

    private void OnEnable()
    {
        TaskManager.Instance.StoredCorrectItem += SpawnPlusOnePopUpText;
        TaskManager.Instance.StoredWrongItem += SpawnFailPopUpText;
    }

    private void OnDisable()
    {
        TaskManager.Instance.StoredCorrectItem -= SpawnPlusOnePopUpText;
        TaskManager.Instance.StoredWrongItem -= SpawnFailPopUpText;
    }
}
