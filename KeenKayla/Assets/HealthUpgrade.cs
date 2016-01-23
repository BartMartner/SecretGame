using UnityEngine;
using System.Collections;

public class HealthUpgrade : Pickup
{
    public int id;

    public override void OnPickup()
    {
        SaveGameManager.instance.saveGameData.healthUpgradesCollected.Add(id);
        Player.instance.maxHealth += 1;
        Player.instance.health = Player.instance.maxHealth;
        SaveGameManager.instance.SaveGame();
        base.OnPickup();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, id.ToString());
    }
}
