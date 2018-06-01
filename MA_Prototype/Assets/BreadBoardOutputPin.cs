﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadBoardOutputPin : MonoBehaviour {

	public GameObject connectedLine;
	Collider2D collisionObject;
	CircleCollider2D circCol;
	Sprite sprite_LED_off, sprite_LED_on;

	[SerializeField]
	SpriteRenderer spritRend;

	public float input;

	void Awake() {

		circCol = GetComponent<CircleCollider2D>();

		sprite_LED_off = Resources.Load("led/LED_off2", typeof(Sprite)) as Sprite;
		sprite_LED_on = Resources.Load("led/LED_on2", typeof(Sprite)) as Sprite;
	}

	void Update() {

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

	void OnMouseDrag() {

		if (connectedLine) {

			connectedLine.GetComponent<Line>().isEndingPointSnapped = false;

			Vector2 screenPos = new Vector2 ();
			Camera.main.ScreenToWorldPoint (screenPos);

			connectedLine.GetComponent<LineRenderer> ().SetPosition (0,
				new Vector3 (connectedLine.GetComponent<Line> ().originObject.transform.position.x + (GetComponent<SpriteRenderer> ().bounds.size.x) / 2,
					connectedLine.GetComponent<Line> ().originObject.transform.position.y,
					connectedLine.GetComponent<Line> ().originObject.transform.position.z));
			connectedLine.GetComponent<LineRenderer> ().SetPosition (1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10);

			collisionObject = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));

			if (collisionObject && (collisionObject.CompareTag ("inputPin")) ||
				collisionObject && collisionObject.CompareTag("outputPin")) {
				connectedLine.GetComponent<LineRenderer> ().SetPosition (1, collisionObject.transform.position);
			}
		}
	}

	void OnMouseUp() {

		if (collisionObject && collisionObject.CompareTag("inputPin")) {
			if (collisionObject.GetComponent<FuncBlockInputPin>().connectedLine) {
				// Line is already connected
				Destroy(connectedLine.gameObject);
				collisionObject.GetComponent<FuncBlockInputPin>().connectedLine = connectedLine;
				connectedLine.GetComponent<Line>().destinObject = collisionObject.gameObject;
				this.connectedLine = null;

			} else {
				// No line connected
				collisionObject.GetComponent<FuncBlockInputPin>().connectedLine = connectedLine;
				connectedLine.GetComponent<Line>().destinObject = collisionObject.gameObject;
				this.connectedLine = null;
			}
		} else if (collisionObject && (collisionObject.CompareTag("outputPin"))) {
			if (collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine) {
				// Line is already connected
				Destroy(collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine.gameObject);
				collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine = connectedLine;
				connectedLine.GetComponent<Line>().destinObject = collisionObject.gameObject;
				connectedLine.GetComponent<Line>().isEndingPointSnapped = true;
				this.connectedLine = null;
			} else {
				// No line connected
				collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine = connectedLine;
				connectedLine.GetComponent<Line>().destinObject = collisionObject.gameObject;
				connectedLine.GetComponent<Line>().isEndingPointSnapped = true;
				this.connectedLine = null;
			}
		} else if (!collisionObject) {
			Destroy (connectedLine);
			connectedLine = null;
			Debug.Log ("Destroyed " + connectedLine);
		}
	}
}
