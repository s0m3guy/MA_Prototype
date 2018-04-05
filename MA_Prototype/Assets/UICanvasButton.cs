using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasButton : MonoBehaviour {

	public Button yourButton;
	public GameObject chosenDropdownEntry;
	public GameObject userInputValue;

	string comparator, value;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		comparator = chosenDropdownEntry.GetComponent<Text>().text;

		switch (comparator) {
			case "Größer": 
				comparator = ">";
				break;
			case "Kleiner":
				comparator = "<";
				break;
			case "Kleiner Gleich": 
				comparator = "≤";
				break;
			case "Größer Gleich": 
				comparator = "≥";
				break;
			case "Gleich": 
				comparator = "=";
				break;
		}

		value = userInputValue.GetComponent<Text>().text;
		Debug.Log("You have clicked the button!");
		Debug.Log(chosenDropdownEntry.GetComponent<Text>().text);
		Debug.Log(userInputValue.GetComponent<Text>().text);
		Manager.currentIFblock.GetComponentInChildren<Text>().text = comparator + value + "V";
		transform.parent.parent.GetComponent<Canvas>().enabled = false;
		Manager.currentIFblock = null;
	}
}
