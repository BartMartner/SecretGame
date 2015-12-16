using UnityEngine;
using System.Collections;

public class Roller : MonoBehaviour
{
    public float moveSpeed = 2;
    public float rotateSpeed = 2;

    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        startPos = transform.position;
        endPos = new Vector3(startPos.x + 20, startPos.y, startPos.z);
    }

	// Update is called once per frame
	void Update ()
    {
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, -Mathf.PingPong(Time.time * rotateSpeed, 180.0f));
        transform.position = Vector3.Lerp(startPos, endPos, Mathf.PingPong(Time.time * moveSpeed, 1));
	}
}
