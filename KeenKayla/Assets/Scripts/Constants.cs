using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour
{
    public static ProjectileStats GreenBolts = new ProjectileStats
    {
        type = ProjectileType.BlasterBolt,
        damage = 1,
        speed = 8,
        lifeSpan = 10,
    };

    public static ProjectileStats RedBolts = new ProjectileStats
    {
        type = ProjectileType.RedBlasterBolt,
        damage = 2,
        speed = 10,
        lifeSpan = 10,
    };

    public static ProjectileStats PurpleBolts = new ProjectileStats
    {
        type = ProjectileType.PurpleBlasterBolt,
        damage = 3,
        speed = 10,
        homing = true,
        lifeSpan = 10,
    };
}
