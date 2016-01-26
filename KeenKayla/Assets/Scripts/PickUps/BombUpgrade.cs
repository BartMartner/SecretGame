using UnityEngine;
using System.Collections;

public class BombUpgrade : Pickup
{
    public int id;

    public void Awake()
    {
        if(SaveGameManager.instance.saveGameData.bombUpgradesCollected.Contains(id))
        {
            Destroy(gameObject);
        }
    }

    public override void OnPickup()
    {
        SaveGameManager.instance.saveGameData.bombUpgradesCollected.Add(id);
        Player.instance.currentBombs += Constants.bombsPerUpgrade;
        Player.instance.maxBombs += Constants.bombsPerUpgrade;
        //SaveGameManager.instance.SaveGame();
        base.OnPickup();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, id.ToString());
    }
}
