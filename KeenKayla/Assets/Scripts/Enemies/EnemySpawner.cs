using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public Transform spawnPoint;
    public float delay = 3f;
    public int enemiesToSpawn = 1;
    public float spawnDelay = 1;
    private SpriteRenderer _renderer;

    public void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void OnEnable()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(0.1f);

        while (gameObject.activeInHierarchy)
        {
            if (_renderer)
            {
                while(!_renderer.isVisible)
                {
                    yield return new WaitForSeconds(1);
                }
            }

            yield return new WaitForSeconds(delay);

            for (int i = 0; i < enemiesToSpawn; i++)
            {
                if(!spawnPoint)
                {
                    spawnPoint = transform;
                }

                Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

                if (i != enemiesToSpawn - 1)
                {
                    yield return new WaitForSeconds(spawnDelay);
                }
            }

            yield return null;
        }
    }
}
