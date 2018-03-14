using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCircle : MonoBehaviour {

	// (As of right now) This class controls the behaviour of the input circles in the function blocks

	private bool set;
	private LineRenderer lineRenderer = new LineRenderer ();
	private LineRenderer newLineRend = new LineRenderer();
	public Transform origin;
	private GameObject newLine;
	private Line line;
	private CircleCollider2D circCol, newCircCol;
	private Vector2[] tempEdgeColliderPoints;

	public GameObject connectedLine;

	private Vector3 screenPoint;
	private Vector3 offset;

	private OutputCircle outputCircle = new OutputCircle ();

	void Awake () {

		origin = GetComponent<Transform> ();
		circCol = GetComponent<CircleCollider2D> ();
		tempEdgeColliderPoints = new Vector2[2];
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

		// If mouse enters bounds and there is a line being drawn, snap end point of line to this
		if(circCol.bounds.Contains(mousePos)) {
			if (Manager.currentlyDrawnLine) {
				Manager.currentlyDrawnLine.GetComponent<LineRenderer>().SetPosition (1, this.transform.position);
				#region unused collider shit
				// Also set end point of Edge Collider
//				tempEdgeColliderPoints = Manager.MouseLineEdgeCollider.points;
//				tempEdgeColliderPoints [1] = transform.position;
//				Manager.MouseLineEdgeCollider.points = tempEdgeColliderPoints;
				#endregion optional collider shit
				Manager.currentlyDrawnLine.GetComponent<Line>().isEndingPointSnapped = true;
			}
		}
	}
		
	void OnMouseEnter() {

		// Set this input circle as destin object in line
		if (Manager.currentlyDrawnLine != null) {
			Manager.currentlyDrawnLine.GetComponent<Line>().destinObject = this.gameObject;
			connectedLine = Manager.currentlyDrawnLine;
		}
	}

	void OnMouseDown() {

		// Let go of the line in case there is one
		if (connectedLine != null) {
			connectedLine.GetComponent<Line> ().unSnap ();
		}
	}

	void OnMouseDrag() {

		// Have the now semi-loose line follow the mouse

		Vector2 screenPos = new Vector2 ();
		Camera.main.ScreenToWorldPoint (screenPos);

		connectedLine.GetComponent<LineRenderer>().SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition)+Vector3.forward*10);

		Debug.Log (connectedLine.GetComponent<LineRenderer>().GetPosition(1));

		Manager.currentlyDrawnLine = connectedLine; // Set reference to current drawn line
	}

	void OnMouseUp() {

		if (!connectedLine.GetComponent<Line> ().isEndingPointSnapped) {
			Destroy (connectedLine.gameObject);
			connectedLine = null;
		}

		Manager.currentlyDrawnLine = null;
	}
}