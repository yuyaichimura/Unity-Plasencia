using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using YieldProlog;


//@TODO Implement ListPair in YP instead of C# List
class Agent{

	String name;
	//index is meant to be used in the disposition matrices
	int index;

	public Agent(String name, int index){
		this.name = name;
		this.index = index;
	}

	public String getName(){
		return this.name;
	}

	public int getIndex(){
		return this.index;
	}
}

public class TestYP : MonoBehaviour {
	List<Agent> Agents = new List<Agent>();
	int totalAgents;

	/*TRUTHS*/
	IEnumerable<bool> person(object Person){
		foreach (bool l1 in YP.Unify(Person, "Aubrey")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Richard")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Rasmus")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Hailey")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Rufus")) yield return false;
	}

	IEnumerable<bool> alive(object Person, int ind1){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)) yield return false;
	}

	IEnumerable<bool> male(object Person){
		foreach (bool l1 in YP.Unify(Person, "Richard")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Rasmus")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Rufus")) yield return false;
	}

	IEnumerable<bool> female(object Person){
		foreach (bool l1 in YP.Unify(Person, "Aubrey")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Hailey")) yield return false;
	}

	IEnumerable<bool> married(object Person, object Spouse){
		foreach(bool l1 in YP.Unify(Person, "Richard")){
			foreach(bool l2 in YP.Unify(Spouse, "Aubrey")) yield return false;
		}
		foreach(bool l1 in YP.Unify(Person, "Aubrey")){
			foreach(bool l2 in YP.Unify(Spouse, "Richard")) yield return false;
		}
		foreach(bool l1 in YP.Unify(Person, "Rasmus")){
			foreach(bool l2 in YP.Unify(Spouse, "Hailey")) yield return false;
		}
		foreach(bool l1 in YP.Unify(Person, "Hailey")){
			foreach(bool l2 in YP.Unify(Spouse, "Rasmus")) yield return false;
		}
	}

	IEnumerable<bool> siblings(object Person, object Sibling){
		foreach(bool l1 in YP.Unify(Person, "Richard")){
			foreach(bool l2 in YP.Unify(Sibling, "Hailey")) yield return false;
		}
		foreach(bool l1 in YP.Unify(Person, "Hailey")){
			foreach(bool l2 in YP.Unify(Sibling, "Richard")) yield return false;
		}
		foreach(bool l1 in YP.Unify(Person, "Rasmus")){
			foreach(bool l2 in YP.Unify(Sibling, "Rufus")) yield return false;
		}
		foreach(bool l1 in YP.Unify(Person, "Rufus")){
			foreach(bool l2 in YP.Unify(Sibling, "Rasmus")) yield return false;
		}
	}
	/*SUPERTRUTHS*/
	
	IEnumerable<bool> angry(object Angered, object Guilty, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Angered, Agent1)){
			if (Dispositions.checkDisposition("PLS_ANG", ind1, ind2) < -0.5F){
				foreach (bool l2 in YP.Unify(Guilty, Agent2)) yield return false;
			}
		}
	}

	IEnumerable<bool> pleased(object Pleased, object Pleaser, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Pleased, Agent1)){
			if (Dispositions.checkDisposition("PLS_ANG", ind1, ind2) > 0.5F){
				foreach (bool l2 in YP.Unify(Pleaser, Agent2)) yield return false;
			}
		}
	}

	IEnumerable<bool> respects(object Respector, object Respected, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Respector, Agent1)){
			if (Dispositions.checkDisposition("RES_DIS", ind1, ind2) > 0.5F){
				foreach (bool l2 in YP.Unify(Respected, Agent2)) yield return false;
			}
		}
	}

	IEnumerable<bool> disrespects(object Disrespector, object Disrespected, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Disrespector, Agent1)){
			if (Dispositions.checkDisposition("RES_DIS", ind1, ind2) < -0.5F){
				foreach (bool l2 in YP.Unify(Disrespected, Agent2)) yield return false;
			}
		}
	}
	IEnumerable<bool> loves(object Lover, object Loved, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Lover, Agent1)){
			if (Dispositions.checkDisposition("LOVE_HATE", ind1, ind2) > 0.5F){
				foreach (bool l2 in YP.Unify(Loved, Agent2)) yield return false;
			}
		}
	}

	IEnumerable<bool> hates(object Hater, object Hated, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Hater, Agent1)){
			if (Dispositions.checkDisposition("LOVE_HATE", ind1, ind2) < -0.5F){
				foreach (bool l2 in YP.Unify(Hated, Agent2)) yield return false;
			}
		}
	}

	IEnumerable<bool> friend(object Person, object Friend, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("FRIEND_EM", ind1, ind2) > 0.3F){
				foreach (bool l2 in YP.Unify(Friend, Agent2)) yield return false;
			}
		}
	}

	IEnumerable<bool> enemy(object Person, object Enemy, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("FRIEND_EM", ind1, ind2) < -0.5F){
				foreach (bool l2 in YP.Unify(Enemy, Agent2)) yield return false;
			}
		}
	}
	
	IEnumerable<bool> empathizes(object Person, object Person2, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("SOR_ENV", ind1, ind2) > 0.8F){
				foreach (bool l2 in YP.Unify(Person2, Agent2)) yield return false;
			}
		}
	}

	IEnumerable<bool> envious(object Person, object Person2, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("SOR_ENV", ind1, ind2) < -0.6F){
				foreach (bool l2 in YP.Unify(Person2, Agent2)) yield return false;
			}
		}
	}

	IEnumerable<bool> attracted(object Person, object Target, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("ATT_DISG", ind1, ind2) >= 0.8F){
				foreach(bool l2 in YP.Unify(Target, Agent2)) yield return false;
			}
		}
	}

	IEnumerable<bool> disgusted(object Person, object Target, int ind1, int ind2){
		string Agent1 = ((Agent)Agents[ind1]).getName();
		string Agent2 = ((Agent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("ATT_DISG", ind1, ind2) <= -0.7F){
				foreach(bool l2 in YP.Unify(Target, Agent2)) yield return false;
			}
		}
	}

	//CHECK ALL
	void allFriends(){
		for (int i = 0; i < totalAgents; i ++){
			Variable Person = new Variable();
			for(int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Friend = new Variable();
				foreach(bool l1 in friend(Person, Friend, i, j)){
					Debug.Log(YP.GetValue(Person) + " considers " + YP.GetValue(Friend) + " a friend.");
				}
			}
		}
	}

	void allEnemies(){
		for (int i = 0; i < totalAgents; i ++){
			Variable Person = new Variable();
			for(int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Enemy = new Variable();
				foreach(bool l1 in enemy(Person, Enemy, i, j)){
					Debug.Log(YP.GetValue(Person) + " considers " + YP.GetValue(Enemy) + " an enemy.");
				}
			}
		}
	}

	void allLoves(){
		for (int i = 0; i < totalAgents; i++){
			Variable Lover = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Loved = new Variable();
				foreach (bool l1 in loves(Lover, Loved, i, j)){
					Debug.Log(YP.GetValue(Lover) + " loves " + YP.GetValue(Loved) + ".");
				}
			}
		}
	}

	void allHates(){
		for (int i = 0; i < totalAgents; i++){
			Variable Hater = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Hated = new Variable();
				foreach (bool l2 in hates(Hater, Hated, i, j)){
					Debug.Log(YP.GetValue(Hater) + " hates " + YP.GetValue(Hated) + ".");
				}
			}
		}
	}

	void allRespects(){
		for (int i = 0; i < totalAgents; i++){
			Variable Respector = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Respected = new Variable();
				foreach (bool l2 in respects(Respector, Respected, i, j)){
					Debug.Log(YP.GetValue(Respector) + " respects " + YP.GetValue(Respected) + ".");
				}
			}
		}
	}

	void allDisrespects(){
		for (int i = 0; i < totalAgents; i++){
			Variable Disrespector = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Disrespected = new Variable();
				foreach (bool l2 in disrespects(Disrespector, Disrespected, i, j)){
					Debug.Log(YP.GetValue(Disrespector) + " looks down on " + YP.GetValue(Disrespected) + ".");
				}
			}
		}
	}

	void allPleased(){
		for (int i = 0; i < totalAgents; i++){
			Variable Pleased = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Pleaser = new Variable();
				foreach (bool l2 in pleased(Pleased, Pleaser, i, j)){
					Debug.Log(YP.GetValue(Pleased) + " is pleased with " + YP.GetValue(Pleaser) + ".");
				}
			}
		}
	}

	void allAngry(){
		for (int i = 0; i < totalAgents; i++){
			Variable Angered = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Angerer = new Variable();
				foreach (bool l2 in angry(Angered, Angerer, i, j)){
					Debug.Log(YP.GetValue(Angered) + " is furious with " + YP.GetValue(Angerer) + ".");
				}
			}
		}
	}

	void allSorry(){
		for (int i = 0; i < totalAgents; i++){
			Variable Sorrowed = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Target = new Variable();
				foreach (bool l1 in empathizes(Sorrowed, Target, i, j)){
					Debug.Log(YP.GetValue(Sorrowed) + " pities " + YP.GetValue(Target) + ".'");
				}
			}
		}
	}

	void allEnvy(){
		for (int i = 0; i < totalAgents; i++){
			Variable Envious = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Target = new Variable();
				foreach (bool l1 in envious(Envious, Target, i, j)){
					Debug.Log(YP.GetValue(Envious) + " pities " + YP.GetValue(Target) + ".'");
				}
			}
		}
	}

	void allAttracted(){
		for (int i = 0; i < totalAgents; i++){
			Variable Attracted = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Target = new Variable();
				foreach (bool l1 in attracted(Attracted, Target, i, j)){
					Debug.Log(YP.GetValue(Attracted) + " is attracted to " + YP.GetValue(Target));
				}
			}
		}
	}

	void allDisgusted(){
		for (int i = 0; i < totalAgents; i++){
			Variable Disgusted = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Target = new Variable();
				foreach (bool l1 in disgusted(Disgusted, Target, i, j)){
					Debug.Log(YP.GetValue(Disgusted) + " is attracted to " + YP.GetValue(Target));
				}
			}
		}
	}

	
	
