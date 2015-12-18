﻿using UnityEngine;
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

    protected override void Awake()
    {
        base.Awake();
        _rigidbody2D = GetComponentInChildren<Rigidbody2D>();
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
                transform.position += _directionX * Vector3.right * speedX * Time.deltaTime;
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
        _directionX = Random.Range((int)-1, (int)2);

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

    public override void Die()
    {
        base.Die();
        _rigidbody2D.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        _animator.SetTrigger("Death1");        
    }
}
