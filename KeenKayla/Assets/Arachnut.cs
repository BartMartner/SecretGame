using UnityEngine;
using System.Collections;

public class Arachnut : Enemy
{
    private DamagePlayerTrigger _damagePlayerTrigger;
    public float wanderRange = 1;
    public float speed = 0.5f;
    
    private Vector3 _startingPosition;
    private Vector3 _targetPosition;
    private Vector3 _direction = Vector3.right;
    private Quaternion _flippedFacing = Quaternion.Euler(0, 180, 0);

    protected override void Awake()
    {
        base.Awake();
        _damagePlayerTrigger = GetComponentInChildren<DamagePlayerTrigger>();
        _startingPosition = transform.position;
        _targetPosition = _startingPosition + wanderRange * _direction;
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

    public override void Die()
    {
        base.Die();
        StopAllCoroutines();

        _damagePlayerTrigger.gameObject.SetActive(false);

        var rigidbody2D = GetComponent<Rigidbody2D>();
        if (rigidbody2D)
        {
            rigidbody2D.isKinematic = true;
        }

        var collider = GetComponent<Collider2D>();
        if (collider)
        {
            collider.enabled = false;
        }

        if (Random.value < 0.5)
        {
            _animator.SetTrigger("Death1");
        }
        else
        {
            _animator.SetTrigger("Death2");
        }
    }

    public void OnDrawGizmosSelected()
    {
        var _boxCollider = GetComponentInChildren<Collider2D>();
        var left = transform.position - _direction * wanderRange;
        var right = transform.position + _direction * wanderRange;
        Debug.DrawLine(left, right);
        Vector3 topLeft, topRight, bottomLeft, bottomRight;
        topLeft = topRight = bottomLeft = bottomRight = Vector3.zero;

        topLeft.y += _boxCollider.bounds.extents.y;
        topRight.y += _boxCollider.bounds.extents.y;
        bottomLeft.y -= _boxCollider.bounds.extents.y;
        bottomRight.y -= _boxCollider.bounds.extents.y;

        topLeft.x -= _boxCollider.bounds.extents.x;
        topRight.x += _boxCollider.bounds.extents.x;
        bottomLeft.x -= _boxCollider.bounds.extents.x;
        bottomRight.x += _boxCollider.bounds.extents.x;

        Debug.DrawLine(left + topLeft, left + bottomLeft);
        Debug.DrawLine(right + topRight, right + bottomRight);
    }
}
