using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCircle_Old : MonoBehaviour {

	// (As of right now) This class controls the behaviour of the input circles in the function blocks

	private bool set;
//	private LineRenderer lineRenderer = new LineRenderer ();
//	private LineRenderer newLineRend = new LineRenderer();
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

//		// If mouse enters bounds and there is a line being drawn, snap end point of line to this
//		if (circCol.bounds.Contains(mousePos)) {																		// if mouse is inside collider bounds
//			if (Manager.currentlyDrawnLine) {																			// if there is a line being drawn
//				Manager.currentlyDrawnLine.GetComponent<LineRenderer>().SetPosition(1, this.transform.position);		// 
//				Manager.currentlyDrawnLine.GetComponent<Line>().isEndingPointSnapped = true;
//			}
//		} else if (!circCol.bounds.Contains(mousePos)) {
//			Manager.currentlyDrawnLine.GetComponent<Line>().isEndingPointSnapped = false;
//		}
	}

	void OnTriggerStay2D(Collider2D other) {
	
		Debug.Log("Staying inside collider");

	}
		
	void OnMouseEnter() {

//		// Set this input circle as destin object in line
//		if (!connectedLine) {
//			if (Manager.currentlyDrawnLine != null) {
//				Manager.currentlyDrawnLine.GetComponent<Line>().destinObject = this.gameObject;
//				connectedLine = Manager.currentlyDrawnLine;
//			}
//		} else if (connectedLine) {
//			if (Manager.currentlyDrawnLine != null) {
//				Manager.currentlyDrawnLine.GetComponent<Line>().destinObject = this.gameObject;
//				Destroy (connectedLine.gameObject);
//				connectedLine = Manager.currentlyDrawnLine;
//			}
//		}

	}

	void OnMouseExit() {

//		Vector2 screenPos = new Vector2 ();
//		Camera.main.ScreenToWorldPoint (screenPos);
//
//		Manager.currentlyDrawnLine.GetComponent<LineRenderer> ().SetPosition (1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10);
	}

	void OnMouseDown() {

		// Let go of the line in case there is one
		if (connectedLine != null) {
			connectedLine.GetComponent<Line> ().unSnap ();
		}
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

//		if (!connectedLine.GetComponent<Line> ().isEndingPointSnapped) {
//			Destroy (connectedLine.gameObject);
//			connectedLine = null;
//		}
//
//		Manager.currentlyDrawnLine = null;
	}
}