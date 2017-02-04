using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterMinimap : minimap
{

    public Texture greendotText;
    public Texture blackdotText;
    public Texture yellowdotText;
    public Texture cyandotText;

    private float texture_X_;	//teture offset
    private float texture_Y_;

    private Vector2 L1_, L2_;	//axis for new coodinate system of the world
    private Vector2 K1_, K2_;	//axis for new coodinate system of the minimap
    float p_;

    //A List of Marker Positions
    private List<float[]> markerPosOnMinimap = new List<float[]>();

    private float alpha_; // alpha value of GUI color

    public GameObject[] foregroundCharacters;
    public GameObject[] backgroundCharacters;


    // Use this for initialization
    new void Awake()
    {
        //you can set here coordX, and coordY to make sure the texture is drawn in the correct position
        texture_X_ = 20f;
        texture_Y_ = 20f;
        showMinimap = true;

        //precalculate change of coordinate system for map proection
        L1_.x = world_p2_X - world_p1_X;
        L1_.y = world_p2_Z - world_p1_Z;	// new coordinate system axis 1 
        L2_.x = L1_.y;
        L2_.y = -L1_.x;					// new coordinate system axis 2 (both in plane of world space)

        K1_.x = minimap_p2_X - minimap_p1_X;
        K1_.y = minimap_p2_Y - minimap_p1_Y;	// new coordinate system axis 1 
        K2_.x = K1_.y;
        K2_.y = -K1_.x;

        p_ = L1_.x / L1_.y;

        fadeMinimap = false;
        alpha_ = 1f;

        foregroundCharacters = GameObject.FindGameObjectsWithTag(SmartCharacterCC.TAG_SMARTCHARACTER);
        backgroundCharacters = GameObject.FindGameObjectsWithTag(SmartCharacterCC.TAG_SMARTCROWD);

    }

    void Update() { }

    new void OnGUI()
    {
        if (showMinimap)
        {
            GUI.depth = 5;

            //calculate position on map
            float[] coordinates = PositionToMapCoordinates(transform.position.x, transform.position.z, minimap_width, minimap_height);

            //add texture offset and reddot width
            coordinates[0] += texture_X_ - reddot_width / 2f;
            coordinates[1] += texture_Y_ - reddot_height / 2f;

            //fade minimap if needed
            if (fadeMinimap) FadeToClear();		//calculate alpha here
            GUI.color = new Color(1, 1, 1, alpha_);	//and then set GUI color with it
            GUI.depth = 5;

            //draw map and position marker
            GUI.DrawTexture(new Rect(texture_X_, texture_Y_, minimap_width, minimap_height), minimapText);
            GUI.DrawTexture(new Rect(coordinates[0], coordinates[1], reddot_width, reddot_height), reddotText);

            if (foregroundCharacters.Length == null)
            {
              //  Debug.Log("Null yo");
            }
            else
            {
                //   Debug.Log("Not null yo");
            }

            foreach (GameObject o in foregroundCharacters)
            {
                float[] coords = PositionToMapCoordinates(o.transform.position.x, o.transform.position.z, minimap_width, minimap_height);
                coords[0] += texture_X_ - reddot_width / 2f;
                coords[1] += texture_Y_ - reddot_height / 2f;
                SmartCharacterCC c = o.gameObject.GetComponent<SmartCharacterCC>();
                if (c == null)
                {
                    continue;
                }
                if (c.Controlled)
                {
                    GUI.DrawTexture(new Rect(coords[0], coords[1], reddot_width, reddot_height), cyandotText);
                }
                else
                {
                    GUI.DrawTexture(new Rect(coords[0], coords[1], reddot_width, reddot_height), greendotText);
                }
            }

            foreach (GameObject o in backgroundCharacters)
            {
                float[] coords = PositionToMapCoordinates(o.transform.position.x, o.transform.position.z, minimap_width, minimap_height);
                coords[0] += texture_X_ - reddot_width / 2f;
                coords[1] += texture_Y_ - reddot_height / 2f;
                GUI.DrawTexture(new Rect(coords[0], coords[1], reddot_width, reddot_height), blackdotText);
            }

            //draw all additional markers
            for (int i = 0; i < markerPosOnMinimap.Count; i++)
            {
                GUI.DrawTexture(new Rect(markerPosOnMinimap[i][0], markerPosOnMinimap[i][1], bluedot_width, bluedot_height), bluedotText);
            }
        }
    }

    void FadeToClear()
    {
        alpha_ = Mathf.Lerp(alpha_, 0f, guiFadeSpeed * Time.deltaTime);

        if (alpha_ < 0.05f)
        {
            fadeMinimap = false;
            alpha_ = 0f;
        }
    }

    public void ResetFadeToClear()
    {
        alpha_ = 1f;
    }

    public float[] PositionToMapCoordinates(float posX, float posZ, float mapWidth, float mapHeight)
    {
        // calculate pos of reddot:
        // interpolate from world space to map space (change of coordinate system)
        float v = (posX - p_ * posZ) / (L2_.x - p_ * L2_.y);
        float u = (posZ - v * L2_.y) / L1_.y;

        float pos_on_map_X = u * K1_.x + v * K2_.x + minimap_original_width / 2f;
        float pos_on_map_Y = -(u * K1_.y + v * K2_.y) + minimap_original_height / 2f;

        // get rid of the ridiculous weird offset
        pos_on_map_X = pos_on_map_X - 10f;		// where does this offset come from? no one knows...
        pos_on_map_Y = pos_on_map_Y - 60f;

        //scale with the minimap height and width
        float pos_on_minimap_X = pos_on_map_X / minimap_original_width * mapWidth;
        float pos_on_minimap_Y = pos_on_map_Y / minimap_original_height * mapHeight;

        float[] coordinatesOnMap = new float[2];
        coordinatesOnMap[0] = pos_on_minimap_X;
        coordinatesOnMap[1] = pos_on_minimap_Y;
        return coordinatesOnMap;
    }

    public void AddMarkerToMinimap(float posX, float posZ)
    {
        float[] posMinimap = PositionToMapCoordinates(posX, posZ, minimap_width, minimap_height);
        posMinimap[0] += texture_X_ - bluedot_width / 2f;
        posMinimap[1] += texture_Y_ - bluedot_height / 2f;
        markerPosOnMinimap.Add(posMinimap);
    }

    public void clearMarkerList()
    {
        markerPosOnMinimap.Clear();
    }
}
