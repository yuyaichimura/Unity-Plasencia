using UnityEngine;
using System.Collections;
using System.Linq;

public class Timeline : MonoBehaviour {

	public Texture yearText;
	public Texture backgroundText;
	public Texture sliderText;
	public int textureHeight;
	public int margin;
	public int sliderHeight;
	public int sliderWidth;
	public int sliderLeft;

	public int yearMin;
	public int yearMax;
	public bool displayYear;
	public Font yearFont;
	public GUISkin noButtonSkin;
	public GUISkin timetravelButtonSkin;
	public GUIStyle sliderBackgroundStyle;
	public GUIStyle sliderThumbStyle;
	public float fadeSpeed;
	public float guiFadeSpeed;

	public int year;
	private int targetYear;
	public static int[] yearList = new int[7]{ 1416, 1422, 1428, 1431, 1442, 1455, 0 };
	private bool timetravel;
	private float alpha;
	private float angle;
	private float fadeTime = 0f;
	private float camZoomDefault;
	private bool showYearBook = false;

	private PauseMenu pausemenu;
	private GameObject cam;
	private minimap map;
	private MapCloseup mapCloseup;
	private MotionBlur motionblurFilter;
	private VortexEffect vortexFilter;
	private SceneFadeInOut fader;
	private GameObject player;
	private Weather weather;

	// Interactive narrative Objects
	private static int amount_of_objects = 11;

	private GameObject[] Obj = new GameObject[amount_of_objects];
	private InformationScreen[] YearBooks = new InformationScreen[6];

	
	void Awake () {
		year = 1416;
		targetYear = year;
		displayYear = true;
		timetravel = false;
		alpha = 1f;

		// get all game objects
		cam 				= GameObject.FindGameObjectWithTag (Tags.main_camera);
		pausemenu 			= cam.GetComponent<PauseMenu> ();
		fader 				= GameObject.FindGameObjectWithTag (Tags.fader).GetComponent<SceneFadeInOut> ();
		map 				= cam.GetComponent<minimap> ();
		mapCloseup 			= cam.GetComponent<MapCloseup> ();
		weather 			= cam.GetComponent<Weather> ();
		motionblurFilter 	= GameObject.FindGameObjectWithTag (Tags.main_camera).GetComponent<MotionBlur>();
		motionblurFilter.enabled = false;
		vortexFilter	 	= GameObject.FindGameObjectWithTag (Tags.main_camera).GetComponent<VortexEffect>();
		vortexFilter.enabled = false;

		//get all narrative objects
		Obj[0]  = GameObject.Find("Object Keys");
		Obj[1]  = GameObject.Find("Object Marketplace");
		Obj[2]  = GameObject.Find("Object Cask of Wine");
		Obj[3]  = GameObject.Find("Object Broad Sword");
		Obj[4]  = GameObject.Find("Object Palace Shield");
		Obj[5]  = GameObject.Find("Object Property Deed");
		Obj[6]  = GameObject.Find("Object Shackles");
		Obj[7]  = GameObject.Find("Object Multiple Barrels of Wine");
		Obj[8]  = GameObject.Find("Object Chalice");
		Obj[9]  = GameObject.Find("Object Book Stack");
		Obj[10] = GameObject.Find("Fountain");

		YearBooks[0] = GameObject.FindGameObjectWithTag("Year1416").GetComponent<InformationScreen>();
		YearBooks[1] = GameObject.FindGameObjectWithTag("Year1422").GetComponent<InformationScreen>();
		YearBooks[2] = GameObject.FindGameObjectWithTag("Year1428").GetComponent<InformationScreen>();
		YearBooks[3] = GameObject.FindGameObjectWithTag("Year1431").GetComponent<InformationScreen>();
		YearBooks[4] = GameObject.FindGameObjectWithTag("Year1442").GetComponent<InformationScreen>();
		YearBooks[5] = GameObject.FindGameObjectWithTag("Year1455").GetComponent<InformationScreen>();

		camZoomDefault = cam.GetComponent<Camera>().fieldOfView; 
	}

