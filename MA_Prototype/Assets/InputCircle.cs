﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCircle : MonoBehaviour {

	// (As of right now) This class 

	private bool set;
	private LineRenderer lineRenderer = new LineRenderer ();
	private LineRenderer newLineRend = new LineRenderer();
	public Transform origin;
	private GameObject newLine;
	private Line line;
	private CircleCollider2D circCol, newCircCol;
	private Vector2[] tempEdgeColliderPoints;

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

		if(circCol.bounds.Contains(mousePos)) {
			if (Manager.MouseLineRenderer) {
				Manager.MouseLineRenderer.SetPosition (1, this.transform.position);
				// Also set end point of Edge Collider
				tempEdgeColliderPoints = Manager.MouseLineEdgeCollider.points;
				tempEdgeColliderPoints [1] = transform.position;
				Manager.MouseLineEdgeCollider.points = tempEdgeColliderPoints;			
			}
		}
	}
		
	void OnMouseEnter() {

		if (Manager.MouseLineScript != null) {
			Manager.MouseLineScript.destinObject = this.gameObject;
		}
	}
}