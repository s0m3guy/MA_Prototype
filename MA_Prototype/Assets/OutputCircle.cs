using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputCircle : MonoBehaviour {

	// (As of right now) This class 

	private bool set;
	private LineRenderer lineRenderer = new LineRenderer ();
	public Transform origin, newLine, currentLine;
	private Line line;
	private CircleCollider2D circCol;

	void Awake () {

		origin = GetComponent<Transform> ();

//		lineRenderer = GameObject.Find ("Line").GetComponent<LineRenderer> (); 	// Deprecated

//		line = GameObject.Find ("Line").GetComponent<Line> ();					// Deprecated
		currentLine = GameObject.Find ("Line").GetComponent<Transform> ();

		circCol = GetComponent<CircleCollider2D> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log (lineRenderer.GetPosition (1));
	}

	void OnMouseDrag () {

		line = newLine.GetComponent<Line>();

		line.originCircle = this;

		lineRenderer = newLine.gameObject.GetComponent<LineRenderer> ();

		Vector2 screenPos = new Vector2();
		Camera.main.ScreenToWorldPoint (screenPos);

		lineRenderer.SetPosition (0,
			new Vector3 (origin.position.x + (GetComponent<SpriteRenderer>().bounds.size.x)/2,
				origin.position.y,
				origin.position.z));
		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

	void OnMouseDown () {

		// instantiate Line after clicking circle
	
		newLine = Instantiate (currentLine);

	}

	void OnMouseOver() {
//		GameObject hitTest = getGameObjectAtPosition ();
	}

	GameObject getGameObjectAtPosition()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
			Debug.Log("found " + hit.collider.name + " at distance: " + hit.distance);
		return hit.collider.gameObject;
	}

	void OnMouseEnter() {

		// For testing purposes
//		Debug.Log ("Hast mich!");
//		set = true;

//		if (circCol.bounds.Contains (lineRenderer.GetPosition(1))) {
//			Debug.Log ("Linie detektiert");
//		}
	}
}