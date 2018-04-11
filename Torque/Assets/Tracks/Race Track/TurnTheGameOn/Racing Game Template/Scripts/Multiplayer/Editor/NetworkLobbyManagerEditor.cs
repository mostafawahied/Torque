#if ENABLE_UNET
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityObject = UnityEngine.Object;

namespace UnityEditor
{
	[CustomEditor(typeof(RG_NetworkLobbyManager), true)]
	[CanEditMultipleObjects]
	class NetworkLobbyManagerEditor : NetworkManagerEditor
	{
		SerializedProperty m_ShowLobbyGUIProperty;
		SerializedProperty m_MaxPlayersProperty;
		SerializedProperty m_MaxPlayersPerConnectionProperty;
		SerializedProperty m_MinPlayersProperty;
		SerializedProperty m_LobbyPlayerPrefabProperty;
		SerializedProperty m_GamePlayerPrefabProperty;
		SerializedProperty gamePlayerPrefab;
		GUIContent m_LobbySceneLabel;
		//GUIContent m_PlaySceneLabel;
		GUIContent m_MaxPlayersLabel;
		GUIContent m_MaxPlayersPerConnectionLabel;
		GUIContent m_MinPlayersLabel;
		bool ShowSlots;
		bool showOptions;

		void InitLobby(){
			
		}

		public override void OnInspectorGUI(){
			
			//RG_NetworkLobbyManager rg_NetworkLobbyManager = (RG_NetworkLobbyManager)target;

			EditorGUILayout.BeginVertical("Box");

			if (m_DontDestroyOnLoadProperty == null || m_DontDestroyOnLoadLabel == null)
				m_Initialized = false;


			InitLobby();

			if (!m_Initialized)	{
				m_LobbySceneLabel = new GUIContent("Lobby Scene", "The scene loaded for the lobby");
				//m_PlaySceneLabel = new GUIContent("Play Scene", "The scene loaded to play the game");
				m_MaxPlayersLabel = new GUIContent("Max Players", "The maximum number of players allowed in the lobby.");
				m_MaxPlayersPerConnectionLabel = new GUIContent("Max Players Per Connection", "The maximum number of players that each connection/client can have in the lobby. Defaults to 1.");
				m_MinPlayersLabel = new GUIContent("Minimum Players", "The minimum number of players required to be ready for the game to start. If this is zero then the game can start with any number of players.");
				//m_ShowLobbyGUIProperty = serializedObject.FindProperty("m_ShowLobbyGUI");
				m_MaxPlayersProperty = serializedObject.FindProperty("m_MaxPlayers");
				m_MaxPlayersPerConnectionProperty = serializedObject.FindProperty("m_MaxPlayersPerConnection");
				m_MinPlayersProperty = serializedObject.FindProperty("m_MinPlayers");
				m_LobbyPlayerPrefabProperty = serializedObject.FindProperty("m_LobbyPlayerPrefab");



				
				//m_GamePlayerPrefabProperty = serializedObject.FindProperty("m_GamePlayerPrefab");
				m_GamePlayerPrefabProperty = serializedObject.FindProperty("m_GamePlayerPrefab");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(m_GamePlayerPrefabProperty, true);
				if (EditorGUI.EndChangeCheck())
					serializedObject.ApplyModifiedProperties();

			}
			Init();




			var lobby = target as NetworkLobbyManager;
			if (lobby == null)
				return;
			serializedObject.Update();
			ShowLobbyScenes();
			EditorGUILayout.PropertyField(m_LobbyPlayerPrefabProperty);
			EditorGUI.BeginChangeCheck();
			var newGamPlayer = EditorGUILayout.ObjectField("Game Player Prefab", lobby.gamePlayerPrefab, typeof(NetworkIdentity), false);
			if (EditorGUI.EndChangeCheck())	{
				if (newGamPlayer == null){
					m_GamePlayerPrefabProperty.objectReferenceValue = null;
				}
				else{
					var newGamePlayerIdentity = newGamPlayer as NetworkIdentity;
					if (newGamePlayerIdentity != null){
						if (newGamePlayerIdentity.gameObject != lobby.gamePlayerPrefab)	{
							m_GamePlayerPrefabProperty.objectReferenceValue = newGamePlayerIdentity.gameObject;
						}
					}
				}
			}


				


				EditorGUILayout.PropertyField(m_DontDestroyOnLoadProperty, m_DontDestroyOnLoadLabel);
				EditorGUILayout.PropertyField(m_RunInBackgroundProperty , m_RunInBackgroundLabel);
				EditorGUILayout.PropertyField(m_MaxPlayersProperty, m_MaxPlayersLabel);
				EditorGUILayout.PropertyField(m_MaxPlayersPerConnectionProperty, m_MaxPlayersPerConnectionLabel);
				EditorGUILayout.PropertyField(m_MinPlayersProperty, m_MinPlayersLabel);
				if (EditorGUILayout.PropertyField(m_LogLevelProperty)){
					LogFilter.currentLogLevel = (int)m_NetworkManager.logLevel;
				}
				ShowConfigInfo();
				ShowSimulatorInfo();
				ShowNetworkInfo();
				ShowSpawnInfo();
				serializedObject.ApplyModifiedProperties();


				EditorGUILayout.EndVertical ();

			EditorGUILayout.Separator();
			//EditorGUILayout.PropertyField(m_ShowLobbyGUIProperty);
			if (!Application.isPlaying)
				return;
			ShowLobbySlots();
			//ShowDerivedProperties(typeof(NetworkLobbyManager), typeof(NetworkManager));
		}

