using TreeSharpPlus;
using UnityEngine;
using RootMotion.FinalIK;
using System.Linq;




/// <summary>
/// </summary>
[LibraryIndex(5)]
public class _Wander2 : GenericEvent<SmartCharacter>
{
    protected override Node Root(Token token, SmartCharacter character)
    {
        Wanderer wanderer = character.GetComponent<Wanderer>();
        if(wanderer == null){
            return null;
        }
        //return new LeafAffordance
       return _ReusableActions.ChooseRandomWaypointAndGo(character, wanderer.GetWanderingWaypoingArea());

    }

    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
   // [HideInGUI(3)]
    public _Wander2(SmartCharacter character)
        : base(character) { }
}
/// <summary>
/// Happy conversation between two characters.
/// </summary>
[LibraryIndex(6)]
public class TalkHappilyCrowd2 : GenericEvent<SmartCharacter, SmartCharacter>
{
    protected override Node Root(Token token, SmartCharacter char1, SmartCharacter char2)
    {
        return new LeafAffordance("TalkHappily", char1, char2);
    }

    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    [HideInGUI(3)]
    public TalkHappilyCrowd2(SmartCharacter char1, SmartCharacter char2)
        : base(char1, char2) { }
}

/// <summary>
/// Event where the crowd flees to a given point.
/// </summary>
[LibraryIndex(5)]
public class Wander3 : CrowdEvent<SmartCharacter>
{

    public override Node BakeParticipantTree(SmartCharacter character, object token)
    {
        Wanderer wanderer = character.GetComponent<Wanderer>();
       /* if (wanderer == null)
        {
            return null;
        }*/

        return _ReusableActions.ChooseRandomWaypointAndGo(character, wanderer.GetWanderingWaypoingArea());
       // return new LeafAffordance("GoTo", )
    }

    [StateRequired(0, StateName.RoleCrowd, StateName.IsStanding)]
    public Wander3(SmartCrowd crowd)
        : base(crowd.GetObjectsByState(StateName.RoleActor).Cast<SmartCharacter>())
    {
    }
}

/// <summary>
/// Normal conversation between two characters.
/// </summary>
[LibraryIndex(6)]
public class _TalkNormallyCrowd : GenericEvent<SmartCharacterCC, SmartCharacterCC>
{
    protected override Node Root(Token token, SmartCharacterCC char1, SmartCharacterCC char2)
    {
        return new Sequence(
            //new LeafInvoke(() => Debug.Log("Talking")),
            new SelectorParallel(
                    char1.Node_GoTo(Val.V(() => char2.transform.position)),
                    char2.Node_GoTo(Val.V(() => char1.transform.position))
                ),
                new SelectorParallel(
                    char1.Node_OrientTowards(Val.V(() => char2.transform.position)),
                    char2.Node_OrientTowards(Val.V(() => char1.transform.position))
                ),
                new LeafAffordance("Talk", char1, char2)
                
            );
    }

    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    [StateRequired(1, StateName.RoleActor, StateName.IsStanding)]
    // [HideInGUI(3)]
    public _TalkNormallyCrowd(SmartCharacterCC char1, SmartCharacterCC char2)
        : base(char1, char2) { }
}

/// <summary>
/// </summary>
[LibraryIndex(6)]
public class _WanderX1 : GenericEvent<SmartCharacterCC>
{
    SmartWaypointArea area;
    SmartWaypoint[] waypoints;

    protected override Node Root(Token token, SmartCharacterCC character)
    {
        return character.Node_GoTo(waypoints[Random.Range(0, waypoints.Length-1)].transform.position);
    }

    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public _WanderX1(SmartCharacterCC character)
        : base(character)
    {
        Wanderer wanderer = character.gameObject.GetComponent<Wanderer>();
        
        if (wanderer == null)
        {
            area = null;
        }
        else
        {
            this.area = wanderer.GetWanderingWaypoingArea();
            waypoints = wanderer.GetWaypoints();
        }

       // Debug.Log("Waypoint length " + waypoints.Length + " area length " + area.Waypoints.Length);
    }
}
