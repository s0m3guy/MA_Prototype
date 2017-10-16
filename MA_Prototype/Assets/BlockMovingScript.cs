using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovingScript: MonoBehaviour {

	public Transform block;
	public int speed;
	private bool isClone = false;
	private Transform clone;

	// Use this for initialization
	void Start () {

		if (gameObject.name.Contains ("(Clone)")) {
			isClone = true;
		}
			
		block = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
//		if (gameObject.name.Contains ("(Clone)")) {
//			Debug.Log ("KLOOON");
//		}
	}

	private Vector3 screenPoint;
	private Vector3 offset;

	void OnMouseDown() {

//		if (!isClone) {
			clone = Instantiate (block);
//		}

		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

	}

	void OnMouseDrag() {

		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

//		transform.position = curPosition;
		clone.position = curPosition;

	}
}