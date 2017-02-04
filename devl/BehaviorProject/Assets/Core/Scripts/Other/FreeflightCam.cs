using UnityEngine;
using System.Collections;

public class FreeflightCam : MonoBehaviour
{
	public float flySpeed = 0.5f;
	public GameObject defaultCam;
	public GameObject playerObject;
	public bool isEnabled;
	private bool shift;
	private bool ctrl;
	private float accelerationAmount = 3f;
	private float accelerationRatio = 1f;
	private float slowDownRatio = 0.5f;
	private GameObject character;

	void Awake()
	{
		//switchCamera();
		character = GameObject.FindGameObjectWithTag("Player");
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
		{
			shift = true;
			flySpeed *= accelerationRatio;
		}
		
		if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
		{
			shift = false;
			flySpeed /= accelerationRatio;
		}
		if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
		{
			ctrl = true;
			flySpeed *= slowDownRatio;
		}
		if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
		{
			ctrl = false;
			flySpeed /= slowDownRatio;
		}
		if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
		{
			var directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			
			if (directionVector != Vector3.zero) {
				// Get the length of the directon vector and then normalize it
				// Dividing by the length is cheaper than normalizing when we already have the length anyway
				var directionLength = directionVector.magnitude;
				directionVector = directionVector / directionLength;
				
				// Make sure the length is no bigger than 1
				directionLength = Mathf.Min(1, directionLength);
				
				// Make the input vector more sensitive towards the extremes and less sensitive in the middle
				// This makes it easier to control slow speeds when using analog sticks
				directionLength = directionLength * directionLength;
				
				// Multiply the normalized direction vector by the modified length
				directionVector = directionVector * directionLength;
			}
			
			playerObject.transform.Translate( flySpeed * ( defaultCam.transform.localRotation * directionVector) );
		}
		if (Input.GetKey(KeyCode.E))
		{
			playerObject.transform.Translate(playerObject.transform.up * flySpeed*0.5f);
		}
		else if (Input.GetKey(KeyCode.Q))
		{
			playerObject.transform.Translate(-playerObject.transform.up * flySpeed*0.5f);
		}
	}
}
