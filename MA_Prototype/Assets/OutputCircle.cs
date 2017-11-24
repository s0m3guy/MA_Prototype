using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputCircle : MonoBehaviour {

	// (As of right now) This class 

	private bool set;
	private LineRenderer lineRenderer = new LineRenderer ();
	private LineRenderer newLineRend = new LineRenderer();
	public Transform origin;
	public GameObject newLine;
	private Line line;
	private CircleCollider2D circCol, newCircCol;
	private EdgeCollider2D edgeCol;

	private List<Vector2> newVertices = new List<Vector2> () {
		new Vector2 (0, 0),
		new Vector2 (0, 0)
	};

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

		line = newLine.GetComponent<Line>();

		line.originCircle = this;

		lineRenderer = newLine.gameObject.GetComponent<LineRenderer> ();
		edgeCol = newLine.gameObject.GetComponent<EdgeCollider2D> ();

		Vector2 screenPos = new Vector2();
		Camera.main.ScreenToWorldPoint (screenPos);

		lineRenderer.SetPosition (0,
			new Vector3 (origin.position.x + (GetComponent<SpriteRenderer>().bounds.size.x)/2,
				origin.position.y,
				origin.position.z));

		newVertices[0] = new Vector2 (origin.position.x + (GetComponent<SpriteRenderer>().bounds.size.x)/2, origin.position.y);

		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

		newVertices[1] = new Vector2 (Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

		edgeCol.points = newVertices.ToArray ();
	}

	void OnMouseDown () {

		// instantiate Line after clicking circle
	
		newLine = Instantiate (Resources.Load("LinePrefab")) as GameObject;
		newLineRend = newLine.GetComponent<LineRenderer> ();
	}
		
	void OnMouseEnter() {

		// Contains the logic for creating lines

		// For testing purposes
//		Debug.Log ("Touched circle");
//		set = true;
//		if (circCol.bounds.Contains (newLineRend.GetPosition(1))) {
//			Debug.Log ("Line detected");
//		}
	}
}