/*CONFLICTS SECTION*/
IEnumerable<bool> rivals(object Rival1, object Rival2, int ind1, int ind2){
	string Agent1 = ((Agent)Agents[ind1]).getName();
	string Agent2Name = ((Agent)Agents[ind2]).getName();
	Variable Agent2 = new Variable();
	foreach (bool l1 in YP.Unify(Rival1, Agent1)){
		foreach (bool l2 in respects(Rival1, Agent2, ind1, ind2)){
			foreach (bool l3 in enemy(Rival1, Agent2, ind1, ind2)){
				foreach(bool l4 in YP.Unify(Agent2, Rival2)){
					foreach(bool l5 in YP.Unify(Rival2,Agent2Name)) yield return false;
				}
		}
	}
}
}

IEnumerable<bool> murderous(object Rival1, object Rival2, int ind1, int ind2){
	string Agent1 = ((Agent)Agents[ind1]).getName();
	string Agent2Name = ((Agent)Agents[ind2]).getName();
	Variable Agent2 = new Variable();
	foreach (bool l1 in YP.Unify(Rival1, Agent1)){
		foreach (bool l2 in hates(Rival1, Agent2, ind1, ind2)){
			foreach (bool l3 in enemy(Rival1, Agent2, ind1, ind2)){
				foreach (bool l4 in angry(Rival1, Agent2, ind1, ind2)){
					foreach (bool l5 in envious(Rival1, Agent2, ind1, ind2)){
						foreach (bool l6 in YP.Unify(Agent2, Rival2)){
							foreach (bool l7 in YP.Unify(Rival2, Agent2Name)) yield return false;
						}
					}
				}
			}
		}
	}
}

