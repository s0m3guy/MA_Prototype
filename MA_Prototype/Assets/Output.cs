using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Output : MonoBehaviour {


	public void checkMouselineAndSnap (CircleCollider2D circCol) {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

		if (circCol.bounds.Contains (mousePos)) {
			Manager.MouseLineRenderer.SetPosition (1, this.transform.position);
		}
	}

}
