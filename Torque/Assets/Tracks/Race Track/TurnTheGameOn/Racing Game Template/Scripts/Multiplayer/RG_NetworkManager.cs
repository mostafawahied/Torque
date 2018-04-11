using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Reflection;

public class RG_NetworkManager : NetworkManager {
	public enum GameType{ OpenWorld, Race }



	[Header("Options")]
	public GameType gameType;
	[Range(0,8)]public uint playerLimit;
	[Header("Open World Scenes List")]
	public Dropdown.OptionDataList openWorldOptionsList;
	[Header("Race Scenes List")]
	public Dropdown.OptionDataList raceOptionsList;
	[Header("Input Fields")]
	public InputField roomSize;
	[Header("Dropdown Selections")]
	public Dropdown mapDropdown;
	public Dropdown gameTypeDropdown;



	public string mapName;

	public override void OnServerConnect(NetworkConnection conn){
		
		base.OnServerConnect(conn);
		Type serverType = typeof(NetworkServer);
		FieldInfo info = serverType.GetField("maxPacketSize",
			BindingFlags.NonPublic | BindingFlags.Static);
		ushort maxPackets = 1500;
		info.SetValue(null, maxPackets);
	}


	void Awake(){
		
		UpdateGameSize ();
		UpdateGameType ();
	}

	public void UpdateGameSize(){
		if (uint.Parse(roomSize.text) > playerLimit) {
			roomSize.text = playerLimit.ToString();
		}
		matchSize = uint.Parse(roomSize.text);
	}

	public void UpdateGameType(){
		if( gameTypeDropdown.value == 0){
			gameType = GameType.OpenWorld;
			Debug.Log("Game Type: Open World");
			mapDropdown.options = openWorldOptionsList.options;
			mapDropdown.value = 0;
			onlineScene = "0" + mapDropdown.options[0].text;
			Debug.Log("Online Scene: " + "0" + mapDropdown.options[0].text);
		}
		else if(gameTypeDropdown.value == 1){
			gameType = GameType.Race;
			Debug.Log("Game Type: Race");
			mapDropdown.options = raceOptionsList.options;
			mapDropdown.value = 0;
			onlineScene = "0" + raceOptionsList.options[0].text;
			Debug.Log("Online Scene: " + "0" + raceOptionsList.options[0].text);
		}
	}

	public void UpdateMap(){
		for(int i = 0; i < mapDropdown.options.Count; i++){
			if(mapDropdown.value == i){
				onlineScene = i.ToString() + mapDropdown.options[i].text;
				Debug.Log("Online Scene: " + i.ToString() + mapDropdown.options[i].text);
			}
		}
	}

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
	
}