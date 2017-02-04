#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using UnityEngine;

namespace POP
{
#if UNITY_EDITOR
	/// <summary>
	/// Saves the needed parameters to correctly show the attribute in the inspector.
	/// </summary>
	public class StatesAttribute : PropertyAttribute {
		public readonly Type obj;
		public string[] states;
		public List<int> index;
		public StatesAttribute (Type obj) {
			this.obj = obj;
			this.states = StateUtils.GetCorrectStates (obj);
			this.index = new List<int> ();
		}
	}	
#endif

	/// <summary>
	/// Inherit from this class to create a new type of smart objects.
	/// </summary>
	public abstract class SmartObject : MonoBehaviour 
	{
		public ulong state;
		protected ulong baseState;

		public virtual ulong BaseState {
			get {return baseState;}
		}

		public virtual ulong StartState {
			get { return BaseState;}
		}

		public virtual void Init() {}
		
		public virtual void Awake()	{}
		
		public virtual void Reset()	{}
	}
}

