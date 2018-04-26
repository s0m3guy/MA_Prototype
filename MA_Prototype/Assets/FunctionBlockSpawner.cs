using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunctionBlockSpawner : MonoBehaviour {

	Vector3 screenPoint, offset;
	[SerializeField]
	Transform breadboardLeft, breadboardRight;
	[SerializeField]
	GameObject clone;
	[SerializeField]
	BoxCollider2D boxCollider;

	GameObject testSquare, testInnerSquare;

	bool isAboveWasteBin;

	enum E
	{
		neutral,
		aboveWasteBin,
		notAboveWasteBin,
		dropped,
	};

	[SerializeField]
	E foo = E.neutral;

	// Use this for initialization
	void Start () {
		testSquare = GameObject.FindGameObjectWithTag("testSquare");
		testInnerSquare = GameObject.FindGameObjectWithTag("testInnerSquare");
	}
	
	// Update is called once per frame
	void Update () {
		if (clone) {
			if (!boxCollider.bounds.Contains(clone.transform.position) && foo == E.aboveWasteBin) {
				Debug.Log("Not inside");
				Manager.toggleOverlay(true);
				foo = E.notAboveWasteBin;

			} else if (boxCollider.bounds.Contains(clone.transform.position)) {
				Debug.Log("Inside");
				foo = E.aboveWasteBin;
			}
		}
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

		clone.GetComponentInChildren<FunctionBlock> ().isClone = true;

		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag() {
		
		Vector3 cursorPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorPoint) + offset;
		clone.transform.position = new Vector3 (
			Mathf.Clamp (cursorPosition.x,
				(breadboardLeft.position.x+breadboardLeft.GetComponent<BoxCollider2D>().bounds.size.x)+0.4f, 
				breadboardRight.position.x-breadboardRight.GetComponent<BoxCollider2D>().bounds.size.x),
			cursorPosition.y,
			cursorPosition.z);
//		clone.transform.position = cursorPosition;
	}

	void OnMouseUp() {
//		if (isAboveWasteBin = true) {
		if(foo == E.aboveWasteBin) {
			Destroy(clone.gameObject);
		}
		testSquare.GetComponent<SpriteRenderer>().enabled = false;
		testSquare.GetComponent<BoxCollider2D>().enabled = false;
		testInnerSquare.GetComponent<SpriteRenderer>().enabled = false;
		testSquare.GetComponentInChildren<SpriteRenderer>().enabled = false;	
	}
}
