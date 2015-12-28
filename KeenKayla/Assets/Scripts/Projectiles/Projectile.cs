using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
    public bool alive;
    public float deathTime = 1;
    public bool hasDeathAnimation;

    private Animator _animator;
    private Vector3 _direction;
    private Vector3 _tangentDirection;
    private Vector3 _origin;
    private ProjectileStats _stats;
    private float _lifeCounter;
    private Material _material;
    private Color _orignalColor;
    public float _destroyDistance;
    public AnimationCurve _motionPattern;

    public delegate void DeathEvent();
    public DeathEvent deathEvent;

    private void Awake()
    {
        _destroyDistance = 40;
        _material = GetComponentInChildren<Renderer>().material;
        _orignalColor = _material.color;
    }

    private  void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        if (_animator)
        {
            _animator.logWarnings = false;
        }
    }

    private void Update()
    {
        if (!alive)
        {
            return;
        }

        if (_motionPattern != null)
        {
            var value = _motionPattern.Evaluate(_lifeCounter);
            var newPosition = _origin + _direction * _lifeCounter * _stats.speed + _tangentDirection * value;
            if (!_stats.lockRotation && transform.position != newPosition)
            {
                transform.rotation = Quaternion.FromToRotation(Vector3.right, newPosition - transform.position);
            }
            transform.position = newPosition;
        }
        else
        {
            if(_stats.homing > 0)
            {
                Vector3 newDirection = _direction;
                if(_stats.team == Team.Enemy)
                {
                    newDirection = (Player.instance.transform.position - transform.position).normalized;
                }
                else
                {
                    float sqrMagnitude;
                    Damagable closest = EnemyManager.instance.GetClosest(transform.position, out sqrMagnitude);
                    if (_stats.team == Team.None)
                    {
                        if ((Player.instance.transform.position - transform.position).sqrMagnitude < sqrMagnitude)
                        {
                            closest = Player.instance;
                        }
                    }

                    if (closest)
                    {
                        newDirection = (closest.transform.position - transform.position).normalized;
                    }
                }

                _direction = Vector3.Lerp(_direction, newDirection, _stats.homing);
            }

            if(_stats.gravity > 0 && _direction.y > -1)
            {
                _direction.y -= _stats.gravity * Time.deltaTime;
                if(!_stats.lockRotation)
                {
                    transform.rotation = Quaternion.FromToRotation(Vector3.right, _direction);
                }
            }
            transform.position += _direction * _stats.speed * Time.deltaTime;
        }

        _lifeCounter += Time.deltaTime;

        var camDelta = MainCamera.instance.transform.position - transform.position;
        camDelta.z = 0;

        if (_lifeCounter > _stats.lifeSpan || (camDelta).sqrMagnitude > _destroyDistance * _destroyDistance)
        {
            StartCoroutine(Die());
        }
    }

    public void Shoot(ProjectileStats stats, Vector3 origin, Vector3 direction)
    {
        alive = true;
        transform.parent = null;
        transform.position = _origin = origin;         
        _stats = stats;
        _direction = direction.normalized;
        _lifeCounter = 0;
        _material.color = _orignalColor;
        gameObject.SetActive(true);

        if (_stats.motionPattern != null && _stats.motionPattern.length > 0)
        {
            _motionPattern = _stats.motionPattern;
            _tangentDirection = Vector3.Cross(Vector3.back, _direction);
        }
        else
        {
            _motionPattern = null;
        }

        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction);

        gameObject.layer = LayerConstants.Projectile;
        if (_stats.ignoreTerrain)
        {
            switch (_stats.team)
            {
                case Team.Player:
                    gameObject.layer = LayerConstants.EnemyTrigger;
                    break;
                case Team.Enemy:
                    gameObject.layer = LayerConstants.PlayerTrigger;
                    break;
            }
        }
    }

    public IEnumerator Die()
    {
        if (deathEvent != null)
        {
            deathEvent();
        }

        alive = false;
        if (hasDeathAnimation)
        {
            if (_animator) { _animator.SetTrigger("Die"); }
            yield return new WaitForSeconds(deathTime);
        }
        else
        {            

            var targetColor = _material.color;
            targetColor.a = 0;
            var halfDeath = deathTime / 2;
            yield return new WaitForSeconds(halfDeath);
            var timer = 0f;
            while (timer < halfDeath)
            {
                timer += Time.deltaTime;
                _material.color = Color.Lerp(_orignalColor, targetColor, timer / halfDeath);
                yield return null;
            }
        }
        transform.parent = ProjectileManager.instance.transform;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!alive)
        {
            return;
        }

        bool hit = false;
        bool validLayer = true;

        switch (_stats.team)
        {
            case Team.Player:
                validLayer = collider.gameObject.layer != LayerConstants.Player;
                break;
            case Team.Enemy:
                validLayer = collider.gameObject.layer != LayerConstants.Enemy;
                break;
        }

        if (validLayer)
        {
            var damagable = collider.GetComponent<Damagable>();
            if (damagable)
            {
                if (damagable.state != DamagableState.Dead && damagable.state != DamagableState.Dying)
                {
                    damagable.Hurt(_stats.damage, gameObject);
                    hit = true;
                }
            }
        }

        if ((!_stats.ignoreTerrain && validLayer) || hit)
        {
            StartCoroutine(Die());
        }
    }
}
