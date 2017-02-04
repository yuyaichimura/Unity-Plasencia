using UnityEngine;
using System.Collections;
using YieldProlog;
using System.Collections.Generic;
using POP;

public class Ch_Mu_Demo : MonoBehaviour {

	POP.ActionPlanner planner;
	POP.SmartCharacter Christian;
	POP.SmartCharacter Muslim;
	//POP.SmartCharacter Wife;
	POP.BearSpaceManager manager;
	List<Agent> Agents = new List<Agent>();
	int totalAgents;
	static bool[] conflicts; //currently not used
	static List<Condition> startConditions = new List<Condition>();
	static List<Condition> endConditions = new List<Condition>();


	void Start () {
		/*conflicts = new bool[2];
		planner = ActionPlanner.InstantiatePlanner();
		Muslim = GameObject.Find ("Muslim 2").GetComponent<POP.SmartCharacter> ();
		Christian = GameObject.Find ("Christian 2").GetComponent<POP.SmartCharacter> ();
		//Wife = GameObject.Find("Muslim 1").GetComponent<POP.SmartCharacter>();
		manager = GetComponent<BearSpaceManager>();
		totalAgents = Dispositions.getMaxAgentNum();

		Variable Person = new Variable();
		int num = 0;
		foreach (bool l1 in Truths.person(Person)){
			string name = Person.ToString();
			Agent ag1 = new Agent(name, num);
			Agents.Insert(num,ag1);
			num++;
		}*/
	}

	public static List<Condition> getStartConditions(){
		return startConditions;
	}
	
	
	void Update () {
		 if (Input.GetKey(KeyCode.Escape)) Application.Quit();
			//CHRISTIAN STEALS FROM MUSLIM
			/*if (Input.GetKeyDown (KeyCode.P)) {
				Variable char1 = new Variable();
				Variable char2 = new Variable();
				foreach(bool l1 in YP.Unify(char1, "Christian 2")){
					foreach(bool l2 in YP.Unify(char2, "Muslim 2")){
						foreach(bool l3 in Truths.envious(char1, char2, 0, 1)){
							Debug.Log("Christian and Muslim are enemies.");
							// define start and end action and let the planner compute a plan for it.
							manager.Init();
							Action start = new BeginSteal();
							start.ArgumentList = new POP.SmartObject[] {Christian};
							start.state = manager.setGlobalStateAfterAffordance(manager.GetGlobalState(), start);
							Action end = new StealGift();
							end.ArgumentList = new POP.SmartObject[] {Christian, Muslim};
							end.state = manager.setGlobalStateAfterAffordance((GlobalState) start.state, end);
							if (planner.StartPlanner(manager, start, end, end.GetGlobalPreconditions(manager))) {
								while (planner.plan.Count > 0) {
									Action action = (Action) planner.plan.Pop();
									action.Execute();
								}
							}
							BehaviorPlanner.executeTree();
						}
					}
				}
			}
			//MUSLIM STEALS FROM CHRISTIAN
			/*if (Input.GetKeyDown (KeyCode.O)) {
				Variable char1 = new Variable();
				Variable char2 = new Variable();
				foreach(bool l1 in YP.Unify(char1, "Muslim 2")){
					foreach(bool l2 in YP.Unify(char2, "Christian 2")){
						foreach(bool l3 in Truths.envious(char1, char2, 1, 0)){
							Debug.Log("Christian and Muslim are enemies.");
							// define start and end action and let the planner compute a plan for it.
							manager.Init();
							Action start = new BeginSteal();
							start.ArgumentList = new POP.SmartObject[] {Christian};
							start.state = manager.setGlobalStateAfterAffordance(manager.GetGlobalState(), start);
							Action end = new StealGift();
							end.ArgumentList = new POP.SmartObject[] {Muslim, Christian};
							end.state = manager.setGlobalStateAfterAffordance((GlobalState) start.state, end);
							if (planner.StartPlanner(manager, start, end, end.GetGlobalPreconditions(manager))) {
								while (planner.plan.Count > 0) {
									Action action = (Action) planner.plan.Pop();
									action.Execute();
								}
							}
							BehaviorPlanner.executeTree();
						}
					}
				}
			}*/
			//CHRISTIAN KILLS MUSLIM
			/*if (Input.GetKeyDown(KeyCode.I)){
				Variable char1 = new Variable();
				Variable char2 = new Variable();
				foreach(bool l1 in YP.Unify(char1, "Christian 2")){
					foreach(bool l2 in YP.Unify(char2, "Muslim 2")){
						foreach(bool l3 in Truths.murderous(char1, char2, 0, 1)){
							Debug.Log("Christian and Muslim are enemies.");
							// define start and end action and let the planner compute a plan for it.
							manager.Init();
							Action start = new BeginSteal();
							start.ArgumentList = new POP.SmartObject[] {Christian};
							start.state = manager.setGlobalStateAfterAffordance(manager.GetGlobalState(), start);
							Action end = new Murder();
							end.ArgumentList = new POP.SmartObject[] {Christian, Muslim};
							end.state = manager.setGlobalStateAfterAffordance((GlobalState) start.state, end);
							if (planner.StartPlanner(manager, start, end, end.GetGlobalPreconditions(manager))) {
								while (planner.plan.Count > 0) {
									Action action = (Action) planner.plan.Pop();
									action.Execute();
								}
							}
							BehaviorPlanner.executeTree();
						}
					}
				}
			}
			//MUSLIM KILLS CHRISTIAN
			if (Input.GetKeyDown(KeyCode.U)){
				Variable char1 = new Variable();
				Variable char2 = new Variable();
				foreach(bool l1 in YP.Unify(char1, "Muslim 2")){
					foreach(bool l2 in YP.Unify(char2, "Christian 2")){
						foreach(bool l3 in Truths.murderous(char1, char2, 1, 0)){
							Debug.Log("Christian and Muslim are enemies.");
							// define start and end action and let the planner compute a plan for it.
							manager.Init();
							Action start = new BeginSteal();
							start.ArgumentList = new POP.SmartObject[] {Christian};
							start.state = manager.setGlobalStateAfterAffordance(manager.GetGlobalState(), start);
							Action end = new Murder();
							end.ArgumentList = new POP.SmartObject[] {Muslim, Christian};
							end.state = manager.setGlobalStateAfterAffordance((GlobalState) start.state, end);
							if (planner.StartPlanner(manager, start, end, end.GetGlobalPreconditions(manager))) {
								while (planner.plan.Count > 0) {
									Action action = (Action) planner.plan.Pop();
									action.Execute();
								}
							}
							BehaviorPlanner.executeTree();
						}
					}
				}
			}*/
			
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