using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance;

    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioClip cine;
    
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        audioSource = AudioManager.instance.Play2dLoop(audioClip, "Master", 0.1f, 1f, 1f);
    }

    private void FixedUpdate()
    {
        
        if (SceneManager.GetActiveScene().name == "EndGameScene" && audioSource.clip != cine)
        {
            audioSource.clip = cine;
            audioSource.Play();
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        AudioSource[] exceptions = new AudioSource[1];
        exceptions[0] = audioSource;
        AudioManager.instance.StopAllAudio(exceptions);
    }
}
