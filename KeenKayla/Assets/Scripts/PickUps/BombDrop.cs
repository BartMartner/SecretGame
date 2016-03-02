using UnityEngine;
using System.Collections;

public class BombDrop : Pickup
{
    public AudioClip pickUpSound;

    public int amount = 1;
    public override void OnPickup()
    {
        AudioSource.PlayClipAtPoint(pickUpSound, transform.position);
        Player.instance.currentBombs += amount;
        if (Player.instance.currentBombs > Player.instance.maxBombs)
        {
            Player.instance.currentBombs = Player.instance.maxBombs;
        }

        base.OnPickup();
    }
}
