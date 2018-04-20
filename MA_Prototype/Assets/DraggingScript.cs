using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggingScript : MonoBehaviour {

	private Vector3 screenPoint;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown() {

		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.parent.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag() {
		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);			// Current touch point

		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;								// Current touch point converted to point in scene

			transform.position = curPosition;
	}
}
