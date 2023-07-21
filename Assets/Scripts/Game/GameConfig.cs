using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig")]
public class GameConfig : ScriptableObject
{
    [Space, Header("Time:")]
    public float playingTime;
    public float countdownToStartTime;

    [Space, Header("Tasks:")]
    public int minItemToCollectCount;
    public int maxItemToCollectCount;
    public int attempts;

    private void OnValidate()
    {
        if (minItemToCollectCount <= 0)
            minItemToCollectCount = 1;

        if (maxItemToCollectCount < minItemToCollectCount)
            maxItemToCollectCount = minItemToCollectCount;

        if (playingTime < 0)
            playingTime = 0;

        if (countdownToStartTime < 0)
            countdownToStartTime = 0;

        if (attempts < 1)
            attempts = 1;

        if (attempts > 5)
            attempts = 5;
    }
}

