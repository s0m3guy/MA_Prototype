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

//		line.transform.localScale += new Vector3 (0.1F, 0, 0);
//		lineRenderer.SetPosition(0, new Vector3 (40,50,0));

//		transform.localScale += new Vector3 (0.1F, 0, 0);

//		lineRenderer.SetPosition (1, new Vector3(transform.position.x, transform.position.y ,0));
//		lineRenderer.SetPosition (1, GameObject.FindGameObjectWithTag("outputA").transform.position);
//		lineRenderer.SetPosition (0, GameObject.FindGameObjectWithTag ("inputB2").transform.position);
		//		lineRenderer.SetPosition (1, new Vector3(2,4,0));

		lineRenderer.SetPosition (0, origin.position);
//		lineRenderer.SetPosition (1, destin.position);
//		lineRenderer.SetPosition (1, Input.mousePosition);
		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

	}
}
