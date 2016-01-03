using UnityEngine;
using System.Collections;

public class PowerUp : Pickup
{
    public PowerUpID powerUpType;

    public override void OnPickup()
    {
        SaveGameManager.instance.saveGameData.powerUpsCollected.Add(powerUpType);
        SaveGameManager.instance.SaveGame();
        Player.instance.RefreshPowerUps();
        base.OnPickup();
    }
}
