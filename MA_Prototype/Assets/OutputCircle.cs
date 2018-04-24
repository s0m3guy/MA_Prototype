using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputCircle : MonoBehaviour {

	// (As of right now) This class 

	private bool set;
	public Transform origin;
	private LineRenderer newLineRend;
	private LineRenderer lineRenderer;
	public GameObject newLineObj;
	private Line line, newLineScript;
	private CircleCollider2D circCol;
	private CircleCollider2D newCircCol;

	private FunctionBlock parentFunctionBlock;

	private Vector2[] tempEdgeColliderPoints;

	public GameObject connectedLine;

	void Awake () {

		origin = GetComponent<Transform> ();
		circCol = GetComponent<CircleCollider2D> ();
		parentFunctionBlock = GetComponentInParent<FunctionBlock> ();
	}

	void OnMouseDown () {
		
		// instantiate Line after clicking circle
		if (parentFunctionBlock.checkClone()) {
			if(connectedLine == null) {
			newLineObj = Instantiate (Resources.Load ("LinePrefab")) as GameObject;
				if (newLineObj) {
					newLineRend = newLineObj.GetComponent<LineRenderer> ();
					newLineScript = newLineObj.GetComponent<Line> ();
				}
			}
		}
	}
	
	void OnMouseDrag () {

		if (newLineObj) {
			line = newLineObj.GetComponent<Line> ();
		}

		line.originObject = this.gameObject;

		lineRenderer = newLineObj.gameObject.GetComponent<LineRenderer> ();

		Vector2 screenPos = new Vector2();
		Camera.main.ScreenToWorldPoint (screenPos);

		lineRenderer.SetPosition (0,
			new Vector3 (origin.position.x + (GetComponent<SpriteRenderer>().bounds.size.x)/2,
				origin.position.y,
				origin.position.z));
		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition)+Vector3.forward*10);

		Manager.currentlyDrawnLine = newLineObj;
		connectedLine = newLineObj;
	}

	void OnMouseUp () {
		if (!newLineScript.isEndingPointSnapped) {
			Destroy (Manager.currentlyDrawnLine.gameObject);
			Manager.currentlyDrawnLine = null;
			newLineObj = null;
			connectedLine = null;
		}
		Manager.currentlyDrawnLine = null;
	}
}