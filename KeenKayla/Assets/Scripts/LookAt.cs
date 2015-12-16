using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour 
{
	public GameObject focus;

	// Update is called once per frame
	void Update () 
	{
		transform.LookAt (-focus.transform.position);
	}
}
