using UnityEngine;

public class PlayerRage : MonoBehaviour
{
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    public void AddRage(int amount)
    {
        playerStats.AddRage(amount);
    }

    public bool CanUseRage()
    {
        return playerStats.CurrentRage >= playerStats.MaxRage;
    }

    public bool UseRage()
    {
        if (!CanUseRage())
            return false;

        playerStats.ConsumeRage();
        return true;
    }

}
