using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace POP
{
	/// <summary>
	/// All actions need to inherit this class.
	/// It provides access to logic concerning the pre- and postconditions of an action.
	/// </summary>
	public class Action : DefaultAction {		

		protected SmartObject[] argumentList;
		protected List<Condition> preconditions;
		protected List<Condition> effects;

		public Action() {}

		public Action (GlobalState state)
		{
			this.state = new GlobalState(state);
		}

		public Action(ulong[] state)
		{
			this.state = new GlobalState (state);
		}

		public virtual SmartObject[] ArgumentList
		{
			get
			{
				return argumentList;
			}

			set
			{
				argumentList = value;
			}
		}

		public virtual StateUtils.Role[] Roles {
			get {
				return new StateUtils.Role[0];
			}
		}

		/// <summary>
		/// Call this function to execute the correct action
		/// </summary>
		public virtual void Execute() { }

		/// <summary>
		/// returns the number of roles needed to execute the corresponding task
		/// </summary>
		public int NrOfNeededRoles()
		{
			return (Roles).Count ();
		}

		/// <summary>
		/// Checks whether the preconditions of the corresponding task are satisfied by
		/// taking as argument the current global state
		/// 
		/// <param name="globalState">current state in the planner</param>
		/// </summary>
		public bool CheckPreconditions (params ulong[] globalState) 
		{
			bool isSatisfied = true;
			if (Preconditions == null)
				return false;
			foreach (Condition entry in Preconditions)
			{
				isSatisfied = isSatisfied && StateUtils.CheckState (globalState[entry.index], entry.name, entry.isTrue);
			}
			return isSatisfied;
		}

		/// <summary>
		/// Checks whether the preconditions of the corresponding task are satisfied by
		/// taking as argument the current global state
		/// 
		/// <param name="globalState">current state in the planner</param>
		/// </summary>
		public bool CheckEffects (params ulong[] globalState) 
		{
			bool isSatisfied = true;
			if (Preconditions == null)
				return false;
			foreach (Condition entry in Effects)
				isSatisfied = isSatisfied && StateUtils.CheckState (globalState[entry.index], entry.name, entry.isTrue);
			return isSatisfied;
		}

		/// <summary>
		/// Checks whether this effect is also contained by this action.
		/// The effect needs to use the global index for its condition to be able to make a comparison.
		/// 
		/// <param name="effect">The effect we are checking whether it is contained in this affordance.</param>
		/// <param name="manager">The state space manager needed to compute the global indices.</param>
		/// </summary>
		public bool ContainsGlobalEffect(Condition effect, BearSpaceManager manager)
		{
			if (Effects == null)
				return false;
			int[] relevantStateIndices = manager.extractRelevantStateIndices(this);
			foreach (Condition currentEffect in Effects)
			{
				Condition tempEffect = new Condition(relevantStateIndices[currentEffect.index], currentEffect.name, currentEffect.isTrue);
				if (tempEffect.Equals(effect))
					return true;
			}
			return false;
		}

		/// <summary>
		/// Returns the preconditions of this affordance and converts the local indices of those preconditions into global indices.
		/// 
		/// <param name="manager">The state space manager needed to compute the global indices.</param>
		/// </summary>
		public List<Condition> GetGlobalPreconditions(BearSpaceManager manager)
		{
			List<Condition> result = new List<Condition> ();
			int[] relevantStateIndices = manager.extractRelevantStateIndices(this);
			if (Preconditions == null)
				return result;
			
			foreach (Condition currentPrecondition in Preconditions)
			{
				Condition tempPrecondition = new Condition(relevantStateIndices[currentPrecondition.index], currentPrecondition.name, currentPrecondition.isTrue);
				result.Add(tempPrecondition);
			}
			return result;
		}

		/// <summary>
		/// Returns the effects of this affordance and converts the local indices of those effects into global indices.
		/// 
		/// <param name="manager">The state space manager needed to compute the global indices.</param>
		/// </summary>
		public List<Condition> GetGlobalEffects(BearSpaceManager manager)
		{
			List<Condition> result = new List<Condition> ();
			int[] relevantStateIndices = manager.extractRelevantStateIndices(this);
			if (Effects == null)
				return result;

			foreach (Condition currentEffect in Effects)
			{
				Condition tempEffect = new Condition(relevantStateIndices[currentEffect.index], currentEffect.name, currentEffect.isTrue);
				result.Add(tempEffect);
			}
			return result;
		}



		/// <summary>
		/// Uses the current global state taken as an argument to compute the new state
		/// using the preconditions of the corresponding task
		/// 
		/// <param name="globalState">current state in the planner</param>
		/// </summary>
		public ulong[] GetPreconditionStates (params ulong[] globalState) 
		{
			if (Preconditions == null)
				return globalState;
			foreach (Condition entry in Preconditions)
				globalState[entry.index] = StateUtils.SetState (globalState[entry.index], entry.name, entry.isTrue);

			return globalState;
		}


		/// <summary>
		/// Uses the current global state taken as an argument to compute the new state
		/// using the effects of the corresponding task
		/// 
		/// <param name="globalState">current state in the planner</param>
		/// </summary>
		public ulong[] GetResultingStates (params ulong[] globalState) 
		{
			if (Effects == null)
				return globalState;

			foreach (Condition entry in Effects)
				globalState[entry.index] = StateUtils.SetState (globalState[entry.index], entry.name, entry.isTrue);

			return globalState;
		}

		public virtual List<Condition> Preconditions
		{
			get 
			{ 
				return null;
			}
		}

		
		public virtual List<Condition> Effects
		{
			get 
			{ 
				effects = new List<Condition>();
				return effects;
			}
		}
	}
}