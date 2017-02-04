using UnityEngine;
using System.Collections;

public class Wanderer : MonoBehaviour {

    public SmartWaypointArea area;
    public SmartWaypoint[] waypoints;

    public SmartWaypointArea GetWanderingWaypoingArea()
    {
        return area;
    }

    public SmartWaypoint[] GetWaypoints()
    {
        return waypoints;
    }
}
