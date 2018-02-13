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

	// forwardInput() in Line.cs takes input value and copies value to target object
	public void forwardInput (int input, int output) {
		string typeOfOriginObject, typeOfDestinObject;

		// Checking the type of origin
		if (originObject != null) {
			typeOfOriginObject = originObject.gameObject.name;
			if (typeOfOriginObject.Contains ("Dot")) {
				randomInputDotScript = originObject.GetComponent<RandomInputDot> ();
				this.output = randomInputDotScript.value;
			} else if(typeOfOriginObject.Contains ("Output")) {
				functionBlockScript = originObject.GetComponentInParent<FunctionBlock> ();
				this.output = functionBlockScript.output;
			}
		}

		if (destinObject != null) {
			typeOfDestinObject = destinObject.gameObject.name;
			if (typeOfDestinObject.Contains ("Input")) {
				functionBlockScript = destinObject.GetComponentInParent<FunctionBlock> ();
				if (typeOfDestinObject.Contains ("Input 1")) {
					functionBlockScript.inputs [0] = this.output;
				} else if (typeOfDestinObject.Contains ("Input 2")) {
					functionBlockScript.inputs [1] = this.output;
				}
			} else if (typeOfDestinObject.Contains ("output_dot")) {
				outputDotScript = destinObject.GetComponent<OutputDot> ();
				outputDotScript.input = this.output;
			}
		}
	}
}