using UnityEngine;
using System.Collections;

public class Enemy : Damagable
{
    public float dropChance = 0.75f;
    protected Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponentInChildren<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        EnemyManager.instance.enemies.Add(this);
    }

    public override void OnDeath()
    {
        if (Random.value < dropChance)
        {
            DropManager.instance.EnemyDrop(transform.position);
        }

        base.OnDeath();
    }

    public virtual void OnDestroy()
    {
        if(EnemyManager.instance)
        { 
            EnemyManager.instance.enemies.Remove(this);
        }
    }
}
