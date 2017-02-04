using UnityEngine;
using System.Collections;

public class FloatingMenu : MonoBehaviour {

	public Texture texture;
	public Texture previewTexture;
	public TextAsset contentText;
	public string headline;
	public GUIStyle contentSkin;
	public GUIStyle headlineSkin;
	public float idleTimeToOpenWindow;
	public float fadeSpeed;
	public bool withButtons;

	public static bool showFloatingMenus;

	public GUISkin skinButton1;
	//public GUISkin skinButton2;
	//public GUISkin skinButton3;
	//public GUISkin skinButton4;
	//public GUISkin skinButton5;

	private Vector3 menuWorldPosition;
	private float menuX;
	private float menuY;
	public float menuWidth;
	public float menuHeight;

	private bool fadingIn;
	private bool fadingOut;
	private float alpha;
	private float lerpTime;

	private int buttonSizeX;
	private int buttonSizeY;
	private int buttonOffsetX;
	private int buttonOffsetY;

	private GameObject cam;
	private PauseMenu pausemenu;
	private minimap map;
	private MapCloseup mapCloseup;
	private GameObject player;
	private Timeline timeline;
	private MovementTracking movementTracking;
	private InformationScreen thisInfoScreen;

	//private float nativeWidth = 1920f;
	//private float nativeHeight = 1080f;	

	void Awake () {
		// Game Objects
		cam 		= GameObject.FindGameObjectWithTag (Tags.main_camera);
		pausemenu 	= cam.GetComponent<PauseMenu>();
		map 		= cam.GetComponent<minimap> ();
		mapCloseup 	= cam.GetComponent<MapCloseup> ();
		player 		= GameObject.FindGameObjectWithTag (Tags.player);
		timeline    = cam.GetComponent<Timeline> ();
		movementTracking = GameObject.FindGameObjectWithTag (Tags.player).GetComponent<MovementTracking> ();
		menuWorldPosition = transform.FindChild("menu_center").transform.position; //Position of the menu in the world
		thisInfoScreen = GetComponent<InformationScreen>();

		// Fading Parameters
		fadingIn = true;
		fadingOut = false;
		alpha = 0;
		lerpTime = 0;

		//Button parameters
		buttonSizeX = 120;
		buttonSizeY = 30;
		buttonOffsetY = 70;

		showFloatingMenus = true;
	}

	void Update () {		
		// get some info about other gui elements
		bool mapOpen = mapCloseup.IsMapOpen ();
		bool gamePaused = pausemenu.IsGamePaused ();
		bool timetravelling = timeline.IsTimetravelling ();

		showFloatingMenus = !mapOpen && !gamePaused && !timetravelling;
	}

	void OnGUI () {

		//draw stuff, check first if nothing else is open
		if( showFloatingMenus ) {
			if (IsInsideMenuArea ()) {
				if (fadingIn)  StartFadingIn ();	// controls the transparency of the drawn elements
				DisplayMenu ();
			} else {
				if(fadingOut)  {
					StartFadingOut();
					DisplayMenu ();
				}
			} 
		}
	}

	public float DistanceToPlayer() {
		return (transform.position - player.transform.position).magnitude;
	}

	public bool IsInsideMenuArea() {
		return ( DistanceToPlayer() < GetComponent<SphereCollider>().radius );
	}

	// --------------- display functions ------------------

