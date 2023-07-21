using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action<GameState> GameStateChanged;
    public event Action<GameOverType> GameOver;
    public event Action StartUp, NewGameStarted;

    [SerializeField] private GameConfig config;
    [SerializeField] private UIManager mainUI;
    [SerializeField] private Player player;
    private GameState currentState;
    
    public static GameManager Instance { get; private set; }
    public GameState CurrentState
    {   
        get => currentState;
        private set
        {
            currentState = value;
            GameStateChanged?.Invoke(currentState);
        }
    }

    public float CountdownToStartTimer { get; private set; }
    public float PlayingTimer { get; private set; }
    public float PlayingTime => config.playingTime;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);

        Instance = this;
        Initialize();
    }

    private void Start()
    {
        CurrentState = GameState.WaitingToStart;
        StartUp?.Invoke();
    }

    private void Update()
    {
        if (CurrentState == GameState.CountdownToStart)
        {
            CountdownToStartTimer -= Time.deltaTime;

            if (CountdownToStartTimer <= 0f)
            {
                PlayingTimer = config.playingTime;
                CurrentState = GameState.Playing;
            }
        }

        if (CurrentState == GameState.Playing)
        {
            PlayingTimer -= Time.deltaTime;

            if (PlayingTimer <= 0f)
            {
                PlayingTimer = 0f;
                FinishGame(GameOverType.LevelFailed);
            }
        }
    }

    private void TaskManager_OnTaskComplete() => FinishGame(GameOverType.LevelComplete);
    private void TaskManager_OnTaskFailed() => FinishGame(GameOverType.LevelFailed);

    private void FinishGame(GameOverType type)
    {
        GameOver?.Invoke(type);
        CurrentState = GameState.WaitingToStart;
    }

    private void UI_OnPlayButtonPressed()
    {
        CountdownToStartTimer = config.countdownToStartTime;
        PlayingTimer = config.playingTime;

        CurrentState = GameState.CountdownToStart;
        NewGameStarted?.Invoke();
    }

    private void UI_OnExitButtonPressed()
    {
        Application.Quit();
    }

    private void Initialize()
    {
        TaskManager.Properties tmInitProps = new TaskManager.Properties()
        {
            minItemToCollectCount = config.minItemToCollectCount,
            maxItemToCollectCount = config.maxItemToCollectCount,
            attempts = config.attempts,
            player = player
        };

        TaskManager.Instance.Initialize(tmInitProps);
        ItemDataProvider.Instance.InitializeAndLoad();
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        mainUI.PlayButtonPressed += UI_OnPlayButtonPressed;
        mainUI.ExitButtonPressed += UI_OnExitButtonPressed;
        TaskManager.Instance.TaskComplete += TaskManager_OnTaskComplete;
        TaskManager.Instance.TaskFailed += TaskManager_OnTaskFailed;
    }

    private void OnDisable()
    {
        mainUI.PlayButtonPressed -= UI_OnPlayButtonPressed;
        mainUI.ExitButtonPressed -= UI_OnExitButtonPressed;
        TaskManager.Instance.TaskComplete -= TaskManager_OnTaskComplete;
        TaskManager.Instance.TaskFailed -= TaskManager_OnTaskFailed;
    } 
}
