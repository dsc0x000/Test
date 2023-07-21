using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public event Action TaskUpdate, AttemptsUpdate;
    public event Action PlayButtonPressed, ExitButtonPressed;
    
    [SerializeField] private TitleScreenUI titleScreen;
    [SerializeField] private CountdownToStartUI countdownToStartUI;
    [SerializeField] private PlayingTimerUI playingTimerUI;
    [SerializeField] private TaskManagerUI taskManagerUI;
    [SerializeField] private AttemptsUI attemptsUI;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);

        Instance = this;
    }

    private void UpdateUIByState(GameState state)
    {
        titleScreen.UpdateVisibility(state);
        countdownToStartUI.UpdateVisibility(state);
        playingTimerUI.UpdateVisibility(state);
        taskManagerUI.UpdateVisibility(state);
        attemptsUI.UpdateVisibility(state);
    }

    private void GameManager_OnStartUp() => titleScreen.SetStartUpText();
    private void GameManager_OnGameOver(GameOverType type)
    {
        if (type == GameOverType.LevelComplete)
            titleScreen.SetCompleteText();
        else
            titleScreen.SetFailedText();
    }

    private void GameManager_OnGameStateChanged(GameState state) => UpdateUIByState(state);
    private void TaskManager_OnTaskUpdate() => TaskUpdate?.Invoke();
    private void TaskManager_AttemptsUpdate() => AttemptsUpdate?.Invoke();
    public void UI_OnPlayButtonPressed() => PlayButtonPressed?.Invoke();
    public void UI_OnExitButtonPressed() => ExitButtonPressed?.Invoke();


    private void OnEnable()
    {
        TaskManager.Instance.TaskGenerated += TaskManager_OnTaskUpdate;
        TaskManager.Instance.StoredCorrectItem += TaskManager_OnTaskUpdate;
        TaskManager.Instance.StoredWrongItem += TaskManager_AttemptsUpdate;

        GameManager.Instance.StartUp += GameManager_OnStartUp;
        GameManager.Instance.GameOver += GameManager_OnGameOver;
        GameManager.Instance.GameStateChanged += GameManager_OnGameStateChanged;
    }

    private void OnDisable()
    {
        TaskManager.Instance.TaskGenerated -= TaskManager_OnTaskUpdate;
        TaskManager.Instance.StoredCorrectItem -= TaskManager_OnTaskUpdate;
        TaskManager.Instance.StoredWrongItem -= TaskManager_AttemptsUpdate;

        GameManager.Instance.StartUp -= GameManager_OnStartUp;
        GameManager.Instance.GameOver -= GameManager_OnGameOver;
        GameManager.Instance.GameStateChanged -= GameManager_OnGameStateChanged;
    }
}
