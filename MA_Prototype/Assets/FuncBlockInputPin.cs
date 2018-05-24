using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncBlockInputPin : MonoBehaviour {

	public GameObject connectedLine;
	Collider2D collisionObject;

	void Update() {

//		if (connectedLine) {
//			GetComponent<SpriteRenderer> ().color = Color.green;
//		} else {
//			GetComponent<SpriteRenderer> ().color = Color.white;
//		}
	}

	void OnMouseDrag() {

		if (connectedLine) {

			Vector2 screenPos = new Vector2 ();
			Camera.main.ScreenToWorldPoint (screenPos);

			connectedLine.GetComponent<LineRenderer> ().SetPosition (0,
				new Vector3 (connectedLine.GetComponent<Line> ().originObject.transform.position.x + (GetComponent<SpriteRenderer> ().bounds.size.x) / 2,
					connectedLine.GetComponent<Line> ().originObject.transform.position.y,
					connectedLine.GetComponent<Line> ().originObject.transform.position.z));
			connectedLine.GetComponent<LineRenderer> ().SetPosition (1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10);

			collisionObject = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));

			if (collisionObject && collisionObject.CompareTag ("inputPin")) {
				connectedLine.GetComponent<LineRenderer> ().SetPosition (1, collisionObject.transform.position);
			}
		}
	}

	void OnMouseUp() {

		if (collisionObject && collisionObject.CompareTag ("inputPin")) {
			if (collisionObject.GetComponent<FuncBlockInputPin> ().connectedLine) {
				// Line is already connected
				Destroy(connectedLine.gameObject);
				collisionObject.GetComponent<FuncBlockInputPin> ().connectedLine = connectedLine;
				connectedLine.GetComponent<Line> ().destinObject = collisionObject.gameObject;
				this.connectedLine = null;

			} else {
				// No line connected
				collisionObject.GetComponent<FuncBlockInputPin> ().connectedLine = connectedLine;
				connectedLine.GetComponent<Line> ().destinObject = collisionObject.gameObject;
				this.connectedLine = null;
			}
		} else if (!collisionObject) {
			Destroy (connectedLine);
			connectedLine = null;
			Debug.Log ("Destroyed " + connectedLine);
		}
	}
}
