using UnityEngine;
using TMPro;

public class PlayerInfoUI : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private PlayerController playerController;

    [Header("Stat Bars")]
    [SerializeField] private UI_StatBar healthBar;      // Kéo UI_StatBar của HP vào đây
    [SerializeField] private UI_StatBar actionPointBar; // Kéo UI_StatBar của AP vào đây (MỚI)
    [SerializeField] private UI_StatBar rageBar;        // Kéo UI_StatBar của Rage vào đây

    [Header("UI Text References")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text actionPointText;
    [SerializeField] private TMP_Text rageText;
    [SerializeField] private TMP_Text parryEnergyText;

    private PlayerParry playerParry;
    private bool isInitialized = false;

    private void Start()
    {
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }

        if (playerController != null)
        {
            playerParry = playerController.GetComponent<PlayerParry>();
        }
    }

    private void Update()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (playerController == null || playerController.stats == null) return;

        PlayerStats stats = playerController.stats;

        // --- 1. KHỞI TẠO GIÁ TRỊ BAN ĐẦU CÁC THANH BAR (Khởi chạy ở Frame đầu) ---
        if (!isInitialized)
        {
            if (healthBar != null) healthBar.Initialize(stats.CurrentHealth, stats.MaxHealth);
            if (actionPointBar != null) actionPointBar.Initialize(stats.CurrentActionPoint, stats.MaxActionPoint);
            if (rageBar != null) rageBar.Initialize(stats.CurrentRage, stats.MaxRage);
            
            isInitialized = true;
        }

        // --- 2. CẬP NHẬT CÁC THANH BAR CÓ DOTWEEN ---
        if (healthBar != null)
        {
            healthBar.UpdateBar(stats.CurrentHealth, stats.MaxHealth);
        }

        if (actionPointBar != null)
        {
            actionPointBar.UpdateBar(stats.CurrentActionPoint, stats.MaxActionPoint);
        }

        if (rageBar != null)
        {
            rageBar.UpdateBar(stats.CurrentRage, stats.MaxRage);
        }

        // --- 3. CẬP NHẬT TEXT ---
        if (healthText != null)
        {
            healthText.text = $"HP: {stats.CurrentHealth} / {stats.MaxHealth}";
        }

        if (actionPointText != null)
        {
            actionPointText.text = $"AP: {stats.CurrentActionPoint} / {stats.MaxActionPoint}";
        }

        if (rageText != null)
        {
            rageText.text = $"Rage: {stats.CurrentRage} / {stats.MaxRage}";
        }

        if (playerParry != null && parryEnergyText != null)
        {
            parryEnergyText.text = $"Parry Energy: {Mathf.RoundToInt(playerParry.CurrentEnergy)}%";
        }
    }
}