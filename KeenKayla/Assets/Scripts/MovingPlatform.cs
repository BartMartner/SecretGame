using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 0.5f;
    public float wanderRange = 1;
    private Vector3 _startingPosition;
    private Vector3 _targetPosition;
    private Vector3 _direction = Vector3.right;

    protected void Awake()
    {
        _startingPosition = transform.position;
        _targetPosition = _startingPosition + wanderRange * _direction;
    }

    public void Update()
    {
        if (_direction != Vector3.zero)
        {
            if (transform.position != _targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
            }
            else
            {
                _direction *= -1;
                _targetPosition = _startingPosition + wanderRange * _direction;
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        var _boxCollider = GetComponentInChildren<BoxCollider2D>();
        var left = transform.position - _direction * wanderRange;
        var right = transform.position + _direction * wanderRange;
        Debug.DrawLine(left, right);
        Vector3 topLeft, topRight, bottomLeft, bottomRight;
        topLeft = topRight = bottomLeft = bottomRight = Vector3.zero;

        topLeft.y += _boxCollider.size.y * 0.5f;
        topRight.y += _boxCollider.size.y * 0.5f;
        bottomLeft.y -= _boxCollider.size.y * 0.5f;
        bottomRight.y -= _boxCollider.size.y * 0.5f;

        topLeft.x -= _boxCollider.size.x * 0.5f;
        topRight.x += _boxCollider.size.x * 0.5f;
        bottomLeft.x -= _boxCollider.size.x * 0.5f;
        bottomRight.x += _boxCollider.size.x * 0.5f;

        Debug.DrawLine(left + topLeft, left+bottomLeft);
        Debug.DrawLine(right + topRight, right + bottomRight);
    }
}

