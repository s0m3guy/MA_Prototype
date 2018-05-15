using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncBlockInputPin : MonoBehaviour {

	public GameObject connectedLine;

	void Update() {

		if (connectedLine) {
			GetComponent<SpriteRenderer> ().color = Color.green;
		} else {
			GetComponent<SpriteRenderer> ().color = Color.white;
		}

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

		if (GetComponent<CircleCollider2D> ().bounds.Contains (mousePos)) {
			Manager.collisionDetected = true;
		} else if (!GetComponent<CircleCollider2D> ().bounds.Contains (mousePos)) {
			Manager.collisionDetected = false;
		}


		if (Manager.collisionDetected && Manager.currentlyDrawnLine) {
			Manager.currentlyDrawnLine.GetComponent<LineRenderer> ().SetPosition (1, this.transform.position);

			connectedLine = Manager.currentlyDrawnLine;
			connectedLine.GetComponent<Line> ().destinObject = this.gameObject;
		} else if (!Manager.collisionDetected) {

			if (connectedLine) {
				connectedLine.GetComponent<Line> ().destinObject = null;
			}
			connectedLine = null;
		}
	}
}
