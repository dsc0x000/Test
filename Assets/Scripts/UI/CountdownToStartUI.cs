using TMPro;
using UnityEngine;

public sealed class CountdownToStartUI : MonoBehaviour, IUIHandler
{
    [SerializeField] private TextMeshProUGUI timerText;
    private void Update()
    {
        float timeToStart = GameManager.Instance.CountdownToStartTimer;
        float secondFraction = 1f - timeToStart % 1f;
        float secondValue = Mathf.Sin(Mathf.PI * secondFraction);

        timerText.alpha = secondValue;
        timerText.transform.localScale = Vector3.one * secondValue;
        timerText.text = Mathf.CeilToInt(timeToStart).ToString();
    }

    public void UpdateVisibility(GameState state)
    {
        if (state == GameState.CountdownToStart)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}