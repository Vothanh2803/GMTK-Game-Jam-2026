using UnityEngine;
using TMPro;

public class PlayerInfoUI : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private PlayerController playerController;

    [Header("Stat Bars")]
    [SerializeField] private UI_StatBar healthBar;
    [SerializeField] private UI_StatBar actionPointBar;
    [SerializeField] private UI_StatBar rageBar;

    [Header("UI Text References")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text actionPointText;
    [SerializeField] private TMP_Text rageText;
    [SerializeField] private TMP_Text parryEnergyText;

    [Header("Button Effects (MỚI)")]
    [SerializeField] private UIButtonEffect lightAttackButton;
    [SerializeField] private UIButtonEffect heavyAttackButton;
    [SerializeField] private UIButtonEffect rageAttackButton;
    [SerializeField] private UIButtonEffect blockButton;
    [SerializeField] private UIButtonEffect parryButton;

    private PlayerCombat playerCombat;
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
            playerCombat = playerController.GetComponent<PlayerCombat>();
            playerParry = playerController.GetComponent<PlayerParry>();
        }
    }

    private void Update()
    {
        UpdateUI();
        UpdateButtonStates(); // Cập nhật trạng thái mờ/sáng của nút
    }

    public void UpdateUI()
    {
        if (playerController == null || playerController.stats == null) return;

        PlayerStats stats = playerController.stats;

        if (!isInitialized)
        {
            if (healthBar != null) healthBar.Initialize(stats.CurrentHealth, stats.MaxHealth);
            if (actionPointBar != null) actionPointBar.Initialize(stats.CurrentActionPoint, stats.MaxActionPoint);
            if (rageBar != null) rageBar.Initialize(stats.CurrentRage, stats.MaxRage);
            
            isInitialized = true;
        }

        if (healthBar != null) healthBar.UpdateBar(stats.CurrentHealth, stats.MaxHealth);
        if (actionPointBar != null) actionPointBar.UpdateBar(stats.CurrentActionPoint, stats.MaxActionPoint);
        if (rageBar != null) rageBar.UpdateBar(stats.CurrentRage, stats.MaxRage);

        if (healthText != null) healthText.text = $"HP: {stats.CurrentHealth} / {stats.MaxHealth}";
        if (actionPointText != null) actionPointText.text = $"AP: {stats.CurrentActionPoint} / {stats.MaxActionPoint}";
        if (rageText != null) rageText.text = $"Rage: {stats.CurrentRage} / {stats.MaxRage}";

        if (playerParry != null && parryEnergyText != null)
        {
            parryEnergyText.text = $"Parry Energy: {Mathf.RoundToInt(playerParry.CurrentEnergy)}%";
        }
    }

    private void UpdateButtonStates()
    {
        if (playerCombat == null) return;

        bool isPlayerTurn = GameManager.Instance.CurrentState == GameState.PlayerTurn;
        bool isEnemyTurn = GameManager.Instance.CurrentState == GameState.EnemyTurn;

        // --- 1. NÚT LIGHT ATTACK ---
        if (lightAttackButton != null)
        {
            // Ẩn trong lượt quái, Hiện trong lượt người chơi
            lightAttackButton.SetVisible(isPlayerTurn);

            // Nếu đang ở lượt người chơi thì mới kiểm tra mờ/sáng theo điểm AP
            if (isPlayerTurn)
            {
                lightAttackButton.SetInteractable(playerCombat.CanLightAttack());
            }
        }

        // --- 2. NÚT HEAVY ATTACK ---
        if (heavyAttackButton != null)
        {
            // Ẩn trong lượt quái, Hiện trong lượt người chơi
            heavyAttackButton.SetVisible(isPlayerTurn);

            // Nếu đang ở lượt người chơi thì mới kiểm tra mờ/sáng theo điểm AP
            if (isPlayerTurn)
            {
                heavyAttackButton.SetInteractable(playerCombat.CanHeavyAttack());
            }
        }

        // --- 3. NÚT RAGE ATTACK ---
        if (rageAttackButton != null)
        {
            rageAttackButton.SetInteractable(playerCombat.CanPerformRageAttack());
        }

        // --- 4. NÚT BLOCK ---
        if (blockButton != null)
        {
            // HIỆN ở Turn Player, ẨN ở Turn Enemy
            blockButton.SetVisible(isPlayerTurn);
        }

        // --- 5. NÚT PARRY ---
        if (parryButton != null)
        {
            // ẨN ở Turn Player, HIỆN ở Turn Enemy
            parryButton.SetVisible(isEnemyTurn);
        }
    }
}