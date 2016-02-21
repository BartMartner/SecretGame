using UnityEngine;
using System.Collections;

public class MadMushroom : Enemy
{
    public float offset = 0f;
    private Quaternion _flippedFacing = Quaternion.Euler(0, 180, 0);
    private Rigidbody2D _rigidBody2D;

    protected override void Start()
    {
        base.Start();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(Bounce());
    }

    protected override void UpdateAlive()
    {
        base.UpdateAlive();

        groundedCheck.UpdateRaycasts();

        var playerDistance = Vector3.Distance(transform.position, Player.instance.transform.position);
        if (playerDistance > 0.5F)
        {
            var direction = Mathf.Sin(Player.instance.transform.position.x - transform.position.x);
            if (direction < 0 && transform.rotation != _flippedFacing)
            {
                transform.rotation = _flippedFacing;
            }
            else if (direction > 0 && transform.rotation != Quaternion.identity)
            {
                transform.rotation = Quaternion.identity;
            }
        }
    }

    public override void OnImmune(DamageType damageType)
    {
        if (damageType == DamageType.Generic)
        {
            StartCoroutine(Flash(2, 0.1f, Color.green, 0.5f));
        }
    }

    public IEnumerator Bounce()
    {
        yield return new WaitForSeconds(offset);

        while (state == DamagableState.Alive)
        {
            for (int i = 0; i < 3; i++)
            {
                var bouncePower = 1f;
                if (i == 2)
                {
                    bouncePower = 1.75f;
                }

                _rigidBody2D.velocity = Vector3.zero;
                _rigidBody2D.AddForce(Vector3.up * bouncePower * 4, ForceMode2D.Impulse);

                yield return new WaitForSeconds(0.25f);

                while (!groundedCheck.onGround)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}
