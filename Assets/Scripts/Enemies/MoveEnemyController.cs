using UnityEngine;

public class MoveEnemyController : MonoBehaviour
{

    private float movementOffset;
    private float moveSpeed;
    private float moveTime;
    private float moveProcess;

    private Vector3 startPos;
    private Vector3 endPos;

    // Update is called once per frame
    void FixedUpdate()
    {
        moveTime += moveSpeed * Time.fixedDeltaTime;
        moveProcess = Mathf.PingPong(moveTime, 1);

        transform.position = Vector3.Lerp(startPos, endPos, moveProcess);
    }

    public void SetMovementValues(float _speed, float _offset)
    {
        moveSpeed = _speed;
        movementOffset = _offset;

        startPos = transform.position + new Vector3(0, 0, movementOffset / 2);
        endPos = transform.position - new Vector3(0, 0, movementOffset / 2);
    }

}
