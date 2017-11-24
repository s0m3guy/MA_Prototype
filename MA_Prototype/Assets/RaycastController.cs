using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Raycast ();
	}

	void Raycast ()
	{
		Vector2 worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (worldPoint, Vector2.zero);
		if (hit.collider != null) {
			Debug.Log (hit.collider.name);
		}
	}
}