void allRivals(){
	for (int i = 0; i < totalAgents; i++){
		Variable Rival1 = new Variable();
		for(int j = 0; j < totalAgents; j++){
			if(i == j) continue;
			Variable Rival2 = new Variable();
			foreach (bool l1 in rivals(Rival1, Rival2, i, j)){
				Debug.Log(YP.GetValue(Rival1) + " sees " + YP.GetValue(Rival2) + " as a rival!");
			}
		}
	}
}

void allMurderous(){
	for (int i = 0; i < totalAgents; i++){
		Variable Murderer = new Variable();
		for (int j = 0; j < totalAgents; j++){
			if(i == j) continue;
			Variable Victim = new Variable();
			foreach (bool l1 in murderous(Murderer, Victim, i, j)){
				Debug.Log(YP.GetValue(Murderer) + " wants to get rid of " + YP.GetValue(Victim) + ".");
			}
		}
	}
}

void allBadMarriages(){
	for(int i = 0; i < totalAgents; i++){
		Variable Person = new Variable();
		for(int j = 0; j < totalAgents; j++){
			Variable Spouse = new Variable();
			foreach (bool l1 in married(Person, Spouse)){
				foreach (bool l2 in hates(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " hates his/her spouse, " + YP.GetValue(Spouse) + "!");
				}
				foreach (bool l2 in disgusted(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " is disgusted by his/her spouse, " + YP.GetValue(Spouse) + "!");
				}
				foreach (bool l2 in angry(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " is angry with his/her spouse, " + YP.GetValue(Spouse) + "!");
				}
				foreach (bool l2 in disrespects(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " is ashamed of his/her spouse, " + YP.GetValue(Spouse) + "!");
				}
			}
		}
	}
}

