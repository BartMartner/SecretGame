using UnityEngine;
using System.Collections;

public class BombDrop : Pickup
{
    public int amount = 1;
    public override void OnPickup()
    {
        Player.instance.currentBombs += amount;
        if (Player.instance.currentBombs > Player.instance.maxBombs)
        {
            Player.instance.currentBombs = Player.instance.maxBombs;
        }

        base.OnPickup();
    }
}
