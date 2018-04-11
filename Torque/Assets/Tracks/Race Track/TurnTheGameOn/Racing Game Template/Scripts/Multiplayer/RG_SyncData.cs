using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

public class RG_SyncData : NetworkBehaviour {

	[SyncVar(hook = "OnHorizontalChanged")] public float horizontalInput;
	[SyncVar(hook = "OnVerticalChanged")] public float verticalInput;
	[SyncVar(hook = "OnGearChanged")] public string gearString;
	[SyncVar(hook = "OnWheelRPMChanged")] public float wheelRPM;
	public Text gearText;
	public WheelCollider wheelCollider;
	public bool mobile;

	void Start () {
		if (gearText == null) {
			gearText = GameObject.Find ("Gear Text").GetComponent<Text>();
		}
		if(GameObject.Find("Car Input") != null){
			mobile = true;
		}
	}

	void Update () {
		if(isLocalPlayer){
			GetInputs ();
		}
	}

	public void GetInputs(){
		wheelRPM = wheelCollider.rpm;
		if (mobile) {
			horizontalInput = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis ("Horizontal");
			verticalInput = UnityStandardAssets.CrossPlatformInput.CrossPlatformInputManager.GetAxis ("Vertical");
		} else {
			horizontalInput = Input.GetAxis ("Horizontal");
			verticalInput = Input.GetAxis ("Vertical");
		}
		if (gearText != null) {
			gearString = gearText.text;
		}
		CmdHorizontalValue (horizontalInput);
		CmdVerticalValue (verticalInput);
		CmdGearStringValue (gearString);
		CmdWheelRPMValue (wheelRPM);
	}

	/// <summary>
	/// Commands for SyncVars
	/// </summary>
	[Command]
	void CmdHorizontalValue(float value){
		SetHorizontalInput (value);
	}
	[Command]
	void CmdVerticalValue(float value){
		SetVerticalInput (value);
	}
	[Command]
	void CmdGearStringValue(string value){
		SetGearString (value);
	}
	[Command]
	void CmdWheelRPMValue(float value){
		SetWheelRPM (value);
	}

	/// <summary>
	/// Client Methods to process SyncVar changes
	/// </summary>
	public void SetHorizontalInput(float value){
		horizontalInput = value;
	}
	public void SetVerticalInput(float value){
		verticalInput = value;
	}
	public void SetGearString(string value){
		gearString = value;
	}
	public void SetWheelRPM(float value){
		wheelRPM = value;
	}

	/// <summary>
	/// SyncVar hooks
	/// </summary>
	void OnHorizontalChanged(float value){
		horizontalInput = value;
	}
	void OnVerticalChanged(float value){
		verticalInput = value;
	}
	void OnGearChanged(string value){
		gearString = value;
	}
	void OnWheelRPMChanged(float value){
		wheelRPM = value;
	}
}
