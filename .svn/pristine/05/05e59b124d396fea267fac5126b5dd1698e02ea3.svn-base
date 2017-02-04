using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using System.Collections.Generic;
using System.Reflection;

public enum ModifierType {
	DECREMENTOR, NEUTRAL, INCREMENTOR
}

public class Modifier {

	public string Name { get; set; }
	public ModifierType ModifierType { get; set; }
	public float Value { get; set; }
	public bool gPersistent;

    public Modifier() { }

	public Modifier (string pName, ModifierType pType, float pVal) {
		this.Name = pName;
		this.ModifierType = pType;
		this.Value = pVal;
		this.gPersistent = true;
	}

	public Modifier (string pName, ModifierType pType, float pVal, bool pPersistent) : this(pName,pType,pVal) {
		this.gPersistent = pPersistent;
	}

	public bool Persistent {
		get {
			return this.gPersistent;
		}
		set {
			this.gPersistent = value;
		}
	}
}

public class Context {
	private Dictionary<string,Modifier> gModifiers;
	public string Name;
	public Dictionary<string,Modifier> Modifiers {
		get {
			return this.gModifiers;
		}
		set {
			this.gModifiers = value;
		}
	}
	public Context(string pName) : this() {
		// this.gModifiers = new Dictionary<string,Modifier>();
		this.Name = pName;
	}

    public Context() {
        this.gModifiers = new Dictionary<string, Modifier>();
    }

	public void AddModifier(Modifier pMod) {
		if(!gModifiers.ContainsKey(pMod.Name)) {
			gModifiers.Add (pMod.Name, pMod);
		}
	}
}

/// <summary>
/// NPC event type. Basically this should manage priorities
/// </summary>
public enum NPCEventPriority {
	Individual = 1,
	Social = 2,
	UserCommand = 3,
	SelfPreservation = 4
}

/// <summary>
/// NPC event type will indicate to a perceiving NPC whether this event should modify its perception of the source agent or not.
/// TARGETED: Modifiers will only apply to target agent
/// NEUTRAL: No Modifiers will be applied
/// BROADCASTED: All perceiving agents will update their perception about the source agent
/// </summary>
public enum NPCEventType {
	TARGETED, NEUTRAL, GLOBAL
}

/// <summary>
/// Wrapper class for a PBT. Will initially hold pre-conditions.
/// </summary>
public class NPCEvent {

    private List<NPCEntity> gNPCCharacters;
	private Dictionary<string,Condition> gSelfConditions;
	private Dictionary<string,Condition> gTargetedConditions;
	private float gPriority;
	private Dictionary<string,Modifier> gModifiers;
	private object[] gBehaviorArguments;
	private float gRandomness = 0f;
	private bool gRandomEvent = false;
    private int gParticipants = 2; // default number in event except specified otherwise.

	public Dictionary<string, Modifier> Modifiers {
		get {
			return this.gModifiers;
		}
	}

    public NPCEvent(string pName, NPCAction pAction, Dictionary<string,Condition> pConditions) {
        this.Name = pName;
        this.Action = pAction;
		this.gSelfConditions = new Dictionary<string,Condition> ();
		this.gTargetedConditions = new Dictionary<string,Condition> ();
		this.gModifiers = new Dictionary<string,Modifier>();
		foreach(KeyValuePair<string,Condition> c in pConditions) {
			if(c.Value.IsTargeted)
				gTargetedConditions.Add (c.Key, c.Value);
			else
                gSelfConditions.Add(c.Key, c.Value);
		}
		this.PreConditions = pConditions;

		// default values should not be used ideally
		this.gPriority = (float) NPCEventPriority.Individual;
		this.NPCEventType = NPCEventType.NEUTRAL;
    }

	public NPCEvent(string pName, NPCAction pAction, Dictionary<string,Condition> pConditions, NPCEventPriority pPriority, NPCEventType pEventType)
	: this (pName, pAction, pConditions) {
		this.Priority = (float) pPriority;
		this.NPCEventType = pEventType;
	}

	public NPCEvent(string pName, NPCAction pAction, Dictionary<string,Condition> pConditions, NPCEventPriority pPriority, NPCEventType pEventType, float pRandomness)
	: this (pName, pAction, pConditions, pPriority, pEventType) {
		this.gRandomness = pRandomness > 100f ? 100f : (pRandomness < 0f ? 0f : pRandomness);
        this.gRandomness = this.gRandomness / 1000f;
		this.gRandomEvent = true;
        // Debug.Log(this.Name + " is a random event with probability: " + this.gRandomness);
	}

