using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBoundTest : MonoBehaviour {

	private BoxCollider2D boxCol;

	void Awake () {
		boxCol = GetComponent<BoxCollider2D> ();
	}


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Debug.Log (mousePos);

		if (boxCol.bounds.Contains(mousePos)) {
			Debug.Log ("HALLLOOOOO");
		}
	}
}