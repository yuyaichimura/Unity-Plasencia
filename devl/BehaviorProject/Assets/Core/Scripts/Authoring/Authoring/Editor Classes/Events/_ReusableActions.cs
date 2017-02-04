using UnityEngine;
using TreeSharpPlus;


public static class _ReusableActions
{

    /*
     * Character returns to the transform location attached to it in the CharacterOrigin script
     */
    public static Node _ReturnToOrigin(SmartCharacter character)
    {
        CharacterOrigin origin = character.GetComponent<CharacterOrigin>();
        return character.Node_GoTo(origin.getOrigin().position);

    }

    /*
     * Character waits from 1 to 3 seconds and move to specified location
     */
    public static Node _WaitAndMoveToArea(SmartCharacter character, Transform waypoint)
    {
        return new Sequence(
             new LeafWait(Random.Range(1000, 3000)),
            character.Node_GoTo(Val.V(() => waypoint.position))
           );
    }

    /*
     * An inspector and the house owner goes to the location of a house and inspects it.
     * TODO: Clean and organize sequence and code. Also add gestures such as handshakes.
     */
    public static Node _InspectHome(SmartCharacter inspector, SmartCharacter owner, SmartWaypointArea area)
    {
        if (area.Waypoints.Length < 3)
        {
            Debug.Log("Need more points");
            return null;
        }


        Transform home = area.Waypoints[0];
        Transform door = area.Waypoints[1];
        Transform exit = area.Waypoints[2];
        Transform inside = area.Waypoints[3];
        Transform ownerspot = area.Waypoints[4];

        return new Sequence(


                new SequenceParallel(
                    new Sequence(
                         inspector.Node_GoTo(Val.V(() => home.position)),
                         inspector.Node_OrientTowards(Val.V(() => ownerspot.position))),
                     new Sequence(
                        new LeafWait(1500),
                        owner.Node_GoTo(Val.V(() => ownerspot.position)),
                        owner.Node_OrientTowards(home.position)
                     )
                ),

                    inspector.Node_OrientTowards(Val.V(() => door.position)),
                    owner.Node_OrientTowards(Val.V(() => door.position)),
                new LeafWait(3000),
                new LeafInvoke(() => inspector.gameObject.SetActive(false)),
                new LeafWait(30000),
                new LeafInvoke(() => inspector.gameObject.SetActive(true)),
                inspector.Node_OrientTowards(Val.V(() => owner.transform.position)),
                owner.Node_OrientTowards(Val.V(() => inspector.transform.position))
            );
    }

    /*
     * A character wanders the points in the SmartWaypointArea
     */
    public static Node _WanderArea(SmartCharacter character, SmartWaypointArea area)
    {
        if (area.Waypoints.Length < 3)
        {
            return null;
        }

        return new SequenceShuffle(
                new Sequence(
                    character.Node_GoToUpToRadius(Val.V(() => area.Waypoints[0].position), 1.0f),
                new LeafWait(Random.Range(1000, 5000))),
                 new Sequence(
                    character.Node_GoToUpToRadius(Val.V(() => area.Waypoints[1].position), 1.0f),
                new LeafWait(Random.Range(1000, 5000))),
                 new Sequence(
                    character.Node_GoToUpToRadius(Val.V(() => area.Waypoints[2].position), 1.0f),
                new LeafWait(Random.Range(1000, 5000)))
            );
    }

    /*
     * The character disables the palace (or any 2 specified locations)
     */
    public static Node _DisablePalace(SmartCharacter character)
    {
        Palace palace = character.GetComponent<Palace>();

        if (palace == null)
        {
            return new LeafWait(3000);
        }
        return new Sequence(
                new LeafInvoke(() => palace.SetActivePalace(false)),
                character.Node_OrientTowards(palace.GetPalace1().position)
            );
    }

    /*
     * The character disables the palace (or any 2 specified locations)
     */
    public static Node _EnablePalace(SmartCharacter character)
    {
        Palace palace = character.GetComponent<Palace>();

        if (palace == null)
        {
            return new LeafWait(3000);
        }
        return new Sequence(
                new LeafInvoke(() => palace.SetActivePalace(true))
            );
    }

    public static Node ChooseRandomWaypointAndGo(SmartCharacter character, SmartWaypointArea area)
    {
        //        return character.Node_GoTo(area.Waypoints[Random.Range(0, area.Waypoints.Length)].position);
        return character.Node_GoTo(area.Waypoints[Random.Range(0, area.Waypoints.Length)].position);
        //return character.Node_GoToUpToRadius(area.Waypoints[Random.Range(0, area.Waypoints.Length)].position, Random.Range(0, 3.0f));
    }

    public static Node GoToOrigin2(SmartCharacterCC character)
    {

        SmartWaypoint waypoint = character.gameObject.GetComponent<CharacterOrigin>().getOrigin().gameObject.GetComponent<SmartWaypoint>();
        Transform lookAt = character.gameObject.GetComponent<CharacterOrigin>().getLookAt();

        return
              new Sequence(character.Node_GoToX(waypoint),
                  character.Node_OrientTowards(lookAt.transform.position));
    }

    public static Node GoToOrigin2(SmartCharacterCC character, string message)
    {

        SmartWaypoint waypoint = character.gameObject.GetComponent<CharacterOrigin>().getOrigin().gameObject.GetComponent<SmartWaypoint>();
        Transform lookAt = character.gameObject.GetComponent<CharacterOrigin>().getLookAt();

        return
              new Sequence(character.Node_GoToX(waypoint, message),
                  character.Node_OrientTowards(lookAt.transform.position));
    }

