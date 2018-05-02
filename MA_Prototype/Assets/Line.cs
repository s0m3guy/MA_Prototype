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
	private AnalogInputDot analogInputScript;

	private Vector3 screenPoint;
	private Vector3 offset;

	private EdgeCollider2D lineCollider;

	public float input, output;

	public bool isEndingPointSnapped = false;	// if line is snapped to a target

	Color lerpedColor;

	void Awake () {

		line = GetComponent<LineRenderer> ();
		lineCollider = GetComponent<EdgeCollider2D> ();
	}

	// Update is called once per frame
	void Update () {
		forwardInput(input, output);

		if (isEndingPointSnapped) {
			if (originObject.CompareTag("inputDot")) {
				randomInputDotScript = originObject.GetComponent<RandomInputDot>();
				if (randomInputDotScript.inputType == "analog") {
					lerpedColor = Color.Lerp(Color.white, Color.green, output / 5);
					this.GetComponent<LineRenderer>().startColor = lerpedColor;
					this.GetComponent<LineRenderer>().endColor = lerpedColor;
				} else if (randomInputDotScript.inputType == "digital") {
					if (output == 0) {
						this.GetComponent<LineRenderer>().startColor = Color.white;
						this.GetComponent<LineRenderer>().endColor = Color.white;
					} else if (output == 5) {
						this.GetComponent<LineRenderer>().startColor = Color.green;
						this.GetComponent<LineRenderer>().endColor = Color.green;
					}
				}
			} else if (originObject.CompareTag("output")) {
				if(output == 0) {
					this.GetComponent<LineRenderer>().startColor = Color.white;
					this.GetComponent<LineRenderer>().endColor = Color.white;
				} else if (output == 5) {
					this.GetComponent<LineRenderer>().startColor = Color.green;
					this.GetComponent<LineRenderer>().endColor = Color.green;
				}
			} else {
				this.GetComponent<LineRenderer>().startColor = Color.white;
				this.GetComponent<LineRenderer>().endColor = Color.white;
			}

			if (isEndingPointSnapped) {
				if (destinObject) {
					GetComponent<LineRenderer>().SetPosition(1, destinObject.transform.position);		// Line stays connected when moving destin object
				}
				GetComponent<LineRenderer>().SetPosition(0, originObject.transform.position);
			}
		}
	}

	// forwardInput() in Line.cs takes input value and copies value to target object
	public void forwardInput (float input, float output) {
		string typeOfOriginObject, typeOfDestinObject;

		// Checking the type of origin
		if (originObject != null) {
			typeOfOriginObject = originObject.gameObject.name;

			if (originObject.CompareTag ("inputDot")) {
				randomInputDotScript = originObject.GetComponent<RandomInputDot> ();
				this.output = randomInputDotScript.inputValue;
				this.input = randomInputDotScript.inputValue;
			} else if (typeOfOriginObject.Contains ("Output")) {
				functionBlockScript = originObject.GetComponentInParent<FunctionBlock> ();
				this.output = functionBlockScript.output;
			} else if (originObject.CompareTag ("inputDotAnalog")) {
				this.output = originObject.GetComponent<AnalogInputDot> ().value;
			}
		}

		// Checking the type of destination
		if (destinObject != null) {
			typeOfDestinObject = destinObject.gameObject.name;
			if (typeOfDestinObject.Contains ("Input")) {
				functionBlockScript = destinObject.GetComponentInParent<FunctionBlock> ();
				if (typeOfDestinObject.Contains ("Input 1")) {
					if (destinObject.transform.parent.gameObject.transform.parent.name.Contains("VALUE")
						|| destinObject.transform.parent.gameObject.transform.parent.name.Contains ("_IF"))
					{
						functionBlockScript.inputs[0] = this.output;
					} else {
						functionBlockScript.inputs [0] = (int)this.output;
					}
				} else if (typeOfDestinObject.Contains ("Input 2")) {
					functionBlockScript.inputs [1] = (int)this.output;
				} else if (destinObject.transform.parent.name.Contains ("VALUE")) {
					functionBlockScript.inputs [0] = this.output;
				}
			} else if (typeOfDestinObject.Contains ("output_dot")) {
				outputDotScript = destinObject.GetComponent<OutputDot> ();

//				if (output == 1) {
//					Debug.Log("Output is: " + output);
//					outputDotScript.input = 5;
//				} else if (output == 0) {
//					Debug.Log("Output is: " + output);
//					outputDotScript.input = 0;
//				}
				 outputDotScript.input = this.output;
			}
		}
	}

	public void unSnap() {
		isEndingPointSnapped = false;
	}
}