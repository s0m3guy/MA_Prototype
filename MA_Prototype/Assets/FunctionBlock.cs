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

	public GameObject removalOverlay, wasteBin, testSquare, testInnerSquare;

	public bool isFBbeingDragged = false;

	bool elementAboveWasteBin = false;

	Color lerpedColor;

	// Variables for short click detection
	float levelTimer = 0.0f;
	bool pressed = false;
	[SerializeField]
	Canvas UIcanvas;

	[SerializeField]
	FuncBlockInputPin[] inputPins;

	[SerializeField]
	Transform breadboardLeft, breadboardRight;

	// Variables for IF block (gained from UI canvas)
	public string comparator;
	public float comparatorValue;

	[SerializeField]
	Vector3 currentDraggingPosition;

	GameObject temporaryChildParentContainer;
	string[] lineNames = new string[3];

	SpriteRenderer outputSpriteRenderer, input1SpriteRenderer, input2SpriteRenderer;

	Transform leftBound, rightBound;

	void Awake () {

		testSquare = GameObject.FindGameObjectWithTag("testSquare");
		testInnerSquare = GameObject.FindGameObjectWithTag("testInnerSquare");
		breadboardLeft = GameObject.FindGameObjectWithTag("breadboardLeft").GetComponent<Transform>();
		breadboardRight = GameObject.FindGameObjectWithTag("breadboardRight").GetComponent<Transform>();
		UIcanvas = GameObject.FindGameObjectWithTag("UIcanvas").GetComponent<Canvas>();

		inputs = new float[transform.childCount - 2];		// Total amount minus canvas+output equals the amount of inputs

		input1GO = transform.Find ("Input 1").gameObject;
		if (transform.parent.name.Contains ("AND") || transform.parent.name.Contains ("OR")) {
			input2GO = transform.Find ("Input 2").gameObject;
		}
		outputGO = transform.Find ("OutputA").gameObject;

		outputSpriteRenderer = outputGO.GetComponent<SpriteRenderer>();
		input1SpriteRenderer = input1GO.GetComponent<SpriteRenderer>();
		if (input2GO) {
			input2SpriteRenderer = input2GO.GetComponent<SpriteRenderer>();
		}
	}

	// Use this for initialization
	void Start () {

		inputPins = GetComponentsInChildren<FuncBlockInputPin>() as FuncBlockInputPin[];
			
		block = transform.parent.gameObject.transform;

		leftBound = GameObject.Find("LeftBound").GetComponent<Transform>();
		rightBound = GameObject.Find("RightBound").GetComponent<Transform>();
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

		if (input1GO.GetComponent<FuncBlockInputPin>().connectedLine) {
			input1GO.GetComponent<SpriteRenderer>().color = input1GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().startColor;
			input1GO.GetComponent<SpriteRenderer>().color = input1GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().endColor;
		}
		if (input2GO) {
			if (input2GO.GetComponent<FuncBlockInputPin>().connectedLine) {
				input2GO.GetComponent<SpriteRenderer>().color = input2GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().startColor;
				input2GO.GetComponent<SpriteRenderer>().color = input2GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().endColor;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		
	}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.tag == "testSquare") {
			GameObject.FindGameObjectWithTag("testInnerSquare").GetComponent<SpriteRenderer>().color = Color.red;
			elementAboveWasteBin = true;

			this.GetComponent<SpriteRenderer>().sortingLayerName = "Above Removal Overlay";
			this.GetComponentInChildren<Canvas>().sortingLayerName = "Above Removal Overlay";
			foreach (SpriteRenderer sr in this.GetComponentsInChildren(typeof(SpriteRenderer))) {
				sr.sortingLayerName = "Above Removal Overlay";
			}


			foreach (FuncBlockInputPin ic in this.GetComponentsInChildren(typeof(FuncBlockInputPin)))
				if (ic.connectedLine) {
					ic.connectedLine.GetComponent<LineRenderer>().sortingLayerName = "Above Removal Overlay";
				}

			if (this.GetComponentInChildren<FuncBlockOutputPin>().connectedLine) {
				this.GetComponentInChildren<FuncBlockOutputPin>().connectedLine.GetComponent<LineRenderer>().sortingLayerName = "Above Removal Overlay";
			}


			this.GetComponent<SpriteRenderer>().sortingOrder = 1;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.tag == "testSquare") {
			GameObject.FindGameObjectWithTag("testInnerSquare").GetComponent<SpriteRenderer>().color = Color.grey;
			elementAboveWasteBin = false;

			this.GetComponent<SpriteRenderer>().sortingLayerName = "Functional Blocks";
			this.GetComponentInChildren<Canvas>().sortingLayerName = "FB Label";
			foreach (SpriteRenderer sr in this.GetComponentsInChildren(typeof(SpriteRenderer))) {
				sr.sortingLayerName = "Functional Blocks";
			}
				
			foreach (FuncBlockInputPin ic in this.GetComponentsInChildren(typeof(FuncBlockInputPin)))
				if (ic.connectedLine) {
					ic.connectedLine.GetComponent<LineRenderer>().sortingLayerName = "Line";
				}

			if (this.GetComponentInChildren<FuncBlockOutputPin>().connectedLine) {
				this.GetComponentInChildren<FuncBlockOutputPin>().connectedLine.GetComponent<LineRenderer>().sortingLayerName = "Line";
			}
		}
	}

	void OnMouseDown() {

		if (this.transform.Find("Input 1") && this.transform.Find("Input 1").GetComponent<FuncBlockInputPin>().connectedLine) {
			lineNames[0] = this.transform.Find("Input 1").GetComponent<FuncBlockInputPin>().connectedLine.name;
		}
		if (this.transform.Find("Input 2") && this.transform.Find("Input 2").GetComponent<FuncBlockInputPin>().connectedLine) {
			lineNames[1] = this.transform.Find("Input 2").GetComponent<FuncBlockInputPin>().connectedLine.name;
		}
			
		foreach (FuncBlockInputPin fip in this.GetComponentsInChildren(typeof(FuncBlockInputPin)))
			if (fip.connectedLine) {
				fip.connectedLine.GetComponent<Bezier_Spline>().tangent2.transform.parent = this.transform;
			}
		
		if (GetComponentInChildren<FuncBlockOutputPin>().connectedLine) {
			lineNames[2] = this.transform.Find("OutputA").GetComponent<FuncBlockOutputPin>().connectedLine.name;
			GetComponentInChildren<FuncBlockOutputPin>().connectedLine.GetComponent<Bezier_Spline>().tangent1.transform.parent = this.transform;
		}

		pressed = true;

		if (!isClone) {

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
			testSquare.GetComponent<SpriteRenderer>().enabled = true;
			testSquare.GetComponent<BoxCollider2D>().enabled = true;
			testInnerSquare.GetComponent<SpriteRenderer>().enabled = true;
			testSquare.GetComponentInChildren<SpriteRenderer>().enabled = true;
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
			transform.parent.position = new Vector3 (
				Mathf.Clamp (curPosition.x,
					leftBound.position.x, 
					rightBound.position.x),
				curPosition.y,
				curPosition.z);
		}
	}

	void OnMouseUp() {

		if (transform.Find("tangent2") && lineNames[0] != null) {
			transform.Find("tangent2").transform.parent = GameObject.Find(lineNames[0]).transform;
		}
		if (transform.Find("tangent2") && lineNames[1] != null){
			transform.Find("tangent2").transform.parent = GameObject.Find(lineNames[1]).transform;
		}
		if (transform.Find("tangent1") && lineNames[2] != null){
			transform.Find("tangent1").transform.parent = GameObject.Find(lineNames[2]).transform;
		}

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

		bool hasOutputLines = (this.GetComponentInChildren<FuncBlockOutputPin> ().connectedLine != null) ? true : false;
		string typeOfDestin = "";

		// Determine type which line is connected to
		if (hasOutputLines) {
			if (this.GetComponentInChildren<FuncBlockOutputPin> ().connectedLine.GetComponent<Bezier_Spline> ().destinObject.name.Contains ("Input")) {
				typeOfDestin = "FB";
			} else {
				typeOfDestin = "outputPin";
			}
		}

		// Depending on type, reset connection
		if (elementAboveWasteBin) {
			if (hasOutputLines && typeOfDestin == "FB") {
				this.GetComponentInChildren<FuncBlockOutputPin>().connectedLine.GetComponent<Bezier_Spline>().destinObject.GetComponent<FuncBlockInputPin>().connectedLine = null;
			} else if (hasOutputLines && typeOfDestin == "outputPin") {
				this.GetComponentInChildren<FuncBlockOutputPin>().connectedLine.GetComponent<Bezier_Spline>().destinObject.GetComponent<BreadBoardOutputPin>().connectedLine = null;
			}

			// Destruction of input is looped because of several inputs
			foreach (FuncBlockInputPin ic in this.GetComponentsInChildren(typeof(FuncBlockInputPin)))
				if (ic.connectedLine) {
					Destroy(ic.connectedLine.gameObject);
				}
			
			// Destruction of output is not because there is only one
			if (this.GetComponentInChildren<FuncBlockOutputPin>().connectedLine) {
				Destroy(this.GetComponentInChildren<FuncBlockOutputPin>().connectedLine.gameObject);
			}
			Destroy(this.transform.parent.gameObject);
		}

		if (testSquare.activeSelf) {
			testSquare.GetComponent<SpriteRenderer>().enabled = false;
			testSquare.GetComponent<BoxCollider2D>().enabled = false;
			testInnerSquare.GetComponent<SpriteRenderer>().enabled = false;
			testSquare.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}
	}
		
	public void forwardInput () {

		if (transform.parent.name.Contains("_AND")) {
			if (inputs[0] == 0 || inputs[1] == 0) {
				output = 0;
				setSpriteRendererColor("output", Color.white);
			} else if (inputs[0] != 0 && inputs[1] != 0) {
				output = 5;
				outputGO.GetComponent<SpriteRenderer>().color = input1GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().startColor;
				outputGO.GetComponent<SpriteRenderer>().color = input1GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().endColor;
			}
		} else if (transform.parent.name.Contains("_OR")) {
			if (inputs[0] != 0 || inputs[1] != 0) {
				output = 5;
				if (input1GO.GetComponent<FuncBlockInputPin>().connectedLine) {
					outputGO.GetComponent<SpriteRenderer>().color = input1GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().startColor;
					outputGO.GetComponent<SpriteRenderer>().color = input1GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().endColor;
				}
			} else if (inputs[0] == 0 && inputs[1] == 0) {
				output = 0;
			}
		} else if (transform.parent.name.Contains("VALUE")) {
			this.GetComponentInChildren<Text>().text = inputs[0].ToString("0.0");
			output = inputs[0];
			if (input1GO.GetComponent<FuncBlockInputPin>().connectedLine) {
				outputGO.GetComponent<SpriteRenderer>().color = input1GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().startColor;
				outputGO.GetComponent<SpriteRenderer>().color = input1GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().endColor;
			} else if (!input1GO.GetComponent<FuncBlockInputPin>().connectedLine) {
				setSpriteRendererColor("output", Color.white);
			}
		} else if (transform.parent.name.Contains("_IF")) {
			if (inputPins[0].connectedLine) {
				if (comparator == "=") {
					if (inputs[0] == comparatorValue) {
						output = 5;
						outputGO.GetComponent<SpriteRenderer>().color = input1GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().startColor;
						outputGO.GetComponent<SpriteRenderer>().color = input1GO.GetComponent<FuncBlockInputPin>().connectedLine.GetComponent<LineRenderer>().endColor;
					} else {
						output = 0;
						outputGO.GetComponent<SpriteRenderer>().color = Color.white;
						outputGO.GetComponent<SpriteRenderer>().color = Color.white;
					}
				} else if (comparator == "<") {
					if (inputs[0] < comparatorValue) {
						output = 5;
						setSpriteRendererColor("output", Color.green);
					} else {
						output = 0;
						setSpriteRendererColor("output", Color.white);
					}
				} else if (comparator == ">") {
					if (inputs[0] > comparatorValue) {
						output = 5;
						setSpriteRendererColor("output", Color.green);
					} else {
						output = 0;
						setSpriteRendererColor("output", Color.white);
					}
				} else if (comparator == "≤") {
					if (inputs[0] <= comparatorValue) {
						output = 5;
						setSpriteRendererColor("output", Color.green);
					} else {
						output = 0;
						setSpriteRendererColor("output", Color.white);
					}
				} else if (comparator == "≥") {
					if (inputs[0] >= comparatorValue) {
						output = 5;
						setSpriteRendererColor("output", Color.green);
					} else {
						output = 0;
						setSpriteRendererColor("output", Color.white);
					}
				}
			} else {
				output = 0;
				setSpriteRendererColor("output", Color.white);
			}
		}
	}

	void setSpriteRendererColor (string type, Color color) {

		if (type == "input1") {
			input1SpriteRenderer.color = color;
		} else if (type == "input2") {
			input2SpriteRenderer.color = color;
		} else if (type == "output") {
			outputSpriteRenderer.color = color;
		}

	}

	public bool checkClone () {
		return isClone;
	}

	public void setCloneStatus (bool cloneStatus) {
		isClone = cloneStatus;
	}
}
