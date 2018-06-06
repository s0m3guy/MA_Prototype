using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier_Spline : MonoBehaviour {

	public GameObject originObject;
	public GameObject destinObject;

	private BreadBoardInputPin breadboardInputPinScript;
	private FunctionBlock functionBlockScript;
	private BreadBoardOutputPin breadboardOutputPinScript;
	BreadBoardInputPin tempBreadBoardInput;

	public float input, output;

	public bool isEndingPointSnapped = false;

	// For Belzier Spline
	public List<GameObject> controlPoints = new List<GameObject>();
	public Color color = Color.blue;
	public float width = 2.1f;
	public int numberOfPoints = 20;
	LineRenderer lineRenderer;
	public GameObject tangent1, tangent2, mouseFollower;

	Vector3 clampVector;
	BoxCollider2D upperBound, lowerBound;

	void Start() {

		upperBound = GameObject.Find("Upperbound").GetComponent<BoxCollider2D>();
		lowerBound = GameObject.Find("Lowerbound").GetComponent<BoxCollider2D>();

		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.useWorldSpace = true;
//		lineRenderer.material = new Material (Shader.Find ("Particles/Additive"));

		tangent1 = Instantiate (Resources.Load ("Tangent")) as GameObject;
		tangent2 = Instantiate (Resources.Load ("Tangent")) as GameObject;
		tangent1.name = "tangent1";
		tangent2.name = "tangent2";

		tangent1.transform.parent = this.transform;;
		tangent2.transform.parent = this.transform;

		mouseFollower = GameObject.Find ("mouseFollower");

		controlPoints.Add (originObject);
		controlPoints.Add (originObject);
		controlPoints.Add (tangent1);
		controlPoints.Add (tangent2);
		controlPoints.Add (mouseFollower);
		controlPoints.Add (mouseFollower);

		tangent1.transform.position = originObject.transform.position;
		tangent1.transform.position = new Vector3 (
			originObject.transform.position.x+originObject.GetComponent<CircleCollider2D>().bounds.size.x*2,
			originObject.transform.position.y,
			originObject.transform.position.z);
	}

	void Update() {

		generateBezierSpline();

		if (destinObject && originObject && isEndingPointSnapped) {
			//			GetComponent<LineRenderer>().SetPosition(0,
			//				new Vector3 (originObject.transform.position.x + (originObject.GetComponent<SpriteRenderer> ().bounds.size.x) / 2,
			//					originObject.transform.position.y,
			//					originObject.transform.position.z));
//			GetComponent<LineRenderer>().SetPosition(0, originObject.transform.position);
//			GetComponent<LineRenderer>().SetPosition(1, destinObject.transform.position);
		}

		forwardInput(input, output);

		if (originObject) {
			if (originObject.CompareTag("inputDot")) {
				tempBreadBoardInput = originObject.GetComponent<BreadBoardInputPin>();
				if (tempBreadBoardInput.inputType != "") {
					GetComponent<LineRenderer>().startColor = originObject.GetComponent<SpriteRenderer>().color;
					GetComponent<LineRenderer>().endColor = originObject.GetComponent<SpriteRenderer>().color;
				}
			} else if (originObject.CompareTag("output")) {
				GetComponent<LineRenderer>().startColor = originObject.GetComponent<SpriteRenderer>().color;
				GetComponent<LineRenderer>().endColor = originObject.GetComponent<SpriteRenderer>().color;
			}
		}
	}

	// forwardInput() in Line.cs takes input value and copies value to target object
	public void forwardInput (float input, float output) {
		string typeOfOriginObject, typeOfDestinObject;

		// Checking the type of origin
		if (originObject != null) {
			typeOfOriginObject = originObject.gameObject.name;

			if (originObject.CompareTag ("inputDot")) {
				breadboardInputPinScript = originObject.GetComponent<BreadBoardInputPin> ();
				this.output = breadboardInputPinScript.inputValue;
				this.input = breadboardInputPinScript.inputValue;
			} else if (typeOfOriginObject.Contains ("Output")) {
				functionBlockScript = originObject.GetComponentInParent<FunctionBlock> ();
				this.output = functionBlockScript.output;
			} else if (originObject.CompareTag ("inputDotAnalog")) {
				this.output = originObject.GetComponent<AnalogInputDot> ().value;
			}
		}

		// Checking the type of destination
		if (destinObject != null) {
			typeOfDestinObject = destinObject.gameObject.name;
			if (typeOfDestinObject.Contains ("Input")) {
				functionBlockScript = destinObject.GetComponentInParent<FunctionBlock> ();
				if (typeOfDestinObject.Contains ("Input 1")) {
					if (destinObject.transform.parent.gameObject.transform.parent.name.Contains("_VALUE")
						|| destinObject.transform.parent.gameObject.transform.parent.name.Contains ("_IF"))
					{
						functionBlockScript.inputs[0] = this.output;
					} else {
						functionBlockScript.inputs [0] = (int)this.output;
					}
				} else if (typeOfDestinObject.Contains ("Input 2")) {
					functionBlockScript.inputs [1] = (int)this.output;
				} else if (destinObject.transform.parent.name.Contains ("VALUE")) {
					functionBlockScript.inputs [0] = this.output;
				}
			} else if (typeOfDestinObject.Contains ("output_dot")) {
				breadboardOutputPinScript = destinObject.GetComponent<BreadBoardOutputPin> ();
				breadboardOutputPinScript.input = this.output;
			}
		}
	}

	void generateBezierSpline() {

		clampVector = new Vector3 ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x,
			Mathf.Clamp ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y,
				lowerBound.bounds.max.y,
				upperBound.bounds.min.y),
			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).z);

		mouseFollower.transform.position = new Vector3 (
			((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x),
			Mathf.Clamp ((Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y,
				lowerBound.bounds.max.y,
				upperBound.bounds.min.y),
			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).z);

//		mouseFollower.transform.position = new Vector3 (
//			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).x,
//			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).y,
//			(Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10).z);

		if (null == lineRenderer || controlPoints == null || controlPoints.Count < 3)
		{
			return; // not enough points specified
		}
		// update line renderer
		lineRenderer.startColor = color;
		lineRenderer.endColor = color;
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;

		if(numberOfPoints < 2)
		{
			numberOfPoints = 2;
		} 
		lineRenderer.positionCount = numberOfPoints * (controlPoints.Count - 2);

		Vector3 p0, p1 ,p2;
		for(int j = 0; j < controlPoints.Count - 2; j++)
		{
			// check control points
			if (controlPoints[j] == null || controlPoints[j + 1] == null 
				||	controlPoints[j + 2] == null)
			{
				return;  
			}
			// determine control points of segment
			p0 = 0.5f * (controlPoints[j].transform.position 
				+ controlPoints[j + 1].transform.position);
			p1 = controlPoints[j + 1].transform.position;
			p2 = 0.5f * (controlPoints[j + 1].transform.position 
				+ controlPoints[j + 2].transform.position);

			// set points of quadratic Bezier curve
			Vector3 position;
			float t;
			float pointStep = 1.0f / numberOfPoints;
			if (j == controlPoints.Count - 3)
			{
				pointStep = 1.0f / (numberOfPoints - 1.0f);
				// last point of last segment should reach p2
			}  
			for(int i = 0; i < numberOfPoints; i++) 
			{
				t = i * pointStep;
				position = (1.0f - t) * (1.0f - t) * p0 
					+ 2.0f * (1.0f - t) * t * p1 + t * t * p2;
				lineRenderer.SetPosition(i + j * numberOfPoints, position);
			}
		}
	}
}