	void Start() {
		//this must be called after the awake method of the minimap class !! therefore use start()
		enableObjectsPerYear (year);
	}

	void OnGUI() {
		GUI.color = new Color(1,1,1,alpha);

		if (timetravel) 	TimetravelAnimation();
		if (displayYear) 	DisplayYear ();
		if (showYearBook)   showYearInfo (year);
	}

// -----------------------------------------
//			Display Timeline
// -----------------------------------------

	public void DisplayTimeline() {
		GUI.color = new Color(1,1,1,alpha);

		//draw background texture for the slider
		float sl = 0.06f * Screen.width;
		//float st = 0.1f * Screen.height;
		float sw = 0.88f * Screen.width;
		//float sh = 0.1f * Screen.height;

		GUI.DrawTexture (new Rect (margin, Screen.height - textureHeight - margin - 150, Screen.width - 2*margin, textureHeight), backgroundText);
		GUI.DrawTexture (new Rect (sl, Screen.height - textureHeight/2 - sliderHeight - 50, sw, 150), sliderText);

		//draw slider
		float l = 0.150f * Screen.width;
		//float t = 0.1f * Screen.height;
		float w = 0.7f * Screen.width;
		float h = 0.1f * Screen.height;

		targetYear = (int) GUI.HorizontalSlider(new Rect (l, Screen.height - textureHeight/2 - margin - 155, w, h), targetYear, yearMin, yearMax, sliderBackgroundStyle, sliderThumbStyle);
		targetYear = yearList.OrderBy(  x => Mathf.Abs( x - targetYear + 1)  ).First();	//sorting ony on a small array -> it's fine :)
	
				//draw button
		GUI.skin = timetravelButtonSkin;
		if (GUI.Button (new Rect ((Screen.width-200)/2 - 210, (Screen.height - 100)/2 - 100,200,80), "Travel through Time")) {		//Rect(top, left, width, height)
			// set everything ready for timetravel
			year 			= targetYear;
			timetravel 		= true;
			map.fadeMinimap = true;
			pausemenu.UnpauseTimeOnly();

			//get new weather
			weather.setSkyByYear(year);
			enableObjectsPerYear(year);
		}

		//draw back button
		if (GUI.Button(new Rect((Screen.width-200)/2 + 210, (Screen.height - 100)/2 - 100,200,80),"Back")) {
			pausemenu.UnPauseGame();
			pausemenu.currentPage = PauseMenu.Page.None;
		}

		//draw all-objects button
		if (GUI.Button(new Rect((Screen.width-200)/2, (Screen.height - 100)/2 - 100,200,80),"Visit all at the same time")) {
			// set everything ready for timetravel
			year 			= 0;
			timetravel 		= true;
			map.fadeMinimap = true;
			pausemenu.UnpauseTimeOnly();
			
			//get new weather
			weather.setRandomSky();
			enableObjectsPerYear(year);
		}
	}

// -----------------------------------------
//		Display Year in top right corner
// -----------------------------------------
	
	// display the year in the top right corner
	void DisplayYear() {
 		GUI.depth = 5;
		GUI.backgroundColor = Color.clear;
		GUI.contentColor = Color.black;
		GUI.skin.label.font = yearFont;
		GUI.skin.label.fontSize = 40;
		float additionalMargin = 0;

		string showYear;

		if (year != 0) showYear = year.ToString ();
		else {
			additionalMargin = -15;
			GUI.skin.label.fontSize = 34;
			showYear = "1390 - 1492";
		}

		GUI.DrawTexture (new Rect (Screen.width - 185, 0, 150, 140), yearText);
		GUI.Label (new Rect (Screen.width - 150, 45 + additionalMargin, 150, 140), showYear);

		if (pausemenu.currentPage == PauseMenu.Page.None) {
			GUI.skin = noButtonSkin;
			if (GUI.Button (new Rect (Screen.width - 185, 0, 150, 140), "")) {
				pausemenu.PauseGame ();
				pausemenu.currentPage = PauseMenu.Page.Timeline;
			}
		}
	}

// -----------------------------------------
//			Timetravel Animation
// -----------------------------------------

