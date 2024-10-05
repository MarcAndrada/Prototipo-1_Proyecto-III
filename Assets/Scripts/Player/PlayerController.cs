using System;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInteractableObjectParent
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float rotateSpeed = 15f;

    [Header("Cannon Movement")]
    [SerializeField] private float cannonMovementSpeed = 2f;
    [SerializeField] private float cannonRotationSpeed = 25f;
    /*
    [Header("Push/Drag Settings")]
    [SerializeField] private float pushForce = 5f;
    [SerializeField] private float dragForce = 3f;
    [SerializeField] private float pushDragSpeed = 2f;
    */
    [Header("Layers")]
    [SerializeField] private LayerMask interactableLayer;
    
    [Header("Interact collisions")]
    [SerializeField] private float interactDistance = 0.7f;
    [SerializeField] private float sphereRadius = 0.7f;
    
    [Header("Miscellaneous")]
    [SerializeField] private float throwForce = 20f;
    [SerializeField] private Transform interactiveObjectHoldPoint;

    private GameInput gameInput;
    private Rigidbody rb;

    private bool isPilot;
    private bool isWalking;

    private BaseFurniture selectedFurniture;
    private InteractableObject selectedObject;
    private InteractableObject heldObject;
    private PlayerHintController hintController;
    private void Awake()
    {
        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();
        hintController = GetComponent<PlayerHintController>();
    }
    private void Start()
    {
        gameInput.OnInteractAction -= GameInputOnInteractAction; 
        gameInput.OnInteractAction += GameInputOnInteractAction;
        gameInput.OnDropAction -= GameInputOnDropAction;
        gameInput.OnDropAction += GameInputOnDropAction;
    }
    private void Update()
    {
        HandleInteractions();
    }
    private void FixedUpdate()
    {
        if (isPilot)
            HandleCannonMovement();
        else
            HandleMovement();
    }

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (selectedFurniture != null)
        {
            selectedFurniture.Interact(this);
        }
        else if (HasInteractableObject())
        {
            heldObject.Interact(this);
        }
        if (selectedObject != null)
        {
            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
            selectedObject.SetInteractableObjectParent(this);
        }
    }
    private void GameInputOnDropAction(object sender, EventArgs e)
    {
        if (HasInteractableObject())
        {
            DropObject();
        }
        if (isPilot)
        {
            ShootWeapon();
        }
    }
    #region Interactions
    private void HandleInteractions()
    {
        if (HasInteractableObject())
            heldObject.GetComponent<Collider>().enabled = false;
        
        DetectInteractableObject();
    }
    private void DetectInteractableObject()
    {
        Vector3 sphereCenter = transform.position + transform.forward * interactDistance;

        Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius, interactableLayer);
        
        if (hitColliders.Length > 0)
        {
            foreach (Collider objCollide in hitColliders)
            {
                if (objCollide.TryGetComponent(out BaseFurniture furniture))
                {
                    ShowFurniture(furniture);
                }
                else
                {
                    HideFurniture();
                }
                if (objCollide.TryGetComponent(out InteractableObject interactable) && !HasInteractableObject())
                {
                    ShowObject(interactable);
                }
                else
                {
                    HideObject();
                }
            }
        }
        else
        {
            HideObject();
            HideFurniture();
        }
    }
    private void DropObject()
    {
        if (HasInteractableObject())
        {
            heldObject.transform.SetParent(null);
            
            heldObject.GetComponent<Collider>().enabled = true;

            if (heldObject.TryGetComponent(out Rigidbody objectRigidbody))
            {
                objectRigidbody.isKinematic = false;
                Vector3 throwDirection = transform.forward + Vector3.up * 0.2f;
                objectRigidbody.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
            }

            ClearInteractableObject();
        }
    }

    private void ShootWeapon()
    {
        if (isPilot)
        {
            selectedFurniture.TryGetComponent(out Cannon selectedCannon);
            selectedCannon.Activate();
        }
    }
    #endregion
    
    #region Player Movement
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        isWalking = moveDir != Vector3.zero;
        
        if (isWalking)
        {
            MovePlayer(moveDir);
            RotatePlayer(moveDir);
        }
        else
        {
            DeceleratePlayer();
        }
        
        rb.angularVelocity = Vector3.zero;
    }
    private void MovePlayer(Vector3 moveDir)
    {
        rb.AddForce(moveDir * acceleration, ForceMode.Acceleration);

        if (rb.velocity.magnitude > moveSpeed)
        {
            rb.velocity = rb.velocity.normalized * moveSpeed;
        }

    }
    private void RotatePlayer(Vector3 moveDir)
    {
        Vector3 forwardDir = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        transform.forward = forwardDir;
    }
    private void DeceleratePlayer()
    {
        float decelerationRate = 10f;
        Vector3 decelerationForce = rb.velocity.normalized * -decelerationRate; 
        rb.AddForce(decelerationForce, ForceMode.Acceleration);
    
        if (rb.velocity.magnitude < 0.1f)
        {
            rb.velocity = Vector3.zero;
        }
    }
    private void HandleCannonMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        float forwardInput = inputVector.y;
        float rotationInput = inputVector.x;
        
        Vector3 moveDirection = transform.forward * forwardInput;
        rb.AddForce(moveDirection * acceleration, ForceMode.Acceleration);

        if (rotationInput != 0)
        {
            float rotationAmount = rotationInput * cannonRotationSpeed * Time.deltaTime;
            transform.Rotate(0, rotationAmount, 0); // Rotate around the Y axis
        }
    
        if (rb.velocity.magnitude > cannonMovementSpeed)
        {
            rb.velocity = rb.velocity.normalized * cannonMovementSpeed;
        }
    }
    #endregion
    
    #region Show and Hide Objects
    private void ShowFurniture(BaseFurniture furniture)
    {
        if (furniture != selectedFurniture)
        {
            // Previene bugs visuales de que se mantenga el anterior mueble iluminado
            HideFurniture();
            
            selectedFurniture = furniture;
            selectedFurniture.GetSelectedFurnitureVisual().Show();
        }
    }
    private void HideFurniture()
    {
        if (selectedFurniture != null)
        {
            selectedFurniture.GetSelectedFurnitureVisual().Hide();
            selectedFurniture = null;
            hintController.UpdateActionType(PlayerHintController.ActionType.NONE);
        }
    }
    private void ShowObject(InteractableObject interactable)
    {
        if (selectedObject != interactable)
        {
            // Previene bugs visuales de que se mantenga el anterior objeto iluminado
            HideObject();
            
            selectedObject = interactable;
            selectedObject.GetSelectedObjectVisual().Show();
            hintController.UpdateActionType(PlayerHintController.ActionType.GRAB);
        }
    }
    private void HideObject()
    {
        if (selectedObject != null)
        {
            selectedObject.GetSelectedObjectVisual().Hide();
            selectedObject = null;
            hintController.UpdateActionType(PlayerHintController.ActionType.NONE);
        }
    }
    #endregion

    #region Interactable Interface
    public Transform GetInteractableObjectFollowTransform()
    {
        return interactiveObjectHoldPoint;
    }
    public InteractableObject GetInteractableObject()
    {
        return heldObject;
    }
    public void ClearInteractableObject()
    {
        heldObject = null;
    }
    public bool HasInteractableObject()
    {
        return heldObject != null;
    }
    public void SetInteractableObject(InteractableObject interactableObject)
    {
        heldObject = interactableObject;
    }
    #endregion
    
    public bool GetIsWalking()
    {
        return isWalking;
    }

    public bool GetIsPilot()
    {
        return isPilot;
    }
    public void SetIsPilot(bool pilot)
    {
        isPilot = pilot;
    }

    private void OnDrawGizmos()
    {        
        Vector3 sphereCenter = transform.position + transform.forward * interactDistance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(sphereCenter, sphereRadius);

        Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius, interactableLayer);

        if (hitColliders.Length > 0)
        {
            foreach (var objCollide in hitColliders)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawWireCube(objCollide.bounds.center, objCollide.bounds.size);
            }
        }
    }

}
