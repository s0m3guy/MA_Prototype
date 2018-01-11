using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

	// (As of right now) This script draws the line following the mouse and checks if the mouse
	// collides with the bounding box of the input of another block

	private LineRenderer line = new LineRenderer();

	private GameObject goalInput;
	private GameObject[] goalInputs;
	private GameObject[] goalInputs2;
	private CircleCollider2D circCol;
	private CircleCollider2D[] circCols;

	public GameObject originObject;
	public GameObject destinObject;

	private FunctionBlock originBlock, destinBlock;

	private RandomInputDot randomInputDotScript;
	private InputCircle inputCircleScript;
	private OutputCircle outputCircleScript;
	private OutputDot outputDotScript;
	private FunctionBlock functionBlockScript;

	public int input, output;

	void Awake () {

		line = GetComponent<LineRenderer> ();
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		forwardInput (input, output);
	}

	void ScanInput () {
		goalInputs = FindGameObjectsWithDifferentTags(new string[] {"inputA", "inputB"});

		circCols = new CircleCollider2D[goalInputs.Length];

		for (int i = 0; i < goalInputs.Length; i++) {
			circCols [i] = goalInputs [i].GetComponent<CircleCollider2D> ();
		}
	}

	public static GameObject[] FindGameObjectsWithDifferentTags(string[] tags) {
		List<GameObject> list = new List<GameObject> ();
		foreach (string tag in tags) {
			GameObject[] objs = GameObject.FindGameObjectsWithTag (tag);
			list.AddRange (objs);
		}
		return list.ToArray ();
	}

	Transform getGameObjectAtPosition()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
					Debug.Log("found " + hit.transform.name + " at distance: " + hit.distance);
		return hit.transform;
	}

	public void forwardInput (int input, int output) {
		string typeOfOriginObject, typeOfDestinObject;

		if (originObject != null) {
			typeOfOriginObject = originObject.gameObject.name;
			if (typeOfOriginObject.Contains ("Dot")) {
				Debug.Log ("Dot connected");			// works
				randomInputDotScript = originObject.GetComponent<RandomInputDot> ();
				output = randomInputDotScript.value;
				Debug.Log ("output: " + output);		// works

			} else if(typeOfOriginObject.Contains ("Output")) {
				functionBlockScript = originObject.GetComponent<FunctionBlock> ();
				output = functionBlockScript.output;
				Debug.Log (output);
			}
		}

		if (destinObject != null) {
			typeOfDestinObject = destinObject.gameObject.name;

			if (typeOfDestinObject.Contains ("Input")) {
				Debug.Log ("Connected to input");
				inputCircleScript = destinObject.GetComponent<InputCircle> ();
				functionBlockScript = destinObject.GetComponentInParent<FunctionBlock> ();
				if (typeOfDestinObject.Contains ("Input 1")) {
					functionBlockScript.inputs [0] = output;
					Debug.Log ("Connected Input 1");			// works
					Debug.Log (functionBlockScript.inputs[0]);	// works
				} else if (typeOfDestinObject.Contains ("Input 2")) {
					functionBlockScript.inputs [1] = output;
					Debug.Log ("Connected to Input 2");		// works
				}
			} else if (typeOfDestinObject.Contains ("output_dot")) {
				outputDotScript = destinObject.GetComponent<OutputDot> ();
				outputDotScript.input = output;
			}
		}
	}
}