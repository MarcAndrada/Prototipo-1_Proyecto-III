using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TestTools;

public class GameInput : MonoBehaviour
{
    public event EventHandler OnInteractAction;
    public event EventHandler OnDropAction;

    private Vector2 playerMovement;

    public void InteractPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInteractAction?.Invoke(this, EventArgs.Empty);
        }
    }
    public void TrowPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnDropAction?.Invoke(this, EventArgs.Empty);
        }
    }
    public void ReadMovement(InputAction.CallbackContext context)
    {
        playerMovement = context.ReadValue<Vector2>();
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerMovement;
        return inputVector.normalized;
    }
}
