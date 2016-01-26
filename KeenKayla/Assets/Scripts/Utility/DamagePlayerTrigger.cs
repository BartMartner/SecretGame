using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]
public class DamagePlayerTrigger : MonoBehaviour 
{
    public float damage = 1;

    public void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player.instance.Hurt(damage, gameObject);
        }
    }
}
