using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class RG_NetworkLobbyPlayer : NetworkBehaviour {

	public bool isLocalPlayer;
	public byte slot;
	public Button.ButtonClickedEvent buttonCallback;
	public NetworkIdentity netId;
	RG_NetworkManagerHUD netManagerHUD;

	// Use this for initialization
	void Start () {
		netManagerHUD = GameObject.Find ("LobbyManager").GetComponent<RG_NetworkManagerHUD> ();
		netId = GetComponent<NetworkIdentity> ();
		slot = GetComponent<NetworkLobbyPlayer> ().slot;
		if (netId.isLocalPlayer) {
			isLocalPlayer = true;
			PlayerPrefs.SetInt ("Slot", slot);
		} else {
			isLocalPlayer = false;
		}
		if(slot == 0 && netId.isLocalPlayer){
			//This player is host
			GameObject.Find("LobbyManager").GetComponent<RG_NetworkManagerHUD>().EnableStartLobbyGameButton();
			GameObject.Find ("StartLobbyGameButton").GetComponent<Button> ().onClick = buttonCallback;
		}
		if (slot == 0) {
			transform.name = "Player " + slot.ToString () + (" (HOST)");
		} else {
			transform.name = "Player " + slot.ToString ();
			//
		}

	}


	
	public void EnableLoadingImage(){
		//netManagerHUD.networkLobbyManager.matchMaker.ma
	//	for(int i = 0; i < netManagerHUD.networkLobbyManager.lobbySlots.Length - 1; i++){
	//		if(netManagerHUD.networkLobbyManager.lobbySlots[i] == null){
	//			netManagerHUD.networkLobbyManager.maxPlayersPerConnection += 1;
	//			netManagerHUD.networkLobbyManager.TryToAddPlayer ();
		//netw
	//		}
	//	}
		RpcEnableLoading ();
		netManagerHUD.loadingMultiplayerScene = true;
		Invoke ("SetStartFlag", 0.1f);
	}

	[ClientRpc]
	public void RpcEnableLoading(){
		netManagerHUD.lobbyHUDReference.loadingImage.SetActive (true);
	}

	public void SetStartFlag(){
		GetComponent<NetworkLobbyPlayer> ().SendReadyToBeginMessage ();
	}

	void OnLevelWasLoaded(){
		
	}

}
