using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FunctionBlock : MonoBehaviour {

	public Transform block;
	public Transform clone;
	public int speed;
	private bool isClone = false;

	public string logicGate;

	public float[] inputs;
	public float output;

	private Vector3 screenPoint;
	private Vector3 offset;

	GameObject input1GO; 
	GameObject input2GO;
	GameObject outputGO;

	public GameObject removalOverlay, wasteBin, testSquare;

	public bool isFBbeingDragged = false;

	bool elementAboveWasteBin = false;

	Color lerpedColor;

	// Variables for short click detection
	float levelTimer = 0.0f;
	bool pressed = false;
	[SerializeField]
	Canvas UIcanvas;

	void Awake () {

		inputs = new float[transform.childCount - 2];		// Total amount minus canvas+output equals the amount of inputs

		input1GO = transform.Find ("Input 1A").gameObject;
		if (transform.parent.name.Contains ("AND") || transform.parent.name.Contains ("OR")) {
			input2GO = transform.Find ("Input 2A").gameObject;
		}
		outputGO = transform.Find ("OutputA").gameObject;
	}

	// Use this for initialization
	void Start () {

//		if (gameObject.transform.parent.name.Contains ("(Clone)")) {
//			isClone = true;
//		}
			
		block = transform.parent.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {

		if (pressed) {
			levelTimer += Time.deltaTime;
		}

		forwardInput ();

		if (transform.parent.name.Contains ("VALUE")) {
			lerpedColor = Color.Lerp (Color.white, Color.green, inputs [0] / 5);
			input1GO.GetComponent<SpriteRenderer> ().color = lerpedColor;
		} else if (!transform.parent.name.Contains("VALUE") && inputs [0] == 0) {
			input1GO.GetComponent<SpriteRenderer> ().color = Color.white;
		} else if (!transform.parent.name.Contains("VALUE") && inputs [0] == 1) {
			input1GO.GetComponent<SpriteRenderer> ().color = Color.green;
		}

		if (inputs.Length == 2 && inputs [1] == 0) {
			input2GO.GetComponent<SpriteRenderer> ().color = Color.white;
		} else if (inputs.Length == 2 && inputs [1] == 1) {
			input2GO.GetComponent<SpriteRenderer> ().color = Color.green;
		}

		if (transform.parent.name.Contains ("VALUE")) {
			lerpedColor = Color.Lerp (Color.white, Color.green, output / 5);
			outputGO.GetComponent<SpriteRenderer> ().color = lerpedColor;
			if (!transform.parent.name.Contains ("VALUE") && output == 0) {
				outputGO.GetComponent<SpriteRenderer> ().color = Color.white;
			} else if (!transform.parent.name.Contains ("VALUE") && output == 1) {
				outputGO.GetComponent<SpriteRenderer> ().color = Color.green;
			}

			// in case removal overlay is active
			if (testSquare != null && testSquare.activeSelf) {
				this.GetComponent<SpriteRenderer> ().sortingLayerName = "Line";
				this.GetComponentInChildren<Canvas> ().sortingLayerName = "Waste Bin On Overlay";
				this.GetComponentInChildren<Canvas> ().sortingOrder = 1;

				foreach (SpriteRenderer spriteRend in GetComponentsInChildren<SpriteRenderer>()) {
					spriteRend.sortingLayerName = "Waste Bin On Overlay";
				}

				// in case it's not active
			} else if (testSquare != null && !testSquare.activeSelf) {
				testSquare.SetActive (false);
				this.GetComponent<SpriteRenderer> ().sortingLayerName = "Functional Blocks";
				this.GetComponentInChildren<Canvas> ().sortingLayerName = "FB Label";
			}

			// In case no line is connected to the inputs, reset input values
			if (!GetComponentInChildren<InputCircle> ().connectedLine) {
				foreach (int element in inputs) {
					inputs [element] = 0;
				}
				output = 0;
			}
		}
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.name == "Square") {
//		Debug.Log ("Staying " + other.name);
			if (testSquare != null && testSquare.activeSelf) {
				GameObject.FindGameObjectWithTag ("testInnerSquare").GetComponent<SpriteRenderer> ().sprite = Resources.Load ("waste-bin-red", typeof(Sprite)) as Sprite;
			}
		}
		elementAboveWasteBin = true;
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "Square") {
//		Debug.Log ("Exiting " + other.name);
			GameObject.FindGameObjectWithTag ("testInnerSquare").GetComponent<SpriteRenderer> ().sprite = Resources.Load ("waste-bin-grey", typeof(Sprite)) as Sprite;
		}
		elementAboveWasteBin = false;
	}

	void OnMouseDown() {

		pressed = true;

		if (!isClone) {
			clone = Instantiate (block);

			/* Experimental code for when using prefab clone instead of transform clone
			if (transform.parent.name.Contains ("AND")) {
				Debug.Log ("Successfully checked for AND block");
				clone = Instantiate (Resources.Load ("FunctionBlock_AND")) as GameObject;
				Debug.Log ("Successfully instantiated prefab of FB_AND");
			}
			if(transform.parent.name.Contains("AND")) {

				clone = Instantiate (Resources.Load ("LinePrefab")) as GameObject;

			} else if (transform.parent.name.Contains("OR")) {

				clone = Instantiate (Resources.Load ("LinePrefab")) as GameObject;

			} else if (transform.parent.name.Contains("VALUE")) {

				clone = Instantiate (Resources.Load ("LinePrefab")) as GameObject;

			}
			*/

			clone.GetComponentInChildren<FunctionBlock> ().isClone = true;
		} else {
			// Enable the removal overlay in order to remove function blocks
			testSquare.SetActive (true);
		}
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.position);

		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag() {
		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);			// Current touch point

		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;								// Current touch point converted to point in scene
		if (!isClone) {
			clone.position = curPosition;																			// Move clone to this position
		} else {
			transform.position = curPosition;
		}
	}

	void OnMouseUp() {

		if (isClone && levelTimer < 0.25) {
			UIcanvas.enabled = true;
			Manager.currentIFblock = transform.parent.gameObject;
		}

		levelTimer = 0;
		pressed = false;

		isFBbeingDragged = false;

		bool hasOutputLines = (this.GetComponentInChildren<OutputCircle> ().connectedLine != null) ? true : false;
		string typeOfDestin = "";

		// Determine type which line is connected to
		if (hasOutputLines) {
			if (this.GetComponentInChildren<OutputCircle> ().connectedLine.GetComponent<Line> ().destinObject.name.Contains ("Input")) {
				typeOfDestin = "FB";
			} else {
				typeOfDestin = "outputPin";
			}
		}

		// Depending on type, reset connection
		if (elementAboveWasteBin) {
			if (hasOutputLines && typeOfDestin == "FB") {
				this.GetComponentInChildren<OutputCircle> ().connectedLine.GetComponent<Line> ().destinObject.GetComponent<InputCircle> ().connectedLine = null;
			} else if (hasOutputLines && typeOfDestin == "outputPin") {
				this.GetComponentInChildren<OutputCircle> ().connectedLine.GetComponent<Line> ().destinObject.GetComponent<OutputDot> ().connectedLine = null;
			}

			// Destruction of input is looped because of several inputs
			foreach (InputCircle ic in this.GetComponentsInChildren(typeof(InputCircle)))
				Destroy (ic.connectedLine.gameObject);

			// Destruction of output is not because there is only one
			Destroy (this.GetComponentInChildren<OutputCircle> ().connectedLine.gameObject);
			Destroy (this.transform.parent.gameObject);
		}

		if (testSquare.activeSelf) {
			testSquare.SetActive (false);
		}
	}
		
	public void forwardInput () {

		if (transform.parent.name.Contains ("_AND")) {
			if (inputs [0] == 0 || inputs [1] == 0) {
				output = 0;
			} else if (inputs [0] == 1 && inputs [1] == 1) {
				output = 1;
			}
		} else if (transform.parent.name.Contains ("_OR")) {
			if (inputs [0] == 1 || inputs [1] == 1) {
				output = 1;
			} else if (inputs [0] == 0 && inputs [1] == 0) {
				output = 0;
			}
		} else if (transform.parent.name.Contains ("_XOR")) {
			if ((inputs [0] == 1 && inputs [1] == 0) || (inputs [0] == 0 && inputs [1] == 1)) {
				output = 1;
			} else if ((inputs [0] == 0 && inputs [1] == 0) || (inputs [0] == 1 && inputs [1] == 1)) {
				output = 0;
			}
		} else if (transform.parent.name.Contains ("VALUE")) {
			this.GetComponentInChildren<Text> ().text = inputs [0].ToString ("0.0") + "V";
			output = inputs [0];
		}
	}

	public bool checkClone () {
		return isClone;
	}

	public void setCloneStatus (bool cloneStatus) {
		isClone = cloneStatus;
	}
}
