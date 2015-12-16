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

public enum WeaponType
{
    Sword = 0,
    Spear = 1,
    Unarmed = 2,
}

public enum EnergyType
{
    Health = 0,
    Stamina = 1,
    Devotion = 2,
}

public enum ItemSlotType
{
    Belt = 0,
    Stash = 1,
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

