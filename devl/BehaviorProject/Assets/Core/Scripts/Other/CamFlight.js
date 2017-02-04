#pragma strict

var flySpeed:float = 0.5;
var defaultCam : GameObject;
var playerObject : GameObject;
var isEnabled : boolean;
var shift : boolean;
var ctrl : boolean;
var accelerationAmount : float = 3;
var accelerationRatio : float = 1;
var slowDownRatio : float = 0.5;
var character;

function Awaken()
{
	//switchCamera();
	character = GameObject.FindGameObjectWithTag("Player");
}


function Update()
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