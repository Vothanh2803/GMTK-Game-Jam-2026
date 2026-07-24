using UnityEngine;
using TMPro;

public class EnemyInfoUI : MonoBehaviour
{
    [Header("Stat Bar Reference")]
    [SerializeField] private UI_StatBar enemyHealthBar;

    [Header("UI Text References")]
    [SerializeField] private TMP_Text enemyHealthText;

    private EnemyController lastTrackedEnemy;

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (EnemyManager.Instance == null) return;

        EnemyController currentEnemy = EnemyManager.Instance.CurrentEnemy;

        if (currentEnemy == null)
        {
            if (enemyHealthText != null) enemyHealthText.text = "HP: 0 / 0";
            if (enemyHealthBar != null) enemyHealthBar.UpdateBar(0, 100);
            lastTrackedEnemy = null;
            return;
        }

        if (currentEnemy != lastTrackedEnemy)
        {
            lastTrackedEnemy = currentEnemy;
            if (enemyHealthBar != null)
            {
                enemyHealthBar.Initialize(currentEnemy.CurrentHP, currentEnemy.MaxHP);
            }
        }

        float currentHP = currentEnemy.CurrentHP;
        float maxHP = currentEnemy.MaxHP;

        if (enemyHealthBar != null)
        {
            enemyHealthBar.UpdateBar(currentHP, maxHP);
        }

        if (enemyHealthText != null)
        {
            enemyHealthText.text = $"Enemy HP: {Mathf.Max(0, currentHP)} / {maxHP}";
        }
    }
}