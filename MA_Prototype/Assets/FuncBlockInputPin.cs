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

			connectedLine.GetComponent<Bezier_Spline>().isEndingPointSnapped = false;

			Vector2 screenPos = new Vector2 ();
			Camera.main.ScreenToWorldPoint (screenPos);

//			connectedLine.GetComponent<LineRenderer> ().SetPosition (0,
//				new Vector3 (connectedLine.GetComponent<Bezier_Spline> ().originObject.transform.position.x + (GetComponent<SpriteRenderer> ().bounds.size.x) / 2,
//					connectedLine.GetComponent<Bezier_Spline> ().originObject.transform.position.y,
//					connectedLine.GetComponent<Bezier_Spline> ().originObject.transform.position.z));
////			connectedLine.GetComponent<LineRenderer>().SetPosition(0, transform.position);
//			connectedLine.GetComponent<LineRenderer> ().SetPosition (1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10);

			collisionObject = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));

			connectedLine.GetComponent<Bezier_Spline>().tangent2.transform.position = new Vector3 (
				//			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x - 2,
				//			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x - GetComponent<CircleCollider2D>().bounds.size.x,
				(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x - 1,
				(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y,
				(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).z);

			connectedLine.GetComponent<Bezier_Spline>().controlPoints[4] = connectedLine.GetComponent<Bezier_Spline>().mouseFollower;
			connectedLine.GetComponent<Bezier_Spline>().controlPoints[5] = connectedLine.GetComponent<Bezier_Spline>().mouseFollower;

			if (collisionObject && (collisionObject.CompareTag ("inputPin")) ||
				collisionObject && collisionObject.CompareTag("outputPin")) {
				connectedLine.GetComponent<Bezier_Spline>().controlPoints[4] = collisionObject.gameObject;
				connectedLine.GetComponent<Bezier_Spline>().controlPoints[5] = collisionObject.gameObject;
			} else if (!collisionObject) {
				connectedLine.GetComponent<Bezier_Spline>().controlPoints[4] = connectedLine.GetComponent<Bezier_Spline>().mouseFollower;
				connectedLine.GetComponent<Bezier_Spline>().controlPoints[5] = connectedLine.GetComponent<Bezier_Spline>().mouseFollower;
			}
		}
	}

	void OnMouseUp() {

		if (collisionObject && (collisionObject.CompareTag("inputPin"))) {
			if (collisionObject.GetComponent<FuncBlockInputPin>().connectedLine) {
				// Line is already connected
				Destroy(collisionObject.GetComponent<FuncBlockInputPin>().connectedLine.gameObject);
				collisionObject.GetComponent<FuncBlockInputPin>().connectedLine = connectedLine;
				connectedLine.GetComponent<Bezier_Spline>().destinObject = collisionObject.gameObject;
				connectedLine.GetComponent<Bezier_Spline>().isEndingPointSnapped = true;
				this.connectedLine = null;

			} else {
				// No line connected
				collisionObject.GetComponent<FuncBlockInputPin>().connectedLine = connectedLine;
				Debug.Log(connectedLine);
				Debug.Log(connectedLine.GetComponent<Bezier_Spline>().destinObject);
				Debug.Log(collisionObject);
				connectedLine.GetComponent<Bezier_Spline>().destinObject = collisionObject.gameObject;
				connectedLine.GetComponent<Bezier_Spline>().isEndingPointSnapped = true;
				this.connectedLine = null;
			}
		} else if (collisionObject && (collisionObject.CompareTag("outputPin"))) {
			if (collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine) {
				// Line is already connected
				Destroy(collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine.gameObject);
				collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine = connectedLine;
				connectedLine.GetComponent<Bezier_Spline>().destinObject = collisionObject.gameObject;
				connectedLine.GetComponent<Bezier_Spline>().isEndingPointSnapped = true;
				this.connectedLine = null;
			} else {
				// No line connected
				collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine = connectedLine;
				connectedLine.GetComponent<Bezier_Spline>().destinObject = collisionObject.gameObject;
				connectedLine.GetComponent<Bezier_Spline>().isEndingPointSnapped = true;
				this.connectedLine = null;
			}
		} else if (!collisionObject
			|| !collisionObject.CompareTag("outputPin")
			|| !collisionObject.CompareTag("inputPin")
			|| !collisionObject.CompareTag("output")) {
			Destroy(connectedLine);
		}
	}
}
