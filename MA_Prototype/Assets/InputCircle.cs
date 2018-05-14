using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCircle : MonoBehaviour {

	// (As of right now) This class controls the behaviour of the input circles in the function blocks

	private bool set;
	public Transform origin;
	private GameObject newLine;
	private Line line;
	private CircleCollider2D circCol, newCircCol;

	public GameObject connectedLine;

	private Vector3 screenPoint;
	private Vector3 offset;

	private OutputCircle outputCircle;

	void Awake () {

		origin = GetComponent<Transform> ();
		circCol = GetComponent<CircleCollider2D> ();
	}

	// Update is called once per frame
	void Update () {
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

		// Check if is in bounds of input circle
		if (GetComponent<CircleCollider2D> ().bounds.Contains (mousePos)) {
			Manager.collisionDetected = true;
		} else if (!GetComponent<CircleCollider2D> ().bounds.Contains (mousePos)) {
			Manager.collisionDetected = false;
		}





		// Inside the collision bounds
		if (Manager.collisionDetected && Manager.currentlyDrawnLine) { 
			Manager.currentlyDrawnLine.GetComponent<LineRenderer> ().SetPosition (1, this.transform.position);

			Manager.currentlyDrawnLine.GetComponent<Line>().destinObject = this.gameObject;
			connectedLine = Manager.currentlyDrawnLine;		// works

		// Outside the collision bounds
		} else if (!Manager.collisionDetected && Manager.currentlyDrawnLine) {

			Manager.currentlyDrawnLine.GetComponent<Line>().destinObject = null;
			connectedLine = null;							// works
		}
	}
		

	void OnMouseDown() {

	}

	void OnMouseDrag() {

		// Have the now semi-loose line follow the mouse
		if (connectedLine) {

			connectedLine.GetComponent<Line> ().unSnap ();

			if (transform.name.Contains ("Input 1")) {
				transform.parent.GetComponent<FunctionBlock> ().inputs [0] = 0;
			} else if (transform.name.Contains ("Input 2")) {
				transform.parent.GetComponent<FunctionBlock> ().inputs [1] = 0;
			}

			Vector2 screenPos = new Vector2 ();
			Camera.main.ScreenToWorldPoint (screenPos);

			connectedLine.GetComponent<LineRenderer> ().SetPosition (1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10);
		}

		Manager.currentlyDrawnLine = connectedLine; // Set reference to current drawn line
	}

	void OnMouseUp() {

	}
}