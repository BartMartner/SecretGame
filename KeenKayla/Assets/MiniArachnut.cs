using UnityEngine;
using System.Collections;

public class MiniArachnut : Enemy
{
    private DamagePlayerTrigger _damagePlayerTrigger;
    public ProjectileStats projectileStats;
    public float wanderRange = 1;
    public float speed = 0.5f;
    public AudioClip shoot;

    private bool _justShot;
    private bool _shooting;
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

        if (!_shooting)
        {
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

            var result = Physics2D.Raycast(transform.position, Vector3.down, 100, Player.instance.playerLayerMask);
            if (result)
            {
                if (!_justShot)
                {
                    _justShot = true;
                    StartCoroutine(Shoot());
                }
            }
            else
            {
                _justShot = false;
            }
        }
    }

    private IEnumerator Shoot()
    {
        _shooting = true;
        yield return StartCoroutine(Flash(1, 0.125f, Color.red, 0.25f));
        _animator.SetTrigger("Shoot");
        audioSource.PlayOneShot(shoot, 0.5f);
        ProjectileManager.instance.Shoot(projectileStats, transform.position, Vector3.down);
        _shooting = false;
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

        Debug.DrawLine(left + topLeft, left + bottomLeft);
        Debug.DrawLine(right + topRight, right + bottomRight);
    }
}
