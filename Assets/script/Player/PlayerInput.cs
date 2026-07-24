using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("UI Button Effects Reference")]
    [SerializeField] private UIButtonEffect lightAttackButton; 
    [SerializeField] private UIButtonEffect heavyAttackButton;  
    [SerializeField] private UIButtonEffect blockButton;     
    [SerializeField] private UIButtonEffect rageAttackButton; 
    [SerializeField] private UIButtonEffect parryButton;      

    private PlayerController playerController;
    private PlayerParry playerParry;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerParry = playerController.GetComponent<PlayerParry>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (lightAttackButton != null) lightAttackButton.SimulateClick();
            playerController.OnLightAttack();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (heavyAttackButton != null) heavyAttackButton.SimulateClick();
            playerController.OnHeavyAttack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (blockButton != null) blockButton.SimulateClick();
            playerController.OnBlock();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (rageAttackButton != null) rageAttackButton.SimulateClick();
            playerController.OnRageAttack();
        }

        if (playerParry != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (parryButton != null) parryButton.SimulatePressDown();
                playerParry.StartCharging();
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                if (parryButton != null) parryButton.SimulatePressUp();
                playerParry.ReleaseParry();
            }
        }
    }
}