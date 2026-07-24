using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    [Header("Parry Cooldown Settings")]
    [SerializeField] private float parryCooldown = 1.5f;
    private float blockDamageReduction = 0.5f;
    private float currentCooldownTimer = 0f;

    [Header("Energy Parry Settings")]
    [SerializeField] private float chargeSpeed = 500f;
    [SerializeField] private float maxHoldTimeAfterFull = 0.4f;

    private float currentEnergy = 0f;
    private float overchargeTimer = 0f;
    private bool isCharging = false;

    public bool isParrying { get; private set; }
    private PlayerController playerController;

    public float CurrentEnergy => currentEnergy;

    private void Start() {
        playerController = GetComponent<PlayerController>();
    }

    private void Update() {
        if (currentCooldownTimer > 0)
        {
            currentCooldownTimer -= Time.deltaTime;
        }

        if (isCharging)
        {
            if (currentEnergy < 100f)
            {
                currentEnergy += chargeSpeed * Time.deltaTime;
                currentEnergy = Mathf.Clamp(currentEnergy, 0f, 100f);

                if (currentEnergy >= 100f)
                {
                    Debug.Log("Thanh nang luong da day");
                    // TODO về sau: Hieu ung rung thanh nang luong
                }
            }
            else 
            {
                overchargeTimer += Time.deltaTime;

                if (overchargeTimer >= maxHoldTimeAfterFull)
                {
                    CancelChargingDueToOvercharge();
                }
            }
        }
    }

    public void StartCharging()
    {
        if (GameManager.Instance.CurrentState == GameState.EnemyTurn && currentCooldownTimer <= 0)
        {
            isCharging = true;
            currentEnergy = 0f;
            overchargeTimer = 0f;
        }
        else if (currentCooldownTimer > 0)
        {
            Debug.Log("parry cooldown: " + currentCooldownTimer);
        }
    }

    public void ReleaseParry()
    {
        if (!isCharging) return;

        bool isFullCharge = currentEnergy >= 100f;
        
        isCharging = false;
        currentEnergy = 0f;

        EvaluateEnergyParry(isFullCharge);
    }

    private void CancelChargingDueToOvercharge()
    {
        isCharging = false;
        currentEnergy = 0f;
        isParrying = false;
        currentCooldownTimer = parryCooldown;
        Debug.Log("parry that bai do thanh nang luong qua tai, vao cooldown");
    }

    private void EvaluateEnergyParry(bool isFullCharge)
    {
        if (GameManager.Instance.CurrentState != GameState.EnemyTurn) return;

        EnemyController currentEnemy = EnemyManager.Instance != null ? EnemyManager.Instance.CurrentEnemy : null;
        bool isWindowOpen = currentEnemy != null && currentEnemy.isParryWindowOpen;

        if (isWindowOpen)
        {
            EnemyAttackType enemyAttack = currentEnemy.CurrentAttackType;

            if (!isFullCharge)
            {
                if (enemyAttack == EnemyAttackType.lightAttack)
                {
                    isParrying = true;
                    currentCooldownTimer = 0f;
                    Debug.Log("parry thanh cong!");
                }
                else
                {
                    isParrying = false;
                    currentCooldownTimer = parryCooldown;
                    Debug.Log("parry that bai do quai danh Heavy attack nhung nang luong chua day, vao cooldown");
                }
            }
            else
            {
                isParrying = true;
                currentCooldownTimer = 0f;
                Debug.Log("parry thanh cong!");
            }
        }
        else
        {
            isParrying = false;
            currentCooldownTimer = parryCooldown;
            Debug.Log("parry that bai, vao cooldown");
        }
    }

    public void TakeDamageWithParry(float damage)
    {
        if (isParrying)
        {
            playerController.stats.AddRage(5);

            if (EnemyManager.Instance != null)
            {
                EnemyManager.Instance.ApplyParryDamagePercent(0.02f);
            }

            Debug.Log("Gay sat thuong len quai do parry");
        }
        else
        {
            float finalDamage = damage;

            PlayerCombat combat = playerController.GetComponent<PlayerCombat>();
            if (combat != null && combat.IsBlocking)
            {
                finalDamage *= blockDamageReduction;
                Debug.Log("Giam sat thuong do do don");
            }

            playerController.stats.TakeDamage(finalDamage);

            if (playerController.stats.CurrentHealth <= 0)
            {
                GameManager.Instance.ChangeState(GameState.Lose);
            }

            Debug.Log("tru mau");
        }

        isParrying = false;
    }
}
