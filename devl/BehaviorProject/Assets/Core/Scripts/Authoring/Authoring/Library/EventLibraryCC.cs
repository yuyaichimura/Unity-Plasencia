using TreeSharpPlus;
using System.Linq;
using UnityEngine;
using RootMotion.FinalIK;

#region STORY_PLASENCIA
[LibraryIndexAttribute(6)]
public class _CCGoTo : SmartEvent
{
    SmartCharacterCC character;
    SmartWaypoint waypoint;

    [Name("CC Go To")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _CCGoTo(SmartCharacterCC character, SmartWaypoint waypoint)
        : base(character, waypoint)
    {
        this.character = character;
        this.waypoint = waypoint;
    }

    public override Node BakeTree(Token token)
    {
        return character.Node_GoToX(waypoint);
    }
}

[LibraryIndexAttribute(6)]
public class _CCGoTo2 : SmartEvent
{
    SmartCharacterCC character;
    SmartCharacterCC character2;
    SmartWaypoint waypoint;

    [Name("CC Go To 2")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _CCGoTo2(SmartCharacterCC character, SmartCharacterCC character2, SmartWaypoint waypoint)
        : base(character, character2, waypoint)
    {
        this.character = character;
        this.character2 = character2;

        this.waypoint = waypoint;
    }

    public override Node BakeTree(Token token)
    {
        return new SelectorParallel(
            character.Node_GoToX(waypoint),
            character2.Node_GoToX(waypoint)
            );
    }
}

[LibraryIndexAttribute(6)]
public class _CCGoToUpToRadius : SmartEvent
{
    SmartCharacterCC character;
    SmartObject obj;
    string message = "Please go to ";

    [Name("CC Go Near")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _CCGoToUpToRadius(SmartCharacterCC character, SmartObject obj)
        : base(character, obj)
    {
        this.character = character;
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        return character.Node_GoToX(obj);
    }
}

[LibraryIndexAttribute(6)]
public class _CCGoToH : SmartEvent
{
    SmartCharacterCC character;
    SmartObject obj;
    string message = "Please go to ";

    [Name("CC Go To -Crowd")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _CCGoToH(SmartCharacterCC character, SmartObject obj)
        : base(character, obj)
    {
        this.character = character;
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        return character.Node_GoToUpToRadius(obj, 0.5f);
    }
}

[LibraryIndexAttribute(6)]
public class _CCLookAt : SmartEvent
{
    SmartCharacterCC character;
    SmartObject obj;

    [Name("CC Look at")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleWaypoint)]
    public _CCLookAt(SmartCharacterCC character, SmartObject obj)
        : base(character, obj)
    {
        this.character = character;
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        return character.Node_OrientTowards(obj.transform.position);
    }
}

[LibraryIndexAttribute(6)]
public class _CCBow : SmartEvent
{
    SmartCharacterCC character;
    SmartCharacterCC obj;

    [Name("Bow to")]
    public _CCBow(SmartCharacterCC character, SmartCharacterCC obj)
        : base(character, obj)
    {
        this.character = character;
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        return character.Node_Interact(obj, "BOW");
    }
}

[LibraryIndexAttribute(6)]
public class _CCBow2 : SmartEvent
{
    SmartCharacterCC character;
    SmartCharacterCC obj;

    [Name("Bow to 2")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _CCBow2(SmartCharacterCC character, SmartCharacterCC obj)
        : base(character, obj)
    {
        this.character = character;
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        return character.Node_Interact(obj, "BOW");
    }
}

[LibraryIndexAttribute(6)]
public class _CCOpenDoor : SmartEvent
{
    SmartCharacterCC character;
    SmartDoor obj;

    [Name("Open Door")]
    public _CCOpenDoor(SmartCharacterCC character, SmartDoor obj)
        : base(character, obj)
    {
        this.character = character;
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        return character.Node_Interact(obj, "OPENDOOR");
    }
}

[LibraryIndexAttribute(6)]
public class _CCIntimidate : SmartEvent
{
    SmartCharacterCC character;
    SmartCharacterCC obj;

    [Name("Intimidate")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _CCIntimidate(SmartCharacterCC character, SmartCharacterCC obj)
        : base(character, obj)
    {
        this.character = character;
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        //        return new Sequence(character.Node_GoToUpToRadius(Val.V(() => waypoint.transform.position), 1.0f));
        //        return character.Node_Bow(obj);
        return SmartCharacterCC.Node_Interaction("intimidate", character, obj);

    }
}

[LibraryIndexAttribute(6)]
public class _CCHandShake : SmartEvent
{
    SmartCharacterCC character;
    SmartCharacterCC obj;

    [Name("HandShake")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _CCHandShake(SmartCharacterCC character, SmartCharacterCC obj)
        : base(character, obj)
    {
        this.character = character;
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        //        return new Sequence(character.Node_GoToUpToRadius(Val.V(() => waypoint.transform.position), 1.0f));
        //        return character.Node_Bow(obj);
        return character.Node_Interact(obj, "SHAKEHAND");

    }
}

[LibraryIndexAttribute(6)]
public class _CCAngryConversation : SmartEvent
{
    SmartCharacterCC character;
    SmartCharacterCC obj;

    [Name("Angry Conversation")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _CCAngryConversation(SmartCharacterCC character, SmartCharacterCC obj)
        : base(character, obj)
    {
        this.character = character;
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            character.Node_InteractY(obj, "INTIMIDATE"),
            obj.Node_InteractY(character, "RIGHTHOOK"),
            character.Node_InteractY(obj, "FISTSHAKE"),
            obj.Node_InteractY(character, "YELL1"),
            character.Node_InteractY(obj, "YELL2"),
            obj.Node_InteractY(character, "DIE")
            );
    }
}

[LibraryIndexAttribute(6)]
public class _CCDisappear : SmartEvent
{
    SmartObject obj;

    [Name("Disappear")]
    [StateRequired(0, StateName.RoleActor)]
    public _CCDisappear(SmartObject obj)
        : base(obj)
    {
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        return new LeafInvoke(() => this.obj.gameObject.active = false);
    }
}

[LibraryIndexAttribute(6)]
public class _CCAppear : SmartEvent
{
    SmartObject obj;

    [Name("Appear")]
    [StateRequired(0, StateName.RoleActor)]
    public _CCAppear(SmartObject obj)
        : base(obj)
    {
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        return new LeafInvoke(() => this.obj.gameObject.active = false);
    }
}

[LibraryIndexAttribute(6)]
public class _CCTalkTo : SmartEvent
{
    SmartCharacterCC character;
    SmartCharacterCC obj;

