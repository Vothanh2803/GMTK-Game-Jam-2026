using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game States")]
    [SerializeField] private GameState currentState;

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyManager enemy;

    [Header("AV / Time Settings")]
    [SerializeField] private float initialAvInterval = 1.5f;
    [SerializeField] private float minAvInterval = 0.5f; 
    [SerializeField] private float avIntervalStep = 0.25f;

    private float currentAvInterval;
    private float avTimer = 0f;
    private int playerTurnCount = 0;
    private bool isLastTurn = false;

    [Header("Endgame setting")]
    [SerializeField] private float delayBeforeChangeScene = 2.0f;
    [SerializeField] private string menuSceneName = "Menu";

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

    private void Update()
    {
        if (currentState == GameState.PlayerTurn)
        {
            avTimer += Time.deltaTime;
            if (avTimer >= currentAvInterval)
            {
                avTimer -= currentAvInterval;
                
                bool hasAV = player.stats.ConsumeActionPoint(1);

                if (player.stats.CurrentActionPoint <= 0 && !isLastTurn)
                {
                    isLastTurn = true;
                    Debug.Log("THIS IS YOUR LAST TURN!");
                }
            }
        }
    }


    //Goi moi khi player hoac enemy ket thuc luot, vd : GameManager.ChangeState(GameState.EnemyTurn);
    public void ChangeState(GameState newState)
    {
        if (newState == GameState.EnemyTurn && enemy != null && enemy.CurrentEnemy != null && enemy.CurrentEnemy.IsDead)
        {
            return;
        }

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

        ChangeState(GameState.PlayerTurn);
    }

    private void HandlePlayerTurn()
    {
        playerTurnCount++;
        
        currentAvInterval = Mathf.Max(initialAvInterval - (playerTurnCount - 1) * avIntervalStep, minAvInterval);
        avTimer = 0f;
        player.isAttackedThisTurn = false;

        Debug.Log($"Player Turn {playerTurnCount} | Tốc độ trừ AV: 1 AV mỗi {currentAvInterval:F1}s");
    }

    private void HandleEnemyTurn()
    {
        Debug.Log("Enemy Turn");

        if (enemy != null)
        {
            enemy.StartEnemyTurn();
        }
    }

    public void OnEnemyTurnCompleted()
    {
        if (enemy != null && (enemy.CurrentEnemy == null || enemy.CurrentEnemy.CurrentHP <= 0))
        {
            Debug.Log("Enemy đã chết trong lượt đánh của nó!");
            return;
        }

        if (isLastTurn)
        {
            if (enemy != null && enemy.CurrentEnemy != null && enemy.CurrentEnemy.CurrentHP > 0)
            {
                Debug.Log("Enemy chua chet sau last turn");
                ChangeState(GameState.Lose);
            }
            else
            {
                Debug.Log("Enemy da chet sau last turn");
                ChangeState(GameState.Win);
            }
            return;
        }

        ChangeState(GameState.PlayerTurn);
    }

    public void ResetAVSpeedForNewEnemy()
    {
        playerTurnCount = 0;
        currentAvInterval = initialAvInterval;
        avTimer = 0f;
    }

    private void HandleWinState()
    {
        Debug.Log("Win State");
        StartCoroutine(EndGameSequence());
    }


    private void HandleLoseState()
    {
        Debug.Log("YOU LOSE!");
        StartCoroutine(EndGameSequence());
    }

    private IEnumerator EndGameSequence()
    {
        yield return new WaitForSeconds(0.75f);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(delayBeforeChangeScene);
        
        if (SceneController.Instance != null)
        {
            SceneController.Instance.ChangeScene(menuSceneName);
        }
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
            player.parry.TakeDamageWithParry(damage);
            Debug.Log("Apply damage on player");
        }
    }

}
