using UnityEngine;
using System.Collections;

public class RG_ContentSize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Vector3 position = GetComponent<RectTransform> ().localPosition;
		position.x = Screen.width * 0.06f;
		GetComponent<RectTransform> ().localPosition = position;
		//	lpos = GetComponent<RectTransform> ().localPosition;
		//	lpos.x = 0;//(Screen.width / 1.54f) / 2;
		//GetComponent<RectTransform> ().localPosition = lpos;
		// position = width/5.71
		//position = width/5.71
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
