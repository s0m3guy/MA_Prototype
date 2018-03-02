using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputDot : MonoBehaviour {

	// Quick and dirty class for testing of output pins, which are currently
	// connected to a testing LED

	private CircleCollider2D circCol, newCircCol;
	private SpriteRenderer spritRend;
	private Sprite sprite_LED_off, sprite_LED_on;
	public int input;
	private Vector2[] tempEdgeColliderPoints;

	private Line line;


	void Awake () {

		circCol = GetComponent<CircleCollider2D> ();

		sprite_LED_off = Resources.Load ("LED_off_raw", typeof (Sprite)) as Sprite;
		sprite_LED_on = Resources.Load ("LED_on_raw", typeof(Sprite)) as Sprite;

		spritRend = GameObject.FindGameObjectWithTag ("testingLED").GetComponent<SpriteRenderer> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

		if(circCol.bounds.Contains(mousePos)) {
			if (Manager.MouseLineRenderer) {
				Manager.MouseLineRenderer.SetPosition (1, this.transform.position);
				// Also set end point of Edge Collider
				tempEdgeColliderPoints = Manager.MouseLineEdgeCollider.points;
				tempEdgeColliderPoints [1] = transform.position;
				Manager.MouseLineEdgeCollider.points = tempEdgeColliderPoints;	
				Manager.MouseLineScript.isEndingPointSnapped = true;
			}
		}


		if (input == 1) {
			spritRend.sprite = sprite_LED_on;
		} else if (input == 0) {
			spritRend.sprite = sprite_LED_off;
		}
	}

	void OnMouseEnter() {

		if (Manager.MouseLineScript != null) {
			Manager.MouseLineScript.destinObject = this.gameObject;
		}
	}
}
