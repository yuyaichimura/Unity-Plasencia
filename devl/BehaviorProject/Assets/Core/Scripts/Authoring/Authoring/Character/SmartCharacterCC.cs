using RootMotion.FinalIK;
using TreeSharpPlus;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//There are still a bunch of duplicate attributes
public class SmartCharacterCC : SmartCharacter
{
    public bool Controllable;
    public static bool done = false;
    public StatusIcon2 icon;

    public static float TIMER_VAL = 15.0f;
    public static string TAG_SMARTCHARACTER = "SmartCharacter";
    public static string TAG_DOOR = "Door";
    public static string TAG_SMARTOBJECT = "Smart Object";
    public static string TAG_SMARTCROWD = "SmartCrowd";
    public static string TAG_SMARTDOOR = "SmartDoor";


    public static Dictionary<int, string> icon_display_map;
    public static Dictionary<int, string> map_anim_state;
    public static Dictionary<string, string> map_anim_state_name;

    static SmartCharacterCC()
    {
        gesture_map_id = new Dictionary<string, int>();
        gesture_map_id.Add("BOW", 0);
        gesture_map_id.Add("SHAKEHAND", 1);
        gesture_map_id.Add("OPENDOOR", 2);
        gesture_map_id.Add("HOOKRIGHT", 10);
        gesture_map_id.Add("NOD", 11);
        gesture_map_id.Add("DIE", 12);
        gesture_map_id.Add("FIGHT", 13);
        gesture_map_id.Add("INTIMIDATE", 14);
        gesture_map_id.Add("FISTSHAKE", 15);
        gesture_map_id.Add("THREATEN", 16);
        gesture_map_id.Add("YELL1", 17);
        gesture_map_id.Add("YELL2", 18);
        gesture_map_id.Add("MOURN", 19);
        gesture_map_id.Add("YELLANGRILY", 20);
        gesture_map_id.Add("CHEERHAPPILY", 21);

        SmartCharacterCC.icon_display_map = new Dictionary<int, string>();
        SmartCharacterCC.icon_display_map.Add(1, "goto_fountain");
        SmartCharacterCC.icon_display_map.Add(2, "goto_palace");
        SmartCharacterCC.icon_display_map.Add(3, "goto_synagogue");
        SmartCharacterCC.icon_display_map.Add(4, "inspect_house");

        SmartCharacterCC.map_anim_state = new Dictionary<int, string>();
        SmartCharacterCC.map_anim_state.Add(Animator.StringToHash("HandGesture.Bow"), "BOW");
        SmartCharacterCC.map_anim_state.Add(Animator.StringToHash("HandGesture.ShakeHand"), "SHAKEHAND");
        SmartCharacterCC.map_anim_state.Add(Animator.StringToHash("HandGesture.OpenDoor"), "OPENDOOR");
        SmartCharacterCC.map_anim_state.Add(Animator.StringToHash("HandGesture.Die"), "DIE");
        SmartCharacterCC.map_anim_state.Add(Animator.StringToHash("HandGesture.HookRight"), "HOOKRIGHT");
        SmartCharacterCC.map_anim_state.Add(Animator.StringToHash("HandGesture.Yell2"), "YELLANGRILY");
        SmartCharacterCC.map_anim_state.Add(Animator.StringToHash("HandGesture.FistShake"), "FISTSHAKE");
        SmartCharacterCC.map_anim_state.Add(Animator.StringToHash("HandGesture.Threaten"), "THREATEN");
        SmartCharacterCC.map_anim_state.Add(Animator.StringToHash("HandGesture.Cry2"), "MOURN");
        SmartCharacterCC.map_anim_state.Add(Animator.StringToHash("HandGesture.Cheer2"), "CHEERHAPPILY");


        SmartCharacterCC.map_anim_state_name = new Dictionary<string, string>();
        SmartCharacterCC.map_anim_state_name.Add("BOW", "HandGesture.Bow");
        SmartCharacterCC.map_anim_state_name.Add("SHAKEHAND", "HandGesture.ShakeHand");
        SmartCharacterCC.map_anim_state_name.Add("OPENDOOR", "HandGesture.OpenDoor");
        SmartCharacterCC.map_anim_state_name.Add("HOOKRIGHT", "HandGesture.HookRight");
        SmartCharacterCC.map_anim_state_name.Add("YELLANGRILY", "HandGesture.Yell2");
        SmartCharacterCC.map_anim_state_name.Add("FISTSHAKE", "HandGesture.FistShake");
        SmartCharacterCC.map_anim_state_name.Add("THREATEN", "HandGesture.Threaten");
        SmartCharacterCC.map_anim_state_name.Add("MOURN", "HandGesture.Cry2");
        SmartCharacterCC.map_anim_state_name.Add("CHEERHAPPILY", "HandGesture.Cheer2");
    }

    public Animator animator { private set; get; }
    public Animation animation { private set; get; }
    // public CharacterMecanim character { private set; get; }

    public InteractionObject InteractionOpenDoor;
    public InteractionObject InteractionHandShake;

    private Interpolator<Vector3> IncapacitateNudge = null;

    delegate Node CurrentTask(Val<Vector3> position);

    public Light light;
    public Light goalLight;
    string currentAnimation = null;

    private bool resettingHandLayerWeight;
    private bool resettingFaceLayerWeight;

    public bool Controlled { private set; get; }
    public string curTaskMessage { get; set; }

    public override string Archetype { get { return "SmartCharacterCC"; } }

    static readonly float STOP_EPSILON = 0.05f;
    public static float stoppingRadius = 0.4f;
    float timeLeft = 0f;
    private bool time { get; set; }

    public static Dictionary<string, int> gesture_map_id;
    Dictionary<string, bool> action_map_on;
    Dictionary<string, int> action_map_hash;
    bool[] action_list = new bool[4];
    Dictionary<string, string> action_animator_map;


    public int status = -1;
    public int interacting = -1;
    public static float prio = 1.0f;

    SmartCharacterCC helper = null;

    InteractionCollider collider_interaction;
    CrowdCollider collider_crowd;

    private bool needAssistance = false;

    private bool helpOnce = false;

    public static float interactionDistance = 0.6f;
    public static float nearDistance = 2.0f;


    private AnimatorStateInfo curAnimState;
    public static int bowState;


    public ParticleSystem particles;
    public bool helpable;

    protected override void Initialize(BehaviorObject obj)
    {
        base.Initialize(obj);

        animator = this.gameObject.GetComponent<Animator>();
        animation = this.gameObject.GetComponent<Animation>();

        this.status = -1;
        this.interacting = -1;

        if (this.MarkerHead == null)
            this.MarkerHead =
                this.GetComponent<Animator>().GetBoneTransform(
                    HumanBodyBones.Head);

        time = false;
        curTaskMessage = "";
        this.Unselect();

        this.collider_interaction = GetComponentInChildren<InteractionCollider>();
        this.collider_crowd = GetComponentInChildren<CrowdCollider>();

        if (collider_crowd == null)
        {
           // Debug.Log("Empty collider crowd");
        }

        if (collider_interaction == null)
        {
          //  Debug.Log("Empty collider interaction");
        }

        if (goalLight != null)
            this.goalLight.enabled = false;
        
        this.helpOnce = false;
        this.helpable = true;

        bowState = Animator.StringToHash("HandGesture.Bow");

        this.icon = this.gameObject.GetComponentInChildren<StatusIcon2>();

        this.particles = this.gameObject.GetComponentInChildren<ParticleSystem>();
        if (this.particles != null)
        {
           // Debug.Log("Particles exist: " + this.gameObject.name);
        }
    }

    void Awake()
    {
        this.Initialize(
            new BehaviorAgent(
                new DecoratorLoop(
                    new LeafAssert(() => true))));
    }
    
