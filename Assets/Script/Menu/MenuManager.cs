using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //[SerializeField] private string gameplayScene = "Gameplay";

    public void PlayGame()
    {
        SceneController.Instance.PlayGame();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        SceneController.Instance.QuitGame();
#endif
    }
}