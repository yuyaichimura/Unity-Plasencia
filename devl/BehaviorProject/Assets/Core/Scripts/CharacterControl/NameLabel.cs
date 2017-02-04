using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NameLabel: MonoBehaviour
{
    public bool FaceCamera = true;

    void Start()
    {
        //this.renderer.enabled = true;
        TextMesh text = this.gameObject.GetComponentInChildren<TextMesh>();
        text.text = this.transform.parent.gameObject.name;
    }

    void Update()
    {
        if (this.FaceCamera == true)
        {
            // Getting a NullReferenceException here? Make sure your camera is
            // tagged as "MainCamera" in your Unity scene.
            Quaternion direction =
                Quaternion.LookRotation(
                    Camera.main.transform.position - transform.position);

            direction.x = 0.0f;
            direction.z = 0.0f;
            transform.rotation = direction;
            transform.Rotate(new Vector3(90.0f, 0.0f, 0.0f));
        }
    }
}
