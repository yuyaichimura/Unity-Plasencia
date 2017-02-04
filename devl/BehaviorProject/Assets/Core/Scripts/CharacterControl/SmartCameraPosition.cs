using UnityEngine;
using TreeSharpPlus;
using System.Collections;

public class SmartCameraPosition : SmartObject {

    public override string Archetype
    {
        get { return this.GetType().Name; }
    }
}
