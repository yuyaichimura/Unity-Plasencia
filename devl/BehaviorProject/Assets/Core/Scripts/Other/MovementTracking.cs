using UnityEngine;
using System.Collections;

public class MovementTracking : MonoBehaviour {

	public float idleTime;
	private Vector3 lastPosition;


	// Use this for initialization
	void Start () {
		idleTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {

		float vel 		= (transform.position - lastPosition).magnitude / Time.deltaTime; // velocity since last Update
		lastPosition 	= transform.position;

		if (vel > 0.001f) 	idleTime = 0f; 				// if vel is slightly greater than 0, reset idle time
		else 				idleTime += Time.deltaTime; // else add to previous idle time
	}
}
