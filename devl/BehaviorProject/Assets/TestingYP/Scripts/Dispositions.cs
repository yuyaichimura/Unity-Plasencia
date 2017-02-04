using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using YieldProlog;


public class Dispositions : MonoBehaviour {

	//SUPER IMPORTANT: DEFINE THE NUMBER OF CHARACTERS IN THE SCENE HERE
	//be sure to take into account conflict factor!
	public static int number_of_agents = 2;
	
	static List<float[][]> DISP = new List<float[][]>();
	static float[][] LOVE_HATE;
	static float[][] FRIEND_EM; //friendliness, enmity
	static float[][] RES_DIS; //respect, disrespect
	static float[][] PLS_ANG; //pleased with/angry with
	static float[][] SOR_ENV; //envy, sorrow for
	static float[][] ATT_DISG; //attracted, disgusted

	public static int getMaxAgentNum(){
		return number_of_agents;
	}

	void Awake () {
		PLS_ANG = new float[number_of_agents][];
		RES_DIS = new float[number_of_agents][];
		FRIEND_EM = new float[number_of_agents][];
		LOVE_HATE = new float[number_of_agents][];
		SOR_ENV = new float[number_of_agents][];
		ATT_DISG = new float[number_of_agents][];
		DISP.Add(LOVE_HATE);
		DISP.Add(FRIEND_EM);
		DISP.Add(RES_DIS);
		DISP.Add(PLS_ANG);
		DISP.Add(SOR_ENV);
		DISP.Add(ATT_DISG);
		foreach (float[][] disp in DISP){
			for (int i = 0; i < number_of_agents; i++){
				disp[i] = new float[number_of_agents];
				for (int j = 0; j < number_of_agents; j++){
					disp[i][j] = Random.Range(-1.0F, 1.0F);
				}
			}
		}	
	}
	
	void Start() {
		
	}

	void Update () {

	}

	//regenerates Disposition values--good if there are no conflicts found
	public static void RegenDisp(){
		foreach (float[][] disp in DISP){
			for (int i = 0; i < number_of_agents; i++){
				disp[i] = new float[number_of_agents];
				for (int j = 0; j < number_of_agents; j++){
					disp[i][j] = Random.Range(-1.0F, 1.0F);
				}
			}
		}
	}

	public static float checkDisposition(string DISP_ARRAY, int ag1, int ag2){
		switch(DISP_ARRAY){
			case "LOVE_HATE":
				return LOVE_HATE[ag1][ag2];
				break;
			case "FRIEND_EM":
				return FRIEND_EM[ag1][ag2];
				break;
			case "RES_DIS":
				return RES_DIS[ag1][ag2];
				break;
			case "PLS_ANG":
				return PLS_ANG[ag1][ag2];
				break;
			case "SOR_ENV":
				return SOR_ENV[ag1][ag2];
				break;
			case "ATT_DISG":
				return ATT_DISG[ag1][ag2];
				break;
			default:
				Debug.Log("No matching disposition matrix found.");
				return 0.0F;
		}
	}

	public static void printDisposition(string disposition){
		for(int i = 0; i < number_of_agents; i++){
			for(int j = 0; j < number_of_agents; j++){
				Debug.Log(disposition + "[" + i + "][" + j + "]: " + checkDisposition(disposition, i, j));
			}
		}
	}

	public static void changeDisposition(float change, string DISP_ARRAY, int ag1, int ag2){
		switch(DISP_ARRAY){
			case "LOVE_HATE":
				LOVE_HATE[ag1][ag2] += change;
				break;
			case "FRIEND_EM":
				FRIEND_EM[ag1][ag2] += change;
				break;
			case "RES_DIS":
				RES_DIS[ag1][ag2] += change;
				break;
			case "PLS_ANG":
				PLS_ANG[ag1][ag2] += change;
				break;
			case "SOR_ENV":
				SOR_ENV[ag1][ag2] += change;
				break;
			case "ATT_DISG":
				ATT_DISG[ag1][ag2] += change;
				break;
			default:
				Debug.Log("No matching disposition matrix found.");
				break;
		}
	}


}
