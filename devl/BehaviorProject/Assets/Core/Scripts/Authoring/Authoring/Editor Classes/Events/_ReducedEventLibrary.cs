using TreeSharpPlus;
using System.Linq;
using UnityEngine;
using RootMotion.FinalIK;

[LibraryIndexAttribute(5)]
public class _GoToAreaEvent1 : SmartEvent
{

    SmartCharacter character1;
    SmartCharacter character2;
    SmartCharacter character3;
    SmartCharacter character4;
    SmartCharacter character5;
    SmartCharacter character6;
    SmartCharacter character7;

    SmartWaypointArea area;

    [Name("Go To Area Event")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(7, StateName.RoleWaypoint)]
    public _GoToAreaEvent1(SmartCharacter character1, SmartCharacter character2, SmartCharacter character3, SmartCharacter character4, SmartCharacter character5, SmartCharacter character6, SmartCharacter character7, SmartWaypointArea area)
        : base(character1, character2, character3, character4, character5, character6, character7, area)
    {
        this.character1 = character1;
        this.character2 = character2;
        this.character3 = character3;
        this.character4 = character4;
        this.character5 = character5;
        this.character6 = character6;
        this.character7 = character7;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        //return Parallel(waypoint.Approach(character));
        return new Sequence(
            new LeafWait(2000),
            new SequenceParallel(
                new Sequence(_ReusableActions._WaitAndMoveToArea(character1, area.Waypoints[1]),
                    character1.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                new Sequence(_ReusableActions._WaitAndMoveToArea(character2, area.Waypoints[2]),
                    character2.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                new Sequence(_ReusableActions._WaitAndMoveToArea(character3, area.Waypoints[3]),
                    character3.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                new Sequence(_ReusableActions._WaitAndMoveToArea(character4, area.Waypoints[4]),
                    character4.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                new Sequence(_ReusableActions._WaitAndMoveToArea(character5, area.Waypoints[5]),
                    character5.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                new Sequence(_ReusableActions._WaitAndMoveToArea(character6, area.Waypoints[6]),
                    character6.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                new Sequence(_ReusableActions._WaitAndMoveToArea(character7, area.Waypoints[7]),
                    character7.Node_OrientTowards(Val.V(() => area.Waypoints[0].position)))        
            ),
            new LeafWait(2000)

            );
    }
}

[LibraryIndexAttribute(5)]
public class _GoToAreaEvent2 : SmartEvent
{

    SmartCharacter character1;
    SmartCharacter character2;
    SmartCharacter character3;
    SmartCharacter character4;
    SmartCharacter character5;
    SmartCharacter character6;

    SmartWaypointArea area;

    [Name("Go To Area Event 6")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleWaypoint)]
    public _GoToAreaEvent2(SmartCharacter character1, SmartCharacter character2, SmartCharacter character3, SmartCharacter character4, SmartCharacter character5, SmartCharacter character6, SmartWaypointArea area)
        : base(character1, character2, character3, character4, character5, character6, area)
    {
        this.character1 = character1;
        this.character2 = character2;
        this.character3 = character3;
        this.character4 = character4;
        this.character5 = character5;
        this.character6 = character6;
        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        //return Parallel(waypoint.Approach(character));
        return new Sequence(
            new LeafWait(2000),
            new SequenceParallel(
                new Sequence(_ReusableActions._WaitAndMoveToArea(character1, area.Waypoints[1]),
                    character1.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                new Sequence(_ReusableActions._WaitAndMoveToArea(character2, area.Waypoints[2]),
                    character2.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                new Sequence(_ReusableActions._WaitAndMoveToArea(character3, area.Waypoints[3]),
                    character3.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                new Sequence(_ReusableActions._WaitAndMoveToArea(character4, area.Waypoints[4]),
                    character4.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                new Sequence(_ReusableActions._WaitAndMoveToArea(character5, area.Waypoints[5]),
                    character5.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                new Sequence(_ReusableActions._WaitAndMoveToArea(character6, area.Waypoints[6]),
                    character6.Node_OrientTowards(Val.V(() => area.Waypoints[0].position)))
            ),
            new LeafWait(2000)

            );
    }
}

[LibraryIndexAttribute(5)]
public class _GoToAreaEvent3 : SmartEvent
{

    SmartCharacter character1;
    SmartCharacter character2;
    SmartCharacter character3;
    SmartCharacter character4;
    SmartCharacter character5;
    SmartCharacter character6;
    SmartCharacter character7;
    SmartCharacter character8;

    SmartWaypointArea area;

    [Name("Go To Area Event 8")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(7, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(8, StateName.RoleWaypoint)]
    public _GoToAreaEvent3(SmartCharacter character1, SmartCharacter character2, SmartCharacter character3, SmartCharacter character4, SmartCharacter character5, SmartCharacter character6, SmartCharacter character7, SmartCharacter character8, SmartWaypointArea area)
        : base(character1, character2, character3, character4, character5, character6, character7, character8, area)
    {
        this.character1 = character1;
        this.character2 = character2;
        this.character3 = character3;
        this.character4 = character4;
        this.character5 = character5;
        this.character6 = character6;
        this.character7 = character7;
        this.character8 = character8;

        this.area= area;
    }

    public override Node BakeTree(Token token)
    {
        //return Parallel(waypoint.Approach(character));
        return new Sequence(
               new LeafWait(2000),
               new SequenceParallel(
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character1, area.Waypoints[1]),
                       character1.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character2, area.Waypoints[2]),
                       character2.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character3, area.Waypoints[3]),
                       character3.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character4, area.Waypoints[4]),
                       character4.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character5, area.Waypoints[5]),
                       character5.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character6, area.Waypoints[6]),
                       character6.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character7, area.Waypoints[7]),
                       character7.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character8, area.Waypoints[8]),
                       character8.Node_OrientTowards(Val.V(() => area.Waypoints[0].position)))
               ),
               new LeafWait(2000)

               );
    }
}

[LibraryIndexAttribute(5)]
public class _GoToAreaEvent4 : SmartEvent
{

    SmartCharacter character1;
    SmartCharacter character2;
    SmartCharacter character3;
    SmartCharacter character4;
    SmartCharacter character5;
    SmartCharacter character6;
    SmartCharacter character7;
    SmartCharacter character8;
    SmartCharacter character9;
    SmartCharacter character10;


    SmartWaypointArea area;

    [Name("Go To Area Event 10")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(4, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(5, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(6, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(7, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(8, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(9, StateName.RoleActor, StateName.IsStanding)]

    [StateRequired(10, StateName.RoleWaypoint)]
    public _GoToAreaEvent4(SmartCharacter character1, SmartCharacter character2, SmartCharacter character3, SmartCharacter character4, SmartCharacter character5, SmartCharacter character6, SmartCharacter character7, SmartCharacter character8, SmartCharacter character9, SmartCharacter character10, SmartWaypointArea area)
        : base(character1, character2, character3, character4, character5, character6, character7, character8, character9, character10, area)
    {
        this.character1 = character1;
        this.character2 = character2;
        this.character3 = character3;
        this.character4 = character4;
        this.character5 = character5;
        this.character6 = character6;
        this.character7 = character7;
        this.character8 = character8;
        this.character9 = character9;
        this.character10 = character10;


        this.area = area;
    }

    public override Node BakeTree(Token token)
    {
        //return Parallel(waypoint.Approach(character));
        return new Sequence(
               new LeafWait(2000),
               new SequenceParallel(
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character1, area.Waypoints[1]),
                       character1.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character2, area.Waypoints[2]),
                       character2.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character3, area.Waypoints[3]),
                       character3.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character4, area.Waypoints[4]),
                       character4.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character5, area.Waypoints[5]),
                       character5.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character6, area.Waypoints[6]),
                       character6.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character7, area.Waypoints[7]),
                       character7.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character8, area.Waypoints[8]),
                       character8.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character9, area.Waypoints[9]),
                       character9.Node_OrientTowards(Val.V(() => area.Waypoints[0].position))),
                   new Sequence(_ReusableActions._WaitAndMoveToArea(character10, area.Waypoints[10]),
                       character10.Node_OrientTowards(Val.V(() => area.Waypoints[0].position)))
               ),
               new LeafWait(2000)

               );
    }
}

