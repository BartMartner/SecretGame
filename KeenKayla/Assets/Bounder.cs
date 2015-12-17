using UnityEngine;
using System.Collections;

public class Bounder : Enemy
{
    public float speedX = 6f;
    public float speedY = 5f;

    private float _directionX;
    private Quaternion _flippedFacing = Quaternion.Euler(0, 180, 0);
    private float _timer;
    private float _time = 1.5f;
    private float _startingY;

    protected override void Awake()
    {
        base.Awake();
        _startingY = transform.position.y;
    }

    protected override void Update()
    {
        base.Update();

        if(_timer < _time)
        {
            
            _timer += Time.deltaTime;
            transform.position += _directionX * Vector3.right * speedX * Time.deltaTime;
            if(_timer < _time*0.5f)
            {
                var delta= (_time*0.5f - _timer) / (_time*0.5f);
                transform.position += Vector3.up * delta * speedY * Time.deltaTime;
            }
            else
            {
                var delta = (_timer -_time*0.5f) / (_time * 0.5f);
                transform.position += Vector3.down * delta * speedY * Time.deltaTime;
            }
        }
        else
        {
            SetupJump();
        }
    }

    public void SetupJump()
    {
        var position = transform.position;
        position.y = _startingY;
        transform.position = position;
        _timer = 0;
        _directionX = Random.Range((int)-1, (int)1);
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
