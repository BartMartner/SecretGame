using UnityEngine;
using System.Collections;

public class LazerPowerUpgrade : Pickup
{
    public int id;

    public void Start()
    {
        if (SaveGameManager.instance.saveGameData.lazerPowerUpgradesCollected.Contains(id))
        {
            Destroy(gameObject);
        }
    }

    public override void OnPickup()
    {
        SaveGameManager.instance.saveGameData.lazerPowerUpgradesCollected.Add(id);
        Player.instance.maxLazerPower += 1;
        Player.instance.lazerPower = Player.instance.maxHealth;
        base.OnPickup();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, id.ToString());
    }
}
