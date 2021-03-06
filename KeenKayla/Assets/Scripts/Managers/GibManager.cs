﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Random = UnityEngine.Random;

public class GibManager : MonoBehaviour
{
    public static GibManager instance;

    public List<Gib> BlueRockGibs;
    public List<Gib> BrownRockGibs;
    public List<Gib> MeatGibs;
    public List<Gib> MetalGibs;
    public Dictionary<GibType, List<Gib>> _gibs = new Dictionary<GibType, List<Gib>>();
    public Dictionary<GibType, int> _gibPrefabIndex = new Dictionary<GibType, int>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    { 
        var gibTypes = Enum.GetValues(typeof(GibType));

        foreach (GibType gType in gibTypes)
        {
            _gibPrefabIndex.Add(gType, 0);
        }

        foreach (GibType gType in Enum.GetValues(typeof(GibType)))
        {
            _gibs.Add(gType, new List<Gib>());
            NewGib(gType);
        }
    }

    public Gib NewGib(GibType gType)
    {
        if(gType == GibType.None)
        {
            return null;
        }

        Gib newGib = null;

        switch (gType)
        {
            case GibType.BlueRock:
                newGib = Instantiate(BlueRockGibs[_gibPrefabIndex[gType]]) as Gib;
                break;
            case GibType.BrownRock:
                newGib = Instantiate(BrownRockGibs[_gibPrefabIndex[gType]]) as Gib;
                break;
            case GibType.Meat:
                newGib = Instantiate(MeatGibs[_gibPrefabIndex[gType]]) as Gib;
                break;
            case GibType.Metal:
                newGib = Instantiate(MetalGibs[_gibPrefabIndex[gType]]) as Gib;
                break;
        }

        if (newGib)
        {
            newGib.transform.parent = transform;
            newGib.gameObject.SetActive(false);
            _gibs[gType].Add(newGib);
        }

        int gibCount = 0;

        switch (gType)
        {
            case GibType.BlueRock:
                gibCount = BlueRockGibs.Count;
                break;
            case GibType.BrownRock:
                gibCount = BrownRockGibs.Count;
                break;
            case GibType.Meat:
                gibCount = MeatGibs.Count;
                break;
            case GibType.Metal:
                gibCount = MetalGibs.Count;
                break;
        }

        _gibPrefabIndex[gType] = (_gibPrefabIndex[gType] + 1) % gibCount;

        return newGib;
    }

    public void SpawnGibs(GibType gType, Rect area, int amount, float force  = 10, float lifeSpan = 5)
    {
        List<Gib> eList;

        if (_gibs.TryGetValue(gType, out eList))
        {
            for (int a = 0; a < amount; a++)
            {
                Gib g = null;

                for (int i = 0; i < eList.Count; i++)
                {
                    if (!eList[i].gameObject.activeInHierarchy)
                    {
                        g = eList[i];
                        break;
                    }
                }

                if (!g)
                {
                    g = NewGib(gType);
                }

                var position = new Vector2(Random.Range(area.xMin, area.xMax), Random.Range(area.yMin, area.yMax));

                g.Spawn(gType, position, force, lifeSpan);
            }
        }
    }
}
