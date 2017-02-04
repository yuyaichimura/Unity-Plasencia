using UnityEngine;
using System.Collections;

public class CharacterControl : MonoBehaviour {

    SmartCharacter character;

	// Use this for initialization
	void Start () {
        this.character = gameObject.GetComponent<SmartCharacter>();
	}
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = getPosition();
            Debug.Log("Moving to " + pos.ToString());
            character.Node_GoTo(pos);
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
