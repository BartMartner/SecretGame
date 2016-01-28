using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class DoorTransitionTrigger : MonoBehaviour
{
    [HideInInspector]
    public string parentScene;
    public string targetScene;    
    public BoxCollider2D trigger;
    public Transform exitPoint;
    public Door door;
    public GameObject[] destroyOnTransition;
    public DoorTransitionTrigger connectedTrigger;

    public void Awake()
    {
        parentScene = transform.root.name;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != Player.instance.gameObject)
        {
            return;
        }

        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        Player.instance.rigidbody2D.isKinematic = true;
        Player.instance.DisableMovement();
        trigger.enabled = false;

        var loadNewScene = false;

        if (!connectedTrigger)
        {
            loadNewScene = true;

            if (!SceneManager.GetSceneByName(targetScene).isLoaded)
            {
                SceneManager.LoadScene(targetScene, LoadSceneMode.Additive);
                yield return null;
            }

            var scene = SceneManager.GetSceneByName(targetScene);
            SceneManager.SetActiveScene(scene);

            var doorTriggers = FindObjectsOfType<DoorTransitionTrigger>().Where(t => Vector3.Distance(t.transform.position, transform.position) < 0.1f && t.parentScene == targetScene);
            Debug.Log("Found " + doorTriggers.Count() + " door triggers");

            yield return null;

            connectedTrigger = doorTriggers.First();
        }

        connectedTrigger.trigger.enabled = false;

        if (connectedTrigger.door)
        {
            connectedTrigger.door.Open();
        }

        foreach (var go in connectedTrigger.destroyOnTransition)
        {
            Destroy(go);
        }

        while (Vector3.Distance(Player.instance.transform.position, connectedTrigger.exitPoint.position) > 0.25)
        {
            Player.instance.transform.position = Vector3.MoveTowards(Player.instance.transform.position, connectedTrigger.exitPoint.position, Time.unscaledDeltaTime * 3);
            yield return false;
        }

        while(MainCamera.instance.tweening)
        {
            yield return false;
        }

        Player.instance.rigidbody2D.isKinematic = false;
        Player.instance.EnableMovement();

        connectedTrigger.trigger.enabled = true;

        if (connectedTrigger.door)
        {
            connectedTrigger.door.Close();
        }

        if(loadNewScene)
        {
            SceneManager.UnloadScene(parentScene);
        }
        else
        {
            trigger.enabled = true;
            if (door)
            {
                door.Close();
            }
        }
    }
}
