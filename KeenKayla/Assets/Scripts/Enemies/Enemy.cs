using UnityEngine;
using System.Collections;

public class Enemy : Damagable
{
    protected Animator _animator;

    protected virtual void Awake()
    {
        base.Awake();
        _animator = GetComponentInChildren<Animator>();
    }
}
