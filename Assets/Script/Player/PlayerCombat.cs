using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Attack Cost")]
    [SerializeField] private int lightAttackCost = 2;
    [SerializeField] private int baseHeavyAttackCost = 10;
    [SerializeField] private int heavyCostIncrement = 2;

    private int currentHeavyCost;    
    private PlayerStats playerStats;

    private PlayerController playerController;

    public bool IsBlocking { get; private set; } = false;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerController = GetComponent<PlayerController>();
        currentHeavyCost = baseHeavyAttackCost;
    }

    public void LightAttack()
    {
        if (GameManager.Instance.CurrentState != GameState.PlayerTurn) return;

        if (!playerStats.ConsumeActionPoint(lightAttackCost))
        {
            Debug.Log("Không đủ Action Point!");
            return;
        }

        IsBlocking = false;
        Debug.Log("Light Attack");
        
        playerController.stats.AddRage(5);
        GameManager.Instance.SendAttack("Enemy", 10);

        if (GameManager.Instance.CurrentState != GameState.Win && GameManager.Instance.CurrentState != GameState.Lose)
            GameManager.Instance.ChangeState(GameState.EnemyTurn);
        
    }

    public void HeavyAttack()
    {
        if (GameManager.Instance.CurrentState != GameState.PlayerTurn) return;

        if (!playerStats.ConsumeActionPoint(baseHeavyAttackCost))
        {
            Debug.Log("Không đủ Action Point!");
            return;
        }

        IsBlocking = false;
        Debug.Log("Heavy Attack");

        playerController.stats.AddRage(10);
        GameManager.Instance.SendAttack("Enemy", 20);

        currentHeavyCost += heavyCostIncrement;
        

        if (GameManager.Instance.CurrentState != GameState.Win && GameManager.Instance.CurrentState != GameState.Lose)
            GameManager.Instance.ChangeState(GameState.EnemyTurn);

    }

    public void RageAttack()
    {
        if (GameManager.Instance.CurrentState != GameState.PlayerTurn) return;
        
        if (playerStats.CurrentRage < playerStats.MaxRage)
        {
            Debug.Log("Rage chưa đay!");
            return;
        }

        playerStats.ConsumeRage();

        Debug.Log("Rage Attack");
        GameManager.Instance.SendAttack("Enemy", 40);

        if (GameManager.Instance.CurrentState != GameState.Win && GameManager.Instance.CurrentState != GameState.Lose)
            GameManager.Instance.ChangeState(GameState.EnemyTurn);
    }

    public void Block()
    {
        if (GameManager.Instance.CurrentState != GameState.PlayerTurn) return;

        IsBlocking = true;
        Debug.Log("Do don");

        if (GameManager.Instance.CurrentState != GameState.Win && GameManager.Instance.CurrentState != GameState.Lose)
            GameManager.Instance.ChangeState(GameState.EnemyTurn);
    }

    public void ResetBlockState()
    {
        IsBlocking = false;
    }

}
