using RootMotion.FinalIK;
using TreeSharpPlus;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//There are still a bunch of duplicate attributes
public class SmartCharacterControl : SmartObject
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

    public static string[] AngryConversation
        = {"YELL1",
          "INTIMIDATE",
          "THREATEN",
          "FISTSHAKE"};

    public static string[] scenario1
        = {"STAYAWAY",
          "SURPRISED",
          "CRY",
          "DIE"};

    public static string[] scenario2
        = {"STAYAWAY",
          "POINTING",
          "HOOKRIGHT",
          "DIE"};

    public static string[] scenario3
        = {"TALK1",
          "TALK2",
          "TALK3",
          "NOD"};

    public static Dictionary<int, string> icon_display_map;
    public static Dictionary<int, string> map_anim_state;
    public static Dictionary<string, string> map_anim_state_name;

    static SmartCharacterControl()
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

        SmartCharacterControl.icon_display_map = new Dictionary<int, string>();
        SmartCharacterControl.icon_display_map.Add(1, "goto_fountain");
        SmartCharacterControl.icon_display_map.Add(2, "goto_palace");
        SmartCharacterControl.icon_display_map.Add(3, "goto_synagogue");
        SmartCharacterControl.icon_display_map.Add(4, "inspect_house");

        SmartCharacterControl.map_anim_state = new Dictionary<int, string>();
        SmartCharacterControl.map_anim_state.Add(Animator.StringToHash("HandGesture.Bow"), "BOW");
        SmartCharacterControl.map_anim_state.Add(Animator.StringToHash("HandGesture.ShakeHand"), "SHAKEHAND");
        SmartCharacterControl.map_anim_state.Add(Animator.StringToHash("HandGesture.OpenDoor"), "OPENDOOR");
        SmartCharacterControl.map_anim_state.Add(Animator.StringToHash("HandGesture.Die"), "DIE");
        SmartCharacterControl.map_anim_state.Add(Animator.StringToHash("HandGesture.HookRight"), "HOOKRIGHT");
        SmartCharacterControl.map_anim_state.Add(Animator.StringToHash("HandGesture.Yell2"), "YELLANGRILY");
        SmartCharacterControl.map_anim_state.Add(Animator.StringToHash("HandGesture.FistShake"), "FISTSHAKE");
        SmartCharacterControl.map_anim_state.Add(Animator.StringToHash("HandGesture.Threaten"), "THREATEN");
        SmartCharacterControl.map_anim_state.Add(Animator.StringToHash("HandGesture.Cry2"), "MOURN");
        SmartCharacterControl.map_anim_state.Add(Animator.StringToHash("HandGesture.Cheer2"), "CHEERHAPPILY");


        SmartCharacterControl.map_anim_state_name = new Dictionary<string, string>();
        SmartCharacterControl.map_anim_state_name.Add("BOW", "HandGesture.Bow");
        SmartCharacterControl.map_anim_state_name.Add("SHAKEHAND", "HandGesture.ShakeHand");
        SmartCharacterControl.map_anim_state_name.Add("OPENDOOR", "HandGesture.OpenDoor");
        SmartCharacterControl.map_anim_state_name.Add("HOOKRIGHT", "HandGesture.HookRight");
        SmartCharacterControl.map_anim_state_name.Add("YELLANGRILY", "HandGesture.Yell2");
        SmartCharacterControl.map_anim_state_name.Add("FISTSHAKE", "HandGesture.FistShake");
        SmartCharacterControl.map_anim_state_name.Add("THREATEN", "HandGesture.Threaten");
        SmartCharacterControl.map_anim_state_name.Add("MOURN", "HandGesture.Cry2");
        SmartCharacterControl.map_anim_state_name.Add("CHEERHAPPILY", "HandGesture.Cheer2");
    }

    public Animator animator { private set; get; }
    public Animation animation { private set; get; }
    public CharacterMecanimControl character { private set; get; }
    public BehaviorMecanimControl behavior { private set; get; }

    public InteractionObject InteractionOpenDoor;
    public InteractionObject InteractionHandShake;

    public BehaviorMecanimControl Behavior { get { return this.behavior; } }
    public CharacterMecanimControl Character { get { return this.character; } }


    public Transform MarkerHead; public FadingObject Backpack; public InteractionObject InteractionHoldRight; public InteractionObject InteractionTake;
    private Interpolator<Vector3> IncapacitateNudge = null;

    delegate Node CurrentTask(Val<Vector3> position);
    public Transform trackTarget = null;

    public Light light;
    public Light goalLight;
    string currentAnimation = null;

    private bool resettingHandLayerWeight;
    private bool resettingFaceLayerWeight;

    public bool Controlled { private set; get; }
    public string curTaskMessage { get; set; }

    public override string Archetype { get { return "SmartCharacterControl"; } }

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

    SmartCharacterControl helper = null;

    InteractionCollider collider_interaction;
    CrowdColliderCC collider_crowd;

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
        character = this.gameObject.GetComponent<CharacterMecanimControl>();
        behavior = this.gameObject.GetComponent<BehaviorMecanimControl>();

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
        this.collider_crowd = GetComponentInChildren<CrowdColliderCC>();

        if (collider_crowd == null)
        {
           // Debug.Log("Empty collider crowd");
        }

        if (collider_interaction == null)
        {
            Debug.Log("Empty collider interaction");
        }

        if (goalLight != null)
            this.goalLight.enabled = false;

        this.helpOnce = false;
        this.helpable = true;

        bowState = Animator.StringToHash("HandGesture.Bow");    //Testing animation hash for detection

        this.icon = this.gameObject.GetComponentInChildren<StatusIcon2>();

        this.particles = this.gameObject.GetComponentInChildren<ParticleSystem>();
        if (this.particles != null)
        {
            Debug.Log("Particles exist: " + this.gameObject.name);
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
    }

    protected void DetectAnimation()
    {
        curAnimState = this.animator.GetCurrentAnimatorStateInfo(1);

        if (this.animator.IsInTransition(1) && SmartCharacterControl.map_anim_state.ContainsKey(curAnimState.nameHash) && this.status != -1)
        {

            string gesture = SmartCharacterControl.map_anim_state[curAnimState.nameHash];
            if (gesture.ToUpper().Equals("DIE"))
            {
                this.Controllable = false;
                return;
            }
            Debug.Log(gesture + " has been detected");

            this.status = -1;
            base.SetIcon(null);
            if (Controlled)
            {
                Character.Body.ResetAnimation();
                //Character.Body.SetResetHandAnimation(true);
                this.HandAnimation(gesture, false);
                Character.NavGoTo(this.gameObject.transform.position); //Character has less chance of getting stuck after animation with this line.
            }
        }

    }

    #region Nudge
    public void Action_GetNearestSmartCrowd()
    {

        SmartCharacterControl character = this.collider_crowd.GetNearestObject();
        if (character == null)
        {
            // Debug.Log("Nearest Crowd not here");
            return;
        }

        SmartObject[] participants = { this, character };
        BehaviorEvent e = new BehaviorEvent(node => SmartCharacterControl.CrowdHelp(this, character), participants);
        //  SmartCharacterControl.prio += 0.1f;
        SmartCharacterControl.AdjustPriority();

        e.StartEvent(SmartCharacterControl.prio);
    }

    public void SmartCrowdAssist(string message)
    {
        SmartCharacterControl character = this.collider_crowd.GetNearestObject();
        if (character == null)
        {
            Debug.Log("Nearest Crowd not here");
            return;
        }

        SmartObject[] participants = { character };
        BehaviorEvent e = new BehaviorEvent(node => SmartCharacterControl.CrowdHelp(this, character, message), participants);
        //  SmartCharacterControl.prio += 0.1f;
        SmartCharacterControl.AdjustPriority();

        e.StartEvent(SmartCharacterControl.prio);
    }
    #endregion

    public void EndEvent(
    BehaviorEvent sender,
    EventStatus newStatus)
    {
        if (newStatus == EventStatus.Finished)
        {
            // Debug.Log(sender.Name + "Finished");
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
        this.Character.Body.HandAnimation(gestureName.Value, true);
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

        SmartCharacterControl char1 = obj.gameObject.GetComponent<SmartCharacterControl>();
        if (char1 != null)
        {
            char1.BaseSetIcon("SHAKEHAND");

            char1.PlayGesture("SHAKEHAND", this);
        }
    }

    public void Action_Bow()
    {

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
        SmartCharacterControl char1 = obj.gameObject.GetComponent<SmartCharacterControl>();
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

        SmartCharacterControl char1 = obj.gameObject.GetComponent<SmartCharacterControl>();
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
        // Debug.Log(gameObject.name + ", please " + curTaskMessage);
    }

    public void Select()
    {
        if (!Controllable)
            return;
        // Debug.Log("This character got selected, yo");
        this.Controlled = true;
        this.light.enabled = true;
        this.needAssistance = true;
        initTimer(SmartCharacterControl.TIMER_VAL);
        helpOnce = false;
        helpable = true;
    }

    public void Unselect()
    {
        //Debug.Log("This character got unselected, yo");
        this.Controlled = false;
        if (this.light != null)
            this.light.enabled = false;
        this.needAssistance = false;
        stopTimer();
    }


    #region Control
    public void MoveCharacter(Val<Vector3> target)
    {
        //Debug.Log("Move character to " + target.Value);

        Character.NavGoTo(target);
    }

    public void StopMoving()
    {
        //   Debug.Log("Stop moving");
        Behavior.Character.Body.NavStop();
    }

    public void TurnTowards(Val<Vector3> target)
    {
        //  this.characterCC.Body.NavSetDesiredOrientation(target.Value);
        Character.NavTurn(target);

    }

    #endregion

    #region Behavior Affordances

    public new Node Node_GoToUpToRadius(SmartObject targ, Val<float> dist)
    {
        return SmartCharacterControl.Node_GoToUpToRadius(targ, dist, this);
    }
    #endregion


    #region Nodes

    public RunStatus Node_CheckInteraction(SmartObject target, Val<string> gestureName)
    {

        if (this.interacting == (int)target.gameObject.GetComponent<AssignId>().Id && this.status == SmartCharacterControl.gesture_map_id[gestureName.Value.ToUpper()])
        {
            IndicatorLight(target, false);
            return RunStatus.Success;
        }

        return RunStatus.Running;
    }

    public RunStatus Node_CheckPerformAt(SmartObject target, Val<string> gestureName)
    {

        if ((target.transform.position - this.transform.position).magnitude < SmartCharacterControl.nearDistance && this.status == SmartCharacterControl.gesture_map_id[gestureName.Value.ToUpper()])
        {
            IndicatorLight(target, false);
            return RunStatus.Success;
        }

        return RunStatus.Running;
    }

    public RunStatus Node_CheckPerformTo(SmartObject target, Val<string> gestureName)
    {

        if ((target.transform.position - this.transform.position).magnitude < SmartCharacterControl.interactionDistance && this.status == SmartCharacterControl.gesture_map_id[gestureName.Value.ToUpper()])
        {
            IndicatorLight(target, false);
            return RunStatus.Success;
        }

        return RunStatus.Running;
    }

    public RunStatus Node_CheckPerform(Val<string> gestureName)
    {
        if (this.status == SmartCharacterControl.gesture_map_id[gestureName.Value.ToUpper()])
        {
            return RunStatus.Success;
        }

        return RunStatus.Running;
    }

    public RunStatus Interact_Clean(SmartCharacterControl target)
    {
        this.status = -1;
        target.status = -1;
        this.helpable = true;
        this.BaseSetIcon(null);

        initTimer(SmartCharacterControl.TIMER_VAL);



        return RunStatus.Success;
    }

    public RunStatus Interact_Clean()
    {
        this.status = -1;
        this.helpable = true;
        this.BaseSetIcon(null);
        initTimer(SmartCharacterControl.TIMER_VAL);


        return RunStatus.Success;
    }

    public Node Node_Interact(SmartCharacterControl target, Val<string> gestureName)
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
                                            new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterControl.interactionDistance),
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
                                            this.CharacterInteraction(gestureName, target.gameObject.GetComponent<SmartCharacterControl>())
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
                                           new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterControl.interactionDistance),
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
                                           new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterControl.nearDistance),
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
                                           new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterControl.nearDistance),
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
                                           new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterControl.interactionDistance),
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
                                           new LeafAssert(() => (target.transform.position - this.transform.position).magnitude < SmartCharacterControl.interactionDistance),
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
                                    //new LeafInvoke(() => Debug.Log("Checking yo: " + this.status + " : " + SmartCharacterControl.gesture_map_id[gestureName.Value.ToUpper()])),
                                    new Selector(
                                        new Sequence(
                                            new LeafAssert(() => this.status == SmartCharacterControl.gesture_map_id[gestureName.Value.ToUpper()]),
                                            //  new LeafInvoke(() => Debug.Log("Match!")),
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
                                new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterControl.interactionDistance),
                                // new LeafInvoke(() => Debug.Log("Interact: Move to target ")),
                                this.Node_GoToX(target),
                                // new LeafInvoke(() => Debug.Log("InteractionGoToX done")),
                                new LeafAssert(() => true)
                            ),
                            //   new LeafInvoke(() => Debug.Log("Interact: In target range")),
                            new Sequence(
                                // new LeafInvoke(() => Debug.Log("This.status:  " + this.status)),
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
                                                                                          //  new LeafInvoke(() => Debug.Log("Controlled"))

                                    )
                                )
                           ),
                           new LeafAssert(() => false)
                       )
                   )
               )
           )//,
            //new LeafInvoke(() => Debug.Log("Done with this yo"))
       ))//, new LeafInvoke(() => Debug.Log("Completed this thing"))
       );
    }

    bool _goto = false;

    public Node Node_GoToX(SmartCharacterControl target)
    {
        return new DecoratorCatch(
            () => IndicatorLight(target, false),
            new DecoratorForceStatus(
                RunStatus.Success,
                    new DecoratorLoop(
                            new Sequence(
                                new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterControl.interactionDistance),
                                //new LeafInvoke(() => Debug.Log("GoToX -- ")),     
                                new Selector(
                                    new Sequence(
                                        // new LeafInvoke(() => Debug.Log("GoTo - checking controlled")),
                                        new LeafAssert(() => this.Controlled),
                                        new LeafInvoke(() => IndicatorLight(target, true)),

                                         new Selector(

                                                new Sequence(
                                                    new LeafAssert(() => this.helpable),
                                                    new LeafInvoke(() => Debug.Log("Helpable")),
                                                    new LeafInvoke(() => this.startTimer()),
                                                    new LeafInvoke(() => Debug.Log("Timer start")),
                                                    new LeafAssert(() => this.checkTimer()),
                                                    new LeafInvoke(() => Debug.Log("timer check")),
                                                    new LeafInvoke(
                                                        () => NavGoToX(target, this, Val.V(() => SmartCharacterControl.interactionDistance))
                                                    ),
                                                    new LeafInvoke(() => Debug.Log("invoked alternative behavior")),
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
                                            () => NavGoToX(this, target, Val.V(() => SmartCharacterControl.interactionDistance))
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
                                       new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterControl.interactionDistance),
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
                                                   () => NavGoToX(this, target, Val.V(() => SmartCharacterControl.interactionDistance))
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
                                       new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterControl.interactionDistance),
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
                                                   () => NavGoToX(this, target, Val.V(() => SmartCharacterControl.interactionDistance))
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
                                       new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterControl.interactionDistance),
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
                                                   () => NavGoToX(this, target, Val.V(() => SmartCharacterControl.interactionDistance))
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
                                       new LeafAssert(() => (target.transform.position - this.transform.position).magnitude >= SmartCharacterControl.interactionDistance),
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
                                                   () => NavGoToX(this, target, Val.V(() => SmartCharacterControl.interactionDistance))
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

    public static RunStatus NavGoToX(SmartCharacterControl character, SmartObject target, Val<float> dist)
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

    public static Node Node_GoToUpToRadius(SmartObject targ, Val<float> dist, SmartCharacterControl character)
    {


        return new LeafInvoke(
            () => SmartCharacterControl.NavGoToUpToRadius(targ, dist, character)//,
            // () => Debug.Log("I Got it ")
            //,
            //() => SmartCharacterControl.NavReachedNearGoal(targ, dist, character)
            );

        /*
         return new DecoratorCatch(
            () => SmartCharacterControl.CleanUp(targ), 
            new LeafInvoke(
            () => SmartCharacterControl.NavGoToUpToRadius(targ, dist, character),
            () => SmartCharacterControl.NavReachedNearGoal(targ, dist, character)));
         */
    }



    public static Node Node_OrientTowardsCC(SmartObject targ, SmartCharacterControl character)
    {
        return new LeafInvoke(
            () => SmartCharacterControl.NavTurnCC(targ, character),
            () => SmartCharacterControl.NavOrientBehaviorCC(OrientationBehavior.LookForward, character));
    }

    /*
     *  Moves the character to the specified location if the character is not controlled 
     *  Continues to run if the character is controleld by user
     */
    public static RunStatus NavGoTo(SmartObject target, SmartCharacterControl character)
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
                character.helper = (SmartCharacterControl)character.collider_crowd.GetNearestObject();

                if (character.helper != null)
                {
                    SmartCharacterControl.CrowdHelp(character, character.helper);
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

    public static RunStatus NavGoToUpToRadius(SmartObject target, Val<float> dist, SmartCharacterControl character)
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

                    if (target.GetType() == typeof(SmartCharacterControl))
                    {

                        SmartCharacterControl[] participants = { (SmartCharacterControl)target, character };

                        BehaviorEvent e = new BehaviorEvent(node => SmartCharacterControl.Node_GoToUpToRadius(character, dist, (SmartCharacterControl)target), participants);
                        // SmartCharacterControl.CharacterInteraction("bow", this, char1);
                        // SmartCharacterControl.prio += 0.1f;
                        SmartCharacterControl.AdjustPriority();

                        e.StartEvent(SmartCharacterControl.prio);

                        character.helpOnce = true;
                    }
                    else if (target.GetType() == typeof(SmartDoor))
                    {
                        SmartCharacterControl.CrowdHelpInitiate(character, "door", target.gameObject.name);
                        character.helpOnce = true;

                    }
                    else if (target.GetType() == typeof(SmartWaypoint))
                    {
                        SmartCharacterControl.CrowdHelpInitiate(character, "waypoint", target.gameObject.name);
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
    public static RunStatus NavReachedGoal(SmartObject target, SmartCharacterControl character)
    {
        // Debug.Log("Do I get called3?");

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
        Val<OrientationBehavior> behavior, SmartCharacterControl character)
    {
        character.Character.Body.NavSetOrientationBehavior(behavior.Value);
        return RunStatus.Success;
    }

    public static RunStatus NavTurnCC(SmartObject target, SmartCharacterControl character)
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
       Val<string> gestureName, SmartCharacterControl character1, SmartCharacterControl character2)
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
      Val<string> gestureName, SmartCharacterControl character)
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

    public static Node Node_Interaction(Val<string> gestureName, SmartCharacterControl character, SmartCharacterControl obj)
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
        SmartCharacterControl.prio += 0.06f;
    }

    public static RunStatus GesturePerform3(Val<string> gestureName, SmartCharacterControl character, SmartObject target)
    {
        Debug.Log("Gesture3");

        int goalStatus = SmartCharacterControl.gesture_map_id[gestureName.Value.ToUpper()];

        if (character.status == goalStatus)
        {
            
            return RunStatus.Success;
        }

        return RunStatus.Running;
    }

    public static RunStatus GestureCheck(Val<string> gestureName, SmartCharacterControl character, SmartObject target)
    {
        Debug.Log("GestureCheck");
        int goalStatus = SmartCharacterControl.gesture_map_id[gestureName.Value.ToUpper()];

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
                new LeafInvoke(() => this.status = SmartCharacterControl.gesture_map_id[gestureName.Value.ToUpper()])
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
                //               this.animatorCC.SetBool("H_Bow", isActive);
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

    #endregion

    public static Node CrowdHelp(SmartCharacterControl character1, SmartCharacterControl character2)
    {

        return CrowdHelp(character1, character2, character1.gameObject.name + ", please go to the wilderness");
    }

    public static Node CrowdHelp(SmartCharacterControl character1, SmartCharacterControl character2, string message)
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

    public static void CrowdHelpInitiate(SmartCharacterControl character1, Val<string> objective, Val<string> target)
    {
        SmartCharacterControl obj = character1.collider_crowd.GetNearestObject();

        if (obj == null)
        {
            // Debug.Log("Nearest Crowd not here");
            return;
        }
        //  Debug.Log("debug: Nearest Crowd Collider: " + obj.gameObject.name);
        SmartCharacterControl character = obj.GetComponent<SmartCharacterControl>();

        SmartObject[] participants = { character };
        SmartCharacterControl.AdjustPriority();

        BehaviorEvent e = new BehaviorEvent(node => SmartCharacterControl.CrowdHelp(character1, character, objective, target), participants);
        e.StartEvent(SmartCharacterControl.prio);
    }

    public static Node CrowdHelp(SmartCharacterControl character1, SmartCharacterControl character2, Val<string> objective, Val<string> target)
    {

        return new Sequence(
            character2.Node_GoToUpToRadius(Val.V(() => character1.transform.position), 0.7f),
            character2.Node_Icon(SmartCharacterControl.GetIcon(objective, target)),
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

    public static Node Node_Icon(SmartCharacterControl character, Val<string> name)
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
    //#####


    protected struct AnimationDescription
    {
        public readonly string Name;
        public readonly AnimationLayer Layer;

        public AnimationDescription(string name, AnimationLayer layer)
        {
            this.Name = name;
            this.Layer = layer;
        }
    }

    #region AnimationDescriptions for various conversation types
    private Tuple<AnimationDescription[], AnimationDescription[]> normalConversation =
        new Tuple<AnimationDescription[], AnimationDescription[]>(
            new AnimationDescription[] {
                new AnimationDescription("cheer", AnimationLayer.Hand),
                new AnimationDescription("BeingCocky", AnimationLayer.Hand),
                new AnimationDescription("acknowledge", AnimationLayer.Face),
                new AnimationDescription("lookaway", AnimationLayer.Face)
            },
            new AnimationDescription[] {
                new AnimationDescription("HeadNod", AnimationLayer.Face),
                new AnimationDescription("HeadShake", AnimationLayer.Face),
                new AnimationDescription("chestpumpsalute", AnimationLayer.Hand),
                new AnimationDescription("cowboy", AnimationLayer.Hand)
            });

    private Tuple<AnimationDescription[], AnimationDescription[]> secretiveConversation =
        new Tuple<AnimationDescription[], AnimationDescription[]>(
            new AnimationDescription[] {
                new AnimationDescription("acknowledge", AnimationLayer.Face),
                new AnimationDescription("headnod", AnimationLayer.Face)
            },
            new AnimationDescription[] {
                new AnimationDescription("acknowledge", AnimationLayer.Face),
                new AnimationDescription("headshake", AnimationLayer.Face)
            });

    private Tuple<AnimationDescription[], AnimationDescription[]> happyConversation =
        new Tuple<AnimationDescription[], AnimationDescription[]>(
            new AnimationDescription[] {
                new AnimationDescription("cheer", AnimationLayer.Hand),
                new AnimationDescription("BeingCocky", AnimationLayer.Hand),
                new AnimationDescription("acknowledge", AnimationLayer.Face),
                new AnimationDescription("lookaway", AnimationLayer.Face)
            },
            new AnimationDescription[] {
                new AnimationDescription("HeadNod", AnimationLayer.Face),
                new AnimationDescription("HeadShake", AnimationLayer.Face),
                new AnimationDescription("chestpumpsalute", AnimationLayer.Hand),
                new AnimationDescription("cowboy", AnimationLayer.Hand)
            });

    private Tuple<AnimationDescription[], AnimationDescription[]> haggle =
        new Tuple<AnimationDescription[], AnimationDescription[]>(
            new AnimationDescription[] {
                new AnimationDescription("cowboy", AnimationLayer.Hand),
                new AnimationDescription("wonderful", AnimationLayer.Hand),
                new AnimationDescription("HeadShakeThink", AnimationLayer.Face),
                new AnimationDescription("Acknowledge", AnimationLayer.Face)
            },
            new AnimationDescription[] {
                new AnimationDescription("LookAway", AnimationLayer.Face),
                new AnimationDescription("HeadNod", AnimationLayer.Face),
                new AnimationDescription("crowdpump", AnimationLayer.Hand),
                new AnimationDescription("cowboy", AnimationLayer.Hand)
            });
    #endregion
    
    /// <summary>
    /// Interaction point for reaching to give the ball
    /// </summary>
    public InteractionObject InteractionGive;

    /// <summary>
    /// Interaction point for reaching when stealing the wallet
    /// </summary>
    public InteractionObject InteractionStealWallet;

    /// <summary>
    /// Interaction point for reaching on self when storing the wallet
    /// </summary>
    public InteractionObject InteractionStoreWallet;

    /// <summary>
    /// Reflexive interaction point for pointing a gun
    /// </summary>
    public InteractionObject InteractionPointGun;

    /// <summary>
    /// Reflexive interaction point for pointing a gun into the air.
    /// </summary>
    public InteractionObject InteractionPointGunUpwards;

    /// <summary>
    /// For giving a key during coercion
    /// </summary>
    public InteractionObject InteractionCoerceKeyGive;

    /// <summary>
    /// For taking a key during coercion
    /// </summary>
    public InteractionObject InteractionCoerceKeyTake;

    /// <summary>
    /// For taking a key from an incapacitated character
    /// </summary>
    public InteractionObject InteractionGetKeyIncapacitated;

    /// <summary>
    /// For taking a gun from an incapacitated character
    /// </summary>
    public InteractionObject InteractionGetGunIncapacitated;

    /// <summary>
    /// For surrendering
    /// </summary>
    public InteractionObject InteractionSurrenderLeft;

    /// <summary>
    /// For surrendering
    /// </summary>
    public InteractionObject InteractionSurrenderRight;

    /// <summary>
    /// For dropping a weapon
    /// </summary>
    public InteractionObject InteractionDropGun;

    /// <summary>
    /// The prop holder for the right hand
    /// </summary>
    public PropHolder HoldPropRightHand;

    /// <summary>
    /// The prop holder for the left hand
    /// </summary>
    public PropHolder HoldPropLeftHand;

    /// <summary>
    /// The prop holder for the pocket
    /// </summary>
    public PropHolder HoldPropPocket;

    /// <summary>
    /// A hidden prop holder for objects to vanish but still be referencable
    /// </summary>
    public PropHolder HoldPropHidden;

    /// <summary>
    /// Waypoint for standing behind the character.
    /// </summary>
    public Transform WaypointBack;

    /// <summary>
    /// Waypoint for standing in front of the character.
    /// </summary>
    public Transform WaypointFront;

    /// <summary>
    /// Waypoint for picking up a key from an incapacitated character
    /// </summary>
    public Transform WaypointPickupKey;

    /// <summary>
    /// Waypoint for picking up a gun from an incapacitated character
    /// </summary>
    public Transform WaypointPickupGun;

    /// <summary>
    /// Stars decoration
    /// </summary>
    public Stars DecorationStars;
    
    private void CleanupOrientation(SmartObject user)
    {
        user.GetComponent<CharacterMecanim>().NavOrientBehavior(OrientationBehavior.LookForward);
        character.NavOrientBehavior(OrientationBehavior.LookForward);
    }

    public Prop GetRightProp()
    {
        return this.HoldPropRightHand.CurrentProp;
    }

    [Affordance]
    protected Node RaiseGun(SmartObject user)
    {
        return this.Node_StartInteraction(FullBodyBipedEffector.RightHand, this.InteractionPointGun);
    }

    [Affordance]
    protected Node LowerGun(SmartObject user)
    {
        return this.Node_StopInteraction(FullBodyBipedEffector.RightHand);
    }

    [Affordance]
    protected Node Surrender(SmartCharacter user)
    {
        return new Sequence(
            this.Node_StartInteraction(FullBodyBipedEffector.LeftHand, this.InteractionSurrenderLeft),
            this.Node_StartInteraction(FullBodyBipedEffector.RightHand, this.InteractionSurrenderRight),
            new LeafWait(1200),
            this.Node_Set(StateName.IsImmobile));
    }

    [Affordance]
    protected Node CoerceGiveKey(SmartCharacter user)
    {
        return new Sequence(
            //user.ST_StandAtWaypoint(this.WaypointFront),
            user.Node_Icon("key"),
            this.Node_StartInteraction(FullBodyBipedEffector.LeftHand, this.InteractionCoerceKeyTake),
            new LeafWait(1000),
            user.Node_StartInteraction(FullBodyBipedEffector.RightHand, this.InteractionCoerceKeyGive),
            new LeafWait(1000),
            user.Node_Icon(null),
            this.Node_Icon("key"),
            this.Node_StopInteraction(FullBodyBipedEffector.LeftHand),
            new LeafWait(300),
            user.Node_StopInteraction(FullBodyBipedEffector.RightHand),
            new LeafWait(700),
            this.Node_Icon(null));
    }

    [Affordance]
    protected Node GetKeyIncapacitated(SmartCharacter user)
    {
        return new Sequence(
            user.ST_StandAtWaypoint(this.WaypointPickupKey),
            user.Node_StartInteraction(FullBodyBipedEffector.LeftHand, this.InteractionGetKeyIncapacitated),
            new LeafWait(1000),
            user.Node_Icon("key"),
            user.Node_StopInteraction(FullBodyBipedEffector.LeftHand),
            new LeafWait(1000),
            user.Node_Set(StateName.HasKeys),
            this.Node_Set(~StateName.HasKeys),
            user.Node_Icon(null));
    }

    [Affordance]
    protected Node GetGunIncapacitated(SmartCharacter user)
    {
        return new Sequence(
            user.ST_StandAtWaypoint(this.WaypointPickupGun),
            user.Node_StartInteraction(FullBodyBipedEffector.RightHand, this.InteractionGetGunIncapacitated),
            new LeafWait(500),
            new LeafInvoke(() => this.GetRightProp().FadeOut()),
            new LeafWait(500),
            new LeafInvoke(() => user.HoldPropRightHand.Attach(this.HoldPropRightHand.Release())),
            new LeafInvoke(() => user.GetRightProp().FadeIn()),
            user.Node_StopInteraction(FullBodyBipedEffector.RightHand),
            this.Node_Set(~StateName.RightHandOccupied, ~StateName.HoldingWeapon),
            user.Node_Set(StateName.RightHandOccupied, StateName.HoldingWeapon),
            new LeafWait(1000));
    }

    [Affordance]
    protected Node GiveBriefcase(SmartCharacter user)
    {
        return new Sequence(
            user.Node_GoToUpToRadius(Val.V(() => this.transform.position), 1.5f),
            new SequenceParallel(
                user.Node_OrientTowards(Val.V(() => this.transform.position)),
                this.Node_OrientTowards(Val.V(() => user.transform.position))),
            new SequenceParallel(
                user.Node_StartInteraction(FullBodyBipedEffector.LeftHand, this.InteractionGive),
                new Sequence(
                    new LeafWait(1000),
                    this.Node_StartInteraction(FullBodyBipedEffector.LeftHand, this.InteractionTake)),
                user.Node_WaitForTrigger(FullBodyBipedEffector.LeftHand),
                this.Node_WaitForTrigger(FullBodyBipedEffector.LeftHand)),
            new LeafInvoke(() => this.HoldPropLeftHand.Attach(user.HoldPropLeftHand.Release())),
            user.Node_Set(~StateName.HasBackpack),
            this.Node_Set(StateName.HasBackpack),
            new SequenceParallel(
                user.Node_StopInteraction(FullBodyBipedEffector.LeftHand),
                this.Node_StopInteraction(FullBodyBipedEffector.LeftHand)));
    }

    [Affordance]
    protected Node TakeWeaponIncapacitated(SmartCharacter user)
    {
        return new Sequence(
            user.Node_GoToUpToRadius(Val.V(() => this.WaypointFront.position), 1.0f),
            user.Node_OrientTowards(Val.V(() => this.transform.position)),
            //TODO new interaction object to take the weapon?
            user.Node_StartInteraction(FullBodyBipedEffector.RightHand, this.InteractionGive),
            user.Node_WaitForTrigger(FullBodyBipedEffector.RightHand),
            new LeafInvoke(() => user.HoldPropRightHand.Attach(this.HoldPropRightHand.Release())),
            this.Node_Set(~StateName.RightHandOccupied, ~StateName.HoldingWeapon),
            user.Node_Set(StateName.RightHandOccupied, StateName.HoldingWeapon),
            new LeafInvoke(() => Debug.Log("Here2")),
            user.Node_StopInteraction(FullBodyBipedEffector.RightHand));
    }

    private void CreateSlidebackTarget()
    {
        Vector3 slideBack =
            this.transform.position - (this.transform.forward * 0.4f);
        this.IncapacitateNudge =
            new Interpolator<Vector3>(
                transform.position,
                slideBack,
                Vector3.Lerp);
        this.IncapacitateNudge.ForceMin();
        this.IncapacitateNudge.ToMax(0.8f);
    }



    [Affordance]
    protected Node Incapacitate(SmartCharacterControl user)
    {
        return new Sequence(
            user.Node_Require(StateName.RoleActor, StateName.IsStanding,
                ~StateName.HoldingBall, ~StateName.HoldingDrink, ~StateName.IsIncapacitated),
            this.Node_Require(StateName.RoleActor, StateName.IsStanding, ~StateName.IsDead),
            new LeafAssert(() => user != this), // Not reflexive
            user.Node_GoToUpToRadius(new Val<Vector3>(this.transform.position), 1.0f),
            new LeafInvoke(() => this.DecorationStars.gameObject.SetActive(true)),
            this.Node_Set(StateName.IsIncapacitated)
        );
    }

    [Affordance]
    protected Node WakeUp(SmartObject user)
    {
        // User Casting
        // TODO: Ths is going to get ugly if the cast fails...
        SmartCharacterControl character = (SmartCharacterControl)user;

        return new Sequence(
            user.Node_Require(StateName.RoleActor, StateName.IsStanding, ~StateName.IsIncapacitated),
            this.Node_Require(StateName.RoleActor, StateName.IsIncapacitated, ~StateName.IsDead),
            new LeafAssert(() => user != this), // Not reflexive
            character.Node_GoToUpToRadius(new Val<Vector3>(this.transform.position), 1.0f),
            new LeafInvoke(() => this.DecorationStars.gameObject.SetActive(false)),
            this.Node_Set(~StateName.IsIncapacitated)
        );
    }

    [Affordance]
    protected Node Kill(SmartObject user)
    {
        // User Casting
        // TODO: Ths is going to get ugly if the cast fails...
        SmartCharacterControl character = (SmartCharacterControl)user;

        return new Sequence(
            user.Node_Require(StateName.RoleActor, StateName.IsStanding, ~StateName.HoldingBall, ~StateName.HoldingDrink, ~StateName.IsIncapacitated),
            this.Node_Require(StateName.RoleActor, StateName.IsStanding, ~StateName.IsDead),
            new LeafAssert(() => user != this), // Not reflexive
            character.Node_GoToUpToRadius(new Val<Vector3>(this.transform.position), 1.0f),
            new LeafInvoke(() => this.DecorationStars.gameObject.SetActive(false)),
            this.Node_Icon("skull"),
            this.Node_Set(StateName.IsDead, StateName.IsIncapacitated)
        );
    }

    [Affordance]
    protected Node TakeBall(SmartObject user)
    {
        // User Casting
        // TODO: Ths is going to get ugly if the cast fails...
        SmartCharacterControl character = (SmartCharacterControl)user;

        return new Sequence(
            user.Node_Require(StateName.RoleActor, StateName.IsStanding, ~StateName.HoldingBall, ~StateName.HoldingDrink, ~StateName.HoldingWallet, ~StateName.IsIncapacitated),
            this.Node_Require(StateName.RoleActor, StateName.HoldingBall, StateName.IsIncapacitated),
            character.Node_GoToUpToRadius(new Val<Vector3>(this.transform.position), 1.0f),
            character.Node_OrientTowards(new Val<Vector3>(this.transform.position)),

            new LeafInvoke(() => character.HoldPropRightHand.Attach(this.HoldPropRightHand.Release())),

            this.Node_Set(~StateName.HoldingBall),
            user.Node_Set(StateName.HoldingBall),

            new SequenceParallel(
                this.Node_StopInteraction(FullBodyBipedEffector.RightHand),
                character.Node_StartInteraction(FullBodyBipedEffector.RightHand, character.InteractionHoldRight)));
    }

    [Affordance]
    public Node TakeWallet(SmartObject user)
    {
        SmartCharacterControl character = (SmartCharacterControl)user;

        return new Sequence(
            //user.Node_Require(StateName.RoleActor, StateName.IsStanding, ~StateName.HoldingBall, ~StateName.HoldingWallet, ~StateName.HoldingDrink, ~StateName.IsIncapacitated),
            // this.Node_Require(StateName.RoleActor, StateName.HoldingWallet, StateName.IsIncapacitated),
            character.Node_GoToUpToRadius(new Val<Vector3>(this.transform.position), 1.0f),
            character.Node_OrientTowards(new Val<Vector3>(this.transform.position)),

            new LeafInvoke(() => character.HoldPropRightHand.Attach(this.HoldPropPocket.Release())),

            this.Node_Set(~StateName.HoldingWallet),
            user.Node_Set(StateName.HoldingWallet),

            character.Node_StartInteraction(FullBodyBipedEffector.RightHand, character.InteractionStoreWallet),
            character.Node_WaitForTrigger(FullBodyBipedEffector.RightHand),
            new LeafInvoke(() => character.HoldPropPocket.Attach(character.HoldPropRightHand.Release())),
            character.Node_ResumeInteraction(FullBodyBipedEffector.RightHand));
    }


    [Affordance]
    public Node Steal(SmartObject user)
    {
        SmartCharacterControl character = (SmartCharacterControl)user;

        return new Sequence(
            // user.Node_Require(StateName.RoleActor, StateName.IsStanding, ~StateName.HoldingBall, ~StateName.HoldingWallet, ~StateName.HoldingDrink, ~StateName.IsIncapacitated),
            // this.Node_Require(StateName.RoleActor, StateName.IsStanding, StateName.HoldingWallet, ~StateName.IsIncapacitated),
            character.Node_GoTo(Val.V(() => this.WaypointBack.position)),
            character.Node_OrientTowards(Val.V(() => this.transform.position)),
            new SelectorParallel(
                character.Node_GoToUpToRadius(Val.V(() => this.transform.position), 0.1f),
                    new LeafWait(1000)), // Timeout
            character.Node_OrientTowards(Val.V(() => this.transform.position)),
            character.Node_StartInteraction(FullBodyBipedEffector.RightHand, this.InteractionStealWallet),
            character.Node_WaitForTrigger(FullBodyBipedEffector.RightHand),
            character.Node_ResumeInteraction(FullBodyBipedEffector.RightHand)
            // new LeafInvoke(() => character.HoldPropRightHand.Attach(this.HoldPropPocket.Release())),

            // this.Node_Set(~StateName.HoldingWallet),
            // user.Node_Set(StateName.HoldingWallet),

            //character.Node_StartInteraction(FullBodyBipedEffector.RightHand, character.InteractionStoreWallet),
            //character.Node_WaitForTrigger(FullBodyBipedEffector.RightHand)
            // new LeafInvoke(() => character.HoldPropPocket.Attach(character.HoldPropRightHand.Release())),
            //character.Node_ResumeInteraction(FullBodyBipedEffector.RightHand));
            );
    }

    [Affordance]
    protected Node Coerce(SmartCharacterControl user)
    {
        return new Sequence(
            // user.Node_Require(StateName.RoleActor, StateName.HoldingWeapon, StateName.IsStanding,
            // ~StateName.IsIncapacitated),
            // this.Node_Require(StateName.RoleActor, StateName.IsStanding, ~StateName.IsIncapacitated),
            new DecoratorForceStatus(
                RunStatus.Success,
                new Sequence(
                    new LeafAssert(() => WaypointFront == null),
                    this.ST_DoApproach(user, 1.0f))),
            new DecoratorForceStatus(
                RunStatus.Success,
                new Sequence(
                    new LeafAssert(() => WaypointFront != null),
                    user.Node_GoTo(Val.V(() => WaypointFront.position)),
                    user.Node_OrientTowards(this.transform.position),
                    this.Node_OrientTowards(Val.V(() => user.transform.position)))),
            //this.ST_DoApproach(user, 1.0f),
            user.Node_Icon("exclamation"),
            new LeafWait(2000),
            user.Node_Icon(null));
    }

    [Affordance]
    protected Node GiveKey(SmartCharacterControl user)
    {
        return new Sequence(
            user.Node_Require(StateName.RoleActor, StateName.HasKeys, StateName.IsStanding,
                ~StateName.IsIncapacitated),
            this.Node_Require(StateName.RoleActor, ~StateName.HasKeys, StateName.IsStanding,
                ~StateName.IsIncapacitated),
            user.Node_Icon("key"),
            new LeafWait(2000),
            user.Node_Icon(null),
            this.Node_Icon("key"),
            new LeafWait(2000),
            this.Node_Icon(null),
            user.Node_Set(~StateName.HasKeys),
            this.Node_Set(StateName.HasKeys));
    }

    #region Helper Subtrees
    public Node ST_StandAtWaypoint(Transform waypoint)
    {
        return new Sequence(
            this.Node_GoTo(Val.V(() => waypoint.position)),
            new Race(
                new LeafWait(3000),
                new Sequence(
                    this.Node_Orient(Val.V(() => waypoint.rotation)),
                    this.Node_NudgeTo(Val.V(() => waypoint.position)))),
            new LeafInvoke(() => this.transform.rotation = waypoint.rotation));
    }

    private Node ST_DoApproach(SmartCharacterControl user, float distance)
    {
        return new Sequence(user.ST_StandAtWaypoint(this.WaypointFront));
    }


    /// <summary>
    /// Subtree for orientation/lookat towards the lookAndOrient position, and reaching with
    /// the right hand for the given interaction object.
    /// </summary>
    protected Node ST_OrientAndReach(Val<Vector3> lookAndOrient, InteractionObject interact)
    {
        return new Sequence(
            this.Node_StartInteraction(FullBodyBipedEffector.RightHand, interact),
            this.Node_WaitForTrigger(FullBodyBipedEffector.RightHand));
    }
    #endregion

    #region Behavior Nodes
    public Node Node_GoTo(Val<Vector3> position)
    {
        return this.behavior.Node_GoTo(position);
    }

    public Node Node_NudgeTo(Val<Vector3> position)
    {
        return this.behavior.Node_NudgeTo(position);
    }

    public Node Node_OrientTowards(Val<Vector3> position)
    {
        return this.behavior.Node_OrientTowards(position);
    }

    public Node Node_Orient(Val<Quaternion> direction)
    {
        return this.behavior.Node_Orient(direction);
    }

    public Node Node_HeadLook(Val<Vector3> position)
    {
        return this.behavior.Node_HeadLook(position);
    }

    public Node Node_HeadLookStop()
    {
        return this.behavior.Node_HeadLookStop();
    }

    public Node Node_PlayHandGesture(Val<string> name, Val<long> miliseconds)
    {
        return this.behavior.ST_PlayHandGesture(name, miliseconds);
    }

    public Node Node_StartInteraction(
        Val<FullBodyBipedEffector> effector,
        Val<InteractionObject> obj)
    {
        return this.behavior.Node_StartInteraction(effector, obj);
    }

    public Node Node_ResumeInteraction(
        Val<FullBodyBipedEffector> effector)
    {
        return this.behavior.Node_ResumeInteraction(effector);
    }

    public Node Node_StopInteraction(
        Val<FullBodyBipedEffector> effector)
    {
        return this.behavior.Node_StopInteraction(effector);
    }

    public Node Node_WaitForTrigger(
        Val<FullBodyBipedEffector> effector)
    {
        return this.behavior.Node_WaitForTrigger(effector);
    }

    public Node Node_WaitForFinish(
        Val<FullBodyBipedEffector> effector)
    {
        return this.behavior.Node_WaitForFinish(effector);
    }

    public Node Node_GoToUpToRadius(Val<Vector3> targ, Val<float> dist)
    {
        return this.behavior.Node_GoToUpToRadius(targ, dist);
    }

    #endregion
}
