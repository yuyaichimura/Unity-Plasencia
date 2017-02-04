using UnityEngine;
using POP;
using System.Collections;
/*
public class DYP_DEMO1 : MonoBehaviour {

	
	POP.ActionPlanner planner;
	POP.SmartCharacter Thief;
	POP.SmartCharacter Mark;
	POP.BearSpaceManager manager;
	
	void Start () {
		Thief = GameObject.Find("THIEF").GetComponent<POP.SmartCharacter>();
		Mark = GameObject.Find("MARK").GetComponent<POP.SmartCharacter>();
		planner = ActionPlanner.InstantiatePlanner();
		manager = GetComponent<BearSpaceManager>();
	}
	
	
	void Update () {*/
		/*if(Input.GetKeyDown(KeyCode.P)){
			manager.Init();
			Action start = new Initialize();
			start.ArgumentList = new POP.SmartObject[] {Mark};
			start.state = manager.setGlobalStateAfterAffordance(manager.GetGlobalState(), start);
			Action end = new StealEscape();
			end.ArgumentList = new POP.SmartObject[] {Thief};
			end.state = manager.setGlobalStateAfterAffordance((GlobalState) start.state, end);
			if (planner.StartPlanner(manager, start, end, end.GetGlobalPreconditions(manager))) {
				while (planner.plan.Count > 0) {
					Action action = (Action) planner.plan.Pop();
					action.Execute();
				}
			}
			BehaviorPlanner.executeTree();
		}*/
/*
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