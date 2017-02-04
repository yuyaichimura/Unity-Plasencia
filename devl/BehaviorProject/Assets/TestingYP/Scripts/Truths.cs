using UnityEngine;
using System.Collections;
using POP;
using System.Collections.Generic;
using YieldProlog;


class DYPAgent{

	string name;
	//index is meant to be used in the disposition matrices
	int index;

	public DYPAgent(string name, int index){
		this.name = name;
		this.index = index;
	}

	public string getName(){
		return this.name;
	}

	public int getIndex(){
		return this.index;
	}
}

public class Truths : MonoBehaviour {
	static List<DYPAgent> Agents = new List<DYPAgent>();
	static int totalAgents = Dispositions.getMaxAgentNum();

	public static string getAgentName(int index){
		return ((DYPAgent)Agents[index]).getName();
	}

	/*TRUTHS--HERE, YOU SET THE SCENE*/
	public static IEnumerable<bool> alive(object Person, int ind1){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)) yield return false;
	}

	public static IEnumerable<bool> person(object Person){
		foreach(bool l1 in YP.Unify(Person, "Cain")) yield return false;
		foreach(bool l1 in YP.Unify(Person, "Abel")) yield return false;

		/*foreach (bool l1 in YP.Unify(Person, "Aubrey")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Richard")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Rasmus")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Hailey")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Rufus")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Cassandra")) yield return false;*/
		
		/*foreach (bool l1 in YP.Unify(Person, "Christian 2")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Muslim 2")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Muslim 1")) yield return false;*/

		/*foreach(bool l1 in YP.Unify(Person, "Thief")) yield return false;
		foreach(bool l1 in YP.Unify(Person, "Mark")) yield return false;*/

	}

	public static IEnumerable<bool> male(object Person){
		foreach(bool l1 in YP.Unify(Person, "Cain")) yield return false;
		foreach(bool l1 in YP.Unify(Person, "Abel")) yield return false;

		/*foreach(bool l1 in YP.Unify(Person, "Thief")) yield return false;
		foreach(bool l1 in YP.Unify(Person, "Mark")) yield return false;

		foreach (bool l1 in YP.Unify(Person, "Christian 2")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Muslim 2")) yield return false;

		foreach (bool l1 in YP.Unify(Person, "Richard")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Rasmus")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Rufus")) yield return false;*/
	}

	public static IEnumerable<bool> female(object Person){
		foreach (bool l1 in YP.Unify(Person, "Muslim 1")) yield return false;

		foreach (bool l1 in YP.Unify(Person, "Aubrey")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Hailey")) yield return false;
		foreach (bool l1 in YP.Unify(Person, "Cassandra")) yield return false;
	}

	public static IEnumerable<bool> married(object Person, object Spouse){
		
		foreach (bool l1 in YP.Unify(Person, "Muslim 1")){
			foreach (bool l2 in YP.Unify(Person, "Muslim 2")) yield return false;
		}
		foreach (bool l1 in YP.Unify(Person, "Muslim 2")){
			foreach (bool l2 in YP.Unify(Person, "Muslim 1")) yield return false;
		}
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

	public static IEnumerable<bool> siblings(object Person, object Sibling){
		foreach(bool l1 in YP.Unify(Person, "Cain")){
			foreach(bool l2 in YP.Unify(Person, "Abel")) yield return false;
		}

		foreach(bool l1 in YP.Unify(Person, "Abel")){
			foreach(bool l2 in YP.Unify(Person, "Cain")) yield return false;
		}

		/*
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
		}*/
	}
	
	
	/*SUPERTRUTHS--FROM THE SCENE YOU SET, WE FIND HOW EVERYONE FEELS ABOUT EACH OTHER*/
	public static IEnumerable<bool> notSiblings(object Person1, object Person2){
		foreach(bool l1 in siblings(Person1, Person2)){
			yield break;
		}
		yield return false;
	}

	public static IEnumerable<bool> angry(object Angered, object Guilty, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Angered, Agent1)){
			if (Dispositions.checkDisposition("PLS_ANG", ind1, ind2) < -0.5F){
				foreach (bool l2 in YP.Unify(Guilty, Agent2)) yield return false;
			}
		}
	}

	public static IEnumerable<bool> pleased(object Pleased, object Pleaser, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Pleased, Agent1)){
			if (Dispositions.checkDisposition("PLS_ANG", ind1, ind2) > 0.5F){
				foreach (bool l2 in YP.Unify(Pleaser, Agent2)) yield return false;
			}
		}
	}

	public static IEnumerable<bool> respects(object Respector, object Respected, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Respector, Agent1)){
			if (Dispositions.checkDisposition("RES_DIS", ind1, ind2) > 0.5F){
				foreach (bool l2 in YP.Unify(Respected, Agent2)) yield return false;
			}
		}
	}

	public static IEnumerable<bool> disrespects(object Disrespector, object Disrespected, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Disrespector, Agent1)){
			if (Dispositions.checkDisposition("RES_DIS", ind1, ind2) < -0.5F){
				foreach (bool l2 in YP.Unify(Disrespected, Agent2)) yield return false;
			}
		}
	}

	public static IEnumerable<bool> loves(object Lover, object Loved, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Lover, Agent1)){
			if (Dispositions.checkDisposition("LOVE_HATE", ind1, ind2) > 0.5F){
				foreach (bool l2 in YP.Unify(Loved, Agent2)) yield return false;
			}
		}
	}

	public static IEnumerable<bool> hates(object Hater, object Hated, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Hater, Agent1)){
			if (Dispositions.checkDisposition("LOVE_HATE", ind1, ind2) < -0.5F){
				foreach (bool l2 in YP.Unify(Hated, Agent2)) yield return false;
			}
		}
	}

	public static IEnumerable<bool> friend(object Person, object Friend, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("FRIEND_EM", ind1, ind2) >= 0F){
				foreach (bool l2 in YP.Unify(Friend, Agent2)) yield return false;
			}
		}
	}

	public static IEnumerable<bool> enemy(object Person, object Enemy, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("FRIEND_EM", ind1, ind2) < 0F){
				foreach (bool l2 in YP.Unify(Enemy, Agent2)) yield return false;
			}
		}
	}
	
	public static IEnumerable<bool> empathizes(object Person, object Person2, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("SOR_ENV", ind1, ind2) > 0.8F){
				foreach (bool l2 in YP.Unify(Person2, Agent2)) yield return false;
			}
		}
	}

	public static IEnumerable<bool> envious(object Person, object Person2, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("SOR_ENV", ind1, ind2) < -0.6F){
				foreach (bool l2 in YP.Unify(Person2, Agent2)) yield return false;
			}
		}
	}

	public static IEnumerable<bool> attracted(object Person, object Target, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("ATT_DISG", ind1, ind2) >= 0.8F){
				foreach(bool l2 in YP.Unify(Target, Agent2)) yield return false;
			}
		}
	}

	public static IEnumerable<bool> disgusted(object Person, object Target, int ind1, int ind2){
		string Agent1 = ((DYPAgent)Agents[ind1]).getName();
		string Agent2 = ((DYPAgent)Agents[ind2]).getName();
		foreach (bool l1 in YP.Unify(Person, Agent1)){
			if(Dispositions.checkDisposition("ATT_DISG", ind1, ind2) <= -0.7F){
				foreach(bool l2 in YP.Unify(Target, Agent2)) yield return false;
			}
		}
	}

	//CHECK ALL
	public static void allFriends(){
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

	public static void allEnemies(){
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

	public static void allLoves(){
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

	public static void allHates(){
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

	public static void allRespects(){
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

	public static void allDisrespects(){
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

	public static void allPleased(){
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

	public static void allAngry(){
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

	public static void allSorry(){
		for (int i = 0; i < totalAgents; i++){
			Variable Sorrowed = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Target = new Variable();
				foreach (bool l1 in empathizes(Sorrowed, Target, i, j)){
					Debug.Log(YP.GetValue(Sorrowed) + " pities " + YP.GetValue(Target) + ".");
				}
			}
		}
	}

	public static void allEnvy(){
		for (int i = 0; i < totalAgents; i++){
			Variable Envious = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Target = new Variable();
				foreach (bool l1 in envious(Envious, Target, i, j)){
					Debug.Log(YP.GetValue(Envious) + " envies " + YP.GetValue(Target) + ".");
				}
			}
		}
	}

	public static void allAttracted(){
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

	public static void allDisgusted(){
		for (int i = 0; i < totalAgents; i++){
			Variable Disgusted = new Variable();
			for (int j = 0; j < totalAgents; j++){
				if(i == j) continue;
				Variable Target = new Variable();
				foreach (bool l1 in disgusted(Disgusted, Target, i, j)){
					Debug.Log(YP.GetValue(Disgusted) + " is disgusted by " + YP.GetValue(Target));
				}
			}
		}
	}

	
	
/*CONFLICTS SECTION*/
public static IEnumerable<bool> feelingsFor(object Person, object Object, int ind1, int ind2){
	string Agent1 = ((DYPAgent)Agents[ind1]).getName();
	string Agent2Name = ((DYPAgent)Agents[ind2]).getName();
	Variable Agent2 = new Variable();
	foreach (bool l1 in YP.Unify(Person, Agent1)){
		foreach (bool l2 in attracted(Person, Agent2, ind1, ind2)){
			foreach (bool l3 in loves(Person, Agent2, ind1, ind2)){
				foreach (bool l4 in male(Person)){
					foreach (bool l5 in female(Agent2)){
						foreach (bool l6 in notSiblings(Person, Agent2)){
							foreach (bool l7 in YP.Unify(Agent2, Object)){
								foreach (bool l8 in YP.Unify(Object, Agent2Name)) yield return false;
							}
						}
					}
				}
				foreach (bool l4 in female(Person)){
					foreach (bool l5 in male(Agent2)){
						foreach (bool l6 in notSiblings(Person, Agent2)){
							foreach (bool l7 in YP.Unify(Agent2, Object)){
								foreach (bool l8 in YP.Unify(Object, Agent2Name)) yield return false;
							}
						}
					}
				}
			}
		}
	}
}
public static IEnumerable<bool> rivals(object Rival1, object Rival2, int ind1, int ind2){
	string Agent1 = ((DYPAgent)Agents[ind1]).getName();
	string Agent2Name = ((DYPAgent)Agents[ind2]).getName();
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

public static IEnumerable<bool> murderous(object Rival1, object Rival2, int ind1, int ind2){
	string Agent1 = ((DYPAgent)Agents[ind1]).getName();
	string Agent2Name = ((DYPAgent)Agents[ind2]).getName();
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


public static IEnumerable<bool> depressed(object Person, int index){
	string AgentName = ((DYPAgent)Agents[index]).getName();
	foreach (bool l1 in empathizes(Person, Person, index, index)){
		foreach (bool l2 in hates(Person, Person, index, index)){
            foreach (bool l3 in disrespects(Person, Person, index, index)){
                foreach (bool l4 in disgusted(Person, Person, index, index)){
                    foreach (bool l5 in YP.Unify(Person, AgentName)) yield return false;
                }
            }
		}
	}
}

public static int allDepressed(){
	int depressedPeople = 0;
	for(int i = 0; i < totalAgents; i++){
		Variable Person = new Variable();
		foreach(bool l1 in depressed(Person, i)){
			Debug.Log(YP.GetValue(Person) + " wonders when it will all end.");
			depressedPeople++;
		}
	}
	return depressedPeople;
}

public static int allRivals(){
	int rivalries = 0;
	for (int i = 0; i < totalAgents; i++){
		Variable Rival1 = new Variable();
		for(int j = 0; j < totalAgents; j++){
			if(i == j) continue;
			Variable Rival2 = new Variable();
			foreach (bool l1 in rivals(Rival1, Rival2, i, j)){
				Debug.Log(YP.GetValue(Rival1) + " sees " + YP.GetValue(Rival2) + " as a rival!");
				rivalries++;
			}
		}
	}
	return rivalries;
}

public static int siblingRivalries(){
	int siblingRivalries = 0;
	for (int i = 0; i < totalAgents; i++){
		Variable Sibling1 = new Variable();
		for (int j = 0; j < totalAgents; j++){
			if(i == j) continue;
			Variable Sibling2 = new Variable();
			foreach (bool l1 in siblings(Sibling1, Sibling2)){
				foreach (bool l3 in male(Sibling1)){
					foreach (bool l2 in rivals(Sibling1, Sibling2, i, j)){
						Debug.Log(YP.GetValue(Sibling1) + " and his sibling, " + YP.GetValue(Sibling2) + ", are not on the best terms.");
						siblingRivalries++;
					}
				}
				foreach (bool l3 in female(Sibling1)){
					foreach (bool l2 in rivals(Sibling1, Sibling2, i, j)){
						Debug.Log(YP.GetValue(Sibling1) + " and her sibling, " + YP.GetValue(Sibling2) + ", are not on the best terms.");
						siblingRivalries++;
					}
				}
			}
		}
	}
	return siblingRivalries;
}

public static int allMurderous(){
	int murderers = 0;
	for (int i = 0; i < totalAgents; i++){
		Variable Murderer = new Variable();
		for (int j = 0; j < totalAgents; j++){
			if(i == j) continue;
			Variable Victim = new Variable();
			foreach (bool l1 in murderous(Murderer, Victim, i, j)){
				Debug.Log(YP.GetValue(Murderer) + " wants to get rid of " + YP.GetValue(Victim) + ".");
				murderers++;
			}
		}
	}
	return murderers;
}

public static int allBadMarriages(){
	int badMarriages = 0;
	for(int i = 0; i < totalAgents; i++){
		Variable Person = new Variable();
		for(int j = 0; j < totalAgents; j++){
			Variable Spouse = new Variable();
			foreach (bool l1 in married(Person, Spouse)){
				foreach (bool l3 in male(Person)){
				bool alreadyBad = false;
				foreach (bool l2 in hates(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " hates his wife, " + YP.GetValue(Spouse) + "!");
					badMarriages++;
					alreadyBad = true;
				}
				foreach (bool l2 in disgusted(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " is disgusted by his wife, " + YP.GetValue(Spouse) + "!");
					if(!alreadyBad){
						badMarriages++;
						alreadyBad = true;
					}
				}
				foreach (bool l2 in angry(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " is angry with his wife, " + YP.GetValue(Spouse) + "!");
					if(!alreadyBad){
						badMarriages++;
						alreadyBad = true;
					}
				}
				foreach (bool l2 in disrespects(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " is ashamed of his wife, " + YP.GetValue(Spouse) + "!");
					if(!alreadyBad){
						badMarriages++;
						alreadyBad = true;
					}
				}
				}
				foreach (bool l3 in female(Person)){
				bool alreadyBad = false;
				foreach (bool l2 in hates(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " hates her husband, " + YP.GetValue(Spouse) + "!");
					badMarriages++;
					alreadyBad = true;
				}
				foreach (bool l2 in disgusted(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " is disgusted by her husband, " + YP.GetValue(Spouse) + "!");
					if(!alreadyBad){
						badMarriages++;
						alreadyBad = true;
					}
				}
				foreach (bool l2 in angry(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " is angry with her husband, " + YP.GetValue(Spouse) + "!");
					if(!alreadyBad){
						badMarriages++;
						alreadyBad = true;
					}
				}
				foreach (bool l2 in disrespects(Person, Spouse, i, j)){
					Debug.Log(YP.GetValue(Person) + " is ashamed of her husband, " + YP.GetValue(Spouse) + "!");
					if(!alreadyBad){
						badMarriages++;
						alreadyBad = true;
					}
				}
				}
			}
		}
	}
	return badMarriages;
}

public static int allKillerMarriages(){
	int killerMarriages = 0;
	for(int i = 0; i < totalAgents; i++){
		Variable Killer = new Variable();
		for(int j = 0; j < totalAgents; j++){
			Variable Victim = new Variable();
			foreach(bool l1 in married(Killer, Victim)){
				foreach(bool l2 in murderous(Killer, Victim, i, j)){
					foreach(bool l3 in female(Killer)){
						Debug.Log(YP.GetValue(Killer) + " wants to end her relationship with " + YP.GetValue(Victim) +" in blood.");
						killerMarriages++;
					}
					foreach(bool l3 in male(Killer)){
						Debug.Log(YP.GetValue(Killer) + " wants to end his relationship with " + YP.GetValue(Victim) +" in blood.");
						killerMarriages++;
					}
				}
			}
		}
	}
	return killerMarriages;
}

public static int allBadFriends(){
	int badFriendPairs = 0;
	for (int i = 0; i < totalAgents; i++){
		Variable Person = new Variable();
		for (int j = 0; j < totalAgents; j++){
			if(i == j) continue;
			Variable Friend = new Variable();
			foreach (bool l1 in friend(Person, Friend, i, j)){
				//alreadyBad is here so that conflicts between the same two friends don't count twice for the overall number of conflicts
				foreach (bool l3 in male(Person)){
				bool alreadyBad = false;
				foreach (bool l2 in angry(Person, Friend, i, j)){
					Debug.Log(YP.GetValue(Person) + " is angry with his friend " + YP.GetValue(Friend) + "!");
					alreadyBad = true;
					badFriendPairs++;
				}
				foreach (bool l2 in envious(Person, Friend, i, j)){
					Debug.Log(YP.GetValue(Person) + " is jealous of his friend " + YP.GetValue(Friend) + "!");
					if(!alreadyBad) {
						badFriendPairs++;
						alreadyBad = true;
					}
				}
				foreach (bool l2 in hates(Person, Friend, i, j)){
					Debug.Log(YP.GetValue(Person) + " hates his friend " + YP.GetValue(Friend + "!"));
					if(!alreadyBad){
						badFriendPairs++;
						alreadyBad = true;
					}
				}
				}
				foreach (bool l3 in female(Person)){
				bool alreadyBad = false;
				foreach (bool l2 in angry(Person, Friend, i, j)){
					Debug.Log(YP.GetValue(Person) + " is angry with her friend " + YP.GetValue(Friend) + "!");
					alreadyBad = true;
					badFriendPairs++;
				}
				foreach (bool l2 in envious(Person, Friend, i, j)){
					Debug.Log(YP.GetValue(Person) + " is jealous of her friend " + YP.GetValue(Friend) + "!");
					if(!alreadyBad) {
						badFriendPairs++;
						alreadyBad = true;
					}
				}
				foreach (bool l2 in hates(Person, Friend, i, j)){
					Debug.Log(YP.GetValue(Person) + " hates her friend " + YP.GetValue(Friend + "!"));
					if(!alreadyBad){
						badFriendPairs++;
						alreadyBad = true;
					}
				}
				}
			}
		}
	}
	return badFriendPairs;
}

public static int allLoveTriangles(){
	int triangleNum = 0;
	for (int i = 0; i < totalAgents; i++){
		Variable Rival1 = new Variable();
		for (int j = 0; j < totalAgents; j ++){
			if(i == j) continue;
			Variable Rival2 = new Variable();
			for (int k = 0; k < totalAgents; k++){
				if(i == k || j == k) continue;
				Variable LoveInterest = new Variable();
				foreach (bool l1 in rivals(Rival1, Rival2, i, j)){
					foreach (bool l2 in feelingsFor(Rival1, LoveInterest, i, k)){
						foreach (bool l3 in feelingsFor(Rival2, LoveInterest, j, k)){
							Debug.Log(YP.GetValue(Rival1) + " and " + YP.GetValue(Rival2) + " both love " + YP.GetValue(LoveInterest) + "!");
							triangleNum++;
						}
					}
				}
			}
		}
	}
	return triangleNum;
}

public static int allFeelingsFor(){
	int loveInterests = 0;
	for(int i = 0; i < totalAgents; i++){
		Variable Person = new Variable();
		for(int j = 0; j < totalAgents; j++){
			if (i == j) continue;
			Variable Object = new Variable();
			foreach (bool l1 in feelingsFor(Person, Object, i, j)){
				//Debug.Log(YP.GetValue(Person) + " has feelings for " + YP.GetValue(Object) + ".");
				loveInterests++;
			}
		}
	}
	return loveInterests;
}

	//regenerates dispositions if RegenDisp is true
	public static void printOrRegenConflicts(float conflictBase, bool RegenDisp){
		float conflictNum = 0;
		bool keepLooping = true;
		while(conflictNum < conflictBase && keepLooping){
			if(RegenDisp) Dispositions.RegenDisp();
			else keepLooping = false;
			//conflictNum += allRivals() * .4F;
			conflictNum += allMurderous();
			//conflictNum += allBadFriends() * .4F;
			//conflictNum += allBadMarriages();
			//conflictNum += allLoveTriangles();
			//conflictNum += allFeelingsFor();
			conflictNum += allDepressed();
			//conflictNum += allKillerMarriages();
			conflictNum += siblingRivalries();
			if(conflictNum < conflictBase) conflictNum = 0;
		}
	}

	void Start () {
		Variable Person = new Variable();
		int num = 0;
		foreach (bool l1 in person(Person)){
			string name = Person.ToString();
			DYPAgent ag1 = new DYPAgent(name, num);
			Agents.Insert(num,ag1);
			num++;
		}	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
