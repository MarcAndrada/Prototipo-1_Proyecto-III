using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplePlayerController : MonoBehaviour
{

    private CameraController cameraController;
    private PlayersReadyController playerReadyController;
    private void Awake()
    {
        cameraController = FindObjectOfType<CameraController>();
        playerReadyController = FindObjectOfType<PlayersReadyController>();
    }


    public void JoinnedPlayer(PlayerInput obj)
    {
        cameraController.AddPlayer(obj.gameObject);
        playerReadyController.AddPlayer(obj);
    }
    public void LeftPlayer(PlayerInput obj)
    {
        cameraController.RemovePlayer(obj.gameObject);
    }
}
