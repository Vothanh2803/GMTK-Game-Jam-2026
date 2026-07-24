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
    [SerializeField] private TMP_Text parryEnergyText;

    private PlayerParry playerParry;

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

        if (playerParry != null)
        {
            float energy = playerParry.CurrentEnergy;

            if (parryEnergyText != null)
            {
                parryEnergyText.text = $"Parry Energy: {Mathf.RoundToInt(energy)}%";
            }
        }
    }
}