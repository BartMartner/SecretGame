using UnityEngine;
using System.Collections;

public class Faller : Enemy
{
    [Header ("Faller")]
    public Sprite aboutToFallSprite;
    public LayerMask explosionLayerMask;
    private Rigidbody2D _rigidbody2D;
    private bool _falling;
    private float _halfHeight;

    protected override void Awake()
    {
        base.Awake();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _halfHeight = _collider2D.bounds.extents.y + 0.125f;
    }

    protected override void UpdateAlive()
    {
        base.UpdateAlive();

        if (_falling)
        {
            var result = Physics2D.Raycast(transform.position, Vector3.down, _halfHeight*2, explosionLayerMask);
            var xDistance = Player.instance.transform.position.x - transform.position.x; 
            if (!_rigidbody2D.isKinematic)
            {
                transform.position += Mathf.Sign(xDistance) * Vector3.right * Mathf.Min(Mathf.Abs(xDistance), 3f * Time.deltaTime);
            }

            if (result.collider && result.collider != _collider2D &&
                result.distance < _halfHeight && !result.collider.gameObject.GetComponent<Projectile>())
            {
                Destroy(gameObject);
                ProjectileManager.instance.SpawnExplosion(transform.position);
            }
        }
        else
        {
            var result = Physics2D.Raycast(transform.position, Vector3.down, 100, Player.instance.playerLayerMask);
            if (result.collider)
            {
                StartCoroutine(WaitThenFall());
                _falling = true;
            }
        }
    }

    public override bool Hurt(float damage, GameObject source = null, DamageType damageType = DamageType.Generic)
    {
        if (base.Hurt(damage, source, damageType))
        {
            _rigidbody2D.velocity = Vector3.zero;
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator WaitThenFall()
    {
        animator.enabled = false;
        _renderers[0].sprite = aboutToFallSprite;
        yield return new WaitForSeconds(0.4f);
        animator.enabled = true;
        _rigidbody2D.isKinematic = false;
        animator.SetTrigger("Fall");
    }
}
