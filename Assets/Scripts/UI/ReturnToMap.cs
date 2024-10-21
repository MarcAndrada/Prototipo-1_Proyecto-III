using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMap : MonoBehaviour
{
    public void Return()
    {
        SceneManager.LoadScene("NodeMap");
    }
    public void RestartLevel ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToLevel (string _sceneName) 
    {
        SceneManager.LoadScene(_sceneName);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
