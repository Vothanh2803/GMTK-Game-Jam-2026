using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    [SerializeField] private float parryCooldown = 1.5f;
    private float currentCooldownTimer = 0f;

    public bool isParrying { get; private set; }
    private PlayerController playerController;

    private void Start() {
        playerController = GetComponent<PlayerController>();
    }

    private void Update() {
        if (currentCooldownTimer > 0)
        {
            currentCooldownTimer -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            OnParryButtonPressed();
        }
    }

    public void OnParryButtonPressed()
    {
        if (GameManager.Instance.CurrentState != GameState.EnemyTurn) return;

        if (currentCooldownTimer > 0)
        {
            Debug.Log("parry cooldown: " + currentCooldownTimer);
            return;
        }

        bool isWindowOpen = EnemyManager.Instance != null && EnemyManager.Instance.CurrentEnemy.isParryWindowOpen;

        if (isWindowOpen)
        {
            isParrying = true;
            currentCooldownTimer = 0f;
            Debug.Log("parry thanh cong!");
            
            // TODO: animation parry va VFX,SFX
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
            
            playerController.stats.AddActionPoint(2); 
            playerController.stats.AddRage(15);

            Debug.Log("cong diem");
            
            // TODO: add sound va hieu ung parry o day
        }
        else
        {
            Debug.Log("parry that bai");

            playerController.stats.TakeDamage(damage);

            if (playerController.stats.CurrentHealth <= 0)
            {
                GameManager.Instance.ChangeState(GameState.Lose);
            }

            Debug.Log("tru mau");
        }

        isParrying = false;
    }
}