    [Name("Talk to")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _CCTalkTo(SmartCharacterCC character, SmartCharacterCC obj)
        : base(character, obj)
    {
        this.character = character;
        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        //        return new Sequence(character.Node_GoToUpToRadius(Val.V(() => waypoint.transform.position), 1.0f));
        //       return character.Node_Bow(obj);
        return character.Node_Interact(obj, "BOW");

    }
}

[LibraryIndexAttribute(6)]
public class _CCGroupMeeting7 : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;
    SmartCharacterCC char6;
    SmartCharacterCC char7;
    SmartWaypointArea area;

    [Name("Group Meeting 7")]
    [StateRequired(0, StateName.RoleWaypoint)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(7, StateName.RoleActor, StateName.IsStanding)]
    public _CCGroupMeeting7(SmartWaypointArea area, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7)
        : base(area, char1, char2, char3, char4, char5, char6, char7)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
        this.char6 = char6;
        this.char7 = char7;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        Vector3 lookAt = this.area.Waypoints[0].position;

        return new SequenceParallel(
            char1.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char2.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char3.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char4.Node_GoToX(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char5.Node_GoToX(area.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char6.Node_GoToX(area.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char7.Node_GoToX(area.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt))
            );
    }
}

[LibraryIndexAttribute(6)]
public class _CCGroupMeetGreet9 : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;
    SmartCharacterCC char6;
    SmartCharacterCC char7;
    SmartCharacterCC char8;
    SmartCharacterCC char9;
    SmartWaypointArea area;

