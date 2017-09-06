using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw_Line_2 : MonoBehaviour {

	private LineRenderer line;
	Transform origin;
	Transform destin;

	// Use this for initialization
	void Start () {

		line = GetComponent<LineRenderer> ();
		line.startWidth = 2;
		line.endWidth = 3;

	}
	
	// Update is called once per frame
	void Update () {

		line.SetPosition (0, origin.position);
		line.SetPosition (1, destin.position);

	}
}
