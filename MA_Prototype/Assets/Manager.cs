using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Manager {
	
	public static GameObject currentlyDrawnLine;
//	public static LineRenderer MouseLineRenderer;
//	public static EdgeCollider2D MouseLineEdgeCollider;

	public static Vector2[] edgeColliderPoints;

	public static GameObject currentIFblock;

	public static GameObject currentInputPin;

	public static bool collisionDetected = false;
}
