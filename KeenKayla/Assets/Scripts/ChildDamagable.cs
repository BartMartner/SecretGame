using UnityEngine;
using System.Collections;

public class ChildDamagable : Damagable
{
    public Damagable parent;

    public override bool Hurt(float damage, GameObject source = null)
    {
        return parent.Hurt(damage, source);
    }
}
