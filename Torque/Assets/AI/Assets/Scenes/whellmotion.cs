using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whellmotion : MonoBehaviour {
    public WheelCollider targetwheel;
    private Vector3 whellposition = new Vector3();
    private Quaternion wheelrotation = new Quaternion();

	
	// Update is called once per frame
	void Update () {
        targetwheel.GetWorldPose(out whellposition, out wheelrotation);
        transform.position = whellposition;
        transform.rotation = wheelrotation;
	}
}
