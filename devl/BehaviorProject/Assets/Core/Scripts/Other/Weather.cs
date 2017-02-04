using UnityEngine;
using System.Collections;

public class Weather : MonoBehaviour {

	public Material materialSunset;
	public Material materialDay1;
	public Material materialDay2;
	public Material materialDay3;
	public Material materialClouds1;
	public Material materialClouds2;

	private enum Sky {
		Sunset,
		Day1,
		Day2,
		Day3,
		Clouds1,
		Clouds2
	}
	private Sky currentSkybox;
	private bool switchSkybox;



	void Awake () {
		currentSkybox = Sky.Day1;
		switchSkybox = true;
	}

	void Update () {	
	}

	void OnGUI() {
		if (switchSkybox) {
			switch (currentSkybox) {
			case Sky.Sunset: 	
				RenderSettings.skybox = materialSunset;  
				RenderSettings.fogColor = new Color(74f/255f, 69f/255f, 66f/255f, 1f); 
				break;
			case Sky.Day1:   	
				RenderSettings.skybox = materialDay1;    
				RenderSettings.fogColor = new Color(171f/255f, 219f/255f, 248f/255f, 1f); 
				break;
			case Sky.Day2:   	
				RenderSettings.skybox = materialDay2;    
				RenderSettings.fogColor = new Color(156f/255f, 186f/255f, 189f/255f, 1f); 
				break;
			case Sky.Day3:   	
				RenderSettings.skybox = materialDay3;    
				RenderSettings.fogColor = new Color(156f/255f, 186f/255f, 189f/255f, 1f);
				break;
			case Sky.Clouds1:   
				RenderSettings.skybox = materialClouds1; 
				RenderSettings.fogColor = new Color(77f/255f, 93f/255f, 103f/255f, 1f);
				break;
			case Sky.Clouds2:   
				RenderSettings.skybox = materialClouds2; 
				RenderSettings.fogColor = new Color(63f/255f, 74f/255f, 82f/255f, 1f);
				break;
			} 

			switchSkybox = false;
		}
	}

	public void setRandomSky() {
		switchSkybox = true;
		currentSkybox = GetRandomEnum<Sky> ();
	}

	public void setSkyByYear(int year) {
		switchSkybox = true;

		if (year ==Timeline.yearList[0]) currentSkybox = Sky.Sunset;
		else if (year == Timeline.yearList[1]) currentSkybox = Sky.Day1;
		else if (year == Timeline.yearList[2]) currentSkybox = Sky.Day2;
	 	else if (year == Timeline.yearList[3]) currentSkybox = Sky.Day3;
		else if (year == Timeline.yearList[4]) currentSkybox = Sky.Clouds1;
		else if (year == Timeline.yearList[5]) currentSkybox = Sky.Clouds2;
	}

	static T GetRandomEnum<T>()
	{
		System.Array A = System.Enum.GetValues(typeof(T));
		T V = (T)A.GetValue(UnityEngine.Random.Range(0,A.Length));
		return V;
	}


}