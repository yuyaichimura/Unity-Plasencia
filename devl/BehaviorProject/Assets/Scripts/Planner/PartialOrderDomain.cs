using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace POP
{
	/// <summary>
	/// The domain for computing a plan under the usage of an partial-order planner.
	/// It provides all the needed functionalities to generate the transitions and checking for the goal state.
	/// </summary>
	class PartialOrderDomain : PlanningDomainBase
	{
		static List<POP.Action> transitionsList;
		BearSpaceManager manager;
		public List<DefaultAction> currentPlan;
		Action start;
		Action end;

		public PartialOrderDomain(BearSpaceManager manager, Action start, Action end)
		{
			this.manager = manager;
			transitionsList = manager.transitionList;
			this.start = start;
			this.end = end;
		}
		
		public override float evaluateDomain (ref DefaultState state)
		{	
			return 1.0f;
		}
		
		public override bool equals (DefaultState s1, DefaultState s2, bool isStart)
		{
			bool result = true;
			for(int i = 0; i < s1.globalState.Length; i++) {
				result = result && ((s1.globalState[i] ^ s2.globalState[i]) == 0x0UL);
			}
			return result;
		}
		
		/// <summary>
		/// Computes the heuristic function (often denoted as f) that estimates the "goodness" of a state towards the goal.
		/// The "goodness" is based on the number of open preconditions we still have to satisfy.
		/// </summary>
		/// 
		/// <param name="currentState">The current state of the planner.</param>
		/// <param name="idealGoalState">The ideal goal state. Not really needed for the partial-order planner</param>
		/// <param name="currentg">The current g</param>
		public override float estimateTotalCost(ref DefaultState currentState, ref DefaultState idealGoalState, float currentg )
		{ 
			return currentg +  ((GlobalState) currentState).openPreconditions.Count  + ((GlobalState) currentState).actions.Count;
		}
		
		/// <summary>
		/// Decides whether the current state is a goal state by checking the consistency of the temporal ordering
		/// and the number of openpreconditions.
		/// If the number is 0 and the ordering is consistent, we got a goal state.
		/// </summary>
		/// 
		/// <param name="state">The current state of the planner.</param>
		/// <param name="idealGoalState">The ideal goal state. Not really needed for the partial-order planner</param>
		public override bool isAGoalState(ref DefaultState state, ref DefaultState idealGoalState)
		{
			if (((GlobalState)state).openPreconditions.Count == 0)
				if (IsTemporalOrderingConsistent ((GlobalState)state))
						return true;

			return false;
		}
		
		/// <summary>
		/// Takes the current state and generates all possible transitions from this point on.
		/// The partial-order planner picks an arbitrary open preconditions and checks all actions to find some that can satisfy it.
		/// There are two steps involved:
		/// Step Addition: search the whole domain space for an appropriate action.
		/// Simple Establishment: Search the current plan whether an action can satisfy more preconditions.
		/// </summary>
		/// 
		/// <param name="currentState">The current state of the planner.</param>
		/// <param name="previousState">The previous state of the planner. Not really needed for the partial-order planner</param>
		/// <param name="idealGoalState">The ideal goal state. Not really needed for the partial-order planner</param>
		/// <param name="transitions">List of possible transitions</param>
		public override void generateTransitions(ref DefaultState currentState, ref DefaultState previousState, ref DefaultState idealGoalState, ref List<DefaultAction> transitions)
		{
			Tuple<Condition, Action> precondition = ((GlobalState) currentState).SelectOpenPrecondition();
			if (precondition == null)
				return;
			// Simple Establishment: search in the current plan, if we can solve the precondition
			for (int i = 0; i < ((GlobalState) currentState).actions.Count; i++)
			{	
				Action a = ((GlobalState) currentState).actions[i];
				if (end == a)
					continue;

				// create new instance of this action to be used in the transition list
				Action affordance = (POP.Action)Activator.CreateInstance(a.GetType());


				if (start == a)
				{
					affordance.state = new GlobalState((GlobalState) currentState);
					GlobalState state = (GlobalState) affordance.state;

					// we got the start node: simply check the global state
					if (StateUtils.CheckState(state.globalState[precondition.Element1.index], precondition.Element1.name, precondition.Element1.isTrue))
					{
						state.causalLinks.Add(new Tuple<Action, Condition, Action>(start, precondition.Element1, precondition.Element2));
						state.temporalOrdering.Add(new Tuple<Action, Action>(start, precondition.Element2));
						if (!ContainsThreats(state) && IsTemporalOrderingConsistent(state))
						{
							state.openPreconditions.Remove(precondition);
							affordance.cost =  0;
							transitions.Add(affordance);
						}
					}
				}
				else
				{
					affordance.state = new GlobalState((GlobalState) currentState);
					GlobalState state = (GlobalState) affordance.state;
					affordance.ArgumentList = a.ArgumentList;
					// we are examining an action added to satisfy a precondition. Need to check the list of effects
					if (affordance.ContainsGlobalEffect(precondition.Element1, manager))
					{
						state.causalLinks.Add(new Tuple<Action, Condition, Action>(a, precondition.Element1, precondition.Element2));
						state.temporalOrdering.Add(new Tuple<Action, Action>(a, precondition.Element2));
						if (!ContainsThreats(state) && IsTemporalOrderingConsistent(state))
					    {
							state.openPreconditions.Remove(precondition);
							affordance.cost = affordance.GetGlobalPreconditions(manager).Count;
							transitions.Add(affordance);
						}
					}
				}
			}

			// Step Addition: haven't found appropriate action yet. Search in all possible actions
			for(int i = 0; i < transitionsList.Count; i++)
			{
				if (transitionsList[i].ContainsGlobalEffect(precondition.Element1, manager))
				{
					Action affordance = (POP.Action)Activator.CreateInstance(transitionsList[i].GetType());
					affordance.ArgumentList = transitionsList[i].ArgumentList;
					affordance.state = new GlobalState((GlobalState) currentState);
					GlobalState state = (GlobalState) affordance.state;

					state.causalLinks.Add(new Tuple<Action, Condition, Action>(affordance, precondition.Element1, precondition.Element2));
					state.temporalOrdering.Add(new Tuple<Action, Action>(affordance, precondition.Element2));
					state.actions.Add(affordance);
					
					// action is not a part of the plan yet: need to order it between start and end
					state.temporalOrdering.Add(new Tuple<Action, Action>(start, affordance));
					state.temporalOrdering.Add(new Tuple<Action, Action>(affordance, end));
					if (IsTemporalOrderingConsistent(state) && !ContainsThreats(state))
					{
						state.openPreconditions.Remove (precondition);
						foreach (Condition entry in affordance.GetGlobalPreconditions(manager))
						{
							if (!state.openPreconditions.Contains(new Tuple<Condition, Action>(entry, affordance)))
								state.openPreconditions.Add (new Tuple<Condition, Action> (entry, affordance));
						}	
						affordance.cost = 0;
						transitions.Add(affordance);
					}
				}
				
			}	
		}
		
		
		/// <summary>
		/// Check the consistency of the temporal ordering.
		/// Search through the whole action space whether it is possible to construct a plan that satisfies
		/// all temporal ordering. One condition for this is that there are no cycles.
		/// </summary>
		/// 
		/// <param name="state">The current state of the planner.</param>
		private bool IsTemporalOrderingConsistent(GlobalState state)
		{

			List<DefaultAction> tempList = new List<DefaultAction> ();
			tempList.Add (start);

			return IsConsistent (state, tempList);
		}

		/// <summary>
		/// Check the consistency of the partial plan that it is currently examining.
		/// If we found a complete plan: return
		/// Otherwise, check whether the plan is consistent and continue adding affordances or continue with the next plan.
		/// </summary>
		/// 
		/// <param name="state">The current state of the planner.</param>
		/// <param name="plan">A consistent partial plan computed out of a subset of the affordances in state</param>
		private bool IsConsistent(GlobalState state, List<DefaultAction> plan)
		{
			// found a complete plan
			if (plan.Count == state.actions.Count)
			{
				currentPlan = new List<DefaultAction>(plan);
				return true;
			}

			for (int i = 0; i < state.actions.Count; i++)
			{
				Action action = state.actions[i];
				if (!plan.Contains(action) && !ViolatesOrdering(state, plan, action))
				{
					plan.Add(action);
					if (IsConsistent (state, plan))
						return true;
					plan.Remove(action);
				}
			}
			return false;
		}

		/// <summary>
		/// Checks whether action violates the consistency of possiblePlan.
		/// </summary>
		/// 
		/// <param name="state">The current state of the planner.</param>
		/// <param name="possiblePlan">A partial plan computed out of a subset of the affordances in state</param>
		/// <param name="action">The affordance that should be added to possiblePlan</param>
		private bool ViolatesOrdering (GlobalState state, List<DefaultAction> possiblePlan, Action action)
		{
			foreach (Tuple<Action, Action> entry in state.temporalOrdering)
				if (entry.Element1 != entry.Element2)
					if (possiblePlan.Contains(entry.Element2) && action == entry.Element1)
						return true;	
	
			return false;
		}
		
		/// <summary>
		/// Check the current plan for possible threats. If yes, try to resolve them.
		/// </summary>
		/// 
		/// <param name="state">The current state of the planner.</param>
		private bool ContainsThreats(GlobalState state)
		{
			for (int i = 0; i < state.causalLinks.Count; i++)
			{
				Tuple<Action, Condition, Action> causalLink = state.causalLinks[i];
				for(int j = 0; j < state.actions.Count; j++)
				{
					Action action = state.actions[j];
					if (action == end)
						continue;

					foreach(Condition effect in action.GetGlobalEffects(manager))
						if (effect.SameConditionAndObject(causalLink.Element2) && effect.isTrue != causalLink.Element2.isTrue)
							// need to resolve this threat
							if(!ResolveThreats(state, action, causalLink.Element1, causalLink.Element3))
								return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Try to resolve the threats. Check whether we can move the action before or after the causal link.
		/// </summary>
		/// 
		/// <param name="state">The current state of the planner.</param>
		/// <param name="affordance">The action that is a possible threat</param>
		/// <param name="a1">The first action in the causal link that is threatened.</param>
		/// <param name="a2">The second action in the causal link that is threatened.</param>
		private bool ResolveThreats(GlobalState state, Action affordance, Action a1, Action a2)
		{
			if (state.temporalOrdering.Contains (new Tuple<Action, Action> (affordance, a1)) || state.temporalOrdering.Contains (new Tuple<Action, Action> (a2, affordance)))
				return true;

			// Promotion: force threatening step to come after the causal link a1 -> a2
			state.temporalOrdering.Add(new Tuple<Action, Action>(affordance, a1));
			if (IsTemporalOrderingConsistent (state))
				return true;
			state.temporalOrdering.Remove (new Tuple<Action, Action> (affordance, a1));
			
			//Demotion: force threatening step to come before the causal link a1 -> a2
			state.temporalOrdering.Add(new Tuple<Action, Action>(a2, affordance));
			if (IsTemporalOrderingConsistent (state))
				return true;
			state.temporalOrdering.Remove (new Tuple<Action, Action> (a2, affordance));
			
			// couldn't reorder threatening step
			return false;
		}

	}
}




