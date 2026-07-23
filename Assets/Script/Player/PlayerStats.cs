using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Action Point")]
    [SerializeField] private int maxActionPoint = 10;
    [SerializeField] private int currentActionPoint;

    [Header("Rage")]
    [SerializeField] private int maxRage = 100;
    [SerializeField] private int currentRage;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int CurrentActionPoint => currentActionPoint;
    public int MaxActionPoint => maxActionPoint;

    public int CurrentRage => currentRage;
    public int MaxRage => maxRage;


    private void Awake()
    {
        currentHealth = maxHealth;
        currentActionPoint = maxActionPoint;
        currentRage = 0;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public void AddActionPoint(int amount)
    {
        currentActionPoint = Mathf.Min(currentActionPoint + amount, maxActionPoint);
    }

    public bool ConsumeActionPoint(int amount)
    {
        if (currentActionPoint < amount)
            return false;

        currentActionPoint -= amount;
        return true;
    }

    public void AddRage(int amount)
    {
        currentRage = Mathf.Min(currentRage + amount, maxRage);
    }

    public void ConsumeRage()
    {
        currentRage = 0;
    }

}
