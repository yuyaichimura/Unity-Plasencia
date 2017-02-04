using UnityEngine;
using System.Collections;
using TreeSharpPlus;


public class SmartMiscObjects : SmartObject
{
    public Transform[] objects;
    public ParticleSystem[] particles;

    public override string Archetype
    {
        get { return this.GetType().Name; }
    }

    public Node Node_SetObject(int index, bool active)
    {
        if (this.objects.Length <= index)
        {
            return null;
        }
        return new LeafInvoke(() => objects[index].gameObject.SetActive(active));
    }

    public Node Node_SetParticle(int index, bool active)
    {
        if(this.particles.Length <= index){
            return null;
        }
        return new LeafInvoke(() => particles[index].enableEmission = active);
    }
}
