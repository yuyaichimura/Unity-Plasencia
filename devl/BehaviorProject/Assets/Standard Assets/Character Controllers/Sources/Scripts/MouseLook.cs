using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float speed = 5F;

	void Update ()
	{


        if (Input.GetKey(KeyCode.W)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                transform.position += transform.forward * Time.deltaTime * speed;
            }
            else {
                Vector3 forth = transform.forward;
                forth.y = 0f;
                transform.position += forth * Time.deltaTime * speed;
            }
        }
        if (Input.GetKey(KeyCode.S)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                transform.position -= transform.forward * Time.deltaTime * speed;
            }
            else {
                Vector3 forth = transform.forward;
                forth.y = 0f;
                transform.position -= forth * Time.deltaTime * speed;
            }
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.position += transform.right * Time.deltaTime * speed;
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.position -= transform.right * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.Q)) {
            transform.eulerAngles +=  new Vector3(0, -3f, 0);
        }
        else if (Input.GetKey(KeyCode.E)) {
            transform.eulerAngles += new Vector3(0, 3f, 0);
        }
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		/*
        if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true; */
	}
}