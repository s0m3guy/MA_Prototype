using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour {

	public void setValue(float value) {
		GetComponent<Text>().text = value.ToString("0.0");
	}
}
