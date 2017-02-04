using UnityEngine;
using System.Collections;

public class SpacebarPauseMovement : MonoBehaviour {

	private bool isPaused;
	private float savedTimeScale;

	private MouseLook mouseLookComponent1;
	private MouseLook mouseLookComponent2;

	// Use this for initialization
	void Awake() {
		mouseLookComponent1 = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<MouseLook>();
		mouseLookComponent2 = GameObject.FindGameObjectWithTag (Tags.main_camera).GetComponent<MouseLook>();

		isPaused = false;	
		//Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate () {
		// menu opens when "esc" is pressed
		if (Input.GetKeyDown("space")) 
		{
			if(! isPaused) {
				StopAllMotion();
				isPaused = true;
			} else {
				ContinueMotion();
				isPaused = false;
			}
		}
	}

	public void StopAllMotion() {
		//savedTimeScale = Time.timeScale;	// save the time at which we pause
		//Time.timeScale = 0;				// no movement
		
		// disable any rotation of the camera
		mouseLookComponent1.enabled = false;
		mouseLookComponent2.enabled = false;
	}

	public void ContinueMotion(){
		//Time.timeScale = savedTimeScale;	// start game again at the time at which we stopped	
		
		// reenable camera rotation
		mouseLookComponent1.enabled = true;
		mouseLookComponent2.enabled = true;
	}

	/*Vector3 ClosestFloatingMenu() {
		GameObject[] menus = GameObject.FindGameObjectsWithTag (Tags.floating_menu);	//get floating menu Object
		float minDist = -1;
		Vector3 position;

		//for (int i = 0; i < menus.Length(); i++) {
			/*if( menus[i].GetComponent<FloatingMenu>().IsInsideMenuArea() ){
				if( minDist == -1 ) {
					minDist 	= menus[i].GetComponent<FloatingMenu>().DistanceToPlayer();
					position 	= menus[i].
				} else {
					minDist = minDist( minDist, menus[i].GetComponent<FloatingMenu>().DistanceToPlayer() );
				}
			}*/
		//}

	//}
}
