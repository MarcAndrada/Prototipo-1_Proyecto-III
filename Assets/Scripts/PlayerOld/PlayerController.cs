using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovement movementController { get; private set; }
    public InputController inputController { get; private set; }
    public Rigidbody rb { get; private set; }
    private void Awake()
    {
        movementController = GetComponent<PlayerMovement>();
        inputController = GetComponent<InputController>();

        rb = GetComponent<Rigidbody>();
    }
}
