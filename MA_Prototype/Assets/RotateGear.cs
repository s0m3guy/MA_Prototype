using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGear : MonoBehaviour {

	public GameObject gear1;
	public GameObject gear2;
	public int speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		gear1.transform.Rotate(0, 0, speed);
		gear2.transform.Rotate(0, 0, speed*-1);
	}
}
