private var motor : CharacterMotor;

// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
}

// Update is called once per frame
function Update () {
	// Get the input vector from keyboard or analog stick
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
	
	// Apply the direction to the CharacterMotor
	motor.inputMoveDirection = transform.rotation * directionVector;
	motor.inputJump = Input.GetButton("Jump");
}

// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")



/*
private var motor : CharacterMotor;

var flySpeed:float = 0.5;
var defaultCam : GameObject;
private var shift : boolean;
private var ctrl : boolean;
var accelerationAmount : float = 3;
var accelerationRatio : float = 1;
var slowDownRatio : float = 0.5;
var flight : boolean = false;
private var finalDirectionVector : Vector3 = Vector3.zero;

// Use this for initialization
function Awake () {
	motor = GetComponent(CharacterMotor);
}

// Update is called once per frame
function Update () {
	finalDirectionVector = Vector3.zero;

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
		
		if (flight)
			//finalDirectionVector +=  transform.rotation * (flySpeed * ( defaultCam.transform.localRotation * directionVector) );
			transform.Translate( flySpeed * ( defaultCam.transform.localRotation * directionVector) );
			
		else
			finalDirectionVector += transform.rotation * directionVector;
	}
	if (flight && Input.GetKey(KeyCode.E))
	{
		//finalDirectionVector += transform.up * flySpeed*0.5f;
		transform.Translate(transform.up * flySpeed*0.5f);
	}
	else if (flight && Input.GetKey(KeyCode.Q))
	{
		//finalDirectionVector += -transform.up * flySpeed*0.5f;
		transform.Translate(-transform.up * flySpeed*0.5f);
	}
	
	// Apply the direction to the CharacterMotor
	motor.inputMoveDirection = finalDirectionVector;
	motor.inputJump = Input.GetButton("Jump");
}

// Require a character controller to be attached to the same game object
@script RequireComponent (CharacterMotor)
@script AddComponentMenu ("Character/FPS Input Controller")*/
