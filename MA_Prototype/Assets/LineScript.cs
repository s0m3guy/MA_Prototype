﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour {

	private LineRenderer line = new LineRenderer();
	private GameObject goalInput;
	private BoxCollider2D boxCol;
	private Rect goalBox;
	private Transform origin, destin;


	void Awake () {
		goalInput = GameObject.FindGameObjectWithTag("inputB1");
		line = GetComponent<LineRenderer> ();
		boxCol = goalInput.GetComponent<BoxCollider2D> ();

		origin = GameObject.FindGameObjectWithTag ("outputA").transform;
		destin = goalInput.transform;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

		if (boxCol.bounds.Contains(mousePos)) {
			line.SetPosition (1, new Vector3(
				destin.position.x - (destin.GetComponent<SpriteRenderer>().bounds.size.x)/2,
				destin.position.y,
				destin.position.z));
		}
	}

	void OnCollisionEnter2D (Collision2D col) {

		if (col.gameObject.CompareTag("inputB1")) {
			Debug.Log ("COLLISION!!!1111");
			Destroy (col.gameObject);
		}
	}
}