using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionBlock_Labeled: MonoBehaviour {

	public Transform block, clone;
	public int speed;
	private bool isClone = false;

	private bool inputAset = false;
	private bool inputBset = false;
	bool outputSet = false;

	// Use this for initialization
	void Start () {

		if (gameObject.name.Contains ("(Clone)")) {
			isClone = true;
		}
			
		block = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	private Vector3 screenPoint;
	private Vector3 offset;

	void OnMouseDown() {

		if (!isClone) {
			clone = Instantiate (block);
		}
//		if (isClone) {
			screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);

			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
//		}
	}

	void OnMouseDrag() {

//		if (isClone) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);			// Current touch point

			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;								// Current touch point converted to point in scene

		gameObject.GetComponentInParent<Transform>().position = curPosition;																				// Move clone to this position
//		}
	}

	public void setInputA() {
		inputAset = true;
	}
	public void setInputB() {
		inputBset = true;
	}
	public void setOutput() {
		outputSet = true;
	}
}