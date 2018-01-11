using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FunctionBlock : MonoBehaviour {

	public Transform block, clone;
	public int speed;
	private bool isClone = false;

	public string logicGate;

	public int[] inputs;
	public int output;

	void Awake () {

		inputs = new int[transform.childCount - 2];
	}

	// Use this for initialization
	void Start () {

		if (gameObject.name.Contains ("(Clone)")) {
			isClone = true;
		}
			
		block = GetComponent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		forwardInput (null, null);		// Testing
	}

	private Vector3 screenPoint;
	private Vector3 offset;

	void OnMouseDown() {

		if (!isClone) {
			clone = Instantiate (block);
		}
//		if (isClone) {
			screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);

			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
//		}
	}

	void OnMouseDrag() {

//		if (isClone) {
			Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);			// Current touch point

			Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;								// Current touch point converted to point in scene

			clone.position = curPosition;																				// Move clone to this position
//		}
	}

	public void setInputA() {
		inputAset = true;
	}
	public void setInputB() {
		inputBset = true;
	}
	public void setOutput() {
		outputSet = true;
	}

	public void forwardInput (GameObject[] inputs, GameObject[] outputs) {

		/*
		int[] inputValues = new int[inputs.Length];
		int[] outputValues = new int[outputs.Length];
		int result = 0;
		string logicGate;
		bool isNegated = false;
		string blockLabel = GetComponentInChildren<Canvas> ().GetComponentInChildren<Text>().text;

		if (blockLabel == "=1") {
			logicGate = "XOR";
		} else if (blockLabel == "&") {
			logicGate = "AND";
		} else {
			logicGate = "OR";
		}

		if (GetComponentInChildren<Canvas> ().GetComponentInChildren <SpriteRenderer> () != null) {
			isNegated = true;
		}
			
		if (logicGate == "AND" && isNegated) {
			logicGate = "NAND";
		} else if (logicGate == "OR" && isNegated) {
			logicGate = "NOR";
		}
		*/

		/*
		for(int i = 0; i <= inputs.Length; i++) {
			inputValues[i] = inputs[i].GetComponent<Pin?>().value;
		}

		/*
		if(logicGate == "AND") {
			if(inputValues.Contains(0)) {
				result = 0;
				if (isNegative) {
					result = 0;
				}
			}
		} else if (LogicGate == "OR") {
			if(inputValues.Contains(1) {
				result = 1;
				if (isNegative) {
					result = 0;
				}
			}
		} else if (LogicGate == "XOR")








		*/
//		Debug.Log (isNegated);
	}
}
