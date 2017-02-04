using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeSharpPlus;

public class MouseCharacterController : MonoBehaviour {

    public Camera Camera1;
    public Camera Camera2;
    private SmartCharacterControl character;

    CursorMode cursorMode = CursorMode.Auto;

	void Start () {
        this.character = null;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(1))
        {
            RightClick();
        }
        else if (Input.GetMouseButton(0))
        {
            LeftClick();
        }
	}

    private void LeftClick()
    {
       
        this.character = getCharacter();
        if(character != null)
        {
            this.character.Select();
        }
    }

    private void RightClick()
    {
        Vector3 target = getPosition();
        if(target != null  && target != Vector3.zero && character != null)
        {
            this.character.MoveCharacter(Val.V(() => target));
        }
    }

    private SmartCharacterControl getCharacter()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        int mask = (1 << LayerMask.NameToLayer("Character"));
        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            GameObject o = hit.transform.gameObject;
            SmartCharacterControl selectedCharacter = o.GetComponent<SmartCharacterControl>();
            AssignId selectedCharacterId = o.GetComponent<AssignId>();
            AssignId thisCharId = null;
            if (this.character != null)
                thisCharId = this.character.GetComponent<AssignId>();
            if (this.character != null && selectedCharacterId != null && selectedCharacterId.Id != thisCharId.Id)
            {
                character.Unselect();
            }
            if (selectedCharacter == null || !selectedCharacter.Controllable)
            {
                return null;
            }

            return selectedCharacter;

        }
        else
        {
            if (this.character != null)
            {
                this.character.Unselect();
            }
            return null;
        }
    }

    private Vector3 getPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        int mask = (1 << LayerMask.NameToLayer("World"));
        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            GameObject o = hit.transform.gameObject;
            return hit.point;
        }
        else return Vector3.zero;
    }
}
