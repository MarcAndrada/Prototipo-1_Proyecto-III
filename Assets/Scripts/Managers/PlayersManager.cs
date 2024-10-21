using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayersManager : MonoBehaviour
{
    public static PlayersManager instance;

    [field: SerializeField]
    public float respawnTime {  get; private set; }
    public List<PlayerInput> players {  get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;


        players = new List<PlayerInput>();
    }
}
