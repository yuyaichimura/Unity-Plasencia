using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using System.Collections.Generic;
using System.Reflection;

public class NPCController : NPCEntity {

    #region Members

    /// <summary>
    /// Only created and exposed to Unity with the sole purpose of testing it by setting it up as a false conditon in an event.
    /// </summary>
	public bool gTEST_NPC;
    public bool gSkipDeliberation = false;
    internal string gReligion = "undefined";

	private bool gSelected = false;
    private static int mNPCIdentifier = 1;

	public static Dictionary<string, Modifier> mAvailableModifiers = new Dictionary<string,Modifier>();
	public static Dictionary<string, Context> mAvailableContexts = new Dictionary<string,Context>();
	public static Dictionary<string, NPCAction> mAvailableActions = new Dictionary<string, NPCAction>();
	public static Dictionary<string, NPCEvent> mAvailableEvents = new Dictionary<string, NPCEvent>();
    public static Dictionary<string, Condition> mAvailableConditions = new Dictionary<string, Condition>();

	protected Dictionary<NPCController,Dictionary<string,List<double>>> gPerceivedTraits;

	protected Context gContext;
	protected Queue<NPCEvent> gCandidateEvents;
    protected int gNPCId;
    protected Vector3 gCenter;
    protected Entity gEntityType;
    protected UnityEngine.AI.NavMeshAgent gNavAgent;
    protected NPCPerception gPerception;
    protected NPCDeliberation gDeliberation;
    protected NPCAct gAct;
    protected Light gSelectHighlight;   
    protected NPCState gCurrentState;
    protected IEnumerable<string> gAffordances;
    protected Dictionary<string, NPCEvent> gEvents;
    protected Dictionary<NPCEntity, NPCState> gCurrentlyPerceived;
    protected Dictionary<string, Condition> gConditions;
    protected Dictionary<string, Trait> gTraits;
	protected NPCEntity gInteractionTarget;
	protected NPCEvent gCurrentNPCEvent;
	protected bool gPendingNPCEvent;
	protected Dictionary<NPCController, Dictionary<string,Modifier>> gForgetModifier;
    // protected Dictionary<NPCController, NPCEvent> gConsideredBehaviors = new Dictionary<NPCController,NPCEvent>();

    #endregion

    #region Static
    static NPCController() {
        RegisterActions();		// All possible actions given by behavior trees
        NPCUtils.LoadBehaviors(); // shoudl any behaviors are defined in XML, suck them up from here first, then proceed to the traditional v0.0 code method.
        InitializeEvents();		// All events to be performed, these encapsulate actions, among other things
		InitializeModifiers();	// How an action will modify an NPC when performed - given by conditions, for now
		InitializeContexts();
    }
    #endregion

    #region UnityUpdates
    void Start() {
		gForgetModifier = new Dictionary<NPCController, Dictionary<string,Modifier>>();
		gPerceivedTraits = new Dictionary<NPCController, Dictionary<string, List<double>>>();
		gCandidateEvents = new Queue<NPCEvent>();
        gNPCId = mNPCIdentifier;
        NPCController.mNPCIdentifier += 1;
        gCenter = GetComponentInChildren<CapsuleCollider>().center;
        gEntityType = Entity.CHARACTER;
        gSelectHighlight = GetComponentInChildren<Light>();
        gSelectHighlight.gameObject.SetActive(false);
        gCurrentState = GetComponent<NPCState>();
        gNavAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        gPerception = GetComponent<NPCPerception>();
        gDeliberation = GetComponent<NPCDeliberation>();
        gAct = GetComponent<NPCAct>();
		gCurrentNPCEvent = null;
        SetReligion();
    }

    void LateUpdate() {
        this.gPerception.Perceive();
        this.gDeliberation.Deliberate();
        this.gAct.Act();
    }
    #endregion

    #region Properties

    /*
    public Dictionary<NPCController, NPCEvent> ConsideredBehaviors {
        get {
            return this.gConsideredBehaviors;
        }
        set {
            this.gConsideredBehaviors = value;
        }
    }
    */
    
    public bool SkipDeliberationTesting {
        get {
            return this.gSkipDeliberation;
        }
    }

    public long Id {
		get {
			return this.gNPCId;
		}
	}

    public Transform Head { get; set; }

    public override Vector3 BodyCenter {
        get {
            return this.transform.position + (gCenter * transform.localScale.y);
        }
    }

