using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickAndDragTest : MonoBehaviour {

	float levelTimer;
	bool updateTimer;
	bool pressed = false;

	private Vector3 screenPoint;
	private Vector3 offset;

	// Use this for initialization
	void Start () {
		levelTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (pressed) {
			levelTimer += Time.deltaTime;
		}
	}

	void OnMouseDown() {
		pressed = true;

		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag() {
		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);			// Current touch point

		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;								// Current touch point converted to point in scene

		transform.position = curPosition;
	}

	void OnMouseUp() {
		Debug.Log ("Mouse Up");
		Debug.Log (levelTimer + " seconds");
		levelTimer = 0;
		pressed = false;
	}
}
