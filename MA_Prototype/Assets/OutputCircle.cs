using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputCircle : MonoBehaviour {

	// (As of right now) This class 

	private bool set;
	private LineRenderer lineRenderer = new LineRenderer ();
	private LineRenderer newLineRend = new LineRenderer();
	public Transform origin;
	public GameObject newLineObj;
	private Line line, newLineScript;
	private CircleCollider2D circCol, newCircCol;

	void Awake () {

		origin = GetComponent<Transform> ();

		circCol = GetComponent<CircleCollider2D> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnMouseDrag () {

		line = newLineObj.GetComponent<Line>();

//		line.originCircle = this;
		line.originObject = this.gameObject;

		lineRenderer = newLineObj.gameObject.GetComponent<LineRenderer> ();

		Vector2 screenPos = new Vector2();
		Camera.main.ScreenToWorldPoint (screenPos);

		lineRenderer.SetPosition (0,
			new Vector3 (origin.position.x + (GetComponent<SpriteRenderer>().bounds.size.x)/2,
				origin.position.y,
				origin.position.z));
		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

		Manager.MouseLineScript = newLineScript; // Set reference to current drawn line
		Manager.MouseLineRenderer = newLineRend;
	}

	void OnMouseDown () {

		// instantiate Line after clicking circle
	
		newLineObj = Instantiate (Resources.Load("LinePrefab")) as GameObject;

		if (newLineObj) {
			newLineRend = newLineObj.GetComponent<LineRenderer> ();
			newLineScript = newLineObj.GetComponent<Line> ();
		}
	}
		
	void OnMouseEnter() {

		#region deprecated
		// Contains the logic for creating lines

		// For testing purposes
//		Debug.Log ("Touched circle");
//		set = true;
//		if (circCol.bounds.Contains (newLineRend.GetPosition(1))) {
//			Debug.Log ("Line detected");
//		}
		#endregion deprecated
	}
}