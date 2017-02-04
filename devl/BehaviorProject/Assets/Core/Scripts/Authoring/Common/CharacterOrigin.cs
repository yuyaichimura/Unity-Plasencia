using UnityEngine;
using System.Collections;

public class CharacterOrigin : MonoBehaviour {

    public Transform origin;
    public Transform lookAt;

    public Transform getOrigin()
    {
        return origin;
    }

    public Transform getLookAt()
    {
        return lookAt;
    }
}
