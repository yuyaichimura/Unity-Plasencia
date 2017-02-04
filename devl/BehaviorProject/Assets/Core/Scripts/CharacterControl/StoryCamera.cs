using UnityEngine;
using System.Collections;

public class StoryCamera : MonoBehaviour
{
    public Transform[] positions;
    int index;

    void Start()
    {
        index = 0;
    }

    public void moveCamera(){
        index++;

        if (index >= positions.Length)
        {
            index = 0;
        }
        target = positions[index];
        StartCoroutine(Transition());

    }

    public float transitionDuration = 2.5f; 
    Transform target;
    IEnumerator Transition()
    {
        float t = 0.0f; Vector3 startingPos = transform.position; while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.position = Vector3.Lerp(startingPos, target.position, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, t/4);
            yield return 0;
        }
    }
}
