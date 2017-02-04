using System.Collections.Generic;
using TreeSharpPlus; //in case you want to create nodes here and THEN queue them up in behaviorplanner
using UnityEngine;

namespace POP {

public class Argue : Action {
	
	//EDIT FOR AS MANY CHARACTERS AS NEEDED
	POP.SmartCharacter char1;
	POP.SmartCharacter char2;
	
	public override void Execute ()
	{
		GameObject chargo1 = GameObject.Find(char1.name);
		GameObject chargo2 = GameObject.Find(char2.name);
		Debug.Log (char1.name + " is arguing with " + char2.name);
		BehaviorPlanner.orientTowardsEachOther(chargo1, chargo2);
		BehaviorPlanner.handGesture(chargo1, "Pointing", 1000L);
		BehaviorPlanner.handGesture(chargo2, "Cry", 1000L);
		BehaviorPlanner.handGesture(chargo2, "Pointing", 1000L);
		BehaviorPlanner.handGesture(chargo1, "BeingCocky", 1000L);
	}
	
	public override List<Condition> Preconditions
	{
		get 
		{ 
			//preconditions of the action here. If there is more than 1 char, make sure they're not the same:
			if(char1 == char2) return null;
			preconditions = new List<Condition>();
			preconditions.Add(new Condition(0, "InScene", false));
			preconditions.Add(new Condition(1, "InScene", false));
			return preconditions;
		}
	}
	
	public override List<Condition> Effects
	{
		get 
		{ 
			//the effects of the action here
			effects = new List<Condition>();
			effects.Add(new Condition(0, "InScene", true));
			effects.Add(new Condition(1, "InScene", true));
			effects.Add(new Condition(0, "Alive", true));
			effects.Add(new Condition(1, "Alive", true));
			/*effects.Add(new Condition(0, "ConflictResolved", false));
			effects.Add(new Condition(1, "ConflictResolved", false));*/

			//debug initialization--Cain and Abel should perform the PartWays action. Argue->PartWays->END
			effects.Add(new Condition(0, "HatesOther", true));
			effects.Add(new Condition(1, "HatesOther", true));

			//debug initialization--Cain should commit suicide after Abel estranges him. Argue->Abel estranges Cain->Cain commits suicide->END
			/*
			if(char1.name.Equals("Cain")){
				effects.Add(new Condition(0, "HatesOther", false));
				effects.Add(new Condition(1, "HatesOther", true));
			}else{
				effects.Add(new Condition(1, "HatesOther", false));
				effects.Add(new Condition(0, "HatesOther", true));
			}
			*/
			
			return effects;
		}
	}
	
	public override StateUtils.Role[] Roles
	{
		get { return new StateUtils.Role[] {StateUtils.Role.Actor, StateUtils.Role.Actor};}//EDIT FOR AS MANY CHARACTERS (Actors) AS NEEDED
	}
	
	public override SmartObject[] ArgumentList
	{			
		get 
		{
			return new SmartObject[] {char1, char2};//EDIT FOR AS MANY CHARACTERS (Actors) AS NEEDED
		}
		set {
			char1 = (POP.SmartCharacter)value [0];//EDIT FOR AS MANY CHARACTERS (Actors) AS NEEDED
			char2 = (POP.SmartCharacter)value [1];
		}
	}
}
}