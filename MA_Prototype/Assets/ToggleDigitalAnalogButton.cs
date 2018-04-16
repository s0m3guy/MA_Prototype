using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleDigitalAnalogButton : MonoBehaviour {

	public bool isOn;
	string status;

	void Awake() {

		// Better readability than "isOn"
		if (isOn) {
			status = "digital";
		} else {
			status = "analog";
		}
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setToggleStatus(bool status) {
		isOn = status;
	}
}
