﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomInputDot : MonoBehaviour {

	public int inputValue = 0;
	private SpriteRenderer spritRend;
	private Sprite sprite_dot_off, sprite_dot_on;

	private Line line, newLineScript;
	private LineRenderer lineRenderer = new LineRenderer ();
	private LineRenderer newLineRend = new LineRenderer();
	public GameObject newLineObj;
	public Transform origin;

	public int[] outputs;

	[SerializeField]
	Canvas UIcanvas;
	[SerializeField]
	GameObject ADPanel;

	[SerializeField]
	float variable;

	// Variables for short click detection
	float levelTimer = 0.0f;
	bool pressed = false;

	public string inputType;

	Vector3 switchUIspawnPosition;

	// Needed for analog input
	public float sineValue = 0;
	float increment = 0.07f;
	public Color lerpedColor = Color.white;
	float x;

	void Awake () {
		switchUIspawnPosition = new Vector3 (transform.position.x+2.6f, transform.position.y-1, transform.position.z);

		spritRend = gameObject.GetComponent<SpriteRenderer> ();
		sprite_dot_off = Resources.Load ("connecting_dot_inactive", typeof (Sprite)) as Sprite;
		sprite_dot_on = Resources.Load ("connecting_dot_active", typeof(Sprite)) as Sprite;

		origin = GetComponent<Transform> ();

		outputs = new int[1];
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

		if (inputValue == 1) {
			spritRend.sprite = sprite_dot_on;
		} else if (inputValue == 0) {
			spritRend.sprite = sprite_dot_off;
		}
			
		forwardInput (inputValue, outputs);

		lerpedColor = Color.Lerp (Color.white, Color.green, sineValue / 5);
		GetComponent<SpriteRenderer>().color = lerpedColor;
	}

	public void startSine() {
		InvokeRepeating("voltAmplitude", 0.07f, .07f);
	}

	void voltAmplitude(){

		sineValue = Mathf.Abs(Mathf.Sin (x)*2.505f + 2.5f);
		x += increment;
	}

	private void SwitchDot () {

		if (inputValue == 1) {
			inputValue = 0;
		} else if (inputValue == 0) {
			inputValue = 1;
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

	public void forwardInput (int input, int[] outputs) {
		for (int i = 0; i <= outputs.Length-1; i++) {
			outputs[i] = inputValue;
		}
	}

	void OnMouseUp () {

		if (levelTimer < 0.25 && inputType == "") {

			// insert transform code here to make it appear close to pin
			ADPanel.transform.position = switchUIspawnPosition;

			UIcanvas.enabled = true;
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
