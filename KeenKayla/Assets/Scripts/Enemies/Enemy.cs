using UnityEngine;
using System.Collections;

public class Enemy : Damagable
{
    protected Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetComponentInChildren<Animator>();
        EnemyManager.instance.enemies.Add(this);
    }

    public virtual void OnDestroy()
    {
        EnemyManager.instance.enemies.Remove(this);
    }
}
