using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrowdColliderCC : MonoBehaviour
{

    List<Collider> CrowdList;

    void Start()
    {
        this.CrowdList = new List<Collider>();
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag(SmartCharacterControl.TAG_SMARTCROWD))
        {
            if (!CrowdList.Contains(obj))
            {
                CrowdList.Add(obj);

            }
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (CrowdList.Contains(obj))
        {
            CrowdList.Remove(obj);
        }
    }

    public SmartCharacterControl GetNearestObject()
    {

        SmartCharacterControl closestObject = null;
        float distance = Mathf.Infinity;
        Vector3 pos = transform.position;

        foreach (Collider i in CrowdList)
        {
            Vector3 dif = i.transform.position - pos;
            float dist = dif.sqrMagnitude;
            if (dist < distance && this.gameObject.name != i.gameObject.name)
            {
                closestObject = i.gameObject.GetComponent<SmartCharacterControl>();
                distance = dist;
            }
        }

        return closestObject;
    }
}
