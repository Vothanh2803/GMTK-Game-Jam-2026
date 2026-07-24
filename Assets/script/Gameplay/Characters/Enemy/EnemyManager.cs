using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [SerializeField] private LevelData levelData;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public Transform attackPoint;

    [SerializeField] private EnemyController currentEnemy;
    private int enemyIndex = 0;

    public EnemyController CurrentEnemy => currentEnemy;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SpawnNextEnemy()
    {
        if (levelData == null || enemyIndex >= levelData.enemyList.Length)
        {
            Debug.Log("Da tieu diet het quai");
            GameManager.Instance.ChangeState(GameState.Win);
            return;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetAVSpeedForNewEnemy();
        }

        EnemyData enemyData = levelData.enemyList[enemyIndex];

        GameObject spawnedGO = Instantiate(enemyData.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        currentEnemy = spawnedGO.GetComponent<EnemyController>();

        if (currentEnemy != null)
        {
            currentEnemy.Init(enemyData);

            currentEnemy.OnEnemyDeath += HandleEnemyDeath;
            currentEnemy.OnTurnCompleted += HandleEnemyTurnCompleted;
        }
    }

    public void StartEnemyTurn()
    {
        if (currentEnemy != null)
        {
            currentEnemy.DoCombo();
        }
        else
        {
            GameManager.Instance.ChangeState(GameState.PlayerTurn);
        }
    }

    public void ApplyDamage(float damage)
    {
        if (currentEnemy != null)
        {
            currentEnemy.TakeDamage(damage);
        }
    }

    public void ApplyParryDamagePercent(float percent)
    {
        if (currentEnemy != null)
        {
            currentEnemy.TakePercentDamage(percent);
        }
    }

    private void HandleEnemyDeath()
    {
        if (currentEnemy != null)
        {
            currentEnemy.OnEnemyDeath -= HandleEnemyDeath;
            currentEnemy.OnTurnCompleted -= HandleEnemyTurnCompleted;
        }

        enemyIndex++;
        SpawnNextEnemy();
    }

    private void HandleEnemyTurnCompleted()
    {
        Debug.Log("combo done!");
        GameManager.Instance.OnEnemyTurnCompleted();
    }
}
