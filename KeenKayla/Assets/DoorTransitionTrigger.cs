using UnityEngine;
using System.Collections;

public class DoorTransitionTrigger : MonoBehaviour
{
    public BoxCollider2D trigger;
    public Transform exitPoint;
    public Door door;
    public DoorTransitionTrigger connectedTrigger;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject != Player.instance.gameObject)
        {
            return;
        }

        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        connectedTrigger.trigger.enabled = false;
        connectedTrigger.door.Open();

        Player.instance.rigidbody2D.isKinematic = true;
        Player.instance.DisableMovement();

        while (Player.instance.transform.position != connectedTrigger.exitPoint.position)
        {
            Player.instance.transform.position = Vector3.MoveTowards(Player.instance.transform.position, connectedTrigger.exitPoint.position, Time.deltaTime * 3);
            yield return false;
        }

        Player.instance.rigidbody2D.isKinematic = false;
        Player.instance.EnableMovement();

        connectedTrigger.trigger.enabled = true;
        connectedTrigger.door.Close();
        door.Close();
    }
}
