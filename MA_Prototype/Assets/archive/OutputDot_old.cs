﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputDot : MonoBehaviour {

	private CircleCollider2D circCol, newCircCol;
	[SerializeField]
	private SpriteRenderer spritRend;
	private Sprite sprite_LED_off, sprite_LED_on;
	public float input;

	private Line line;

	public GameObject connectedLine;

	private Vector3 screenPoint;
	private Vector3 offset;

	Color lerpedColor;

	void Awake () {

		circCol = GetComponent<CircleCollider2D> ();

		sprite_LED_off = Resources.Load ("led/LED_off2", typeof (Sprite)) as Sprite;
		sprite_LED_on = Resources.Load ("led/LED_on2", typeof(Sprite)) as Sprite;

//		spritRend = GameObject.FindGameObjectWithTag ("outputLED").GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

//		if(circCol.bounds.Contains(mousePos)) {
//			if (Manager.currentlyDrawnLine) {
//				Manager.currentlyDrawnLine.GetComponent<LineRenderer>().SetPosition (1, this.transform.position);	
//				Manager.currentlyDrawnLine.GetComponent<Line>().isEndingPointSnapped = true;
//			}
//		}

		if (transform.parent.name.Contains("LED")) {

			spritRend.color = Color.Lerp(new Color(186 / 255f, 180 / 255f, 180 / 255f, 255 / 255f), new Color(1, 0.302f, 0.208f, 1.000f), input / 5f);

		} else if (transform.parent.name.Contains("Gears")) {

			if (input != 0) {
				transform.parent.GetComponent<RotateGear>().speed = input + 2.5f;
			} else {
				transform.parent.GetComponent<RotateGear>().speed = 0;
			}

			if (connectedLine == null) {
				transform.parent.GetComponent<RotateGear>().speed = 0;
			}
		}

		if (connectedLine == null) {
			input = 0;
		}
	}

	void OnMouseDown() {

		if (connectedLine != null) {
			connectedLine.GetComponent<Line> ().unSnap ();
		}
	}

	void OnMouseDrag() {

		if (connectedLine) {
			connectedLine.GetComponent<Line> ().unSnap ();

			input = 0;

			Vector2 screenPos = new Vector2 ();
			Camera.main.ScreenToWorldPoint (screenPos);

			connectedLine.GetComponent<LineRenderer> ().SetPosition (1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10);
		}

		Manager.currentlyDrawnLine = connectedLine; // Set reference to current drawn line
	}

	void OnMouseUp() {

		if (!connectedLine.GetComponent<Line> ().isEndingPointSnapped) {
			Destroy (connectedLine.gameObject);
			connectedLine = null;
		}

		Manager.currentlyDrawnLine = null;
	}

	void OnMouseEnter() {

		if (Manager.currentlyDrawnLine != null) {
			Manager.currentlyDrawnLine.GetComponent<Line>().destinObject = this.gameObject;
			connectedLine = Manager.currentlyDrawnLine;
		}
	}
}
