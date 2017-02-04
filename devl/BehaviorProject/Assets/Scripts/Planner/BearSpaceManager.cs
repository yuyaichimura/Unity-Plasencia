using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Reflection;
using System.Linq;

namespace POP {
	/// <summary>
	/// Manages the complete state space. It is responsible for storing all smart objects in the scene and 
	/// generating the complete list of affordances and interactions.
	/// </summary>
	public class BearSpaceManager : MonoBehaviour {

		public int noOfObjects;
		public SmartObject[] allSmartObjects;
		public List<POP.Action> allAffordances;
		public List<POP.Action> transitionList;
		public List<POP.Action> interactionList;
	    public Dictionary<Tuple<StateUtils.Role,int>, int> globalStateIndexes;
	    public SmartObject[] orderedSmartObjects;

	    int actorCount;

		// Use this for initialization
		public void Start () {
			Init ();
		}
		
		public void Init() 
		{
			allAffordances = FindSubClassesOf<POP.Action> ();
			globalStateIndexes = new Dictionary<Tuple<StateUtils.Role, int>, int>();
			transitionList = new List<Action> ();
			interactionList = new List<Action> ();

			assignRoleNumbers ();
			dictToList ();
			createTransitionList ();
		}

		/// <summary>
		/// Sets the given global state to the state before executing the given transition
		/// 
		/// <param name="curState">The current global state</param>
		/// <param name="transition">The given transition that we want to set the state to</param>
		/// </summary>
		public GlobalState setGlobalStateBeforeAffordance(GlobalState curState, POP.Action transition) {
			int[] indices = extractRelevantStateIndices (transition);
			GlobalState newState = new GlobalState(curState);
			int nrRoles = transition.NrOfNeededRoles ();
			ulong[] resultingStates = new ulong[nrRoles];

			for (int i = 0; i < nrRoles; i++) {
				resultingStates[i] = curState.globalState[indices[i]];
			}
			resultingStates = transition.GetPreconditionStates (resultingStates);
			for (int i = 0; i < nrRoles; i++) {
				newState.globalState[indices[i]] = resultingStates[i];
			}
			return newState;
		}

		/// <summary>
		/// Sets the given global state to the state after executing the given transition
		/// 
		/// <param name="curState">The current global state</param>
		/// <param name="transition">The given transition that we want to set the state to</param>
		/// </summary>
		public GlobalState setGlobalStateAfterAffordance(GlobalState curState, POP.Action transition) {
			int[] indices = extractRelevantStateIndices (transition);
			GlobalState newState = new GlobalState(curState);
			int nrRoles = transition.NrOfNeededRoles ();
			ulong[] resultingStates = new ulong[nrRoles];

			for (int i = 0; i < nrRoles; i++) {
				resultingStates[i] = curState.globalState[indices[i]];
			}
			resultingStates = transition.GetResultingStates (resultingStates);
			for (int i = 0; i < nrRoles; i++) {
				newState.globalState[indices[i]] = resultingStates[i];
			}
			return newState;
		}

		/// <summary>
		/// Extracts the indices of the relevant states for the given transition.
		/// 
		/// <param name="transition">The given transition we want to get the state indices for</param>
		/// </summary>
		public int[] extractRelevantStateIndices(POP.Action transition) {
			int nrRoles = transition.NrOfNeededRoles();
			int[] indices = new int[nrRoles];
			for (int i=0; i<nrRoles; i++) {
				SmartObject curObj = transition.ArgumentList[i];
				indices[i] = globalStateIndexes[new Tuple<StateUtils.Role,int>(StateUtils.GetRole(curObj),StateUtils.GetNumber(curObj))];
			}
			return indices;
		}


		/// <summary>
		/// Extracts the relevat states for the given transition.
		/// 
		/// <param name="state">The current global state.</param>
		/// <param name="transition">The given transition we want to get the state indices for</param>
		/// </summary>
		public ulong[] extractRelevantStates(GlobalState state, POP.Action transition) {
			int[] relevantStateIndices = extractRelevantStateIndices(transition);
			ulong[] relevantStates = new ulong[relevantStateIndices.Length];
			for(int j = 0; j < relevantStates.Length; j++) 
			{
				relevantStates[j] = state.globalState[relevantStateIndices[j]];
			}
			return relevantStates;
		}

		/// <summary>
		/// Creates an instance of every possible affordance in the given scene. 
		/// For every affordance there are multiple instances with all
		/// possible argument-combinations. These are saved in the transitionList.
		/// </summary>
		void createTransitionList()
		{
			foreach (Action nes in allAffordances) 
			{
				StateUtils.Role[] roles = nes.Roles;
				int length = roles.Length;
				List<SmartObject>[] possibleArguments = new List<SmartObject>[length];
				for (int arg = 0; arg<length; arg++) 
				{
					possibleArguments[arg] = new List<SmartObject>();
					foreach (SmartObject obj in allSmartObjects) 
					{
						if(StateUtils.GetRole(obj) == roles[arg]) 
						{
							possibleArguments[arg].Add(obj);
						}
					}
				}

				//Create instances with all possible combinations of arguments
				IEnumerable<IEnumerable<SmartObject>> allCombs = HelperMethods.CartesianProduct(possibleArguments);
				foreach (IEnumerable<SmartObject> args in allCombs) 
				{
					// check here if affordances are correct
					POP.Action newAction = (POP.Action)Activator.CreateInstance(nes.GetType());
					newAction.ArgumentList = args.ToArray();
					if (newAction.Preconditions != null)
						transitionList.Add(newAction);
				}
			}
		}

