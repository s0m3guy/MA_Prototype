using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputDot : MonoBehaviour {

	// Quick and dirty class for testing of output pins, which are currently
	// connected to a testing LED

	private CircleCollider2D circCol, newCircCol;
	private SpriteRenderer spritRend;
	private Sprite sprite_LED_off, sprite_LED_on;
	public int input;

	private Line line;

	public GameObject connectedLine;

	private Vector3 screenPoint;
	private Vector3 offset;

	void Awake () {

		circCol = GetComponent<CircleCollider2D> ();

		sprite_LED_off = Resources.Load ("LED_off_raw", typeof (Sprite)) as Sprite;
		sprite_LED_on = Resources.Load ("LED_on_raw", typeof(Sprite)) as Sprite;

		spritRend = GameObject.FindGameObjectWithTag ("outputLED").GetComponent<SpriteRenderer> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

		if(circCol.bounds.Contains(mousePos)) {
			if (Manager.currentlyDrawnLine) {
				Manager.currentlyDrawnLine.GetComponent<LineRenderer>().SetPosition (1, this.transform.position);	
				Manager.currentlyDrawnLine.GetComponent<Line>().isEndingPointSnapped = true;
			}
		}

		if (input == 1) {
			spritRend.sprite = sprite_LED_on;
		} else if (input == 0) {
			spritRend.sprite = sprite_LED_off;
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

	void OnMouseEnter() {

		if (Manager.currentlyDrawnLine != null) {
			Manager.currentlyDrawnLine.GetComponent<Line>().destinObject = this.gameObject;
			connectedLine = Manager.currentlyDrawnLine;
		}
	}
}
