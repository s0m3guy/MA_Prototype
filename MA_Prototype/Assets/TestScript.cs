using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour {

	[SerializeField]
	Image img;

	[SerializeField]
	SpriteRenderer sr;

	Color32 clr = new Color32(0x71, 0x82, 0x9D, 0xFF);

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	img.color = clr;
	sr.color = clr;
//		img.color = Color.red;
//		sr.color = Color.red;
	}
}
