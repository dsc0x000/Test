using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class PlayingTimerUI : MonoBehaviour, IUIHandler
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI timeText;

    private void Update()
    {
        int timeRemaining = Mathf.FloorToInt(GameManager.Instance.PlayingTimer);
        float timeNormalized = timeRemaining / GameManager.Instance.PlayingTime;

        fillImage.fillAmount = timeNormalized;
        timeText.text = timeRemaining.ToString();
    }

    public void UpdateVisibility(GameState state)
    {
        if (state == GameState.Playing)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}
