using UnityEngine;
using System.Collections;

public class InformationScreen : MonoBehaviour {
	
	public Texture backgroundText;
	public Texture2D closeX;
	public TextAsset short_story;
	public GUIStyle contentStyle;
	public int minCharactersPerSection;

	private float textureWidth;
	private float textureHeight;
	private float margin;

	public int leftPageX;
	public int leftPageY;
	public int leftPageWidth;
	public int leftPageHeigth;
	public int rightPageX;
	public int rightPageY;
	public int rightPageWidth;
	public int rightPageHeigth;
	private int current_doublepage;
	private int shortstory_doublepages;

	private bool show_short_story;
	
	private PauseMenu pausemenu;
	private minimap map;
	private SpacebarPauseMovement camMotion;

	private PauseMenu.Page returnPage = PauseMenu.Page.None;

	public bool hyperlink = false;
	public int  linkPage  = 0;
	public int  linkLeft = 0;
	public int  linkTop   = 0;
	public int  linkWidth = 0;
	public int  linkHeight = 0;
	public string link = "";

	void Awake () {
		pausemenu 		= GameObject.FindGameObjectWithTag(Tags.main_camera).GetComponent<PauseMenu>();
		map 			= GameObject.FindGameObjectWithTag(Tags.main_camera).GetComponent<minimap> ();
		camMotion   	= GameObject.FindGameObjectWithTag(Tags.main_camera).GetComponent<SpacebarPauseMovement> ();

		textureWidth 	= 1500;
		textureHeight 	= 1082;
		margin 			= 60;

		//calculate new width and hight according to screen size
		float scalingFactor = 1f;
		if ( textureWidth + 2f*margin > Screen.width ) {	
			scalingFactor = ((float) Screen.width ) / ((float) textureWidth  + 2f*margin );		
		}
		if ( textureHeight + 2f*margin > Screen.height ) {
			float q = ((float) Screen.height) / ((float) textureHeight + 2f*margin );
			scalingFactor = Mathf.Min( scalingFactor, q );
		}

		// set the book to a fixed size for now!! TODO
		//textureWidth *= scalingFactor;
		//textureHeight *= scalingFactor;
		textureWidth = 800;
		textureHeight = 600;

		shortstory_doublepages = Mathf.CeilToInt( (float) countPages(short_story.ToString()) / 2.0f );

	}

	void Update () {
		if (pausemenu.currentPage != PauseMenu.Page.Book) {
			show_short_story = false;
			current_doublepage = 1;
		}	
	}

	void OnGUI () {
		if (show_short_story) DrawShortStory ();
	}



	// ------------- Information Pages ---------------------
	
	public void OpenInformationPage() {
		pausemenu.currentPage = PauseMenu.Page.Book;

		//map.showMinimap = false; // close minimap
		FloatingMenu.showFloatingMenus = false;	// close all floating menus
		camMotion.StopAllMotion ();
	}

	public static void CloseInformationScreen() {
		FloatingMenu.showFloatingMenus = true;
	}
	

	public void OpenShortStory() {
		OpenInformationPage ();
		show_short_story = true;
	}

