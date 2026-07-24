using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerParry playerParry;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerParry = playerController.GetComponent<PlayerParry>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerController.OnLightAttack();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            playerController.OnHeavyAttack();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            playerController.OnBlock();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerController.OnRageAttack();
        }

        if (playerParry != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                playerParry.StartCharging();
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                playerParry.ReleaseParry();
            }
        }

    }
}
