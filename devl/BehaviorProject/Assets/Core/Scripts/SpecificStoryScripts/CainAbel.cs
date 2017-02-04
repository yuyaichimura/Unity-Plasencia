using UnityEngine;
using System.Collections;
using TreeSharpPlus;

[LibraryIndexAttribute(10)]
public class Discredit : SmartEvent
{
	SmartCharacterCC character1;
	SmartCharacterCC character2;

	[Name("Discredit")]
	public Discredit (SmartCharacterCC character1, SmartCharacterCC character2)
        : base(character1, character2)
	{
		this.character1 = character1;
		this.character2 = character2;
	}

	public override Node BakeTree (Token token)
	{
		if (character1.gameObject.GetComponent<AssignId> ().Id == character2.gameObject.GetComponent<AssignId> ().Id) {
			return new Sequence (
				this.character1.Node_Icon ("discredit_xx"),
				this.character1.ST_PlayHandGesture_NI ("intimidate", 2300),
				this.character1.Node_Icon (null)
			);
		}

		return new Sequence (
            this.character1.Node_Icon ("discredit_xy"),
            this.character1.ST_PlayHandGesture_NI ("fistshake", 2000),
            new SequenceParallel (
                   new Sequence (new LeafWait (1200), character1.Node_Icon (null)),

                   this.character2.ST_PlayHandGesture_NI ("intimidate", 2500))

		);
	}
}


[LibraryIndexAttribute(10)]
public class Incite : SmartEvent
{
	SmartCharacterCC character1;
	SmartCharacterCC character2;

	[Name("Incite")]
	public Incite (SmartCharacterCC character1, SmartCharacterCC character2)
        : base(character1, character2)
	{
		this.character1 = character1;
		this.character2 = character2;
	}

	public override Node BakeTree (Token token)
	{
		return new Sequence (
            this.character1.Node_Icon ("incite_xy"),
            this.character1.ST_PlayHandGesture_NI ("fistSHake", 2000),
           // this.character2.Node_Icon("incite_xy_r"),
            new SequenceParallel (
                   new Sequence (new LeafWait (1200), character1.Node_Icon (null)),

                   this.character2.ST_PlayHandGesture_NI ("yell1", 2500))//,
                   //this.character2.Node_Icon(null)
                   
		);
	}
}

[LibraryIndexAttribute(10)]
public class Scorn : SmartEvent
{
	SmartCharacterCC character1;
	SmartCharacterCC character2;

	[Name("Scorn")]
	public Scorn (SmartCharacterCC character1, SmartCharacterCC character2)
        : base(character1, character2)
	{
		this.character1 = character1;
		this.character2 = character2;
	}

	public override Node BakeTree (Token token)
	{
		return new Sequence (
            this.character1.ST_PlayHandGesture_NI ("dismiss", 2000),

                   this.character2.ST_PlayHandGesture_NI ("forward", 2500)
		);
	}
}

[LibraryIndexAttribute(10)]
public class Deride : SmartEvent
{
	SmartCharacterCC character1;
	SmartCharacterCC character2;

	[Name("Deride")]
	public Deride (SmartCharacterCC character1, SmartCharacterCC character2)
        : base(character1, character2)
	{
		this.character1 = character1;
		this.character2 = character2;
	}

	public override Node BakeTree (Token token)
	{
		return new Sequence (
            new SequenceParallel (
                character1.Node_OrientTowards (character2.transform.position),
                character2.Node_OrientTowards (character1.transform.position)
		),
            this.character1.Node_Icon ("deride_xy"),
            this.character1.ST_PlayHandGesture_NI ("yell2", 2500),
            this.character1.Node_Icon (null),
            this.character2.ST_PlayHandGesture_NI ("BEINGCOCKY", 2300)
		);
	}
}



          

[LibraryIndexAttribute(10)]
public class DismissAndPoint : SmartEvent
{
	SmartCharacterCC character1;
	SmartCharacterCC character2;

	[Name("Call Worthless")]
	public DismissAndPoint (SmartCharacterCC character1, SmartCharacterCC character2)
        : base(character1, character2)
	{
		this.character1 = character1;
		this.character2 = character2;
	}

	public override Node BakeTree (Token token)
	{

		return new Sequence (new SequenceParallel (
                character1.Node_OrientTowards (character2.transform.position),
                character2.Node_OrientTowards (character1.transform.position)
		),
            this.character1.Node_Icon ("callworthless_xy"),
            this.character1.ST_PlayHandGesture_NI ("STAYAWAY", 3000),
            new SequenceParallel (
                new Sequence (
                    new LeafWait (1000), character1.Node_Icon (null)
		),
                this.character2.Node_Icon ("callworthless_xy_r"),
                this.character2.ST_PlayHandGesture_NI ("POINTING", 3000)
		),
                new LeafWait (1000),
                character2.Node_Icon (null)
		);
	}
}

[LibraryIndexAttribute(10)]
public class GoTo : SmartEvent
{
	SmartCharacterCC character1;
	SmartWaypoint waypoint;

	[Name("GoTo")]
	public GoTo (SmartCharacterCC character1, SmartWaypoint waypoint)
        : base(character1, waypoint)
	{
		this.character1 = character1;
		this.waypoint = waypoint;
	}

	public override Node BakeTree (Token token)
	{
		return new Sequence (
            this.character1.Node_GoToX (waypoint)
		);
	}
}

