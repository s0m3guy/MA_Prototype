using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D col) {

		if (col.gameObject.CompareTag("inputB1")) {
			Debug.Log ("COLLISION!!!1111");
			Destroy (col.gameObject);
		}
	}
}