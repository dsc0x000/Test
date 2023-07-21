using UnityEngine;

[CreateAssetMenu(fileName = "TitleScreenConfig", menuName = "ScriptableObjects/TitleScreenConfig")]
public class TitleScreenConfig : ScriptableObject
{
    [Header("Start Up")]
    public string startUpTitleText;
    public string startUpInfoText;
    public string startUpPlayButtonText;
    public string startUpExitButtonText;

    [Header("Level Complete")]
    public string lvlCompleteTitleText;
    public string lvlCompleteInfoText;
    public string lvlCompletePlayButtonText;
    public string lvlCompleteExitButtonText;

    [Header("Level Failed")]
    public string lvlFailedTitleText;
    public string lvlFailedInfoText;
    public string lvlFailedPlayButtonText;
    public string lvlFailedExitButtonText;
}