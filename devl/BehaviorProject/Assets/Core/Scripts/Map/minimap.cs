using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class minimap : MonoBehaviour {
	public bool showMinimap;

	//Texture 
	public Texture minimapText;	//texture of minimap
	public Texture reddotText;	//texture of position marker on map
	public Texture bluedotText;
	private float texture_X;	//teture offset
	private float texture_Y;
	public float minimap_height;	//target height on screen
	public float minimap_width;
	public float minimap_original_width;
	public float minimap_original_height;
	public float reddot_height;		//target marker height
	public float reddot_width;
	public float bluedot_height;		//target marker height
	public float bluedot_width;

	//the two minimap points denote pixel values in the original texture picture
	public float minimap_p1_X;
	public float minimap_p1_Y;
	public float minimap_p2_X;
	public float minimap_p2_Y;
	public float world_p1_X;
	public float world_p1_Z;
	public float world_p2_X;
	public float world_p2_Z;

	private Vector2 L1, L2;	//axis for new coodinate system of the world
	private Vector2 K1, K2;	//axis for new coodinate system of the minimap
	float p;

	//A List of Marker Positions
	private List<float[]> markerPosOnMinimap = new List<float[]>();

	private float alpha; // alpha value of GUI color
	public float guiFadeSpeed;
	public bool fadeMinimap;

	// Use this for initialization
	void Awake (){
		//you can set here coordX, and coordY to make sure the texture is drawn in the correct position
		texture_X 	= 20f;
		texture_Y 	= 20f;
		showMinimap = true;

		//precalculate change of coordinate system for map proection
		L1.x = world_p2_X - world_p1_X;
		L1.y = world_p2_Z - world_p1_Z;	// new coordinate system axis 1 
		L2.x = L1.y;
		L2.y = - L1.x;					// new coordinate system axis 2 (both in plane of world space)

		K1.x = minimap_p2_X - minimap_p1_X;
		K1.y = minimap_p2_Y - minimap_p1_Y;	// new coordinate system axis 1 
		K2.x = K1.y;
		K2.y = - K1.x;
		
		p = L1.x / L1.y;

		fadeMinimap = false;
		alpha = 1f;
	}

	void Update () {}

	void OnGUI () {
		if (showMinimap) {
			GUI.depth = 5;

			//calculate position on map
			float[] coordinates = PositionToMapCoordinates (transform.position.x, transform.position.z, minimap_width, minimap_height);
			
			//add texture offset and reddot width
			coordinates[0] += texture_X - reddot_width / 2f;
			coordinates[1] += texture_Y - reddot_height / 2f;

			//fade minimap if needed
			if(fadeMinimap) FadeToClear();		//calculate alpha here
			GUI.color = new Color(1,1,1,alpha);	//and then set GUI color with it
			GUI.depth = 5;

			//draw map and position marker
			GUI.DrawTexture (new Rect (texture_X, texture_Y, minimap_width, minimap_height), minimapText);
			GUI.DrawTexture (new Rect (coordinates[0], coordinates[1], reddot_width, reddot_height), reddotText);

			//draw all additional markers
			for( int i=0; i < markerPosOnMinimap.Count; i++ ){
				GUI.DrawTexture (new Rect (markerPosOnMinimap[i][0], markerPosOnMinimap[i][1], bluedot_width, bluedot_height), bluedotText);
			}
		}
	}

	void FadeToClear()
	{
		alpha = Mathf.Lerp (alpha, 0f, guiFadeSpeed * Time.deltaTime);

		if (alpha < 0.05f) {
			fadeMinimap = false;
			alpha = 0f;
		}
	}
	
	public void ResetFadeToClear(){
		alpha = 1f;
	}

	public float[] PositionToMapCoordinates(float posX, float posZ, float mapWidth, float mapHeight) {
		// calculate pos of reddot:
		// interpolate from world space to map space (change of coordinate system)
		float v = (posX - p * posZ) / (L2.x - p * L2.y);
		float u = (posZ - v * L2.y) / L1.y;
		
		float pos_on_map_X =  u * K1.x + v * K2.x  +  minimap_original_width / 2f;
		float pos_on_map_Y =  -( u * K1.y + v * K2.y ) + minimap_original_height / 2f;
		
		// get rid of the ridiculous weird offset
		pos_on_map_X = pos_on_map_X - 10f;		// where does this offset come from? no one knows...
		pos_on_map_Y = pos_on_map_Y - 60f;
		
		//scale with the minimap height and width
		float pos_on_minimap_X = pos_on_map_X / minimap_original_width * mapWidth;
		float pos_on_minimap_Y = pos_on_map_Y / minimap_original_height * mapHeight;

		float[] coordinatesOnMap = new float[2];
		coordinatesOnMap [0] = pos_on_minimap_X;
		coordinatesOnMap [1] = pos_on_minimap_Y;
		return coordinatesOnMap;
	}

	public void AddMarkerToMinimap(float posX, float posZ){
		float[] posMinimap = PositionToMapCoordinates (posX, posZ, minimap_width, minimap_height);
		posMinimap[0] += texture_X - bluedot_width / 2f;
		posMinimap[1] += texture_Y - bluedot_height / 2f;
		markerPosOnMinimap.Add (posMinimap);
	}

	public void clearMarkerList(){
		markerPosOnMinimap.Clear ();
	}
}
