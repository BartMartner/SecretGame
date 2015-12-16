using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour 
{
    public ProjectileStats projectileStats;
    public Vector3 direction = Vector3.right;
    public bool targetPlayer;
    public bool trackPlayer;
    public bool applyTransform;

    public float shootTime = 1;
    public float shootCounter;

    public int burstCount = 1;
    public float burstTime;

    public int arcShots = 1;
    [Range(0,360)]
    public float fireArc = 0;

    public Animator animator;

    private bool _shooting;
    private Vector3 _currentDirection;

    private void Start()
    {
        direction.Normalize();

        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        if (!_shooting)
        {
            shootCounter += Time.deltaTime;
            if (shootCounter > shootTime)
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        shootCounter = 0;

        if (animator)
        {
            animator.SetTrigger("Shoot");
        }

        if (targetPlayer && trackPlayer)
        {
            StartCoroutine(TrackingShot());
        }
        else
        {
            if (targetPlayer)
            {
                _currentDirection = (Player.instance.transform.position - transform.position).normalized;
            }
            else if (applyTransform)
            {
                _currentDirection = transform.TransformDirection(direction.normalized);
            }
            else
            {
                _currentDirection = direction.normalized;
            }

            if (burstCount <= 1)
            {
                if (fireArc != 0 && arcShots > 1)
                {
                    ProjectileManager.instance.ArcShoot(projectileStats, transform.position, _currentDirection, arcShots, fireArc);
                }
                else
                {
                    ProjectileManager.instance.Shoot(projectileStats, transform.position, _currentDirection);
                }
            }
            else
            {
                ProjectileManager.instance.BurstShoot(projectileStats, transform.position, _currentDirection, burstCount, burstTime, arcShots, fireArc);
                StartCoroutine(WaitForBurst());
            }
        }
    }

    public IEnumerator WaitForBurst()
    {
        _shooting = true;
        yield return new WaitForSeconds(burstTime);
        _shooting = false;
    }

    public IEnumerator TrackingShot()
    {
        _shooting = true;

        var shotsFired = 0;
        while (shotsFired < burstCount)
        {
            shotsFired++;

            direction = (Player.instance.transform.position - transform.position).normalized;

            if (fireArc != 0 && arcShots > 1)
            {
                ProjectileManager.instance.ArcShoot(projectileStats, transform.position, direction, arcShots, fireArc);
            }
            else
            {
                ProjectileManager.instance.Shoot(projectileStats, transform.position, direction);
            }

            yield return new WaitForSeconds(burstTime/burstCount);
        }

        _shooting = false;
    }

    public void OnDrawGizmosSelected()
    {
        var color = new Color(1, 0, 0, 0.5f);
        if (targetPlayer && Player.instance)
        {
            _currentDirection = (Player.instance.transform.position - transform.position).normalized;
        }
        else if (applyTransform)
        {
            _currentDirection = transform.TransformDirection(direction);
        }
        else
        {
            _currentDirection = direction;
        }


        if (projectileStats.gravity > 0)
        {
            var lifeSpan = projectileStats.lifeSpan;
            var timeDelta = 1f / 4f;
            var start = transform.position;
            direction = transform.TransformDirection(_currentDirection);

            while (lifeSpan > 0)
            {
                lifeSpan -= timeDelta;
                if (direction.y > -1)
                {
                    direction.y -= projectileStats.gravity * timeDelta;
                }

                var end = start + direction * timeDelta * projectileStats.speed;
                Debug.DrawLine(start, end, color);
                start = end;
            }
        }
        else if (fireArc != 0 && arcShots > 1)
        {
            for (int i = 0; i < arcShots; i++)
            {
                float angleMod = (((float)i / (arcShots - 1f)) * 2f) - 1f;
                Vector3 shotDirection = (Quaternion.AngleAxis(angleMod * fireArc / 2, Vector3.forward) * _currentDirection).normalized;
                Debug.DrawLine(transform.position, transform.position + shotDirection * projectileStats.lifeSpan * projectileStats.speed, color);
            }
        }
        else
        {
            Debug.DrawLine(transform.position, transform.position + _currentDirection * projectileStats.lifeSpan * projectileStats.speed, color);
        }
    }
}