    [Name("Group Meet and Greet 9")]
    [StateRequired(0, StateName.RoleWaypoint)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(7, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(8, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(9, StateName.RoleActor, StateName.IsStanding)]
    public _CCGroupMeetGreet9(SmartWaypointArea area, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8, SmartCharacterCC char9)
        : base(area, char1, char2, char3, char4, char5, char6, char7, char8, char9)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
        this.char6 = char6;
        this.char7 = char7;
        this.char8 = char8;
        this.char9 = char9;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        Vector3 lookAt = this.area.Waypoints[0].position;

        return new Sequence(
            new SequenceParallel(
            char1.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char2.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char3.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char4.Node_GoToX(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char5.Node_GoToX(area.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char6.Node_GoToX(area.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char7.Node_GoToX(area.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char8.Node_GoToX(area.Waypoints[8].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char9.Node_GoToX(area.Waypoints[9].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt))
            ),
            new Sequence(
                char1.Node_Perform("bow"),
                new LeafWait(1000)),
            new SequenceParallel(
                char2.Node_Perform("bow"),
                char3.Node_Perform("bow"),
                char4.Node_Perform("bow"),
                char5.Node_Perform("bow"),
                char6.Node_Perform("bow"),
                char7.Node_Perform("bow"),
                char8.Node_Perform("bow"),
                char9.Node_Perform("bow")
            ), new LeafWait(1300));
    }
}

[LibraryIndexAttribute(6)]
public class _CCGroupMeeting8 : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;
    SmartCharacterCC char6;
    SmartCharacterCC char7;
    SmartCharacterCC char8;

    SmartWaypointArea area;

    [Name("Group Meeting 8")]
    [StateRequired(0, StateName.RoleWaypoint)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(7, StateName.RoleActor, StateName.IsStanding)]
    public _CCGroupMeeting8(SmartWaypointArea area, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8)
        : base(area, char1, char2, char3, char4, char5, char6, char7, char8)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
        this.char6 = char6;
        this.char7 = char7;
        this.char8 = char8;

        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        Vector3 lookAt = this.area.Waypoints[0].position;

        return new SequenceParallel(
            char1.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char2.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char3.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char4.Node_GoToX(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char5.Node_GoToX(area.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char6.Node_GoToX(area.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char7.Node_GoToX(area.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char8.Node_GoToX(area.Waypoints[8].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt))

            );
    }
}

[LibraryIndexAttribute(6)]
public class _CCGroupMeeting9 : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;
    SmartCharacterCC char6;
    SmartCharacterCC char7;
    SmartCharacterCC char8;
    SmartCharacterCC char9;

    SmartWaypointArea area;

    [Name("Group Meeting 9")]
    [StateRequired(0, StateName.RoleWaypoint)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(7, StateName.RoleActor, StateName.IsStanding)]
    public _CCGroupMeeting9(SmartWaypointArea area, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8, SmartCharacterCC char9)
        : base(area, char1, char2, char3, char4, char5, char6, char7, char8, char9)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
        this.char6 = char6;
        this.char7 = char7;
        this.char8 = char8;
        this.char9 = char9;

        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        Vector3 lookAt = this.area.Waypoints[0].position;

        return new SequenceParallel(
            char1.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char2.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char3.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char4.Node_GoToX(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char5.Node_GoToX(area.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char6.Node_GoToX(area.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char7.Node_GoToX(area.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char8.Node_GoToX(area.Waypoints[8].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char9.Node_GoToX(area.Waypoints[9].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt))

            );
    }
}

[LibraryIndexAttribute(6)]
public class _CCGroupMeeting10 : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;
    SmartCharacterCC char6;
    SmartCharacterCC char7;
    SmartCharacterCC char8;
    SmartCharacterCC char9;
    SmartCharacterCC char10;

    SmartWaypointArea area;

    [Name("Group Meeting 10")]
    [StateRequired(0, StateName.RoleWaypoint)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(7, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(8, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(9, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(10, StateName.RoleActor, StateName.IsStanding)]
    public _CCGroupMeeting10(SmartWaypointArea area, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8, SmartCharacterCC char9, SmartCharacterCC char10)
        : base(area, char1, char2, char3, char4, char5, char6, char7, char8, char9, char10)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
        this.char6 = char6;
        this.char7 = char7;
        this.char8 = char8;
        this.char9 = char9;
        this.char10 = char10;

        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        Vector3 lookAt = this.area.Waypoints[0].position;

        return new SequenceParallel(
            char1.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char2.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char3.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char4.Node_GoToX(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char5.Node_GoToX(area.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char6.Node_GoToX(area.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char7.Node_GoToX(area.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char8.Node_GoToX(area.Waypoints[8].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char9.Node_GoToX(area.Waypoints[9].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char10.Node_GoToX(area.Waypoints[10].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt))
            );
    }
}

[LibraryIndexAttribute(6)]
public class _CCGroupMeetGreetRace12 : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;
    SmartCharacterCC char6;
    SmartCharacterCC char7;
    SmartCharacterCC char8;
    SmartCharacterCC char9;
    SmartCharacterCC char10;
    SmartCharacterCC char11;
    SmartCharacterCC char12;
    SmartWaypointArea area;

    [Name("Group Meet and Greet 12 Race")]
    [StateRequired(0, StateName.RoleWaypoint)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(7, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(8, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(9, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(10, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(11, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(12, StateName.RoleActor, StateName.IsStanding)]
    public _CCGroupMeetGreetRace12(SmartWaypointArea area, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8, SmartCharacterCC char9, SmartCharacterCC char10, SmartCharacterCC char11, SmartCharacterCC char12)
        : base(area, char1, char2, char3, char4, char5, char6, char7, char8, char9, char10, char11, char12)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
        this.char6 = char6;
        this.char7 = char7;
        this.char8 = char8;
        this.char9 = char9;
        this.char10 = char10;
        this.char11 = char11;
        this.char12 = char12;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        Vector3 lookAt = this.area.Waypoints[0].position;

        return new Race(
            new Sequence(
            new SequenceParallel(
            char1.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char2.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char3.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char4.Node_GoToX(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char5.Node_GoToX(area.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char6.Node_GoToX(area.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char7.Node_GoToX(area.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char8.Node_GoToX(area.Waypoints[8].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char9.Node_GoToX(area.Waypoints[9].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char10.Node_GoToX(area.Waypoints[10].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char11.Node_GoToX(area.Waypoints[11].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char12.Node_GoToX(area.Waypoints[12].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt))
            )),
            new LeafWait(2000)
            );
    }
}

[LibraryIndexAttribute(6)]
public class _CCGroupMeetGreet12 : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;
    SmartCharacterCC char6;
    SmartCharacterCC char7;
    SmartCharacterCC char8;
    SmartCharacterCC char9;
    SmartCharacterCC char10;
    SmartCharacterCC char11;
    SmartCharacterCC char12;
    SmartWaypointArea area;

    [Name("Group Meet and Greet 12")]
    [StateRequired(0, StateName.RoleWaypoint)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(7, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(8, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(9, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(10, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(11, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(12, StateName.RoleActor, StateName.IsStanding)]
    public _CCGroupMeetGreet12(SmartWaypointArea area, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8, SmartCharacterCC char9, SmartCharacterCC char10, SmartCharacterCC char11, SmartCharacterCC char12)
        : base(area, char1, char2, char3, char4, char5, char6, char7, char8, char9, char10, char11, char12)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
        this.char6 = char6;
        this.char7 = char7;
        this.char8 = char8;
        this.char9 = char9;
        this.char10 = char10;
        this.char11 = char11;
        this.char12 = char12;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        Vector3 lookAt = this.area.Waypoints[0].position;

        return new Sequence(
            new SequenceParallel(
            char1.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char2.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char3.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char4.Node_GoToX(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char5.Node_GoToX(area.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char6.Node_GoToX(area.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char7.Node_GoToX(area.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char8.Node_GoToX(area.Waypoints[8].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char9.Node_GoToX(area.Waypoints[9].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char10.Node_GoToX(area.Waypoints[10].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char11.Node_GoToX(area.Waypoints[11].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char12.Node_GoToX(area.Waypoints[12].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt))
            ),
            new Sequence(
                char1.Node_Perform("bow"),
                new LeafWait(1000)),
            new SequenceParallel(
                char2.Node_Perform("bow"),
                char3.Node_Perform("bow"),
                char4.Node_Perform("bow"),
                char5.Node_Perform("bow"),
                char6.Node_Perform("bow"),
                char7.Node_Perform("bow"),
                char8.Node_Perform("bow"),
                char9.Node_Perform("bow"),
                char10.Node_Perform("bow"),
                char11.Node_Perform("bow"),
                char12.Node_Perform("bow")
            ),
            new LeafWait(2500));
    }
}

[LibraryIndexAttribute(6)]
public class _CCMeetAndGreet : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartWaypoint point1;
    SmartWaypoint point2;

    [Name("Greet")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    public _CCMeetAndGreet(SmartCharacterCC char1, SmartCharacterCC char2, SmartWaypoint point1, SmartWaypoint point2)
        : base(char1, char2)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.point1 = point1;
        this.point2 = point2;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            new SequenceParallel(
                char1.Node_GoToX(point1),
                char2.Node_GoToX(point2)),
            char1.Node_Interact(char2, "BOW")

            );
    }
}

[LibraryIndexAttribute(6)]
public class _CCGroupMeeting6 : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;
    SmartCharacterCC char6;
    SmartCharacterCC char7;
    SmartWaypointArea area;

    [Name("Group Meeting 6")]
    [StateRequired(0, StateName.RoleWaypoint)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    public _CCGroupMeeting6(SmartWaypointArea area, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6)
        : base(area, char1, char2, char3, char4, char5, char6)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
        this.char6 = char6;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        Vector3 lookAt = this.area.Waypoints[0].position;
        SmartWaypoint point = area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>();
        if (point == null)
        {
            Debug.Log("NULL TARGET");

        }
        return new SequenceParallel(
            char1.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char2.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char3.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char4.Node_GoToX(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char5.Node_GoToX(area.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt)),
            char6.Node_GoToX(area.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), Val.V(() => lookAt))
            );
    }
}

[LibraryIndexAttribute(6)]
public class _CCGroupGreeting7 : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;
    SmartCharacterCC char6;
    SmartCharacterCC char7;


    [Name("GroupGreeting7")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    public _CCGroupGreeting7(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7)
        : base(char1, char2, char3, char4, char5, char6, char7)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
        this.char6 = char6;
        this.char7 = char7;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            new SequenceParallel(

                    char2.Node_Perform("bow"),
                    char3.Node_Perform("bow"),
                    char4.Node_Perform("bow"),
                    char5.Node_Perform("bow"),
                    char6.Node_Perform("bow"),
                    char7.Node_Perform("bow")
                ),
                    char1.Node_Perform("bow")
            );
    }
}

[LibraryIndexAttribute(6)]
public class _CCInspectHouse : SmartEvent
{
    SmartDoor door;
    SmartCharacterCC inspector;
    SmartCharacterCC character;
    SmartWaypointArea area;

    [Name("Inspect House")]
    [StateRequired(0, StateName.RoleDoor)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleWaypoint)]
    public _CCInspectHouse(SmartDoor door, SmartCharacterCC inspector, SmartCharacterCC character, SmartWaypointArea area)
        : base(door, inspector, character, area)
    {
        this.inspector = inspector;
        this.character = character;
        this.door = door;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
           new SequenceParallel(
               new Sequence(inspector.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>()),
                   inspector.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position)),
               new Sequence(character.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>()),
                   character.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position))
               ),
               inspector.Node_Interact(character, "BOW"),
               inspector.Node_Interact(door, "OPENDOOR"),
               inspector.Inspect(10000),
               inspector.Node_Interact(character, "SHAKEHAND"),
               new LeafWait(2000)
               );
    }
}

[LibraryIndexAttribute(6)]
public class _CCInspectHouse2 : SmartEvent
{
    SmartDoor door;
    SmartCharacterCC inspector;
    SmartCharacterCC character;
    SmartCharacterCC character2;
    SmartWaypointArea area;

    [Name("Inspect House 2")]
    [StateRequired(0, StateName.RoleDoor)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleWaypoint)]
    public _CCInspectHouse2(SmartDoor door, SmartCharacterCC inspector, SmartCharacterCC character, SmartCharacterCC character2, SmartWaypointArea area)
        : base(door, inspector, character, character2, area)
    {
        this.inspector = inspector;
        this.character = character;
        this.character2 = character2;
        this.door = door;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            new SequenceParallel(
                new Sequence(inspector.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>()),
                    inspector.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position)),
                new Sequence(character.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>()),
                    character.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position)),
                new Sequence(character2.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>()),
                    character2.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position))
                ),
                inspector.Node_Interact(character, "BOW"),
                inspector.Node_Interact(character2, "BOW"),
                inspector.Node_Interact(door, "OPENDOOR"),
                inspector.Inspect(10000),
                inspector.Node_Interact(character, "SHAKEHAND"),
                inspector.Node_Interact(character2, "SHAKEHAND"),
                new LeafWait(2000)
                );
    }
}

[LibraryIndexAttribute(6)]
public class _CCGoToOrigin : SmartEvent
{
    SmartCharacterCC character;
    SmartWaypoint waypoint;
    Transform lookAt;

    [Name("GoBack To Origin")]
    [StateRequired(0, StateName.RoleActor)]
    public _CCGoToOrigin(SmartCharacterCC character)
        : base(character)
    {

        this.character = character;
        this.waypoint = character.gameObject.GetComponent<CharacterOrigin>().getOrigin().gameObject.GetComponent<SmartWaypoint>();
        this.lookAt = character.gameObject.GetComponent<CharacterOrigin>().getLookAt();
    }

    public override Node BakeTree(Token token)
    {

        return
            new Race(new Sequence(character.Node_GoToX(waypoint),
                character.Node_OrientTowards(lookAt.transform.position)),
                new LeafWait(2000))

            ;
    }
}

[LibraryIndexAttribute(6)]
public class _CCGoToOrigin2 : SmartEvent
{
    SmartCharacterCC character;
    SmartWaypoint waypoint;
    Transform lookAt;

    [Name("GoBack To Origin 2")]
    [StateRequired(0, StateName.RoleActor)]
    public _CCGoToOrigin2(SmartCharacterCC character)
        : base(character)
    {

        this.character = character;
        this.waypoint = character.gameObject.GetComponent<CharacterOrigin>().getOrigin().gameObject.GetComponent<SmartWaypoint>();
        this.lookAt = character.gameObject.GetComponent<CharacterOrigin>().getLookAt();
    }

    public override Node BakeTree(Token token)
    {

        return
            new Sequence(character.Node_GoToX(waypoint),
                character.Node_OrientTowards(lookAt.transform.position));
    }
}

//Activate Palace
[LibraryIndexAttribute(6)]
public class _CCPalaceActivate : SmartEvent
{
    SmartCharacterCC character;
    Palace palace;

    [Name("Palace Activate")]
    [StateRequired(0, StateName.RoleActor)]
    public _CCPalaceActivate(SmartCharacterCC character)
        : base(character)
    {

        this.character = character;

        this.palace = character.GetComponent<Palace>();
    }

    public override Node BakeTree(Token token)
    {

        return new Selector(

                new LeafWait(4000),

            new LeafInvoke(() => palace.SetActivePalace(true))
            );
    }
}

//Display text for story
[LibraryIndexAttribute(6)]
public class _CCDisplayText : SmartEvent
{
    SmartStoryText text;

    [Name("DisplayText")]
    public _CCDisplayText(SmartStoryText storytext)
        : base(storytext)
    {

        this.text = storytext;
    }

    public override Node BakeTree(Token token)
    {

        return new Sequence(
            text.Node_DisplayText());
    }
}

[LibraryIndexAttribute(6)]
public class _CCLogName : SmartEvent
{
    SmartObject obj;

    [Name("Log Nname")]
    public _CCLogName(SmartObject obj)
        : base(obj)
    {

        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {

        return new LeafInvoke(() => Debug.Log("Hello, my name is " + this.obj.name));
    }
}

[LibraryIndexAttribute(6)]
public class _CCParticleTest : SmartEvent
{
    SmartMiscObjects obj;

    [Name("ParticleEnable 0")]
    public _CCParticleTest(SmartMiscObjects obj)
        : base(obj)
    {

        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            this.obj.Node_SetParticle(0, true),
            new LeafWait(3000),
            this.obj.Node_SetParticle(0, false));
    }
}

[LibraryIndexAttribute(6)]
public class _CCEnableObjectWithParticle : SmartEvent
{
    SmartMiscObjects obj;

    [Name("Object Enable")]
    public _CCEnableObjectWithParticle(SmartMiscObjects obj)
        : base(obj)
    {

        this.obj = obj;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            new LeafWait(3000),
            this.obj.Node_SetParticle(0, true),
            new LeafWait(2000),
            this.obj.Node_SetObject(0, true),
            new LeafWait(2000),
            this.obj.Node_SetParticle(0, false));
    }
}
#endregion

#region STORY_OF_AMIR
[LibraryIndexAttribute(6)]
public class _ReturnToFamily : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartWaypointArea area;

    [Name("Return to Family")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    public _ReturnToFamily(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartWaypointArea area)
        : base(char1, char2, char3, area)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
                new SequenceParallel(
                    char1.Node_GoToX(area.Waypoints[0].gameObject.GetComponent<SmartWaypoint>()),


                    new Sequence(
                        new LeafWait(7000),
                        char2.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>()),
                        char2.Node_OrientTowards(area.Waypoints[0].transform.position)),
                    new Sequence(
                        new LeafWait(9000),
                        char3.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>()),
                        char3.Node_OrientTowards(area.Waypoints[0].transform.position))
                    ),
                new SequenceParallel(
                    char1.Node_OrientTowards(Val.V(() => char2.transform.position)),
                    char2.Node_OrientTowards(Val.V(() => char1.transform.position)),
                    char3.Node_OrientTowards(Val.V(() => char1.transform.position))
                    ),
                char1.Node_PerformAt(area.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), "CheerHappily"),
                new SequenceParallel(
                    char2.Node_PerformAt(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "CheerHappily"),
                    char3.Node_PerformAt(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "CheerHappily")
                    )
            );
    }
}

[LibraryIndexAttribute(6)]
public class _AccuseFamily : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC survivor;
    SmartWaypointArea area;

    [Name("Accuse Family")]
    public _AccuseFamily(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC survivor, SmartWaypointArea area)
        : base(char1, char2, survivor, area)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.survivor = survivor;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            new SequenceParallel(
                char1.Node_GoToX(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>()),
                char2.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>())
                ),
            new SequenceParallel(
                char1.Node_OrientTowards(survivor.gameObject.transform.position),
                char2.Node_OrientTowards(survivor.gameObject.transform.position)
                ),
                char1.Node_Perform("Threaten"),
                char2.Node_Perform("FistShake")
            );
    }
}

[LibraryIndexAttribute(6)]
public class FamilyReplyDefensive : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartWaypointArea family;

    [Name("Family Response Defensive")]
    public FamilyReplyDefensive(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartWaypointArea family)
        : base(char1, char2, char3, family)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.family = family;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(

            new SequenceParallel(
                char1.Node_OrientTowards(family.Waypoints[0].gameObject.transform.position),
                char2.Node_OrientTowards(family.Waypoints[0].gameObject.transform.position),
                char3.Node_OrientTowards(family.Waypoints[0].gameObject.transform.position)
                ),
                char1.Node_Perform("FistShake"),
                char2.Node_Perform("YellAngrily"),
                char3.Node_Perform("FistShake")

            );
    }
}

[LibraryIndexAttribute(6)]
public class FamilyReplyHostile : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartWaypointArea family;

    [Name("Family Response Hostile")]
    public FamilyReplyHostile(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartWaypointArea family)
        : base(char1, char2, char3, family)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.family = family;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(

            new SequenceParallel(
                char1.Node_OrientTowards(family.Waypoints[0].gameObject.transform.position),
                char2.Node_OrientTowards(family.Waypoints[0].gameObject.transform.position),
                char3.Node_OrientTowards(family.Waypoints[0].gameObject.transform.position)
                ),
                char1.Node_Perform("CheerHappily"),
                char2.Node_Perform("FistShake")
            );
    }
}

[LibraryIndexAttribute(6)]
public class FamilyReplyMourn : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartWaypointArea family;
    SmartWaypointArea family2;


