using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private EnemyController currentEnemy;
    private int enemyIndex = 0;

    public EnemyController CurrentEnemy => currentEnemy;

    public void SpawnNextEnemy()
    {
        if (levelData == null || enemyIndex >= levelData.enemyList.Length)
        {
            Debug.Log("Da tieu diet het quai");
            GameManager.Instance.ChangeState(GameState.Win);
            return;
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
            Debug.Log("Do combo");
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
        GameManager.Instance.ChangeState(GameState.PlayerTurn);
    }
}
