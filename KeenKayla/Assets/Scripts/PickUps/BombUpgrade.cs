using UnityEngine;
using System.Collections;

public class BombUpgrade : Pickup
{
    public int id;

    public override void OnPickup()
    {
        SaveGameManager.instance.saveGameData.bombUpgradesCollected.Add(id);
        Player.instance.currentBombs += 3;
        Player.instance.maxBombs += 3;
        SaveGameManager.instance.SaveGame();
        base.OnPickup();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, id.ToString());
    }
}