    public static Node GoToOrient(SmartCharacterCC character, SmartWaypoint waypoint, SmartWaypoint waypoint2, string message)
    {
        return character.Node_GoToX(waypoint, Val.V(() => waypoint2.gameObject.transform.position), message);
    }


    public static Node _Die2(SmartCharacterCC char1)
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

    public static Node _ReturnToFamily(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartWaypointArea area)
    {
        return new Sequence(
                new SequenceParallel(
                    new Sequence(
                        char1.Node_GoToX(area.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), "Go back to your family. They are waiting for you."),
                        char1.Node_OrientTowards(area.Waypoints[3].transform.position)
                        ),
                    new Sequence(
             new LeafWait(7000),
                        char2.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "Await your son's return"),
                        char2.Node_OrientTowards(area.Waypoints[0].transform.position)),
                    new Sequence(
            new LeafWait(9000),
                        char3.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "Await your brother's return"),
                        char3.Node_OrientTowards(area.Waypoints[0].transform.position))
                    ),
                char1.Node_PerformAt(area.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), "CheerHappily", "Wow, you are back! Your family was worried about you!"),
                new SequenceParallel(
                    char2.Node_PerformAt(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "CheerHappily", "Yay, your son is back! Where are his friends?"),
                    char3.Node_PerformAt(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "CheerHappily", "Yay, your brother is back! Where are his friends?")
                    )
            );
    }

    public static Node _ReturnToFamily(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartWaypointArea area, SmartStoryTextList list)
    {
        return new SequenceParallel(
            list.Text[2].Node_DisplayText(),
            new Sequence(
                new SequenceParallel(
                    new Sequence(
                        char1.Node_GoToX(area.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), "Go back to your family. They are waiting for you."),
                        char1.Node_OrientTowards(area.Waypoints[3].transform.position)
                        ),
                    new Sequence(
            new LeafWait(7000),
                        char2.Node_GoToX(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "Await your son's return"),
                        char2.Node_OrientTowards(area.Waypoints[0].transform.position)),
                    new Sequence(
            new LeafWait(9000),
                        char3.Node_GoToX(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "Await your brother's return"),
                        char3.Node_OrientTowards(area.Waypoints[0].transform.position))
                    ),
                char1.Node_PerformAt(area.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), "CheerHappily", "Wow, you are back! Your family was worried about you!"),
                new SequenceParallel(
                    char2.Node_PerformAt(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "CheerHappily", "Yay, your son is back! Where are his friends?"),
                    char3.Node_PerformAt(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "CheerHappily", "Yay, your brother is back! Where are his friends?")
                    )
            )
            
            );
    }

    public static Node Mourning(SmartCharacterCC char0, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartWaypointArea circle)
    {

        return
        new Sequence(
            new SequenceParallel(
            char0.Node_GoToX(circle.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char1.Node_GoToX(circle.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char2.Node_GoToX(circle.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char3.Node_GoToX(circle.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char4.Node_GoToX(circle.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char5.Node_GoToX(circle.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char6.Node_GoToX(circle.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss.")
        ),
            new SequenceParallel(
            char4.Node_PerformAt(circle.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
            char3.Node_PerformAt(circle.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new SequenceParallel(
                char0.Node_PerformAt(circle.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char6.Node_PerformAt(circle.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new SequenceParallel(
                char1.Node_PerformAt(circle.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char6.Node_PerformAt(circle.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new SequenceParallel(
                char2.Node_PerformAt(circle.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char5.Node_PerformAt(circle.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new LeafInvoke(() => SmartCharacterCC.done = true),
            new LeafInvoke(() => Debug.Log("Mourning end"))
        );
    }

    public static Node Mourning(SmartCharacterCC char0, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartWaypointArea circle, SmartStoryTextList list)
    {

        return
        new SequenceParallel(
            list.Text[10].Node_DisplayText(),
            new Sequence(
            new SequenceParallel(
            char0.Node_GoToX(circle.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char1.Node_GoToX(circle.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char2.Node_GoToX(circle.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char3.Node_GoToX(circle.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char4.Node_GoToX(circle.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char5.Node_GoToX(circle.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char6.Node_GoToX(circle.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss.")
        ),
            new SequenceParallel(
            char4.Node_PerformAt(circle.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
            char3.Node_PerformAt(circle.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new SequenceParallel(
                char0.Node_PerformAt(circle.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char6.Node_PerformAt(circle.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new SequenceParallel(
                char1.Node_PerformAt(circle.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char6.Node_PerformAt(circle.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new SequenceParallel(
                char2.Node_PerformAt(circle.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char5.Node_PerformAt(circle.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new LeafInvoke(() => SmartCharacterCC.done = true),
            new LeafInvoke(() => Debug.Log("Mourning end"))
        ));
    }

    public static Node Mourning2(SmartCharacterCC char0, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartWaypointArea circle, SmartStoryTextList list)
    {

        return
        new SequenceParallel(
            list.Text[15].Node_DisplayText(),
            new Sequence(
            new SequenceParallel(
            char0.Node_GoToX(circle.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char1.Node_GoToX(circle.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char2.Node_GoToX(circle.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char3.Node_GoToX(circle.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char4.Node_GoToX(circle.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char5.Node_GoToX(circle.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char6.Node_GoToX(circle.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss.")
        ),
            new SequenceParallel(
            char4.Node_PerformAt(circle.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
            char3.Node_PerformAt(circle.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new SequenceParallel(
                char0.Node_PerformAt(circle.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char6.Node_PerformAt(circle.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new SequenceParallel(
                char1.Node_PerformAt(circle.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char6.Node_PerformAt(circle.Waypoints[7].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new SequenceParallel(
                char2.Node_PerformAt(circle.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char5.Node_PerformAt(circle.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new LeafInvoke(() => SmartCharacterCC.done = true),
            new LeafInvoke(() => Debug.Log("Mourning end"))
        ));
    }

    public static Node Mourning(SmartCharacterCC char0, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartWaypointArea circle, SmartStoryTextList list)
    {

        return
        new SequenceParallel(
            list.Text[10].Node_DisplayText(),
        new Sequence(

            new SequenceParallel(
            char0.Node_GoToX(circle.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char1.Node_GoToX(circle.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char2.Node_GoToX(circle.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char3.Node_GoToX(circle.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char4.Node_GoToX(circle.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char5.Node_GoToX(circle.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss.")
        ),
            char4.Node_PerformAt(circle.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
            char3.Node_PerformAt(circle.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
            new SequenceParallel(
                char0.Node_PerformAt(circle.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char5.Node_PerformAt(circle.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new SequenceParallel(
                char1.Node_PerformAt(circle.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char2.Node_PerformAt(circle.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new LeafInvoke(() => SmartCharacterCC.done = true),
            new LeafInvoke(() => Debug.Log("Mourning end"))
            )
        );
    }

    public static Node Mourning(SmartCharacterCC char0, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartWaypointArea circle)
    {

        return
        new Sequence(

            new SequenceParallel(
            char0.Node_GoToX(circle.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char1.Node_GoToX(circle.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char2.Node_GoToX(circle.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char3.Node_GoToX(circle.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char4.Node_GoToX(circle.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss."),
            char5.Node_GoToX(circle.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), circle.Waypoints[0].gameObject.GetComponent<SmartWaypoint>().transform.position, "I am very sorry for the tragic loss.")
        ),
            char4.Node_PerformAt(circle.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
            char3.Node_PerformAt(circle.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
            new SequenceParallel(
                char0.Node_PerformAt(circle.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char5.Node_PerformAt(circle.Waypoints[6].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new SequenceParallel(
                char1.Node_PerformAt(circle.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss"),
                char2.Node_PerformAt(circle.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), "Mourn", "I am sorry for your loss")
            ),
            new LeafInvoke(() => SmartCharacterCC.done = true),
            new LeafInvoke(() => Debug.Log("Mourning end"))
        );
    }

    public static Node _AccuseFamily(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC survivor, SmartWaypointArea area)
    {
        return new Sequence(
            new SequenceParallel(
                char1.Node_GoToX(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), "What happened to your son? Was he murdered by " + survivor.gameObject.name + "? That fool!"),
                char2.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), "What happened to your brother? Was he murdered by " + survivor.gameObject.name + "? That fool!")
                ),
            new SequenceParallel(
                char1.Node_OrientTowards(survivor.gameObject.transform.position),
                char2.Node_OrientTowards(survivor.gameObject.transform.position)
                ),
                char1.Node_PerformAt(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "What happened to your son? Was he murdered by " + survivor.gameObject.name + "? That fool!"),
                char2.Node_PerformAt(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), "Threaten", "What happened to your brother? Was he murdered by " + survivor.gameObject.name + "? That fool!")
            );
    }

    public static Node _AccuseFamily(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC survivor, SmartWaypointArea area, SmartStoryTextList list)
    {
        return new SequenceParallel(
            list.Text[14].Node_DisplayText(),
            new Sequence(
            new SequenceParallel(
                char1.Node_GoToX(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), "What happened to your son? Was he murdered by " + survivor.gameObject.name + "? That fool!"),
                char2.Node_GoToX(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), "What happened to your brother? Was he murdered by " + survivor.gameObject.name + "? That fool!")
                ),
            new SequenceParallel(
                char1.Node_OrientTowards(survivor.gameObject.transform.position),
                char2.Node_OrientTowards(survivor.gameObject.transform.position)
                ),
                char1.Node_PerformAt(area.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "What happened to your son? Was he murdered by " + survivor.gameObject.name + "? That fool!"),
                char2.Node_PerformAt(area.Waypoints[3].gameObject.GetComponent<SmartWaypoint>(), "Threaten", "What happened to your brother? Was he murdered by " + survivor.gameObject.name + "? That fool!")
            ));
    }

    public static Node FamilyReplyDefensive(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartWaypointArea family, SmartWaypointArea area)
    {
        return new Sequence(

            new SequenceParallel(
                char1.Node_OrientTowards(family.Waypoints[3].gameObject.transform.position),
                char2.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position),
                char3.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position)
                ),
                /*char1.Node_PerformAt(area.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "The " + family.gameObject.name + " is insulting your family!"),
                char2.Node_PerformAt(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "YellAngrily", "The " + family.gameObject.name + " is insulting your family!"),
                char3.Node_PerformAt(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "The " + family.gameObject.name + " is insulting your family!")*/
                char1.Node_PerformAt(area.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "The Caleb Family is insulting your family!"),
                char2.Node_PerformAt(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "YellAngrily", "The Caleb Family is insulting your family!"),
                char3.Node_PerformAt(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "The Caleb Family is insulting your family!")
            );
    }

    public static Node FamilyReplyDefensive(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartWaypointArea family, SmartWaypointArea area, SmartStoryTextList list)
    {
        return 
            new SequenceParallel(
                list.Text[4].Node_DisplayText(),
            new Sequence(

            new SequenceParallel(
                char1.Node_OrientTowards(family.Waypoints[3].gameObject.transform.position),
                char2.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position),
                char3.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position)
                ),
            /*char1.Node_PerformAt(area.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "The " + family.gameObject.name + " is insulting your family!"),
char2.Node_PerformAt(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "YellAngrily", "The " + family.gameObject.name + " is insulting your family!"),
char3.Node_PerformAt(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "The " + family.gameObject.name + " is insulting your family!")*/
                char1.Node_PerformAt(area.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "The Caleb Family is insulting your family!"),
                char2.Node_PerformAt(area.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "YellAngrily", "The Caleb Family is insulting your family!"),
                char3.Node_PerformAt(area.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "The Caleb Family is insulting your family!")
                )
            );
    }

    public static Node AngryResponse(SmartCharacterCC char1, SmartCharacterCC char2, SmartWaypointArea area, SmartWaypointArea angryarea)
    {
        return new Sequence(
            new SequenceParallel(
                char1.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position),
                char2.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position)
                ),
                char1.Node_PerformAt(angryarea.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "Your dead family member has been insulted!"),
                char2.Node_PerformAt(angryarea.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "YellAngrily", "Your dead family member has been insulted!")
            );
    }

    public static Node AngryResponse(SmartCharacterCC char1, SmartCharacterCC char2, SmartWaypointArea area, SmartWaypointArea angryarea, SmartStoryTextList list)
    {
        return new SequenceParallel(
            list.Text[11].Node_DisplayText(),
            new Sequence(
            new SequenceParallel(
                char1.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position),
                char2.Node_OrientTowards(area.Waypoints[0].gameObject.transform.position)
                ),
                char1.Node_PerformAt(angryarea.Waypoints[1].gameObject.GetComponent<SmartWaypoint>(), "FistShake", "Your dead family member has been insulted!"),
                char2.Node_PerformAt(angryarea.Waypoints[2].gameObject.GetComponent<SmartWaypoint>(), "YellAngrily", "Your dead family member has been insulted!")
            ));
    }

    public static Node DescribeAnger(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4)
    {
        return new Sequence(
            new SequenceParallel(

                    char4.Node_PerformAt(char4.gameObject.GetComponent<CharacterOrigin>().getOrigin().gameObject.GetComponent<SmartWaypoint>(), "mourn", "I am sorry for your loss"),
                    char1.Node_PerformTo(char4, "threaten", "How do you respond to such reactions from the survivor's family?"),

                    char3.Node_PerformAt(char3.gameObject.GetComponent<CharacterOrigin>().getOrigin().gameObject.GetComponent<SmartWaypoint>(), "mourn", "I am sorry for your loss"),
                    char2.Node_PerformTo(char3, "threaten", "How do you respond to such reactions from the survivor's family?")

                ),
            new SequenceParallel(
                char1.Node_Interact(char4, "bow"),
                char2.Node_Interact(char3, "ShakeHand")
                )
            );
    }

    public static Node DescribeAnger(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartStoryTextList list)
    {
        return 
            new SequenceParallel(
                list.Text[5].Node_DisplayText(),
            new Sequence(
            new SequenceParallel(

                    char4.Node_PerformAt(char4.gameObject.GetComponent<CharacterOrigin>().getOrigin().gameObject.GetComponent<SmartWaypoint>(), "mourn", "I am sorry for your loss"),
                    char1.Node_PerformTo(char4, "threaten", "How do you respond to such reactions from the survivor's family?"),

                    char3.Node_PerformAt(char3.gameObject.GetComponent<CharacterOrigin>().getOrigin().gameObject.GetComponent<SmartWaypoint>(), "mourn", "I am sorry for your loss"),
                    char2.Node_PerformTo(char3, "threaten", "How do you respond to such reactions from the survivor's family?")

                ),
            new SequenceParallel(
                char1.Node_Interact(char4, "bow"),
                char2.Node_Interact(char3, "ShakeHand")
                )
 )           );
    }

    public static Node DemandSearch(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5)
    {
        return new Sequence(
                new SequenceParallel(
                    char1.Node_PerformTo(char4, "FistShake", "Whoa, what are you waiting for? Demand the family survivor to go into the wild!"),
                    char2.Node_PerformTo(char5, "FistShake", "Whoa, what are you waiting for? Demand the family survivor to go into the wild!")
                    ),
                    new LeafWait(1500),
                    new SequenceParallel(
                    char4.Node_PerformTo(char1, "FistShake", "There's no way you should go into the wilderness!"),
                    char5.Node_PerformTo(char2, "FistShake", "There's no way you should go into the wilderness!"),
                    char3.Node_PerformAt(char1, "FistShake", "They really want you go back into the dangerous wildernous?")
                    )
            );
    }

    public static Node DemandSearch(SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartStoryTextList list)
    {
        return new SequenceParallel(
            list.Text[6].Node_DisplayText(),
            new Sequence(
                new SequenceParallel(
                    char1.Node_PerformTo(char4, "FistShake", "Whoa, what are you waiting for? Demand the family survivor to go into the wild!"),
                    char2.Node_PerformTo(char5, "FistShake", "Whoa, what are you waiting for? Demand the family survivor to go into the wild!")
                    ),
                    new LeafWait(1500),
                    new SequenceParallel(
                    char4.Node_PerformTo(char1, "FistShake", "There's no way you should go into the wilderness!"),
                    char5.Node_PerformTo(char2, "FistShake", "There's no way you should go into the wilderness!"),
                    char3.Node_PerformAt(char1, "FistShake", "They really want you go back into the dangerous wildernous?")
                    ))
            );
    }

    public static Node WanderAndDisappear(SmartCharacterCC char0, SmartWaypointArea wild, string message)
    {
        return new Sequence(
            new LeafWait(2000),
            _ReusableActions.GoToOrient(char0, wild.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), message),
            new LeafWait(1000),
            char0.Node_PerformAt(wild.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), "FistShake", message),
            new LeafWait(2000),
            _ReusableActions.GoToOrient(char0, wild.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[3].GetComponent<SmartWaypoint>(), message),
            _ReusableActions._Die2(char0), new LeafInvoke(() => SmartCharacterCC.done = true)

            );
    }

    public static Node WanderAndDisappear(SmartCharacterCC char0, SmartWaypointArea wild, string message, SmartStoryTextList list)
    {
        return 
            new Sequence(
                new SequenceParallel(
                list.Text[12].Node_DisplayText(),
            new Sequence(
            new LeafWait(2000),
            _ReusableActions.GoToOrient(char0, wild.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), message),
            new LeafWait(1000),
            char0.Node_PerformAt(wild.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), "FistShake", message),
            new LeafWait(2000),
            _ReusableActions.GoToOrient(char0, wild.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[3].GetComponent<SmartWaypoint>(), message),
            _ReusableActions._Die2(char0), new LeafInvoke(() => SmartCharacterCC.done = true)
            ))
            , new LeafInvoke(() => SmartCharacterCC.done = true));
    }

    public static Node Murder(SmartCharacterCC murderer, SmartCharacterCC char0, SmartWaypointArea wild)
    {
        return new Sequence(
            new LeafWait(2000),
            _ReusableActions.GoToOrient(char0, wild.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), "You are going into the wilderness? Be careful!"),
            new LeafWait(1000),
            char0.Node_GoToX(wild.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), "You are going to look for your friends in the wild? Be careful!"),
            murderer.Node_PerformTo(char0, "hookright", "End him and get revenge!"),
            char0.Node_Perform("DIE"), new LeafInvoke(() => SmartCharacterCC.done = true), new LeafInvoke(() => SmartCharacterCC.done = true)
            );
    }
    public static Node Murder(SmartCharacterCC murderer, SmartCharacterCC char0, SmartWaypointArea wild, SmartStoryTextList list)
    {
        return 
            new SequenceParallel(
                list.Text[13].Node_DisplayText(),
                new Sequence(
            new LeafWait(2000),
            _ReusableActions.GoToOrient(char0, wild.Waypoints[4].gameObject.GetComponent<SmartWaypoint>(), wild.Waypoints[5].gameObject.GetComponent<SmartWaypoint>(), "You are going into the wilderness? Be careful!"),
            new LeafWait(1000),
            char0.Node_GoToX(wild.Waypoints[0].gameObject.GetComponent<SmartWaypoint>(), "You are going to look for your friends in the wild? Be careful!"),
            murderer.Node_PerformTo(char0, "hookright", "End him and get revenge!"),
            char0.Node_Perform("DIE"), new LeafInvoke(() => SmartCharacterCC.done = true)
            )
            );
    }

    public static bool seq3helper = true;
    public static Node Sequence3(SmartCharacterCC char0, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8, SmartWaypointArea area, SmartWaypointArea angryarea, SmartWaypointArea wild, SmartWaypointArea circle, SmartStoryTextList list)
    {
        return new Sequence(
            new LeafInvoke(() => Debug.Log("SEQUENCE3")),
            _ReusableActions.FamilyReplyDefensive(char0, char3, char4, area, angryarea, list),
            new LeafInvoke(() => Debug.Log("1")),
            // _ReusableActions.AngryResponse(char3, char4, area, angryarea),
            new LeafInvoke(() => Debug.Log("2")),
            
            new DecoratorInvert(
                new DecoratorLoop(
                        new Selector(
                            new Sequence(
                                new LeafAssert(() => !char5.Controlled && !char6.Controlled),
                                new LeafInvoke(() => Debug.Log("Caleb Family !controlled 2")),
                                new LeafInvoke(() => seq3helper = false),
                                new LeafAssert(() => false)
                                ),
                            new Sequence(
                                new LeafAssert(() => char5.Controlled || char6.Controlled),
                                new LeafInvoke(() => Debug.Log("Caleb Family controlled 2")),
                                new Selector(
                                    new Sequence(
                                         new LeafAssert(() => char5.status == SmartCharacterCC.gesture_map_id["FISTSHAKE"] || char6.status == SmartCharacterCC.gesture_map_id["FISTSHAKE"] || char5.status == SmartCharacterCC.gesture_map_id["THREATEN"] || char6.status == SmartCharacterCC.gesture_map_id["THREATEN"]),
                                        new LeafInvoke(() => seq3helper = false)
                                        ),
                                        new Sequence(
                                            new LeafAssert(() => char5.status == SmartCharacterCC.gesture_map_id["MOURN"] || char6.status == SmartCharacterCC.gesture_map_id["MOURN"] || char5.status == SmartCharacterCC.gesture_map_id["CHEERHAPPILY"] || char6.status == SmartCharacterCC.gesture_map_id["CHEERHAPPILY"]),
                                            _ReusableActions.Mourning2(char0, char3, char4, char5, char6, char7, char8, circle, list),
                                            new LeafInvoke(() => SmartCharacterCC.done = true)
                                            
                                            
                                            ),
                                            new LeafAssert(() => true)
                                    ),
                                    new LeafAssert(() => seq3helper)
                                ),
                                new Sequence(
                                                    new LeafAssert(() => char5.Controlled),
                                                    new LeafAssert(() => char5.helpable),
                                                    new LeafInvoke(() => Debug.Log("Helpable")),
                                                    new LeafInvoke(() => char5.startTimer()),
                                                    new LeafInvoke(() => Debug.Log("Timer start")),
                                                    new LeafAssert(() => char5.checkTimer()),
                                                    new LeafInvoke(() => Debug.Log("timer check")),
                                                    new LeafInvoke(() => char5.SmartCrowdAssist("Benjamin and his family says they didn't murder your family member. Are they really in the wrong?")),
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
                                                    new LeafInvoke(() => char6.SmartCrowdAssist("Benjamin and his family says they didn't murder your family member. Are they really in the wrong?")),
                                                    new LeafInvoke(() => Debug.Log("invoked alternative behavior")),
                                                    new LeafInvoke(() => char6.helpable = false)
                                                    ),
                                new LeafAssert(() => seq3helper)
                            )
                    )
                ),

            _ReusableActions.DescribeAnger(char5, char6, char7, char8, list),
            new LeafInvoke(() => Debug.Log("3")),
            _ReusableActions.DemandSearch(char5, char6, char0, char3, char4, list),
            new LeafInvoke(() => Debug.Log("4")),
            new DecoratorLoop(
                new Selector(
                    new Sequence(
                        new LeafAssert(() => !char1.Controlled && !char0.Controlled && !char2.Controlled),
                        new SelectorShuffle(
            //Character not controlled

                            new Sequence(
                                _ReusableActions.WanderAndDisappear(char0, wild, "Where are your friends that went into the wilderness?", list),
                                _ReusableActions.Mourning(char3, char4, char5, char6, char7, char8, circle, list)),
                            new Sequence(
                                _ReusableActions.WanderAndDisappear(char3, wild, "Where are your son's friends?", list),
                                _ReusableActions.Mourning(char0, char4, char5, char6, char7, char8, circle, list)),
                            new Sequence(
                                _ReusableActions.WanderAndDisappear(char4, wild, "Where have your brother's friends?", list),
                                _ReusableActions.Mourning(char0, char3, char5, char6, char7, char8, circle, list)),
                            new Sequence(
                                _ReusableActions.Murder(char5, char0, wild, list)//,
                                          //  _ReusableActions.Mourning(char3, char4, char5, char6, char7, char8, circle)),
                                           ),
                            new Sequence(
                                _ReusableActions.Murder(char5, char3, wild, list)),
            //  _ReusableActions.Mourning(char0, char4, char5, char6, char7, char8, circle)),
                            new Sequence(
                                _ReusableActions.Murder(char5, char4, wild, list)//),
            // _ReusableActions.Mourning(char0, char3, char5, char6, char7, char8, circle))
                                ))
                        ),
                    new Sequence(
                        new DecoratorLoop(
                            new Sequence(
                                new LeafAssert(() => char1.Controlled || char0.Controlled || char2.Controlled || char5.Controlled),
                                new LeafInvoke(() => Debug.Log("Player is controlling Benjamin Family")),
                                new Selector(
                                    new Sequence(
                                        new LeafAssert(() => ((wild.Waypoints[4].gameObject.transform.position - char0.gameObject.transform.position).magnitude < SmartCharacterCC.nearDistance)),
                                        new SelectorShuffle(
                                            _ReusableActions.Murder(char5, char0, wild, list),
                                            _ReusableActions.WanderAndDisappear(char0, wild, "Be careful out there!", list)
                                            ),
                                            new LeafInvoke(() => SmartCharacterCC.done = true)
                                    ),
                                    new Sequence(
                                        new LeafAssert(() => ((wild.Waypoints[4].gameObject.transform.position - char3.gameObject.transform.position).magnitude < SmartCharacterCC.nearDistance)),
                                        new SelectorShuffle(
                                            _ReusableActions.Murder(char5, char3, wild, list),
                                            _ReusableActions.WanderAndDisappear(char1, wild, "Be careful out there!", list)
                                            ),
                                            new LeafInvoke(() => SmartCharacterCC.done = true)
                                    ),
                                    new Sequence(
                                        new LeafAssert(() => ((wild.Waypoints[4].gameObject.transform.position - char4.gameObject.transform.position).magnitude < SmartCharacterCC.nearDistance)),
                                        new SelectorShuffle(
                                            _ReusableActions.Murder(char5, char4, wild, list),
                                            _ReusableActions.WanderAndDisappear(char2, wild, "Be careful out there!", list)
                                            ),
                                            new LeafInvoke(() => SmartCharacterCC.done = true)
                                    ),
                                    new Sequence(
                                        new LeafAssert(() => ((wild.Waypoints[4].gameObject.transform.position - char5.gameObject.transform.position).magnitude < SmartCharacterCC.nearDistance)),
                                        new SelectorShuffle(
                                            _ReusableActions.Murder(char5, char4, wild, list),
                                            _ReusableActions.Murder(char5, char3, wild, list),
                                            _ReusableActions.Murder(char5, char0, wild, list)
                                            ),
                                            new LeafInvoke(() => SmartCharacterCC.done = true)
                                    ),
                                    new Sequence(
                                                    new LeafAssert(() => char0.Controlled),
                                                    new LeafAssert(() => char0.helpable),
                                                    new LeafInvoke(() => Debug.Log("Helpable")),
                                                    new LeafInvoke(() => char0.startTimer()),
                                                    new LeafInvoke(() => Debug.Log("Timer start")),
                                                    new LeafAssert(() => char0.checkTimer()),
                                                    new LeafInvoke(() => Debug.Log("timer check")),
                                                    new LeafInvoke(() => char0.SmartCrowdAssist("The other families are demanding you go back into the wild to search for their family members, what do you do?")),
                                                    new LeafInvoke(() => Debug.Log("invoked alternative behavior")),
                                                    new LeafInvoke(() => char0.helpable = false)
                                                    ),
                                    new Sequence(
                                                    new LeafAssert(() => char3.Controlled),
                                                    new LeafAssert(() => char3.helpable),
                                                    new LeafInvoke(() => Debug.Log("Helpable")),
                                                    new LeafInvoke(() => char3.startTimer()),
                                                    new LeafInvoke(() => Debug.Log("Timer start")),
                                                    new LeafAssert(() => char3.checkTimer()),
                                                    new LeafInvoke(() => Debug.Log("timer check")),
                                                    new LeafInvoke(() => char3.SmartCrowdAssist("The other families are demanding your son to go back into the wild to search for their family members, what do you do?")),
                                                    new LeafInvoke(() => Debug.Log("invoked alternative behavior")),
                                                    new LeafInvoke(() => char3.helpable = false)
                                                    ),
                                    new Sequence(
                                                    new LeafAssert(() => char4.Controlled),
                                                    new LeafAssert(() => char4.helpable),
                                                    new LeafInvoke(() => Debug.Log("Helpable")),
                                                    new LeafInvoke(() => char4.startTimer()),
                                                    new LeafInvoke(() => Debug.Log("Timer start")),
                                                    new LeafAssert(() => char4.checkTimer()),
                                                    new LeafInvoke(() => Debug.Log("timer check")),
                                                    new LeafInvoke(() => char4.SmartCrowdAssist("The other families are demanding your brother to go back into the wild to search for their family members, what do you do?")),
                                                    new LeafInvoke(() => Debug.Log("invoked alternative behavior")),
                                                    new LeafInvoke(() => char4.helpable = false)
                                                    ),
                                    new Sequence(
                                                    new LeafAssert(() => char4.Controlled),
                                                    new LeafAssert(() => char4.helpable),
                                                    new LeafInvoke(() => Debug.Log("Helpable")),
                                                    new LeafInvoke(() => char4.startTimer()),
                                                    new LeafInvoke(() => Debug.Log("Timer start")),
                                                    new LeafAssert(() => char4.checkTimer()),
                                                    new LeafInvoke(() => Debug.Log("timer check")),
                                                    new LeafInvoke(() => char4.SmartCrowdAssist("Y-You aren't doing what I think you're doing... right!?")),
                                                    new LeafInvoke(() => Debug.Log("invoked alternative behavior")),
                                                    new LeafInvoke(() => char4.helpable = false)
                                                    )
                                )
                            )
                    )
                ), new LeafAssert(() => true))
            )
            );
    }

    public static Node Sequence3(SmartCharacterCC char0, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8, SmartWaypointArea area, SmartWaypointArea angryarea, SmartWaypointArea wild, SmartWaypointArea circle)
    {
        return new Sequence(
            new LeafInvoke(() => Debug.Log("SEQUENCE3")),
            _ReusableActions.FamilyReplyDefensive(char0, char3, char4, area, angryarea),
            new LeafInvoke(() => Debug.Log("1")),
           // _ReusableActions.AngryResponse(char3, char4, area, angryarea),
            new LeafInvoke(() => Debug.Log("2")),
            _ReusableActions.DescribeAnger(char5, char6, char7, char8),
            new LeafInvoke(() => Debug.Log("3")),
            _ReusableActions.DemandSearch(char5, char6, char0, char3, char4),
            new LeafInvoke(() => Debug.Log("4")),
            new DecoratorLoop(
                new Selector(
                    new Sequence(
                        new LeafAssert(() => !char1.Controlled && !char0.Controlled && !char2.Controlled),
                        new SelectorShuffle(
            //Character not controlled

                            new Sequence(
                                _ReusableActions.WanderAndDisappear(char0, wild, "Where are your friends that went into the wilderness?"),
                                _ReusableActions.Mourning(char3, char4, char5, char6, char7, char8, circle)),
                            new Sequence(
                                _ReusableActions.WanderAndDisappear(char3, wild, "Where are your son's friends?"),
                                _ReusableActions.Mourning(char0, char4, char5, char6, char7, char8, circle)),
                            new Sequence(
                                _ReusableActions.WanderAndDisappear(char4, wild, "Where have your brother's friends?"),
                                _ReusableActions.Mourning(char0, char3, char5, char6, char7, char8, circle)),
                            new Sequence(
                                _ReusableActions.Murder(char5, char0, wild)//,
//                                _ReusableActions.Mourning(char3, char4, char5, char6, char7, char8, circle)),
                                           ),
                            new Sequence(
                                _ReusableActions.Murder(char5, char3, wild)),
                              //  _ReusableActions.Mourning(char0, char4, char5, char6, char7, char8, circle)),
                            new Sequence(
                                _ReusableActions.Murder(char5, char4, wild)//),
                               // _ReusableActions.Mourning(char0, char3, char5, char6, char7, char8, circle))
                                ))
                        ),
                    new Sequence(
                        new DecoratorLoop(
                            new Sequence(
                                new LeafAssert(() => char1.Controlled || char0.Controlled || char2.Controlled),
                                new LeafInvoke(() => Debug.Log("Player is controlling Benjamin Family")),
                                new Selector(
                                    new Sequence(
                                        new LeafAssert(() => ((wild.Waypoints[4].gameObject.transform.position - char0.gameObject.transform.position).magnitude >= SmartCharacterCC.interactionDistance)),
                                        new SelectorShuffle(
                                            _ReusableActions.Murder(char5, char0, wild),
                                            _ReusableActions.WanderAndDisappear(char0, wild, "Be careful out there!")
                                            )
                                    ),
                                    new Sequence(
                                        new LeafAssert(() => ((wild.Waypoints[4].gameObject.transform.position - char1.gameObject.transform.position).magnitude >= SmartCharacterCC.interactionDistance)),
                                        new SelectorShuffle(
                                            _ReusableActions.Murder(char5, char3, wild),
                                            _ReusableActions.WanderAndDisappear(char1, wild, "Be careful out there!")
                                            )
                                    ),
                                    new Sequence(
                                        new LeafAssert(() => ((wild.Waypoints[4].gameObject.transform.position - char2.gameObject.transform.position).magnitude >= SmartCharacterCC.interactionDistance)),
                                        new SelectorShuffle(
                                            _ReusableActions.Murder(char5, char4, wild),
                                            _ReusableActions.WanderAndDisappear(char2, wild, "Be careful out there!")
                                            )
                                    )
                                )
                            )
                    )
                ))
            )
            );
    }

    public static Node Sequence2(SmartCharacterCC char0, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8, SmartWaypointArea area, SmartWaypointArea angryarea, SmartWaypointArea wild, SmartWaypointArea circle, SmartStoryTextList list)
    {
        return new DecoratorLoop(
            new Selector(
                new Sequence(
                    new DecoratorLoop(
                        new Sequence(

                            new LeafAssert(() => char3.Controlled || char4.Controlled || char0.Controlled),
                            new LeafInvoke(() => Debug.Log("Benjamin Family Controlled")),
                            new Selector(
                                    new Sequence(
                                        new LeafAssert(() => char3.status == SmartCharacterCC.gesture_map_id["FISTSHAKE"] || char4.status == SmartCharacterCC.gesture_map_id["FISTSHAKE"]),
                                        new LeafInvoke(() => Debug.Log("Player has Defended family member")),
                                        _ReusableActions.Sequence3(char0, char1, char2, char3, char4, char5, char6, char7, char8, area, angryarea, wild, circle, list),
                                    new Sequence(
                                        new LeafAssert(() => char3.status == SmartCharacterCC.gesture_map_id["MOURN"] || char4.status == SmartCharacterCC.gesture_map_id["MOURN"]),
                                        new LeafInvoke(() => Debug.Log("Player has chosen to mourn for the losses")),
                                        _ReusableActions.Mourning(char0, char3, char4, char5, char6, char7, char8, circle, list)
            //THE STORY ENDS HERE WITH EVERYONE MOURNING
                                        )
                                )
                            )

                            )


                        )
                ),
                new SelectorShuffle(
                                           //_ReusableActions.Mourning(char0, char3, char4, char5, char6, char7, char8, circle, list),
                                _ReusableActions.Sequence3(char0, char1, char2, char3, char4, char5, char6, char7, char8, area, angryarea, wild, circle, list)
                        )
                    )
            );
    }

    public static Node Sequence2(SmartCharacterCC char0, SmartCharacterCC char1, SmartCharacterCC char2, SmartCharacterCC char3, SmartCharacterCC char4, SmartCharacterCC char5, SmartCharacterCC char6, SmartCharacterCC char7, SmartCharacterCC char8, SmartWaypointArea area, SmartWaypointArea angryarea, SmartWaypointArea wild, SmartWaypointArea circle)
    {
        return new DecoratorLoop(
            new Selector(
                new Sequence(
                    new DecoratorLoop(
                        new Sequence(

                            new LeafAssert(() => char3.Controlled || char4.Controlled || char0.Controlled),
                            new LeafInvoke(() => Debug.Log("Benjamin Family Controlled")),
                            new Selector(
                                    new Sequence(
                                        new LeafAssert(() => char3.status == SmartCharacterCC.gesture_map_id["FISTSHAKE"] || char4.status == SmartCharacterCC.gesture_map_id["FISTSHAKE"]),
                                        new LeafInvoke(() => Debug.Log("Player has Defended family member")),
                                        _ReusableActions.Sequence3(char0, char1, char2, char3, char4, char5, char6, char7, char8, area, angryarea, wild, circle),
                                    new Sequence(
                                        new LeafAssert(() => char3.status == SmartCharacterCC.gesture_map_id["MOURN"] || char4.status == SmartCharacterCC.gesture_map_id["MOURN"]),
                                        new LeafInvoke(() => Debug.Log("Player has chosen to mourn for the losses")),
                                        _ReusableActions.Mourning(char0, char3, char4, char5, char6, char7, char8, circle)
            //THE STORY ENDS HERE WITH EVERYONE MOURNING
                                        )
                                )
                            )

                            )


                        )
                ),
                new SelectorShuffle(
            //                                _ReusableActions.Mourning(char0, char3, char4, char5, char6, char7, char8),
                                _ReusableActions.Sequence3(char0, char1, char2, char3, char4, char5, char6, char7, char8, area, angryarea, wild, circle)
                        )
                    )
            );
    }
}