	void DisplayMenu() {
		menuX = -10000;		// initialize like this to ensure it does not appear on screen if it's not in set later on
		menuY = -10000;
		GUI.depth = 10;
		GUI.contentColor = Color.black;
		GUI.skin.label.font = (Font) Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
		GUI.skin.label.fontSize = 14;

		//calculate position of the menu
		Vector3 menuScreenPos = cam.GetComponent<Camera>().WorldToScreenPoint (menuWorldPosition);
		Vector3 heading = menuWorldPosition - cam.GetComponent<Camera>().transform.position;		

		// test if the object is in front of the camera!
		if (Vector3.Dot(cam.GetComponent<Camera>().transform.forward , heading) > 0) { 
			menuX = menuScreenPos.x - menuWidth/2f;
			menuY = (Screen.height - menuScreenPos.y);
			/*if (menuY < 0) menuY = 0; 
			if (menuX < 0) menuX = 0;
			if (menuX > Screen.width - menuWidth) menuX = Screen.width - menuWidth;*/
		} 

		//calculate time spend standing near the menu -> open it eventually -> show menu
		if ( !pausemenu.IsGamePaused ()) {
			if (movementTracking.idleTime >= idleTimeToOpenWindow) {
				//draw background texture
				GUI.DrawTexture (new Rect (menuX, menuY, menuWidth, menuHeight), texture);
				//draw text
				GUI.Box (new Rect (menuX, menuY, menuWidth, menuHeight), contentText.ToString (), contentSkin);
				//draw buttons
				if(withButtons) drawButtons();
				//draw headline
				GUI.Box (new Rect (menuX, menuY, menuWidth, 80), headline, headlineSkin);
			} else {
				//draw background texture
				GUI.DrawTexture (new Rect (menuX, menuY, menuWidth, 80), previewTexture);
				//draw headline
				GUI.Box (new Rect (menuX, menuY, menuWidth, 80), headline, headlineSkin);
			}
		}
	}

	void drawButtons(){
		int amount_of_buttons = 1;

		buttonOffsetX = (int) Mathf.Floor ( (menuWidth - amount_of_buttons * buttonSizeX) / 2f );

		GUILayout.BeginArea(new Rect(menuX, menuY, menuWidth, menuHeight));

		GUI.skin = skinButton1;
		if (GUI.Button (new Rect (buttonOffsetX ,  				buttonOffsetY ,buttonSizeX,buttonSizeY), "")) {		//Rect(top, left, width, height)
			thisInfoScreen.OpenShortStory();
		}

		/*
		GUI.skin = skinButton2;
		if (GUI.Button (new Rect (buttonOffsetX + buttonSizeX,   buttonOffsetY ,buttonSizeX,buttonSizeY), "")) {
			pausemenu.currentPage = PauseMenu.Page.Info2;
		}
		
		GUI.skin = skinButton3;
		if (GUI.Button (new Rect (buttonOffsetX + 2*buttonSizeX, buttonOffsetY ,buttonSizeX,buttonSizeY), "")) {
			pausemenu.currentPage = PauseMenu.Page.Info3;
		}

		GUI.skin = skinButton4;
		if (GUI.Button (new Rect (buttonOffsetX + 3*buttonSizeX, buttonOffsetY ,buttonSizeX,buttonSizeY), "")) {
			pausemenu.currentPage = PauseMenu.Page.Info4;
		}

		GUI.skin = skinButton5;
		if (GUI.Button (new Rect (buttonOffsetX + 4*buttonSizeX, buttonOffsetY ,buttonSizeX,buttonSizeY), "")) {
			pausemenu.currentPage = PauseMenu.Page.Info5;
		}
		*/

		GUILayout.EndArea();
	}

	// -------------- fading in and out functions --------------------

	void FadeToMenu()
	{
		lerpTime += fadeSpeed * Time.deltaTime;
		alpha = Mathf.Lerp (alpha, 1f, lerpTime);
		GUI.color = new Color(1,1,1,alpha);
	}

	void FadeToClear()
	{
		lerpTime += fadeSpeed * Time.deltaTime;
		alpha = Mathf.Lerp (alpha, 0, lerpTime);
		GUI.color = new Color(1,1,1,alpha);
	}

	void StartFadingIn()
	{
		FadeToMenu();
		
		if (GUI.color.a >= 0.95f) {
			GUI.color = new Color(1,1,1,1);
			fadingIn = false;
			fadingOut = true;
			lerpTime = 0;
		}
	}

	void StartFadingOut()
	{
		FadeToClear();
		
		if (GUI.color.a <= 0.05f) {
			GUI.color = new Color(1,1,1,0);
			fadingIn = true;
			fadingOut = false;
			lerpTime = 0;
		}
	}
}
