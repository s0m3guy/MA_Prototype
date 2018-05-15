using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadBoardInputPin : MonoBehaviour {

	// Instantiated line and object in order to alter EdgeCollider2D points
	GameObject line;
	Vector2[] tempEdges;

	public float inputValue = 0;
	public float[] outputs;
	public string inputType = "";

	// Needed for analog input
	public float sineValue = 0;
	float increment = 0.07f;
	public Color lerpedColor;
	float x;

	// Variables for short click detection
	float levelTimer = 0.0f;
	bool pressed = false;

	[SerializeField]
	Canvas UIcanvas;
	[SerializeField]
	GameObject ADPanel;
	[SerializeField]
	SpriteRenderer triangle;

	void Awake() {
		outputs = new float[1];
	}

	void Start() {
		x  = Random.Range(0,10); // Generates randomization for all analog inputs
	}

	void Update() {
		if (pressed) {
			levelTimer += Time.deltaTime;
		}

		if (inputType == "analog") {
			lerpedColor = Color.Lerp(Color.white, Color.green, inputValue / 5);
			GetComponent<SpriteRenderer>().color = lerpedColor;
		} else if (inputType == "digital") {
			if (inputValue == 5) {
				GetComponent<SpriteRenderer>().color = Color.green;			
			} else if (inputValue == 0) {
				GetComponent<SpriteRenderer>().color = Color.white;			
			}
		}

		forwardInput (inputValue, outputs);
	}

	void OnMouseDown () {

		// instantiate Line after clicking circle
		line = Instantiate (Resources.Load("LinePrefab")) as GameObject;
		Manager.currentlyDrawnLine = line.gameObject;
	}

	void OnMouseDrag () {

		if (!Manager.collisionDetected) {
			Vector2 screenPos = new Vector2 ();
			Camera.main.ScreenToWorldPoint (screenPos);

			line.GetComponent<LineRenderer> ().SetPosition (0,
				new Vector3 (transform.position.x + (GetComponent<SpriteRenderer> ().bounds.size.x) / 2,
					transform.position.y,
					transform.position.z));
			line.GetComponent<LineRenderer> ().SetPosition (1, Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10);

			tempEdges = line.GetComponent<EdgeCollider2D> ().points;
			tempEdges [0] = new Vector2 (
				transform.position.x + (GetComponent<SpriteRenderer> ().bounds.size.x) / 2 - 0.7f,
				transform.position.y - 0.217f);
			tempEdges [1] = new Vector2 (
				(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x - 0.7f,
				(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y - 0.217f);

			line.GetComponent<EdgeCollider2D> ().points = tempEdges;
		}
	}

	void OnMouseUp() {

		if (levelTimer < 0.25 && inputType == "") {

			// insert transform code here to make it appear close to pin
			triangle.transform.position = new Vector3(transform.position.x+0.59f, transform.position.y-0.33f, transform.position.z);
			ADPanel.transform.position = new Vector3(transform.position.x + 2.5f, transform.position.y - 0.9f, transform.position.z);

			UIcanvas.enabled = true;
			triangle.enabled = true;
			Manager.currentInputPin = this.gameObject;

		} else if (levelTimer < 0.25 && inputType == "digital") {
			SwitchDot();
		} else if (levelTimer < 0.25 && inputType == "analog") {

		}

		levelTimer = 0;
		pressed = false;

		Manager.currentlyDrawnLine = null;
	}

	public void forwardInput (float input, float[] outputs) {
		for (int i = 0; i <= outputs.Length-1; i++) {
			outputs[i] = inputValue;
		}
	}

	private void SwitchDot () {

		if (inputValue == 0) {
			inputValue = 5;
		} else if (inputValue == 5) {
			inputValue = 0;
		}
	}

	public void startSine() {
		InvokeRepeating("voltAmplitude", 0.07f, .07f);
	}
}
