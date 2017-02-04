using RootMotion.FinalIK;
using System.Collections.Generic;
using TreeSharpPlus;
using UnityEngine;

public class SmartWaypointArea2 : SmartWaypointArea {

    [Affordance]
    protected Node GoToRandomArea(SmartCharacter character)
    {
        Debug.Log("Character - " + character.gameObject.name);
       
        return character
            .ST_StandAtWaypoint(
            this.Waypoints[Random.Range(0, this.Waypoints.Length)]
            .transform);
    }
}
