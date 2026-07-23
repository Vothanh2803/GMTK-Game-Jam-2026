using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Attack Cost")]
    [SerializeField] private int lightAttackCost = 2;
    [SerializeField] private int heavyAttackCost = 4;

    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    public void LightAttack()
    {
        if (!playerStats.ConsumeActionPoint(lightAttackCost))
        {
            Debug.Log("Không đủ Action Point!");
            return;
        }

        Debug.Log("Light Attack");
        GameManager.Instance.SendAttack("Enemy", 10);
        if (GameManager.Instance.CurrentState != GameState.Win && GameManager.Instance.CurrentState != GameState.Lose)
            GameManager.Instance.ChangeState(GameState.EnemyTurn);
    }

    public void HeavyAttack()
    {
        if (!playerStats.ConsumeActionPoint(heavyAttackCost))
        {
            Debug.Log("Không đủ Action Point!");
            return;
        }

        Debug.Log("Heavy Attack");
        GameManager.Instance.SendAttack("Enemy", 20);

        if (GameManager.Instance.CurrentState != GameState.Win && GameManager.Instance.CurrentState != GameState.Lose)
            GameManager.Instance.ChangeState(GameState.EnemyTurn);
    }

    public void RageAttack()
    {
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

    public void SkipTurn()
    {
        Debug.Log("Skip Turn");
        if (GameManager.Instance.CurrentState != GameState.Win && GameManager.Instance.CurrentState != GameState.Lose)
            GameManager.Instance.ChangeState(GameState.EnemyTurn);
    }

}
