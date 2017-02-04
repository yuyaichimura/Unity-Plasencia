using System;
using System.Collections.Generic;

using UnityEngine;
using UDebug = UnityEngine.Debug;

namespace POP
{
	/// <summary>
	/// The general class for all characters.
	/// </summary>
	public class SmartCharacter : SmartObject
	{		
		#if UNITY_EDITOR
		[StatesAttribute(typeof(SmartCharacter))]
		#endif
		public string[] startStates;

	    public override void Awake()
	    {
			Init ();
			state = 0;
			foreach (string entry in startStates) {	
				//state = StateUtils.SetState(state, entry, true); 
				if (String.IsNullOrEmpty(entry)) {
					state = StateUtils.SetState(state, StateUtils.GetCorrectStates(this)[0], true);
				} else {
					state = StateUtils.SetState(state, entry, true);
				}
			}
		}

		void Update() {	}

		public override void Init()
		{
			baseState = BaseState;
		}

		public override ulong BaseState {
			get 
			{
				baseState = 0; 
				return baseState;
			}
		}

		public override ulong StartState {
			get
			{
				ulong state = BaseState;
				foreach (string entry in startStates) {	
					//state = StateUtils.SetState(state, entry, true); 
					if (String.IsNullOrEmpty(entry)) {
						state = StateUtils.SetState(state, StateUtils.GetCorrectStates(this)[0], true);
					} else {
						state = StateUtils.SetState(state, entry, true);
					}
				}
				return state;
			}
		}
	}
}