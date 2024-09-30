using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float rotateSpeed = 15f;

    private GameInput gameInput;
    private Rigidbody rb;

    private bool isWalking;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        isWalking = moveDir != Vector3.zero;
        
        if (isWalking)
        {
            rb.AddForce(moveDir * acceleration, ForceMode.Acceleration);

            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }

            Vector3 forwardDir = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
            transform.forward = forwardDir;
        }
        else
        {
            float decelerationRate = 10f;
            Vector3 decelerationForce = rb.velocity.normalized * -decelerationRate; 
            rb.AddForce(decelerationForce, ForceMode.Acceleration);
    
            if (rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector3.zero;
            }
        }
        
        rb.angularVelocity = Vector3.zero;
    }
    
    public bool IsWalking()
    {
        return isWalking;
    }
}
