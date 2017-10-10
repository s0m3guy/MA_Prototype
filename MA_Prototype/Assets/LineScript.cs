using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour {

	// (As of right now) This script draws the line following the mouse and checks if the mouse
	// collides with the bounding box of the input of another block

	private LineRenderer line = new LineRenderer();

	private GameObject goalInput = null;
	private GameObject[] goalInputs;
	private CircleCollider2D circCol;
	private CircleCollider2D[] circCols;

	private Transform origin, destin;

	void Awake () {
		goalInputs = GameObject.FindGameObjectsWithTag ("input");
		circCols = new CircleCollider2D[goalInputs.Length];

		for(int i = 0; i < goalInputs.Length; i++) {
			circCols[i] = goalInputs[i].GetComponent<CircleCollider2D>();
		}
			
//		goalInput = GameObject.FindGameObjectWithTag("inputB2");
		line = GetComponent<LineRenderer> ();
//		circCol = goalInput.GetComponent<CircleCollider2D> ();

		origin = GameObject.FindGameObjectWithTag ("output").transform;
//		destin = goalInput.transform;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

		for (int i = 0; i < circCols.Length; i++) {
			if (circCols[i].bounds.Contains (mousePos)) {
				destin = circCols [i].transform;
				line.SetPosition (1, new Vector3 (
					destin.position.x - (destin.GetComponent<SpriteRenderer> ().bounds.size.x) / 2,
					destin.position.y,
					destin.position.z));
			}
		}

//		if (circCol.bounds.Contains(mousePos)) {
//				
//			line.SetPosition (1, new Vector3(
//				destin.position.x - (destin.GetComponent<SpriteRenderer>().bounds.size.x)/2,
//				destin.position.y,
//				destin.position.z));
//		}
	}
}