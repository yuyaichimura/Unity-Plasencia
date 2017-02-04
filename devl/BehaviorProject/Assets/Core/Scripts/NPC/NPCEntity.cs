using UnityEngine;
using System.Collections;

public abstract class NPCEntity : MonoBehaviour, IPercievable {
    public abstract string Name { get; }
    public abstract Vector3 BodyCenter { get; }
    public abstract Entity EntityType { get; }
    public abstract NPCState CurrentState { get; }
}
