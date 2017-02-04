using UnityEngine;
using System.Collections;
using TreeSharpPlus;

public class MouseController2 : MonoBehaviour
{
    public Camera cam1;
    public Camera cam2;
    private SmartCharacterCC character;

    CursorMode cursorMode = CursorMode.Auto;

    void Start()
    {
        this.character = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RightClick();
        }
        else if (Input.GetMouseButtonDown(0))
        {
            LeftClick();
        }
        else if (Input.GetButtonDown("Bow"))
        {
            if (this.character != null)
            {
                character.Action_Bow();
            }
        }
        else if (Input.GetButtonDown("ShakeHand"))
        {
            if (this.character != null)
                character.Action_ShakeHand();
        }
        else if (Input.GetButtonDown("OpenDoor"))
        {
            if (this.character != null)
                character.Action_OpenDoor();
        }
        else if (Input.GetButtonDown("action1"))
        {
            if (this.character != null)
                character.Action_Mourn();
        }
        else if (Input.GetButtonDown("action2"))
        {
            if (this.character != null)
                character.Action_Cheer();
        }
        else if (Input.GetButtonDown("action3"))
        {
            if (this.character != null)
                character.Action_Yell();
        }
        else if (Input.GetButtonDown("action4"))
        {
            if (this.character != null)
                character.Action_FistShake();
        }
        else if (Input.GetButtonDown("action5"))
        {
            if (this.character != null)
                character.Action_Threaten();
        }
        else if (Input.GetButtonDown("action6"))
        {
            if (this.character != null)
                character.Action_Kill();
        }
        else if (Input.GetButtonDown("action7"))
        {
            if (this.character != null)
                character.Action_Die();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            if (this.character != null)
                character.Action_GetNearestSmartCrowd();
        } else if(Input.GetKeyDown(KeyCode.P)){
            this.cam1.gameObject.SetActive(!this.cam1.gameObject.activeSelf);
            this.cam2.gameObject.SetActive(!this.cam2.gameObject.activeSelf);

        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            StoryCamera cam = this.cam2.GetComponent<StoryCamera>();
            if (cam!= null) cam.moveCamera();
        }

    }

    private void LeftClick()
    {
        this.character = getCharacter();
        if (character != null)
        {        
            this.character.Select();
        }
    }

    private void RightClick()
    {

        Vector3 target = getPosition();
        if (target != null && target != Vector3.zero && character != null)
        {
            character.MoveCharacter(Val.V(() => target));
        }
    }

    private SmartCharacterCC getCharacter()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        int mask = (1 << LayerMask.NameToLayer("Character"));

        if (Physics.Raycast(ray, out hit, 100, mask))
        {
            GameObject o = hit.transform.gameObject;
            SmartCharacterCC selectedCharacter = o.GetComponent<SmartCharacterCC>();
            AssignId selectedCharacterId = o.GetComponent<AssignId>();
            AssignId thisCharId  = null;
            if(this.character != null)
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
