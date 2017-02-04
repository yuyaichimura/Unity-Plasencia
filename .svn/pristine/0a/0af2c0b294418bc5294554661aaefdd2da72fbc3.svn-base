using UnityEngine;
using TreeSharpPlus;

public class SmartStoryText : SmartObject
{
    public StoryText texts;
    public TextDisplay textDisplay;

    public string message_key;

    public override string Archetype
    {
        get { return this.GetType().Name; }
    }

    public Node Node_DisplayText()
    {
        return new Sequence(

            new LeafInvoke(() => textDisplay.SetText(texts.texts[message_key])),
            new DecoratorInvert(new DecoratorLoop(
                new LeafAssert(() => textDisplay.displayingMessage())))
            );
    }
}
