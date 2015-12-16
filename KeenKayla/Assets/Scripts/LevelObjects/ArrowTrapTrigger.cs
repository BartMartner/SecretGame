using UnityEngine;
using System.Collections;

public class ArrowTrapTrigger : MonoBehaviour
{
    public bool firing;
    public ProjectileStats projectileStats;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!firing)
        {
            StartCoroutine(ArrowTrap());
        }
    }

    public IEnumerator ArrowTrap()
    {
        firing = true;
        yield return new WaitForSeconds(0.25f);
        Vector3 origin;
        Vector3 destination;

        for (int i = 0; i < 30; i++)
        {
            origin = transform.position;
            destination = transform.position;
            origin.y += 8;
            origin.x += Random.Range(-2.5f, 2.5f);
            origin.z += Random.Range(-2.5f, 2.5f);
            destination.x += Random.Range(-2.5f, 2.5f);
            destination.z += Random.Range(-0.5f,0.5f);
            ProjectileManager.instance.Shoot(projectileStats, origin, (destination-origin).normalized);
            yield return new WaitForSeconds(Random.Range(0.02f, 0.15f));
        }

        firing = false;
    }
}
