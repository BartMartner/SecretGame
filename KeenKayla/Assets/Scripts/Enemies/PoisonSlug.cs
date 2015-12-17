using UnityEngine;
using System.Collections;

public class PoisonSlug : Enemy
{
    private DamagePlayerTrigger _damagePlayerTrigger;

    protected override void Awake()
    {
        base.Awake();
        _damagePlayerTrigger = GetComponentInChildren<DamagePlayerTrigger>();
    }

    public override void Die()
    {
        base.Die();

        _damagePlayerTrigger.gameObject.SetActive(false);

        if (Random.value < 0.5)
        {
            _animator.SetTrigger("Death1");
        }
        else
        {
            _animator.SetTrigger("Death2");
        }

        base.OnDeath();
    }
}
