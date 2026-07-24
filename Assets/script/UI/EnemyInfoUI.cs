using UnityEngine;
using TMPro;

public class EnemyInfoUI : MonoBehaviour
{
    [Header("Stat Bar Reference")]
    [SerializeField] private UI_StatBar enemyHealthBar; // Kéo UI_StatBar của Enemy vào đây

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

        // Nếu không có quái (Đã thắng hoặc chưa spawn)
        if (currentEnemy == null)
        {
            if (enemyHealthText != null) enemyHealthText.text = "HP: 0 / 0";
            if (enemyHealthBar != null) enemyHealthBar.UpdateBar(0, 100);
            lastTrackedEnemy = null;
            return;
        }

        // --- KIỂM TRA NẾU LÀ QUÁI MỚI SPAWN ---
        if (currentEnemy != lastTrackedEnemy)
        {
            lastTrackedEnemy = currentEnemy;
            if (enemyHealthBar != null)
            {
                // Khởi tạo ngay lập tức thanh HP đầy cho con quái mới
                enemyHealthBar.Initialize(currentEnemy.CurrentHP, currentEnemy.MaxHP);
            }
        }

        float currentHP = currentEnemy.CurrentHP;
        float maxHP = currentEnemy.MaxHP;

        // --- CẬP NHẬT THANH HP MÁU ÁO & GIẬT ---
        if (enemyHealthBar != null)
        {
            enemyHealthBar.UpdateBar(currentHP, maxHP);
        }

        // --- CẬP NHẬT TEXT ---
        if (enemyHealthText != null)
        {
            enemyHealthText.text = $"Enemy HP: {Mathf.Max(0, currentHP)} / {maxHP}";
        }
    }
}