using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private PlayerController playerController;

    [Header("UI Text References")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text actionPointText;
    [SerializeField] private TMP_Text rageText;

    private void Start()
    {
        if (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
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
    }
}