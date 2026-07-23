using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game States")]
    [SerializeField] private GameState currentState;

    [Header("References")]
    // [SerializeField] private PlayerController player;
    [SerializeField] private EnemyManager enemy;

    public GameState CurrentState => currentState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ChangeState(GameState.Init);
    }


    //Goi moi khi player hoac enemy ket thuc luot, vd : GameManager.ChangeState(GameState.EnemyTurn);
    public void ChangeState(GameState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case GameState.Init:
                HandleInitState();
                break;

            case GameState.PlayerTurn:
                HandlePlayerTurn();
                break;

            case GameState.EnemyTurn:
                HandleEnemyTurn();
                break;

            case GameState.Win:
                HandleWinState();
                break;

            case GameState.Lose:
                HandleLoseState();
                break;
        }
    }

    private void HandleInitState()
    {
        Debug.Log("Init state");

        if (enemy != null)
        {
            enemy.SpawnNextEnemy();
        }

        ChangeState(GameState.EnemyTurn);
    }

    private void HandlePlayerTurn()
    {
        Debug.Log("Player Turn");
    }

    private void HandleEnemyTurn()
    {
        Debug.Log("Enemy Turn");

        if (enemy != null)
        {
            enemy.StartEnemyTurn();
        }
    }

    private void HandleWinState()
    {
        Debug.Log("Win State");
    }

    private void HandleLoseState()
    {
        Debug.Log("Lose state");
    }

    public void SendAttack(string targetName, float damage)
    {
        if (currentState == GameState.Win || currentState == GameState.Lose) return;

        if (targetName == "Enemy")
        {
            if (enemy != null)
                enemy.ApplyDamage(damage);
            Debug.Log("Apply damage on enemy");
        }

        if (targetName == "Player")
        {
            //if (player != null) 
                //apply damage on player
            Debug.Log("Apply damage on player");
        }
    }

}
