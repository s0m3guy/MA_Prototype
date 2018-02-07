using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLEDScript : MonoBehaviour {

	private bool isOn = true;
	private SpriteRenderer spritRend;
	private Sprite sprite_LED_off, sprite_LED_on;

	void Awake () {
		spritRend = gameObject.GetComponent<SpriteRenderer> ();
		sprite_LED_off = Resources.Load ("LED_off", typeof (Sprite)) as Sprite;
		sprite_LED_on = Resources.Load ("LED_on", typeof(Sprite)) as Sprite;
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating("SwitchDot", 2.0f, 2.0f);
	}

	// Update is called once per frame
	void Update () {
		if (isOn) {
			spritRend.sprite = sprite_LED_on;
		} else {
			spritRend.sprite = sprite_LED_off;
		}
	}

	private void SwitchDot () {
		isOn = !isOn;
	}
}