    [Name("Family Response Mourn")]
    public FamilyReplyMourn(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartWaypointArea family, SmartWaypointArea family2)
        : base(char1, char2, char3, family, family2)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.family = family;
        this.family2 = family2;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(

            new SequenceParallel(
                char1.Node_OrientTowards(family.Waypoints[0].gameObject.transform.position),
                char2.Node_OrientTowards(family2.Waypoints[0].gameObject.transform.position),
                char3.Node_OrientTowards(family2.Waypoints[0].gameObject.transform.position)
                ),
                char1.Node_Perform("Mourn"),
                new LeafWait(3000),
                char2.Node_Perform("Mourn"),
                new LeafWait(2000)
            );
    }
}

[LibraryIndexAttribute(6)]
public class PlotMurder : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;

    [Name("Plot Murder")]
    public PlotMurder(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4)
        : base(char1, char2, char3, char4)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            new SequenceParallel(
                
                    char4.Node_Perform("mourn"),
                    char1.Node_Perform(char4, "threaten"),
                
                    char3.Node_Perform("mourn"),
                    char2.Node_Perform(char3, "threaten")
                    
                ),
            new SequenceParallel(
                char1.Node_Interact(char4, "bow"),
                char2.Node_Interact(char3, "ShakeHand")
                )
            );
    }
}

[LibraryIndexAttribute(6)]
public class AngryProtest : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;

