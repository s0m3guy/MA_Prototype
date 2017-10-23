using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDotScript : MonoBehaviour {

	public bool isOn = true;
	private SpriteRenderer spritRend;
	private Sprite sprite_dot_off, sprite_dot_on;

	void Awake () {
		spritRend = gameObject.GetComponent<SpriteRenderer> ();
		sprite_dot_off = Resources.Load ("connecting_dot_inactive", typeof (Sprite)) as Sprite;
		sprite_dot_on = Resources.Load ("connecting_dot_active", typeof(Sprite)) as Sprite;
	}

	// Use this for initialization
	void Start () {
		InvokeRepeating("SwitchDot", 2.0f, 2.0f);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (isOn);
		if (isOn) {
			spritRend.sprite = sprite_dot_off;
		} else {
			spritRend.sprite = sprite_dot_on;
		}
	}

	private void SwitchDot () {
		isOn = !isOn;
	}
}
