using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace POP
{
	/// <summary>
	/// GlobalState used by the partial-order planner.
	/// </summary>
	public class GlobalState : DefaultState 
	{

		public List<Tuple<Condition, Action>> openPreconditions;
		public List<Tuple<Action,Condition, Action>> causalLinks;
		public List<Tuple<Action, Action>> temporalOrdering;
		public List<Action> actions;
		
		public GlobalState(ulong[] gS) {
			this.globalState = gS;
			causalLinks = new List<Tuple<Action, Condition, Action>> ();
			openPreconditions = new List<Tuple<Condition, Action>> ();
			temporalOrdering = new List<Tuple<Action, Action>> ();
			actions = new List<Action> ();
		}
		
		public GlobalState(GlobalState gS) {
			this.globalState = new ulong[gS.globalState.Length];
			Array.Copy (gS.globalState, this.globalState, gS.globalState.LongLength);
		
			this.openPreconditions = new List<Tuple<Condition, Action>> (gS.openPreconditions);
			this.causalLinks = new List<Tuple<Action, Condition, Action>> (gS.causalLinks);
			this.temporalOrdering = new List<Tuple<Action, Action>> (gS.temporalOrdering);
			this.actions = new List<Action> (gS.actions);
		}
		
		public Tuple<Condition, Action> SelectOpenPrecondition ()
		{
			if (openPreconditions.Count > 0)
				return openPreconditions [0];
			return null;
		}
	}
}

