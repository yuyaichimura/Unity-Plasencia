using System;
using System.Collections.Generic;

using UnityEngine;

namespace POP {
/// <summary>
/// Contains all the states used by the planner and offers various methods to operate on them
/// </summary>
public static class StateUtils
{
	// defines all possible attributes and their mask to set the state accordingly.
	public static Dictionary<string,ulong> StateMasks = new Dictionary<string, ulong>()
	{
		//ORIGINAL
		/*{"InScene", 0x100UL},
		{"IsHappy", 0x200UL},
		{"KnowsBear1", 0x400UL},
		{"KnowsBear2", 0x800UL},
		{"FeelsBad", 0x1000UL},
		{"IsAngryAtBear1", 0x2000UL},
		{"IsAngryAtBear2", 0x4000UL}*/
		
		//DEMO WITH CHRISTIAN AND MUSLIM
		/*{"enemyIs0", 0x100UL},
		{"enemyIs1", 0x200UL},
		{"friendsWith0", 0x400UL},
		{"friendsWith1", 0x800UL},
		{"HasGift", 0x1000UL},
		{"InScene", 0x2000UL},
		{"hatesTheOther", 0x8000UL},
		{"murdered", 0x300UL}*/

		//GENERIC--WIP
		/*
		{"InScene", 0x10UL},
		{"enemyIs0", 0x100UL},
		{"enemyIs1", 0x200UL},
		{"friendsWith0", 0x300UL},
		{"friendsWith1", 0x400UL},
		{"loves0", 0x500UL},
		{"loves1", 0x600UL},
		{"hates0", 0x1},
		{"hates1", 0x2},
		{"respects0", 0x700UL},
		{"respects1", 0x800UL},
		{"disrespects0", 0x900UL},
		{"disrespects1", 0x1000UL},
		{"pleasedWith0", 0x2000UL},
		{"pleasedWith1", 0x3000UL},
		{"displeasedWith0", 0x4000UL},
		{"displeasedWith1", 0x5000UL},
		{"sorryFor0", 0x6000UL},
		{"sorryFor1", 0x7000UL},
		{"envies0", 0x8000UL},
		{"envies1", 0x9000UL},
		{"attractedTo0", 0x10000UL},
		{"attractedTo1", 0x20000UL},
		{"disgustedWith0", 0x30000UL},
		{"disgustedWith1", 0x40000UL}*/

		//SEDUCTION DEMO ATTEMPT
		/*{"InScene", 0x10UL},
		{"alive", 0x20UL},
		{"envies1", 0x30UL},
		{"attractedTo2", 0x20000UL},
		{"marriedTo2", 0x40UL},
		{"marriedTo1", 0x50UL},
		{"seduced2", 0x60UL},
		{"HasGift", 0x70UL}*/

		//DEMO_DAY1
		/*{"InScene", 0x100UL},
		{"HasItem", 0x200UL},
		{"KnowsOfItem", 0x300UL},
		{"Distracted", 0x400UL},
		{"MurderousToMark", 0x500UL},
		{"Alive", 0x600UL},
		{"NextToMark", 0x700UL},
		{"Initialized", 0x800UL}*/

		//DEMO_DAY2
		{"Suicidal", 0x100UL},
		{"HatesOther", 0x200UL},
		{"Murderous", 0x400UL},
		{"Estranged", 0x800UL},
		{"InScene", 0x1000UL},
		{"Alive", 0x2000UL},
		{"ConflictResolved", 0x4000UL},
		{"Finished", 0x8000UL}

	};

	// All attributes that can be used by objects of the class SmartCharacter
	public static string[] characterStates = 
	/*{"InScene", "KnowsBear1", "KnowsBear2", "IsHappy", "FeelsBad", "IsAngryAtBear1", "IsAngryAtBear2"};*/ //ORIGINAL
	
	//{"enemyIs0", "enemyIs1", "friendsWith0", "friendsWith1", "HasGift", "InScene", "hatesTheOther", "murdered"} //ORIGINAL DEMO STEAL
	
	/*{"InScene", "alive", "envies1", "attractedTo2", "marriedTo2", "marriedTo1", "seduced2", "HasGift"}*/ //SEDUCE DEMO ATTEMPT
	
