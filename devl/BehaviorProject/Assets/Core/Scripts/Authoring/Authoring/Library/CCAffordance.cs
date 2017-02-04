using RootMotion.FinalIK;
using System.Collections.Generic;
using TreeSharpPlus;
using UnityEngine;

public class CCAffordance 
{
    static readonly float STOP_EPSILON = 0.05f;
    public static float stoppingRadius = 0.4f;

    #region Navigation
    
    public static Node Node_GoTo(Val<Vector3> targ, SmartCharacterCC character)
    {
        return new LeafInvoke(
            () => character.Behavior.Character.NavGoTo(targ),
            () => CCAffordance.NavArrived(character.Character, targ));
    }

    public static RunStatus NavArrived(CharacterMecanim charMech, Val<Vector3> targ)
    {
        if ((charMech.transform.position - targ.Value).magnitude <= (stoppingRadius + STOP_EPSILON))
        {
               return RunStatus.Success;
        }

        return RunStatus.Running;
    }

    #endregion
}

