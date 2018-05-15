using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadBoardInputPin : MonoBehaviour {

	GameObject line;
	Vector2[] tempEdges;

	void OnMouseDown () {

		// instantiate Line after clicking circle
		line = Instantiate (Resources.Load("LinePrefab")) as GameObject;
		Manager.currentlyDrawnLine = line.gameObject;
	}

	void OnMouseDrag () {

		if (!Manager.collisionDetected) {
			Vector2 screenPos = new Vector2 ();
			Camera.main.ScreenToWorldPoint (screenPos);

			line.GetComponent<LineRenderer> ().SetPosition (0,
				new Vector3 (transform.position.x + (GetComponent<SpriteRenderer> ().bounds.size.x) / 2,
					transform.position.y,
					transform.position.z));
			line.GetComponent<LineRenderer> ().SetPosition (1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10);

			tempEdges = line.GetComponent<EdgeCollider2D> ().points;
			tempEdges [0] = new Vector2 (
				transform.position.x + (GetComponent<SpriteRenderer> ().bounds.size.x) / 2 - 0.7f,
				transform.position.y - 0.217f);
			tempEdges [1] = new Vector2 (
				(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x - 0.7f,
				(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y - 0.217f);

			line.GetComponent<EdgeCollider2D> ().points = tempEdges;
		}
	}
}
