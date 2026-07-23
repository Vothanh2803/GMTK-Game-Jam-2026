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
    }

    public void HeavyAttack()
    {
        if (!playerStats.ConsumeActionPoint(heavyAttackCost))
        {
            Debug.Log("Không đủ Action Point!");
            return;
        }

        Debug.Log("Heavy Attack");
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
    }

}
