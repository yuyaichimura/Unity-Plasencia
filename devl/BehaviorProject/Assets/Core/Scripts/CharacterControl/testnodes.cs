using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;
using TreeSharpPlus;


public class testnodes : MonoBehaviour {

    public SmartCharacter[] characters;
    public Transform[] targets;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Test();
        }
	}

    void Test()
    {
        Debug.Log("test");

//        Node[] nodeArray = { characters[0].Node_GoTo(targets[0].position), characters[0].Node_GoTo(targets[1].position), characters[0].Node_GoTo(targets[2].position) };

//        Node n = new Sequence(nodeArray);

        Node n = new Sequence(characters[0].Node_GoTo(Val.V(() => targets[0].position)), characters[0].Node_GoTo(Val.V(() => targets[1].position)), characters[0].Node_GoTo(Val.V(() => targets[2].position)));

        n.Start();
        n.Tick();
        n.Tick();

    }
}
