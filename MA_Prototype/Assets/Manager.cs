using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Manager {
	
	public static GameObject currentlyDrawnLine;

	public static Vector2[] edgeColliderPoints;

	public static GameObject currentIFblock;

	public static GameObject currentInputPin;

	static GameObject testSquare = GameObject.FindGameObjectWithTag("testSquare");
	static GameObject testInnerSquare = GameObject.FindGameObjectWithTag("testInnerSquare");
	static Transform breadboardLeft = GameObject.FindGameObjectWithTag("breadboardLeft").GetComponent<Transform>();
	static Transform breadboardRight = GameObject.FindGameObjectWithTag("breadboardRight").GetComponent<Transform>();

	public static void toggleOverlay (bool state) {
		testSquare.GetComponent<SpriteRenderer>().enabled = state;
		testSquare.GetComponent<BoxCollider2D>().enabled = state;
		testInnerSquare.GetComponent<SpriteRenderer>().enabled = state;
		testSquare.GetComponentInChildren<SpriteRenderer>().enabled = state;
	}
}
