using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour
{
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerConstants.Player)
        {
            Destroy(gameObject);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerConstants.Player)
        {
            Destroy(gameObject);
        }
    }
}
