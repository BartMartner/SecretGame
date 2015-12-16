using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager instance;
    
    public Projectile blasterBoltPrefab;
    public int projectilesToInstantiate = 20;

    private Dictionary<ProjectileType, List<Projectile>> _projectiles = new Dictionary<ProjectileType, List<Projectile>>();

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (ProjectileType pType in Enum.GetValues(typeof(ProjectileType)))
	    {
            _projectiles.Add(pType, new List<Projectile>());
            NewProjectile(pType);
        }
    }

    public Projectile NewProjectile(ProjectileType pType)
    {
        Projectile newProjectile = null;

        switch(pType)
        {
            case ProjectileType.BlasterBolt:
                newProjectile = Instantiate(blasterBoltPrefab) as Projectile;
                break;
        }

        if (newProjectile)
        {
            newProjectile.transform.parent = transform;
            newProjectile.gameObject.SetActive(false);
            _projectiles[pType].Add(newProjectile);
        }

        return newProjectile;
    }

    public void Shoot(ProjectileStats stats, Vector3 origin, Vector3 direction)
    {
        List<Projectile> pList;

        if (_projectiles.TryGetValue(stats.type, out pList))
        {
            Projectile p = null;

            for (int i = 0; i < pList.Count; i++)
            {
                if (!pList[i].gameObject.activeInHierarchy)
                {
                    p = pList[i];
                    break;
                }
            }

            if (!p)
            {
                p = NewProjectile(stats.type);
            }

            p.Shoot(stats, origin, direction);
        }
    }

    public void ArcShoot(ProjectileStats stats, Vector3 origin, Vector3 direction, int arcShots, float fireArc)
    {
        if (fireArc != 0 && arcShots > 1)
        {
                for (int i = 0; i < arcShots; i++)
                {
                    float angleMod = (((float)i / (arcShots - 1f)) * 2f) - 1f;
                    Vector3 shotDirection = (Quaternion.AngleAxis(angleMod * fireArc / 2, Vector3.forward) * direction).normalized;
                    Shoot(stats, origin, shotDirection);
                }
        }
        else
        {
            Debug.LogWarning("ArcShot Should not be called for a single shot or with a fire arc of 0");
        }
    }

    public void BurstShoot(ProjectileStats stats, Vector3 origin, Vector3 direction, int burstCount, float burstTime, int arcShots = 1, float fireArc = 0)
    {
        StartCoroutine(BurstShot(stats, origin, direction, burstCount, burstTime, arcShots, fireArc));
    }

    private IEnumerator BurstShot(ProjectileStats stats, Vector3 origin, Vector3 direction, int burstCount, float burstTime, int arcShots, float fireArc)
    {
        var shotsFired = 0;
        while (shotsFired < burstCount)
        {
            shotsFired++;           

            if (fireArc != 0 && arcShots > 1)
            {
                ArcShoot(stats, origin, direction, arcShots, fireArc);
            }
            else
            {
                Shoot(stats, origin, direction);
            }

            yield return new WaitForSeconds(burstTime/burstCount);
        }
    }
}
