using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    public void OnDestroy()
    {
        ProjectileManager.instance.SpawnExplosion(transform.position);
    }
}
