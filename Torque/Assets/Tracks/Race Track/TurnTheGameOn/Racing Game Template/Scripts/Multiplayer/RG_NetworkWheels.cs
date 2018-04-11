using UnityEngine;
using System.Collections;

public class RG_NetworkWheels : MonoBehaviour {

	public Transform[] frontWheels;
	public Transform[] frontWheelsChild;
	public Transform[] rearWheels;
	//The default rotation values of the wheels
	public Vector3[] fWRotation;
	public Vector3[] rWRotation;
	public RG_SyncData syncVars;
	private float yVelocity;
	public float maxWheelRotation = 45;
	float xRotation;

	// Use this for initialization
	void Start () {

		for (int i = 0; i < frontWheels.Length; i++) {
			fWRotation [i] = frontWheels [i].localEulerAngles;
		}

		for(int i = 0; i < rearWheels.Length; i++){
			rWRotation [i] = rearWheels [i].localEulerAngles;
		}
	}

	// Update is called once per frame
	void Update () {
		for(int i = 0; i < frontWheels.Length; i++){
			Vector3 temp2;
			temp2 = new Vector3 (frontWheels[i].localEulerAngles.x, fWRotation[i].y + (syncVars.horizontalInput * maxWheelRotation), fWRotation[i].z);
			float yAngle = Mathf.SmoothDampAngle (frontWheels[i].localEulerAngles.y, temp2.y, ref yVelocity, 0.07f);
			//float xAngle = Mathf.SmoothDampAngle (frontWheels[i].localEulerAngles.x, temp2.x, ref yVelocity, 0.01f);
			frontWheels [i].localEulerAngles = new Vector3 (temp2.z, yAngle, temp2.z);
			frontWheelsChild [i].Rotate (Vector3.right * (Time.deltaTime * syncVars.wheelRPM * 5));
				//		temp2 = new Vector3 (frontWheels[i].localEulerAngles.x - (syncVars.wheelRPM), frontWheels [i].localEulerAngles.y, frontWheels [i].localEulerAngles.z);
				//		//float yAngle = Mathf.SmoothDampAngle (frontWheels[i].localEulerAngles.y, temp2.y, ref yVelocity, 0.07f);
				//		float xAngle = Mathf.SmoothDampAngle (frontWheels[i].localEulerAngles.x, temp2.x, ref yVelocity, 0.01f);
				//		frontWheels [i].localEulerAngles = new Vector3 (temp2.x, temp2.y, temp2.z);
		}
		for(int i2 = 0; i2 < rearWheels.Length; i2++){
			rearWheels [i2].Rotate (Vector3.right * (Time.deltaTime * syncVars.wheelRPM * 5));
		}
	}
}