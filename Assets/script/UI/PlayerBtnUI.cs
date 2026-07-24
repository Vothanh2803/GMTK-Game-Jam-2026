using UnityEngine;

public class PlayerBtnUI : MonoBehaviour
{
    [SerializeField] private GameObject inTurnBtnPanel;
    [SerializeField] private GameObject parryTurnBtnPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inTurnBtnPanel.SetActive(GameManager.Instance.CurrentState == GameState.PlayerTurn);
        parryTurnBtnPanel.SetActive(GameManager.Instance.CurrentState != GameState.PlayerTurn);
    }
}
