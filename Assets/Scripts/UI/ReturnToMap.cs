using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMap : MonoBehaviour
{
    [SerializeField] private SceneController sceneController;
    public void Return()
    {
        sceneController.NextLevel("NodeMap");
    }
    public void RestartLevel()
    {
        sceneController.NextLevel(SceneManager.GetActiveScene().name);
    }

    public void GoToLevel (string _sceneName) 
    {
        sceneController.NextLevel(_sceneName);
    }

    public void GoToMainMenu()
    {
        sceneController.NextLevel("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
