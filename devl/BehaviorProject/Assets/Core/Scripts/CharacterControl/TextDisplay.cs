using UnityEngine;
using System.Collections;

public class TextDisplay : MonoBehaviour
{

    string[] texts;
    int index;

    public GUISkin skinContinue2;

    public Font font;


    public Texture pageHalf;
    public Texture pageText;
    public Texture pageWhite;
    public Texture2D closeX;
    public Texture2D bContinue;

    private GameObject cam;
    private FreeflightCam freeflightCam;
    private CharacterMotor characterMotor;
    private FPSInputController inputController;

    private GUISkin defaultSkin;

    bool displayText;

    void Awake()
    {
        cam = GameObject.FindGameObjectWithTag(Tags.main_camera);
        freeflightCam = cam.GetComponent<FreeflightCam>();

        characterMotor = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<CharacterMotor>();
        inputController = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<FPSInputController>();
        displayText = false;

        
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown("k"))
        {
            displayText = !displayText;
            //string[] t = { "Dubugging: Welcome to Plasencia, the year of Our Lord, one-thousand-four-hundred and sixteen (5176 of the Hebrew calendar and 819 of the Islamic calendar). \n\nEver since the 1390s and the massive anti-Jewish riots that characterized this period, Jews in the Kingdom of Castile and Leon had resided in a religiously-charged environment that placed their communities in economic, religious, and physical jeopardy. ", "Plasencia, ruled directly by Christian King Juan II, was not free of anti-Jewish sentiment. There were serious disruptions to the traditional residential intermixing of Jewish, Christian, and Muslim families, which was a hallmark of co-existence in medieval Spain." };
            string[] t = { "DEBUGGING, DEBUGGING, DEBUGGING, DEBUGGING, DEBUGGING, DEBUGGING", "i", "This text should be in italics", "b", "This text should be bold", "bi", "This text should be bold AND italic", "n", "This text should be normal" };
            this.texts = t;
        }
    }

    void OnGUI()
    {

        if (this.texts != null)
        {
            DisplayText();
        }
    }

    void DisplayText()
    {
       // int textmode = 0;
        if (this.texts == null || this.texts.Length <= this.index)
        {
            return;
        }

        if (this.texts[this.index].CompareTo("n") == 0)
        {
            //Text is normal
           // textmode = 0;
            GUI.skin.label.fontStyle = FontStyle.Normal;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            nextMessage();

            return;
        }
        else if (this.texts[this.index].CompareTo("i") == 0)
        {
            //Text is in italics
            // textmode = 1;
            GUI.skin.label.fontStyle = FontStyle.Italic;
            nextMessage();

            return;

        }
        else if (this.texts[this.index].CompareTo("b") == 0)
        {
            //Text is in italics
            // textmode = 1;
            GUI.skin.label.fontStyle = FontStyle.Bold;
            nextMessage();

            return;

        }
        else if (this.texts[this.index].CompareTo("bi") == 0)
        {
            //Text is in italics
            // textmode = 1;
            GUI.skin.label.fontStyle = FontStyle.BoldAndItalic;
            nextMessage();

            return;

        }
        else if (this.texts[this.index].CompareTo("mc") == 0)
        {
            //Text is in italics
            // textmode = 1;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            nextMessage();

            return;

        }

        int textureHeight = 160;
        int textureWidth = 1300;

        float top = (Screen.height - textureHeight);
        float left = (Screen.width - textureWidth) / 2f;
        GUI.DrawTexture(new Rect(left, top, textureWidth, textureHeight + 20), pageWhite);

        GUI.skin = skinContinue2; 
        if (GUI.Button(new Rect(left + textureWidth - 90, top + textureHeight - 30, 30, 30), bContinue, GUIStyle.none))
        {
            nextMessage();
        }

        //TEXT SETTINGS
        GUI.skin = defaultSkin;
        GUI.contentColor = Color.black;
        GUI.skin.label.font = font;
        GUI.skin.label.fontSize = 19;

        if (this.texts == null)
        {
        //    Debug.Log("Null text");
            return;
        }
       // Debug.Log("index: " + index);
        GUI.Label(new Rect(left + 75, top + 39, textureWidth - 135, textureHeight - 50), this.texts[this.index]);
    }

    public void SetText(string[] texts)
    {
        this.texts = texts;
        this.index = 0;
    }

    void nextMessage()
    {
        if (this.texts == null)
            return;

        this.index++;
        if (this.texts.Length <= this.index)
        {
            this.index = 0;
            this.texts = null;
        }
    }

    public bool displayingMessage()
    {
        if (this.texts != null)
        {
            return true;
        }
        return false;
    }

    public bool TextComplete()
    {
        if (this.texts != null)
        {
            return false;
        }
        return true;
    }
}
