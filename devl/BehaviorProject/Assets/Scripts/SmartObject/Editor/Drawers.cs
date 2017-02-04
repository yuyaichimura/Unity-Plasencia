using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace POP
{
	
	/// <summary>
	/// Used to display the start states of all smart objects in a user friendly way in the inspector.
	/// </summary>
	[CustomPropertyDrawer (typeof (StatesAttribute))]
	public class StateDrawer : PropertyDrawer {

		// Provide easy access to the statesAttribute for reading information from it.
		StatesAttribute statesAttribute { get { return ((StatesAttribute)attribute); } }
		
		// Here you must define the height of your property drawer. Called by Unity.
		public override float GetPropertyHeight (SerializedProperty prop, GUIContent label) {
			return base.GetPropertyHeight (prop, label);
		}


		// Here you can define the GUI for your property drawer. Called by Unity.
		public override void OnGUI (Rect position, SerializedProperty prop, GUIContent label) {
			EditorGUI.BeginChangeCheck ();
			int index = 0;
			
			for (int i = 0; i < statesAttribute.states.Length; i++)
			{
				if (prop.stringValue == statesAttribute.states[i])
				{
					index = i;
					break;
				}
			}

			
			index = EditorGUI.Popup (position, "State: ",  index, statesAttribute.states);
			if (EditorGUI.EndChangeCheck ())
				prop.stringValue = statesAttribute.states[index];
		}
	}
}
	