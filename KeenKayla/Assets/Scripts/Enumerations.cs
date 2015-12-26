using UnityEngine;
using System.Collections;

public enum ItemType
{
    Consumables = 0,
    Weapons = 1,
    ActiveBoons = 2,
    PassiveBoons = 3,
    Artifacts = 4,
}

public enum PowerUpType
{
    PogoStick = 0,
    MaruMari = 1,
}

public enum LazeMods
{
    RedLazer = 0,
}

public enum Team
{
    None = 0,
    Player = 1,
    Enemy = 2,
}

public enum DamagableState
{
    Alive = 0,
    Dying = 1,
    Dead = 2,
}

public enum ProjectileType
{
    Generic = 0,
    BlasterBolt = 1,
    RedBlasterBolt = 2,
    PurpleBlasterBolt = 3,
}

public enum EnvironmentType
{
    None = 0,
    Beach = 1,
    Temple = 2,
    DeathArena = 3,
}

public enum Possibility
{
    Never = 0,
    Sometimes = 1,
    Always = 2,
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
}

public enum SurfaceType
{
    Sand,
}

public enum DamageType
{
    Generic,
    Fire,
}

