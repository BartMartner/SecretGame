using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour 
{
    public static EnemyManager instance;
    public List<Enemy> enemies = new List<Enemy>();

	private void Awake () 
    {
        instance = this;
	}

    private void OnDestroy()
    {
        instance = null;
    }

    public Enemy GetClosest(Vector3 position, out float lowestMagnitude)
    {
        lowestMagnitude = float.MaxValue;
        Enemy closest = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemy = enemies[i];
            if (enemy.state == DamagableState.Alive)
            {
                var magnitude = (enemy.transform.position - position).sqrMagnitude;
                if(magnitude < lowestMagnitude)
                {
                    lowestMagnitude = magnitude;
                    closest = enemy;
                }
            }
        }

        return closest;
    }
}
