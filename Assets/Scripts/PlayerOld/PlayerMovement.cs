using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum MovementState
    {
        WALKING,
        GRABBING_LIGHT,
        GRABBING_HEAVY_S,
        GRABBING_HEAVY_M,
        STUNNED,
        INTERACTING,
        THROWNING_ITEM
    }

    public MovementState currentMovementState = MovementState.WALKING;

    PlayerController playerController;

    [Header("Movement")]
    [SerializeField]
    private float walkingSpeed;
    [SerializeField]
    private float grabbingSpeed;
    [SerializeField]
    private float grabbingHeavyS;
    [SerializeField]
    private float grabbingHeavyM;
    private float throwSpeed = 0;
    private float speed;
    [SerializeField] private float acceleration;
    public float speedPenalty { private get; set; }

    [SerializeField] private Vector2 movementDirection;

    [SerializeField] private float knockback;

    [SerializeField] private float rotationSpeed;
    //public BoxCollider objectCollider { get; private set; }


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        //objectCollider = GetComponent<BoxCollider>();
        //objectCollider.enabled = false;
        ChangeState(MovementState.WALKING);
    }

    void Update()
    {
        Debug.Log(movementDirection);
        movementDirection = playerController.inputController.movementInput;
    }

    private void FixedUpdate()
    {
        switch (currentMovementState)
        {
            case MovementState.WALKING:
                Walking();
                PlayerRotation();
                break;
            case MovementState.GRABBING_HEAVY_S:
            case MovementState.GRABBING_HEAVY_M:
                Grabbing();
                PlayerRotation();
                break;
            case MovementState.GRABBING_LIGHT:
                Grabbing();
                break;
            case MovementState.STUNNED:
                break;
            case MovementState.INTERACTING:
                break;
            default:
                break;
        }
    }

    private void Walking()
    {
        Move();
    }

    private void Grabbing()
    {
        Move();
    }

    private void Stunned()
    {
        if (playerController.rb.velocity.magnitude < 1f)
            ChangeState(MovementState.WALKING);
        else
        {
            Invoke("Stunned", 0.2f);
        }
    }

    private void Acceleration()
    {
        if (movementDirection.x >= 0.01f || movementDirection.y >= 0.01f)
        {
            Debug.Log("Moving");
            acceleration += Time.deltaTime + 0.06f;
            if (acceleration >= 1f)
                acceleration = 1f;

        }
        else if (movementDirection.x <= -0.1f || movementDirection.y <= -0.1f)
        {
            acceleration += Time.deltaTime;
            if (acceleration >= 1f)
                acceleration = 1f;
        }
        else if (movementDirection.x <= 0.01f && movementDirection.y <= 0.01f || movementDirection.x >= 0.01f && movementDirection.y >= 0.01f)
        {
            acceleration -= Time.deltaTime;
            if (acceleration <= 0f)
                acceleration = 0f;
        }
    }

    private void Move()
    {
        Acceleration();

        playerController.rb.velocity = new Vector3(speed * movementDirection.x * acceleration * Time.deltaTime, 0f, speed * movementDirection.y * acceleration * Time.deltaTime);
    }
    private void Move(float speedToReduce)
    {
        float reducedSpeed = speed - speedToReduce;
        Acceleration();
        playerController.rb.velocity = new Vector3(reducedSpeed * movementDirection.x * Time.deltaTime, 0f, reducedSpeed * movementDirection.y * Time.deltaTime);
    }


    private void HitByFire(Vector3 fireCenter)
    {
        Vector3 vectorToKnockBack = fireCenter - transform.position;
        ChangeState(MovementState.STUNNED);
        playerController.rb.velocity = Vector3.zero;
        playerController.rb.AddForce(vectorToKnockBack.normalized * knockback, ForceMode.Impulse);
        Invoke("Stunned", 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            Vector3 fireCenter = other.transform.position;
            HitByFire(fireCenter);
        }
    }

    private void PlayerRotation()
    {
        Vector3 movementDirectionToRotate = new Vector3(movementDirection.x, 0, movementDirection.y);
        movementDirectionToRotate.Normalize();

        if (movementDirectionToRotate != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(movementDirectionToRotate, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }
    public void ChangeState(MovementState currentState)
    {
        switch (currentMovementState)
        {
            case MovementState.WALKING:
                break;
            case MovementState.GRABBING_HEAVY_S:
            case MovementState.GRABBING_HEAVY_M:
            case MovementState.GRABBING_LIGHT:
                //objectCollider.enabled = false;
                break;
            case MovementState.STUNNED:
                break;
            case MovementState.INTERACTING:
                break;
            case MovementState.THROWNING_ITEM:
                break;
            default:
                break;
        }

        currentMovementState = currentState;

        switch (currentState)
        {
            case MovementState.WALKING:
                speed = walkingSpeed;
                break;
            case MovementState.GRABBING_HEAVY_S:
                speed = grabbingHeavyS;
                break;
            case MovementState.GRABBING_HEAVY_M:
                speed = grabbingHeavyM;
                break;
            case MovementState.GRABBING_LIGHT:
                speed = grabbingSpeed;
                //objectCollider.enabled = true;
                break;
            case MovementState.STUNNED:
                break;
            case MovementState.INTERACTING:
                break;
            case MovementState.THROWNING_ITEM:
                speed = throwSpeed;
                break;
            default:
                break;
        }
    }
}
