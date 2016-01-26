using UnityEngine;
using System.Collections;

public static class Constants
{
    public const int bombsPerUpgrade = 2;
    public const float startingHealth = 3;

    public static LayerMask terrain = LayerMask.GetMask("Default", "OneWayPlatform", "DamagableTerrain");

    public static ProjectileStats GreenBolts = new ProjectileStats
    {
        type = ProjectileType.BlasterBolt,
        team = Team.Player,
        damage = 1,
        speed = 8,
        lifeSpan = 10,
    };

    public static ProjectileStats RedBolts = new ProjectileStats
    {
        type = ProjectileType.RedBlasterBolt,
        team = Team.Player,
        damage = 2,
        speed = 10,
        lifeSpan = 10,
    };

    public static ProjectileStats PurpleBolts = new ProjectileStats
    {
        type = ProjectileType.PurpleBlasterBolt,
        team = Team.Player,
        damage = 3,
        speed = 10,
        homing = 0.05f,
        lifeSpan = 10,
    };
}
