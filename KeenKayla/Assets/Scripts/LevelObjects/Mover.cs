using UnityEngine;
using System.Collections;

using System;

public class Mover : MonoBehaviour {

    public float speed = 2;

    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(startPos.x, startPos.y + 3.3f, startPos.z);
    }

	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector3.Lerp(startPos, endPos, Mathf.PingPong(Time.time * speed, 1));
    }
}
