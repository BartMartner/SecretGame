using UnityEngine;
using System.Collections;

public class HealthDrop : Pickup
{
    public AudioClip pickUpSound;
    public float amount = 1;
    public override void OnPickup()
    {
        AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
        Player.instance.health += amount;
        if(Player.instance.health > Player.instance.maxHealth)
        {
            Player.instance.health = Player.instance.maxHealth;
        }

        base.OnPickup();
    }
}
