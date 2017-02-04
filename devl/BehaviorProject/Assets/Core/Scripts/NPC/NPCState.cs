using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)] 
public class Condition : System.Attribute, IComparable {

	// TODO - document and fix this s^%$ code

    internal List<Condition> gNestedConditions = new List<Condition>();
	public static string MATCH_IDENTIFIER = "Match";
    public static string SELF_IDENTIFIER = "Self";
    internal bool gMandatory = true; // every condition is mandatory unless otherwise specified
	internal bool gMatching = false;
	internal Modifier gTempTraitModifier = null;
    internal bool gOnEntity;
    internal object gTargetValue;
    internal MethodInfo Property { get; set; }
    public Type Type { get; set; }
    internal NPCState Owner { get; set; }
    internal bool gInverted = false;

    public bool Inverted {
        get {
            return this.gInverted;
        }
        set {
            this.gInverted = value;
        }
    }

    public List<Condition> NestedConditions {
        get {
            return this.gNestedConditions;
        }
    }

    public bool Mandatory {
        get {
            return this.gMandatory;
        }
        set {
            this.gMandatory = value;
        }
    }

	public Modifier TraitModifier {
		get {
			return this.gTempTraitModifier;
		}
		set {
			this.gTempTraitModifier = value;
		}
	}

	public bool Matching {
		get {
			return this.gMatching;
		}
        set {
            this.gMatching = value;
        }
	}

	public string Name { get; set; }
    
	public object Value(params object[] pParams) {
        if (gTargetValue != null) 
			return gTargetValue; // pre-defined propoerty with no entity attached to invoke a function on.
		else {
			if (Trait != null && Target != float.NaN ) {
				pParams = new object[3];
				pParams[0] = Trait;
				pParams[1] = Target;
				pParams[2] = TraitModifier;
			}
			return  this.Property.Invoke (this.Owner, pParams);
		}
    }

    public bool IsTargeted {
        get {
            return gOnEntity;
        }
        set {
            this.gOnEntity = value;
        }
    }

	public object TargetValue {  
		get {
			return this.gTargetValue;
		}
		set {
			this.gTargetValue = value;
		}
	}

	[System.ComponentModel.DefaultValue("")]
	public string Trait { get; set; }

	[System.ComponentModel.DefaultValue(float.NaN)]
	public float Target { get; set; }

    public Condition() {
        // shell for XML initializer
    }

	/// <summary>
	/// Condition on self as true
	/// </summary>
	/// <param name="pName">P name.</param>
	/// <param name="pType">P type.</param>
	public Condition(string pName, Type pType) {
		this.gMatching = pName.Contains(Condition.MATCH_IDENTIFIER);
        this.Name = pName;
        this.Type = pType;
    }

	/// <summary>
	/// Condition on self trait against a float value
	/// </summary>
	/// <param name="pName">P name.</param>
	/// <param name="pType">P type.</param>
	/// <param name="pTrait">P trait.</param>
	/// <param name="pTarget">P target.</param>
	public Condition(string pName, Type pType, string pTrait, float pTarget) : this (pName,pType) {
		this.Trait = pTrait;
		this.Target = pTarget;
		this.gTargetValue = true;
	}

	/// <summary>
	/// Condition on target NPC's trait against a float value
	/// </summary>
	/// <param name="pName">P name.</param>
	/// <param name="pType">P type.</param>
	/// <param name="pTrait">P trait.</param>
	/// <param name="pTarget">P target.</param>
	/// <param name="pNPC">P NP.</param>
	public Condition(string pName, Type pType, string pTrait, float pTarget, bool pOnEntity) : this (pName,pType,pTrait,pTarget) {
		this.gOnEntity = pOnEntity;
		this.gTargetValue = true;
	}

    /// <summary>
    /// Conditions on self
    /// </summary>
    /// <param name="pName"></param>
    /// <param name="pType"></param>
    /// <param name="pValue"></param>
    public Condition(string pName, Type pType, object pValue) : this(pName,pType) {
        this.gTargetValue = pValue;
    }