	public Dictionary<NPCController, Dictionary<string,Modifier>> ForgetModifier {
		get {
			return this.gForgetModifier;
		}
		set {
			this.gForgetModifier = value;
		}
	}

    public override Entity EntityType {
        get {
            return gEntityType;
        }
    }

    public override string Name {
        get {
            return "Agent: " + gNPCId;
        }
    }

    public override NPCState CurrentState {
        get {
            return this.gCurrentState;
        }
    }

    public bool Selected {
        get {
            return gSelectHighlight.gameObject.activeSelf;
        }
        set {
            this.gSelected = value;
        }
    }

    public Dictionary<NPCEntity, NPCState> CurrentlyPerceived {
        get {
            return this.gCurrentlyPerceived;
        }
        set {
            this.gCurrentlyPerceived = value;
        }
    }

    public Dictionary<string, Condition> Conditions {
        get {
            return this.gConditions;
        }
        set {
            this.gConditions = value;
        }
    }

    public Dictionary<string, Trait> Traits {
        get {
            return this.gTraits;
        }
        set {
            this.gTraits = value;
        }
    }

    public BehaviorEvent CurrentEvent {
		get {
			return this.GetComponent<SmartObject>().Behavior.CurrentEvent;
		}
	}

	public NPCEntity InteractionTarget {
		get {
			return gInteractionTarget;
		}
		set {
			this.gInteractionTarget = value;
		}
	}

	public Queue<NPCEvent> CandidateEvents {
		get {
			return this.gCandidateEvents;
		}
		set {
			gCandidateEvents = value;
		}
	}

	public NPCEvent CurrentNPCEvent {
		get {
			return this.gCurrentNPCEvent;
		}
		set {
			this.gCurrentNPCEvent = value;
		}
	}

	public BehaviorStatus BehaviorStatus {
		get {
			return this.GetComponent<SmartObject>().Behavior.Status; // Running == Idle OR InEvent
		}
	}

	public Dictionary<NPCController,Dictionary<string,List<double>>> PerceivedTraits { 
		get {
			return this.gPerceivedTraits;
		}
	}

	public Context CurrentContext { 
		get {
			return this.gContext;
		}
		set {
			this.gContext = value;
		}
	}

	public bool PendingNPCEvent { 
		get {
			return this.gPendingNPCEvent;
		}
		set {
			this.gPendingNPCEvent = value;
		}
	}

	public bool TEST_NPC {
		get {
			return this.gTEST_NPC;
		}
	}

    public string Religion {
        get {
            return this.gReligion;
        }
    }

    #endregion

    #region Interface
    
	#region UserCommandedActions

	public void WalkTo(Vector3 pTarg) {
		gInteractionTarget = null;
		NPCEvent e = NPCController.mAvailableEvents["USER-GoTo"];
		object[] args = { e.Priority, this, pTarg };
		e.BehaviorArguments = args;
		e.ExecuteEvent();
    }

	public void ApproachEntity(NPCEntity entity) {
		gInteractionTarget = entity;
		NPCEvent e = NPCController.mAvailableEvents["USER-ApproachEntity"];
		object[] args = { e.Priority, this, entity };
		e.BehaviorArguments = args;
		e.ExecuteEvent();
	}

	public void WaveTo(NPCEntity pTarg) {
		if(pTarg != null && pTarg.GetComponent<NPCController>() != null) {
			gInteractionTarget = pTarg.GetComponent<NPCController>();	
		} else gInteractionTarget = this;
		NPCEvent e = NPCController.mAvailableEvents["USER-WaveTo"];
		object[] args = { e.Priority, this, pTarg };
		e.BehaviorArguments = args;
		e.ExecuteEvent();
	}

	public void Dismiss(NPCEntity pTarg) {
		if(pTarg != null && pTarg.GetComponent<NPCController>() != null) {
			gInteractionTarget = pTarg.GetComponent<NPCController>();	
		} else gInteractionTarget = this;
		NPCEvent e = NPCController.mAvailableEvents["USER-Dismiss"];
		object[] args = { e.Priority, this, "Dismiss" };
		e.BehaviorArguments = args;
		e.ExecuteEvent();
	}

	public void ConverseWith(NPCController pTarg) {
		NPCEvent e = NPCController.mAvailableEvents["USER-ConverseWith"];
		object[] args = { e.Priority, this, pTarg };
		e.BehaviorArguments = args;
		e.ExecuteEvent();
	}

