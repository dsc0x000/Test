using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    [SerializeField] private float conveyorSpeed;
    [SerializeField] private float onGameOverYMovement;

    private Rigidbody rb;
    private ObjectTransformer transformer;
    private Vector3 initialPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        transformer = GetComponent<ObjectTransformer>();
        initialPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 initialPosition = rb.position;

        rb.position -= transform.right * conveyorSpeed * Time.fixedDeltaTime;
        rb.MovePosition(initialPosition);
    }

    private void GameManager_OnNewGameStart()
    {
        transformer.PerformMovement(initialPosition);
    }

    private void GameManager_OnGameOver(GameOverType type)
    {
        transformer.PerformMovement(initialPosition + Vector3.up * onGameOverYMovement);
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
