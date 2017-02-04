using UnityEngine;
using System.Collections;
using TreeSharpPlus;


public class SmartCamera : SmartObject{

    public float fadeSpeed = 1.8f;

    public override string Archetype
    {
        get { return this.GetType().Name; }
    }

    public IEnumerator moveCamera(float transitionDuration, Transform target, float turningValue)
    {

        yield return StartCoroutine(TransitionCamera(transitionDuration, target, turningValue));
    }

    public float transitionDuration = 2.5f;
    Transform target;
    IEnumerator TransitionCamera(float transitionDuration, Transform target, float turningValue)
    {
        float t = 0.0f; Vector3 startingPos = transform.position; while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale / transitionDuration);

            transform.position = Vector3.Lerp(startingPos, target.position, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, t / turningValue);
            yield return 0;
        }
    }

    public Node CameraMove2(float transitionDuration, Transform target, float turningValue)
    {
//        return new LeafInvoke(() => moveCamera(transitionDuration, target, turningValue));
        return new LeafInvoke(() => Debug.Log("Moving to " + transform.position));
    }

    public Node CameraMove(float transitionDuration, Transform target, float turningValue)
    {
        //        return new LeafInvoke(() => moveCamera(transitionDuration, target, turningValue));
      //return new LeafInvoke(() => Debug.Log("Moving to " + transform.position));
      return new LeafInvoke(() => StartCoroutine(TransitionCamera(transitionDuration, target, turningValue)));

    }

    float s = 0.0f;
    /*
    public RunStatus Moving(float transitionDuration, Transform target, float turningValue)
    {
        if()
    }*/

    void FadeToBlack()
    {
        Camera camera = this.gameObject.GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Color.black;
        camera.cullingMask = 0;
        this.gameObject.GetComponent<Timeline>().enabled = false;
        this.gameObject.GetComponent<CharacterMinimap>().enabled = false;

        /*
        GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<SceneFadeInOut>().sceneStarting = false;
        GameObject.FindGameObjectWithTag(Tags.fader).GetComponent<SceneFadeInOut>().EndScene();*/

    }

    public Node Node_CameraFadeBlack()
    {
        return new LeafInvoke(() => FadeToBlack());
    }
}