    /// <summary>
    /// Conditions on target
    /// </summary>
    /// <param name="pName"></param>
    /// <param name="pType"></param>
    /// <param name="pValue"></param>
    /// <param name="pTarget"></param>
    public Condition(string pName, Type pType, object pValue, bool pOnTarget)
        : this(pName, pType, pValue) {
            this.gOnEntity = pOnTarget;
    }

    public int CompareTo(object o) {
        if (o != null && (o as Condition) != null) {
            Condition c = o as Condition;
            if (this.Name == c.Name && this.Type.GetType() == c.Type.GetType() || c.Matching) {
				Type t = this.Type;
                object thisVal = this.Value();
                if (c.Inverted) {
                    thisVal = !(bool)thisVal;
                }
                object value = c.Value();
				if(thisVal == null || value == null) return -1;
                bool match = (Convert.ChangeType(thisVal, this.Type)).Equals((Convert.ChangeType(value, this.Type)));
				TraitModifier = null; // clean up. - TODO - do the same thing with the other properties
                return match ? 0 : -1; // this will need to be expanded to allow -1 or 1 float values... not right now though.
            }
        }
        return -1;
    }
}

public class Trait : System.Attribute {
	public string Name { get; set; }
    internal MethodInfo Property { get; set; }
    internal NPCEntity Owner { get; set; }
	internal bool gPersistent;
    public Double Value() {
        return (Double) this.Property.Invoke(this.Owner, null);
    }
    public Trait(string pName) {
        this.Name = pName;
		this.gPersistent = true;
    }

	public Trait(string pName, bool pPersistent) : this(pName) {
		this.gPersistent = pPersistent;
	}
}

public class NPCState : NPCController {

    #region Members
    private bool gInEvent = false;
    private NPCController gController;
    #endregion

    void Awake() {
		this.Friendliness = UnityEngine.Random.Range (0f, 2f);
        this.gTraits = new Dictionary<string, Trait>();
        gController = GetComponent<NPCController>();
        gConditions = new Dictionary<string, Condition>();
        InitializeTraits();
        InitializeConditions();
        gController.Conditions = gConditions;
    }

    #region Traits

    [System.ComponentModel.DefaultValue(0)]
    [Trait("Friendliness")]
    public Double Friendliness { get; set; }

    [System.ComponentModel.DefaultValue(0)]
    [Trait("Hostility")]    
    public Double Hostility { get; set; }

    [System.ComponentModel.DefaultValue(0)]
    [Trait("Tiredness")]
    public Double Tiredness { get; set; }

    [System.ComponentModel.DefaultValue(0)]
    [Trait("Happiness")]
    public Double Happiness { get; set; }

	[System.ComponentModel.DefaultValue(0)]
	[Trait("Courage")]
	public Double Courage { get; set; }

	[System.ComponentModel.DefaultValue(0)]
	[Trait("Rudeness")]
	public Double Rudeness { get; set; }

	[System.ComponentModel.DefaultValue(0)]
	[Trait("Attractiveness")]
	public Double Attractiveness { get; set; }

	[System.ComponentModel.DefaultValue(0)]
	[Trait("SenseOfHumor")]
	public Double SenseOfHumor { get; set; }

	[System.ComponentModel.DefaultValue(0)]
	[Trait("Funny")]
	public Double Funny { get; set; }

	[System.ComponentModel.DefaultValue(0)]
	[Trait("Rude")]
	public Double Rude { get; set; }

	[System.ComponentModel.DefaultValue(0)]
	[Trait("Faith")]
	public Double Faith { get; set; }

    #endregion

    #region Conditions

    [Condition("Self", typeof(NPCEntity))]
    public NPCController Source {
        get {
			return this.gController;
        }
    }