		protected void ShowLobbySlots()	{
			var lobby = target as NetworkLobbyManager;
			if (lobby == null)
				return;
			ShowSlots = EditorGUILayout.Foldout(ShowSlots, "LobbySlots");
			if (ShowSlots)	{
				EditorGUI.indentLevel += 1;
				foreach (var slot in lobby.lobbySlots){
					if (slot == null)
						continue;
					EditorGUILayout.ObjectField("Slot " + slot.slot, slot.gameObject, typeof(UnityObject), true);
				}
				EditorGUI.indentLevel -= 1;
			}
		}

		protected void ShowLobbyScenes(){
			var lobby = target as NetworkLobbyManager;
			if (lobby == null)
				return;
			var offlineObj = GetSceneObject(lobby.lobbyScene);
			EditorGUI.BeginChangeCheck();
			var newOfflineScene = EditorGUILayout.ObjectField(m_LobbySceneLabel, offlineObj, typeof(SceneAsset), false);
			if (EditorGUI.EndChangeCheck())	{
				if (newOfflineScene == null){
					m_NetworkManager.offlineScene = "";
					var prop = serializedObject.FindProperty("m_LobbyScene");
					prop.stringValue = "";
					EditorUtility.SetDirty(lobby);
				}
				else{
					if (newOfflineScene.name != m_NetworkManager.offlineScene){
						var sceneObj = GetSceneObject(newOfflineScene.name);
						if (sceneObj == null){
							Debug.LogWarning("The scene " + newOfflineScene.name + " cannot be used. To use this scene add it to the build settings for the project");
						}
						else{
							var prop = serializedObject.FindProperty("m_LobbyScene");
							prop.stringValue = newOfflineScene.name;
							m_NetworkManager.offlineScene = newOfflineScene.name;


							RG_NetworkLobbyManager rg_NetworkLobbyManager = (RG_NetworkLobbyManager)target;
							rg_NetworkLobbyManager.lobbyScene = newOfflineScene.name;










							EditorUtility.SetDirty(lobby);
						}
					}
				}
			}
//			var onlineObj = GetSceneObject(lobby.playScene);
//			EditorGUI.BeginChangeCheck();
//			var newOnlineScene = EditorGUILayout.ObjectField(m_PlaySceneLabel, onlineObj, typeof(SceneAsset), false);
//			if (EditorGUI.EndChangeCheck()){
//				if (newOnlineScene == null){
//					m_NetworkManager.onlineScene = "";
//					var prop = serializedObject.FindProperty("m_PlayScene");
//					prop.stringValue = "";
//					EditorUtility.SetDirty(lobby);
//				}
//				else{
//					if (newOnlineScene.name != m_NetworkManager.onlineScene){
//						var sceneObj = GetSceneObject(newOnlineScene.name);
//						if (sceneObj == null){
//							Debug.LogWarning("The scene " + newOnlineScene.name + " cannot be used. To use this scene add it to the build settings for the project");
//						}
//						else{
//							var prop = serializedObject.FindProperty("m_PlayScene");
//							prop.stringValue = newOnlineScene.name;
//							m_NetworkManager.onlineScene = "";
//							EditorUtility.SetDirty(lobby);
//						}
//					}
//				}
//			}
		}
	}
}
#endif // ENABLE_UNET