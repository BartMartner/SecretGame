using UnityEngine;
using System.Collections;

public class DamagedByPowerSuit : MonoBehaviour
{
    public float amount = 1;
    public Damagable damagable;
    public Collider2D collider;
    public bool fromTop = true;
    public bool fromLeft = true;
    public bool fromBottom = true;
    public bool fromRight = true;

    public void Awake()
    {
        if(!damagable)
        {
            damagable = GetComponent<Damagable>();
        }

        if(!collider)
        {
            collider = GetComponent<Collider2D>();
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {
            Damage();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player.instance.gameObject)
        {
            Damage();
        }
    }

    public void Damage()
    {
        var distance = Vector3.Distance(transform.position, Player.instance.transform.position) * 1.1f;

        if (fromTop && Physics2D.Raycast(Player.instance.transform.position, Vector3.down, distance, 1 << gameObject.layer))
        {
            damagable.Hurt(amount, Player.instance.gameObject, DamageType.PowerSuit);
            return;
        }

        if (fromBottom && Physics2D.Raycast(Player.instance.transform.position, Vector3.up, distance, 1 << gameObject.layer))
        {
            damagable.Hurt(amount, Player.instance.gameObject, DamageType.PowerSuit);
            return;
        }

        if (fromLeft && Physics2D.Raycast(Player.instance.transform.position, Vector3.right, distance, 1<< gameObject.layer))
        {
            damagable.Hurt(amount, Player.instance.gameObject, DamageType.PowerSuit);
            return;
        }

        if (fromRight && Physics2D.Raycast(Player.instance.transform.position, Vector3.left, distance, 1 << gameObject.layer))
        {
            damagable.Hurt(amount, Player.instance.gameObject, DamageType.PowerSuit);
            return;
        }
    }
}
