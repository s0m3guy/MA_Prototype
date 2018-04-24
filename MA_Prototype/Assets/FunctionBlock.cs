using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FunctionBlock : MonoBehaviour {

	public Transform block;
	public GameObject clone;
	public int speed;
	public bool isClone = false;

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

	[SerializeField]
	InputCircle[] inputPins;

	[SerializeField]
	Transform breadboardLeft, breadboardRight;

	// Variables for IF block (gained from UI canvas)

	public string comparator;
	public float comparatorValue;

	[SerializeField]
	Vector3 currentDraggingPosition;

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

		Physics2D.IgnoreLayerCollision(15, 9);

		inputPins = GetComponentsInChildren<InputCircle>() as InputCircle[];

//		if (gameObject.transform.parent.name.Contains ("(Clone)")) {
//			isClone = true;
//		}
			
		block = transform.parent.gameObject.transform;
	}
	
	// Update is called once per frame
	void Update () {

		if (isClone && isFBbeingDragged) {
			currentDraggingPosition = transform.position;
		}

		if (inputPins[0].connectedLine == null) {
			inputs[0] = 0;
		}
		if (inputs.Length == 2 && inputPins[1].connectedLine == null) {
			inputs[1] = 0;
		}

		if (pressed) {
			levelTimer += Time.deltaTime;
		}

		forwardInput ();

		if (transform.parent.name.Contains ("VALUE")) {
			lerpedColor = Color.Lerp (Color.white, Color.green, inputs [0] / 5);
			input1GO.GetComponent<SpriteRenderer> ().color = lerpedColor;
		} else if (!transform.parent.name.Contains("VALUE") && inputs [0] == 0) {
			input1GO.GetComponent<SpriteRenderer> ().color = Color.white;
		} else if (!transform.parent.name.Contains("VALUE") && inputs [0] == 5) {
			input1GO.GetComponent<SpriteRenderer> ().color = Color.green;
		}

		if (inputs.Length == 2 && inputs [1] == 0) {
			input2GO.GetComponent<SpriteRenderer> ().color = Color.white;
		} else if (inputs.Length == 2 && inputs [1] == 5) {
			input2GO.GetComponent<SpriteRenderer> ().color = Color.green;
		}

		if (output == 0) {
			outputGO.GetComponent<SpriteRenderer>().color = Color.white;
		} else if (output == 5) {
			outputGO.GetComponent<SpriteRenderer> ().color = Color.green;
		}

		if (transform.parent.name.Contains("VALUE")) {
			lerpedColor = Color.Lerp(Color.white, Color.green, output / 5);
			outputGO.GetComponent<SpriteRenderer>().color = lerpedColor;
			if (!transform.parent.name.Contains("VALUE") && output == 0) {
				outputGO.GetComponent<SpriteRenderer>().color = Color.white;
			} else if (!transform.parent.name.Contains("VALUE") && output == 1) {
				outputGO.GetComponent<SpriteRenderer>().color = Color.green;
			}
		}

			// in case removal overlay is active
		if (testSquare != null && testSquare.activeSelf) {
				this.GetComponent<SpriteRenderer> ().sortingLayerName = "Line";
				this.GetComponentInChildren<Canvas> ().sortingLayerName = "FB Label";
				this.GetComponentInChildren<Canvas> ().sortingOrder = 1;

			foreach (SpriteRenderer spriteRend in GetComponentsInChildren<SpriteRenderer>()) {
				spriteRend.sortingLayerName = "Functional Blocks";
			}

				// in case it's not active
			} else if (testSquare != null && !testSquare.activeSelf) {
				testSquare.SetActive (false);
				this.GetComponent<SpriteRenderer> ().sortingLayerName = "Functional Blocks";
				this.GetComponentInChildren<Canvas> ().sortingLayerName = "FB Label";
			}
	}

	void OnCollisionEnter2D(Collision2D other) {
		
	}

	void OnTriggerStay2D(Collider2D other) {
		/* Temporary out
//		if (other.name == "Square") {
//		Debug.Log ("Staying " + other.name);
//			if (testSquare != null && testSquare.activeSelf) {
//				GameObject.FindGameObjectWithTag ("testInnerSquare").GetComponent<SpriteRenderer> ().sprite = Resources.Load ("waste-bin-red", typeof(Sprite)) as Sprite;
//			}
//		}
//		elementAboveWasteBin = true;
		*/
		if (other.gameObject.tag == "testSquare") {
			Debug.Log("Staying in removal overlay");
			GameObject.FindGameObjectWithTag("testInnerSquare").GetComponent<SpriteRenderer>().color = Color.red;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
//		if (other.name == "Square") {
////		Debug.Log ("Exiting " + other.name);
//			GameObject.FindGameObjectWithTag ("testInnerSquare").GetComponent<SpriteRenderer> ().sprite = Resources.Load ("waste-bin-grey", typeof(Sprite)) as Sprite;
//		}
//		elementAboveWasteBin = false;
		Debug.Log("Exiting removal overlay");
		GameObject.FindGameObjectWithTag("testInnerSquare").GetComponent<SpriteRenderer>().color = Color.grey;
	}

	void OnMouseDown() {

		pressed = true;

		if (!isClone) {
//			clone = Instantiate (block);

			// Experimental code for when using prefab clone instead of transform clone
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
			clone.tag = "funcBlockClone";
			clone.gameObject.layer = 10;

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
			clone.transform.position = curPosition;																			// Move clone to this position
		} else {
//			transform.position = curPosition;		// non clamped version
			// Mathf.Clamp() restricts the movement of the dragged FB to the working area
			transform.position = new Vector3 (
				Mathf.Clamp (curPosition.x,
					(breadboardLeft.position.x+breadboardLeft.GetComponent<BoxCollider2D>().bounds.size.x)+0.4f, 
					breadboardRight.position.x-breadboardRight.GetComponent<BoxCollider2D>().bounds.size.x),
				curPosition.y,
				curPosition.z);
		}
	}

	void OnMouseUp() {

		isFBbeingDragged = false;

		if (!isClone && transform.parent.name.Contains("_IF")) {
			UIcanvas.enabled = true;
			Manager.currentIFblock = clone.gameObject;
		}

		if (isClone && levelTimer < 0.25 && transform.parent.name.Contains("_IF")) {
			UIcanvas.enabled = true;
			Manager.currentIFblock = transform.parent.gameObject;
		}

		levelTimer = 0;
		pressed = false;

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

		if (transform.parent.name.Contains("_AND")) {
			if (inputs[0] == 0 || inputs[1] == 0) {
				output = 0;
			} else if (inputs[0] == 5 && inputs[1] == 5) {
				output = 5;
			}
		} else if (transform.parent.name.Contains("_OR")) {
			if (inputs[0] == 5 || inputs[1] == 5) {
				output = 5;
			} else if (inputs[0] == 0 && inputs[1] == 0) {
				output = 0;
			}
		} else if (transform.parent.name.Contains("_XOR")) {
			if ((inputs[0] == 1 && inputs[1] == 0) || (inputs[0] == 0 && inputs[1] == 1)) {
				output = 1;
			} else if ((inputs[0] == 0 && inputs[1] == 0) || (inputs[0] == 1 && inputs[1] == 1)) {
				output = 0;
			}
		} else if (transform.parent.name.Contains("VALUE")) {
			this.GetComponentInChildren<Text>().text = inputs[0].ToString("0.0");
			output = inputs[0];
		} else if (transform.parent.name.Contains("_IF")) {
			if (inputPins[0].connectedLine) {
				if (comparator == "=") {
					if (inputs[0] == comparatorValue) {
						output = 5;
					} else {
						output = 0;
					}
				} else if (comparator == "<") {
					if (inputs[0] < comparatorValue) {
						output = 5;
					} else {
						output = 0;
					}
				} else if (comparator == ">") {
					if (inputs[0] > comparatorValue) {
						output = 5;
					} else {
						output = 0;
					}
				} else if (comparator == "≤") {
					if (inputs[0] <= comparatorValue) {
						output = 5;
					} else {
						output = 0;
					}
				} else if (comparator == "≥") {
					if (inputs[0] >= comparatorValue) {
						output = 5;
					} else {
						output = 0;
					}
				}
			} else {
				output = 0;
			}
		}
	}

	public bool checkClone () {
		return isClone;
	}

	public void setCloneStatus (bool cloneStatus) {
		isClone = cloneStatus;
	}
}