	//GENERIC--WIP
	/*{"enemyIs0",
	"enemyIs1",
	"friendsWith0",
	"friendsWith1",
	"loves0",
	"loves1",
	"respects0",
	"respects1",
	"disrespects0",
	"disrespects1",
	"pleasedWith0",
	"pleasedWith1",
	"displeasedWith0",
	"displeasedWith1",
	"sorryFor0",
	"sorryFor1",
	"envies0",
	"envies1",
	"attractedTo0",
	"attractedTo1",
	"disgustedWith0",
	"disgustedWith1"}*/

	//DEMO_1 : Thief and Mark
	/*
	{
		"InScene",
		"HasItem",
		"KnowsOfItem",
		"Distracted",
		"MurderousToMark",
		"Alive",
		"NextToMark",
		"Initialized"
	}
	*/
	{
		"Suicidal",
		"HatesOther",
		"Murderous",
		"Estranged",
		"InScene",
		"Alive",
		"ConflictResolved",
		"Finished"
	}
	;

	/// <summary>
	/// Returns the possible states this type of smart object can have.
	/// </summary>
	/// <param name="type">The type of the smart object</param>
	public static string[] GetCorrectStates (Type type)
	{
		if (typeof(SmartCharacter).IsAssignableFrom(type))
		    return characterStates;
		else
			return null;
	}

	/// <summary>
	/// Returns the possible attributes this smart object can have.
	/// </summary>
	/// 
	/// <param name="obj">The smart object we want to retrieve the possible attributes for.</param>
	public static string[] GetCorrectStates (SmartObject obj)
	{
		switch (StateUtils.GetRole(obj))
		{
		case StateUtils.Role.Actor:
			return characterStates;
		default:
			return null;
		}

	}

	/// <summary>
	/// All possible roles for the current narrative. Each smart object is assigned one specific role.
	/// </summary>
    public enum Role
    {
        Actor = 0
    }

	/// <summary>
	/// Returns the current role of this smart object.
	/// </summary>
	/// 
	/// <param name="obj">The smart object we want to retrieve the role for.</param>
    public static Role GetRole(SmartObject obj)
    {
        return (Role) (obj.state & 0xFFUL);
    }

	/// <summary>
	/// Returns the current number of this smart object.
	/// Needed for the planner to differentiate between two smart objects of the same role.
	/// </summary>
	/// 
	/// <param name="obj">The smart object we want to retrieve the number of.</param>
    public static int GetNumber(SmartObject obj)
    {
        return (int) ((obj.state & 0xFF00000000000000UL) >> 56);
    }

	/// <summary>
	/// Sets the current number of this smart object.
	/// Needed for the planner to differentiate between two smart objects of the same role.
	/// </summary>
	///
	/// <param name="obj">The smart object we want to set the number of.</param>
	/// <param name="no">The number we want to set obj to.</param> 
    public static ulong SetNumber(SmartObject obj, int no)
    {
        ulong noToZero = (obj.state & ~(0xFF00000000000000UL));
        ulong extNo = (ulong)no;
        return (ulong) ((extNo << 56) | noToZero);
    }

	/// <summary>
	/// Checks whether this state contains a particular attribute.
	/// </summary>
	/// 
	/// <param name="state">The state we examine.</param>
	/// <param name="name">The name of the attribute.</param> 
	/// <param name="isTrue">Defines, if we check whether this attribute is true or false for this state</param>
    public static bool CheckState(ulong state, string name, bool isTrue)
	{
        if(isTrue)
            return (state & StateMasks[name]) > 0;

        return (state & StateMasks[name]) == 0;
    }

	/// <summary>
	/// Sets the attribute for the current state.
	/// </summary>
	///
	/// <param name="state">The state we examine.</param>
	/// <param name="name">The name of the attribute.</param> 
	/// <param name="AttributeValue">Defines, if we set this attribute to true or false for this state</param>
    public static ulong SetState(ulong state, string name, bool AttributeValue)
    {
        if (AttributeValue)
            return state | StateMasks [name];
        else
            return state & ~(StateMasks [name]);
    }
}
}