	// Will affect an agents perceived traits of a source npc - for example, setting friendliness to 0.3f will make the target NPC to perceive increased friendliness
	public void AddModifier(Modifier pModifier) {
		if(!gModifiers.ContainsKey(pModifier.Name))	{
			gModifiers.Add(pModifier.Name, pModifier);
		}
	}

	#region Properties
	// for now to be used by a user command ... in reality the ExecuteEvent should deal with all required arguments

    public List<NPCEntity> Characters {
        get {
            return this.gNPCCharacters;
        }
        set {
            this.gNPCCharacters = value;
        }
    }

    public int Participants {
        get {
            return this.gParticipants;
        }
        set {
            this.gParticipants = value;
        }
    }

	public object[] BehaviorArguments { get; set; }
	public float Priority {
		get {
			return this.gPriority;
		}
		set {
			this.gPriority = (float) value;
		}
	}
	public NPCEventType NPCEventType { get; set; }
	public bool RandomEvent {
		get {
			return this.gRandomEvent;
		}
		set {
			this.gRandomEvent = value;
		}
	}
	public float Randomness {
		get {
			return this.gRandomness;
		}
		set {
			this.gRandomness= value;
		}
	}
    public NPCEntity Source { get; set; }
    public NPCEntity Target { get; set; }
    public Dictionary<string, Condition> PreConditions { get; set; }
	public Dictionary<string, Condition> SelfConditions {
		get {
			return this.gSelfConditions;
		}
	}
	public Dictionary<string, Condition> TargetedConditions {
		get {
			return this.gTargetedConditions;
		}
	}
    public NPCAction Action { get; set; }
    public string Name { get; set; }
	#endregion

    public void ExecuteEvent() {
        // reaching this points means the pre-conditions were met.
		object[] arguments = new object[Action.Method.GetParameters().Length]; // at least 1
		if(this.BehaviorArguments == null) { // this piece of ugly code should be decomissioned eventually for generic arguments
	        arguments[0] = Priority;
			arguments[1] = Source;
	        // if (arguments.Length > 2) arguments[2] = Target; // this should be generalized
            if (arguments.Length > 2) {
                if (this.Participants > 2) {
                    arguments[2] = this.Characters; // many characters passed in a N length list to the right action. This makes wrong assumptions though.
                }
                else {
                    arguments[2] = Target;
                }
            }
		} else {
			// this is only primed on user triggered events
			Priority = (float) this.BehaviorArguments[0];
			Source = (NPCEntity) this.BehaviorArguments[1];
			if(this.BehaviorArguments.Length > 2 && this.NPCEventType == NPCEventType.TARGETED) {
				if(this.BehaviorArguments[2] as NPCEntity != null)
					Target = (NPCEntity) this.BehaviorArguments[2];
			}
			arguments = this.BehaviorArguments;
		}

		// Source.CurrentState.CurrentNPCEvent = this; // set this event and the current NPCEvent
		Action.Method.Invoke(Source,arguments);

		// only possible if this is a user triggered event
		NPCController npc = this.Source.GetComponent<NPCController>();

		// pass event's state onto NPC for persistency
		if(npc != null)	{ 
			/*
			if(!npc.Selected)
				Debug.Log ("Blocking "+npc+" for events");
			*/
			npc.CurrentNPCEvent = this;
			npc.PendingNPCEvent = true;
		}
		if(Target != null) npc.InteractionTarget = Target as NPCEntity;

		// clear event's state
		this.Source = null;
		this.Target = null;
		this.BehaviorArguments = null;
    }
}

public class NPCDeliberation : NPCController {

    #region Members
    private NPCController gController;
	private Queue<NPCEvent> gCandidateEvents;
    #endregion

    #region UnityUpdates
    void Awake() {
		gCandidateEvents = new Queue<NPCEvent>();
        gController = GetComponent<NPCController>();
    }
    #endregion

