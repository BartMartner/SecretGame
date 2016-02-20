﻿using UnityEngine;
using System.Collections;

public class HealthUpgrade : Pickup
{
    public int id;

    public void Start()
    {
        if (SaveGameManager.instance.saveGameData.healthUpgradesCollected.Contains(id))
        {
            Destroy(gameObject);
        }
    }

    public override void OnPickup()
    {
        SaveGameManager.instance.saveGameData.healthUpgradesCollected.Add(id);
        Player.instance.maxHealth += 1;
        Player.instance.health = Player.instance.maxHealth;
        //SaveGameManager.instance.SaveGame();
        UIMain.instance.ShowHeartPopup();
        base.OnPickup();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, id.ToString());
    }
}
