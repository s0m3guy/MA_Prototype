using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncBlockOutputPin : MonoBehaviour {

	GameObject line;
	public GameObject connectedLine;
	Collider2D overlappedCollider;

	Vector3 clampVector;
	BoxCollider2D upperBound, lowerBound;

	// Use this for initialization
	void Start () {
		upperBound = GameObject.Find("Upperbound").GetComponent<BoxCollider2D>();
		lowerBound = GameObject.Find("Lowerbound").GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown () {

		// instantiate Line after clicking circle
		line = Instantiate (Resources.Load("LinePrefab")) as GameObject;
		line.name = "Line_(" + line.GetHashCode() + ")";
		line.GetComponent<Bezier_Spline> ().originObject = this.gameObject;

//		Manager.currentlyDrawnLine = newLineObj;
	}

	void OnMouseDrag () {

		line.GetComponent<LineRenderer>().enabled = true;

		Vector2 screenPos = new Vector2 ();
		Camera.main.ScreenToWorldPoint (screenPos);

		clampVector = new Vector3 ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x,
			Mathf.Clamp ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y,
				lowerBound.bounds.max.y,
				upperBound.bounds.min.y),
			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).z);

//		line.GetComponent<LineRenderer> ().SetPosition (0,
//			new Vector3 (transform.position.x + (GetComponent<SpriteRenderer> ().bounds.size.x) / 2,
//				transform.position.y,
//				transform.position.z));
		line.GetComponent<LineRenderer>().SetPosition(0, transform.position);
//		line.GetComponent<LineRenderer> ().SetPosition (1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10);
		line.GetComponent<LineRenderer> ().SetPosition (1, clampVector);

		overlappedCollider = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));

		line.GetComponent<Bezier_Spline>().tangent2.transform.position = new Vector3 (
			//			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x - 2,
			((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x - GetComponent<CircleCollider2D>().bounds.size.x/2),
			Mathf.Clamp ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y,
				lowerBound.bounds.max.y,
				upperBound.bounds.min.y),
			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).z);

		if ((overlappedCollider && overlappedCollider.CompareTag("inputPin")) ||
			overlappedCollider && overlappedCollider.CompareTag("outputPin")) {
			line.GetComponent<Bezier_Spline>().controlPoints[4] = overlappedCollider.gameObject;
			line.GetComponent<Bezier_Spline>().controlPoints[5] = overlappedCollider.gameObject;
		} else if (!overlappedCollider) {
			line.GetComponent<Bezier_Spline>().controlPoints[4] = line.GetComponent<Bezier_Spline>().mouseFollower;
			line.GetComponent<Bezier_Spline>().controlPoints[5] = line.GetComponent<Bezier_Spline>().mouseFollower;
		}
	}

	void OnMouseUp() {
		if (overlappedCollider && overlappedCollider.CompareTag("inputPin")) {
			if (overlappedCollider.GetComponent<FuncBlockInputPin>().connectedLine) {
				// Line is already connected
				Destroy(overlappedCollider.GetComponent<FuncBlockInputPin>().connectedLine.gameObject);
				overlappedCollider.GetComponent<FuncBlockInputPin>().connectedLine = line;
				line.GetComponent<Bezier_Spline>().destinObject = overlappedCollider.gameObject;
				line.GetComponent<Bezier_Spline>().isEndingPointSnapped = true;
				line.GetComponent<Bezier_Spline>().originObject = this.gameObject;
				connectedLine = line;
			} else {
				// No line connected
				overlappedCollider.GetComponent<FuncBlockInputPin>().connectedLine = line;
				line.GetComponent<Bezier_Spline>().destinObject = overlappedCollider.gameObject;
				line.GetComponent<Bezier_Spline>().isEndingPointSnapped = true;
				line.GetComponent<Bezier_Spline>().originObject = this.gameObject;
				connectedLine = line;
			}
		} else if (overlappedCollider && overlappedCollider.CompareTag("outputPin")) {
			if (overlappedCollider.GetComponent<BreadBoardOutputPin>().connectedLine) {
				Destroy(overlappedCollider.GetComponent<BreadBoardOutputPin>().connectedLine.gameObject);
				overlappedCollider.GetComponent<BreadBoardOutputPin>().connectedLine = line;
				line.GetComponent<Bezier_Spline>().destinObject = overlappedCollider.gameObject;
				line.GetComponent<Bezier_Spline>().isEndingPointSnapped = true;
				line.GetComponent<Bezier_Spline>().originObject = this.gameObject;
				connectedLine = line;
			} else {
				overlappedCollider.GetComponent<BreadBoardOutputPin>().connectedLine = line;
				line.GetComponent<Bezier_Spline>().destinObject = overlappedCollider.gameObject;
				line.GetComponent<Bezier_Spline>().isEndingPointSnapped = true;
				line.GetComponent<Bezier_Spline>().originObject = this.gameObject;
				connectedLine = line;
			}
		} else if (!overlappedCollider
		           || !overlappedCollider.CompareTag("outputPin")
		           || !overlappedCollider.CompareTag("inputPin")
		           || !overlappedCollider.CompareTag("output")) {
			Destroy(line);
			Debug.Log("Destroyed " + line);
		}
	}

}
