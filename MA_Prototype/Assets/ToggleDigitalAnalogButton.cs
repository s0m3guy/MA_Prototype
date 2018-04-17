using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ToggleDigitalAnalogButton : MonoBehaviour {

	public bool isOn;
	[SerializeField]
	string status;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		// Better readability than "isOn"
		if (isOn) {
			status = "digital";
		} else {
			status = "analog";
		}
		
	}

	public void setToggleStatus(bool status) {
		isOn = status;
	}

	public void TaskOnClick()
	{
		Manager.currentInputPin.GetComponent<RandomInputDot>().inputType = status;
		if (status == "analog") {
			Manager.currentInputPin.GetComponent<RandomInputDot>().startSine();
		}
		transform.parent.parent.GetComponent<Canvas>().enabled = false;
		Manager.currentInputPin = null;
	}


//	public void TaskOnClick()
//	{
//		Debug.Log("You have clicked the button!");
//	}


}
