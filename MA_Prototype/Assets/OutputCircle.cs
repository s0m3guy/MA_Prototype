﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputCircle : MonoBehaviour {

	// (As of right now) This class 

	private bool set;
	private LineRenderer lineRenderer = new LineRenderer ();
	private LineRenderer newLineRend = new LineRenderer();
	public Transform origin;
	public GameObject newLine;
	private Line line;
	private CircleCollider2D circCol, newCircCol;

	void Awake () {

		origin = GetComponent<Transform> ();

		circCol = GetComponent<CircleCollider2D> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDrag () {

		line = newLine.GetComponent<Line>();

		line.originCircle = this;

		lineRenderer = newLine.gameObject.GetComponent<LineRenderer> ();

		Vector2 screenPos = new Vector2();
		Camera.main.ScreenToWorldPoint (screenPos);

		lineRenderer.SetPosition (0,
			new Vector3 (origin.position.x + (GetComponent<SpriteRenderer>().bounds.size.x)/2,
				origin.position.y,
				origin.position.z));
		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	void OnMouseDown () {

		// instantiate Line after clicking circle
	
		newLine = Instantiate (Resources.Load("LinePrefab")) as GameObject;
		newLineRend = newLine.GetComponent<LineRenderer> ();
	}
		
	void OnMouseEnter() {

		// Contains the logic for creating lines

		// For testing purposes
//		Debug.Log ("Touched circle");
//		set = true;
//		if (circCol.bounds.Contains (newLineRend.GetPosition(1))) {
//			Debug.Log ("Line detected");
//		}
	}
}