	public void Yawn() {
		NPCEvent e = NPCController.mAvailableEvents["USER-Yawn"];
		object[] args = { e.Priority, this, "Yawn" };
		e.BehaviorArguments = args;
		e.ExecuteEvent();
	}

    public void Roar() {
        NPCEvent e = NPCController.mAvailableEvents["USER-Roar"];
        object[] args = { e.Priority, this, "Roar" };
        e.BehaviorArguments = args;
        e.ExecuteEvent();
    }

    public void CallForAttention() {
        NPCEvent e = NPCController.mAvailableEvents["USER-CallForAttention"];
        object[] args = { e.Priority, this, "Wave" };
        e.BehaviorArguments = args;
        e.ExecuteEvent(); 
    }

    public void Steal(NPCController pTarg) {
		NPCEvent e = NPCController.mAvailableEvents["USER-Steal"];
		object[] args = { e.Priority, this, pTarg };
		e.BehaviorArguments = args;
		e.ExecuteEvent();
	}

	#endregion

    public void SetSelected(bool pSelected) {
        this.Selected = pSelected;
        gSelectHighlight.gameObject.SetActive(pSelected);
    }
    #endregion

    #region Utilities

    private static void RegisterActions() {
        foreach (MethodInfo m in typeof(NPCAct).GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance)) {
            NPCAction a = null;
            foreach (object o in m.GetCustomAttributes(true)) {
                if (o as NPCAction != null) {
                    a = o as NPCAction;
                    break;
                }
            }
            if (a != null) {
                // register available action - this should be automated by reading canvas affordances but canvas is very limited so we could build our own trees
                a.Method = m;
                mAvailableActions.Add(a.Name, a);
            }
        }
    }

    private void SetReligion() {
        if (this.gameObject.name.Contains("Christian")) {
            this.gReligion = "christian";
        }
        else if (this.gameObject.name.Contains("Jew")) {
            this.gReligion = "jew";
        }
        else if (this.gameObject.name.Contains("Muslim")) {
            this.gReligion = "muslim";
        }
    }

    #endregion

    #region Events

    private static void InitializeModifiers() {
        // TODO - potentially for static modifiers. I can't think it as a good design option right now.
    }


    private static void InitializeEvents() {

        

        /* Init test event - manually created  */
        /*
         string aName;
         NPCAction aAction; // <<< BEHAVIOR !!!!
         Dictionary<string, Condition> aConditions = new Dictionary<string, Condition>();
         NPCEvent e;
        
        /* Initially, I will start replacing the code defined conditions with, if available, the same one defined on the XML file. */
        /*
        Condition cB;
        if (NPCController.mAvailableConditions.ContainsKey("IsForeground")) {
            cB = mAvailableConditions["IsForeground"];
        } else cB = new Condition("IsForeground", typeof(bool), true, true); 	// if the targeted NPC a foreground character  

        Condition cATarg;
        if (NPCController.mAvailableConditions.ContainsKey("InEvent")) {
            cATarg = mAvailableConditions["InEvent"];
        } else cATarg = new Condition("InEvent", typeof(bool), false, true);

        Condition cC;
        if (NPCController.mAvailableConditions.ContainsKey("Friendliness")) {
            cC = mAvailableConditions["Friendliness"];
        } else cC = new Condition(Condition.MATCH_IDENTIFIER + "-Friendliness", typeof(float), "Friendliness", 0.5f, true);	// does the targeted NPC has a trait of at least N?

        Condition cD;
        if (NPCController.mAvailableConditions.ContainsKey("Target")) {
            cD = mAvailableConditions["Target"];
        } else cD = new Condition("Target", typeof(NPCController), null, true); // is the npc targetting me?

        Condition cA;
        if (NPCController.mAvailableConditions.ContainsKey("InEventSelf")) {
            cA = mAvailableConditions["InEventSelf"];
        } else cA = new Condition("InEvent", typeof(bool), false);

        Condition testNPC; 
        if (NPCController.mAvailableConditions.ContainsKey("TestNPC")) {
            testNPC = mAvailableConditions["TestNPC"];
        } else testNPC = new Condition("TestNPC", typeof(bool), false);

        Condition cE;
        if (mAvailableConditions.ContainsKey("Rude")) {
            cE = mAvailableConditions["Rude"];
        } else cE = new Condition(Condition.MATCH_IDENTIFIER + "-Rude", typeof(float), "Rude", 1f, true);

        Condition cF;
        if(mAvailableConditions.ContainsKey("Attractiveness")) {
            cF = mAvailableConditions["Attractiveness"];
        } else cF = new Condition(Condition.MATCH_IDENTIFIER + "-Attractiveness", typeof(float), "Attractiveness", 1f, true);

        Condition highHostility;
        if (mAvailableConditions.ContainsKey("Hostility")) {
            highHostility = mAvailableConditions["Hostility"];
        }
        else highHostility = new Condition(Condition.MATCH_IDENTIFIER + "-Hostility", typeof(float), "Hostility", 1f, true);

        Condition selfCourage;
        if (mAvailableConditions.ContainsKey("Courage")) {
            selfCourage = mAvailableConditions["Courage"];
        } else {
            selfCourage = new Condition(Condition.MATCH_IDENTIFIER + "-Courage", typeof(float), "Courage", 0f);
            selfCourage.Inverted = true;
        }
        
        // An event needs: A name, a behavior, conditions and a priority.


        /* Example 4 - respond to a foreground player call  */
        /*
        aName = "RespondToCall";
        aAction = NPCController.mAvailableActions["ApproachEntity"];
        aConditions = new Dictionary<string, Condition>();
        aConditions.Add(cB.Name, cB);
        aConditions.Add(cC.Name, cC);
        aConditions.Add(cD.Name, cD);
        aConditions.Add(cATarg.Name, cATarg);
        e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.Social, NPCEventType.TARGETED);
        mAvailableEvents.Add(e.Name, e);
        */
        /* Example 3 - Wander  */
        /*
        if (!mAvailableEvents.ContainsKey("Wander")) {
            aName = "Wander";
            aAction = NPCController.mAvailableActions[aName];
            aConditions = new Dictionary<string, Condition>();
            aConditions.Add(cA.Name, cA); // i am not in an event
            aConditions.Add(testNPC.Name, testNPC); // i am NOT test NPC
            e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.Individual, NPCEventType.NEUTRAL);
            mAvailableEvents.Add(e.Name, e);
        }
        */
        /* Example 3 - Reprehend  */
        /*
        if (!mAvailableEvents.ContainsKey("Reprehend")) {
            aName = "Reprehend";
            aAction = NPCController.mAvailableActions["Reprehend"];
            aConditions = new Dictionary<string, Condition>();
            aConditions.Add(cB.Name, cB); // target is the foreground
            aConditions.Add(cE.Name, cE); // he is being rude
            aConditions.Add(cF.Name, cF); // he is calling for attention
            // aConditions.Add (cA.Name, cA); // he is NOT in an event
            e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.Social, NPCEventType.TARGETED);
            mAvailableEvents.Add(e.Name, e);
        }
        */
        /*
        if (!mAvailableEvents.ContainsKey("GetScared")) {
            aName = "GetScared";
            aAction = NPCController.mAvailableActions["GetScared"];
            aConditions = new Dictionary<string, Condition>();
            aConditions.Add(cB.Name, cB); // target is the foreground
            aConditions.Add(cF.Name, cF); // he is calling for attention
            aConditions.Add(highHostility.Name, highHostility); // he has high hostility
            e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.SelfPreservation, NPCEventType.GLOBAL);
            mAvailableEvents.Add(e.Name, e);
        }
        */
        /*
        if (!mAvailableEvents.ContainsKey("GetTerrified")) {
            aName = "GetTerrified";
            aAction = NPCController.mAvailableActions["GetTerrified"];
            aConditions = new Dictionary<string, Condition>();
            aConditions.Add(cB.Name, cB); // target is the foreground
            aConditions.Add(cF.Name, cF); // he is calling for attention
            aConditions.Add(highHostility.Name, highHostility); // he has high hostility
            aConditions.Add(selfCourage.Name, selfCourage);
            e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.SelfPreservation, NPCEventType.GLOBAL);
            mAvailableEvents.Add(e.Name, e);
        }
        */

        /*
        aName = "Converse";
        aAction = NPCController.mAvailableActions["ConverseWith"];
        aConditions = new Dictionary<string,Condition>();
        aConditions.Add(cA.Name, cA); // i am not in an event
        aConditions.Add (cC.Name, cC); // friendliness level is high
        e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.Social,NPCEventType.TARGETED);
        mAvailableEvents.Add (e.Name, e);
        */

        /* USER ACTION Events  */

        // aConditions = new Dictionary<string, Condition>();
        
        /*
        if (!mAvailableEvents.ContainsKey("USER-GoTo")) {
            aName = "USER-GoTo";
            aAction = NPCController.mAvailableActions["GoTo"];
            e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.UserCommand, NPCEventType.NEUTRAL);
            mAvailableEvents.Add(e.Name, e);
        }
        */
        /*
        if (!mAvailableEvents.ContainsKey("USER-WaveTo")) {
            aName = "USER-WaveTo";
            aAction = NPCController.mAvailableActions["WaveTo"];
            e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.UserCommand, NPCEventType.TARGETED);
            e.AddModifier(new Modifier("Friendliness", ModifierType.INCREMENTOR, 0.3f));
            mAvailableEvents.Add(e.Name, e);
        }
        */
        /*
        aName = "USER-Dismiss";
        aAction = NPCController.mAvailableActions["DoGesture"];
        e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.UserCommand, NPCEventType.TARGETED);
        e.AddModifier(new Modifier("Rudeness", ModifierType.INCREMENTOR, 0.3f));
        e.AddModifier(new Modifier("Friendliness", ModifierType.DECREMENTOR, 0.1f));
        mAvailableEvents.Add(e.Name, e);

        aName = "USER-ApproachEntity";
        aAction = NPCController.mAvailableActions["ApproachEntity"];
        e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.UserCommand, NPCEventType.TARGETED);
        mAvailableEvents.Add(e.Name, e);
        */
        /*
        if(!mAvailableEvents.ContainsKey("USER-ConverseWith")) {
            aName = "USER-ConverseWith";
            aAction = NPCController.mAvailableActions["ConverseWith"];
            e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.UserCommand, NPCEventType.TARGETED);
            mAvailableEvents.Add(e.Name, e);
        }
        */
        /*
        if (!mAvailableEvents.ContainsKey("USER-Yawn")) {
            aName = "USER-Yawn";
            aAction = NPCController.mAvailableActions["DoGesture"];
            e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.UserCommand, NPCEventType.GLOBAL);
            e.AddModifier(new Modifier("Rude", ModifierType.INCREMENTOR, 0.2f, false));
            e.AddModifier(new Modifier("Attractiveness", ModifierType.INCREMENTOR, 0.3f, false));
            mAvailableEvents.Add(e.Name, e);
        }
        */
        /*
        if (!mAvailableEvents.ContainsKey("USER-Roar")) {
            aName = "USER-Roar";
            aAction = NPCController.mAvailableActions["DoGesture"];
            e = new NPCEvent(aName, aAction, aConditions, NPCEventPriority.UserCommand, NPCEventType.GLOBAL);
            e.AddModifier(new Modifier("Hostility", ModifierType.INCREMENTOR, 2f, false));
            e.AddModifier(new Modifier("Attractiveness", ModifierType.INCREMENTOR, 2f, false));
            mAvailableEvents.Add(e.Name, e);
        }
        */
    }


    private static void InitializeContexts() {
        /*
         Context c1 = new Context("Market");
         Context c2 = new Context("FountainSquare");
        // Context c3 = new Context("TestContext");
         Context c4 = new Context("HolyPlace");

         Modifier m0 = new Modifier("Friendliness", ModifierType.INCREMENTOR, 1f);
         Modifier m1 = new Modifier("Friendliness", ModifierType.NEUTRAL, 0f);
         Modifier m2 = new Modifier("Rude", ModifierType.INCREMENTOR, 2f);
         Modifier m3 = new Modifier("Attractiveness", ModifierType.INCREMENTOR, 2f);

         c1.AddModifier(m1);

         c2.AddModifier(m0);

        // c3.AddModifier(m2);
        // c3.AddModifier(m3);

        c4.AddModifier(m2);
        c4.AddModifier(m3);

         NPCController.mAvailableContexts.Add(c1.Name, c1);
         NPCController.mAvailableContexts.Add(c2.Name, c2);
        // NPCController.mAvailableContexts.Add(c3.Name, c3);
         NPCController.mAvailableContexts.Add(c4.Name, c4);
        */
    }
    
    #endregion
}
