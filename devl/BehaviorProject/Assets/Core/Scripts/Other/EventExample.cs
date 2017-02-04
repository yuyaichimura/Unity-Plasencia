using UnityEngine;
using TreeSharpPlus;
using System.Collections;

public class EventExample : MonoBehaviour {

    public SmartCharacter sc;
    public delegate Node doSomething(Token t);
    public Transform destination;

    private bool exec = false;

    void Update() {
        if (!exec) {
            Val<Vector3> val = Val.V(() => destination.position);
            sc.StopBehavior();
            SmartCharacter[] chars = { sc };
            BehaviorEvent e = new BehaviorEvent(doSomething => sc.Node_GoTo(val), chars);
            e.StartEvent(0f);
            exec = true;
        }
    }

}