[LibraryIndexAttribute(5)]
public class _GoToEvent : SmartEvent
{
    SmartCharacter character;

    SmartWaypoint waypoint;

    [Name("Go To")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleWaypoint)]
    public _GoToEvent(SmartCharacter character, SmartWaypoint waypoint)
        : base(character, waypoint)
    {
        this.character = character;
        this.waypoint = waypoint;
    }

    public override Node BakeTree(Token token)
    {
        //return new SequenceParallel(waypoint.Approach(character));
        return new Sequence(character.Node_GoToUpToRadius(Val.V(() => waypoint.transform.position), 1.0f));
        //return waypoint.Approach(character);
    }
}

[LibraryIndexAttribute(5)]
public class _GoToAndConverse : SmartEvent
{
    SmartCharacter character1;
    SmartCharacter character2;


    [Name("Go To and Converse")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    public _GoToAndConverse(SmartCharacter character1, SmartCharacter character2)
        : base(character1, character2)
    {
        this.character1 = character1;
        this.character2 = character2;
    }

    public override Node BakeTree(Token token)
    {
        /*
        return new Sequence(
            character2.Node_OrientTowards(Val.V(() => character1.transform.position)),
            character1.Node_GoToUpToRadius(Val.V(() => character2.transform.position), 1.0f),
            new SequenceParallel(
                character1.Node_OrientTowards(Val.V(() => character2.transform.position)),
                character2.Node_OrientTowards(Val.V(() =>character1.transform.position))),
            new LeafAffordance("Talk", character1, character2));*/
        //return waypoint.Approach(character);
        /*
     new LeafAffordance("Talk", character1, character2)
          character1.Node_OrientTowards(Val.V(() => character2.transform.position)),
                character2.Node_OrientTowards(Val.V(() => character1.transform.position)),
                new LeafAffordance("Talk", character1, character2));*/
        //return new LeafAffordance("Talk", character1, character2);
        return new Sequence(character2.Node_OrientTowards(Val.V(() => character1.transform.position)),
            new LeafAffordance("Talk", character1, character2));
    }
}

[LibraryIndexAttribute(5)]
public class _GoToArea : SmartEvent
{
    SmartCharacter character;

