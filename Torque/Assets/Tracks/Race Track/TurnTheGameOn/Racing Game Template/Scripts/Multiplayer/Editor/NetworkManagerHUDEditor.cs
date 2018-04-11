#if ENABLE_UNET
using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityObject = UnityEngine.Object;

namespace UnityEditor
{
    [CustomEditor(typeof(RG_NetworkManagerHUD), true)]
    [CanEditMultipleObjects]
    public class NetworkManagerHUDEditor : Editor
    {
       
		bool showRaceScenes;
		bool showFreeRoamScenes;

		public override void OnInspectorGUI(){
			EditorGUILayout.BeginVertical ("Box");
			SerializedProperty selectMatchButton = serializedObject.FindProperty ("selectMatchButton");
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (selectMatchButton, true);
			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();

			SerializedProperty playerLimit = serializedObject.FindProperty ("playerLimit");
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (playerLimit, true);
			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();

			SerializedProperty refreshRate = serializedObject.FindProperty ("refreshRate");
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (refreshRate, true);
			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();

			SerializedProperty joinGameTimeout = serializedObject.FindProperty ("joinGameTimeout");
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (joinGameTimeout, true);
			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();

			showFreeRoamScenes = EditorGUILayout.Foldout (showFreeRoamScenes, "Configure Free Roam Scenes");
			if (showFreeRoamScenes) {
				EditorGUILayout.BeginVertical ("Box");


				SerializedProperty freeRoamOptionsList = serializedObject.FindProperty ("freeRoamOptionsList");
				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.PropertyField (freeRoamOptionsList, true);
				if (EditorGUI.EndChangeCheck ())
					serializedObject.ApplyModifiedProperties ();


				EditorGUILayout.EndVertical ();
			}

			showRaceScenes = EditorGUILayout.Foldout (showRaceScenes, "Configure Race Scenes");
			if (showRaceScenes) {
				EditorGUILayout.BeginVertical ("Box");


				SerializedProperty raceOptionsList = serializedObject.FindProperty ("raceOptionsList");
				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.PropertyField (raceOptionsList, true);
				if (EditorGUI.EndChangeCheck ())
					serializedObject.ApplyModifiedProperties ();


				EditorGUILayout.EndVertical ();
			}
				
			SerializedProperty lobbyHUDReference = serializedObject.FindProperty ("lobbyHUDReference");
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (lobbyHUDReference, true);
			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();

			EditorGUILayout.EndVertical ();
		}

    }
}
#endif //ENABLE_UNET
