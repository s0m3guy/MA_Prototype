using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ToggleDigitalAnalogButton : MonoBehaviour {

	public bool isOn;
	[SerializeField]
	string status;

	[SerializeField]
	SpriteRenderer sr;

	BreadBoardInputPin bbInputPinScript;

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
		bbInputPinScript = Manager.currentInputPin.GetComponent<BreadBoardInputPin>();

		if (status == "analog") {				// if new status is analog
			bbInputPinScript.startSine();
		} else if (status == "digital") {		// if new status is digital
			bbInputPinScript.CancelInvoke();
			if (bbInputPinScript.inputType == "analog") {
				bbInputPinScript.inputValue = 0;
			}
		}
		bbInputPinScript.inputType = status;

		transform.parent.parent.GetComponent<Canvas>().enabled = false;
		sr.enabled = false;
		Manager.currentInputPin = null;
	}


//	public void TaskOnClick()
//	{
//		Debug.Log("You have clicked the button!");
//	}


}
