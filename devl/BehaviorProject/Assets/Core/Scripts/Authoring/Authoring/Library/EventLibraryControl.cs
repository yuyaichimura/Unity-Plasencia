using TreeSharpPlus;
using System.Linq;
using UnityEngine;
using RootMotion.FinalIK;
using System;


[LibraryIndexAttribute(6)]
public class ControlledGoTo : SmartEvent
{
    SmartCharacterCC character;
    SmartWaypoint waypoint;

    [Name("Controlled Go To")]
    [StateRequired(0, StateName.RoleActor, StateName.IsStanding)]
    public ControlledGoTo(SmartCharacterCC character, SmartWaypoint waypoint)
        : base(character, waypoint)
    {
        this.character = character;
        this.waypoint = waypoint;
    }

    public override Node BakeTree(Token token)
    {
        return character.Node_GoTo(waypoint);
    }
}