void allBadFriends(){
	for (int i = 0; i < totalAgents; i++){
		Variable Person = new Variable();
		for (int j = 0; j < totalAgents; j++){
			if(i == j) continue;
			Variable Friend = new Variable();
			foreach (bool l1 in friend(Person, Friend, i, j)){
				foreach (bool l2 in angry(Person, Friend, i, j)){
					Debug.Log(YP.GetValue(Person) + " is angry with his/her friend " + YP.GetValue(Friend) + "!");
				}
				foreach (bool l2 in envious(Person, Friend, i, j)){
					Debug.Log(YP.GetValue(Person) + " is jealous of his/her friend " + YP.GetValue(Friend) + "!");
				}
				foreach (bool l2 in hates(Person, Friend, i, j)){
					Debug.Log(YP.GetValue(Person) + " hates his/her friend " + YP.GetValue(Friend + "!"));
				}
			}
		}
	}
}

void allLoveTriangles(){
	for (int i = 0; i < totalAgents; i++){
		Variable Rival1 = new Variable();
		for (int j = 0; j < totalAgents; j ++){
			if(i == j) continue;
			Variable Rival2 = new Variable();
			for (int k = 0; k < totalAgents; k++){
				if(i == k || j == k) continue;
				Variable LoveInterest = new Variable();
				foreach (bool l1 in rivals(Rival1, Rival2, i, j)){
					foreach (bool l2 in loves(Rival1, LoveInterest, i, k)){
						foreach (bool l3 in loves(Rival2, LoveInterest, j, k)){
							Debug.Log(YP.GetValue(Rival1) + " and " + YP.GetValue(Rival2) + " both love " + YP.GetValue(LoveInterest) + "!");
						}
					}
				}
			}
		}
	}
}



	void Start () {
		totalAgents = Dispositions.getMaxAgentNum();
		//initializing Agent list
		Variable AgentList = new Variable();
		String[] names = {"Aubrey", "Rasmus", "Richard", "Hailey", "Rufus"};
		foreach (String name1 in names){
			int num = 0;
			Agent ag1 = new Agent(name1, num);
			Agents.Insert(num,ag1);
			num++;
		}


		//Debug.Log("Use YP.Unify to check a person:");
		//foreach (bool l1 in person("Rasmus")) Debug.Log("Rasmus is a person.");

		//Debug.Log("Use YP.Unify to check all people:");
		//Variable People = new Variable();
			/*foreach (bool l1 in person("Rasmus")){
				Debug.Log("Rasmus" + " is a person.");
			}*/

		/*Variable var = new Variable();
		foreach(bool l1 in hates("Rasmus", var)){
			Debug.Log("Rasmus hates " + YP.GetValue(var));
		}*/

		//Debug.Log("Use YP.Unify to check marriage:");
		/*Variable Husband = new Variable();
		foreach (bool l1 in married("Aubrey", Husband)){
			Debug.Log("Aubrey is married to " + YP.GetValue(Husband));
		}*/

	}


	void Update(){
		if(Input.GetKeyDown(KeyCode.R)){
			Dispositions.RegenDisp();
		}
		else if(Input.GetKeyDown(KeyCode.Z)){
			allFriends();
			allEnemies();
			Dispositions.printDisposition("FRIEND_EM");
		}
		else if(Input.GetKeyDown(KeyCode.X)){
			allLoves();
			allHates();
			Dispositions.printDisposition("LOVE_HATE");
		}
		else if(Input.GetKeyDown(KeyCode.C)){
			allPleased();
			allAngry();
			Dispositions.printDisposition("PLS_ANG");
		}
		else if(Input.GetKeyDown(KeyCode.V)){
			allRespects();
			allDisrespects();
			Dispositions.printDisposition("RES_DIS");
		}
		else if(Input.GetKeyDown(KeyCode.B)){
			allSorry();
			allEnvy();
			Dispositions.printDisposition("SOR_ENV");
		}
		else if(Input.GetKeyDown(KeyCode.N)){
			allAttracted();
			allDisgusted();
			Dispositions.printDisposition("ATT_DISG");
		}
		else if(Input.GetKeyDown(KeyCode.Q)){
			allRivals();
			allMurderous();
			allBadFriends();
			allBadMarriages();
			allLoveTriangles();
		}
	}
	
}
