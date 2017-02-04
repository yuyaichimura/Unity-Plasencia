using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapCloseup : MonoBehaviour {
	
	public Texture mapText;
	public Texture markerText;
	public Texture blueMarkerText;
	public Texture closeX;
	public float mapHeight;
	public float mapWidth;
	public float map_original_height;
	public float map_original_width;
	public float margin;

	private PauseMenu pausemenu;
	private GameObject cam;
	//public  bool openMainMenuOnExit;
	private float posX;
	private float posY;

	//A List of Marker Positions
	private List<float[]> markerPosOnCloseUpMap = new List<float[]>();

	private minimap map;
	
	void Awake (){
		cam 		= GameObject.FindGameObjectWithTag (Tags.main_camera);
		pausemenu 	= GameObject.FindGameObjectWithTag(Tags.main_camera).GetComponent<PauseMenu>();
		map 		= GameObject.FindGameObjectWithTag (Tags.main_camera).GetComponent<minimap> ();
	}

	public bool IsMapOpen(){
		return (pausemenu.currentPage == PauseMenu.Page.Map);
	}


	public void DisplayMap() {
		float[] markerPos = map.PositionToMapCoordinates (transform.position.x, transform.position.z, mapWidth, mapHeight);
		markerPos 		  = PositionOnCloseUpMap (markerPos[0], markerPos[1], markerText);

		// calculate size of the big map on screen
		float scalingFactor = ScalingFactor ();
		float top = (Screen.height - mapHeight * scalingFactor) / 2;
		float left = (Screen.width - mapWidth * scalingFactor) / 2;
		GUI.DrawTexture(new Rect( left, top, mapWidth * scalingFactor, mapHeight * scalingFactor), mapText);

		//draw legend
		GUI.contentColor = Color.black;
		GUI.skin.label.font = (Font) Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
		GUI.skin.label.fontSize = 12;

		GUI.DrawTexture(new Rect( left + 120* scalingFactor, top + 80 * scalingFactor, markerText.width, markerText.height), markerText);
		GUI.Label(      new Rect (left + 170* scalingFactor, top + 90 * scalingFactor, 140,80), "Your Position");
		GUI.DrawTexture(new Rect( left + 130* scalingFactor, top + 140* scalingFactor, blueMarkerText.width, blueMarkerText.height), blueMarkerText);
		GUI.Label(      new Rect (left + 170* scalingFactor, top + 130* scalingFactor, 140,80), "Objects which tell a Story");


		//draw all additional markers
		for( int i=0; i < markerPosOnCloseUpMap.Count; i++ ){
			float[] xy = PositionOnCloseUpMap(markerPosOnCloseUpMap[i][0], markerPosOnCloseUpMap[i][1], blueMarkerText);
			GUI.DrawTexture (new Rect (xy[0], xy[1], blueMarkerText.width, blueMarkerText.height), blueMarkerText);
		}

		// Show Close Button
		if (GUI.Button(new Rect(left + mapWidth*0.85f, top + mapHeight*0.08f , 40, 40),closeX,GUIStyle.none)) {
			pausemenu.currentPage = PauseMenu.Page.None;
			map.showMinimap = true; // open minimap!
		}

		// calculate view direction
		Vector2 markerPosVector 	= new Vector2 (markerPos [0] + markerText.width/2f, markerPos [1] + markerText.height/2f);
		Vector2 camViewPlanar 		= new Vector2 (cam.transform.forward.x, cam.transform.forward.z);
		camViewPlanar.Normalize ();
		float angle = Mathf.Acos ( camViewPlanar.y ) * 180f / Mathf.PI;
		if (camViewPlanar.x < 0) angle = - angle;
		angle += 180f;

		GUIUtility.RotateAroundPivot (angle, markerPosVector);
		GUI.DrawTexture (new Rect (markerPos[0], markerPos[1], markerText.width, markerText.height), markerText);
		GUIUtility.RotateAroundPivot (-angle, markerPosVector);
	}


	public float[] PositionOnCloseUpMap(float x, float z, Texture tex) {
		//draw position on map
		//float[] coordinates = map.PositionToMapCoordinates (posX, posZ, mapWidth, mapHeight);
		float[] coordinates = new float[2];
		coordinates[0] = x;
		coordinates[1] = z;
		
		//scale, subtract marker width, add offset left/top
		float scalingFactor = ScalingFactor ();
		float top = (Screen.height - mapHeight * scalingFactor) / 2f;
		float left = (Screen.width - mapWidth * scalingFactor) / 2f;
		coordinates [0] = left + (coordinates [0] * scalingFactor) - tex.width  / 2f;
		coordinates[1]  = top  + (coordinates[1] * scalingFactor)  - tex.height / 2f;
 
		return coordinates;
	}

	public void AddMarkerToCloseUpMap(float posX, float posZ){
		//float[] pos = PositionOnCloseUpMap (posX, posZ);
		float[] pos = map.PositionToMapCoordinates (posX, posZ, mapWidth, mapHeight);
		markerPosOnCloseUpMap.Add (pos);
	}

	private float ScalingFactor() {
		float scalingFactor = 1f;
		if ( mapWidth + 2f*margin > Screen.width ) {	
			scalingFactor = ((float) Screen.width ) / ((float) mapWidth  + 2f*margin );		
		}
		if ( mapHeight + 2f*margin > Screen.height ) {
			float q = ((float) Screen.height) / ((float) mapHeight + 2f*margin );
			scalingFactor = Mathf.Min( scalingFactor, q );
		}
		return scalingFactor;
	}

	public void clearMarkerList(){
		markerPosOnCloseUpMap.Clear ();
	}
}
