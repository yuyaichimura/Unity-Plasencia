using TreeSharpPlus;
using System.Linq;
using UnityEngine;
using RootMotion.FinalIK;

public class TestHeadLook : MonoBehaviour {

    public Transform t1;
    public Transform t2;
    public CharacterMecanim charMecanim;
	
	// Update is called once per frame
	void Update () {
        charMecanim.HeadLookAt(t1.position);
	}
}
