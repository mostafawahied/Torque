using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.UI;
using System;
using System.Reflection;

public class RG_NetworkLobbyManager : NetworkLobbyManager {

	new public CreateMatchResponse matchInfo;
	private RG_GarageManager garageManager;

	void Start(){
		garageManager = GameObject.Find ("Garage Manager").GetComponent<RG_GarageManager>();
	}

	void Update(){
		
		gamePlayerPrefab = garageManager.playableVehicles.vehicles [garageManager.playableVehicles.currentVehicleNumber];
	}

	// Used to remove a spamming debug message
	public override void OnServerConnect(NetworkConnection conn){
		
		base.OnServerConnect(conn);
		Type serverType = typeof(NetworkServer);
		FieldInfo info = serverType.GetField("maxPacketSize",
			BindingFlags.NonPublic | BindingFlags.Static);
		ushort maxPackets = 1500;
		info.SetValue(null, maxPackets);
	}

	//public override void OnMatchCreate(CreateMatchResponse matchInfo){
	//	if (LogFilter.logDebug) { Debug.Log ("Network Manager OnMatchCreate " + matchInfo); }
	//	if (matchInfo.success) {
	//		Utility.SetAccessTokenForNetwork (matchInfo.networkId, new NetworkAccessToken (matchInfo.accessTokenString));
	//		StartHost (new MatchInfo (matchInfo));
	//		this.matchInfo = matchInfo;
	//		Debug.Log ("OnMatchCreate networkId: " + matchInfo.networkId);
	//	} else {
	//		if (LogFilter.logError) { Debug.LogError ("Create failed: " + matchInfo); }
	//	}
	//}

	public virtual void OnMatchCreate(CreateMatchResponse matchInfo){
		if (LogFilter.logDebug) { 
			Debug.Log ("Network Manager OnMatchCreate " + matchInfo);
		}
		if (matchInfo.success) {
			try{
				Utility.SetAccessTokenForNetwork (matchInfo.networkId, new NetworkAccessToken (matchInfo.accessTokenString));
			}catch (System.ArgumentException ex) {
				if (LogFilter.logError) {
					//Debug.LogError (ex);
				}
			}
			StartHost (new MatchInfo (matchInfo));
			this.matchInfo = matchInfo;
			Debug.Log ("OnMatchCreate networkId: " + matchInfo.networkId);
		} else {
			if (LogFilter.logError) { Debug.LogError ("Create failed: " + matchInfo); }
		}
	}

	

	/*
	public void StartupHost(){
		SetPort ();
		NetworkManager.singleton.StartHost ();
	}

	public void JoinGame(){
		SetIPAddress ();
		SetPort ();
		NetworkManager.singleton.StartClient ();
	}

	void SetIPAddress(){
		string ipAddress = "127.0.0.1";
		NetworkManager.singleton.networkAddress = ipAddress;
	}

	void SetPort(){
		NetworkManager.singleton.networkPort = 7777;
	}

	public void InternetGame(){
		matchMaker.CreateMatch(matchName, matchSize, true, "", OnMatchCreate);
	}

	public void SetupConnectIPButtons(){
		GameObject.Find ("ButtonStartHost").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonStartHost").GetComponent<Button> ().onClick.AddListener(StartupHost);

		GameObject.Find ("ButtonJoinGame").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonJoinGame").GetComponent<Button> ().onClick.AddListener(JoinGame);
	}

	public void SetupMatchmakingButtons(){	
		GameObject.Find ("ButtonMatchmaking").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonMatchmaking").GetComponent<Button> ().onClick.AddListener(InternetGame);
	}

	public void SetupOtherSceneButtons(){
		GameObject.Find ("ButtonDisconnect").GetComponent<Button> ().onClick.RemoveAllListeners ();
		GameObject.Find ("ButtonDisconnect").GetComponent<Button> ().onClick.AddListener(NetworkManager.singleton.StopHost);
	}
	*/

}
