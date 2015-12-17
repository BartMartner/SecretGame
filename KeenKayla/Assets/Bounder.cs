using UnityEngine;
using System.Collections;

public class Bounder : Enemy
{
    public float wanderRange = 10;
    public float speed = 0.5f;

    private Vector3 _startingPosition;
    private Vector3 _targetPosition;
    private Vector3 _direction = Vector3.right;
    private Quaternion _flippedFacing = Quaternion.Euler(0, 180, 0);

    protected override void Update()
    {
        base.Update();
    
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

        if (Random.value < 0.5)
        {
            _animator.SetTrigger("Death1");
        }
        else
        {
            _animator.SetTrigger("Death2");
        }
    }
}
