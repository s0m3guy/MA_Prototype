using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class AnalogInputDot : MonoBehaviour {

	public float value = 0;
	float increment = 0.07f;
	public Color lerpedColor = Color.white;
	float x;

	public SpriteRenderer sr;

	// Use this for initialization
	void Start () {

//		sr.color = new Color(1, 0.302f, 0.208f, 1.000f);

		x  = Random.Range(0,10); // Generates randomization for all analog inputs
		InvokeRepeating("voltAmplitude", 0.07f, .07f);
	}
	
	// Update is called once per frame
	void Update () {
		lerpedColor = Color.Lerp (Color.white, Color.green, value / 5);
//		lerpedColor = Color.Lerp(new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f), new Color(1, 0.302f, 0.208f, 1.000f), value / 5f);
//		sr.color = lerpedColor;
		GetComponent<SpriteRenderer>().color = lerpedColor;
	}

	void voltAmplitude(){

		value = Mathf.Abs(Mathf.Sin (x)*2.505f + 2.5f);
		x += increment;
	}
}
