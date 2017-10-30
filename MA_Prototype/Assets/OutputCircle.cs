using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputCircle : MonoBehaviour {

	// (As of right now) This class 

	private LineRenderer lineRenderer = new LineRenderer ();
		
	public Transform origin, newLine, currentLine;

	private Line line;

	void Awake () {

		origin = GetComponent<Transform> ();

//		lineRenderer = GameObject.Find ("Line").GetComponent<LineRenderer> ();

		line = GameObject.Find ("Line").GetComponent<Line> ();

		line.originCircle = this;

		currentLine = GameObject.Find ("Line").GetComponent<Transform> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDrag () {

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
}