using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class FunctionBlock : MonoBehaviour {

	public Transform block, block2, clone;
	public int speed;
	private bool isClone = false;

	public string logicGate;

	public int[] inputs;
	public int output;

	private Vector3 screenPoint;
	private Vector3 offset;

	GameObject input1GO; 
	GameObject input2GO;
	GameObject outputGO;

	void Awake () {

		inputs = new int[transform.childCount - 2];		// Total amount minus canvas+output equals the amount of inputs

		input1GO = transform.Find ("Input 1A").gameObject;
		input2GO = transform.Find ("Input 2A").gameObject;
		outputGO = transform.Find ("OutputA").gameObject;
	}

	// Use this for initialization
	void Start () {

//		if (gameObject.transform.parent.name.Contains ("(Clone)")) {
//			isClone = true;
//		}
			
		block = transform.parent.gameObject.transform;
		block2 = this.GetComponentInParent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		forwardInput ();

//		GameObject input1GO = this.transform.Find ("Input 1A").gameObject;
//		GameObject input2GO = this.transform.Find ("Input 2A").gameObject;
//		GameObject outputGO = this.transform.Find ("OutputA").gameObject;

		if (inputs [0] == 0) {
				input1GO.GetComponent<SpriteRenderer> ().color = Color.white;
		} else if (inputs[0] == 1) {
				input1GO.GetComponent<SpriteRenderer> ().color = Color.green;
			}

		if (inputs [1] == 0) {
			input2GO.GetComponent<SpriteRenderer> ().color = Color.white;
		} else if (inputs[1] == 1) {
			input2GO.GetComponent<SpriteRenderer> ().color = Color.green;
		}

		if (output == 0) {
			outputGO.GetComponent<SpriteRenderer> ().color = Color.white;
		} else if (output == 1) {
			outputGO.GetComponent<SpriteRenderer> ().color = Color.green;
		}
	}

	void OnMouseDown() {
		if (!isClone) {
			
			clone = Instantiate (block);
			clone.GetComponentInChildren<FunctionBlock> ().isClone = true;
		}
		screenPoint = Camera.main.WorldToScreenPoint (gameObject.transform.parent.position);

			offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag() {
		Vector3 curScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);			// Current touch point

		Vector3 curPosition = Camera.main.ScreenToWorldPoint (curScreenPoint) + offset;								// Current touch point converted to point in scene
		if (!isClone) {
			clone.position = curPosition;																			// Move clone to this position
		} else {
			transform.position = curPosition;
		}
	}
		

	public void forwardInput () {

		// For Testing, no logic

		if (transform.parent.name.Contains ("_AND")) {
			if (inputs [0] == 0 || inputs [1] == 0) {
				output = 0;
			} else if (inputs [0] == 1 && inputs [1] == 1) {
				output = 1;
			}
		} else if (transform.parent.name.Contains ("_OR")) {
			if (inputs [0] == 1 || inputs [1] == 1) {
				output = 1;
			} else if (inputs [0] == 0 && inputs [1] == 0) {
				output = 0;
			}
		} else if (transform.parent.name.Contains ("_XOR")) {
			if ((inputs [0] == 1 && inputs [1] == 0) || (inputs [0] == 0 && inputs [1] == 1)) {
				output = 1;
			} else if ((inputs [0] == 0 && inputs [1] == 0) || (inputs [0] == 1 && inputs [1] == 1)) {
				output = 0;
			}
		}
	}

	public bool checkClone () {
		return isClone;
	}

	public void setCloneStatus (bool cloneStatus) {
		isClone = cloneStatus;
	}
}
