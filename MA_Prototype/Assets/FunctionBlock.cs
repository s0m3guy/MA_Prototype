using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionBlock: MonoBehaviour {

	public Transform block;
	public int speed;
	private bool isClone = false;
	private Transform clone;

	private bool inputAset = false;
	private bool inputBset = false;
	private bool outputSet = false;

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

		clone = Instantiate (block);

		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

	}

	void OnMouseDrag() {

		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

		clone.position = curPosition;

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