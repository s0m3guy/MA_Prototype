using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class AnalogInputDot : MonoBehaviour {

	float value = 0;
	bool up = true;
	float limit = 5;
	float increment = 0.1f;
	public Color lerpedColor = Color.white;
	private float voltProportion;


	// Use this for initialization
	void Start () {
		voltProportion = value / limit;

		InvokeRepeating("voltAmplitude", 0.07f, .07f);
	}
	
	// Update is called once per frame
	void Update () {
		lerpedColor = Color.Lerp (Color.white, Color.red, value / limit);

		GameObject.FindGameObjectWithTag ("inputDotAnalog").GetComponent<SpriteRenderer> ().color = lerpedColor;
		GameObject.FindGameObjectWithTag ("testingLED").GetComponent<Text> ().text = value.ToString() + "V";
	}

	void voltAmplitude(){

		if (up == true && value <= limit) {
			value += increment;

			if (value == limit) {
				up = false;
			}
		} else {
			up = false;
			value -= increment;

			if (value <= 0.0) {
				up = true;
			}
		}
	}
}
