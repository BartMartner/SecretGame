using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public static class Extensions
{
    public static TEnum[] GetEnumValues<TEnum>()
    {
        return (TEnum[])Enum.GetValues(typeof(TEnum));
    }

    public static bool GetFirstNullOrEmpty<T>(this T[] array, out uint index)
    {
        for (uint i = 0; i < array.Length; i++)
        {
            if (array[i] == null)
            {
                index = i;
                return true;
            }
        }

        index = 0;
        return false;
    }

    public static bool GetNextNonEmpty<T>(this T[] array, uint startingIndex, out uint index)
    {
        index = (uint)((startingIndex+1) % array.Length);

        while (index != startingIndex)
        {
            if (array[index] != null)
            {
                return true;
            }
            index = (uint)((index+1) % array.Length);
        }

        return false;
    }

    public static uint GetFirstNullOrEmpty<T>(this Dictionary<uint, T> dictionary)
    {
        uint i = 0;
        while (i < uint.MaxValue)
        {
            if (!dictionary.ContainsKey(i) || dictionary[i] == null)
            {
                return i;
            }
            i++;
        }

        return 0;
    }
}