    #region Implementation
    public void Deliberate() {

		gCandidateEvents.Clear();
		if (!this.gController.Selected && !gController.SkipDeliberationTesting) { // Skip is just for testing and blocking event
			bool firstEvent = true;
            List<NPCEvent> events = new List<NPCEvent>();
            List<NPCEvent> candidateEvents = new List<NPCEvent>();
            foreach (KeyValuePair<string, NPCEvent> entry in mAvailableEvents) {
                if (ValidateConditions(entry.Value)) {
					NPCEvent e = entry.Value;
					if(firstEvent) { // first event, shove it in
						candidateEvents.Add(entry.Value);
						firstEvent = false;
					} else {
						if(e.Priority > candidateEvents[candidateEvents.Count - 1].Priority) { // check if this is the highes priority event yet
							candidateEvents.Add(e);
						} else if (candidateEvents[candidateEvents.Count - 1].Priority > e.Priority) { // put it right behind the first even with higher priority
							int i = 0;
							foreach(NPCEvent tmp in candidateEvents) {
								if(candidateEvents[i].Priority > e.Priority) {
									candidateEvents.Insert(i,e);
									break;
								}
							}
						} else {
							// DEATHMATCH - both top events have the same priority. Refine the criteria
							NPCEvent curTopEvent = candidateEvents[candidateEvents.Count -1];
							float curPriority = e.Priority + (e.PreConditions.Count / 100f);
							float topPriority = e.Priority + (curTopEvent.PreConditions.Count / 100f);
							if(curPriority != topPriority) {
								if(curPriority > topPriority) {
									candidateEvents.Add (e);
								} else {
									candidateEvents.Insert(candidateEvents.Count - 1, e); // right before the top, and shift forward
								}
							} else {
								// As per the proposal, pick one randomly, the implement the constranints matcher
								float result = UnityEngine.Random.Range(0f, 1f);
								if(result > 0.5f) {
									candidateEvents.Add (e);
								} else candidateEvents.Insert(candidateEvents.Count - 1, e);
							}
						}
					}

					// gCandidateEvents.Enqueue(entry.Value);
                }
            }

			if(candidateEvents.Count >= 1) {
				gCandidateEvents.Enqueue(candidateEvents[candidateEvents.Count - 1]); // enque the latest greatest
			}
        }
		gController.CandidateEvents = gCandidateEvents;
    }



    #region Utility
    
