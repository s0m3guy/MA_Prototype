using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInputDotSwitchable : MonoBehaviour {

	public int value = 0;
	private SpriteRenderer spritRend;
	private Sprite sprite_dot_off, sprite_dot_on;

	private Line line, newLineScript;
	private LineRenderer lineRenderer = new LineRenderer ();
	private LineRenderer newLineRend = new LineRenderer();
	public GameObject newLineObj;
	public Transform origin;

	public int[] outputs;

	void Awake () {
		spritRend = gameObject.GetComponent<SpriteRenderer> ();
		sprite_dot_off = Resources.Load ("connecting_dot_inactive", typeof (Sprite)) as Sprite;
		sprite_dot_on = Resources.Load ("connecting_dot_active", typeof(Sprite)) as Sprite;

		origin = GetComponent<Transform> ();

		outputs = new int[1];
	}

	// Use this for initialization
	void Start () {
//		InvokeRepeating("SwitchDot", 2.0f, 2.0f);
	}

	// Update is called once per frame
	void Update () {
		if (value == 1) {
			spritRend.sprite = sprite_dot_on;
		} else if (value == 0) {
			spritRend.sprite = sprite_dot_off;
		}

		forwardInput (value, outputs);
	}

	private void SwitchDot () {

		if (value == 1) {
			value = 0;
		} else if (value == 0) {
			value = 1;
		}
	}

	void OnMouseDrag () {

		line = newLineObj.GetComponent<Line>();

		line.originObject = this.gameObject;

		lineRenderer = newLineObj.gameObject.GetComponent<LineRenderer> ();

		Vector2 screenPos = new Vector2();
		Camera.main.ScreenToWorldPoint (screenPos);

		lineRenderer.SetPosition (0,
			new Vector3 (origin.position.x + (GetComponent<SpriteRenderer>().bounds.size.x)/2,
				origin.position.y,
				origin.position.z));
		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition)+Vector3.forward*10);

//		Manager.MouseLineEdgeCollider.points [0] = transform.position;
	}

	void OnMouseDown () {

		if (value == 0) {
			value = 1;
		} else if (value == 1) {
			value = 0;
		}

		// Instantiate Line after clicking circle
		newLineObj = Instantiate (Resources.Load("LinePrefab")) as GameObject;

		if (newLineObj) {
			newLineRend = newLineObj.GetComponent<LineRenderer> ();
			newLineScript = newLineObj.GetComponent<Line> ();
		}

		// Set references to current drawn line in manager
//		Manager.currentlyDrawnLine = newLineScript; 	
//		Manager.MouseLineRenderer = newLineRend;
//		Manager.MouseLineEdgeCollider = newLineScript.gameObject.GetComponent<EdgeCollider2D> ();
		Manager.currentlyDrawnLine = newLineObj;
	}

	public void forwardInput (int input, int[] outputs) {
		for (int i = 0; i <= outputs.Length-1; i++) {
			outputs[i] = value;
		}
	}

	void OnMouseUp () {
		if (!newLineScript.isEndingPointSnapped) {
			Destroy (Manager.currentlyDrawnLine.gameObject);
		}
//		Manager.MouseLineRenderer = null;
//		Manager.currentlyDrawnLine = null;
//		Manager.MouseLineEdgeCollider = null;

		Manager.currentlyDrawnLine = null;
	}
}
