using UnityEngine;
using TreeSharpPlus;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;

[AttributeUsage(AttributeTargets.Method)]
public class NPCAction : System.Attribute {
    
	public string Name { get; set; }
    
	public MethodInfo Method { get; set; }
    
	public NPCAction(string pName) {
        this.Name = pName;
    }

}

public class NPCAct : NPCController {

    #region Members
	private NPCController gController;
    protected delegate Node doEvent(Token t);
    private SmartCharacter gSmartCharacter;
    #endregion

    #region UnityUpdates
    void Awake() {
        this.gSmartCharacter = GetComponent<SmartCharacter>();
        this.gController = GetComponent<NPCController>();
    }
    #endregion
    
    public void Act() {

		bool execute = false;

		if(gController.PendingNPCEvent && gController.CurrentState.InEvent) {
			gController.PendingNPCEvent = false;
			if(!gController.Selected)
			ClearFogettableModifiers();
		} else if (gController.PendingNPCEvent) {
			if(gController.CandidateEvents.Count > 0) {
				// gController.CurrentNPCEvent = gController.CandidateEvents.Dequeue();
				// gController.CurrentNPCEvent.ExecuteEvent();
				execute = true;
				Debug.Log ("Attempting: "+gController.CandidateEvents.Peek().Name);
			}
		}

		if(!gController.CurrentState.InEvent && !gController.PendingNPCEvent) { // trigger if idle
			gController.CurrentNPCEvent = null;
			if(gController.CandidateEvents.Count > 0) {
				if (	gController.CurrentNPCEvent == null ||
				    gController.CurrentNPCEvent.Name != gController.CandidateEvents.Peek().Name) {
					execute = true;
					// gController.CurrentNPCEvent = gController.CandidateEvents.Dequeue();
					// gController.CurrentNPCEvent.ExecuteEvent();
				}
				
			}
		}

        try {
            // handle interruption here
            if (!execute) { // the character is not idle
                if (gController.CandidateEvents.Count > 0) {
                    NPCEvent e = gController.CandidateEvents.Peek();
                    if (e.Priority > gController.CurrentNPCEvent.Priority) {
                        Debug.Log("Interruption time baby");
                        execute = true;
                    }
                }
            }
        }
        catch (Exception e) {
            Debug.Log("Error while handling interruption of: " + this.gController);
        }

		if(execute) {
			gController.CurrentNPCEvent = gController.CandidateEvents.Dequeue();
			ClearFogettableModifiers();
            try {
                gController.GetComponent<CharacterMecanim>().HeadLookStop();
            }
            catch (Exception ex) { }
			gController.CurrentNPCEvent.ExecuteEvent();
		}
	}

	private void ClearFogettableModifiers() {
		
		if(gController.ForgetModifier.Count > 0) {
			foreach (KeyValuePair<NPCController, Dictionary<string,Modifier>> e in gController.ForgetModifier) {
				foreach(KeyValuePair<string,Modifier> m in e.Value) {
					if(gController.PerceivedTraits[e.Key].ContainsKey(m.Value.Name)) {
						gController.PerceivedTraits[e.Key].Remove(m.Value.Name);
						Debug.Log ("Removed modifier " + m.Value.Name);
					}
				}
			}
		}
		
		gController.ForgetModifier.Clear();
		
	}

    #region Implementation

    #region Affordances

    [NPCAction("Steal")]
    internal static void Steal(float pPriority, NPCController pChar, NPCController pTarg) {
        pChar = pChar.GetComponent<NPCController>();
        pTarg = pTarg.GetComponent<NPCController>();
        SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
        BehaviorEvent e = new BehaviorEvent(doEvent => pTarg.GetComponent<SmartCharacter>().Steal(chars[0]), chars);
        AttemptNPCEvent(pPriority, e, "Steal", (pChar as NPCController));
    }