    SmartWaypoint waypoint;
    float distance = 1.1f;

    [Name("Go To Area")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleWaypoint)]
    public _GoToArea(SmartCharacter character, SmartWaypoint waypoint)
        : base(character, waypoint)
    {
        this.character = character;
        this.waypoint = waypoint;
    }

    public override Node BakeTree(Token token)
    {
        //return Parallel(waypoint.Approach(character));
        return new Sequence(this.character.Node_GoToUpToRadius(Val.V(() => waypoint.transform.position), distance), this.character.Node_OrientTowards(waypoint.transform.position));
    }
}

[LibraryIndexAttribute(5)]
public class _Disappear : SmartEvent
{
    SmartCharacter character;

    SmartWaypoint waypoint;

    [Name("Disappear")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleWaypoint)]
    public _Disappear(SmartCharacter character, SmartWaypoint waypoint)
        : base(character, waypoint)
    {
        this.character = character;
        this.waypoint = waypoint;
    }

    public override Node BakeTree(Token token)
    {
        //return Parallel(waypoint.Approach(character));
        
        //return waypoint.Approach(character);
        return new Sequence(new LeafInvoke(() => character.gameObject.SetActive(false)));
    }
}

[LibraryIndexAttribute(5)]
public class _Reappear : SmartEvent
{
    SmartCharacter character;

    SmartWaypoint waypoint;

    [Name("Reappear")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleWaypoint)]
    public _Reappear(SmartCharacter character, SmartWaypoint waypoint)
        : base(character, waypoint)
    {
        this.character = character;
        this.waypoint = waypoint;
    }

    public override Node BakeTree(Token token)
    {
        //return Parallel(waypoint.Approach(character));

        //return waypoint.Approach(character);
        return new Sequence(new LeafInvoke(() => character.gameObject.SetActive(true)));
    }
}

[LibraryIndexAttribute(5)]
public class _EnterHouse: SmartEvent
{
    SmartCharacter character;

    SmartWaypoint waypoint;

    [Name("Enter House")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleWaypoint)]
    public _EnterHouse(SmartCharacter character, SmartWaypoint waypoint)
        : base(character, waypoint)
    {
        this.character = character;
        this.waypoint = waypoint;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(this.character.Node_OrientTowards(Val.V(() => this.waypoint.transform.position)),
            new LeafInvoke(() => character.gameObject.SetActive(false)));
    }
}

[LibraryIndexAttribute(5)]
public class _ExitHouse : SmartEvent
{
    SmartCharacter character;

    SmartWaypoint waypoint;

    [Name("Enter House")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleWaypoint)]
    public _ExitHouse(SmartCharacter character, SmartWaypoint waypoint)
        : base(character, waypoint)
    {
        this.character = character;
        this.waypoint = waypoint;
    }

    public override Node BakeTree(Token token)
    {
       // return new Sequence(this.character.Node_OrientTowards(Val.V(() => this.waypoint.transform.position)),
         //   new LeafInvoke(() => character.gameObject.SetActive(false)));
        return new LeafInvoke(() => character.gameObject.SetActive(true));
    }
}

[LibraryIndexAttribute(5)]
public class _GreetGroup: SmartEvent
{
    SmartCharacter character;

    SmartWaypoint waypoint;

    [Name("GreetGroup")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleWaypoint)]
    public _GreetGroup(SmartCharacter character, SmartWaypoint waypoint)
        : base(character, waypoint)
    {
        this.character = character;
        this.waypoint = waypoint;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            this.character.Node_OrientTowards(Val.V(() => this.waypoint.transform.position)),
            new LeafAffordance("UnlockDoorFront", character, null),
            new LeafInvoke(() => character.gameObject.SetActive(false)));
    }
}

