using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class AnalogInputDot : MonoBehaviour {

	float value = 0;
//	bool up = true;
//	float limit = 2*Mathf.PI;
	float increment = 0.01f;
	public Color lerpedColor = Color.white;
	float x = 0;


	// Use this for initialization
	void Start () {

		InvokeRepeating("voltAmplitude", 0.07f, .07f);
	}
	
	// Update is called once per frame
	void Update () {
		lerpedColor = Color.Lerp (Color.white, Color.red, value / 5);

		GameObject.FindGameObjectWithTag ("inputDotAnalog").GetComponent<SpriteRenderer> ().color = lerpedColor;
//		GameObject.FindGameObjectWithTag ("testingLED").GetComponent<Text> ().text = value.ToString() + "V";
	}

	void voltAmplitude(){

		value = Mathf.Abs(Mathf.Sin (x)*2.505f + 2.5f);
		x += increment;

//		if (value <= 5.00f) {
//			Debug.Log (Mathf.Sin (Mathf.PI/2)*2.5f + 2.5f);
//		}

//		if (x > limit) {
//			x = 0;
//		}
	}
}
