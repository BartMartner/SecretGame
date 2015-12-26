using UnityEngine;
using System.Collections;

public class PowerUp : Pickup
{
    public PowerUpID powerUpType;

    public override void OnPickup()
    {
        SaveGameManager.instance.saveGameData.powerUpsCollected.Add(powerUpType);
        SaveGameManager.instance.SaveGame();
        PlayerController.instance.RefreshPowerUps();
        base.OnPickup();
    }
}