    private bool ValidateConditions(NPCEvent e) {

        if (e.RandomEvent) {
            float val = UnityEngine.Random.Range(0f, 1f);
            if (val > e.Randomness) {
                return false;
            }
        }

		// consider context modifiers, if any
		Dictionary<string,Modifier> contextModifiers = gController.CurrentContext != null ? gController.CurrentContext.Modifiers : new Dictionary<string,Modifier>(); // for a much cleaner way that checking for null
        Dictionary<string,Condition> satisfiedConditions = new Dictionary<string,Condition>();

		// Ignore all events which are of NPCEventType.UserCommand
		if(e.Priority == (float) NPCEventPriority.UserCommand) return false;

        List<NPCEntity> candidates = new List<NPCEntity>();
        
		// the following implementation is SO STUPID it hurts but it will stay for now. (quick and dirty, right :D ).
        foreach (KeyValuePair<string, Condition> currCondition in e.SelfConditions) {
            Condition c = currCondition.Value;
			string condName = c.Name.Contains(Condition.SELF_IDENTIFIER) ? c.Name.Substring(0,c.Name.IndexOf(Condition.SELF_IDENTIFIER)) : c.Name;
			if(c.Matching) {
				condName = Condition.MATCH_IDENTIFIER;
				gController.Conditions[condName].Trait = c.Trait;
				gController.Conditions[condName].Target = c.Target;
			}
			if (gController.Conditions[condName].CompareTo(c) != 0) {
                if (c.Mandatory)
                    return false;
                else
                    continue;
            }
            else {
                if (c.NestedConditions.Count > 0) { // in theory, this shouldn't be used for self evaluated conditions, but whatever, yet another design option i guess.
                    bool complies = false;
                    // if there are any nested conditions, evaluate them
                    foreach (Condition curCond in c.NestedConditions) {
                        if (curCond.IsTargeted) {
                            // handle no available targets
                            foreach (KeyValuePair<NPCEntity, NPCState> entity in gController.CurrentlyPerceived) {
                                NPCController npc = entity.Key.GetComponent<NPCController>();
                                if (npc != null) {
                                    if (!CharCompliesCondition(npc, curCond, contextModifiers)) {
                                        complies = false;
                                        break;
                                    }
                                    else {
                                        complies = true;
                                        candidates.Add(npc);
                                    }
                                }
                            }
                            if (!complies) return false;
                        }
                        else {
                            // Debug.Log("Evaluating nested condition: " + curCond.Name);
                            if (!CharCompliesCondition(gController, curCond, contextModifiers)) {
                                return false;
                            }
                        }
                    }
                }
            }

            // add a satisfied conditions
            satisfiedConditions.Add(currCondition.Key, currCondition.Value);
        }

		if (e.TargetedConditions.Count > 0) {
			if(gController.CurrentlyPerceived.Count > 0) {
				// for each NPC
				foreach (KeyValuePair<NPCEntity, NPCState> entity in gController.CurrentlyPerceived) {
					bool complies = false;
                    NPCController curNpc = entity.Key as NPCController;

                    if (entity.Key as NPCController != null) {

						// for each targeted condition
						foreach(KeyValuePair<string,Condition> c in e.TargetedConditions) {

							bool clearTarget = false;

							Condition curCond = c.Value;

							// if this is a trait targeted condition, prime it up on the entity baby. < this allows the CompareTo method to get all the params while invoking the functions pointers.

							// if this is a matchable condition, the trait name is embedded in the Condition so just call the right Propoerty - I feel so dirty right now.
							string traitKey = curCond.Name;
							if(curCond.Matching) {
								traitKey = Condition.MATCH_IDENTIFIER;
							}
							Condition entityCondition = entity.Value.Conditions [traitKey];

							// stuff the condition object on the fly
							/* **** */
							if(curCond.Trait != null) entityCondition.Trait = curCond.Trait;
							if(curCond.Target != float.NaN) entityCondition.Target = curCond.Target;
							if(curCond.TargetValue == null) { // do not define gTargetValue if condition is on NPC < really stupid
								curCond.TargetValue = this.GetComponent<NPCController>(); // iof the conditions requires the entity, add it
								clearTarget = true;
							}
							if(gController.PerceivedTraits.ContainsKey(curNpc)) {
								if(curCond.Trait != null) {
									if(gController.PerceivedTraits[curNpc].ContainsKey(curCond.Trait)) { // this should always be true because this is set when an event is perceived
										Modifier traitMod = null;
										List<double> history = gController.PerceivedTraits[curNpc][curCond.Trait];
										float curMod = (float) history[history.Count - 1];
										if(contextModifiers.ContainsKey (curCond.Trait)) {
											traitMod = contextModifiers[curCond.Trait];
											switch(traitMod.ModifierType) {
											case ModifierType.INCREMENTOR:
												curMod += traitMod.Value;
												break;
											case ModifierType.DECREMENTOR:
												curMod -= traitMod.Value;
												break;
											case ModifierType.NEUTRAL:
												curMod = traitMod.Value;
												break;
											}
										}
										entity.Value.Conditions[traitKey].TraitModifier = new Modifier(curCond.Trait, ModifierType.INCREMENTOR, curMod);
									}
								}
							}
							/* **** */

							// has the condition?
							if (entity.Value.Conditions [traitKey].CompareTo (c.Value) != 0) {
								// candidate target for condition, add it to the candidates list
                                if (c.Value.Mandatory)
                                    complies = false;
                                else
                                    continue; // if it not satisfied, but is not mandatory, just skip it.
							} else {
								complies = true;
                                // are there any nested conditions to consider?
                                if (curCond.NestedConditions.Count > 0) {
                                    /* Evaluate nested conditions here */
                                    foreach (Condition subC in curCond.NestedConditions) { // TODO - TEST THIS ASAP
                                        if (!subC.IsTargeted) { // this is getting so comboluted it hurts.
                                            if (!CharCompliesCondition(gController, subC, contextModifiers)) {
                                                complies = false; // change complies back to false since nested conditions are not satisfied but the root one. < man this is getting really sticky.
                                                break;
                                            }
                                        }
                                        else {
                                            // Debug.Log("Evaluating nested condition " + subC.Name);
                                            if (!CharCompliesCondition(entity.Value, subC, contextModifiers)) {
                                                complies = false; // change complies back to false since nested conditions are not satisfied but the root one. < man this is getting really sticky.
                                                break;
                                            }
                                            // otherwise, keep goning and leave complies alone as true for adding the character to the event's candidates list
                                        }
                                    }
                                }
							}
							entity.Value.Conditions[traitKey].TraitModifier = null; // clean up the current modifier
							if(clearTarget) curCond.TargetValue = null;
                            else if (complies) {
                                // add a satisfied conditions
                                if(!satisfiedConditions.ContainsKey(c.Key))
                                    satisfiedConditions.Add(c.Key, c.Value);
                            }
						}
					}
                    if (complies) {
                        candidates.Add(curNpc);
                    }
				}
			}
			if (candidates.Count < e.Participants - 1) { // if there are targeted conditions, make sure there are enough participants.
				// Debug.Log ("Agent: "+gController+" NOT valid event: "+e.Name);
				return false; 
			}
		}
        
		// if all the conditions are valid, set the Source of the event as this NPC
        e.Source = this as NPCEntity;

        // lets assume up to one target for now.
        if (candidates.Count == 1) {
            e.Target = candidates[0]; // this should be the foreground character, try to put him first... for convention, whatever.
        }
        else if (candidates.Count > 1) {
            e.Target = candidates[0];
            e.Characters = candidates;
        }

        foreach(KeyValuePair<string,Condition> c in e.PreConditions) {
            if (!satisfiedConditions.ContainsKey(c.Key)) return false;
        }

		return true;
    }

