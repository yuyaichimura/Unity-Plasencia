using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace POP
{
	class ActionPlanner
	{
		BestFirstSearchPlanner planner;
		List<PlanningDomainBase> domains;
		public Stack<DefaultAction> plan;
		public int[] relevantIndices;

		public static ActionPlanner InstantiatePlanner ()
		{
			ActionPlanner planner = new ActionPlanner ();
			return planner;
		}

		
		private ActionPlanner() 
		{
			planner 	= new BestFirstSearchPlanner ();
			domains 	= new List<PlanningDomainBase>();
			plan 		= new Stack<DefaultAction> ();
		}

		public void Reset ()
		{
			domains.Clear ();
			plan.Clear ();
			planner = new BestFirstSearchPlanner ();
			relevantIndices = null;
		}

		/// <summary>
		/// Starts the corresponding planner. By default this will be the partial-order planner.
		/// <param name="manager">The state space manager used to retrieve the affordances</param>
		/// <param name="arc">The story arc we are planning for, needed to retrict the number of smart objects considered during the planning.</param>
		/// <param name="start">The start affordance.</param>
		/// <param name="end">The end affordance. Can be null, if the partial-order planner is used.</param>
		/// <param name="preconditions">The list of open preconditions that need to be fulfilled. Only needed by the partial-order planner.</param>
		/// <param name="time">Defines the maximum time that the planner will be searching for a plan before stopping.</param>
		/// <param name="involveUser">Defines wether affordances on the user should be considered during planning.</param>
		/// </summary>
		public bool StartPlanner(BearSpaceManager manager, Action start, Action end, List<Condition> preconditions, float time = 10f, bool involveUser = true) 
		{
			if (start == null)
			{
				UnityEngine.Debug.LogError ("The start action can't be null!");
				return false;
			}

			if (end == null)
			{
				UnityEngine.Debug.LogError ("The end action can't be null!");
				return false;
			}
			if (start == end)
			{
				UnityEngine.Debug.LogError ("start equals end!!!");
				return false;
			}
			bool found = false;

			PartialOrderDomain domain = new PartialOrderDomain (manager, start, end);
			domains.Add (domain);
			
			foreach (Condition precondition in preconditions)
			{
				((GlobalState) start.state).openPreconditions.Add(new Tuple<Condition, Action>(precondition, end));
			}
			((GlobalState)start.state).actions.Add (start);
			((GlobalState)start.state).actions.Add (end);
			((GlobalState)start.state).temporalOrdering.Add (new Tuple<Action, Action> (start, end));
			
			planner.init (ref domains, 30000000);
			
			found = planner.computePlan (ref start.state, ref end.state, ref plan, time);
			plan.Clear();
			
			plan.Push (end);
			if (domain.currentPlan != null) {
				if (start != null)
					domain.currentPlan.Remove (start);
				if (end != null)
					domain.currentPlan.Remove (end);
				for (int j = domain.currentPlan.Count - 1; j >= 0; j--) {
					plan.Push (domain.currentPlan [j]);
				}
			}
			plan.Push (start);
			return found;
		}
	}
}