	void TimetravelAnimation(){
		//fade gui
		FadeToClear ();

		//move forward somehow
		ZoomCamera ();

		//increase  vortex effect angle more and more
		vortexFilter.enabled = true;
		FadeVortexAngleTo180 ();

		//fade to black
		fader.EndScene ();

		//keep time until scene is ended
		fadeTime = Mathf.Lerp (fadeTime, 1f, fadeSpeed * Time.deltaTime);
		if (fadeTime >= 0.95f) {
			timetravel = false;
			fader.sceneStarting = true;
			fadeTime = 0f;
			if(year != 0 ) showYearBook = true;

			//reset all image effects!!
			ResetCamZoom();
			ResetVortex();
			ResetFadeToClear();
			map.ResetFadeToClear();

			pausemenu.UnPauseGame();
			pausemenu.currentPage = PauseMenu.Page.None;
		}
	}

	void ZoomCamera() {
		float zoomSpeed = 0.2f;
		cam.GetComponent<Camera>().fieldOfView -= zoomSpeed;
	}

	void ResetCamZoom() {
		cam.GetComponent<Camera>().fieldOfView = camZoomDefault;
	}

	void FadeToClear() {
		alpha = Mathf.Lerp (alpha, 0f, guiFadeSpeed * Time.deltaTime);
	}

	void ResetFadeToClear(){
		alpha = 1f;
	}

	void FadeVortexAngleTo180()
	{
		float acceleration = 15;
		angle = angle + Time.deltaTime * acceleration * acceleration;
		vortexFilter.angle = angle;
	}

	void ResetVortex(){
		angle = 0;
		vortexFilter.enabled = false;
	}


// ---------------------------------------------------------
//			Books and Objects displayed per Year
// ---------------------------------------------------------

	public bool IsTimetravelling(){
		return timetravel;
	}

	void deactivateAllObjects() {
		for (int i = 0; i < amount_of_objects; i++) Obj[i].SetActive (false);
	}

	void showYearInfo(int time) {

		pausemenu.currentPage = PauseMenu.Page.Book;

		if (time == yearList[0]) YearBooks [0].OpenShortStory();		
		if (time == yearList[1]) YearBooks [1].OpenShortStory();		
		if (time == yearList[2]) YearBooks [2].OpenShortStory();		
		if (time == yearList[3]) YearBooks [3].OpenShortStory();		
		if (time == yearList[4]) YearBooks [4].OpenShortStory();		
		if (time == yearList[5]) YearBooks [5].OpenShortStory();

		showYearBook = false;

	}

	public void enableObjectsPerYear(int time) {
		deactivateAllObjects ();
		map.clearMarkerList ();
		mapCloseup.clearMarkerList();

		//always there:
		// ActivateObject (10);

		for( int i = 0; i < amount_of_objects; i++ ) 
			ActivateObject(i);

		/*
		//only visible in specific time periods:
		if (time == yearList[0]) {
			ActivateObject(0);
			ActivateObject(7);
		}

		if (time == yearList[1]) {
			ActivateObject(7);
			ActivateObject(8);
		}

		if (time == yearList[2]) {
			ActivateObject(1);
			ActivateObject(2);
			ActivateObject(7);
			ActivateObject(9);
		}

		if (time == yearList [3]) {
			ActivateObject(6);
			ActivateObject(7);
			ActivateObject(9);
		}

		if (time == yearList [4]) {
			ActivateObject(4);
			ActivateObject(5);
			ActivateObject(7);
			ActivateObject(9);
		}

		if (time == yearList [5]) {
			ActivateObject(3);
			ActivateObject(7);
		}

		//one world to show them all, one world to find them:
		if (time == yearList [6]) {
			for( int i = 0; i < amount_of_objects; i++ ) 
				ActivateObject(i);
		}
		*/
	}

	private void ActivateObject(int i){
		Obj[i].SetActive(true);
		Transform t = Obj[i].transform.Find("floating_menu_01/menu_center").transform;
		map.AddMarkerToMinimap( t.position.x, t.position.z);
	    mapCloseup.AddMarkerToCloseUpMap( t.position.x, t.position.z);
	}

}
