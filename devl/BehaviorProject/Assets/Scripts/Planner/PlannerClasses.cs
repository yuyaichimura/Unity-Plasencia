using UnityEngine;
using System;
using System.Collections.Generic;

namespace POP
{
	/// <summary>
	/// used to define preconditions and postconditions for the planner.
	/// </summary>
	[Serializable]
	public class Condition
	{
		// the index of the smart object.
		public int index;
		// the name of the attribute.
		public string name;
		//Defines, whether the attribute is true or false for this particular object.
		public bool isTrue;
		
		public Condition(int index, string name, bool isTrue)
		{
			this.index = index;
			this.name = name;
			this.isTrue = isTrue;
		}

		/// <summary>
		/// Return true, if both objects refer to the same smartobject and the same the state
		/// </summary>
		/// 
		/// <param name="item2">The object we want to compare this one with.</param>
		public bool SameConditionAndObject (Condition item2)
		{
			if (this.index == item2.index && this.name == item2.name)
				return true;
			return false;
		}

		public override int GetHashCode ()
		{
			return base.GetHashCode ();
		}

		public override bool Equals(object obj)
		{

			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			return Equals((Condition)obj);
		}
		
		public bool Equals(Condition item2)
		{
			if (this.index.Equals(item2.index) && this.name.Equals(item2.name) && this.isTrue.Equals(item2.isTrue))
				return true;

			return false;
		}
	}
}