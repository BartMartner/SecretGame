using UnityEngine;
using System.Collections;

public class PowerUp : Pickup
{
    public PowerUpID powerUpType;

    public void Start()
    {
        if (SaveGameManager.instance.saveGameData.powerUpsCollected.Contains(powerUpType))
        {
            Destroy(gameObject);
        }
    }

    public override void OnPickup()
    {
        SaveGameManager.instance.saveGameData.powerUpsCollected.Add(powerUpType);
        //SaveGameManager.instance.SaveGame();
        Player.instance.health = Player.instance.maxHealth;
        Player.instance.RefreshPowerUps();
        UIMain.instance.ShowPopUp(powerUpType);
        base.OnPickup();
    }
}