	[Condition("Match", typeof(bool))]
	public bool MatchTrait(string pTrait, float pTarg, Modifier modifier) {
		Trait t = gTraits [pTrait];
		if(modifier != null) {
			Debug.Log ("I have modifier: "+modifier.Name+" with value: "+modifier.Value+" of type: "+modifier.ModifierType);
			switch(modifier.ModifierType) {
			case ModifierType.INCREMENTOR: 
			case ModifierType.DECREMENTOR:
				// return (modifier == null ? t.Value () : t.Value () + modifier.Value)  > pTarg;
				return ((t.Value () + modifier.Value)  > pTarg);
			case ModifierType.NEUTRAL:
				return (modifier.Value  > pTarg); // set the entities value to the modifier's neutralized one.
			}
		}
		return t.Value() > pTarg;
	}

    [Condition("Target", typeof(NPCController))] // this should be NPCEntity but the stupid casting is not working right now.
    public NPCEntity Target {  
		get {
			return gController.InteractionTarget == null ? null : gController.InteractionTarget.GetComponent<NPCEntity>();
		}
	}

    [Condition("CurrentAction", typeof(NPCAction))]
    public NPCAction CurrentAction { get; set; }

    [Condition("InEvent", typeof(bool))]
    public bool InEvent {
        get {
            return this.gController.BehaviorStatus == BehaviorStatus.InEvent; // working ok
        }
        set {
            this.gInEvent = value;
        }
    }

    [Condition("TestCondition", typeof(bool))]
    public bool TestCondition {
        get {
            return true;
        }
    }

    [Condition("IsForeground", typeof(bool))]
    public bool IsForeground {
        get {
            return this.gController.Selected;
        }
    }

	[Condition("ContextName", typeof(string))]
	public string ContextName {
		get {
			return this.gController.CurrentContext.Name;
		}
	}

	[Condition("TestNPC", typeof(bool))]
	public bool TestNPC {
		get {
			return this.gController.TEST_NPC;
		}
	}

    [Condition("Religion", typeof(string))]
    public string Religion {
        get {
            return gController.Religion;
        }
    }

    #endregion Conditions

    #region Utilities

    /// <summary>
    /// Register all the Conditions properties as delegated methods from a Condition instance.
    /// </summary>
    private void InitializeConditions() {
        foreach (PropertyInfo pi in this.GetType().GetProperties()) {
            object[] attribs = pi.GetCustomAttributes(true);
            if (attribs.Length > 0) {
                Condition c = attribs[0] as Condition;
                if (c != null) {
                    if (!gConditions.ContainsKey(c.Name)) {
                        c.Property = pi.GetGetMethod(true);
                        c.Owner = this;
                        gConditions.Add(c.Name, c);
                    }
                }
            }
        }
		foreach (MethodInfo mi in this.GetType().GetMethods()) {
			object[] attribs = mi.GetCustomAttributes(true);
			if (attribs.Length > 0) {
				Condition c = attribs[0] as Condition;
				if (c != null) {
					if (!gConditions.ContainsKey(c.Name)) {
						c.Property = mi;
						c.Owner = this;
						gConditions.Add(c.Name, c);
					}
				}
			}
		}
    }

    /// <summary>
    /// Register character's traits
    /// </summary>
    private void InitializeTraits() {
        foreach (PropertyInfo pi in this.GetType().GetProperties()) {
            object[] attribs = pi.GetCustomAttributes(true);
            if (attribs.Length > 0) {
                Trait t = null;
                foreach (object o in attribs)
                    if(o as Trait != null) t = o as Trait;
                if (t != null) {
                    if (!gTraits.ContainsKey(t.Name)) {
                        t.Owner = this;
                        t.Property = pi.GetGetMethod(true);
                        float val = UnityEngine.Random.Range(-1f,1f);
						pi.SetValue( this, Convert.ChangeType(val, pi.PropertyType), null);
                        gTraits.Add(t.Name, t);
                    }
                }
            }
        }
    }
    #endregion
}   
