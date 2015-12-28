using UnityEngine;
using System.Collections;

public class Enemy : Damagable
{
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

    public virtual void OnDestroy()
    {
        if(EnemyManager.instance)
        { 
            EnemyManager.instance.enemies.Remove(this);
        }
    }
}
