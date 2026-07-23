using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerCombat combat;
    private PlayerParry parry;
    private PlayerRage rage;
    private PlayerStats stats;

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

    public void OnParry()
    {
        parry.StartParry();
    }
}
