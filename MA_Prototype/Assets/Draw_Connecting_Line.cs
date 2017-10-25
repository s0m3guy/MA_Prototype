using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw_Connecting_Line : MonoBehaviour {

	// (As of right now) This class 

	private LineRenderer lineRenderer = new LineRenderer ();
		
	public Transform origin;

	private LineScript line;

	void Awake () {

		origin = GetComponent<Transform> ();

		lineRenderer = GameObject.Find ("Line").GetComponent<LineRenderer> ();

		line = GameObject.Find ("Line").GetComponent<LineScript> ();

		line.originCircle = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDrag () {

		Vector2 screenPos = new Vector2();
		Camera.main.ScreenToWorldPoint (screenPos);

		lineRenderer.SetPosition (0,
			new Vector3 (origin.position.x + (GetComponent<SpriteRenderer>().bounds.size.x)/2,
				origin.position.y,
				origin.position.z));
		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition));


	}
}