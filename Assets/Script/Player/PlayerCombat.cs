using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack Cost")]
    [SerializeField] private int lightAttackCost = 2;
    [SerializeField] private int baseHeavyAttackCost = 10;
    [SerializeField] private int heavyCostIncrement = 2;

    private int currentHeavyCost;    
    private PlayerStats playerStats;
    private PlayerController playerController;
    private PlayerSFXController sfxController;

    public bool IsBlocking { get; private set; } = false;
    public int CurrentHeavyCost => currentHeavyCost;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerController = GetComponent<PlayerController>();
        sfxController = GetComponent<PlayerSFXController>();
        currentHeavyCost = baseHeavyAttackCost;
    }

    public bool CanLightAttack()
    {
        if (GameManager.Instance.CurrentState != GameState.PlayerTurn) return false;
        if (playerStats.CurrentActionPoint < lightAttackCost)
        {
            Debug.Log("Khong du AV cho LightAttack");
            return false;
        }
        return true;
    }

    public bool CanHeavyAttack()
    {
        if (GameManager.Instance.CurrentState != GameState.PlayerTurn) return false;
        if (playerStats.CurrentActionPoint < currentHeavyCost)
        {
            Debug.Log("Khong du AV cho HeavyAttack");
            return false;
        }
        return true;
    }

    public bool CanPerformRageAttack()
    {
        if (GameManager.Instance.CurrentState != GameState.PlayerTurn) return false;
        if (playerStats == null || playerStats.CurrentRage < playerStats.MaxRage)
        {
            return false;
        }
        return true;
    }

    public void LightAttack()
    {
        if (!playerStats.ConsumeActionPoint(lightAttackCost)) return;

        IsBlocking = false;
        Debug.Log("Light Attack");
        
        playerController.stats.AddRage(10);
        GameManager.Instance.SendAttack("Enemy", 10);
    }

    public void HeavyAttack()
    {
        if (!playerStats.ConsumeActionPoint(currentHeavyCost)) return;

        IsBlocking = false;
        Debug.Log("Heavy Attack");

        playerController.stats.AddRage(25);
        GameManager.Instance.SendAttack("Enemy", 20);

        currentHeavyCost += heavyCostIncrement;
    }

    public void RageAttack()
    {
        IsBlocking = false;
        playerStats.ConsumeRage();
        Debug.Log("Bắt đầu thi triển Rage Attack 3 Hit!");
    }

    public void ExecuteRageSingleHitDamage()
    {
        float damagePerHit = 20;
        GameManager.Instance.SendAttack("Enemy", damagePerHit);
        Debug.Log($"Rage Hit! Gây {damagePerHit} damage");
    }

    public void Block()
    {
        if (GameManager.Instance.CurrentState != GameState.PlayerTurn) return;

        IsBlocking = true;
        Debug.Log("Do don");

        sfxController.PlayBlockSound();

        if (GameManager.Instance.CurrentState != GameState.Win && GameManager.Instance.CurrentState != GameState.Lose)
            GameManager.Instance.ChangeState(GameState.EnemyTurn);
    }

    public void ResetBlockState()
    {
        IsBlocking = false;
    }
}