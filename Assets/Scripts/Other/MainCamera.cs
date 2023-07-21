using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform endTransform;
    
    private ObjectTransformer transformer;

    private void Awake()
    {
        transformer = GetComponent<ObjectTransformer>();
    }

    private void GameManager_OnNewGameStart()
    {
        transformer.PerformMovement(startTransform.position);
        transformer.PerformRotation(startTransform.rotation);
    }

    private void GameManager_OnGameOver(GameOverType type)
    {
        transformer.PerformMovement(endTransform.position);
        transformer.PerformRotation(endTransform.rotation);
    }

    private void OnEnable()
    {
        GameManager.Instance.NewGameStarted += GameManager_OnNewGameStart;
        GameManager.Instance.GameOver += GameManager_OnGameOver;
    }

    private void OnDisable()
    {
        GameManager.Instance.NewGameStarted -= GameManager_OnNewGameStart;
        GameManager.Instance.GameOver -= GameManager_OnGameOver;
    }
}
