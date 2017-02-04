using System.Collections.Generic;
using TreeSharpPlus; //in case you want to create nodes here and THEN queue them up in behaviorplanner
using UnityEngine;

namespace POP {

public class Kill : Action {
	
	//EDIT FOR AS MANY CHARACTERS AS NEEDED
	POP.SmartCharacter char1;
	POP.SmartCharacter char2;
	
	public override void Execute ()
	{
		GameObject chargo1 = GameObject.Find(char1.name);
		Debug.Log (char1.name + " kills " + char2.name + ".");
		//BehaviorPlanner.[whatever method you need that queues the corresponding node for this action]();
	}
	
	public override List<Condition> Preconditions
	{
		get 
		{ 
			//preconditions of the action here. If there is more than 1 char, make sure they're not the same:
			
			if(char1 == char2) return null;
			preconditions = new List<Condition>();
			preconditions.Add(new Condition(0, "InScene", true));
			preconditions.Add(new Condition(1, "InScene", true));
			preconditions.Add(new Condition(0, "Alive", true));
			preconditions.Add(new Condition(1, "Alive", true));

			preconditions.Add(new Condition(0, "Murderous", true));
			preconditions.Add(new Condition(0, "Estranged", true));
			return preconditions;
		}
	}
	
	public override List<Condition> Effects
	{
		get 
		{ 
			//the effects of the action here
			effects = new List<Condition>();
			effects.Add(new Condition(1, "Alive", false));
			effects.Add(new Condition(0, "ConflictResolved", true));
			effects.Add(new Condition(1, "ConflictResolved", true));
			return effects;
		}
	}
	
	public override StateUtils.Role[] Roles
	{
		get { return new StateUtils.Role[] {StateUtils.Role.Actor,StateUtils.Role.Actor};}//EDIT FOR AS MANY CHARACTERS (Actors) AS NEEDED
	}
	
	public override SmartObject[] ArgumentList
	{			
		get 
		{
			return new SmartObject[] {char1, char2};//EDIT FOR AS MANY CHARACTERS (Actors) AS NEEDED
		}
		set {
			char1 = (POP.SmartCharacter)value [0];
			char2 = (POP.SmartCharacter)value [1];//EDIT FOR AS MANY CHARACTERS (Actors) AS NEEDED
		}
	}
}
}