    private bool CharCompliesCondition(NPCController pNPC, Condition pCond, Dictionary<string, Modifier> contextModifiers) {
        //return true;

        pNPC = pNPC.GetComponent<NPCController>();

        bool clearTarget = false;
        bool complies = false;
        Condition curCond = pCond;

        // if this is a trait targeted condition, prime it up on the entity baby. < this allows the CompareTo method to get all the params while invoking the functions pointers.

        // if this is a matchable condition, the trait name is embedded in the Condition so just call the right Propoerty - I feel so dirty right now.
        string traitKey = curCond.Name;
        if (curCond.Matching) {
            traitKey = Condition.MATCH_IDENTIFIER;
        }
        Condition entityCondition = pNPC.Conditions[traitKey];

        // stuff the condition object on the fly
        /* **** */
        if (curCond.Trait != null) entityCondition.Trait = curCond.Trait;
        if (curCond.Target != float.NaN) entityCondition.Target = curCond.Target;
        if (curCond.TargetValue == null) { // do not define gTargetValue if condition is on NPC < really stupid
            curCond.TargetValue = this.GetComponent<NPCController>(); // iof the conditions requires the entity, add it
            clearTarget = true;
        }
        if (gController.PerceivedTraits.ContainsKey(pNPC)) {
            if (curCond.Trait != null) {
                if (gController.PerceivedTraits[pNPC].ContainsKey(curCond.Trait)) { // this should always be true because this is set when an event is perceived
                    Modifier traitMod = null;
                    List<double> history = gController.PerceivedTraits[pNPC][curCond.Trait];
                    float curMod = (float)history[history.Count - 1];
                    if (contextModifiers.ContainsKey(curCond.Trait)) {
                        traitMod = contextModifiers[curCond.Trait];
                        switch (traitMod.ModifierType) {
                            case ModifierType.INCREMENTOR:
                                curMod += traitMod.Value;
                                break;
                            case ModifierType.DECREMENTOR:
                                curMod -= traitMod.Value;
                                break;
                            case ModifierType.NEUTRAL:
                                curMod = traitMod.Value;
                                break;
                        }
                    }
                    pNPC.Conditions[traitKey].TraitModifier = new Modifier(curCond.Trait, ModifierType.INCREMENTOR, curMod);
                }
            }
        }
        /* **** */

        // has the condition?
        if (pNPC.Conditions[traitKey].CompareTo(curCond) != 0) {
            // candidate target for condition, add it to the candidates list
            complies = false;
        }
        else {
            complies = true;
            // are there any nested conditions to consider?
            if (curCond.NestedConditions.Count > 0) {
                foreach (Condition subC in curCond.NestedConditions) {
                    if (subC.IsTargeted) {
                        complies = false;
                        // handle no available targets
                        foreach (KeyValuePair<NPCEntity, NPCState> entity in pNPC.CurrentlyPerceived) {
                            NPCController npc = entity.Key.GetComponent<NPCController>();
                            if (npc != null) {
                                if (!CharCompliesCondition(npc, subC, contextModifiers)) {
                                    complies = false;
                                    break;
                                }
                                else complies = true;
                            }
                        }
                        if (!complies) break; // stop the loop should a nested condition is false
                    }
                    else {
                        if (!CharCompliesCondition(pNPC, subC, contextModifiers)) { // recursive call for further nested conditions.... be carefull with this. It might be a terrible idea to allow this but otherwise, would be restrictive.
                            complies = false; // change complies back to false since nested conditions are not satisfied but the root one. < man this is getting really sticky.
                            break;
                        }
                        // otherwise, keep goning and leave complies alone as true for adding the character to the event's candidates list
                        
                    }
                }
            }
        }
        pNPC.Conditions[traitKey].TraitModifier = null; // clean up the current modifier
        if (clearTarget) curCond.TargetValue = null;
        if (!complies && curCond.Mandatory) return false;
        else return true;
    }

	#endregion



    #endregion
}
