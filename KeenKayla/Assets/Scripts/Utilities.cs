using UnityEngine;
using System.Collections;

public static class Utilities
{
	public static int GetAttackStates(WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Spear:
                return 3;
            case WeaponType.Sword:
                return 3;
            default:
                return 1;
        }
    }
}
