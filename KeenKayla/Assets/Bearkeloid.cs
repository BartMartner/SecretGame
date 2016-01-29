using UnityEngine;
using System.Collections;

public class Bearkeloid : Enemy
{
    public float wanderRange = 1;
    public float speed = 0.5f;
    public float offset = 0f;

    private Vector3 _startingPosition;
    private Vector3 _targetPosition;
    private Vector3 _direction = Vector3.right;
    private Quaternion _flippedFacing = Quaternion.Euler(0, 180, 0);

    protected override void Awake()
    {
        base.Awake();
        _startingPosition = transform.position;
        _targetPosition = _startingPosition + wanderRange * _direction;
        transform.position += Vector3.right * offset;
    }

    protected override void UpdateAlive()
    {
        base.UpdateAlive();

        if (transform.position != _targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
        }
        else
        {
            _direction *= -1;
            _targetPosition = _startingPosition + wanderRange * _direction;

            if (_direction.x < 0 && transform.rotation != _flippedFacing)
            {
                transform.rotation = _flippedFacing;
            }
            else if (_direction.x > 0 && transform.rotation != Quaternion.identity)
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        var _boxCollider = GetComponentInChildren<BoxCollider2D>();
        var left = transform.position - _direction * wanderRange;
        var right = transform.position + _direction * wanderRange;
        var offsetMark = transform.position + Vector3.right * offset;
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

        if (offset != 0)
        {
            Debug.DrawLine(offsetMark + Vector3.up * _boxCollider.size.y * 0.5f, offsetMark + Vector3.down * _boxCollider.size.y * 0.5f, Color.cyan);
        }
        Debug.DrawLine(left + topLeft, left + bottomLeft);
        Debug.DrawLine(right + topRight, right + bottomRight);
    }
}
