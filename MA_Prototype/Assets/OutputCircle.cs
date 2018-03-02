﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputCircle : MonoBehaviour {

	// (As of right now) This class 

	private bool set;
	private LineRenderer lineRenderer = new LineRenderer ();
	private LineRenderer newLineRend = new LineRenderer();
	public Transform origin;
	public GameObject newLineObj;
	private Line line, newLineScript;
	private CircleCollider2D circCol, newCircCol;

	private FunctionBlock parentFunctionBlock;

	private Vector2[] tempEdgeColliderPoints;

	void Awake () {

		origin = GetComponent<Transform> ();
		circCol = GetComponent<CircleCollider2D> ();
		parentFunctionBlock = GetComponentInParent<FunctionBlock> ();
		tempEdgeColliderPoints = new Vector2[2];

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown () {
		
		// instantiate Line after clicking circle
		
		if (parentFunctionBlock.checkClone()) {
			newLineObj = Instantiate (Resources.Load ("LinePrefab")) as GameObject;
			if (newLineObj) {
				newLineRend = newLineObj.GetComponent<LineRenderer> ();
				newLineScript = newLineObj.GetComponent<Line> ();
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


		Manager.MouseLineScript = newLineScript; // Set reference to current drawn line
		Manager.MouseLineRenderer = newLineRend;
		Manager.MouseLineEdgeCollider = newLineScript.gameObject.GetComponent<EdgeCollider2D> ();

		// Sets the starting point of the line to this circle
		tempEdgeColliderPoints = Manager.MouseLineEdgeCollider.points;
		tempEdgeColliderPoints [0] = transform.position;
		Manager.MouseLineEdgeCollider.points = tempEdgeColliderPoints;
	}

	void OnMouseUp () {
		if (!newLineScript.isEndingPointSnapped) {
			Destroy (Manager.MouseLineScript.gameObject);
		}
		Manager.MouseLineRenderer = null;
		Manager.MouseLineScript = null;
		Manager.MouseLineEdgeCollider = null;
	}
}