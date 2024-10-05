using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [field: SerializeField]
    public PlayerController controller {  get; private set; }

    [field: SerializeField]
    public PlayerHintController hintController {  get; private set; }

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);

        instance = this;
    }
}
