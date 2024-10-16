using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameController : MonoBehaviour
{
    public ModulesConfiguration baseModule;
    public ModulesConfiguration currentModule;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        currentModule.SetConfig(baseModule.Height, baseModule.Width, baseModule.ModulesPositions);
        SceneManager.LoadScene("NodeMap");
    }
}
