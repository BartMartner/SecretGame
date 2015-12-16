using UnityEngine;
using System.Collections;

public class PitTrigger : MonoBehaviour 
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        var damagable = other.GetComponent<Damagable>();
        if (damagable)
        {
            if (damagable.gameObject.layer == LayerConstants.Player)
            {
                MainCamera.instance.activeTracking = false;
            }

            damagable.Die();
        }
    }
}
