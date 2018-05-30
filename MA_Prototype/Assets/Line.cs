using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {

	public GameObject originObject;
	public GameObject destinObject;

	private BreadBoardInputPin breadboardInputPinScript;
	private FunctionBlock functionBlockScript;
	private BreadBoardOutputPin breadboardOutputPinScript;
	BreadBoardInputPin tempBreadBoardInput;

	public float input, output;

	public bool isEndingPointSnapped = false;

	void Update() {

		if (destinObject && originObject) {
			GetComponent<LineRenderer>().SetPosition(0, originObject.transform.position);
			GetComponent<LineRenderer>().SetPosition(1, destinObject.transform.position);
		}

		forwardInput(input, output);

		if (originObject) {
			if (originObject.CompareTag("inputDot")) {
				tempBreadBoardInput = originObject.GetComponent<BreadBoardInputPin>();
				if (tempBreadBoardInput.inputType != "") {
					GetComponent<LineRenderer>().startColor = originObject.GetComponent<SpriteRenderer>().color;
					GetComponent<LineRenderer>().endColor = originObject.GetComponent<SpriteRenderer>().color;
				}
			} else if (originObject.CompareTag("output")) {
				GetComponent<LineRenderer>().startColor = originObject.GetComponent<SpriteRenderer>().color;
				GetComponent<LineRenderer>().endColor = originObject.GetComponent<SpriteRenderer>().color;
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
				breadboardInputPinScript = originObject.GetComponent<BreadBoardInputPin> ();
				this.output = breadboardInputPinScript.inputValue;
				this.input = breadboardInputPinScript.inputValue;
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
					if (destinObject.transform.parent.gameObject.transform.parent.name.Contains("_VALUE")
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
				breadboardOutputPinScript = destinObject.GetComponent<BreadBoardOutputPin> ();
				 breadboardOutputPinScript.input = this.output;
			}
		}
	}

	public void unSnap() {

	}
}