﻿using UnityEngine;
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
        parentScene = transform.root.name.ToLowerInvariant();
        targetScene = targetScene.ToLowerInvariant();
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
        Player.instance.lockAnimator = true;
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

            var scene = SceneManager.GetSceneByName("Core");
            SceneManager.SetActiveScene(scene);

            yield return null;

            var doorTriggers = FindObjectsOfType<DoorTransitionTrigger>();
            Debug.Log("Found " + doorTriggers.Count() + " door triggers");
            var validTriggers = doorTriggers.Where(t => Vector3.Distance(t.transform.position, transform.position) < 0.25f && t.parentScene == targetScene);
            Debug.Log("Found " + validTriggers.Count() + "valid door triggers");
            connectedTrigger = validTriggers.First();
        }

        connectedTrigger.StopAllCoroutines();
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
            Player.instance.transform.position = Vector3.MoveTowards(Player.instance.transform.position, connectedTrigger.exitPoint.position, Time.unscaledDeltaTime * 2.5f);
            yield return false;
        }

        if (connectedTrigger.door)
        {
            connectedTrigger.door.Close();
            connectedTrigger.door.locked = true;
        }

        Player.instance.rigidbody2D.isKinematic = false;
        Player.instance.EnableMovement();
        Player.instance.lockAnimator = false;

        while (MainCamera.instance.tweening)
        {
            yield return false;
        }

        if (connectedTrigger.door)
        {
            connectedTrigger.door.locked = false;
        }

        connectedTrigger.trigger.enabled = true; 

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
