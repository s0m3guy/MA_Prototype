using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

	// (As of right now) This script draws the line following the mouse and checks if the mouse
	// collides with the bounding box of the input of another block

	private LineRenderer line = new LineRenderer();

	private GameObject goalInput;
	private GameObject[] goalInputs;
	private GameObject[] goalInputs2;
	private CircleCollider2D circCol;
	private CircleCollider2D[] circCols;

	private Transform origin, destin;

	public OutputCircle originCircle;

	private FunctionBlock originBlockScript, destinBlockScript;

	void Awake () {
		goalInputs = GameObject.FindGameObjectsWithTag ("input");
		circCols = new CircleCollider2D[goalInputs.Length];

		for(int i = 0; i < goalInputs.Length; i++) {
			circCols[i] = goalInputs[i].GetComponent<CircleCollider2D>();
		}
			
		line = GetComponent<LineRenderer> ();
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		ScanInput ();

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

		// Run through all circle colliders and check 

		for (int i = 0; i < circCols.Length; i++) {
			if (circCols[i].bounds.Contains (mousePos)) {
				destin = circCols [i].transform;
				line.SetPosition (1, new Vector3 (
					destin.position.x - (destin.GetComponent<SpriteRenderer> ().bounds.size.x) / 2,
					destin.position.y,
					destin.position.z));

				if (circCols [i].CompareTag ("inputA")) {
					destinBlockScript = circCols [i].GetComponentInParent<FunctionBlock> ();
					destinBlockScript.setInputA ();
				} else if (circCols [i].CompareTag ("inputB")) {
					destinBlockScript = circCols [i].GetComponentInParent<FunctionBlock> ();
					destinBlockScript.setInputB ();
				}
				originCircle.GetComponentInParent<FunctionBlock> ().setOutput ();
			}
		}
	}

	void ScanInput () {
		goalInputs = FindGameObjectsWithDifferentTags(new string[] {"inputA", "inputB"});

		circCols = new CircleCollider2D[goalInputs.Length];

		for (int i = 0; i < goalInputs.Length; i++) {
			circCols [i] = goalInputs [i].GetComponent<CircleCollider2D> ();
		}
	}

	public static GameObject[] FindGameObjectsWithDifferentTags(string[] tags) {
		List<GameObject> list = new List<GameObject> ();
		foreach (string tag in tags) {
			GameObject[] objs = GameObject.FindGameObjectsWithTag (tag);
			list.AddRange (objs);
		}
		return list.ToArray ();
	}
}