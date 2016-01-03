using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    public float speed = 0.5f;
    public float movementRange = 1;
    public Vector3 direction = Vector3.right;
    private Vector3 _startingPosition;
    private Vector3 _targetPosition;

    protected void Awake()
    {
        _startingPosition = transform.position;
        _targetPosition = _startingPosition + movementRange * direction;
        direction.Normalize();
    }

    public void Update()
    {
        if (direction != Vector3.zero)
        {
            if (transform.position != _targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
            }
            else
            {
                direction *= -1;
                _targetPosition = _startingPosition + movementRange * direction;
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        var _boxCollider = GetComponentInChildren<BoxCollider2D>();
        if(!Application.isPlaying)
        {
            _startingPosition = transform.position;
        }

        direction.Normalize();
        var min = _startingPosition - direction * movementRange;
        var max = _startingPosition + direction * movementRange;
        Debug.DrawLine(min, max);
        Vector3 topLeft, topRight, bottomLeft, bottomRight;
        topLeft = topRight = bottomLeft = bottomRight = Vector3.zero;

        topLeft.y += _boxCollider.size.y * transform.lossyScale.y * 0.5f;
        topRight.y += _boxCollider.size.y * transform.lossyScale.y * 0.5f;
        bottomLeft.y -= _boxCollider.size.y * transform.lossyScale.y * 0.5f;
        bottomRight.y -= _boxCollider.size.y * transform.lossyScale.y * 0.5f;

        topLeft.x -= _boxCollider.size.x * transform.lossyScale.x * 0.5f;
        topRight.x += _boxCollider.size.x * transform.lossyScale.x * 0.5f;
        bottomLeft.x -= _boxCollider.size.x * transform.lossyScale.x * 0.5f;
        bottomRight.x += _boxCollider.size.x * transform.lossyScale.x * 0.5f;

        Debug.DrawLine(min + topLeft, min + topRight, Color.green);
        Debug.DrawLine(min + bottomLeft, min + bottomRight, Color.green);
        Debug.DrawLine(min + topLeft, min + bottomLeft, Color.green);
        Debug.DrawLine(min + topRight, min + bottomRight, Color.green);

        Debug.DrawLine(max + topLeft, max + topRight, Color.green);
        Debug.DrawLine(max + bottomLeft, max + bottomRight, Color.green);
        Debug.DrawLine(max + topLeft, max + bottomLeft, Color.green);
        Debug.DrawLine(max + topRight, max + bottomRight, Color.green);
    }
}

