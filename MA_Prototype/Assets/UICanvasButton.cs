using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasButton : MonoBehaviour {

	public Button yourButton;
	public GameObject chosenDropdownEntry;
	public GameObject userInputValue;

	string comparator, value;
	float floatValue;

	BoxCollider2D panelCollider;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);

		panelCollider = GetComponentInParent<BoxCollider2D>();
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
		floatValue = float.Parse(userInputValue.GetComponent<Text>().text);
		if (Manager.currentIFblock) {
			Manager.currentIFblock.GetComponentInChildren<Text>().text = "WENN \n" + comparator + value + "V?";
			Manager.currentIFblock.GetComponentInChildren<FunctionBlock>().comparator = comparator;
			Manager.currentIFblock.GetComponentInChildren<FunctionBlock>().comparatorValue = floatValue;
		}
		panelCollider.enabled = false;
		transform.parent.parent.GetComponent<Canvas>().enabled = false;
		Manager.currentIFblock = null;
	}
}
