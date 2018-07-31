using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreadBoardInputPin : MonoBehaviour {

	public float inputValue = 5;
	private SpriteRenderer spritRend;

//	private Line, newLineScript;
	private LineRenderer lineRenderer = new LineRenderer ();
	private LineRenderer newLineRend = new LineRenderer();
	public GameObject newLineObj;
	public Transform origin;

	public float[] outputs;

	[SerializeField]
	Canvas UIcanvas;
	[SerializeField]
	GameObject ADPanel;
	[SerializeField]
	SpriteRenderer triangle;

	[SerializeField]
	float variable;

	// Variables for short click detection
	float levelTimer = 0.0f;
	bool pressed = false;

	public string inputType = "digital";

	// For analog input
	public float sineValue = 0;
	float increment = 0.07f;
	public Color lerpedColor;
	float x;

	GameObject line;
	Collider2D collisionObject;

	Vector3 clampVector;
	BoxCollider2D upperBound, lowerBound;

	GameObject debugBox1, debugBox2;
	Text debugText1, debugText2;

	void Awake () {
		spritRend = gameObject.GetComponent<SpriteRenderer> ();

		origin = GetComponent<Transform> ();

		outputs = new float[1];
	}

	// Use this for initialization
	void Start () {
		upperBound = GameObject.Find("Upperbound").GetComponent<BoxCollider2D>();
		lowerBound = GameObject.Find("Lowerbound").GetComponent<BoxCollider2D>();

		x  = Random.Range(0,10); // Generates randomization for all analog inputs

		debugBox1 = GameObject.Find("DebugText1");
		debugBox2 = GameObject.Find("DebugText2");

		if (debugText1 && debugText2) {
			debugText1 = debugBox1.GetComponent<Text>();
			debugText2 = debugBox2.GetComponent<Text>();
		}
	}

	// Update is called once per frame
	void Update () {

		if (pressed) {
			levelTimer += Time.deltaTime;
		}

		if (inputType == "analog") {
			lerpedColor = Color.Lerp(Color.white, Color.green, inputValue / 5);
			spritRend.color = lerpedColor;
		} else if (inputType == "digital") {
			if (inputValue == 5) {
				spritRend.color = Color.green;			
			} else if (inputValue == 0) {
				spritRend.color = Color.white;			
			}
		}

		forwardInput (inputValue, outputs);
	}

	public void startSine() {
		InvokeRepeating("voltAmplitude", 0.07f, .07f);
	}

	void voltAmplitude(){

		inputValue = Mathf.Abs(Mathf.Sin (x)*2.505f + 2.5f);
		x += increment;
	}

	private void SwitchDot () {

		if (inputValue == 0) {
			inputValue = 5;
		} else if (inputValue == 5) {
			inputValue = 0;
		}
	}
		
	void OnMouseDown () {

		pressed = true;

		// instantiate Line after clicking circle
		line = Instantiate (Resources.Load("LinePrefab")) as GameObject;
		line.name = "Line_(" + line.GetHashCode() + ")";
		if (debugBox1) {
			debugText1.text = "Instantiated " + line.name;
		}
		line.GetComponent<Bezier_Spline> ().originObject = this.gameObject;

		Manager.currentlyDrawnLine = newLineObj;
	}

	void OnMouseDrag () {

		if (collisionObject && collisionObject.CompareTag("breadboardLeft") ) {
			line.GetComponent<LineRenderer>().enabled = true;
		}
//
		Vector2 screenPos = new Vector2 ();				
		Camera.main.ScreenToWorldPoint (screenPos);

		clampVector = new Vector3 ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x,
			Mathf.Clamp ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y,
				lowerBound.bounds.max.y,
				upperBound.bounds.min.y),
			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).z);

		line.GetComponent<LineRenderer>().SetPosition(0, transform.position);
		line.GetComponent<LineRenderer> ().SetPosition (1, clampVector);

		collisionObject = Physics2D.OverlapPoint (Camera.main.ScreenToWorldPoint (Input.mousePosition));

		line.GetComponent<Bezier_Spline>().tangent2.transform.position = new Vector3 (
			((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x - 1),
			Mathf.Clamp ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y,
				lowerBound.bounds.max.y,
				upperBound.bounds.min.y),
			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).z);

		if ((collisionObject && collisionObject.CompareTag("inputPin")) ||
			collisionObject && collisionObject.CompareTag("outputPin")) {
			line.GetComponent<Bezier_Spline>().controlPoints[4] = collisionObject.gameObject;
			line.GetComponent<Bezier_Spline>().controlPoints[5] = collisionObject.gameObject;
		} else if (!collisionObject) {
			line.GetComponent<Bezier_Spline>().controlPoints[4] = line.GetComponent<Bezier_Spline>().mouseFollower;
			line.GetComponent<Bezier_Spline>().controlPoints[5] = line.GetComponent<Bezier_Spline>().mouseFollower;
		}
	}
		
	void OnMouseUp () {

//		if (levelTimer < 0.25 && inputType == "") {
//
//			// insert transform code here to make it appear close to pin
//			triangle.transform.position = new Vector3(transform.position.x + 0.59f, transform.position.y - 0.33f, transform.position.z);
//			ADPanel.transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y - 0.9f, transform.position.z);
//
//			UIcanvas.enabled = true;
//			triangle.enabled = true;
//			Manager.currentInputPin = this.gameObject;
//			debugText2.text = "About to destroy line " + line.name;
////			Destroy(line);
//			DestroyImmediate(line);
//			// ***here is where the already drawn and connected line is destroyed***
//		} else if (levelTimer < 0.25 && inputType == "digital") {
//			SwitchDot();
//			Destroy(line);
//		} else if (levelTimer > 0.25 && inputType != "" && (collisionObject && collisionObject.gameObject == this.gameObject)) {
//			Handheld.Vibrate();
//			triangle.transform.position = new Vector3(transform.position.x + 0.59f, transform.position.y - 0.33f, transform.position.z);
//			ADPanel.transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y - 0.9f, transform.position.z);
//
//			UIcanvas.enabled = true;
//			triangle.enabled = true;
//			Manager.currentInputPin = this.gameObject;
//			Destroy(line);
//		}


		if (levelTimer < 0.25 && inputType == "") {
			if (!UIcanvas.enabled) {
				setTriangleAndADPanelPositions(this.transform);
				popADPanel();
				Manager.currentInputPin = this.gameObject;
			}
		} else if (levelTimer < 0.25 && inputType == "digital") {
			SwitchDot();
		} else if (levelTimer > 0.25 && inputType != "" && (collisionObject && collisionObject.gameObject == this.gameObject)) {
			Debug.Log("A");
			setTriangleAndADPanelPositions(this.transform);
			popADPanel();
			Manager.currentInputPin = this.gameObject;
			Destroy(line);
			Debug.Log("A");
		}

		levelTimer = 0;
		pressed = false;

		if (collisionObject && collisionObject.CompareTag("inputPin")) {
			if (collisionObject.GetComponent<FuncBlockInputPin>().connectedLine) {
				// Line is already connected
				Destroy(collisionObject.GetComponent<FuncBlockInputPin>().connectedLine.gameObject);
				Debug.Log("C");
				collisionObject.GetComponent<FuncBlockInputPin>().connectedLine = line;
				line.GetComponent<Bezier_Spline>().destinObject = collisionObject.gameObject;
				line.GetComponent<Bezier_Spline>().originObject = this.gameObject;

				if (inputType == "") {
					setTriangleAndADPanelPositions(this.transform);
					popADPanel();
					Manager.currentInputPin = this.gameObject;
				}
			} else {
				// No line connected
				collisionObject.GetComponent<FuncBlockInputPin>().connectedLine = line;
				line.GetComponent<Bezier_Spline>().destinObject = collisionObject.gameObject;
				line.GetComponent<Bezier_Spline>().originObject = this.gameObject;
				line.GetComponent<Bezier_Spline>().isEndingPointSnapped = true;

				if (inputType == "") {
					setTriangleAndADPanelPositions(this.transform);
					popADPanel();
					Manager.currentInputPin = this.gameObject;
				}
			}
		} else if (collisionObject && collisionObject.CompareTag("outputPin")) {
			if (collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine) {
				Destroy(collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine.gameObject);
				Debug.Log("D");
				collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine = line;
				line.GetComponent<Bezier_Spline>().destinObject = collisionObject.gameObject;
				line.GetComponent<Bezier_Spline>().originObject = this.gameObject;

				if (inputType == "") {
					setTriangleAndADPanelPositions(this.transform);
					popADPanel();
					Manager.currentInputPin = this.gameObject;
				}

			} else {
				collisionObject.GetComponent<BreadBoardOutputPin>().connectedLine = line;
				line.GetComponent<Bezier_Spline>().destinObject = collisionObject.gameObject;
				line.GetComponent<Bezier_Spline>().originObject = this.gameObject;
				line.GetComponent<Bezier_Spline>().isEndingPointSnapped = true;

				if (inputType == "") {
					setTriangleAndADPanelPositions(this.transform);
					popADPanel();
					Manager.currentInputPin = this.gameObject;
				}
			}
		} else if (!collisionObject
		          || !collisionObject.CompareTag("outputPin")
		          || !collisionObject.CompareTag("inputPin")
		          || !collisionObject.CompareTag("output")) {
			Destroy(line);
			Debug.Log("B");
		}

		Manager.currentlyDrawnLine = null;
	}

	public void forwardInput (float input, float[] outputs) {
		for (int i = 0; i <= outputs.Length-1; i++) {
			outputs[i] = inputValue;
		}
	}

	void setTriangleAndADPanelPositions(Transform t) {
		triangle.transform.position = new Vector3 (t.position.x + 0.59f, t.position.y - 0.33f, t.position.z);
		ADPanel.transform.position = new Vector3(t.position.x + 2.5f, t.position.y - 0.9f, t.position.z);
	}

	void popADPanel() {
		UIcanvas.enabled = true;
		triangle.enabled = true;
	}
}
