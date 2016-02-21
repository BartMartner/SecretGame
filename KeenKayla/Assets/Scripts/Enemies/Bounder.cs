using UnityEngine;
using System.Collections;

public class Bounder : Enemy
{
    public float speedX = 6f;
    public float speedY = 5f;
    private Rigidbody2D _rigidbody2D;

    private float _directionX;
    private Quaternion _flippedFacing = Quaternion.Euler(0, 180, 0);
    private float _timer;
    private float _time = 1.25f;
    private LockPlayerOnCollide _lockPlayer;

    protected override void Awake()
    {
        base.Awake();
        _lockPlayer = GetComponentInChildren<LockPlayerOnCollide>();
        _rigidbody2D = GetComponentInChildren<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        base.Update();

        if (state == DamagableState.Alive)
        {
            groundedCheck.UpdateRaycasts();

            if (_timer < _time)
            {
                _timer += Time.deltaTime;
                var hit = Physics2D.Raycast(transform.position, _directionX * Vector3.right, _collider2D.bounds.extents.x, LayerMask.GetMask("Default", "DamagableTerrain"));

                if (!hit.collider)
                {
                    transform.position += _directionX * Vector3.right * speedX * Time.deltaTime;
                }

                if (_timer < _time * 0.5f)
                {
                    transform.position += Vector3.up * speedY * (_time * 0.5f - _timer) / (_time * 0.5f) * Time.deltaTime;
                }
                else
                {
                    _rigidbody2D.isKinematic = false;
                }
            }
            else if (groundedCheck.onGround)
            {
                SetupJump();
            }
        }
    }

    public void SetupJump()
    {
        _rigidbody2D.isKinematic = true;
        _timer = 0;
        if (_lockPlayer.playerPresent)
        {
            _directionX = Mathf.Sign(Player.instance.transform.position.x - transform.position.x);
        }
        else
        {
            _directionX = Random.Range((int)-1, (int)2);
        }

        if (_directionX == 0)
        {
            _animator.SetBool("Moving", false);
        }
        else
        {
            _animator.SetBool("Moving", true);
            if (_directionX < 0 && transform.rotation != Quaternion.identity)
            {
                transform.rotation = Quaternion.identity;
            }
            else if (_directionX > 0 && transform.rotation != _flippedFacing)
            {
                transform.rotation = _flippedFacing;
            }
        }
    }

    public override void OnImmune(DamageType damageType)
    {
        if(damageType == DamageType.Generic)
        {
            StartCoroutine(Flash(2, 0.1f, Color.green, 0.5f));
        }
    }

    public override void Die()
    {
        base.Die();
        _rigidbody2D.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        _animator.SetTrigger("Death1");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(state == DamagableState.Dead)
        {
            _rigidbody2D.isKinematic = true;
            var collider = GetComponent<Collider2D>();
            if(collider)
            {
                collider.enabled = false;
            }
        }
    }
}
