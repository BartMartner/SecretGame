using UnityEngine;
using System.Collections;

public class ProjectileSpawnEffect : MonoBehaviour
{
    public GameObject effectPrefab;

	// Use this for initialization
	public void Start ()
    {
        var projectile = GetComponent<Projectile>();
        projectile.deathEvent += SpawnEffect;
	}
	
	public void SpawnEffect()
    {
        Instantiate(effectPrefab, transform.position, Quaternion.identity);
    }
}
