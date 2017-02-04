using UnityEngine;
using POP;
using System.Collections;
/*
public class DYP_DEMO2 : MonoBehaviour {

	POP.ActionPlanner planner;
		POP.SmartCharacter Cain;
		POP.SmartCharacter Abel; //needed for every character in the story
		POP.BearSpaceManager manager;
		GameObject char1;
		GameObject char2;

		void Start(){
			Cain = GameObject.Find("Cain").GetComponent<POP.SmartCharacter>();
			Abel = GameObject.Find("Abel").GetComponent<POP.SmartCharacter>();
			char1 = GameObject.Find("Cain");
			char2 = GameObject.Find("Abel");
			planner = ActionPlanner.InstantiatePlanner();
			manager = GetComponent<BearSpaceManager>();
		}

		//Run the tree by placing this in the Update() function tied to an if-statement that checks for keyboard input
		void Update(){
			
			if(Input.GetKeyDown(KeyCode.P)){
				BehaviorPlanner.startDemo2_Quiet(char1, char2);
			}else if(Input.GetKeyDown(KeyCode.O)){
				BehaviorPlanner.startDemo2_Sad(char1, char2);
			}else if(Input.GetKeyDown(KeyCode.I)){
				BehaviorPlanner.startDemo2_Bad(char1, char2);
			}else if(Input.GetKeyDown(KeyCode.U)){
				manager.Init();
				Action start = new Argue();
				start.ArgumentList = new POP.SmartObject[] {Cain, Abel};
				start.state = manager.setGlobalStateAfterAffordance(manager.GetGlobalState(), start);
				Action end = new END();
				end.ArgumentList = new POP.SmartObject[] {Cain, Abel};
				end.state = manager.setGlobalStateAfterAffordance((GlobalState) start.state, end);
				if (planner.StartPlanner(manager, start, end, end.GetGlobalPreconditions(manager))) {
					while (planner.plan.Count > 0) {
						Action action = (Action) planner.plan.Pop();
						action.Execute();
					}
				}
				BehaviorPlanner.executeTree();
			}

		//The following is great for Disposition and Truth debugging and regeneration:
			if(Input.GetKeyDown(KeyCode.R)){
				Truths.printOrRegenConflicts(1F, true);
			}
			else if(Input.GetKeyDown(KeyCode.Z)){
				Truths.allFriends();
				Truths.allEnemies();
				Dispositions.printDisposition("FRIEND_EM");
			}
			else if(Input.GetKeyDown(KeyCode.X)){
				Truths.allLoves();
				Truths.allHates();
				Dispositions.printDisposition("LOVE_HATE");
			}
			else if(Input.GetKeyDown(KeyCode.C)){
				Truths.allPleased();
				Truths.allAngry();
				Dispositions.printDisposition("PLS_ANG");
			}
			else if(Input.GetKeyDown(KeyCode.V)){
				Truths.allRespects();
				Truths.allDisrespects();
				Dispositions.printDisposition("RES_DIS");
			}
			else if(Input.GetKeyDown(KeyCode.B)){
				Truths.allSorry();
				Truths.allEnvy();
				Dispositions.printDisposition("SOR_ENV");
			}
			else if(Input.GetKeyDown(KeyCode.N)){
				Truths.allAttracted();
				Truths.allDisgusted();
				Dispositions.printDisposition("ATT_DISG");
			}
			else if(Input.GetKeyDown(KeyCode.Q)){
				Truths.printOrRegenConflicts(1F, false);
			}
		}
}
*/