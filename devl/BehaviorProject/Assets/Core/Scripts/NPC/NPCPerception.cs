using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class NPCPerception : NPCController {

    #region Members
    private SphereCollider gAreaOfSight;
	private NPCController gController;
	private Dictionary<NPCController, NPCEvent> gConsideredBehaviors;
    public float gFieldAngle = 135f;
    public Transform gHead;

	private static float HIGH_ROOF = 2f;
	private static float LOW_FLOOR = -2f;
    #endregion

    #region UnityStates
    void Awake() {
		gController = GetComponent<NPCController> ();
        gController.Head = gHead;
        base.gCurrentlyPerceived = new Dictionary<NPCEntity, NPCState>();
        gAreaOfSight = GetComponent<SphereCollider>();
		gConsideredBehaviors = new Dictionary<NPCController,NPCEvent>();
    }
    #endregion

    #region Implementation

    public void Perceive() {
        List<NPCEntity> toRemove = new List<NPCEntity>();
        foreach (KeyValuePair<NPCEntity, NPCState> entry in gCurrentlyPerceived) {
            if (!Visible(entry.Key.transform)) {
                toRemove.Add(entry.Key);
            }
            else {
				if((entry.Key as NPCController) != null) {
					NPCController npc = entry.Key as NPCController;
					if(npc.CurrentState.InEvent) {
						if(npc.CurrentNPCEvent != null) { // this might be cause because a TARGETED npc is in Event BUT NOT in NPCEvent
							if ((npc.CurrentNPCEvent.NPCEventType == NPCEventType.TARGETED && npc.InteractionTarget == (gController as NPCEntity))
							    || npc.CurrentNPCEvent.NPCEventType == NPCEventType.GLOBAL){

									if(!gConsideredBehaviors.ContainsKey(npc)) {

										// Debug.Log ("Considering "+npc+" behavior.");
										gConsideredBehaviors.Add(npc, npc.CurrentNPCEvent);

										/* APPLY MODIFIERS */ 
										Dictionary<string, Modifier> eventModifiers = npc.CurrentNPCEvent.Modifiers;

										if(eventModifiers.Count > 0) { // if not, not even bother

											Dictionary<string, List<double>> perceivedTraits;
											if(gController.PerceivedTraits.ContainsKey(npc)) {
												perceivedTraits = gController.PerceivedTraits[npc];
											} else {
												perceivedTraits = new Dictionary<string, List<double>>();
												gController.PerceivedTraits.Add (npc, perceivedTraits);
											}

											// now, add the modifiers
											foreach(KeyValuePair<string,Modifier> e in eventModifiers) {
												
												Modifier m = e.Value;
												
												
												if(!m.Persistent) {
													if(!gController.ForgetModifier.ContainsKey(npc)) {
														// Debug.Log("Added new npc and non-persistent trait: "+m.Name);
														gController.ForgetModifier.Add (npc, new Dictionary<string, Modifier>());
														gController.ForgetModifier[npc].Add (m.Name,m);
													} else {
														if(!gController.ForgetModifier[npc].ContainsKey(m.Name)) {
															// Debug.Log("For an existing NPC, added non-persistewnt trait: "+m.Name);
															gController.ForgetModifier[npc].Add (m.Name, m);
														}
													}
												}
												

												List<double> currentModList;
												if(!perceivedTraits.ContainsKey(m.Name)) {
													// Debug.Log (gController+" never felt: "+m.Name+" from " + npc + " adding modifier: "+m.Value);
													currentModList = new List<double>();
													perceivedTraits.Add (m.Name, currentModList);
													currentModList.Add(m.Value);
												} else {
													currentModList = perceivedTraits[m.Name];
													// TODO - the following if statement is redundant and should never trigger the else, but.. just in case.								
													if(currentModList.Count > 0) {
														double lastVal = currentModList[currentModList.Count-1];
														switch(m.ModifierType) {
														case ModifierType.INCREMENTOR:
															currentModList.Add ((m.Value + lastVal) < HIGH_ROOF ? (m.Value + lastVal) : HIGH_ROOF);
															break;
														case ModifierType.DECREMENTOR:
															currentModList.Add ((m.Value - lastVal) > LOW_FLOOR ? (m.Value - lastVal) : LOW_FLOOR);
															break;
														default:
															// TODO - complete this part... is this actually possible? Nah, NEUTRALIZERS are just for context for now
															// for now, just duplicate the last value for keeping record of interactions
															currentModList.Add(lastVal);
															break;
														}
													} else {
														currentModList.Add(m.Value); // this SHOULD NEVER happen
													}
													// Debug.Log (gController+" knows "+npc+" modifier name: "+m.Name+" with last value: "+currentModList[currentModList.Count - 1]);
												}

											}
										}

										/* APPLY MODIFIERS */
									}
								
							}
						}
					} else {
						if(gConsideredBehaviors.ContainsKey(npc)) {
							// Debug.Log (npc+" stopped doing: "+gConsideredBehaviors[npc].Name);
							// ClearFogettableModifiers();
							gConsideredBehaviors.Remove(npc);
						}
					}
				}
                Debug.DrawLine(gHead.position, entry.Key.BodyCenter, Color.green);
            }
        }
        foreach (NPCEntity e in toRemove)
            gCurrentlyPerceived.Remove(e);
        gController.CurrentlyPerceived = gCurrentlyPerceived;
        // gController.ConsideredBehaviors = gConsideredBehaviors;
    }

    #region Colliders
    void OnTriggerStay(Collider obj) {
        if (isPerceivable(obj.gameObject)) {
            NPCEntity entity = obj.gameObject.GetComponent<NPCEntity>();
            if (Visible(obj.transform) && !gCurrentlyPerceived.ContainsKey(entity)) {
                if(obj.GetComponent<NPCController>() != null)
                    gCurrentlyPerceived.Add(entity, entity.GetComponent<NPCController>().CurrentState);
            }
        }
	}

	void OnTriggerEnter(Collider obj) {
		if(obj.gameObject.tag == "Context") {
            if (NPCController.mAvailableContexts.ContainsKey(obj.gameObject.name))
			    gController.CurrentContext = NPCController.mAvailableContexts[obj.gameObject.name];
			// Debug.Log (gController + " is at: "+obj.gameObject.name);
		}
	}

	void OnTriggerExit(Collider obj) {
		if(obj.gameObject.tag == "Context") {
			gController.CurrentContext = null;
			// Debug.Log (gController + " exit from: "+obj.gameObject.name);
		}
	}
    #endregion

    #region Utilities

	private bool Visible(Transform pT) {
        if (InFieldOfView(pT) && Unblocked(pT)) {
            return true;
        }
        else {
            return false;
        }
	}

    private bool InFieldOfView(Transform pT) {
        Vector3 targDirection = pT.gameObject.GetComponent<NPCEntity>().BodyCenter - gHead.position;
        float targAngle = Vector3.Angle(targDirection, gHead.forward);
		bool inField = (targAngle <= (gFieldAngle * 0.5f) && Vector3.Distance(pT.position, transform.position) <= gAreaOfSight.radius);
		return inField;
    }

    private bool Unblocked(Transform pT) {
        Vector3 targDirection = pT.gameObject.GetComponent<NPCEntity>().BodyCenter - gHead.position;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(gHead.position, targDirection, out hit, gAreaOfSight.radius)) {
			if (hit.transform.gameObject == pT.gameObject) {
				return true;
			} else
				return false;
		} else
			return false;
    }

    private bool isPerceivable(GameObject o) {
        return o.GetComponent<NPCEntity>() != null;
    }

    #endregion

    #endregion
}