    public void Update()
    {
        if (this.trackTarget != null)
            this.transform.rotation =
                Quaternion.LookRotation(
                    this.trackTarget.position - this.transform.position,
                    Vector3.up);
        if (this.IncapacitateNudge != null)
            this.transform.position = this.IncapacitateNudge.Value;

        if (Controlled)
            checkTimer();

        DetectAnimation();

        if (resettingHandLayerWeight)
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0, Time.deltaTime));
        }
    }

    protected void DetectAnimation()
    {
        curAnimState = this.animator.GetCurrentAnimatorStateInfo(1);

        if (this.animator.IsInTransition(1) && SmartCharacterCC.map_anim_state.ContainsKey(curAnimState.nameHash) && this.status != -1)
        {
            Debug.Log("Animation Detection");

            string gesture = SmartCharacterCC.map_anim_state[curAnimState.nameHash];
            if (gesture.ToUpper().Equals("DIE"))
            {
                this.Controllable = false;
                return;
            }
            this.HandAnimation(gesture.ToUpper(), false);
            Debug.Log(gesture + " has been detected");

            this.status = -1;
            base.SetIcon(null);

            if (Controlled)
            {
                Character.Body.ResetAnimation();
                this.HandAnimation(gesture, false);
                Character.NavGoTo(this.gameObject.transform.position); //Character has less chance of getting stuck after animation with this line.
            }
        }

    }

    #region Nudge
    public void Action_GetNearestSmartCrowd()
    {
        SmartCharacterCC character = this.collider_crowd.GetNearestObject();
        if (character == null)
        {
            // Debug.Log("Nearest Crowd not here");
            return;
        }

        SmartObject[] participants = { this, character };
        BehaviorEvent e = new BehaviorEvent(node => SmartCharacterCC.CrowdHelp(this, character), participants);
        //  SmartCharacterCC.prio += 0.1f;
        SmartCharacterCC.AdjustPriority();

        e.StartEvent(SmartCharacterCC.prio);
    }

    public void SmartCrowdAssist(string message)
    {
        SmartCharacterCC character = this.collider_crowd.GetNearestObject();
        if (character == null)
        {
            Debug.Log("Nearest Crowd not here");
            return;
        }

        SmartObject[] participants = { character };
        BehaviorEvent e = new BehaviorEvent(node => SmartCharacterCC.CrowdHelp(this, character, message), participants);
        //  SmartCharacterCC.prio += 0.1f;
        SmartCharacterCC.AdjustPriority();

        e.StartEvent(SmartCharacterCC.prio);
    }
    #endregion

    public void EndEvent(
    BehaviorEvent sender,
    EventStatus newStatus)
    {
        if (newStatus == EventStatus.Finished)
        {
            sender.StopEvent();
        }
    }

    public void PlayGesture(Val<string> gestureName, SmartObject obj)
    {
        PlayGesture(gestureName, obj, false);
    }

    public void PlayGesture(Val<string> gestureName, SmartObject obj, bool reciprocate)
    {
        this.StopMoving();
        if (obj != null)
        {
            this.TurnTowards(obj.transform.position);
        }
        this.HandAnimation(gestureName.Value.ToUpper(), true);
    }

    #region Player control
    public void Action_ShakeHand()
    {

        SmartObject obj = collider_interaction.GetNearestObject(TAG_SMARTCHARACTER);
        if (obj == null)
        {
            return;
        }
        base.SetIcon("SHAKEHAND");
        this.PlayGesture("SHAKEHAND", obj);

        SmartCharacterCC char1 = obj.gameObject.GetComponent<SmartCharacterCC>();
        if (char1 != null)
        {
            char1.BaseSetIcon("SHAKEHAND");
            char1.PlayGesture("SHAKEHAND", this);
        }
    }

    public void Action_Bow()
    {
        //this.Character.Body.HandAnimation("BOW", true);
        this.PlayGesture("BOW", null);
    }

    public void Action_Bow(bool character)
    {
        if (!character)
        {
            this.PlayGesture("BOW", null);
            return;
        }
        SmartObject obj = collider_interaction.GetNearestObject(TAG_SMARTCHARACTER);
        if (obj == null)
        {
            this.PlayGesture("BOW", null);
            return;
        }
        SmartCharacterCC char1 = obj.gameObject.GetComponent<SmartCharacterCC>();
        if (char1 != null)
        {
            char1.interacting = (int)this.gameObject.GetComponent<AssignId>().Id;

        }
        this.interacting = (int)obj.gameObject.GetComponent<AssignId>().Id;

        this.PlayGesture("BOW", obj);

        if (char1 != null)
        {
            char1.PlayGesture("bow", this);
            char1.interacting = -1;
        }
    }

    public void Action_OpenDoor()
    {
        Debug.Log("-Action OpenDoor-");

        SmartObject obj = collider_interaction.GetNearestObject(TAG_SMARTDOOR);

        if (obj == null)
        {
            Debug.Log("Can't find object");

            return;
        }
        SmartDoor char1 = obj.gameObject.GetComponent<SmartDoor>();
        this.interacting = (int)obj.gameObject.GetComponent<AssignId>().Id;

        this.PlayGesture("OPENDOOR", obj);
    }

    public void Action_Mourn()
    {
        base.SetIcon("MOURN");
        this.PlayGesture("MOURN", null);
    }

    public void Action_Cheer()
    {
        SmartObject obj = collider_interaction.GetNearestObject(TAG_SMARTCHARACTER);
        base.SetIcon("CHEERHAPPILY");

        this.PlayGesture("CHEERHAPPILY", null);
        if (obj != null && obj.gameObject.GetComponent<AssignId>() != null)
            this.interacting = (int)obj.gameObject.GetComponent<AssignId>().Id;

    }

    public void Action_Yell()
    {
        SmartObject obj = collider_interaction.GetNearestObject(TAG_SMARTCHARACTER);
        base.SetIcon("YELLANGRILY");

        this.PlayGesture("YELLANGRILY", null);
        if (obj != null && obj.gameObject.GetComponent<AssignId>() != null)
            this.interacting = (int)obj.gameObject.GetComponent<AssignId>().Id;
    }

    public void Action_FistShake()
    {
        SmartObject obj = collider_interaction.GetNearestObject(TAG_SMARTCHARACTER);

        base.SetIcon("FISTSHAKE");
        this.PlayGesture("FISTSHAKE", null);
        if (obj != null && obj.gameObject.GetComponent<AssignId>() != null)
            this.interacting = (int)obj.gameObject.GetComponent<AssignId>().Id;
    }

    public void Action_Threaten()
    {
        SmartObject obj = collider_interaction.GetNearestObject(TAG_SMARTCHARACTER);
        base.SetIcon("THREATEN");

        this.PlayGesture("THREATEN", null);
        if (obj != null && obj.gameObject.GetComponent<AssignId>() != null)
            this.interacting = (int)obj.gameObject.GetComponent<AssignId>().Id;
    }

    public void Action_Kill()
    {

        SmartObject obj = collider_interaction.GetNearestObject(TAG_SMARTCHARACTER);
        if (obj == null)
        {
            return;
        }

        base.SetIcon("HOOKRIGHT");
        this.PlayGesture("HOOKRIGHT", obj);

        SmartCharacterCC char1 = obj.gameObject.GetComponent<SmartCharacterCC>();
        if (char1 != null)
        {
            base.SetIcon("DIE");
            char1.PlayGesture("DIE", this);
            if (obj != null && obj.gameObject.GetComponent<AssignId>() != null)
                this.interacting = (int)obj.gameObject.GetComponent<AssignId>().Id;
        }
    }

    public void Action_Die()
    {
        base.SetIcon("DIE");
        this.PlayGesture("DIE", null);
    }

    #endregion

    private void initTimer(float time)
    {
        timeLeft = time;
    }

    public void setTaskMessage(string message)
    {
        this.curTaskMessage = message;
    }

    public void startTimer()
    {
        time = true;
    }

    private void stopTimer()
    {
        time = false;
    }

    public bool checkTimer()
    {
        if (time == false)
        {
            return false;
        }
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            timeOver();
            return true;
        }
        return false;
    }

    protected void timeOver()
    {
        // stopTimer();
    }

    public void Select()
    {
        if (!Controllable)
            return;
        this.Controlled = true;
        this.light.enabled = true;
        this.needAssistance = true;
        initTimer(SmartCharacterCC.TIMER_VAL);
        helpOnce = false;
        helpable = true;
    }

    public void Unselect()
    {
        this.Controlled = false;
        if (this.light != null)
            this.light.enabled = false;
        this.needAssistance = false;
        stopTimer();
    }


    #region Control
    public void MoveCharacter(Val<Vector3> target)
    {
        Character.NavGoTo(target);
    }

    public void StopMoving()
    {
        Behavior.Character.Body.NavStop();
    }

    public void TurnTowards(Val<Vector3> target)
    {
        Character.NavTurn(target);

    }

    #endregion

    #region Behavior Affordances

    public new Node Node_GoToUpToRadius(SmartObject targ, Val<float> dist)
    {
        return SmartCharacterCC.Node_GoToUpToRadius(targ, dist, this);
    }


    #endregion


    #region Nodes

    public RunStatus Node_CheckInteraction(SmartObject target, Val<string> gestureName)
    {
        if (this.interacting == (int)target.gameObject.GetComponent<AssignId>().Id && this.status == SmartCharacterCC.gesture_map_id[gestureName.Value.ToUpper()])
        {
            IndicatorLight(target, false);
            return RunStatus.Success;
        }
        return RunStatus.Running;
    }

    public RunStatus Node_CheckPerformAt(SmartObject target, Val<string> gestureName)
    {
        if ((target.transform.position - this.transform.position).magnitude < SmartCharacterCC.nearDistance && this.status == SmartCharacterCC.gesture_map_id[gestureName.Value.ToUpper()])
        {
            IndicatorLight(target, false);
            return RunStatus.Success;
        }

        return RunStatus.Running;
    }

    public RunStatus Node_CheckPerformTo(SmartObject target, Val<string> gestureName)
    {
        if ((target.transform.position - this.transform.position).magnitude < SmartCharacterCC.interactionDistance && this.status == SmartCharacterCC.gesture_map_id[gestureName.Value.ToUpper()])
        {
            IndicatorLight(target, false);
            return RunStatus.Success;
        }
        return RunStatus.Running;
    }

    public RunStatus Node_CheckPerform(Val<string> gestureName)
    {
        if (this.status == SmartCharacterCC.gesture_map_id[gestureName.Value.ToUpper()])
        {
            return RunStatus.Success;
        }
        return RunStatus.Running;
    }

    public RunStatus Interact_Clean(SmartCharacterCC target)
    {
        this.status = -1;
        target.status = -1;
        this.helpable = true;
        this.BaseSetIcon(null);
        initTimer(SmartCharacterCC.TIMER_VAL);

        return RunStatus.Success;
    }

    public RunStatus Interact_Clean()
    {
        this.status = -1;
        this.helpable = true;
        this.BaseSetIcon(null);
        initTimer(SmartCharacterCC.TIMER_VAL);

        return RunStatus.Success;
    }

    public Node Node_Interact(SmartCharacterCC target, Val<string> gestureName)
    {
        return new DecoratorCatch(
            () => Interact_Clean(target),
            new DecoratorForceStatus(
                RunStatus.Success,
                new SelectorParallel(
                    new LeafInvoke(() => Node_CheckInteraction(target, gestureName)),
                    new DecoratorLoop(
                        new DecoratorInvert(
                                new Sequence(
                                    new Selector(
                                            new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterCC.interactionDistance),
                                            Node_GoToX(target)
                                        ),
                                    new Selector(
                                        new Sequence(
                                            new LeafAssert(() => this.Controlled),
                                            new LeafInvoke(() => IndicatorLight(target, true)),
                                            new Selector(
                                                new Sequence(
                                                    new LeafAssert(() => this.helpable),
                                                    new LeafInvoke(() => this.startTimer()),
                                                    new LeafAssert(() => this.checkTimer()),
                                                    target.CharacterInteraction(gestureName, this),
                                                    new LeafInvoke(() => this.helpable = false)
                                                    ),
                                                new LeafAssert(() => true)
                                                )
                                            ),
                                        new Sequence(
                                            new LeafAssert(() => !this.Controlled),
                                            new LeafInvoke(() => IndicatorLight(target, false)),
                                            this.CharacterInteraction(gestureName, target.gameObject.GetComponent<SmartCharacterCC>())
                                            )
                                        ),
                                        new LeafAssert(() => false)
                                )
                            )
                        )
                    )
                )
            );
    }

    public Node Node_Interact(SmartObject target, Val<string> gestureName)
    {
        return new DecoratorCatch(
           () => Interact_Clean(),
           new DecoratorForceStatus(
               RunStatus.Success,
               new SelectorParallel(
                   new LeafInvoke(() => Node_CheckInteraction(target, gestureName)),
                   new DecoratorLoop(
                       new DecoratorInvert(
                               new Sequence(
                                   new Selector(
                                           new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterCC.interactionDistance),
                                           Node_GoToX(target)
                                       ),
                                   new Selector(
                                       new Sequence(
                                           new LeafAssert(() => this.Controlled),
                                           new LeafInvoke(() => IndicatorLight(target, true)),
                                           new Selector(
                                               new Sequence(
                                                   new LeafAssert(() => this.helpable),
                                                   new LeafInvoke(() => this.startTimer()),
                                                   new LeafAssert(() => this.checkTimer()),
                                                    new LeafInvoke(() => this.SmartCrowdAssist(this.gameObject.name + ", please go to " + target.gameObject.name + " and " + gestureName.Value)),
                                                   new LeafInvoke(() => this.helpable = false)
                                                   ),
                                               new LeafAssert(() => true)
                                               )
                                           ),
                                       new Sequence(
                                           new LeafAssert(() => !this.Controlled),
                                           new LeafInvoke(() => IndicatorLight(target, false)),
                                           this.CharacterInteraction(gestureName, target)
                                           )
                                       ),
                                       new LeafAssert(() => false)
                               )
                           )
                       )
                   )
               )
           );
    }

    public Node Node_PerformAt(SmartObject target, Val<string> gestureName)
    {
        return new Sequence(
           new DecoratorForceStatus(
               RunStatus.Success,
               new SelectorParallel(
                   new LeafInvoke(() => Node_CheckPerformAt(target, gestureName)),
                   new DecoratorLoop(
                       new DecoratorInvert(
                               new Sequence(
                                       new Selector(
                                           new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterCC.nearDistance),
                                           Node_GoToX(target)
                                       ),
                                       new Selector(
                                            new Sequence(
                                               new LeafAssert(() => this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, true)),
                                               new Selector(
                                                   new Sequence(
                                                       new LeafAssert(() => this.helpable),
                                                       new LeafInvoke(() => this.startTimer()),
                                                       new LeafAssert(() => this.checkTimer()),
                                                        new LeafInvoke(() => this.SmartCrowdAssist(this.gameObject.name + ", please go to " + target.gameObject.name + " and " + gestureName.Value)),
                                                       new LeafInvoke(() => this.helpable = false)
                                                       ),
                                                   new LeafAssert(() => true)
                                               )
                                           ),
                                           new Sequence(

                                               new LeafAssert(() => !this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, false)),
                                               this.CharacterInteraction(gestureName, target)
                                               )
                                       ),
                                       new LeafAssert(() => false)
                               )
                           )
                       )
                   )
               ), new LeafInvoke(() => Interact_Clean()), new LeafInvoke(() => Debug.Log("node_performat donezo"))
           );
    }

    public Node Node_PerformAt(SmartObject target, Val<string> gestureName, string message)
    {
        return new Sequence(
           new DecoratorForceStatus(
               RunStatus.Success,
               new SelectorParallel(
                   new LeafInvoke(() => Node_CheckPerformAt(target, gestureName)),
                   new DecoratorLoop(
                       new DecoratorInvert(
                               new Sequence(
                                       new Selector(
                                           new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterCC.nearDistance),
                                           Node_GoToX(target, message)
                                       ),
                                       new Selector(
                                            new Sequence(
                                               new LeafAssert(() => this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, true)),
                                               new Selector(
                                                   new Sequence(
                                                       new LeafAssert(() => this.helpable),
                                                       new LeafInvoke(() => this.startTimer()),
                                                       new LeafAssert(() => this.checkTimer()),
                                                        new LeafInvoke(() => this.SmartCrowdAssist(message)),
                                                       new LeafInvoke(() => this.helpable = false)
                                                       ),
                                                   new LeafAssert(() => true)
                                               )
                                           ),
                                           new Sequence(
                                               new LeafAssert(() => !this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, false)),
                                               this.CharacterInteraction(gestureName, target)
                                               )
                                       ),
                                       new LeafAssert(() => false)
                               )
                           )
                       )
                   )
               ), new LeafInvoke(() => Interact_Clean())
           );
    }

    public Node Node_PerformTo(SmartObject target, Val<string> gestureName, string message)
    {
        return new Sequence(
           new DecoratorForceStatus(
               RunStatus.Success,
               new SelectorParallel(
                   new LeafInvoke(() => Node_CheckPerformTo(target, gestureName)),
                   new DecoratorLoop(
                       new DecoratorInvert(
                               new Sequence(
                                       new Selector(
                                           new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterCC.interactionDistance),
                                           Node_GoToX(target, message)
                                       ),
                                       new Selector(
                                            new Sequence(
                                               new LeafAssert(() => this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, true)),
                                               new Selector(
                                                   new Sequence(
                                                       new LeafAssert(() => this.helpable),
                                                       new LeafInvoke(() => this.startTimer()),
                                                       new LeafAssert(() => this.checkTimer()),
                                                       new LeafInvoke(() => this.SmartCrowdAssist(message)),
                                                       new LeafInvoke(() => this.helpable = false)
                                                       ),
                                                   new LeafAssert(() => true)
                                               )
                                           ),
                                           new Sequence(
                                               new LeafAssert(() => !this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, false)),
                                               this.CharacterInteraction(gestureName, target)
                                               )
                                       ),
                                       new LeafAssert(() => false)
                               )
                           )
                       )
                   )
               ), new LeafInvoke(() => Interact_Clean())
           );
    }

    public Node Node_Perform(SmartObject target, Val<string> gestureName)
    {
        return new Sequence(
           new DecoratorForceStatus(
               RunStatus.Success,
               new SelectorParallel(
                   new LeafInvoke(() => Node_CheckInteraction(target, gestureName)),
                   new DecoratorLoop(
                       new DecoratorInvert(
                               new Sequence(
                                       new Selector(
                                           new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterCC.interactionDistance),
                                           Node_GoToX(target)
                                       ),
                                       new Selector(
                                            new Sequence(
                                               new LeafAssert(() => this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, true)),
                                               new Selector(
                                                   new Sequence(
                                                       new LeafAssert(() => this.helpable),
                                                       new LeafInvoke(() => this.startTimer()),
                                                       new LeafAssert(() => this.checkTimer()),
                                                        new LeafInvoke(() => this.SmartCrowdAssist(this.gameObject.name + ", please go to " + target.gameObject.name + " and " + gestureName.Value)),
                                                       new LeafInvoke(() => this.helpable = false)
                                                       ),
                                                   new LeafAssert(() => true)
                                               )
                                           ),
                                           new Sequence(
                                               new LeafAssert(() => !this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, false)),
                                               this.CharacterInteraction(gestureName, target)
                                               )
                                       ),
                                       new LeafAssert(() => false)
                               )
                           )
                       )
                   )
               ), new LeafInvoke(() => Interact_Clean())
           );
    }

    public Node Node_Perform(Val<string> gestureName)
    {
        return new Sequence(new DecoratorCatch(
           () => Interact_Clean(),
           new DecoratorForceStatus(
               RunStatus.Success,
               new SelectorParallel(
                   new LeafInvoke(() => Node_CheckPerform(gestureName)),
                   new DecoratorLoop(
                       new DecoratorInvert(
                               new Sequence(
                                   new Selector(
                                       new Sequence(
                                           new LeafAssert(() => this.Controlled),
                                           new Selector(
                                               new Sequence(
                                                   new LeafAssert(() => this.helpable),
                                                   new LeafInvoke(() => this.startTimer()),
                                                   new LeafAssert(() => this.checkTimer()),
                                                    new LeafInvoke(() => this.SmartCrowdAssist(this.gameObject.name + ", please " + gestureName.Value)),
                                                   new LeafInvoke(() => this.helpable = false)
                                                   ),
                                               new LeafAssert(() => true)
                                               )
                                           ),
                                       new Sequence(
                                           new LeafAssert(() => !this.Controlled),
                                           this.CharacterInteraction(gestureName)
                                           )
                                       ),
                                       new LeafAssert(() => false)
                               )
                           )
                       )
                   )
               )
           ), new LeafInvoke(() => Interact_Clean()));
    }

    public Node CharacterInteraction(
      Val<string> gestureName)
    {
        if (gestureName.Value.ToUpper().Equals("DIE"))
        {
            return new Sequence(
                new LeafInvoke(() => this.BaseSetIcon(gestureName.Value)),
                this.Node_HandAnimation_NI(gestureName, true));
        }
        return
            new Sequence(
                new LeafInvoke(() => this.BaseSetIcon(gestureName.Value)),
                this.ST_PlayHandGesture(gestureName, 3000)
);
    }

    public Node Node_InteractY(SmartObject target, Val<string> gestureName)
    {
        return new Sequence(new DecoratorForceStatus(RunStatus.Success,
            new Sequence(
                new DecoratorLoop(
                new DecoratorInvert(
                    new SelectorParallel(
                        new DecoratorInvert(
                        new DecoratorLoop(
                            new DecoratorInvert(
                                new Sequence(
                                    new Selector(
                                        new Sequence(
                                            new LeafAssert(() => this.status == SmartCharacterCC.gesture_map_id[gestureName.Value.ToUpper()]),
                                            new LeafAssert(() => true)
                                        ),
                                        new LeafAssert(() => false)
                                    )
                                )
                            )
                        )
                            ),
                        new Sequence(
                            new Sequence(
                                new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterCC.interactionDistance),
                                this.Node_GoToX(target),
                                new LeafAssert(() => true)
                            ),
                            new Sequence(
                                new Selector(
                                    new Sequence(
                                        new LeafAssert(() => !Controlled),
                                        new LeafInvoke(() => IndicatorLight(target, false)),
                                        // new LeafInvoke(() => Debug.Log("!Controlled")),
                                        this.CharacterInteractionY(gestureName, target)
                                    ),
                                    new Sequence(
                                        new LeafAssert(() => Controlled),
                                        new LeafInvoke(() => IndicatorLight(target, true))//,
                                    )
                                )
                           ),
                           new LeafAssert(() => false)
                       )
                   )
               )
           )
       ))
       );
    }

    bool _goto = false;

    public Node Node_GoToX(SmartCharacterCC target)
    {
        return new DecoratorCatch(
            () => IndicatorLight(target, false),
            new DecoratorForceStatus(
                RunStatus.Success,
                    new DecoratorLoop(
                            new Sequence(
                                new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterCC.interactionDistance),
                                //new LeafInvoke(() => Debug.Log("GoToX -- ")),     
                                new Selector(
                                    new Sequence(
                                        // new LeafInvoke(() => Debug.Log("GoTo - checking controlled")),
                                        new LeafAssert(() => this.Controlled),
                                        new LeafInvoke(() => IndicatorLight(target, true)),
                                         new Selector(
                                                new Sequence(
                                                    new LeafAssert(() => this.helpable),
                                                   // new LeafInvoke(() => Debug.Log("Helpable")),
                                                    new LeafInvoke(() => this.startTimer()),
                                                   // new LeafInvoke(() => Debug.Log("Timer start")),
                                                    new LeafAssert(() => this.checkTimer()),
                                                    //new LeafInvoke(() => Debug.Log("timer check")),
                                                    new LeafInvoke(
                                                        () => NavGoToX(target, this, Val.V(() => SmartCharacterCC.interactionDistance))
                                                    ),
                                                    //new LeafInvoke(() => Debug.Log("invoked alternative behavior")),
                                                    new LeafInvoke(() => this.helpable = false)
                                                    ),
                                                new LeafAssert(() => true)
                                            )
                                    ),
                                    new Sequence(
                                        // new LeafInvoke(() => Debug.Log("GoTo - checking !controlled")),
                                        new LeafAssert(() => !this.Controlled),
                                        new LeafInvoke(() => IndicatorLight(target, false)),
                                        //new LeafInvoke(() => Debug.Log("GoTo - !controlled 1")),
                                        new LeafInvoke(
                                            () => NavGoToX(this, target, Val.V(() => SmartCharacterCC.interactionDistance))
                                        ),
                                        // new LeafInvoke(() => Debug.Log("GoTo - !controlled 2")),
                                        new LeafAssert(() => true)
                                    )
                                )
                        )
                    )
                )
            );
    }

    public Node Node_GoToX(SmartObject target)
    {
        return
            new Sequence(
            new DecoratorCatch(
                   () => IndicatorLight(target, false),
                   new DecoratorForceStatus(
                       RunStatus.Success,
                           new DecoratorLoop(
                                   new Sequence(
                                       new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterCC.interactionDistance),
                                       new Selector(
                                           new Sequence(
                                               new LeafAssert(() => this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, true)),
                                                new Selector(
                                                       new Sequence(
                                                           new LeafAssert(() => this.helpable),
                                                           new LeafInvoke(() => this.startTimer()),
                                                           new LeafAssert(() => this.checkTimer()),
                                                            new LeafInvoke(() => this.SmartCrowdAssist(this.gameObject.name + ", please go to " + target.gameObject.name)),
                                                            new LeafInvoke(() => this.helpable = false)
                                                           ),
                                                       new LeafAssert(() => true)

                                                   )
                                           ),
                                           new Sequence(
                                               new LeafAssert(() => !this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, false)),
                                               new LeafInvoke(
                                                   () => NavGoToX(this, target, Val.V(() => SmartCharacterCC.interactionDistance))
                                               ),
                                               new LeafAssert(() => true)
                                           )
                                       )
                               )
                           )
                       )),
                       new LeafInvoke(() => IndicatorLight(target, false))

                   );
    }

    public Node Node_GoToX(SmartObject target, string message)
    {
        return
            new Sequence(
            new DecoratorCatch(
                   () => IndicatorLight(target, false),
                   new DecoratorForceStatus(
                       RunStatus.Success,
                           new DecoratorLoop(
                                   new Sequence(
                                       new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterCC.interactionDistance),
                                       new Selector(
                                           new Sequence(
                                               new LeafAssert(() => this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, true)),

                                                new Selector(

                                                       new Sequence(
                                                           new LeafAssert(() => this.helpable),
                                                           new LeafInvoke(() => this.startTimer()),
                                                           new LeafAssert(() => this.checkTimer()),
                                                            new LeafInvoke(() => this.SmartCrowdAssist(message)),

                                                           new LeafInvoke(() => this.helpable = false)
                                                           ),
                                                       new LeafAssert(() => true)

                                                   )
                                           ),
                                           new Sequence(
                                               new LeafAssert(() => !this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, false)),
                                               new LeafInvoke(
                                                   () => NavGoToX(this, target, Val.V(() => SmartCharacterCC.interactionDistance))
                                               ),
                                               new LeafAssert(() => true)
                                           )
                                       )

                               )
                           )
                       )),
                       new LeafInvoke(() => IndicatorLight(target, false))

                   );
    }

    public Node Node_GoToX(SmartObject target, Val<Vector3> direction)
    {
        return new Sequence(
            new DecoratorCatch(
                   () => IndicatorLight(target, false),

                   new DecoratorForceStatus(
                       RunStatus.Success,
                           new DecoratorLoop(
                                   new Sequence(
                                       new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterCC.interactionDistance),
                                       new Selector(
                                           new Sequence(
                                               new LeafAssert(() => this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, true)),

                                                new Selector(

                                                       new Sequence(
                                                           new LeafAssert(() => this.helpable),
                                                           new LeafInvoke(() => this.startTimer()),
                                                           new LeafAssert(() => this.checkTimer()),

                                                            new LeafInvoke(() => this.SmartCrowdAssist(this.gameObject.name + ", please go to " + target.gameObject.name)),
                                                           // new LeafInvoke(() => Debug.Log("Assist 2")),

                                                           new LeafInvoke(() => this.helpable = false)
                                                           ),
                                                       new LeafAssert(() => true)

                                                   )
                                           ),
                                           new Sequence(
                                               // new LeafInvoke(() => Debug.Log("GoTo - checking !controlled")),
                                               new LeafAssert(() => !this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, false)),
                                               //new LeafInvoke(() => Debug.Log("GoTo - !controlled 1")),
                                               new LeafInvoke(
                                                   () => NavGoToX(this, target, Val.V(() => SmartCharacterCC.interactionDistance))
                                               ),
                                               // new LeafInvoke(() => Debug.Log("GoTo - !controlled 2")),
                                               new LeafAssert(() => true)
                                           )
                                       )

                               )
                           )
                       )
                            ),
                                               new LeafInvoke(() => IndicatorLight(target, false)),
                            this.Node_OrientTowards(direction)
                   );
    }

    public Node Node_GoToX(SmartObject target, Val<Vector3> direction, string message)
    {

        return new Sequence(
            new DecoratorCatch(
                   () => IndicatorLight(target, false),

                   new DecoratorForceStatus(
                       RunStatus.Success,
                           new DecoratorLoop(
                                   new Sequence(
                                       new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterCC.interactionDistance),
                                       new Selector(
                                           new Sequence(
                                               new LeafAssert(() => this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, true)),

                                                new Selector(

                                                       new Sequence(
                                                           new LeafAssert(() => this.helpable),
                                                           new LeafInvoke(() => this.startTimer()),
                                                           new LeafAssert(() => this.checkTimer()),

                                                            new LeafInvoke(() => this.SmartCrowdAssist(message)),
                                                           // new LeafInvoke(() => Debug.Log("Assist 2")),

                                                           new LeafInvoke(() => this.helpable = false)
                                                           ),
                                                       new LeafAssert(() => true)

                                                   )
                                           ),
                                           new Sequence(
                                               // new LeafInvoke(() => Debug.Log("GoTo - checking !controlled")),
                                               new LeafAssert(() => !this.Controlled),
                                               new LeafInvoke(() => IndicatorLight(target, false)),
                                               //new LeafInvoke(() => Debug.Log("GoTo - !controlled 1")),
                                               new LeafInvoke(
                                                   () => NavGoToX(this, target, Val.V(() => SmartCharacterCC.interactionDistance))
                                               ),
                                               // new LeafInvoke(() => Debug.Log("GoTo - !controlled 2")),
                                               new LeafAssert(() => true)
                                           )
                                       )

                               )
                           )
                       )
                            ),
                                               new LeafInvoke(() => IndicatorLight(target, false)),
                            this.Node_OrientTowards(direction)
                   );
    }

    public static RunStatus NavGoToX(SmartCharacterCC character, SmartObject target, Val<float> dist)
    {
        //Debug.Log("NavGotoX");
        if (character.Character.Body.NavCanReach(target.transform.position) == false)
        {
            // Debug.LogWarning("NavGoTo failed -- can't reach target");
            return RunStatus.Failure;
        }

        character.Character.Body.NavGoTo(target.transform.position);

        return RunStatus.Success;
    }

    public static void IndicatorLight(SmartObject target, bool active)
    {
        IndicatorLight light = target.gameObject.GetComponent<IndicatorLight>();

        if (light != null)
        {
            if (active && !light.on)
                light.turnOn();
            else if (!active && light.on)
                light.turnOff();
        }
    }

    public static Node Node_GoToUpToRadius(SmartObject targ, Val<float> dist, SmartCharacterCC character)
    {


        return new LeafInvoke(
            () => SmartCharacterCC.NavGoToUpToRadius(targ, dist, character)
            //,
            //() => SmartCharacterCC.NavReachedNearGoal(targ, dist, character)
            );
        /*
         return new DecoratorCatch(
            () => SmartCharacterCC.CleanUp(targ), 
            new LeafInvoke(
            () => SmartCharacterCC.NavGoToUpToRadius(targ, dist, character),
            () => SmartCharacterCC.NavReachedNearGoal(targ, dist, character)));
         */
    }



    public static Node Node_OrientTowardsCC(SmartObject targ, SmartCharacterCC character)
    {
        return new LeafInvoke(
            () => SmartCharacterCC.NavTurnCC(targ, character),
            () => SmartCharacterCC.NavOrientBehaviorCC(OrientationBehavior.LookForward, character));
    }

    /*
     *  Moves the character to the specified location if the character is not controlled 
     *  Continues to run if the character is controleld by user
     */
    public static RunStatus NavGoTo(SmartObject target, SmartCharacterCC character)
    {
        if (character.Controlled == true)
        {
            IndicatorLight light = target.gameObject.GetComponent<IndicatorLight>();

            if (light != null && !light.on)
            {
                light.turnOn();
            }

            character.setTaskMessage(" walk to the goal location");
            character.startTimer();
            if (!character.time)
            {
                character.setTaskMessage(character.gameObject.name + ". please go to " + target.gameObject.name);
                character.initTimer(15.0f);
                character.startTimer();
            }
            if (character.timeLeft < 0)
            {
                character.helper = (SmartCharacterCC)character.collider_crowd.GetNearestObject();

                if (character.helper != null)
                {
                    SmartCharacterCC.CrowdHelp(character, character.helper);
                }
            }

            //TODO: Need better way to check the status of the goal Light
            if (target.GetComponentInChildren<Light>() != null && target.GetComponentInChildren<Light>().enabled == false)
            {

                target.GetComponentInChildren<Light>().enabled = true;
            }

            if (character.Character.Body.NavCanReach(target.transform.position) == false)
            {
                Debug.LogWarning("NavGoTo failed -- can't reach target");
                return RunStatus.Failure;
            }
            if (((character.transform.position - target.transform.position).magnitude <= (stoppingRadius + STOP_EPSILON)) && character.Character.Body.NavIsStopped() == true)
            {
                character.helper = null;
                return RunStatus.Success;
            }

            return RunStatus.Running;
        }
        else
        {
            IndicatorLight light = target.gameObject.GetComponent<IndicatorLight>();

            if (light != null && light.on)
            {
                light.turnOff();
            }
            if (character.Character.Body.NavCanReach(target.transform.position) == false)
            {
                //  Debug.LogWarning("NavGoTo failed -- can't reach target");
                return RunStatus.Failure;
            }
            character.Character.Body.NavGoTo(target.transform.position);
            if (character.Character.Body.NavHasArrived() == true)
            {
                character.Character.Body.NavStop();
                character.helper = null;

                return RunStatus.Success;
            }
            return RunStatus.Running;
        }
    }

    public static RunStatus NavGoToUpToRadius(SmartObject target, Val<float> dist, SmartCharacterCC character)
    {
        //  Debug.Log("NavGoUpToRadius Yo");
        IndicatorLight light2 = target.gameObject.GetComponent<IndicatorLight>();

        if ((target.transform.position - character.transform.position).magnitude < dist.Value)
        {
            character.helpOnce = false;
            if (light2 != null && light2.on)
            {
                // Debug.Log("Goal light off");
                light2.turnOff();
            }

            //  Debug.Log("Node Return success");
            character.Character.NavStop();
            return RunStatus.Success;
        }

        if (character.Controlled == true)
        {
            character.setTaskMessage(" walk to the goal location");
            character.startTimer();

            if (character.Character.Body.NavCanReach(target.transform.position) == false)
            {
                //  Debug.LogWarning("NavGoTo failed -- can't reach target");
                return RunStatus.Failure;
            }

            IndicatorLight light = target.gameObject.GetComponent<IndicatorLight>();

            if (light != null && !light.on)
            {
                light.turnOn();
            }

            if (character.timeLeft < 0)
            {

                if (!character.helpOnce)
                {

                    if (target.GetType() == typeof(SmartCharacterCC))
                    {

                        SmartCharacterCC[] participants = { (SmartCharacterCC)target, character };

                        BehaviorEvent e = new BehaviorEvent(node => SmartCharacterCC.Node_GoToUpToRadius(character, dist, (SmartCharacterCC)target), participants);
                        // SmartCharacterCC.CharacterInteraction("bow", this, char1);
                        // SmartCharacterCC.prio += 0.1f;
                        SmartCharacterCC.AdjustPriority();

                        e.StartEvent(SmartCharacterCC.prio);

                        character.helpOnce = true;
                    }
                    else if (target.GetType() == typeof(SmartDoor))
                    {
                        SmartCharacterCC.CrowdHelpInitiate(character, "door", target.gameObject.name);
                        character.helpOnce = true;

                    }
                    else if (target.GetType() == typeof(SmartWaypoint))
                    {
                        SmartCharacterCC.CrowdHelpInitiate(character, "waypoint", target.gameObject.name);
                        character.helpOnce = true;
                    }
                }
            }
        }
        else
        {

            if (character.Character.Body.NavCanReach(target.transform.position) == false)
            {
                //  Debug.LogWarning("NavGoTo failed -- can't reach target");
                return RunStatus.Failure;
            }

            // Debug.Log("Goal light will be turned off");
            IndicatorLight light = target.gameObject.GetComponent<IndicatorLight>();

            if (light != null && light.on)
            {
                //   Debug.Log("Goal light off");
                light.turnOff();
            }

            character.Character.Body.NavGoTo(target.transform.position);
        }

        return RunStatus.Running;
    }

    /*
     *  Termination function which checks for whether the character reaches the goal
     */
    public static RunStatus NavReachedGoal(SmartObject target, SmartCharacterCC character)
    {

        if (((character.transform.position - target.transform.position).magnitude <= (stoppingRadius + STOP_EPSILON)))
        {
            if (target.GetComponentInChildren<Light>() != null && target.GetComponentInChildren<Light>().enabled)
            {

                target.GetComponentInChildren<Light>().enabled = false;
            }

            // Debug.Log("Navreachgoal success");
            return RunStatus.Success;
        }
        //  Debug.Log("Navreachgoal runnig");

        return RunStatus.Running;
    }

    #endregion


    #region Turning

    /*
     * Function to check to see whether orientation is over
     */
    public static RunStatus NavOrientBehaviorCC(
        Val<OrientationBehavior> behavior, SmartCharacterCC character)
    {
        character.Character.Body.NavSetOrientationBehavior(behavior.Value);
        return RunStatus.Success;
    }

    public static RunStatus NavTurnCC(SmartObject target, SmartCharacterCC character)
    {
        if (character.Controlled)
        {

            character.setTaskMessage(" look towards the target ");
            character.startTimer();

            if (character.goalLight.enabled == false)
            {
                character.goalLight.enabled = true;
            }

            if (character.goalLight.transform.position != target.transform.position)
            {
                character.goalLight.transform.position = target.transform.position;
            }

            Vector3 direction = target.transform.position - character.transform.position;
            float angle = Vector3.Angle(direction, character.transform.forward);

            if (angle < 20)
            {
                // Debug.Log("I'm facing the target");
            }

            if (character.Character.Body.NavIsFacingDesired() == true)
            {
                character.Character.Body.NavSetOrientationBehavior(
                    OrientationBehavior.LookForward);

                return RunStatus.Success;
            }

            return RunStatus.Running;
        }
        else
        {
            character.Character.Body.NavSetOrientationBehavior(OrientationBehavior.None);
            character.Character.Body.NavSetDesiredOrientation(target.transform.position);
            if (character.Character.Body.NavIsFacingDesired() == true)
            {
                character.Character.Body.NavSetOrientationBehavior(
                    OrientationBehavior.LookForward);

                return RunStatus.Success;
            }

            return RunStatus.Running;
        }
    }

    #endregion


    #region Interaction

    public static Node CharacterInteraction(
       Val<string> gestureName, SmartCharacterCC character1, SmartCharacterCC character2)
    {

        return new Sequence(
            new SequenceParallel(
                    character1.Node_OrientTowards(Val.V(() => character2.transform.position)),
                    character2.Node_OrientTowards(Val.V(() => character1.transform.position))
                ),
            new SequenceParallel(
                character1.ST_PlayHandGesture(gestureName, 2000),
                character2.ST_PlayHandGesture(gestureName, 2000)
                )
            ///, new LeafInvoke(() => Debug.Log("Finished a character interaction: " + gestureName.Value))
            );
    }

    public Node CharacterInteraction(
      Val<string> gestureName, SmartCharacterCC character)
    {

        return new Sequence(
            new LeafInvoke(() => this.interacting = (int)character.gameObject.GetComponent<AssignId>().Id),
            new LeafInvoke(() => character.interacting = (int)(this.gameObject.GetComponent<AssignId>().Id)),
            new SequenceParallel(
                this.Node_OrientTowards(Val.V(() => character.transform.position)),
                character.Node_OrientTowards(Val.V(() => this.transform.position))
                ),
            new SequenceParallel(
                new LeafInvoke(() => this.BaseSetIcon(gestureName.Value)),
                new LeafInvoke(() => character.BaseSetIcon(gestureName.Value)),

                this.ST_PlayHandGesture(gestureName, 3000),
                character.ST_PlayHandGesture(gestureName, 3000)
                ),
            new LeafInvoke(() => this.interacting = -1),
            new LeafInvoke(() => character.interacting = -1)
            );
    }

    public Node CharacterInteraction(
      Val<string> gestureName, SmartObject character)
    {
        if (character.GetType() == typeof(SmartWaypoint))
        {
            return new Sequence(
           new LeafInvoke(() => this.interacting = (int)character.gameObject.GetComponent<AssignId>().Id),
           new LeafInvoke(() => this.BaseSetIcon(gestureName.Value)),
           this.ST_PlayHandGesture(gestureName, 3000),
           new LeafInvoke(() => this.interacting = -1)
           );
        }

        return new Sequence(
            new LeafInvoke(() => this.interacting = (int)character.gameObject.GetComponent<AssignId>().Id),
            this.Node_OrientTowards(Val.V(() => character.transform.position)),
            new LeafInvoke(() => this.BaseSetIcon(gestureName.Value)),
            this.ST_PlayHandGesture(gestureName, 3000),
            new LeafInvoke(() => this.interacting = -1)
            );
    }

    public Node CharacterInteractionY(
       Val<string> gestureName, SmartObject character)
    {

        return new Sequence(
            new SequenceParallel(
                    this.Node_OrientTowards(Val.V(() => character.transform.position))
                ),
            new SequenceParallel(
                this.ST_PlayHandGesture(gestureName, 3000)//,
                                                          // new LeafInvoke(() => Debug.Log("Finished a character interaction: " + gestureName.Value))
               )
            );
    }

    public static Node Node_Interaction(Val<string> gestureName, SmartCharacterCC character, SmartCharacterCC obj)
    {
        return new Sequence(new LeafInvoke(
            () => GesturePerform3(gestureName, character, obj),
            () => GestureCheck(gestureName, character, obj)
            // () => CheckGestureComplete(gestureName, character, obj)
            )//,
         );
    }

    public static void AdjustPriority()
    {
        SmartCharacterCC.prio += 0.06f;
    }

    public static RunStatus GesturePerform3(Val<string> gestureName, SmartCharacterCC character, SmartObject target)
    {
        Debug.Log("Gesture3");

        int goalStatus = SmartCharacterCC.gesture_map_id[gestureName.Value.ToUpper()];

        if (character.status == goalStatus)
        {

            Debug.Log("--------11Success!");
            return RunStatus.Success;
        }

        return RunStatus.Running;
    }

    public static RunStatus GestureCheck(Val<string> gestureName, SmartCharacterCC character, SmartObject target)
    {
        Debug.Log("GestureCheck");
        int goalStatus = SmartCharacterCC.gesture_map_id[gestureName.Value.ToUpper()];

        if (character.status == goalStatus)
        {

            return RunStatus.Success;
        }
        return RunStatus.Running;

    }

    public Node ST_PlayHandGesture(
        Val<string> gestureName, Val<long> duration)
    {
        return new DecoratorCatch(
                () => this.status = -1,
            new DecoratorCatch(
            () => this.HandAnimation(gestureName, false),
                new DecoratorCatch(
                   () => Debug.Log(""),
                    new Sequence(
                        //new LeafInvoke(() => Debug.Log("ST_PlayHandGesture start '" + gestureName.Value.ToUpper() + "'")),
                        //new LeafInvoke(() => Debug.Log("1")),
                        Node_HandAnimation_NI(gestureName, true),
                        new LeafWait(duration),
                        Node_HandAnimation_NI(gestureName, false),
                new LeafInvoke(() => this.status = SmartCharacterCC.gesture_map_id[gestureName.Value.ToUpper()])
                ))
            ));
    }


    public RunStatus HandAnimation(
        Val<string> gestureName, Val<bool> isActive)
    {

        if (gestureName.Value.ToUpper().Equals("DIE"))
        {

            HandAnimation_NI(gestureName.Value, isActive.Value);
            return RunStatus.Success;
        }

        this.HandAnimation(gestureName.Value, isActive.Value);
        return RunStatus.Success;
    }

    public void HandAnimation(string gestureName, bool isActive)
    {
        if (isActive == true)
        {
            this.ResetAnimation2();
            this.status = gesture_map_id[gestureName.ToUpper()];
        }
        else
        {
            this.status = -1;
        }

        Character.Body.HandAnimation(gestureName, isActive);
        this.animator.SetBool("HandAnimation", isActive);

        if (isActive)
        {
            this.animator.SetLayerWeight(1, 1);
            resettingHandLayerWeight = false;
        }
        else
        {
            resettingHandLayerWeight = true;
        }

        switch (gestureName.ToUpper())
        {
            case "BOW":
                if (isActive)
                    this.animator.SetTrigger("H_Bow_T");
                break;
            case "SHAKEHAND":
                this.animator.SetBool("H_ShakeHand", isActive);
                break;
            case "OPENDOOR":
                this.animator.SetBool("H_OpenDoor", isActive);
                break;
            case "CLOSEDOOR":
                this.animator.SetBool("H_CloseDoor", isActive);
                break;
            case "NOD":
                this.animator.SetBool("H_Nod", isActive);
                break;
            case "DIE":
                this.animator.SetBool("H_Die", isActive);
                break;
            case "FIGHT":
                this.animator.SetBool("H_Fight", isActive);
                break;
            case "INTIMIDATE":
                this.animator.SetBool("H_Intimidate", isActive);
                break;
            case "FISTSHAKE":
                this.animator.SetBool("H_FistShake", isActive);
                break;
            case "THREATEN":
                this.animator.SetBool("H_Threaten", isActive);
                break;
            case "YELL1":
                this.animator.SetBool("H_Yell1", isActive);
                break;
            case "YELL2":
                this.animator.SetBool("H_Yell2", isActive);
                break;
            case "HOOKRIGHT":
                this.animator.SetBool("H_HookRight", isActive);
                break;
            case "MOURN":
                this.animator.SetBool("H_Cry2", isActive);
                break;
            case "CHEERHAPPILY":
                this.animator.SetBool("H_Cheer2", isActive);
                break;
            case "YELLANGRILY":
                this.animator.SetBool("H_Yell2", isActive);
                break;

        }
    }

    public void ResetAnimation2()
    {
        this.animator.SetBool("H_Bow", false);
        this.animator.SetBool("H_ShakeHand", false);
        this.animator.SetBool("H_OpenDoor", false);
        this.animator.SetBool("H_CloseDoor", false);
        this.animator.SetBool("H_HookRight", false);
        this.animator.SetBool("H_Nod", false);
        this.animator.SetBool("H_Die", false);
        this.animator.SetBool("H_Fight", false);
        this.animator.SetBool("H_Intimidate", false);
        this.animator.SetBool("H_FistShake", false);
        this.animator.SetBool("H_Threaten", false);
        this.animator.SetBool("H_Yell1", false);
        this.animator.SetBool("H_Yell2", false);
        this.animator.SetBool("H_Happy", false);
        this.animator.SetBool("H_FistPump", false);
        this.animator.SetBool("H_Dismiss", false);
        this.animator.SetBool("H_Forward", false);

        this.animator.SetBool("H_Cry2", false);
        this.animator.SetBool("H_Cheer2", false);

        this.animator.SetBool("HandAnimation", false);
    }

    public void BodyUpdate()
    {
        if (resettingHandLayerWeight)
        {
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0, Time.deltaTime));
        }
        if (resettingFaceLayerWeight)
        {
            animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0, Time.deltaTime));
        }
    }

    #endregion

    public static Node CrowdHelp(SmartCharacterCC character1, SmartCharacterCC character2)
    {

        return CrowdHelp(character1, character2, character1.gameObject.name + ", please go to the wilderness");
    }

    public static Node CrowdHelp(SmartCharacterCC character1, SmartCharacterCC character2, string message)
    {

        return new Sequence(
            new Selector(
                new LeafAssert(() => character2 == null),
                new LeafAssert(() => true)
                ),
            character2.Node_GoToUpToRadius(Val.V(() => character1.transform.position), 0.7f),
            Node_Icon(character2, message),
           new LeafWait(5000),
            Node_Icon(character2, null)
            );
    }

    public static void CrowdHelpInitiate(SmartCharacterCC character1, Val<string> objective, Val<string> target)
    {
        SmartCharacterCC obj = character1.collider_crowd.GetNearestObject();

        if (obj == null)
        {
            // Debug.Log("Nearest Crowd not here");
            return;
        }
        //  Debug.Log("debug: Nearest Crowd Collider: " + obj.gameObject.name);
        SmartCharacterCC character = obj.GetComponent<SmartCharacterCC>();

        SmartObject[] participants = { character };
        SmartCharacterCC.AdjustPriority();

        BehaviorEvent e = new BehaviorEvent(node => SmartCharacterCC.CrowdHelp(character1, character, objective, target), participants);
        e.StartEvent(SmartCharacterCC.prio);
    }

    public static Node CrowdHelp(SmartCharacterCC character1, SmartCharacterCC character2, Val<string> objective, Val<string> target)
    {

        return new Sequence(
            character2.Node_GoToUpToRadius(Val.V(() => character1.transform.position), 0.7f),
            character2.Node_Icon(SmartCharacterCC.GetIcon(objective, target)),
            new LeafWait(3000),
            character2.Node_Icon(null)
            );
    }

    public static string GetIcon(Val<string> objective, Val<string> target)
    {
        string result = "";

        switch (objective.Value.ToUpper())
        {
            case "GOTO":
                result += "goto";
                break;
            case "INTERACT":
                result += "interact";
                break;
            case "DOOR":
                result += "door";
                break;
            case "WAYPOINT":
                result += "waypoint";
                break;
        }

        switch (target.Value.ToUpper())
        {
            case "GOTO":
                result += "goto";
                break;
            case "INTERACT":
                result += "interact";
                break;
            case "DOOR":
                result += "door";
                break;

        }

        return "sad";
    }

    public Node ST_PlayHandGesture_NI2(
        Val<string> gestureName, Val<long> duration)
    {
        return this.Node_HandAnimation_NI(gestureName, true);
    }

    public Node ST_PlayHandGesture_NI(
        Val<string> gestureName, Val<long> duration)
    {
        return new DecoratorCatch(
            () => this.HandAnimation_NI(gestureName.Value, false),
            new Sequence(
                this.Node_HandAnimation_NI(gestureName, true),
                new LeafWait(duration),
                this.Node_HandAnimation_NI(gestureName, false)));
    }

    public Node Node_HandAnimation_NI(Val<string> gestureName, Val<bool> start)
    {

        return
            new LeafInvoke(
            () => this.HandAnimation_NI(gestureName.Value, start.Value),
            () => this.HandAnimation_NI(gestureName.Value, false));
    }

    public void HandAnimation_NI(string gestureName, bool isActive)
    {

        if (isActive == true)
        {
            this.ResetAnimation2();
        }
        else
        {
            this.status = -1;
        }

        Character.Body.HandAnimation(gestureName, isActive);
        this.animator.SetBool("HandAnimation", isActive);

        if (isActive)
        {
            this.animator.SetLayerWeight(1, 1);
            resettingHandLayerWeight = false;
        }
        else
        {
            resettingHandLayerWeight = true;
        }

        switch (gestureName.ToUpper())
        {
            case "BOW":
                if (isActive)
                    this.animator.SetTrigger("H_Bow_T");
                break;
            case "SHAKEHAND":
                this.animator.SetBool("H_ShakeHand", isActive);
                break;
            case "OPENDOOR":
                this.animator.SetBool("H_OpenDoor", isActive);
                break;
            case "CLOSEDOOR":
                this.animator.SetBool("H_CloseDoor", isActive);
                break;
            case "NOD":
                this.animator.SetBool("H_Nod", isActive);
                break;
            case "DIE":
                this.animator.SetBool("H_Die", isActive);
                break;
            case "FIGHT":
                this.animator.SetBool("H_Fight", isActive);
                break;
            case "INTIMIDATE":
                this.animator.SetBool("H_Intimidate", isActive);
                break;
            case "FISTSHAKE":
                this.animator.SetBool("H_FistShake", isActive);
                break;
            case "THREATEN":
                this.animator.SetBool("H_Threaten", isActive);
                break;
            case "YELL1":
                this.animator.SetBool("H_Yell1", isActive);
                break;
            case "YELL2":
                this.animator.SetBool("H_Yell2", isActive);
                break;
            case "HOOKRIGHT":
                this.animator.SetBool("H_HookRight", isActive);
                break;
            case "MOURN":
                this.animator.SetBool("H_Cry2", isActive);
                break;
            case "CHEERHAPPILY":
                this.animator.SetBool("H_Cheer2", isActive);
                break;
            case "YELLANGRILY":
                this.animator.SetBool("H_Yell2", isActive);
                break;
        }
    }

    public static Node Node_Icon(SmartCharacterCC character, Val<string> name)
    {
        return new LeafInvoke(
            () => character.SetIcon((name == null) ? null : name.Value));
    }

    public void SetIcon(string name)
    {
        if (this.icon != null)
            this.icon.Icon = name;
    }

    public void BaseSetIcon(string name)
    {

        if (name == null || name == "")
            base.SetIcon(null);
        else
            base.SetIcon(name.ToUpper());
    }

    public Node Inspect(int duration)
    {
        return new Sequence(

            new LeafInvoke(() => this.Unselect()),
                    new Selector(
                        new Sequence(
                            new LeafAssert(() => this.particles != null),
                            new LeafInvoke(() => this.particles.enableEmission = true)
                            ),
                        new LeafAssert(() => true)
                        ),

                new LeafWait(1000),
                new LeafInvoke(() => this.gameObject.SetActive(false)),
                new LeafWait(1000),
            new LeafWait(duration),
           new LeafInvoke(() => this.gameObject.SetActive(true)),
           new Selector(
                        new Sequence(
                            new LeafAssert(() => this.particles != null),
                            new LeafInvoke(() => this.particles.enableEmission = true)
                            ),
                        new LeafAssert(() => true)
                        ),
            new LeafWait(1000),
             new Selector(
                        new Sequence(
                            new LeafAssert(() => this.particles != null),
                            new LeafInvoke(() => this.particles.enableEmission = false)
                            ),
                        new LeafAssert(() => true)
                        )
            );
    }

    void setAnimation(string animationState)
    {

        if (currentAnimation == animationState)
        {
            this.animator.ResetTrigger(animationState);
        }
        else
        {
            if (currentAnimation != null)
                animator.ResetTrigger(currentAnimation);
            animator.SetTrigger(animationState);
            currentAnimation = animationState;
        }
    }


    public new Node Node_GoTo(SmartObject position)
    {
        return SmartCharacterCC.Node_GoTo(position, this);
    }

    public static Node Node_GoTo(SmartObject targ, SmartCharacterCC character)
    {
        return new LeafInvoke(
            () => SmartCharacterCC.NavGoTo(targ, character),
            () => SmartCharacterCC.NavReachedGoal(targ, character));
    }
}