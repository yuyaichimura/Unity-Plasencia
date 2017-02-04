using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

    Transform gCurrentTarget;
	Texture2D talkToIcon;
	bool onHover = false;
	CursorMode cursorMode = CursorMode.Auto;
	Vector2 hotspot = Vector2.zero;

    void Start() {
		talkToIcon = (Texture2D) Resources.Load("approachIcon");
        gCurrentTarget = null;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            handleLeftClick();
        }
        else if (Input.GetMouseButtonDown(1)) {
            handleRightClick();
        }  /* else { forget about the stupid cursor for now
			Transform tmp = selectObject();
			if(!onHover  && tmp != null && gCurrentTarget != null && tmp != gCurrentTarget) {
				Debug.Log ("Hovering over NPC");
				onHover = true;
				Cursor.SetCursor(talkToIcon,hotspot,cursorMode);
			} else {
				onHover = false;
			}
		}
		*/
    }


	private bool HoverOnNPC() {
		Transform tmp = selectObject();
		if(tmp != null) {
			return true;
		}
		return false;
	}

    private void handleRightClick() {
		if (gCurrentTarget != null) {
            Vector3 target = getPosition();
			if(HoverOnNPC()) {
				Transform tmp = selectObject();
				gCurrentTarget.GetComponent<NPCController>().ApproachEntity(tmp.GetComponent<NPCEntity>());
			} else if (target != Vector3.zero) {
                gCurrentTarget.GetComponent<NPCController>().WalkTo(target);
            } 
        }
    }

    private void handleLeftClick() {
        Transform tmp = selectObject();
        if (tmp != null) {
            if (gCurrentTarget != null) gCurrentTarget.GetComponent<NPCController>().SetSelected(false);
            gCurrentTarget = tmp;
            gCurrentTarget.GetComponent<NPCController>().SetSelected(true);
        }
        else {
            if (gCurrentTarget != null) {
                gCurrentTarget.GetComponent<NPCController>().SetSelected(false);
                gCurrentTarget = null;
            }
        }
    }

    private Transform selectObject() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        int layer = (1 << LayerMask.NameToLayer("NPC"));
        if (Physics.Raycast(ray, out hit, 100, layer)) {
            GameObject o = hit.transform.gameObject;
            return o.GetComponent<NPCController>() != null ? o.transform : null;
        } else return null;
    }

    private Vector3 getPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        int mask = (1 << LayerMask.NameToLayer("World"));
        if (Physics.Raycast(ray, out hit, 100, mask)) {
            GameObject o = hit.transform.gameObject;
            return hit.point;
        }
        else return Vector3.zero;
    }
}
