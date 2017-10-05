using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour {

	GameObject bridge;

	public int speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnCollisionEnter2D (Collision2D col) {
		Debug.Log ("Collision");
		if (col.gameObject.tag == "bridge") {
			Destroy (gameObject);
		}
	}

	private Vector3 screenPoint;
	private Vector3 offset;

	void OnMouseDown() {

		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

	}

	void OnMouseDrag() {

		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

		Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

		transform.position = curPosition;

	}
}