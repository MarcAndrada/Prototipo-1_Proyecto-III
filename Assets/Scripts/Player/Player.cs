using System;
using UnityEngine;

public class Player : MonoBehaviour, IInteractableObjectParent
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float rotateSpeed = 15f;
    
    [Header("Layers")]
    [SerializeField] private LayerMask interactableLayer;
    
    [Header("Utilities")]
    [SerializeField] private float throwForce = 20f;
    [SerializeField] private Transform interactiveObjectHoldPoint;

    private GameInput gameInput;
    private Rigidbody rb;

    private bool isWalking;
    private Vector3 lastInteractDir;

    private BaseFurniture selectedFurniture;
    private InteractableObject selectedObject;

    private InteractableObject obj;

    private void Awake()
    {

        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInputOnInteractAction;
        gameInput.OnDropAction += GameInputOnDropAction;
    }
    private void GameInputOnInteractAction(object sender, System.EventArgs e)
    {
        if (selectedFurniture != null)
        {
            selectedFurniture.Interact(this);
        }
        if (selectedObject != null)
        {
            selectedObject.GetComponent<Rigidbody>().isKinematic = true;
            selectedObject.SetInteractableObjectParent(this);
        }

    }
    private void GameInputOnDropAction(object sender, System.EventArgs e)
    {
        if (HasInteractableObject())
        {
            DropObject();
        }
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void Update()
    {
        HandleInteractions();
    }
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero)
        {
            lastInteractDir = transform.forward;
        }
        
        float interactDistance = 2f; 
        Vector3 rayCastPos = transform.position - new Vector3(0f, 0.5f, 0f);
        if (Physics.Raycast(rayCastPos, lastInteractDir, out RaycastHit raycastHit, interactDistance, interactableLayer))
        {
            if (raycastHit.transform.TryGetComponent(out BaseFurniture furniture))
            {
                ShowFurniture(furniture);
            }
            else
            {
                HideFurniture();
            }
            if (raycastHit.transform.TryGetComponent(out InteractableObject interactable) && !HasInteractableObject())
            {
                ShowObject(interactable);
            }
            else
            {
                HideObject();
            }
        }
        else
        {
            HideObject();
            HideFurniture();
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
    private void DropObject()
    {
        if (HasInteractableObject())
        {
            obj.transform.SetParent(null);
        
            Rigidbody objectRigidbody = obj.GetComponent<Rigidbody>();
            if (objectRigidbody != null)
            {
                objectRigidbody.isKinematic = false;
                Vector3 throwDirection = transform.forward;
                objectRigidbody.AddForce(throwDirection.normalized * throwForce, ForceMode.Impulse);
            }
            
            ClearInteractableObject();
        }
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    #region Show Hide Objects
    private void ShowFurniture(BaseFurniture furniture)
    {
        if (furniture != selectedFurniture)
        {
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
        }
    }
    private void ShowObject(InteractableObject interactable)
    {
        if (selectedObject != interactable)
        {
            selectedObject = interactable;
            selectedObject.GetSelectedObjectVisual().Show();
        }
    }
    private void HideObject()
    {
        if (selectedObject != null)
        {
            selectedObject.GetSelectedObjectVisual().Hide();
            selectedObject = null;
        }
    }
    #endregion
    public Transform GetInteractableObjectFollowTransform()
    {
        return interactiveObjectHoldPoint;
    }
    public InteractableObject GetInteractableObject()
    {
        return obj;
    }
    public void ClearInteractableObject()
    {
        obj = null;
    }
    public bool HasInteractableObject()
    {
        return obj != null;
    }
    public void SetInteractableObject(InteractableObject interactableObject)
    {
        obj = interactableObject;
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && gameInput != null)
        {
            Gizmos.color = Color.green;
        
            Gizmos.DrawRay(transform.position - new Vector3(0f, 0.5f, 0f), lastInteractDir * 2f);
        }
    }

}