    [Name("Angry Protest")]
    public AngryProtest(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4)
        : base(char1, char2, char3, char4)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(

                char1.Node_Perform("FistShake"),
                char3.Node_Perform("Threaten"),
                char4.Node_Perform("Mourn"),
                char2.Node_Perform("YellAngrily")
            );
    }
}

[LibraryIndexAttribute(6)]
public class ResentAccusing : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;

    [Name("Resent Accusing")]
    public ResentAccusing(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4)
        : base(char1, char2, char3, char4)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(

            new SequenceParallel(
                    char3.Node_Perform("mourn"),
                    char1.Node_Perform(char4, "mourn"),
                
                    char4.Node_Perform("mourn"),
                    char2.Node_Perform(char3, "mourn")
                ),
                char1.Node_Interact(char4, "bow"),
                char2.Node_Interact(char3, "bow")
            );
    }
}

[LibraryIndexAttribute(6)]
public class DemandSearch : SmartEvent
{
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;

    [Name("Demand Search")]
    public DemandSearch(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5)
        : base(char1, char2, char3, char4, char5)
    {
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
                new SequenceParallel(
                    char1.Node_Perform(char3, "FistShake"),
                    char2.Node_Perform(char4, "FistShake")
                    ),
                    new LeafWait(1500),
                     new SequenceParallel(
                    char3.Node_Perform("FistShake"),
                    char4.Node_Perform("FistShake"), 
                    char5.Node_Perform("FistShake")
                    )
            );
    }
}

[LibraryIndexAttribute(6)]
public class _GoToWildernessAndDie : SmartEvent
{
    SmartCharacterCC char1;
    SmartWaypointArea area;

    [Name("Go To Wilderness and Die")]
    public _GoToWildernessAndDie(SmartCharacterCC char1, SmartWaypointArea area)
        : base(char1, area)
    {
        this.char1 = char1;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
                char1.Node_Perform("fistshake"),
                char1.Node_GoToX(area.Waypoints[0].gameObject.GetComponent<SmartWaypoint>()),
                char1.Node_Perform("DIE")

            );
    }
}

