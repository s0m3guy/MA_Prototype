﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunctionBlockSpawner : MonoBehaviour {

	Vector3 screenPoint, offset;
	[SerializeField]
	Transform breadboardLeft, breadboardRight;
	[SerializeField]
	GameObject clone;

	Canvas UIcanvas;

	SpriteRenderer[] childSprites;

	Transform leftBound, rightBound;

	BoxCollider2D panelCollider;

	void Awake() {
		UIcanvas = GameObject.FindGameObjectWithTag("UIcanvas").GetComponent<Canvas>();

		leftBound = GameObject.Find("LeftBound").GetComponent<Transform>();
		rightBound = GameObject.Find("RightBound").GetComponent<Transform>();

		panelCollider = UIcanvas.GetComponentInChildren<BoxCollider2D>();
	}

	void OnMouseDown() {
		
		if (transform.parent.name.Contains("AND")) {
			clone = Instantiate(Resources.Load("FB/FunctionBlock_AND")) as GameObject;
		} else if (transform.parent.name.Contains("OR")) {
			clone = Instantiate(Resources.Load("FB/FunctionBlock_OR")) as GameObject;
		} else if (transform.parent.name.Contains("VALUE")) {
			clone = Instantiate(Resources.Load("FB/FunctionBlock_VALUE")) as GameObject;
		} else if (transform.parent.name.Contains("IF")) {
			clone = Instantiate(Resources.Load("FB/FunctionBlock_IF")) as GameObject;
		}
		childSprites = clone.GetComponentsInChildren<SpriteRenderer>();

		clone.GetComponentInChildren<FunctionBlock> ().isClone = true;
		clone.tag = "funcBlockClone";

		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag() {

		foreach (SpriteRenderer sr in childSprites)
			sr.enabled = true;

		clone.GetComponentInChildren<Text>().enabled = true;

		Vector3 cursorPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorPoint) + offset;
		clone.transform.position = new Vector3 (
			Mathf.Clamp (cursorPosition.x,
				leftBound.position.x, 
				rightBound.position.x),
			cursorPosition.y,
			cursorPosition.z);

//		clone.transform.position = cursorPosition;
	}

	void OnMouseUp() {
		if (transform.parent.name.Contains("_IF")) {
			UIcanvas.enabled = true;
			panelCollider.size = new Vector2(493.2578f, 382.9383f);
			Manager.currentIFblock = clone.gameObject;
		}
	}
}
