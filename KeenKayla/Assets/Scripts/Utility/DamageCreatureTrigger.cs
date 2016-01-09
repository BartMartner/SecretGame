using UnityEngine;
using System.Collections;

public class DamageCreatureTrigger : MonoBehaviour
{
    public float damage;
    public DamageType damageType;

    public void OnTriggerStay2D(Collider2D other)
    {
        var damagable = other.GetComponentInChildren<Damagable>();
        if(damagable)
        {
            damagable.Hurt(damage, gameObject);
        }
    }
}
