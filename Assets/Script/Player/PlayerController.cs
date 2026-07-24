using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerCombat combat;
    public PlayerParry parry;
    private PlayerRage rage;
    public PlayerStats stats;

    private void Awake()
    {
        combat = GetComponent<PlayerCombat>();
        parry = GetComponent<PlayerParry>();
        rage = GetComponent<PlayerRage>();
        stats = GetComponent<PlayerStats>();
    }

    public void OnLightAttack()
    {
        combat.LightAttack();
    }

    public void OnHeavyAttack()
    {
        combat.HeavyAttack();
    }

    public void OnRageAttack()
    {
        combat.RageAttack();
    }

    public void OnBlock()
    {
        combat.Block();
    }
}
