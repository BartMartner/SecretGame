using UnityEngine;
using System.Collections;

public class Door : Damagable
{
    public BoxCollider2D boxCollider2D;
    public GameObject shield;

    protected override void Awake()
    {
        base.Awake();
        boxCollider2D = GetComponentInChildren<BoxCollider2D>();
    }

    public override bool Hurt(float damage, GameObject source = null, DamageType damageType = DamageType.Generic)
    {
        if (!immunities.Contains(damageType))
        {
            boxCollider2D.enabled = false;
            shield.gameObject.SetActive(false);
            return true;
        }
        else
        {
            return false;
        }
    }
}
