using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
	public GUISkin skinContinue2;

	public Texture startmenuText;
	public Texture helpmenuText;
	public Texture keyArrows;
	public Texture keyEscText;
	public Texture keyTText;
	public Texture keyMText;
	public Texture keySpaceText;
	public Texture keyFText;
	public Texture keyHText;
	public Texture keyRText;
	public Texture keyCText;
	public Texture keyYText;
	public Texture2D closeX;

	public Font font;
	public TextAsset welcomeString;
	public TextAsset keyArrowsString;
	public TextAsset keyEscString;
	public TextAsset keyMString;
	public TextAsset keyTString;
	public TextAsset keySpaceString;

	private GUISkin defaultSkin;
	
	public enum Page {
		None,
		Start,
		Main,
		Options,
		Controls,
		Credits,
		HelpPage,
		Map, 
		Timeline,
		Book
	}
	public Page currentPage;

	private float 		savedTimeScale;
	private int 		toolbarInt = 0;
	private string[]  	toolbarstrings =  {"Audio","Graphics", "Mouse Speed"};
	private bool 		showHelp = false;
	private int 		scrWebWidth;
	private int 		scrWebHeight;
	private bool		fullscreen = false;
	private bool 		running = false;
	private bool 		flying = false;
	private float 		mouseSensitivity;

	private SepiaToneEffect 	pauseFilter1;
	private BlurEffect 			pauseFilter2;

	private GameObject cam;
	private MapCloseup mapCloseup;
	private minimap map;
	private Timeline timeline;
	private MouseLook mouseLookComponent1;
	private MouseLook mouseLookComponent2;
	private CharacterMotor characterMotor;
	private FPSInputController inputController;
	private SceneFadeInOut fader;
	private SpacebarPauseMovement camMotion;
	private InformationScreen introductionNarrative;
	private InformationScreen creditsBook;
	private FreeflightCam freeflightCam;

	

	// ---------- predefined functions ---------------

	void Awake (){
		//get game objects
		cam 		= GameObject.FindGameObjectWithTag (Tags.main_camera);
		mapCloseup 	= cam.GetComponent<MapCloseup> ();
		timeline    = cam.GetComponent<Timeline> ();
		map 		= cam.GetComponent<minimap> ();
		fader 		= GameObject.FindGameObjectWithTag (Tags.fader).GetComponent<SceneFadeInOut> ();
		camMotion   = cam.GetComponent<SpacebarPauseMovement> ();
		introductionNarrative = cam.GetComponent<InformationScreen> ();
		freeflightCam = cam.GetComponent<FreeflightCam> ();

		mouseLookComponent1 = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<MouseLook>();
		mouseLookComponent2 = cam.GetComponent<MouseLook>();
		characterMotor = GameObject.FindGameObjectWithTag (Tags.player).GetComponent<CharacterMotor> ();
		inputController = GameObject.FindGameObjectWithTag (Tags.player).GetComponent<FPSInputController> ();

		pauseFilter1 = cam.GetComponent<SepiaToneEffect>();
		pauseFilter1.enabled = false;
		pauseFilter2 = cam.GetComponent<BlurEffect> ();
		pauseFilter2.enabled = false;

		Time.timeScale = 1;
		currentPage = Page.Start;
		PauseGame ();

		scrWebWidth = Screen.width;
		scrWebHeight = Screen.height;

		creditsBook = GameObject.Find("Credits").GetComponent<InformationScreen>();

		mouseSensitivity = mouseLookComponent1.sensitivityX;
	}

	//--------------------------------------------
	//		Input Handling
	//--------------------------------------------
	void LateUpdate () {

		// --- help ---
		if (Input.GetKeyDown ("h")) 
			showHelp = !showHelp;

		// --- fullscreen ---
		if (Input.GetKeyDown ("f")) {
			if( fullscreen ) {
				Screen.SetResolution (scrWebWidth, scrWebHeight, false);
				//timeline.enableObjectsPerYear(timeline.year);
			}
			else {
				Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
				//Screen.fullScreen = !Screen.fullScreen;
				//Screen.SetResolution (Screen.currentResolution.width, Screen.currentResolution.height, true);
				//timeline.enableObjectsPerYear(timeline.year);
			}

			fullscreen = !fullscreen;
		}

		// --- run ---
		if (Input.GetKeyDown ("r")) {
			// if( running ) characterMotor.movement.maxForwardSpeed = 1.5f;
			// else characterMotor.movement.maxForwardSpeed = 3.0f;			
			running = !running;
		}

		// --- fly ---
		if (Input.GetKeyDown ("y")) {
			if( flying ){
				characterMotor.enabled = true;
				inputController.enabled = true;
				freeflightCam.enabled = false;
				//inputController.flight = true;
			}
			else {
				characterMotor.enabled = false;
				inputController.enabled = false;
				freeflightCam.enabled = true;
				//inputController.flight = false;
			}		
			flying = !flying;
		}


		// --- cursor ---
		if (Input.GetKeyDown ("c")) {
			Cursor.visible = !Cursor.visible;
		}

		// --- menu ---
		switch (currentPage) 
		{
		case Page.None: 
			if (Input.GetKeyDown("return")) {
				PauseGame(); 
				currentPage = Page.Main;	
			}
			if (Input.GetKeyDown("m")) {
				currentPage 	= Page.Map;		// open main page of menu
				map.showMinimap = false; 	// close minimap
			}
			if (Input.GetKeyDown("t")) {
				PauseGame();
				currentPage = Page.Timeline;
			}
			break;

		case Page.Start:
			break;

		case Page.Main: 
			if (Input.GetKeyDown("return")) {
				UnPauseGame(); 
				currentPage = Page.None;
			}
			if (Input.GetKeyDown("m")) {
				currentPage 	= Page.Map;		// open main page of menu
				map.showMinimap = false; 	// close minimap
			}
			break;	

		case Page.Map:
			if (Input.GetKeyDown("m")) {currentPage = Page.None;
				map.showMinimap = true; // open minimap!
			}
			break;
		
		case Page.Timeline:
			if (Input.GetKeyDown("t")) {
				UnPauseGame();
				currentPage = Page.None;
			}
			break;

		default: break;
		}
	}

	// -------------------- display pages handling -----------------
	void OnGUI () {
		if(defaultSkin == null) {
			defaultSkin = GUI.skin;
		}

		switch (currentPage) {
			case Page.Start: 	StartMenu(); break;
			case Page.Main: 	MainPauseMenu(); break;
			case Page.Options: 	SettingsMenu(); ShowBackButton(Page.Main); break;
			case Page.Map:		mapCloseup.DisplayMap();  break;
			case Page.Timeline: timeline.DisplayTimeline(); break;
			case Page.Book: 	/*ShowCloseButton(Page.None);*/ break;
			case Page.Credits: 	CreditsMenu(); break;
		} 

		if (showHelp) 	HelpPage ();
		//else 			HelpPageHint (); 
	}

	//--------------------------------------------
	//		Pause / Unpause
	//--------------------------------------------

	public void PauseGame() {
		savedTimeScale = Time.timeScale;	// save the time at which we pause
		Time.timeScale = 0;				// no movement
		AudioListener.pause = true;		// no listening to 3D sources
		pauseFilter1.enabled = true;
		pauseFilter2.enabled = true;
		
		// disable any rotation of the camera
		mouseLookComponent1.enabled = false;
		mouseLookComponent2.enabled = false;
	}
	
	public void UnPauseGame() {
		Time.timeScale = savedTimeScale;	// start game again at the time at which we stopped
		AudioListener.pause = false;		// start listening to audio again		
		pauseFilter1.enabled = false;
		pauseFilter2.enabled = false;
		
		// reenable camera rotation
		mouseLookComponent1.enabled = true;
		mouseLookComponent2.enabled = true;
	}

	public void PauseTimeOnly() {
		savedTimeScale = Time.timeScale;	// save the time at which we pause
		Time.timeScale = 0;				// no movement
	}

	public void UnpauseTimeOnly() {
		Time.timeScale = savedTimeScale;
	}
	
	public bool IsGamePaused() {
		return (Time.timeScale == 0);
	}

	//--------------------------------------------
	//		Begin, End Menu / Back Buttons
	//--------------------------------------------

	void BeginPage(int width, int height) {
		GUILayout.BeginArea( new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));
	}
	
	void EndPage() {
		GUILayout.EndArea();
		//if (currentPage != Page.Main) ShowBackButton();
	}

	void ShowBackButton( Page page ) {
		if (GUI.Button(new Rect(Screen.width/2 - 50, Screen.height/2 + 40, 100, 50),"Back")) {
			currentPage = page;
		}
	}

	//--------------------------------------------
	//		Start Menu
	//--------------------------------------------
	void StartMenu() {
		timeline.displayYear = false;
		map.showMinimap = false;

		GUI.contentColor = Color.black;
		GUI.skin.label.font = font;
		GUI.skin.label.fontStyle = FontStyle.Normal;
		
		// background book
		int textureHeight = 541; 
		int textureWidth = 750;
		float top = (Screen.height - textureHeight) / 2f;
		float left = (Screen.width - textureWidth) / 2f;
		GUI.DrawTexture (new Rect (left, top, textureWidth, textureHeight), startmenuText);
		
		BeginPage(textureWidth,textureHeight);
		
		
		//display welcome text and description on the left
		GUI.skin = skinContinue2;
		if (GUI.Button (new Rect (200,200,80,80), "")) {
			currentPage = Page.Book;
			fader.sceneStarting = true;
			timeline.displayYear = true;
			map.showMinimap = true;
			UnPauseGame();
			
			introductionNarrative.OpenShortStory();
		}
		GUI.skin.label.fontSize = 24;
		GUI.Label( new Rect (215,280,150,300), "Play!");

		//welcome introduction on the right
		GUI.skin = defaultSkin;
		GUI.skin.label.fontSize = 14;
		GUI.Label( new Rect (400,70,250,400), welcomeString.ToString ());
		
		//display buttons on the right 
		/*GUI.DrawTexture (new Rect (380, 50, 120, 60), keyArrows);
		GUI.DrawTexture (new Rect (400, 120, 75, 75), keyEscText);
		GUI.DrawTexture (new Rect (400, 190, 75, 75), keyMText);
		GUI.DrawTexture (new Rect (400, 260, 75, 75), keyTText);
		GUI.DrawTexture (new Rect (400, 350, 200, 75), keySpaceText);
		
		GUI.skin.label.fontSize = 14;
		GUI.Label( new Rect (510,40, 140,80), keyArrowsString.ToString ());
		GUI.Label( new Rect (490,130, 140,80), keyEscString.ToString ());
		GUI.Label( new Rect (490,200,140,80), keyMString.ToString ());
		GUI.Label( new Rect (490,270,140,80), keyTString.ToString ());
		GUI.Label( new Rect (410,420,220,80), keySpaceString.ToString ());*/
		
		EndPage ();
	}

	//--------------------------------------------
	//		Main Menu
	//--------------------------------------------
	void MainPauseMenu() {

		BeginPage(320,80);
		if (GUI.Button (new Rect (0,0,80,80), "Continue")) {		//Rect(top, left, width, height)
			UnPauseGame();
			currentPage = Page.None;
		}

		if (GUI.Button (new Rect (80,0,80,80), "Restart")) {
			Application.LoadLevel(0);
		}

		/*if (GUI.Button (new Rect (160,0,80,80), "Map")) {
			openMainMenuOnExit 	= true;
			currentPage 		= Page.Map;		// open main page of menu
			map.showMinimap 	= false; 	// close minimap
		}*/

		if (GUI.Button (new Rect (160,0,80,80), "Settings")) {
			currentPage = Page.Options;
		}

		/*if (GUI.Button (new Rect (160,0,80,80), "Help")) {
			showHelp = true;
		}*/

		if (GUI.Button (new Rect (240,0,80,80), "Credits")) {
			currentPage = Page.Credits;
		}

		/*if (GUI.Button (new Rect (320,0,80,80), "Exit")) {
			Application.Quit();
		}*/

		EndPage();
	}

	//--------------------------------------------
	//		Help Page
	//--------------------------------------------
	void HelpPage() {

		// background scroll
		int textureHeight = 160; 
		int textureWidth = 800;
		float top = (Screen.height - textureHeight);
		float left = (Screen.width - textureWidth) / 2f;
		GUI.DrawTexture (new Rect (left, top, textureWidth, textureHeight), helpmenuText);
				
		//display welcome text and description on the left
		GUI.skin = skinContinue2; // TODO : why do I need this skin here - try on another computer!
		if (GUI.Button (new Rect (left+textureWidth-50,top + 20,30,30),closeX,GUIStyle.none)) {
			showHelp = false;
		}	
		
		//display key icons and their labels
		GUI.skin = defaultSkin;
		GUI.contentColor = Color.black;
		GUI.skin.label.font = font;
		GUI.skin.label.fontSize = 12;

		GUI.DrawTexture (new Rect (left + 20, top + 45, 120, 60), keyArrows);
		GUI.DrawTexture (new Rect (left + 145, top + 48, 65, 65), keyEscText);
		GUI.DrawTexture (new Rect (left + 210, top + 48, 65, 65), keyMText);
		GUI.DrawTexture (new Rect (left + 275, top + 48, 65, 65), keyTText);
		GUI.DrawTexture (new Rect (left + 340, top + 48, 185, 65), keySpaceText);
		GUI.DrawTexture (new Rect (left + 525, top + 48, 65, 65), keyFText);
		GUI.DrawTexture (new Rect (left + 590, top + 48, 65, 65), keyRText);
		GUI.DrawTexture (new Rect (left + 655, top + 48, 65, 65), keyCText);
		GUI.DrawTexture (new Rect (left + 720, top + 48, 65, 65), keyYText);

		GUI.Label( new Rect (left + 65,  top + 110, 140,80), "Walk");
		GUI.Label( new Rect (left + 145, top + 110, 140,80), "Main Menu");
		GUI.Label( new Rect (left + 230, top + 110,140,80),  "Map");
		GUI.Label( new Rect (left + 277, top + 110,140,80),  "Timetravel");
		GUI.Label( new Rect (left + 370, top + 110,220,80),  "Toggle the Camera");
		GUI.Label( new Rect (left + 530, top + 110,140,80),  "Fullscreen");
		GUI.Label( new Rect (left + 608, top + 110,140,80),  "Run");
		GUI.Label( new Rect (left + 650, top + 110,140,80),  "Toggle Cursor");
		GUI.Label( new Rect (left + 745, top + 110,140,80),  "Fly");
	}

	void HelpPageHint() {
		GUI.depth = 5;

		// background scroll
		int textureHeight = 65; 
		int textureWidth = 170;
		float top = (Screen.height - textureHeight);
		GUI.DrawTexture (new Rect (30, top, textureWidth, textureHeight), helpmenuText);

		//h-key
		GUI.DrawTexture (new Rect (40, top + 5, 60, 60), keyHText);

		//label
		GUI.skin.label.fontSize = 14;
		GUI.contentColor = Color.black;
		GUI.Label( new Rect (110,  top + 25, 140,80), "Help Menu",GUIStyle.none);

		//make it also clickable!!
		if (GUI.Button (new Rect (30, top, textureWidth, textureHeight), "", GUIStyle.none)) {
			showHelp = true;
		}
	}

	//--------------------------------------------
	//		Credits
	//--------------------------------------------
	void CreditsMenu() {
		creditsBook.setReturnPage(Page.Main);
		creditsBook.OpenShortStory();

	}

	//--------------------------------------------
	//		Settings Menu
	//--------------------------------------------
	void SettingsMenu() {
		GUI.contentColor = Color.black;
		GUI.skin.label.font = font;
		GUI.skin.label.fontSize = 14;

		BeginPage(400,300);
		toolbarInt = GUILayout.Toolbar (toolbarInt, toolbarstrings);
		switch (toolbarInt) {
			case 0: VolumeControl(); break;
			case 1: Qualities(); QualityControl(); break;
			case 2: MouseSpeedControl(); break;
		}
		EndPage();
	}

	// ------------------------------------------

	// Opened when choosing Volume Icon
	void VolumeControl() {
		GUILayout.Label("Volume");
		AudioListener.volume = GUILayout.HorizontalSlider(AudioListener.volume, 0, 1);
	}

	void MouseSpeedControl() {
		GUILayout.Label("Mouse Speed");
		mouseSensitivity = GUILayout.HorizontalSlider(mouseSensitivity, 0.5f, 7.0f);
		GUILayout.Label(mouseSensitivity.ToString("F2"));

		mouseLookComponent1.sensitivityX = mouseSensitivity;
		mouseLookComponent2.sensitivityY = mouseSensitivity;
	}

	void QualityControl() {
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Decrease")) {
			QualitySettings.DecreaseLevel();
		}
		if (GUILayout.Button("Increase")) {
			QualitySettings.IncreaseLevel();
		}
		GUILayout.EndHorizontal();
	}

	void Qualities() {
		GUILayout.Label(QualitySettings.names[ QualitySettings.GetQualityLevel() ]);
	}

	void OnApplicationPause(bool pause) {
		if (IsGamePaused()) {
			AudioListener.pause = true;
		}
	}
}