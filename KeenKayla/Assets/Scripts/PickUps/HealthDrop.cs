using UnityEngine;
using System.Collections;

public class HealthDrop : Pickup
{
    public float amount = 1;
    public override void OnPickup()
    {
        Player.instance.health += amount;
        if(Player.instance.health > Player.instance.maxHealth)
        {
            Player.instance.health = Player.instance.maxHealth;
        }

        base.OnPickup();
    }
}