    [NPCAction("MultiConverse")]
    internal static void MultiConverse(float pPriority, NPCController pChar, List<NPCEntity> pActors) {
        pChar = pChar.GetComponent<NPCController>();
        NPCController pTarg = pActors[0].GetComponent<NPCController>();
        NPCController pTarg2 = pActors[1].GetComponent<NPCController>();
        SmartCharacter mainChar = pTarg.Selected ? pTarg.GetComponent<SmartCharacter>() : pTarg2.GetComponent<SmartCharacter>();
        SmartCharacter secChar = pTarg.GetComponent<SmartCharacter>() == mainChar ? pTarg2.GetComponent<SmartCharacter>() : pTarg.GetComponent<SmartCharacter>();
        SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>(), secChar.GetComponent<SmartCharacter>() };
        BehaviorEvent e = new BehaviorEvent(doEvent => mainChar.ST_ConverseThree(chars[0], chars[1]), chars);
        AttemptNPCEvent(pPriority, e, "MultiConverse", (pChar as NPCController));
    }

    [NPCAction("DistractAndSteal")]
    internal static void DistractAndSteal(float pPriority, NPCController pChar, List<NPCEntity> pActors) {
        pChar = pChar.GetComponent<NPCController>();
        NPCController pTarg = pActors[0].GetComponent<NPCController>();
        NPCController pTarg2 = pActors[1].GetComponent<NPCController>();
        SmartCharacter mainChar = pTarg.Selected ? pTarg.GetComponent<SmartCharacter>() : pTarg2.GetComponent<SmartCharacter>();
        SmartCharacter secChar = pTarg.GetComponent<SmartCharacter>() == mainChar ? pTarg2.GetComponent<SmartCharacter>() : pTarg.GetComponent<SmartCharacter>();
        SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>(), secChar.GetComponent<SmartCharacter>() };
        BehaviorEvent e = new BehaviorEvent(doEvent => mainChar.ST_DistractAndSteal(chars[0], chars[1]), chars);
        AttemptNPCEvent(pPriority, e, "DistractAndSteal", (pChar as NPCController));
    }

	[NPCAction("Reprehend")]
	internal static void Reprehend(float pPriority, NPCController pChar, NPCController pTarg) {
		pChar = pChar.GetComponent<NPCController>();
		pTarg = pTarg.GetComponent<NPCController>();
		SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
		BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].Node_Reprehend(pTarg.GetComponent<SmartCharacter>()), chars);
		AttemptNPCEvent(pPriority, e, "Reprehend", (pChar as NPCController));
	}


	[NPCAction("DoGesture")]
	internal static void DoGesture(float pPriority, NPCController pChar, string pGestureName) {
		pChar = pChar.GetComponent<NPCController>();
		SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
		BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].Node_PlayHandGesture(pGestureName, 2000), chars);
		AttemptNPCEvent(pPriority, e, "DoGesture", pChar);
	}


	// overloaded for other gestures types
	[NPCAction("DoGenericGesture")]
	internal static void DoGesture(float pPriority, NPCController pChar, string pGestureName, AnimationLayer pLayer) {
		pChar = pChar.GetComponent<NPCController>();
		SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
		BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].GetComponent<BehaviorMecanim>().ST_PlayGesture(pGestureName, pLayer,2000), chars);
		AttemptNPCEvent(pPriority, e, "DoGesture", pChar);
	}

    // overloaded for other gestures types
    [NPCAction("WaveTo")]
    internal static void WaveTo(float pPriority, NPCController pChar, NPCController pTarg) {
        if (pTarg == null) return;
        pChar = pChar.GetComponent<NPCController>();
        pTarg = pTarg.GetComponent<NPCController>();
        SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
        BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].Node_WaveTo(pTarg.GetComponent<SmartCharacter>()), chars);
        AttemptNPCEvent(pPriority, e, "WaveTo", (pChar as NPCController));
    }

    // overloaded for other gestures types
    [NPCAction("LoserTo")]
    internal static void LoserTo(float pPriority, NPCController pChar, NPCController pTarg) {
        if (pTarg == null) return;
        pChar = pChar.GetComponent<NPCController>();
        pTarg = pTarg.GetComponent<NPCController>();
        SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
        BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].Node_LoserTo(pTarg.GetComponent<SmartCharacter>()), chars);
        AttemptNPCEvent(pPriority, e, "LoserTo", (pChar as NPCController));
    }

    // overloaded for other gestures types
    [NPCAction("ClapTo")]
    internal static void ClapTo(float pPriority, NPCController pChar, NPCController pTarg) {
        if (pTarg == null) return;
        pChar = pChar.GetComponent<NPCController>();
        pTarg = pTarg.GetComponent<NPCController>();
        SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
        BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].Node_Clap(pTarg.GetComponent<SmartCharacter>()), chars);
        AttemptNPCEvent(pPriority, e, "ClapTo", (pChar as NPCController));
    }

	[NPCAction("ConverseWith")]
	internal static void ConverseWith(float pPriority, NPCController pChar, NPCController pTarg) {
		pChar = pChar.GetComponent<NPCController> ();
		SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>(), pTarg.GetComponent<SmartCharacter>() };
		chars[0].WaypointFront = pTarg.transform; 
		BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].ST_TalkHappily(pTarg.GetComponent<SmartCharacter>()), chars);
		AttemptNPCEvent(pPriority, e, "ConverseWith", pChar);
	}

    [NPCAction("ConverseWithAlone")]
    internal static void ConverseWithAlone(float pPriority, NPCController pChar, NPCController pTarg) {
        pChar = pChar.GetComponent<NPCController>();
        SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
        chars[0].WaypointFront = pTarg.transform;
        BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].ST_ApproachAndConverse(pTarg.GetComponent<SmartCharacter>()), chars);
        AttemptNPCEvent(pPriority, e, "ConverseWith", pChar);
    }

    [NPCAction("PointTo")]
    internal static void PointTo(float pPriority, NPCController pChar, NPCController pTarg) {
        pChar = pChar.GetComponent<NPCController>();
        SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
        chars[0].WaypointFront = pTarg.transform;
        BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].Node_PointTo(pTarg.GetComponent<SmartCharacter>()), chars);
        AttemptNPCEvent(pPriority, e, "PointTo", pChar);
    }

    [NPCAction("GoTo")]
    internal static void GoTo(float pPriority, NPCController pChar, Vector3 pTarg) {
		pChar = pChar.GetComponent<NPCController>();
		float priority = 0f;
        if (pChar.CurrentState.InEvent) {
            pChar.CurrentState.CurrentEvent.StopEvent();
            priority = pChar.CurrentState.CurrentEvent.Priority + 1f;
        }
        Val<Vector3> val = Val.V<Vector3>(() => pTarg);
        SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
        BehaviorEvent e = new BehaviorEvent(doEvent => pChar.GetComponent<SmartCharacter>().Node_GoTo(val), chars);
		AttemptNPCEvent(pPriority, e, "GoTo", pChar);
    }

    [NPCAction("LookAtNPC")]
	internal static void LookAt(float pPriority, NPCController pChar, NPCController ptarg) {
		// make each method responsible for handling the parameter types of their PBTs
		ptarg = ptarg.GetComponent<NPCController> ();
		pChar = pChar.GetComponent<NPCController> ();
        Val<Vector3> val = Val.V<Vector3>(() => ptarg.Head.position);   
		SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
		BehaviorEvent e = new BehaviorEvent(doEvent => pChar.GetComponent<SmartCharacter>().Node_HeadLook(val), chars);
		AttemptNPCEvent(pPriority, e, "LookAtNPC", pChar);
    }

	[NPCAction("Wander")]
	internal static void Wander(float pPriority, NPCController pChar) {
		pChar = pChar.GetComponent<NPCController>();
		GameObject[] objs = GameObject.FindGameObjectsWithTag("Spot");
		if(objs.Length == 0) {
			// Debug.Log ("No waypoints defined for wandering around");
			return;
		}
		Vector3 v = Vector3.zero;
		bool found = false;
		// select wandering points randomly
		while(!found) {
			int index = UnityEngine.Random.Range (0, objs.Length);
			Transform t = objs[index].transform;
			if(Vector3.Distance(t.position, pChar.transform.position) > 7f) {
				v = t.position;
				found = true;
			}
		}
		v.y = 0;
		Val<Vector3> v1 = Val.V (() => v);
		Val<float> v2 = Val.V (() => 2f);
		SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
		BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].Node_GoToUpToRadius(v1,v2), chars);
		AttemptNPCEvent(pPriority, e,"Wander", pChar);
	}

	[NPCAction("ApproachEntity")]
	internal static void ApproachEntity(float pPriority, NPCController pChar, NPCEntity pTarg) {
		if (pChar.GetComponent<SmartObject> ().Behavior.PendingEvent != null)
			return;
		pTarg = pTarg.GetComponent<NPCEntity> ();
		pChar = pChar.GetComponent<NPCController> ();
		SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>(), pTarg.GetComponent<SmartCharacter>() };
		BehaviorEvent e = new BehaviorEvent(doEvent => pChar.GetComponent<SmartCharacter>().Node_ApproachUser(pTarg.GetComponent<SmartCharacter>()), chars);
		AttemptNPCEvent(pPriority, e, "ApproachEntity", pChar);
	}

	[NPCAction("OrientToPlayer")]
	internal static void OrientTo(float pPriority, NPCController pChar, NPCEntity pTarg) {
		pChar = pChar.GetComponent<NPCController>();
		Transform t = pTarg.GetComponent<NPCController>().Head.transform;
		Val<Vector3> v1 = Val.V (() => t.position);
		SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
		BehaviorEvent e = new BehaviorEvent(doEvent => pChar.GetComponent<SmartCharacter>().Node_OrientTowards(v1) , chars);
		AttemptNPCEvent(pPriority, e,"OrientToPlayer",pChar);
	}

    [NPCAction("GetScared")]
    internal static void GetSurprised(float pPriority, NPCController pChar, NPCEntity pTarg) {
        pChar = pChar.GetComponent<NPCController>();
        pTarg = pTarg.GetComponent<NPCController>();
        SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
        BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].Node_GetScared(pTarg.GetComponent<SmartCharacter>()), chars);
        AttemptNPCEvent(pPriority, e, "GetScared", (pChar as NPCController)); 
    }

    [NPCAction("GetTerrified")]
    internal static void GetTerrified(float pPriority, NPCController pChar, NPCEntity pTarg) {
        pChar = pChar.GetComponent<NPCController>();
        pTarg = pTarg.GetComponent<NPCController>();
        SmartCharacter[] chars = { pChar.GetComponent<SmartCharacter>() };
        BehaviorEvent e = new BehaviorEvent(doEvent => chars[0].Node_GetTerrified(pTarg.GetComponent<SmartCharacter>()), chars);
        AttemptNPCEvent(pPriority, e, "GetTerrified", (pChar as NPCController));
    }

    #endregion

	#region Utilities
	private static void AttemptNPCEvent(float pPriority, BehaviorEvent e, string pActionName, NPCController pChar) {
		if(pChar.Selected && pChar.CurrentState.InEvent) {
			pChar.CurrentEvent.StopEvent();
			pPriority = Math.Max(pChar.CurrentEvent.Priority + 0.01f, pPriority);
		}
		// Debug.Log (pChar + " is executing " + pActionName + " with priority: " + pPriority);
        try {
            pChar.GetComponent<CharacterMecanim>().HeadLookStop();
        }
        catch (Exception ex) { }
		e.StartEvent(pPriority);
		pChar.CurrentState.CurrentAction = NPCController.mAvailableActions[pActionName];
	}
	#endregion

    #endregion
}
