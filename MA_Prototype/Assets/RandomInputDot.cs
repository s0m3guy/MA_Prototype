using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInputDot : MonoBehaviour {

	public int value = 0;
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

	// Variables for short click detection
	float levelTimer = 0.0f;
	bool pressed = false;

	public string inputType;

	void Awake () {
		spritRend = gameObject.GetComponent<SpriteRenderer> ();
		sprite_dot_off = Resources.Load ("connecting_dot_inactive", typeof (Sprite)) as Sprite;
		sprite_dot_on = Resources.Load ("connecting_dot_active", typeof(Sprite)) as Sprite;

		origin = GetComponent<Transform> ();

		outputs = new int[1];
	}

	// Use this for initialization
	void Start () {
//		InvokeRepeating("SwitchDot", 2.0f, 2.0f);
	}
	
	// Update is called once per frame
	void Update () {

		if (pressed) {
			levelTimer += Time.deltaTime;
		}

		if (value == 1) {
			spritRend.sprite = sprite_dot_on;
		} else if (value == 0) {
			spritRend.sprite = sprite_dot_off;
		}
			
		forwardInput (value, outputs);

	}

	private void SwitchDot () {

		if (value == 1) {
			value = 0;
		} else if (value == 0) {
			value = 1;
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
			outputs[i] = value;
		}
	}

	void OnMouseUp () {

		if (levelTimer < 0.25) {
//			SwitchDot();
			UIcanvas.enabled = true;
			Manager.currentInputPin = this.gameObject;
		}

		levelTimer = 0;
		pressed = false;

		if (!newLineScript.isEndingPointSnapped) {
			Destroy (Manager.currentlyDrawnLine.gameObject);
		}

		Manager.currentlyDrawnLine = null;
	}
}
