using UnityEngine;
using System.Collections;

public class ColdZone : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;

    private void Awake()
    {
        gameObject.layer = LayerConstants.PlayerTrigger;
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _boxCollider2D.isTrigger = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player.instance.coldZoneID = gameObject.GetInstanceID();
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (Player.instance.coldZoneID == gameObject.GetInstanceID())
        {
            Player.instance.coldZoneID = 0;
        }
    }

    public void OnDrawGizmos()
    {
    }
}
