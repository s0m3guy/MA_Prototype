using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICanvasButton : MonoBehaviour {

	public Button yourButton;
	public GameObject chosenDropdownEntry;
	public GameObject userInputValue;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		Debug.Log("You have clicked the button!");
		Debug.Log(chosenDropdownEntry.GetComponent<Text>().text);
		Debug.Log(userInputValue.GetComponent<Text>().text);
	}
}