//Dissapear instead of displaying a death animation (Not the best way to handle)
//TODO: Add "Dead" state to SmartCharacter
[LibraryIndexAttribute(6)]
public class _Die : SmartEvent
{
    SmartCharacterCC char1;

    [Name("Die")]
    public _Die(SmartCharacterCC char1)
        : base(char1)
    {
        this.char1 = char1;
    }

    public override Node BakeTree(Token token)
    {
        return new Race(
            
            char1.Node_Perform("DIE"),
            new Sequence(new LeafWait(3500), new LeafAssert(() => true))
            );
    }
}

//Dissapear instead of displaying a death animation (Not the best way to handle)
//TODO: Add "Dead" state to SmartCharacter
[LibraryIndexAttribute(6)]
public class _Die2 : SmartEvent
{
    SmartCharacterCC char1;

    [Name("Die 2")]
    public _Die2(SmartCharacterCC char1)
        : base(char1)
    {
        this.char1 = char1;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            new LeafInvoke(() => char1.Controllable = false),
            new Selector(
                        new Sequence(
                            new LeafAssert(() => char1.particles != null),
                            new LeafInvoke(() => char1.particles.enableEmission = true)
                            ),
                        new LeafAssert(() => true)
                        ),
                        new LeafWait(2000),
                        new LeafInvoke(() => char1.gameObject.SetActive(false))

            );
    }
}

//The character performs "Mourn"
[LibraryIndexAttribute(6)]
public class Mourn : SmartEvent
{
    SmartCharacterCC char1;

    [Name("Mourn")]
    public Mourn(SmartCharacterCC char1)
        : base(char1)
    {
        this.char1 = char1;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
                    char1.Node_Perform("Mourn")

            );
    }
}

//Wait for a few seconds
[LibraryIndexAttribute(6)]
public class _Wait: SmartEvent
{
    [Name("Wait")]
    public _Wait()
        : base()
    {
    }

    public override Node BakeTree(Token token)
    {
        return new LeafWait(2000);
    }
}

[LibraryIndexAttribute(6)]
public class GoToOrient: SmartEvent
{
    SmartCharacterCC character;
    SmartWaypoint waypoint;
    SmartWaypoint waypoint2;

    [Name("GoTo and Orient")]
    public GoToOrient(SmartCharacterCC character, SmartWaypoint waypoint, SmartWaypoint waypoint2)
        : base(character, waypoint, waypoint2)
    {
        this.character = character;
        this.waypoint = waypoint;
        this.waypoint2 = waypoint2;
    }

    public override Node BakeTree(Token token)
    {
        return character.Node_GoToX(waypoint, Val.V(() => waypoint2.gameObject.transform.position));
    }
}

[LibraryIndexAttribute(6)]
public class _Test : SmartEvent
{
    SmartCharacterCC char1;

    [Name("test")]
    public _Test(SmartCharacterCC char1)
        : base(char1)
    {
        this.char1 = char1;
    }

    public override Node BakeTree(Token token)
    {
        return new DecoratorLoop(
            new Selector(
                new Sequence(
                    new LeafAssert(() => !char1.Controlled), 
                    new LeafInvoke(() => Debug.Log("Character was not selected"))
                    ),
                new Sequence(
                    new DecoratorLoop(
                        new Sequence(
                            new LeafAssert(() => char1.Controlled == true),
                            new LeafInvoke(() => Debug.Log("Character is currently controlled")),
                            new Selector(
                                new Sequence(
                                    new LeafAssert(() => char1.status == SmartCharacterCC.gesture_map_id["BOW"]),
                                    new LeafInvoke(() => Debug.Log("Character has selected Bow"))
                                    ),
                                new Sequence(
                                    new LeafAssert(() => char1.status == SmartCharacterCC.gesture_map_id["FISTSHAKE"]),
                                    new LeafInvoke(() => Debug.Log("Character has selected fistshake"))
                                    )
                                )
                            )
                        )
            
                ),
           new LeafAssert(() => true)

           )
       );
    }
}

[LibraryIndexAttribute(6)]
public class Story : SmartEvent
{
    SmartCharacterCC char0;
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;
    SmartCharacterCC char6;
    SmartCharacterCC char7;
    SmartCharacterCC char8;
    SmartWaypointArea area;
    SmartWaypointArea wild;
    SmartWaypointArea angryarea;
    SmartWaypointArea circle;

    [Name("Story")]
    public Story(SmartCharacterCC char0, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4
        , SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8, SmartWaypointArea area, SmartWaypointArea wild, SmartWaypointArea angryarea, SmartWaypointArea circle)
        : base(char0, char1, char2, char3, char4, char5, char6, char7, char8, area, wild, angryarea, circle)
    {
        this.char0 = char0;
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
        this.char6 = char6;
        this.char7 = char7;
        this.char8 = char8;
        this.area = area;
        this.wild = wild;
        this.angryarea = angryarea;
        this.circle = circle;
    }

