using UnityEngine;
using System.Collections;

public enum Entity {
    OBJECT, CHARACTER
}

public interface IPercievable {
    Vector3 BodyCenter { get; }
    Entity EntityType { get; }
    NPCState CurrentState { get; }
}
