using UnityEngine;
using System.Collections;

public class Door : Damagable
{
    [Header("Door")]
    public BoxCollider2D boxCollider2D;
    public GameObject shield;

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
