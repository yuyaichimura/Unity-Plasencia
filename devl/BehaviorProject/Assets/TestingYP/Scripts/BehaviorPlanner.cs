using UnityEngine;
using System.Collections;
using TreeSharpPlus;
using System.Collections.Generic;

public class BehaviorPlanner : MonoBehaviour {

	static Queue<Node> nodeQueue;
	//keep the indices for behaviorQueues[] and characters[] synchronized with agent indices!

	void Start () {
		nodeQueue = new Queue<Node>();
		/*smartCharacters = new SmartCharacter[agentNum];
		int n = 0;
		foreach(GameObject go in characters){
			smartCharacters[n] = go.GetComponent<SmartCharacter>();
			Debug.Log(smartCharacters[n]);
			n++;
		}*/
		//spot = GameObject.Find("Cube");
		//spot2 = GameObject.Find("Sphere");
	}
	
	void Update () {
		
	}


	protected delegate Node doEvent(Token token);


	public static void executeTree(){
		SmartCharacter schar = GameObject.Find("Cain").GetComponent<SmartCharacter>();
		SmartCharacter schar2 = GameObject.Find("Abel").GetComponent<SmartCharacter>();
		SmartCharacter[] chars = {schar, schar2};
		BehaviorEvent BE = new BehaviorEvent(doEvent => new Sequence(nodeQueue.ToArray()), chars);
		BE.StartEvent(1F);
	}	

	public static void walkToNode(GameObject character, Val<Vector3> position){
		SmartCharacter schar = character.GetComponent<SmartCharacter>();
		Node n = schar.Node_GoTo(position);
		nodeQueue.Enqueue(n);
	}

	public static void orientTowardsEachOther(GameObject char1, GameObject char2){
		SmartCharacter schar1 = char1.GetComponent<SmartCharacter>();
		SmartCharacter schar2 = char2.GetComponent<SmartCharacter>();
		Val<Vector3> spot1pos = new Val<Vector3>(char1.transform.position);
		Val<Vector3> spot2pos = new Val<Vector3>(char2.transform.position);
		Node n = new Sequence(schar1.Node_OrientTowards(spot2pos), schar2.Node_OrientTowards(spot1pos));
		nodeQueue.Enqueue(n);
	}

	public static void orientTowards(GameObject char1, Vector3 position){
		SmartCharacter schar1 = char1.GetComponent<SmartCharacter>();
		Node n = new Sequence(schar1.Node_OrientTowards(position));
		nodeQueue.Enqueue(n);
	}

	public static void orientTowards(GameObject char1, Val<Vector3> position){
		SmartCharacter schar1 = char1.GetComponent<SmartCharacter>();
		Node n = new Sequence(schar1.Node_OrientTowards(position));
		nodeQueue.Enqueue(n);
	}
	public static void handGesture(GameObject character, string animation, long milliseconds){
		SmartCharacter schar = character.GetComponent<SmartCharacter>();
		Val<string> name = new Val<string>(animation);
		Val<long> duration = new Val<long>(milliseconds);
		Node n = schar.Node_PlayHandGesture(animation, milliseconds);
		nodeQueue.Enqueue(n);
	}

	public static void enqueueNode(Node n){
		nodeQueue.Enqueue(n);
	}

	//QuietBlood
	public static void startDemo2_Quiet(GameObject char1, GameObject char2){
		SmartCharacter schar1 = char1.GetComponent<SmartCharacter>();
		SmartCharacter schar2 = char2.GetComponent<SmartCharacter>();
		Val<Vector3> spot1 = new Val<Vector3>(char1.transform.position);
		Val<Vector3> spot2 = new Val<Vector3>(char2.transform.position);
		Val<Vector3> escape1 = new Val<Vector3>(GameObject.Find("Escape1").transform.position);
		Val<Vector3> escape2 = new Val<Vector3>(GameObject.Find("Escape2").transform.position);

		nodeQueue.Enqueue(new Sequence(schar1.Node_OrientTowards(spot2), schar2.Node_OrientTowards(spot1)));
		handGesture(char1, "Pointing", 1000L);
		handGesture(char2, "Cry", 1000L);
		handGesture(char2, "Pointing", 1000L);
		handGesture(char1, "BeingCocky", 1000L);
		nodeQueue.Enqueue(new SequenceParallel(schar1.Node_PlayHandGesture("ChestPumpSalute", 1000L), schar2.Node_PlayHandGesture("ChestPumpSalute", 1000L)));
		nodeQueue.Enqueue(new SequenceParallel(schar1.Node_GoTo(escape1), schar2.Node_GoTo(escape2)));
		executeTree();
	}

	//SadBlood
	public static void startDemo2_Sad(GameObject char1, GameObject char2){
		SmartCharacter schar1 = char1.GetComponent<SmartCharacter>();
		SmartCharacter schar2 = char2.GetComponent<SmartCharacter>();
		Val<Vector3> spot1 = new Val<Vector3>(char1.transform.position);
		Val<Vector3> spot2 = new Val<Vector3>(char2.transform.position);
		Val<Vector3> escape1 = new Val<Vector3>(GameObject.Find("Escape1").transform.position);
		Val<Vector3> escape2 = new Val<Vector3>(GameObject.Find("Escape2").transform.position);

		nodeQueue.Enqueue(new Sequence(schar1.Node_OrientTowards(spot2), schar2.Node_OrientTowards(spot1)));
		handGesture(char1, "Pointing", 1000L);
		handGesture(char2, "Cry", 1000L);
		handGesture(char2, "Pointing", 1000L);
		//handGesture(char1, "BeingCocky", 1000L);
		nodeQueue.Enqueue(new SequenceParallel(schar1.Node_PlayHandGesture("StayAway", 1000L), schar2.Node_PlayHandGesture("Shocked", 1000L)));
		nodeQueue.Enqueue(new SequenceParallel(schar1.Node_GoTo(escape1), schar2.Node_PlayHandGesture("Cry", 1000L)));
		nodeQueue.Enqueue(schar2.Node_GoTo(escape2));
		executeTree();
	}

	//BadBlood
	public static void startDemo2_Bad(GameObject char1, GameObject char2){
		SmartCharacter schar1 = char1.GetComponent<SmartCharacter>();
		SmartCharacter schar2 = char2.GetComponent<SmartCharacter>();
		Val<Vector3> spot1 = new Val<Vector3>(char1.transform.position);
		Val<Vector3> spot2 = new Val<Vector3>(char2.transform.position);
		Val<Vector3> escape1 = new Val<Vector3>(GameObject.Find("Escape1").transform.position);
		Val<Vector3> escape2 = new Val<Vector3>(GameObject.Find("Escape2").transform.position);

		nodeQueue.Enqueue(new Sequence(schar1.Node_OrientTowards(spot2), schar2.Node_OrientTowards(spot1)));
		handGesture(char1, "Pointing", 1000L);
		handGesture(char2, "Cry", 1000L);
		handGesture(char2, "Pointing", 1000L);
		handGesture(char1, "BeingCocky", 1000L);
		nodeQueue.Enqueue(new SequenceParallel(schar1.Node_PlayHandGesture("BlockWay", 1000L), schar2.Node_PlayHandGesture("Shocked", 1000L)));
		nodeQueue.Enqueue(new SequenceParallel(schar1.Node_GoTo(char2.transform.position), schar2.Node_PlayHandGesture("HandsUp", 1000L)));
		nodeQueue.Enqueue(schar1.Node_PlayHandGesture("HitStealth", 1000L));
		nodeQueue.Enqueue(schar1.Node_GoTo(escape1));
		executeTree();
	}
}
