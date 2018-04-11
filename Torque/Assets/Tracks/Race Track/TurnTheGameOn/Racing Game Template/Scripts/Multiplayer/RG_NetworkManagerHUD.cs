using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

#if ENABLE_UNET

namespace UnityEngine.Networking{
	[AddComponentMenu("Network/NetworkManagerHUD")]
	[RequireComponent(typeof(NetworkLobbyManager))]
	[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
	public class RG_NetworkManagerHUD : NetworkBehaviour	{


		[System.Serializable]
		public class LobbyHUDReference{
			public InputField roomSize;
			public Dropdown mapDropdown;
			public Dropdown gameTypeDropdown;
			public Text availableGamesText;
			public Text playerCountText;
			public Text L_gametypeText;
			public Text L_mapText;
			public Text L_roomNameText;
			public GameObject lobbyWindow;
			public GameObject backButtonFindGames;
			public GameObject matchmakingWindow;
			public GameObject findMatchWindow;
			public GameObject disconnectButton;
			public InputField roomNameField;
			public GameObject startGameButton;
			public GameObject backButtonCreateGame;
			public GameObject backButtonLobby;
			public GameObject loadingImage;
			public GameObject hostCancelWindow;
			public Transform contentPanel;
			public GameObject matchmakingImage;
		}


		public GameObject selectMatchButton;
		[Range(0,8)]public uint playerLimit;
		[Header("Free Roam Scenes List")]
		public Dropdown.OptionDataList freeRoamOptionsList;
		[Header("Race Scenes List")]
		public Dropdown.OptionDataList raceOptionsList;
		public NetworkLobbyManager networkLobbyManager;
		private RG_GarageManager garageManager;
		public enum GameType{ FreeRoam, Race }
		public GameType gameType;
		public float refreshRate;
		private float refreshTimer;
		private bool lookingForMatch;
		private bool readyToSpawn = true;
		public bool loadingMultiplayerScene;
		private int matches;
		private int[] currentSize;
		private Text[] sizeText;
		private Text[] mapText;
		private Text[] gameTypeText;
		private int selectedGame;
		private bool joinedMatch;
		private int availableGames;
		private string[] rawDescriptionArray;
		private char[] splitchar = { '-' };
		private string rawDescription;
		private string[] b_rawDescriptionArray;
		private string b_rawDescription;
		private string[] c_rawDescriptionArray;
		private string c_rawDescription;
		private BasicResponse destroyMatchRequest;
		public LobbyHUDReference lobbyHUDReference;
		private bool joinedLobby;
		public int playerCount;
		public GameObject garageObjects;
		private bool awakeCall;
		private bool playerHost;
		public float joinGameTimeout;
		float joinGameTimeoutTimer;
		//float currentPlayerCount;

		//public void OnMatchCreate(CreateMatchResponse matchInfo){
		//	this.matchInfo = matchInfo;
		//	Debug.Log ("OnMatchCreate networkId: " + matchInfo.networkId);
		//}

		void Awake(){
			StartupCall ();
			awakeCall = true;
		}

		void OnLevelWasLoaded(){
			if (SceneManager.GetActiveScene ().name == "Garage" && !awakeCall) {
				garageObjects.SetActive (true);
				Button_BackLobby ();
				garageManager.Button_BackLobby ();
				lobbyHUDReference.hostCancelWindow.SetActive (true);
			} else {
				//networkLobbyManager.lobbySlots
			}
			StartupCall ();
			awakeCall = false;
		}

		void StartupCall(){	
			if (networkLobbyManager == null)
				networkLobbyManager = GetComponent<NetworkLobbyManager> ();			
			if (SceneManager.GetActiveScene ().name == "Garage") {
				garageObjects.SetActive (true);
				if (garageManager == null)
					garageManager = GameObject.Find ("Garage Manager").GetComponent<RG_GarageManager> ();				
				lobbyHUDReference.matchmakingWindow.SetActive (false);
				lobbyHUDReference.matchmakingImage.SetActive (false);
				lobbyHUDReference.findMatchWindow.SetActive (false);
				lobbyHUDReference.disconnectButton.SetActive (false);
				lobbyHUDReference.backButtonFindGames.SetActive (true);
				UpdateGameType ();
				UpdateGameSize ();
			}
			if (SceneManager.GetActiveScene ().name != "Garage") {
				garageObjects.SetActive (false);
				loadingMultiplayerScene = false;
				lobbyHUDReference.disconnectButton.SetActive (true);
				lobbyHUDReference.startGameButton.SetActive (false);
				lobbyHUDReference.lobbyWindow.SetActive (false);
			}

		}

		void Update(){
			playerCount = 0;
			for (int i = 0; i < networkLobbyManager.lobbySlots.Length; i++){
				if(networkLobbyManager.lobbySlots[i] != null){
					playerCount += 1;
				}
			}
			// Use a timer control to update and refresh the available games list from mm server
			refreshTimer -= Time.deltaTime;
			if (refreshTimer <= 0) {
				//Reset the timer
				refreshTimer = refreshRate;
				availableGames = 0;
				// Update and refresh the matchmaking settings
				if (networkLobbyManager.matches != null && networkLobbyManager.matchInfo == null) {					
					FindGames ();
					UpdateMatchInfo ();
				}
			}
			networkLobbyManager.networkAddress = "127.0.0.1";
			if (NetworkServer.active || NetworkClient.active) {						lobbyHUDReference.disconnectButton.SetActive(true);			}
			UpdateUI ();
			lobbyHUDReference.playerCountText.text = playerCount.ToString () + " / " + (networkLobbyManager.maxPlayers - 1).ToString () + " Players";
			if (joinedMatch && lobbyHUDReference.lobbyWindow.activeInHierarchy && !joinedLobby) {
				joinGameTimeoutTimer += 1 * Time.deltaTime;
				if(playerCount > 0 && !loadingMultiplayerScene){
					lobbyHUDReference.findMatchWindow.SetActive (false);
					lobbyHUDReference.matchmakingImage.SetActive (true);
					lobbyHUDReference.loadingImage.SetActive (false);
					joinedLobby = true;
				}else if(joinGameTimeoutTimer >= joinGameTimeout){
					joinGameTimeoutTimer = 0f;
					lobbyHUDReference.loadingImage.SetActive (false);
					garageObjects.SetActive (true);
					Button_BackLobby ();
					garageManager.Button_BackLobby ();
					lobbyHUDReference.hostCancelWindow.SetActive (true);
					Debug.Log ("Join Game Timeout");

				}

			} else if(!joinedMatch && lobbyHUDReference.lobbyWindow.activeInHierarchy){
				
				if(playerCount > 0 && !loadingMultiplayerScene){
					lobbyHUDReference.findMatchWindow.SetActive (false);
					lobbyHUDReference.matchmakingImage.SetActive (true);
					lobbyHUDReference.loadingImage.SetActive (false);
				}
			}else if(playerCount == 0 && joinedMatch && lobbyHUDReference.lobbyWindow.activeInHierarchy){
				garageObjects.SetActive (true);
				Button_BackLobby ();
				garageManager.Button_BackLobby ();
				lobbyHUDReference.hostCancelWindow.SetActive (true);
			}
		}

		void UpdateUI(){
			if (!joinedMatch) {
				rawDescription = lobbyHUDReference.roomNameField.text + "-" + lobbyHUDReference.gameTypeDropdown.options [lobbyHUDReference.gameTypeDropdown.value].text + "-" + lobbyHUDReference.mapDropdown.options [lobbyHUDReference.mapDropdown.value].text + "-" + (networkLobbyManager.maxPlayers).ToString ();
				networkLobbyManager.matchName = rawDescription;
				rawDescriptionArray = rawDescription.Split (splitchar);
			}

		}

		void UpdateMatchInfo(){
			// Only spawn new buttons when the readyToSpawn bool is true
			if (readyToSpawn) {
				SpawnButtons ();
			}
			networkLobbyManager.matchMaker.ListMatches(0,20, "", networkLobbyManager.OnMatchList);
			Debug.Log ("matches Count" + networkLobbyManager.matches.Count);
			lobbyHUDReference.availableGamesText.text = availableGames.ToString() + " Games Available";
			if(networkLobbyManager.matches.Count != matches){	
				FindGames ();
				matches = networkLobbyManager.matches.Count;
				System.Array.Resize (ref sizeText, networkLobbyManager.matches.Count);
				System.Array.Resize (ref currentSize, networkLobbyManager.matches.Count);
				System.Array.Resize (ref mapText, networkLobbyManager.matches.Count);
				System.Array.Resize (ref gameTypeText, networkLobbyManager.matches.Count);
				if (matches == 0) {	networkLobbyManager.matches = null;		}
				return;
			}
			for (int i = 0; i < networkLobbyManager.matches.Count; i++) {
				if (i <= availableGames && availableGames != 0) {
					if (currentSize [i] != networkLobbyManager.matches [i].currentSize) {
						currentSize [i] = networkLobbyManager.matches [i].currentSize;
						//Max players - 2. This is the string for available games.
						sizeText [i].text = networkLobbyManager.matches [i].currentSize.ToString () + " / " + (networkLobbyManager.matches [i].maxSize - 1).ToString ();
					}
					else if(currentSize [i] == networkLobbyManager.matches [i].maxSize){
						FindGames ();
						SpawnButtons ();
						return;
					}
				}
			}
		}

		void SpawnButtons(){			
			readyToSpawn = false;
			if (networkLobbyManager.matches.Count != 0 && lookingForMatch) {
				availableGames = 0;
				System.Array.Resize (ref currentSize, networkLobbyManager.matches.Count);
				System.Array.Resize (ref sizeText, networkLobbyManager.matches.Count);
				System.Array.Resize (ref mapText, networkLobbyManager.matches.Count);
				System.Array.Resize (ref gameTypeText, networkLobbyManager.matches.Count);
				for (int i = 0; i < networkLobbyManager.matches.Count;i++){	
					if (networkLobbyManager.matches [i].currentSize < (networkLobbyManager.matches [i].maxSize - 1) && networkLobbyManager.matches [i].isPrivate == false) {
						availableGames += 1;
						GameObject newButton = Instantiate (selectMatchButton) as GameObject;
						RG_Game game = newButton.GetComponent<RG_Game> ();
						sizeText [i] = game.size;
						mapText [i] = game.map;
						gameTypeText [i] = game.mode;
						sizeText [i].text = networkLobbyManager.matches [i].currentSize.ToString () + " / " + (networkLobbyManager.matches [i].maxSize - 1).ToString ();
						b_rawDescription = networkLobbyManager.matches [i].name;
						b_rawDescriptionArray = b_rawDescription.Split (splitchar);
						game.roomName.text = b_rawDescriptionArray [0];
						gameTypeText [i].text = b_rawDescriptionArray [1];
						mapText [i].text = b_rawDescriptionArray [2];
						game.gameNumber = i;
						newButton.transform.SetParent (lobbyHUDReference.contentPanel);
					} else {
						networkLobbyManager.matches [i].isPrivate = true;
					}
				}
			}
		}

		// UI Update Game Type
		public void UpdateGameType(){
			if(lobbyHUDReference.gameTypeDropdown.value == 0){
				gameType = GameType.FreeRoam;
				lobbyHUDReference.mapDropdown.options = freeRoamOptionsList.options;
				lobbyHUDReference.mapDropdown.value = 0;
				networkLobbyManager.playScene = "M" + "0" + lobbyHUDReference.mapDropdown.options[0].text;
				Debug.Log(">Multiplayer Game Type: Free Roam \n>Online Scene: " + "M" + "0" + lobbyHUDReference.mapDropdown.options[0].text);
			}
			else if(lobbyHUDReference.gameTypeDropdown.value == 1){
				gameType = GameType.Race;
				Debug.Log("Game Type: Race");
				lobbyHUDReference.mapDropdown.options = raceOptionsList.options;
				lobbyHUDReference.mapDropdown.value = 0;
				networkLobbyManager.playScene = "0" + raceOptionsList.options[0].text;
				Debug.Log(">Online Scene: " + "0" + raceOptionsList.options[0].text);
			}
		}
		// UI Update Map
		public void UpdateMap(){
			for(int i = 0; i < lobbyHUDReference.mapDropdown.options.Count; i++){
				if(lobbyHUDReference.mapDropdown.value == i){
					networkLobbyManager.playScene = "M" + i.ToString() + lobbyHUDReference.mapDropdown.options[i].text;
					Debug.Log(">Online Scene: " + "M" +  i.ToString() + lobbyHUDReference.mapDropdown.options[i].text);
				}
			}
		}

		public void UpdateGameSize(){
			if (uint.Parse (lobbyHUDReference.roomSize.text) > playerLimit) {
				lobbyHUDReference.roomSize.text = playerLimit.ToString ();
			}
			networkLobbyManager.maxPlayers = int.Parse (lobbyHUDReference.roomSize.text) + 1;
		}

		public void FindGames(){
			var children = new List<GameObject>();
			foreach (Transform child in lobbyHUDReference.contentPanel) children.Add(child.gameObject);
			children.ForEach(child => Destroy(child));
			networkLobbyManager.matchMaker.ListMatches(0,20, "", networkLobbyManager.OnMatchList);
			lookingForMatch = true;
			lobbyHUDReference.matchmakingWindow.SetActive (false);
			lobbyHUDReference.matchmakingImage.SetActive (false);
			lobbyHUDReference.findMatchWindow.SetActive (true);
			readyToSpawn = true;
		}

		public void InternetMatchmakingWindow(){
			if (lobbyHUDReference.matchmakingWindow.activeInHierarchy) {
				lobbyHUDReference.matchmakingWindow.SetActive (false);
				lobbyHUDReference.matchmakingImage.SetActive (false);
				networkLobbyManager.StopMatchMaker();
				lobbyHUDReference.backButtonFindGames.SetActive(true);
				lobbyHUDReference.backButtonCreateGame.SetActive (false);
			} else {
				
				lobbyHUDReference.backButtonCreateGame.SetActive (true);
				lobbyHUDReference.backButtonFindGames.SetActive(false);
				lobbyHUDReference.matchmakingImage.SetActive (true);
				lobbyHUDReference.matchmakingWindow.SetActive(true);
				networkLobbyManager.StartMatchMaker();
			}
		}

		public void OnDestroyMatch(BasicResponse destroyMatchRequest){
			Debug.Log ("OnMatchDestroyed");
		}

		public void DestroyMatch(){
		
		}

		[ContextMenu("Button: Back Lobby")]
		public void Button_BackLobby(){	
			if (playerHost) {
				networkLobbyManager.matchMaker.DestroyMatch ((NetworkID)networkLobbyManager.matchInfo.networkId, OnDestroyMatch);
				networkLobbyManager.StopServer();
				networkLobbyManager.StopClient();
				networkLobbyManager.StopHost();
				networkLobbyManager.StopMatchMaker();


				networkLobbyManager.StopMatchMaker();
				playerHost = false;
				Debug.Log ("Destroy Match");
			} else {
				networkLobbyManager.StopClient();
				networkLobbyManager.StopMatchMaker();
				Debug.Log ("Leave Match");
			}
			joinedMatch = false;
			joinedLobby = false;
			lobbyHUDReference.lobbyWindow.SetActive (false);
			lobbyHUDReference.backButtonFindGames.SetActive(true);
			lobbyHUDReference.backButtonLobby.SetActive (false);
			lobbyHUDReference.startGameButton.SetActive (false);
			lobbyHUDReference.matchmakingImage.SetActive (false);
		}

		public void InternetMatchmakingButton(){
			lobbyHUDReference.loadingImage.SetActive (true);
			Invoke ("InternetMatch", 0.1f);

		}

		void InternetMatch(){
			playerHost = true;
			lobbyHUDReference.L_roomNameText.text = rawDescriptionArray[0];
			lobbyHUDReference.L_gametypeText.text = rawDescriptionArray[1];
			lobbyHUDReference.L_mapText.text = rawDescriptionArray[2];
			lobbyHUDReference.matchmakingWindow.SetActive (false);
			lobbyHUDReference.lobbyWindow.SetActive (true);
			lobbyHUDReference.backButtonCreateGame.SetActive (false);
			lobbyHUDReference.backButtonLobby.SetActive (true);				
			networkLobbyManager.matchName = rawDescription;
			NetworkManager.singleton.matchSize = (uint)networkLobbyManager.maxPlayers;
			NetworkManager.singleton.maxConnections = networkLobbyManager.maxPlayers - 1;
			networkLobbyManager.matchMaker.CreateMatch(networkLobbyManager.matchName, networkLobbyManager.matchSize, true, "", new NetworkMatch.ResponseDelegate<CreateMatchResponse>(networkLobbyManager.OnMatchCreate));

			//networkLobbyManager.matchMaker.JoinMatch(networkId, string.Empty, new NetworkMatch.ResponseDelegate<JoinMatchResponse>(MatchJoined));
		}

	//	public virtual void OnMatchCreate(CreateMatchResponse matchInfo){
	///		if (LogFilter.logDebug) { 
	//			Debug.Log ("Network Manager OnMatchCreate " + matchInfo);
	//		}
	//		if (matchInfo.success) {
	//			try{
	//				Utility.SetAccessTokenForNetwork (matchInfo.networkId, new NetworkAccessToken (matchInfo.accessTokenString));
	//			}catch (System.ArgumentException ex) {
	//				if (LogFilter.logError) {
	//					//Debug.LogError (ex);
	//				}
	//			}
	//			networkLobbyManager.StartHost (new MatchInfo (matchInfo));
	//			networkLobbyManager.matchInfo = matchInfo;
	//			Debug.Log ("OnMatchCreate networkId: " + matchInfo.networkId);
	//		} else {
	//			if (LogFilter.logError) { Debug.LogError ("Create failed: " + matchInfo); }
	//		}
	//	}

		public virtual void MatchJoined(JoinMatchResponse matchInfo){
			if(LogFilter.logDebug){
				Debug.Log ("Network Manager OnMatchJoined");
			}
			if (matchInfo.success) {
				try {
					Utility.SetAccessTokenForNetwork (matchInfo.networkId, new NetworkAccessToken (matchInfo.accessTokenString));
				} catch (System.ArgumentException ex) {
					if (LogFilter.logError) {
						//Debug.LogError (ex);
					}
				}
				networkLobbyManager.StartClient (new MatchInfo (matchInfo));
			}
			else if(LogFilter.logError) {
				Debug.LogError (string.Concat("Join Failed:", matchInfo));
			}
		}

		public void JoinMatch(int i){
			lobbyHUDReference.matchmakingImage.SetActive (true);
			lobbyHUDReference.loadingImage.SetActive (true);
			joinedMatch = true;
			lobbyHUDReference.findMatchWindow.SetActive (false);
			lobbyHUDReference.backButtonFindGames.SetActive (false);
			lobbyHUDReference.lobbyWindow.SetActive (true);
			lobbyHUDReference.backButtonLobby.SetActive (true);
			selectedGame = i;
			NetworkID networkId = networkLobbyManager.matches[i].networkId;
			c_rawDescription = networkLobbyManager.matches [selectedGame].name;
			networkLobbyManager.matchName = networkLobbyManager.matches [selectedGame].name;
			c_rawDescriptionArray = c_rawDescription.Split (splitchar);
			lobbyHUDReference.L_roomNameText.text = c_rawDescriptionArray[0];
			lobbyHUDReference.L_gametypeText.text = c_rawDescriptionArray[1];
			lobbyHUDReference.L_mapText.text = c_rawDescriptionArray[2];
			networkLobbyManager.maxPlayers = int.Parse(c_rawDescriptionArray[3])  + 1;
			//
			//networkLobbyManager.matchMaker.JoinMatch(networkId, "", networkLobbyManager.OnMatchJoined);
			networkLobbyManager.matchMaker.JoinMatch(networkId, string.Empty, new NetworkMatch.ResponseDelegate<JoinMatchResponse>(MatchJoined));
			NetworkManager.singleton.matchSize = (uint)networkLobbyManager.matches [i].maxSize;
			NetworkManager.singleton.maxConnections = (networkLobbyManager.matches [i].maxSize - 1);
			networkLobbyManager.maxPlayers = (int)NetworkManager.singleton.matchSize;
		}

		public void FindGameButton(){
			if (lobbyHUDReference.findMatchWindow.activeInHierarchy) {				
				lobbyHUDReference.findMatchWindow.SetActive (false);
				lookingForMatch = false;
				networkLobbyManager.StopMatchMaker();
			} else {
				networkLobbyManager.StartMatchMaker();
				FindGames ();
			}
		}

		public void EnableStartLobbyGameButton(){
			lobbyHUDReference.startGameButton.SetActive (true);

			//NetworkManager.singleton.client.Send (MsgType.LobbyReadyToBegin, msg);
		}

		public void DisconnectButton(){
			networkLobbyManager.StopHost();
		}

	}
};
#endif //ENABLE_UNET