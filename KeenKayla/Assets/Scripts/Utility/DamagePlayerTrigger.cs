using UnityEngine;
using System.Collections;

public class DamagePlayerTrigger : MonoBehaviour 
{
    public float damage;

    public void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player.instance.Hurt(damage, gameObject);
        }
    }
}
