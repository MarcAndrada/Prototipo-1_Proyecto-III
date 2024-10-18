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
    
    [Header("Layers")]
    [SerializeField] private LayerMask interactableLayer;
    
    [Header("Interact collisions")]
    [SerializeField] private float interactDistance = 0.7f;
    [SerializeField] private float sphereRadius = 0.9f;
    
    [Header("Miscellaneous")]
    [SerializeField] private float throwForce = 20f;
    [SerializeField] private Transform interactiveObjectHoldPoint;

    private GameInput gameInput;
    private Rigidbody rb;

    private bool canMove;
    private bool isPilot;
    private bool isWalking;
    
    public bool isAlive { private set; get; } = true;
    private BaseFurniture selectedFurniture;
    private InteractableObject selectedObject;
    private InteractableObject heldObject;
    public PlayerHintController hintController {  get; private set; }
    public Transform spawnPos;

    [HideInInspector]
    public BaseWeapon currentWeapon;
    private void Awake()
    {
        gameInput = GetComponent<GameInput>();
        rb = GetComponent<Rigidbody>();
        hintController = GetComponent<PlayerHintController>();

        canMove = true;
    }

    private void OnEnable()
    {
        // Llama al los events del Game Input
        gameInput.OnInteractAction += GameInputOnInteractAction;
        gameInput.OnInteractReleaseAction += GameInputOnReleaseAction;
        gameInput.OnDropAction += GameInputOnDropAction;
    }
    private void OnDisable()
    {
        gameInput.OnInteractAction -= GameInputOnInteractAction; 
        gameInput.OnInteractReleaseAction -= GameInputOnReleaseAction;
        gameInput.OnDropAction -= GameInputOnDropAction;
    }
    private void Update()
    {
        HandleInteractions();
    }
    private void FixedUpdate()
    {
        if (isAlive && canMove)
        {
            if (isPilot) // Si esta pilotando usar el movimiento del cañon
                HandleCannonMovement();
            else
                HandleMovement();
        }
    }

    private void GameInputOnInteractAction(object sender, EventArgs e)
    {
        // Funciones para cuando uses el boton de interactuar
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
            selectedObject.OnPickUp();
            selectedObject.SetInteractableObjectParent(this);
        }
    }
    private void GameInputOnReleaseAction(object sender, EventArgs e)
    {
        // Funciones para cuando sueltes el boton de interactuar pero para soltar el objeto
        if (selectedFurniture != null)
        {
            selectedFurniture.Release(this);
        }
    }
    private void GameInputOnDropAction(object sender, EventArgs e)
    {
        // Funciones para cuando uses el boton de trow
        if (HasInteractableObject())
        {
            DropObject();
        }
        if (isPilot)
        {
            ShootWeapon();
        }
    }

    public void KillPlayer()
    {
        if (isPilot)
        {
            SetIsPilot(false, null);
            if (selectedFurniture != null)
            {
                if (selectedFurniture.TryGetComponent(out BaseWeapon selectedCannon))
                    selectedCannon.ExitPilot();
            }
        }
        isAlive = false;
        Invoke("RevivePlayer", PlayersManager.instance.respawnTime);
        
    }
    public void RevivePlayer()
    {
        transform.position = spawnPos.position;
        isAlive = true;
    }
    #region Interactions
    private void HandleInteractions()
    {
        // Mirar si tiene un objeto para desactivar su colision
        if (HasInteractableObject())
            heldObject.GetComponent<Collider>().enabled = false;
        
        DetectInteractableObject();
    }
    private void DetectInteractableObject()
    {
        // Crear una esfera para detectar objetos en frente del personaje
        Vector3 sphereCenter = transform.position + transform.forward * interactDistance;

        Collider[] hitColliders = Physics.OverlapSphere(sphereCenter, sphereRadius, interactableLayer);
        
        // Mirar la cantidad de objetos que ha colisionado la esfera
        if (hitColliders.Length > 0)
        {
            Collider selectedCollider = null;
            int maxPriority = -1;
            // Si el collider detecta un objeto le activa o desactivar el outline
            foreach (Collider objCollide in hitColliders)
            {
                PriorityLevelController priority = objCollide.GetComponent<PriorityLevelController>();
                if(priority.priorityLevel > maxPriority)
                {
                    selectedCollider = objCollide;
                    maxPriority = priority.priorityLevel;
                }
            }

            if (selectedCollider.TryGetComponent(out BaseFurniture furniture))
            {
                ShowFurniture(furniture);
            }
            else
            {
                HideFurniture();
            }
            if (selectedCollider.TryGetComponent(out InteractableObject interactable) && !HasInteractableObject())
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
    private void DropObject()
    {
        // Tirar el objeto hacia al frente
        if (HasInteractableObject())
        {
            heldObject.transform.SetParent(null);
            
            heldObject.GetComponent<Collider>().enabled = true;

            if (heldObject.TryGetComponent(out Rigidbody objectRigidbody))
            {
                objectRigidbody.isKinematic = false;
                Vector3 throwDirection = transform.forward + Vector3.up * 0.2f;
                objectRigidbody.AddForce(throwDirection.normalized * throwForce, ForceMode.VelocityChange);
            }
            
            // Limpiamos el objeto que tiene el player
            ClearInteractableObject();
        }
    }

    private void ShootWeapon()
    {
        // Si el jugador esta montado, que pueda disparar
        if (isPilot)
        {
            if(selectedFurniture.TryGetComponent(out BaseWeapon selectedCannon))
                selectedCannon.Activate(this);
        }
    }
    #endregion
    
    #region Player Movement
    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        
        // Variable para saber si se esta moviendo
        isWalking = moveDir != Vector3.zero;
        
        if (isWalking)
        {
            // Mover y rotar el jugador
            MovePlayer(moveDir);
            RotatePlayer(moveDir);
        }
        else
        {
            // Desacelerar el jugador
            DeceleratePlayer();
        }
        
        rb.angularVelocity = Vector3.zero;
    }
    private void MovePlayer(Vector3 moveDir)
    {
        rb.AddForce(moveDir * acceleration, ForceMode.Acceleration);

        if (rb.velocity.magnitude > moveSpeed)
        {
            rb.velocity = new Vector3(
                rb.velocity.normalized.x * moveSpeed,
                rb.velocity.y,
                rb.velocity.normalized.z * moveSpeed
                );
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
        Vector3 decelerationForce = new Vector3(
            rb.velocity.normalized.x * -decelerationRate,
            rb.velocity.y,
            rb.velocity.normalized.z * -decelerationRate
            ); 
        rb.AddForce(decelerationForce, ForceMode.Acceleration);

        if (rb.velocity.magnitude < 0.1f)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }
    private void HandleCannonMovement()
    {
        if (currentWeapon.IsBroke())
            SetIsPilot(false, null);

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
            rb.velocity = new Vector3(
                rb.velocity.normalized.x * cannonMovementSpeed,
                rb.velocity.y,
                rb.velocity.normalized.z * cannonMovementSpeed
                );
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
            
            // Cambia el objeto seleccionado al actual.
            selectedFurniture = furniture;
            selectedFurniture.GetSelectedFurnitureVisual().Show();
            selectedFurniture.ShowNeededInputHint(this, hintController);
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
            
            // Cambia el objeto seleccionado al actual.
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
    public void SetIsPilot(bool pilot, BaseWeapon _weapon)
    {
        isPilot = pilot;
        currentWeapon = _weapon;
    }
    public void SetCanMove(bool move)
    {
        canMove = move;
    }
    public PlayerHintController GetPlayerHintController()
    {
        return hintController;
    }
    private void OnDrawGizmos()
    {        
        Vector3 sphereCenter = transform.position + transform.forward * interactDistance;

        Gizmos.color = Color.green;
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
