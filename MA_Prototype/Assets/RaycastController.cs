using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour {
	public Camera cam;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Raycast2 ();
	}

	void Raycast ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 10;
			Vector3 screenPos = cam.ScreenToWorldPoint (mousePos);
			RaycastHit2D hit = Physics2D.Raycast (screenPos, Vector2.zero);
			if (hit.collider.gameObject.name == "OutputA") {
				Debug.Log ("Output!");
			} else if (hit.collider.gameObject.name == "inputB") {
				Debug.Log ("InputB!");
			}
		}
	}

	void Raycast2 ()
	{
		Vector2 worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast (worldPoint, Vector2.zero);
		if (hit.collider != null) {
			Debug.Log (hit.collider.name);
		}
	}
}
