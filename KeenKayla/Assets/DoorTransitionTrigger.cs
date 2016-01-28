using UnityEngine;
using System.Collections;

public class DoorTransitionTrigger : MonoBehaviour
{
    public BoxCollider2D trigger;
    public Transform exitPoint;
    public Door door;
    public GameObject[] destroyOnTransition;
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

        if (connectedTrigger.door)
        {
            connectedTrigger.door.Open();
        }

        foreach (var go in connectedTrigger.destroyOnTransition)
        {
            Destroy(go);
        }

        Player.instance.rigidbody2D.isKinematic = true;
        Player.instance.DisableMovement();

        while (Vector3.Distance(Player.instance.transform.position, connectedTrigger.exitPoint.position) > 0.25)
        {
            Player.instance.transform.position = Vector3.MoveTowards(Player.instance.transform.position, connectedTrigger.exitPoint.position, Time.deltaTime * 3);
            yield return false;
        }

        Player.instance.rigidbody2D.isKinematic = false;
        Player.instance.EnableMovement();

        connectedTrigger.trigger.enabled = true;

        if (connectedTrigger.door)
        {
            connectedTrigger.door.Close();
        }

        if (door)
        {
            door.Close();
        }
    }
}
