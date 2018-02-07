using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCircle : MonoBehaviour {

	// (As of right now) This class 

	private bool set;
	private LineRenderer lineRenderer = new LineRenderer ();
	private LineRenderer newLineRend = new LineRenderer();
	public Transform origin;
	private GameObject newLine;
	private Line line;
	private CircleCollider2D circCol, newCircCol;

	private OutputCircle outputCircle = new OutputCircle ();

	void Awake () {

		origin = GetComponent<Transform> ();

		circCol = GetComponent<CircleCollider2D> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

		if(circCol.bounds.Contains(mousePos)) {
			Manager.MouseLineRenderer.SetPosition (1, this.transform.position);
		}
	}
			
			

	void OnMouseDrag () {

		#region currently deprecated
//		line = newLine.GetComponent<Line>();
//
////		line.originCircle = this;		// needs fix
//
//		lineRenderer = newLine.gameObject.GetComponent<LineRenderer> ();
//
//		Vector2 screenPos = new Vector2();
//		Camera.main.ScreenToWorldPoint (screenPos);
//
//		lineRenderer.SetPosition (0,
//			new Vector3 (origin.position.x + (GetComponent<SpriteRenderer>().bounds.size.x)/2,
//				origin.position.y,
//				origin.position.z));
//		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
		#endregion currently deprecated
	}

	void OnMouseDown () {

		// instantiate Line after clicking circle
	
		newLine = Instantiate (Resources.Load("LinePrefab")) as GameObject;
		newLineRend = newLine.GetComponent<LineRenderer> ();

	}
		
	void OnMouseEnter() {

		if (Manager.MouseLineScript != null) {
			Manager.MouseLineScript.destinObject = this.gameObject;

//			Debug.Log (Manager.MouseLineScript.destinObject.name);
		}
	}
}