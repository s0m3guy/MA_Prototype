﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomInputDot : MonoBehaviour {

	public float inputValue = 0;
	private SpriteRenderer spritRend;
//	private Sprite sprite_dot_off, sprite_dot_on;

	private Line line, newLineScript;
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

	public string inputType;

	// Needed for analog input
	public float sineValue = 0;
	float increment = 0.07f;
	public Color lerpedColor;
	float x;

	void Awake () {
		spritRend = gameObject.GetComponent<SpriteRenderer> ();

		origin = GetComponent<Transform> ();

		outputs = new float[1];
	}

	// Use this for initialization
	void Start () {
		x  = Random.Range(0,10); // Generates randomization for all analog inputs
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

	void OnMouseDrag () {

		line = newLineObj.GetComponent<Line>();

		line.originObject = this.gameObject;

		lineRenderer = newLineObj.gameObject.GetComponent<LineRenderer> ();

		Vector2 screenPos = new Vector2();
		Camera.main.ScreenToWorldPoint (screenPos);

		lineRenderer.SetPosition (0,
			new Vector3 (origin.position.x + (GetComponent<SpriteRenderer>().bounds.size.x)/2,
				origin.position.y,
				origin.position.z));
		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition)+Vector3.forward*10);
	}

	void OnMouseDown () {

		pressed = true;

		// instantiate Line after clicking circle
		newLineObj = Instantiate (Resources.Load("LinePrefab")) as GameObject;

		if (newLineObj) {
			newLineRend = newLineObj.GetComponent<LineRenderer> ();
			newLineScript = newLineObj.GetComponent<Line> ();
		}

		Manager.currentlyDrawnLine = newLineObj;
	}

	public void forwardInput (float input, float[] outputs) {
		for (int i = 0; i <= outputs.Length-1; i++) {
			outputs[i] = inputValue;
		}
	}

	void OnMouseUp () {

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

		if (!newLineScript.isEndingPointSnapped) {
			Destroy (Manager.currentlyDrawnLine.gameObject);
		}

		Manager.currentlyDrawnLine = null;
	}
}
