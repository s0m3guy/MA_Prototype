using System.Collections;
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

	Vector3 clampVector;
	BoxCollider2D upperBound, lowerBound;

	void Start() {
		upperBound = GameObject.Find("Upperbound").GetComponent<BoxCollider2D>();
		lowerBound = GameObject.Find("Lowerbound").GetComponent<BoxCollider2D>();
	}

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

		if (connectedLine) {
			GetComponent<SpriteRenderer>().color = connectedLine.GetComponent<LineRenderer>().endColor;
		}
	}

	void OnMouseDrag() {

		if (connectedLine) {

			connectedLine.GetComponent<Bezier_Spline>().isEndingPointSnapped = false;

			Vector2 screenPos = new Vector2 ();
			Camera.main.ScreenToWorldPoint (screenPos);

//			clampVector = new Vector3 ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x,
//				Mathf.Clamp ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y,
//					lowerBound.bounds.max.y,
//					upperBound.bounds.min.y),
//				(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).z);

//			connectedLine.GetComponent<LineRenderer> ().SetPosition (0,
//				new Vector3 (connectedLine.GetComponent<Line> ().originObject.transform.position.x + (GetComponent<SpriteRenderer> ().bounds.size.x) / 2,
//					connectedLine.GetComponent<Bezier_Spline> ().originObject.transform.position.y,
//					connectedLine.GetComponent<Bezier_Spline> ().originObject.transform.position.z));
////			connectedLine.GetComponent<LineRenderer> ().SetPosition (1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10);
//			connectedLine.GetComponent<LineRenderer> ().SetPosition (1, clampVector);

			collisionObject = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));

			connectedLine.GetComponent<Bezier_Spline>().tangent2.transform.position = new Vector3 (
				//			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x - 2,
				//			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x - GetComponent<CircleCollider2D>().bounds.size.x,
				((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x - 1),
				Mathf.Clamp ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y,
					lowerBound.bounds.max.y,
					upperBound.bounds.min.y),
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

		if (collisionObject && collisionObject.CompareTag("inputPin")) {
			if (collisionObject.GetComponent<FuncBlockInputPin>().connectedLine) {
				// Line is already connected
				Destroy(connectedLine.gameObject);
				collisionObject.GetComponent<FuncBlockInputPin>().connectedLine = connectedLine;
				connectedLine.GetComponent<Bezier_Spline>().destinObject = collisionObject.gameObject;
				this.connectedLine = null;

			} else {
				// No line connected
				collisionObject.GetComponent<FuncBlockInputPin>().connectedLine = connectedLine;
				connectedLine.GetComponent<Bezier_Spline>().destinObject = collisionObject.gameObject;
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
