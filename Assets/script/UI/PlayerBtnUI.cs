using UnityEngine;

public class PlayerBtnUI : MonoBehaviour
{
    [SerializeField] private GameObject inTurnBtnPanel;
    [SerializeField] private GameObject parryTurnBtnPanel;
    void Update()
    {
        inTurnBtnPanel.SetActive(GameManager.Instance.CurrentState == GameState.PlayerTurn);
        parryTurnBtnPanel.SetActive(GameManager.Instance.CurrentState != GameState.PlayerTurn);
    }
}
