using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    public void OnDestroy()
    {
        ProjectileManager.instance.SpawnExplosion(transform.position + Vector3.up * 0.125f);
    }
}