[LibraryIndexAttribute(5)]
public class _InspectHouse : SmartEvent
{
    SmartCharacter inspector;
    SmartCharacter owner;
    SmartWaypointArea house;

    [Name("Inspect House")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleWaypoint)]
    public _InspectHouse(SmartCharacter inspector, SmartCharacter owner, SmartWaypointArea house)
        : base(inspector, owner, house)
    {
        Debug.Log("Constructor");

        this.inspector = inspector;
        this.owner = owner;
        this.house = house;
    }

    public override Node BakeTree(Token token)
    {

        return _ReusableActions._InspectHome(inspector, owner, house);
    }
}




[LibraryIndexAttribute(5)]
public class Wander : SmartEvent
{
    SmartCharacter character1;
    SmartCharacter character2;
    SmartCharacter character3;

    SmartWaypointArea waypointarea;

    [Name("Wander")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(2, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(3, StateName.RoleWaypoint)]
    public Wander(SmartCharacter character1, SmartCharacter character2, SmartCharacter character3, SmartWaypointArea waypointarea)
        : base(character1, character2, character3, waypointarea)
    {
        this.character1 = character1;
        this.character2 = character2;
        this.character3 = character3;

        this.waypointarea = waypointarea;
    }

    public override Node BakeTree(Token token)
    {
        return new DecoratorLoop(
            new SequenceParallel(
                _ReusableActions._WanderArea(character1, waypointarea),
                 _ReusableActions._WanderArea(character2, waypointarea),
                  _ReusableActions._WanderArea(character3, waypointarea)
                ));
    }
}

[LibraryIndexAttribute(5)]
public class _Converse : SmartEvent
{
    SmartCharacter character1;
    SmartCharacter character2;
    
    [Name("Converse")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    public _Converse(SmartCharacter character1, SmartCharacter character2)
        : base(character1, character2)
    {
        this.character1 = character1;
        this.character2 = character2;

    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            character1.Node_GoToUpToRadius(Val.V(() => character2.transform.position), 0.6f),
                character1.Node_OrientTowards(Val.V(() => character2.transform.position)),
                character2.Node_OrientTowards(Val.V(() => character1.transform.position)),
                new DecoratorLoop(new LeafAffordance("Talk", character1, character2))
            );
        
    }
}

[LibraryIndexAttribute(5)]
public class _Write : SmartEvent
{
    SmartCharacter character;

    [Name("Write")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _Write(SmartCharacter character)
        : base(character)
    {
        this.character = character;
    }

    public override Node BakeTree(Token token)
    {
        return character.Behavior.Node_BodyAnimation("write", true);

    }
}

[LibraryIndexAttribute(5)]
public class _GoToOrigin : SmartEvent
{

    SmartCharacter character;
    
    [Name("Character return to origin")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _GoToOrigin(SmartCharacter character)
        : base(character)
    {
        this.character = character;
    }

    public override Node BakeTree(Token token)
    {
        //return Parallel(waypoint.Approach(character));
        return _ReusableActions._ReturnToOrigin(character);
    }
}

[LibraryIndexAttribute(5)]
public class _DisablePalace : SmartEvent
{

    SmartCharacter character;

    [Name("DisablePalace")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _DisablePalace(SmartCharacter character)
        : base(character)
    {
        this.character = character;
    }

    public override Node BakeTree(Token token)
    {
        return _ReusableActions._DisablePalace(character);
    }
}

[LibraryIndexAttribute(5)]
public class _EnablePalace : SmartEvent
{

    SmartCharacter character;

    [Name("EnablePalace")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _EnablePalace(SmartCharacter character)
        : base(character)
    {
        this.character = character;
    }

    public override Node BakeTree(Token token)
    {
        return _ReusableActions._EnablePalace(character);
    }
}

[LibraryIndexAttribute(5)]
public class _EnterPalace : SmartEvent
{

    SmartCharacter character;
    SmartWaypoint waypoint;

    [Name("EnterPalace")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleWaypoint)]
    public _EnterPalace(SmartCharacter character, SmartWaypoint waypoint)
        : base(character)
    {
        this.character = character;
        this.waypoint = waypoint;
    }

    public override Node BakeTree(Token token)
    {
        return new Sequence(
            _ReusableActions._EnablePalace(character),
            waypoint.Approach(character),
            new LeafWait(3000),
            new LeafInvoke(() => character.gameObject.SetActive(false)));
    }
}


[LibraryIndexAttribute(5)]
public class _WriteNotary: SmartEvent
{
    SmartCharacter character;

    [Name("_WriteNotary")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _WriteNotary(SmartCharacter character)
        : base(character)
    {
        this.character = character;
    }

    public override Node BakeTree(Token token)
    {
        return this.character.Behavior.ST_PlayHandGesture("writing", 6000);
    }
}