    public override Node BakeTree(Token token)
    {

        return new Sequence(
            new LeafInvoke(() => SmartCharacterCC.done = false),
            new SelectorParallel(
                new LeafAssert(() => SmartCharacterCC.done),



                new Sequence(
            new SequenceParallel(
                _ReusableActions.GoToOrigin2(char0),
                _ReusableActions.GoToOrigin2(char1),
                _ReusableActions.GoToOrigin2(char2),
                new LeafWait(2000)
                ),
            new SequenceParallel(

                    _ReusableActions.GoToOrient(char0, wild.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[3].GetComponent<SmartWaypoint>(), "Hello, " + char0.gameObject.name + ", I believe your friends have gone off into the wilderness and are waiting for you."),
                    _ReusableActions.GoToOrient(char1, wild.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[3].GetComponent<SmartWaypoint>(), "Hello, " + char1.gameObject.name + ", I believe your friends have gone off into the wilderness and are waiting for you."),
                    _ReusableActions.GoToOrient(char2, wild.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[3].GetComponent<SmartWaypoint>(), "Hello, " + char2.gameObject.name + ", I believe your friends have gone off into the wilderness and are waiting for you."),

                    _ReusableActions.GoToOrigin2(char3, "Good Afternoon. Your son is coming back from an adventure. Go see his return."),
                    _ReusableActions.GoToOrigin2(char4, "Good Afternoon. Your brother is coming back from an adventure. Go see his return."),
                    _ReusableActions.GoToOrigin2(char5, "Good Afternoon. Your son is coming back from an adventure. Go see his return."),
                    _ReusableActions.GoToOrigin2(char6, "Good Afternoon. Your brother is coming back from an adventure. Go see his return."),
                    _ReusableActions.GoToOrigin2(char7, "Good Afternoon. Your son is coming back from an adventure. Go see his return."),
                    _ReusableActions.GoToOrigin2(char8, "Good Afternoon. Your brother is coming back from an adventure. Go see his return.")
                ),
            new SequenceParallel(

                   _ReusableActions._Die2(char1),
                   _ReusableActions._Die2(char2)
                ),
            _ReusableActions._ReturnToFamily(char0, char3, char4, area),

            new DecoratorLoop(
                new Selector(
                    new Sequence(
                        new LeafAssert(() => !char5.Controlled && !char6.Controlled),
            //Use randomselector to choose the next storyline
                        new LeafInvoke(() => Debug.Log("Caleb Family not controlled")),
                        new SelectorShuffle(
                            _ReusableActions.Mourning(char0, char3, char4, char5, char6, char7, char8, circle),
            //THE STORY ENDS HERE WITH EVERYONE MOURNING
                            new Sequence(
                                _ReusableActions._AccuseFamily(char5, char6, char0, angryarea),
                                _ReusableActions.Sequence2(char0, char1, char2, char3, char4, char5, char6, char7, char8, area, angryarea, wild, circle)
                                )
                            )
                        ),
                    new Sequence(
                        new DecoratorLoop(
                            new Selector(
                                new Sequence(
                                    new LeafAssert(() => char5.Controlled || char6.Controlled),
                                    new LeafInvoke(() => Debug.Log("Caleb Family Controlled")),
                                    new Selector(
                                        new Sequence(
                                            new LeafAssert(() => char5.status == SmartCharacterCC.gesture_map_id["FISTSHAKE"] || char6.status == SmartCharacterCC.gesture_map_id["FISTSHAKE"] || char5.status == SmartCharacterCC.gesture_map_id["THREATEN"] || char6.status == SmartCharacterCC.gesture_map_id["THREATEN"]),
                                            new LeafInvoke(() => Debug.Log("Player has chosen hostile family")),
                                            _ReusableActions._AccuseFamily(char5, char6, char0, angryarea),
                                             _ReusableActions.Sequence2(char0, char1, char2, char3, char4, char5, char6, char7, char8, area, angryarea, wild, circle)
                                            ),
                                        new Sequence(
                                            new LeafAssert(() => char5.status == SmartCharacterCC.gesture_map_id["MOURN"] || char6.status == SmartCharacterCC.gesture_map_id["MOURN"] || char5.status == SmartCharacterCC.gesture_map_id["CHEERHAPPILY"] || char6.status == SmartCharacterCC.gesture_map_id["CHEERHAPPILY"]),
                                            new LeafInvoke(() => Debug.Log("Character has chosen to mourn")),
            //Caleb Family !hates Benjamin Family
                                            _ReusableActions.Mourning(char0, char3, char4, char5, char6, char7, char8, circle)
                                            ),
                                            new Sequence(
                                                    new LeafAssert(() => char5.Controlled),
                                                    new LeafAssert(() => char5.helpable),
                                                    new LeafInvoke(() => Debug.Log("Helpable")),
                                                    new LeafInvoke(() => char5.startTimer()),
                                                    new LeafInvoke(() => Debug.Log("Timer start")),
                                                    new LeafAssert(() => char5.checkTimer()),
                                                    new LeafInvoke(() => Debug.Log("timer check")),
                                                    new LeafInvoke(() => char5.SmartCrowdAssist("Although your son's friend has come back, your son has not returned from the wilderness! How do you feel about this?")),
                                                    new LeafInvoke(() => Debug.Log("invoked alternative behavior")),
                                                    new LeafInvoke(() => char5.helpable = false)
                                                    ),
                                                    new Sequence(
                                                    new LeafAssert(() => char6.Controlled),
                                                    new LeafAssert(() => char6.helpable),
                                                    new LeafInvoke(() => Debug.Log("Helpable")),
                                                    new LeafInvoke(() => char6.startTimer()),
                                                    new LeafInvoke(() => Debug.Log("Timer start")),
                                                    new LeafAssert(() => char6.checkTimer()),
                                                    new LeafInvoke(() => Debug.Log("timer check")),
                                                    new LeafInvoke(() => char6.SmartCrowdAssist("Your brother has not returned from the wilderness but his friend has! How do you feel about this?")),
                                                    new LeafInvoke(() => Debug.Log("invoked alternative behavior")),
                                                    new LeafInvoke(() => char6.helpable = false)
                                                    )
                                        )
                                    )
                                )
                            )

                    ),
               new LeafAssert(() => true)

               )
           )
            )
                ),
                new LeafInvoke(() => Debug.Log("Story done"))
            
            );
    }
}


[LibraryIndexAttribute(6)]
public class _StoryText : SmartEvent
{
    SmartCharacterCC char0;
    SmartCharacterCC char1;
    SmartCharacterCC char2;
    SmartCharacterCC char3;
    SmartCharacterCC char4;
    SmartCharacterCC char5;
    SmartCharacterCC char6;
    SmartCharacterCC char7;
    SmartCharacterCC char8;
    SmartWaypointArea area;
    SmartWaypointArea wild;
    SmartWaypointArea angryarea;
    SmartWaypointArea circle;
    SmartStoryTextList list;

    [Name("_StoryText")]
    public _StoryText(SmartCharacterCC char0, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4
        , SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8, SmartWaypointArea area, SmartWaypointArea wild, SmartWaypointArea angryarea, SmartWaypointArea circle, SmartStoryTextList list)
        : base(char0, char1, char2, char3, char4, char5, char6, char7, char8, area, wild, angryarea, circle, list)
    {
        this.char0 = char0;
        this.char1 = char1;
        this.char2 = char2;
        this.char3 = char3;
        this.char4 = char4;
        this.char5 = char5;
        this.char6 = char6;
        this.char7 = char7;
        this.char8 = char8;
        this.area = area;
        this.wild = wild;
        this.angryarea = angryarea;
        this.circle = circle;
        this.list = list;
    }

