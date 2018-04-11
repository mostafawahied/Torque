using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class RG_PlayerName : NetworkBehaviour {

	[SyncVar] public string playerName;
	private NetworkInstanceId playerNetID;
	private Transform myTransform;

	public override void OnStartLocalPlayer(){
		GetNetworkIdentity ();
		SetIdentity ();
	}

	void Awake () {
		myTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		if(myTransform.name == "" || myTransform.name == "Networked Player Car (0)(Clone)"){
			SetIdentity ();
		}
	}


	void SetIdentity(){
		if (!isLocalPlayer) {
			myTransform.name = playerName;
		} else {
			myTransform.name = MakeUniqueIdentity ();
		}
	}

	[Client]
	void GetNetworkIdentity(){
		playerNetID = GetComponent<NetworkIdentity> ().netId;
		CmdTellServerMyIdentity (MakeUniqueIdentity());
	}

	string MakeUniqueIdentity(){
		string uniqueName = "Player " + playerNetID.ToString ();
		return uniqueName;
	}

	[Command]
	void CmdTellServerMyIdentity(string name){
		playerName = name;
	}
}
