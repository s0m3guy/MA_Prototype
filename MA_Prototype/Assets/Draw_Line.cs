using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw_Line : MonoBehaviour {

	private LineRenderer lineRenderer = new LineRenderer();

	public Transform origin;
	public Transform destin;

	void Awake () {

		lineRenderer = GetComponent<LineRenderer> ();

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		lineRenderer.SetPosition (0, origin.position);
		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

	}
}
