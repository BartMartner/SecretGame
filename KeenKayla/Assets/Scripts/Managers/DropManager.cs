using UnityEngine;
using System.Collections;

public class DropManager : MonoBehaviour
{
    public static DropManager instance;

    public HealthDrop healthDropPrefab;
    public BombDrop bombDropPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void EnemyDrop(Vector3 position)
    {
        var maxHealth = Player.instance.health == Player.instance.maxHealth;
        var maxBombs = Player.instance.currentBombs == Player.instance.maxBombs;

        if(maxHealth && maxBombs)
        {
            return;
        }

        if (Player.instance.health <= 1 || maxBombs)
        {
            Instantiate(healthDropPrefab, position, Quaternion.identity);
        }
        else if (Player.instance.currentBombs <= 1 || maxHealth)
        {
            Instantiate(bombDropPrefab, position, Quaternion.identity);
        }
        else if (Random.value > 0.5)
        {
            Instantiate(healthDropPrefab, position, Quaternion.identity);
        }
        else
        {
            Instantiate(bombDropPrefab, position, Quaternion.identity);
        }
    }
}