[LibraryIndexAttribute(10)]
public class Converse1 : SmartEvent
{
	SmartCharacterCC character1;
	SmartCharacterCC character2;
	SmartWaypoint waypoint;

	[Name("Converse 1")]
	public Converse1 (SmartCharacterCC character1, SmartCharacterCC character2)
        : base(character1, character2)
	{
		this.character1 = character1;
		this.character2 = character2;
	}

	public override Node BakeTree (Token token)
	{
		return new Sequence (
             new SequenceParallel (
                character1.Node_OrientTowards (character2.transform.position),
                character2.Node_OrientTowards (character1.transform.position)
		),
                this.character1.ST_PlayHandGesture_NI ("happy", 1500),
            this.character2.ST_PlayHandGesture_NI ("think", 3000)
		);
	}
}

[LibraryIndexAttribute(10)]
public class Converse2 : SmartEvent
{
	SmartCharacterCC character1;
	SmartCharacterCC character2;
	SmartWaypoint waypoint;

	[Name("Converse 2")]
	public Converse2 (SmartCharacterCC character1, SmartCharacterCC character2)
        : base(character1, character2)
	{
		this.character1 = character1;
		this.character2 = character2;
	}

	public override Node BakeTree (Token token)
	{
		return new Sequence (
             new SequenceParallel (
                character1.Node_OrientTowards (character2.transform.position),
                character2.Node_OrientTowards (character1.transform.position)
		),
                this.character1.ST_PlayHandGesture_NI ("fistpump", 1500),
            this.character2.ST_PlayHandGesture_NI ("nod", 1300)
		);
	}
}

[LibraryIndexAttribute(10)]
public class Estrange : SmartEvent
{
	SmartCharacterCC character1;
	SmartCharacterCC character2;

	[Name("Estrange")]
	public Estrange (SmartCharacterCC character1, SmartCharacterCC character2)
        : base(character1, character2)
	{
		this.character1 = character1;
		this.character2 = character2;
	}

	public override Node BakeTree (Token token)
	{
		return new Sequence (
            new SequenceParallel (
                character1.Node_OrientTowards (character2.transform.position),
                character2.Node_OrientTowards (character1.transform.position)
		),
            this.character1.Node_Icon ("estrange_xy"),
            this.character1.ST_PlayHandGesture_NI ("STAYAWAY", 3000),
            new LeafWait (1500),
            character1.Node_Icon (null)
		);
	}
}


[LibraryIndexAttribute(10)]
public class EstrangeLeave : SmartEvent
{
	SmartCharacterCC character1;
	SmartCharacterCC character2;
	SmartWaypoint waypoint;

	[Name("Estrange and Leave")]
	public EstrangeLeave (SmartCharacterCC character1, SmartCharacterCC character2, SmartWaypoint waypoint)
        : base(character1, character2, waypoint)
	{
		this.character1 = character1;
		this.character2 = character2;
		this.waypoint = waypoint;
	}

	public override Node BakeTree (Token token)
	{
		return new Sequence (
            new SequenceParallel (
                character1.Node_OrientTowards (character2.transform.position),
                character2.Node_OrientTowards (character1.transform.position)
		),
            this.character1.Node_Icon ("estrange_xy"),
            this.character1.ST_PlayHandGesture_NI ("STAYAWAY", 3000),
            new SequenceParallel (
                new Sequence (
                    new LeafWait (1500),
                    character1.Node_Icon (null)
		),
                this.character1.Node_GoToX (waypoint)
		)
		);
	}
}

[LibraryIndexAttribute(10)]
public class Kill : SmartEvent
{
	SmartCharacterCC character1;
	SmartCharacterCC character2;

	[Name("Kill")]
	public Kill (SmartCharacterCC character1, SmartCharacterCC character2)
        : base(character1, character2)
	{
		this.character1 = character1;
		this.character2 = character2;
	}

	public override Node BakeTree (Token token)
	{
		return new Sequence (
            new SequenceParallel (
                character1.Node_OrientTowards (character2.transform.position),
                character2.Node_OrientTowards (character1.transform.position)
		),
                this.character1.Node_Icon ("kill_xy"),

            new SequenceParallel (
                this.character1.ST_PlayHandGesture_NI ("HOOKRIGHT", 700),

                new Sequence (new LeafWait (700),
                    this.character2.ST_PlayHandGesture_NI2 ("DIE", 1000))
		),
                this.character2.Node_Icon ("skull"),

                new LeafWait (1500),
                this.character1.Node_Icon (null)
		);
	}
}

[LibraryIndexAttribute(10)]
public class Suicide : SmartEvent
{
	SmartCharacterCC character1;
	SmartCharacterCC character2;

	[Name("Suicide")]
	public Suicide (SmartCharacterCC character1, SmartCharacterCC character2)
        : base(character1, character2)
	{
		this.character1 = character1;
		this.character2 = character2;
	}

	public override Node BakeTree (Token token)
	{
		return new Sequence (
            new SequenceParallel (
                character1.Node_OrientTowards (character2.transform.position),
                character2.Node_OrientTowards (character1.transform.position)
		),
                this.character2.ST_PlayHandGesture_NI ("surprised", 2000),
            this.character1.ST_PlayHandGesture_NI ("cry", 3000),
            this.character1.Node_Icon ("suicide_xx"),
            this.character1.ST_PlayHandGesture_NI ("THREATEN", 3500),
            this.character1.ST_PlayHandGesture_NI2 ("DIE", 3000),
            this.character1.Node_Icon ("skull")
		);
	}
}
