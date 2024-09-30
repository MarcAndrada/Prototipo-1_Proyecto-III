using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Change singleton to multiplayer friendly
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public Counter selectedCounter;
    }
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float rotateSpeed = 15f;
    
    [Header("Layers")]
    [SerializeField] private LayerMask interactableLayer;
    
    private GameInput gameInput;
    private Rigidbody rb;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private Counter selectedCounter;
    
    private void Awake()
    {
        Instance = this;

        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnInteractAction;
    }

    private void GameInputOnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleInteractions();
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        
        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, interactableLayer))
        {
            if (raycastHit.transform.TryGetComponent(out Counter counter))
            {
                if (counter != selectedCounter)
                {
                    SetSelectedCounter(counter);
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
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
    private void SetSelectedCounter(Counter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs()
        {
            selectedCounter = selectedCounter
        });
    }
}
