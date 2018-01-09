using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomInputDot : MonoBehaviour {

	public bool isOn = true;
	private SpriteRenderer spritRend;
	private Sprite sprite_dot_off, sprite_dot_on;

	private Line line, newLineScript;
	private LineRenderer lineRenderer = new LineRenderer ();
	private LineRenderer newLineRend = new LineRenderer();
	public GameObject newLineObj;
	public Transform origin;

	public int[] inputs, outputs;


	void Awake () {
		spritRend = gameObject.GetComponent<SpriteRenderer> ();
		sprite_dot_off = Resources.Load ("connecting_dot_inactive", typeof (Sprite)) as Sprite;
		sprite_dot_on = Resources.Load ("connecting_dot_active", typeof(Sprite)) as Sprite;

		origin = GetComponent<Transform> ();
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating("SwitchDot", 2.0f, 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (isOn) {
			spritRend.sprite = sprite_dot_off;
		} else {
			spritRend.sprite = sprite_dot_on;
		}
	}

	private void SwitchDot () {
		isOn = !isOn;
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
		lineRenderer.SetPosition (1, Camera.main.ScreenToWorldPoint(Input.mousePosition));

		Manager.MouseLineScript = newLineScript; // Set reference to current drawn line
		Manager.MouseLineRenderer = newLineRend;
	}

	void OnMouseDown () {

		// instantiate Line after clicking circle

		newLineObj = Instantiate (Resources.Load("LinePrefab")) as GameObject;

		if (newLineObj) {
			newLineRend = newLineObj.GetComponent<LineRenderer> ();
			newLineScript = newLineObj.GetComponent<Line> ();
		}
	}

	public void forwardInput (GameObject[] inputs, GameObject[] output) {
		for (int i = 0; i <= output.Length; i++) {
			output [i] = inputs [0];
		}
	}
}
