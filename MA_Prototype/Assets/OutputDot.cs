using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputDot : MonoBehaviour {

	private CircleCollider2D circCol, newCircCol;

	public int input;

	void Awake () {

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

	void OnMouseEnter() {

		if (Manager.MouseLineScript != null) {
			Manager.MouseLineScript.destinObject = this.gameObject;

			Debug.Log (Manager.MouseLineScript.destinObject.name);
		}
	}
}
