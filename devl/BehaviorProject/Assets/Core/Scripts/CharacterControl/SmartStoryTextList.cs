using UnityEngine;
using TreeSharpPlus;

public class SmartStoryTextList : SmartObject {

	public SmartStoryText[] Text;

    public override string Archetype
    {
        get { return this.GetType().Name; }
    }
}
