using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractionCollider : MonoBehaviour
{

    List<Collider> InteractableList;

    void Start()
    {
        this.InteractableList = new List<Collider>();
    }

    void OnTriggerEnter(Collider obj)
    {
        if (obj.gameObject.CompareTag(SmartCharacterCC.TAG_SMARTCHARACTER) || obj.gameObject.CompareTag(SmartCharacterCC.TAG_SMARTDOOR))
        {
            if (!InteractableList.Contains(obj))
            {
                InteractableList.Add(obj);
              //  Debug.Log("Interaction collider - Adding " + obj.name);
            }

        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (InteractableList.Contains(obj))
        {
            InteractableList.Remove(obj);
        }
    }

    public SmartObject GetNearestObject(string tag)
    {

        SmartObject closestObject = null;
        float distance = Mathf.Infinity;
        Vector3 pos = transform.position;

        foreach (Collider i in InteractableList)
        {
            Vector3 dif = i.transform.position - pos;
            float dist = dif.sqrMagnitude;
            if (dist < distance && this.gameObject.transform.parent.name != i.gameObject.name && i.CompareTag(tag))
            {
                closestObject = i.gameObject.GetComponentInChildren<SmartObject>();
                distance = dist;
            }
        }
        if (closestObject != null)
        {
           // Debug.Log(this.gameObject.transform.parent.name + " -> " + closestObject.name);
        }
        else
        {
          //  Debug.Log("No nearby SMartOBject");

        }
        return closestObject;
    }

    public SmartObject GetNearestObject()
    {

        SmartObject closestObject = null;
        float distance = Mathf.Infinity;
        Vector3 pos = transform.position;

        foreach (Collider i in InteractableList)
        {
            Vector3 dif = i.transform.position - pos;
            float dist = dif.sqrMagnitude;
            if (dist < distance && this.gameObject.name != i.gameObject.name && i.CompareTag(tag))
            {
                closestObject = i.gameObject.GetComponentInChildren<SmartObject>();
                distance = dist;
            }
        }
        if (closestObject != null)
        {
            Debug.Log(this.gameObject.name + " -> " + closestObject.name);
        }
        else
        {
            Debug.Log("No nearby SmartOBject");

        }
        return closestObject;
    }
}