		/// <summary>
		/// Gets the start state of all objects.
		/// </summary>
		public GlobalState GetGlobalStartState ()
		{
			ulong[] currentState = new ulong[noOfObjects];
			foreach (SmartObject curObj in allSmartObjects)
			{
				int i = globalStateIndexes[new Tuple<StateUtils.Role,int>(StateUtils.GetRole(curObj),StateUtils.GetNumber(curObj))];
				currentState[i] = curObj.StartState;
				currentState[i] = StateUtils.SetNumber(curObj, StateUtils.GetNumber(curObj));
			}
			
			return new GlobalState (currentState);	
		}

		/// <summary>
		/// Gets the base state of all objects.
		/// </summary>
		public GlobalState GetGlobalBaseState ()
		{
			ulong[] currentState = new ulong[noOfObjects];
			foreach (SmartObject curObj in allSmartObjects)
			{
				int i = globalStateIndexes[new Tuple<StateUtils.Role,int>(StateUtils.GetRole(curObj),StateUtils.GetNumber(curObj))];
				currentState[i] = curObj.BaseState;
				currentState[i] = StateUtils.SetNumber(curObj, StateUtils.GetNumber(curObj));
			}
			
			return new GlobalState (currentState);	
		}

		/// <summary>
		/// If application is playing: Gets current global state of all objects.
		/// Otherwise: Gets the base state of all objects.
		/// </summary>
		public GlobalState GetGlobalState ()
		{
			if (allSmartObjects == null || allSmartObjects.Length == 0)
				Init ();
			ulong[] currentState = new ulong[noOfObjects];
			foreach (SmartObject curObj in allSmartObjects)
			{
				int i = globalStateIndexes[new Tuple<StateUtils.Role,int>(StateUtils.GetRole(curObj),StateUtils.GetNumber(curObj))];
				currentState[i] = curObj.state;
			}
			
			return new GlobalState (currentState);	
		}
		
		/// <summary>
		/// Converts the Dictionary of all objects into a list of them sorted by role. Maintains a lookuptable from Role and RoleNumber to the
		/// index of the object in the list.
		/// </summary>
		void dictToList()
		{
			int index = 0;
			Tuple<StateUtils.Role,int> t = new Tuple<StateUtils.Role,int>(StateUtils.Role.Actor,0);

			// add object state
			for(int i = 0; i < actorCount; i++) {
				t.Element2 = i;
				globalStateIndexes.Add(new Tuple<StateUtils.Role,int>(StateUtils.Role.Actor,i),index);
				index++;
			}
			// create an array of the ordered list to quickly retrieve an object, if necessary
			orderedSmartObjects = new SmartObject[noOfObjects];
			foreach (SmartObject obj in allSmartObjects)
			{
				index = globalStateIndexes[new Tuple<StateUtils.Role, int> (StateUtils.GetRole(obj), StateUtils.GetNumber(obj))];
				orderedSmartObjects[index] = obj;
			}
		}

		/// <summary>
		/// Finds all the Subclasses of a class that inherits from MonoBehavioor and returns them as a gameobject.
		/// </summary>
		public List<Action> FindSubClassesOf<TBaseType>()
		{   
			var baseType = typeof(TBaseType);
			var assembly = baseType.Assembly;
			Type[] types = assembly.GetTypes ();
			List<Action> result = new List<Action> ();
			foreach (Type t in types){
				if(t.IsSubclassOf(baseType) && !t.IsAbstract)
				{
					result.Add((Action) Activator.CreateInstance(t));
				}
			}
			
			return result;
		}

		/// <summary>
		/// Finds all the SmartObjects in the scene and adds them to the dictionary according to their type. Also assigns a role number for
		/// every SmartObject.
		/// </summary>
		void assignRoleNumbers()
		{
			SmartObject curObj;

			allSmartObjects = (SmartObject[]) FindObjectsOfType (typeof(SmartObject));
			noOfObjects = allSmartObjects.Length;
			actorCount = 0;

			for (int i = 0; i < noOfObjects; i++) 
			{
				curObj = allSmartObjects[i];
				curObj.Init();
				switch (StateUtils.GetRole(curObj))
				{
				case StateUtils.Role.Actor:
					curObj.state = StateUtils.SetNumber(curObj, actorCount);
					actorCount++;
					break;
				}
			}
		}

	}
}