	void DrawShortStory() {
		GUI.depth = 2;

		FloatingMenu.showFloatingMenus = false;	// close all floating menus
		// TODO: why is this necessary every frame?!?!?

		//draw the book
		int top = (Screen.height - (int) textureHeight) / 2;
		int left = (Screen.width - (int) textureWidth) / 2;
		GUI.DrawTexture (new Rect (left, top, textureWidth, textureHeight), backgroundText);
		
		//select the doublepage to display
		string leftPage = GetSectionOfText(short_story.ToString(), current_doublepage * 2 - 1);
		string rightPage = GetSectionOfText(short_story.ToString(), current_doublepage * 2 );

		//now draw the text into the book
		float lX = leftPageX / 100f * textureWidth;
		float lY = leftPageY / 100f * textureHeight;
		float lW = leftPageWidth / 100f * textureWidth;
		float lH = leftPageHeigth / 100f * textureHeight;
		float rX = rightPageX / 100f * textureWidth;
		float rY = rightPageY / 100f * textureHeight;
		float rW = rightPageWidth / 100f * textureWidth;
		float rH = rightPageHeigth / 100f * textureHeight;

		GUI.Box (new Rect (left+lX, top+lY+40, lW, lH), leftPage, contentStyle);
		GUI.Box (new Rect (left+rX, top+rY+40, rW, rH), rightPage, contentStyle);

		//show next page button
		if (current_doublepage < shortstory_doublepages) {
			if (GUI.Button(new Rect(left+rX+rW - 100, top+rY+rH+10, 100, 50), "Next Page" )) {
				current_doublepage++;
			}
		}

		// Show Previous Page Button
		if (current_doublepage > 1) {
			if (GUI.Button(new Rect(left + lX, top+lY+lH+10, 110, 50), "Previous Page" )) {
				current_doublepage--;
			}
			
		}

		// Show Close Button
		if (GUI.Button(new Rect(left+rX+rW-15, top+rY-20, 30, 30),closeX,GUIStyle.none)) {
			if ( returnPage == PauseMenu.Page.None ) {
				camMotion.ContinueMotion();
				map.showMinimap = true;
			}
			pausemenu.currentPage = returnPage;
		}

		//show page numbers
		int totalPages = shortstory_doublepages * 2;
		int leftPageNo = current_doublepage * 2 - 1;
		int rightPageNo = current_doublepage * 2;

		GUI.skin.label.fontSize = 16;
		GUI.contentColor = Color.black;
		GUI.Label( new Rect (left+lX+lW-130, top+rH+rY+40, 140,80), leftPageNo.ToString() + "/" + totalPages.ToString());
		GUI.Label( new Rect (left+rX+rW-150, top+rH+rY+40, 140,80), rightPageNo.ToString() + "/" + totalPages.ToString());

		//show hyperlink
		if(hyperlink && current_doublepage == linkPage/2 + linkPage%2) {
			if (GUI.Button(new Rect(left+linkLeft, top+linkTop, linkWidth, linkHeight), "", GUIStyle.none)) {	//need GUIStyle.none here later
				Application.OpenURL(link);
			}
		}

	}

	int countPages( string text ) {
		text = text.Replace("\n", " <newline>");
		text = text.Replace("<nextpage>", " <nextpage>");
		string[] words = text.Split(" "[0]); //Split the string into seperate words
		int currentSection = 1;
		int currentCharacters = 0;
		
		for( var index = 0; index < words.Length; index++)
		{
			// put all words into the result until the current section is fully in there
			string word = words[index].Trim();
			
			if( word != "<nextpage>" ) {
				currentCharacters += word.Length;
			}
			
			if (currentCharacters > minCharactersPerSection ||  word == "<nextpage>"  )
			{
				currentCharacters = 0;
				currentSection++;
			}
		}
		return currentSection;
	}

	string GetSectionOfText( string text, int section ) {
		text = text.Replace("\n", " <newline>");
		text = text.Replace("<nextpage>", " <nextpage>");	//cause of the newlines
		string[] words = text.Split(" "[0]); //Split the string into seperate words
		string result = "";
		int currentSection = 1;
		int currentCharacters = 0;
		
		for( var index = 0; index < words.Length; index++)
		{
			// put all words into the result until the current section is fully in there
			string word = words[index].Trim();

			if( word != "<nextpage>" ) {
				if (index == 0) result = word;			//first word	
				if (index > 0 ) result += " " + word;	//all other words
				currentCharacters += word.Length;
			}

			if (currentCharacters > minCharactersPerSection ||  word == "<nextpage>"  )
			{
				// now see if this is the section we want
				if( currentSection == section ) {
					result = result.Replace("<newline>", "\n"); 
					return result;
				}
				else {
					result = "";
					currentCharacters = 0;
					currentSection++;
				}
			}
		}
		// now see if perhaps the last section is the section we want
		if( currentSection == section ) return result.Replace("<newline>", "\n");
		else return "";
	}

	public void setReturnPage(PauseMenu.Page newReturnPage) {
		returnPage = newReturnPage;
	}

	public void setHyperlink(int page, int l, int t, int w, int h, string s){
		linkPage = page;
		linkTop = t;
		linkLeft = l;
		linkWidth = w;
		linkHeight = h;
		link = s;
	}
	
	
}
