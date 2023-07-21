using TMPro;
using UnityEngine;

public class TitleScreenUI : MonoBehaviour, IUIHandler
{
    [SerializeField] private TitleScreenConfig config;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private TextMeshProUGUI playButtonText;
    [SerializeField] private TextMeshProUGUI exitButtonText;
 
    public void UpdateVisibility(GameState state)
    {
        if (state == GameState.WaitingToStart)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public void SetStartUpText()
    {
        titleText.text = config.startUpTitleText;
        infoText.text = config.startUpInfoText;
        playButtonText.text = config.startUpPlayButtonText;
        exitButtonText.text = config.startUpExitButtonText;
    }
    public void SetCompleteText()
    {
        titleText.text = config.lvlCompleteTitleText;
        infoText.text = config.lvlCompleteInfoText;
        playButtonText.text = config.lvlCompletePlayButtonText;
        exitButtonText.text = config.lvlCompleteExitButtonText;
    }
    public void SetFailedText()
    {
        titleText.text = config.lvlFailedTitleText;
        infoText.text = config.lvlFailedInfoText;
        playButtonText.text = config.lvlFailedPlayButtonText;
        exitButtonText.text = config.lvlFailedExitButtonText;
    }
}
