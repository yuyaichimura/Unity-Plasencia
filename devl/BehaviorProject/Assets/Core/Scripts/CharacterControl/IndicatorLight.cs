using UnityEngine;
using System.Collections;

public class IndicatorLight : MonoBehaviour {

    Light goalLight;

    public bool on = false;

    void Start()
    {
        Light[] lights = this.gameObject.GetComponentsInChildren<Light>();

        if (lights != null)
        {
            foreach (Light light in lights)
            {
                if (light.CompareTag("goal"))
                {
                    goalLight = light;
                }
            }
            if(goalLight != null)
                goalLight.enabled = false;
        }
    }

    public void turnOn()
    {
        if (goalLight != null)
        {
            goalLight.enabled = true;
            on = true;
        }
    }

    public void turnOff()
    {
        if (goalLight != null)
        {
            goalLight.enabled = false;
            on = false;
        }
    }
}