    public override Node BakeTree(Token token)
    {

        return new Sequence(
            new LeafInvoke(() => SmartCharacterCC.done = false),
            new SelectorParallel(
                new DecoratorLoop(new LeafAssert(() => SmartCharacterCC.done)),



                new Sequence(
            new SequenceParallel(
                _ReusableActions.GoToOrigin2(char0),
                _ReusableActions.GoToOrigin2(char1),
                _ReusableActions.GoToOrigin2(char2),
                list.Text[0].Node_DisplayText(),
                new LeafWait(2000)
                ),
            new SequenceParallel(

                    _ReusableActions.GoToOrient(char0, wild.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[3].GetComponent<SmartWaypoint>(), "Hello, " + char0.gameObject.name + ", I believe your friends have gone off into the wilderness and are waiting for you."),
                    _ReusableActions.GoToOrient(char1, wild.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[3].GetComponent<SmartWaypoint>(), "Hello, " + char1.gameObject.name + ", I believe your friends have gone off into the wilderness and are waiting for you."),
                    _ReusableActions.GoToOrient(char2, wild.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[3].GetComponent<SmartWaypoint>(), "Hello, " + char2.gameObject.name + ", I believe your friends have gone off into the wilderness and are waiting for you."),

                    _ReusableActions.GoToOrigin2(char3, "Good Afternoon. Your son is coming back from an adventure. Go see his return."),
                    _ReusableActions.GoToOrigin2(char4, "Good Afternoon. Your brother is coming back from an adventure. Go see his return."),
                    _ReusableActions.GoToOrigin2(char5, "Good Afternoon. Your son is coming back from an adventure. Go see his return."),
                    _ReusableActions.GoToOrigin2(char6, "Good Afternoon. Your brother is coming back from an adventure. Go see his return."),
                    _ReusableActions.GoToOrigin2(char7, "Good Afternoon. Your son is coming back from an adventure. Go see his return."),
                    _ReusableActions.GoToOrigin2(char8, "Good Afternoon. Your brother is coming back from an adventure. Go see his return."),
                    list.Text[1].Node_DisplayText()
                ),
            new SequenceParallel(

                   _ReusableActions._Die2(char1),
                   _ReusableActions._Die2(char2)
                ),
                    _ReusableActions._ReturnToFamily(char0, char3, char4, area, list),
                    list.Text[3].Node_DisplayText(),
                new DecoratorLoop(
                new Selector(
                    new Sequence(
                        new LeafAssert(() => !char5.Controlled && !char6.Controlled),
            //Use randomselector to choose the next storyline
                        new LeafInvoke(() => Debug.Log("Caleb Family not controlled")),
                        new SelectorShuffle(
                            //_ReusableActions.Mourning(char0, char3, char4, char5, char6, char7, char8, circle, list),
            //THE STORY ENDS HERE WITH EVERYONE MOURNING
                            new Sequence(
                                _ReusableActions._AccuseFamily(char5, char6, char0, angryarea, list),
                                _ReusableActions.Sequence2(char0, char1, char2, char3, char4, char5, char6, char7, char8, area, angryarea, wild, circle, list)
                                )
                            )
                        ),
                    new Sequence(
                        new DecoratorLoop(
                            new Selector(
                                new Sequence(
                                    new LeafAssert(() => char5.Controlled || char6.Controlled),
                                    new LeafInvoke(() => Debug.Log("Caleb Family Controlled")),
                                    new Selector(
                                        new Sequence(
                                            new LeafAssert(() => char5.status == SmartCharacterCC.gesture_map_id["FISTSHAKE"] || char6.status == SmartCharacterCC.gesture_map_id["FISTSHAKE"] || char5.status == SmartCharacterCC.gesture_map_id["THREATEN"] || char6.status == SmartCharacterCC.gesture_map_id["THREATEN"]),
                                            new LeafInvoke(() => Debug.Log("Player has chosen hostile family")),
                                            _ReusableActions._AccuseFamily(char5, char6, char0, angryarea, list),
                                             _ReusableActions.Sequence2(char0, char1, char2, char3, char4, char5, char6, char7, char8, area, angryarea, wild, circle, list)
                                            ),
                                        new Sequence(
                                            new LeafAssert(() => char5.status == SmartCharacterCC.gesture_map_id["MOURN"] || char6.status == SmartCharacterCC.gesture_map_id["MOURN"] || char5.status == SmartCharacterCC.gesture_map_id["CHEERHAPPILY"] || char6.status == SmartCharacterCC.gesture_map_id["CHEERHAPPILY"]),
                                            new LeafInvoke(() => Debug.Log("Character has chosen to mourn")),
            //Caleb Family !hates Benjamin Family
                                            _ReusableActions.Mourning(char0, char3, char4, char5, char6, char7, char8, circle, list)
                                            ),
                                            new Sequence(
                                                    new LeafAssert(() => char5.Controlled),
                                                    new LeafAssert(() => char5.helpable),
                                                    new LeafInvoke(() => Debug.Log("Helpable")),
                                                    new LeafInvoke(() => char5.startTimer()),
                                                    new LeafInvoke(() => Debug.Log("Timer start")),
                                                    new LeafAssert(() => char5.checkTimer()),
                                                    new LeafInvoke(() => Debug.Log("timer check")),
                                                    new LeafInvoke(() => char5.SmartCrowdAssist("Although your son's friend has come back, your son has not returned from the wilderness! How do you feel about this?")),
                                                    new LeafInvoke(() => Debug.Log("invoked alternative behavior")),
                                                    new LeafInvoke(() => char5.helpable = false)
                                                    ),
                                                    new Sequence(
                                                    new LeafAssert(() => char6.Controlled),
                                                    new LeafAssert(() => char6.helpable),
                                                    new LeafInvoke(() => Debug.Log("Helpable")),
                                                    new LeafInvoke(() => char6.startTimer()),
                                                    new LeafInvoke(() => Debug.Log("Timer start")),
                                                    new LeafAssert(() => char6.checkTimer()),
                                                    new LeafInvoke(() => Debug.Log("timer check")),
                                                    new LeafInvoke(() => char6.SmartCrowdAssist("Your brother has not returned from the wilderness but his friend has! How do you feel about this?")),
                                                    new LeafInvoke(() => Debug.Log("invoked alternative behavior")),
                                                    new LeafInvoke(() => char6.helpable = false)
                                                    )
                                        )
                                    )
                                )
                            )

                    ),
               new LeafAssert(() => true)

               )
           )
                
                
            )
                ),
                new LeafInvoke(() => Debug.Log("Story done"))

            );
    }
}
#endregion