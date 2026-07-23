using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyInfoUI : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TMP_Text enemyHealthText;

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
            if (enemyHealthText != null) enemyHealthText.text = "0";
            return;
        }

        float currentHP = currentEnemy.CurrentHP;
        float maxHP = currentEnemy.MaxHP;

        if (enemyHealthText != null)
        {
            enemyHealthText.text = $"Enemy HP: {currentHP} / {maxHP}";
        }
    }
}