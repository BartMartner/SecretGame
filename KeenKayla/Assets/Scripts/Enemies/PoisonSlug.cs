using UnityEngine;
using System.Collections;

public class PoisonSlug : Enemy
{
    private DamagePlayerTrigger _damagePlayerTrigger;
    public PosionPuddle poisonPuddlePrefab;
    public float wanderRange = 1;
    public float speed = 0.5f;
    public float minPoopTime = 1;
    public float maxPoopTime = 4;

    private bool _pooping;
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
        StartCoroutine(Poop());
    }

    protected override void UpdateAlive()
    {
        base.UpdateAlive();

        if (!_pooping)
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
        }
    }

    private IEnumerator Poop()
    {
        while(state == DamagableState.Alive)
        {
            yield return new WaitForSeconds(Random.Range(minPoopTime, maxPoopTime));
            _pooping = true;
            _animator.SetBool("Pooping", true);
            Instantiate(poisonPuddlePrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            _animator.SetBool("Pooping", false);
            _pooping = false;
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
}
