using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ProjectileStats
{
    public ProjectileType type = ProjectileType.Generic;
    public Team team;
    public float damage;
    public DamageType damageType = DamageType.Generic;
    public float speed = 8;
    public float lifeSpan = 10;
    public float gravity = 0;
    public float homing = 0;
    public AnimationCurve motionPattern;
    public bool lockRotation;
    public bool ignoreTerrain;
}
