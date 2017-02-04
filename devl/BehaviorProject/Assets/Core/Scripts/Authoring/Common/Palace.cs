using UnityEngine;
using System.Collections;

public class Palace : MonoBehaviour {


    public Transform palace1;
    public Transform palace2;

    public void SetActivePalace(bool active)
    {
        this.palace1.gameObject.SetActive(active);
        this.palace2.gameObject.SetActive(active);
    }

    public Transform GetPalace1()
    {
        return palace1;
    }

    public Transform GetPalace2()
    {
        return palace2;
    }
}
