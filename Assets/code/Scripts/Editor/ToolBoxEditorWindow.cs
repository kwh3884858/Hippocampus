using System;
using System.Collections;
using System.Collections.Generic;
using Config.GameRoot;
using GamePlay;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolBoxEditorWindow : EditorWindow
{
	private MissionEnum m_currentMissionEnum = MissionEnum.None;
	private Scene m_currentMissionScene = new Scene ();
	bool groupEnabled;
	bool myBool = true;
	float myFloat = 1.23f;

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/Tool Box %l")]
	static void Init ()
	{
		// Get existing open window or if none, make a new one:
		ToolBoxEditorWindow window = (ToolBoxEditorWindow)EditorWindow.GetWindow (typeof (ToolBoxEditorWindow));
		window.Show ();

	}

	void OnGUI ()
	{
		GUILayout.Label ("Prefab Object Name", EditorStyles.boldLabel);
		ConfigMission.Instance.Text_Spawn_Point_Name =
			EditorGUILayout.TextField ("Spawn Point Name", ConfigMission.Instance.Text_Spawn_Point_Name);

		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);

		GUILayout.Label ("Group Name Constant", EditorStyles.boldLabel);
		ConfigMission.Instance.Text_Interactable_Object_Group =
			EditorGUILayout.TextField ("Interactable Group Name", ConfigMission.Instance.Text_Interactable_Object_Group);
		ConfigMission.Instance.Text_Event_Trigger_Group =
			EditorGUILayout.TextField ("Trigger Group Name", ConfigMission.Instance.Text_Event_Trigger_Group);
		ConfigMission.Instance.Text_Mission_Group =
			EditorGUILayout.TextField ("Mission Name", ConfigMission.Instance.Text_Mission_Group);

		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
		myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);

		GUILayout.Label ("Path to Prefab", EditorStyles.boldLabel);
		ConfigMission.Instance.Path_To_InteractableObject =
			EditorGUILayout.TextField ("Path to Interactable Object", ConfigMission.Instance.Path_To_InteractableObject);
		ConfigMission.Instance.Path_To_WorldTrigger =
			EditorGUILayout.TextField ("Path to World Trigger", ConfigMission.Instance.Path_To_WorldTrigger);
		ConfigMission.Instance.Path_To_SpawnPoint =
			EditorGUILayout.TextField ("Path to Spawn Point", ConfigMission.Instance.Path_To_SpawnPoint);

		EditorGUILayout.EndToggleGroup ();

		GUILayout.Label ("Mission", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical ();
		EditorGUILayout.LabelField ("Current Active Sccene:  " + SceneManager.GetActiveScene ().name);
		EditorGUILayout.LabelField ("Current Active Mission Scene:  " + m_currentMissionScene.name);
		m_currentMissionEnum = (MissionEnum)EditorGUILayout.EnumPopup ("Current Mission", m_currentMissionEnum);
		if (GUILayout.Button ("Create Mission Scene")) {
			if (m_currentMissionEnum != MissionEnum.None) {
				string folderName = MissionSceneManager.Instance.GenerateFolderName (m_currentMissionEnum);
				string sceneName = MissionSceneManager.Instance.GenerateSceneName (m_currentMissionEnum);
				bool isConfirm = EditorUtility.DisplayDialog ("Create New Scene", "New scene file will be create.\nScene Name: " + sceneName + "\nLocation: data/graphics/World/" + folderName, "Ok, create it", "Cancel");
				if (isConfirm) {
					CreateMissionFolder (folderName);

					bool isExistMissionScene = MissionSceneManager.Instance.IsMissionSceneExist (folderName, sceneName);
					if (!isExistMissionScene) {
						Scene newScene = EditorSceneManager.NewScene (NewSceneSetup.EmptyScene, NewSceneMode.Additive);
						newScene.name = sceneName;
						bool saveOK = EditorSceneManager.SaveScene (newScene, MissionSceneManager.Instance.GenerateScenePath (folderName, sceneName));
						if (!saveOK) {
							Debug.LogError ("Save is faild. Scene:" + sceneName);
						} else {
							m_currentMissionScene = newScene;
							EdtorSceneAutomaticOperatioin.UpdateSceneBuildSetting ();
						}
					} else {
						EditorUtility.DisplayDialog ("Scene Conflict", "Scene file Already Exist, Please load this scene.\nScene Name: " + sceneName + "\nLocation: data/graphics/World" + folderName, "Ok");
					}
				}
			}
		}

		if (GUILayout.Button ("Load Mission Scene")) {
			if (m_currentMissionEnum != MissionEnum.None) {
				string folderName = ConfigMission.Instance.Prefix_Mission_Folder + m_currentMissionEnum.ToString () + "_" + SceneManager.GetActiveScene ().name;
				string sceneName = ConfigMission.Instance.Prefix_Mission_Scene + m_currentMissionEnum.ToString () + "_" + SceneManager.GetActiveScene ().name;

				if (m_currentMissionScene.name != sceneName) {
					string pathToSceneFolder = MissionSceneManager.Instance.GenerateSceneFolderPath (folderName);
					if (!AssetDatabase.IsValidFolder (pathToSceneFolder)) {
						EditorUtility.DisplayDialog ("Scene Folder Not Exist", "Click [Create Mission Scene] to create new mission scene", "Ok");
					}
					bool isExistMissionScene = MissionSceneManager.Instance.IsMissionSceneExist (folderName, sceneName);
					if (isExistMissionScene) {
						string pathToScene = MissionSceneManager.Instance.GenerateScenePath (folderName, sceneName);
						m_currentMissionScene = EditorSceneManager.OpenScene (pathToScene, OpenSceneMode.Additive);
						EditorSceneManager.SetActiveScene (m_currentMissionScene);
					} else {
						EditorUtility.DisplayDialog ("Scene Not Exist", "Click [Create Mission Scene] to create new mission scene", "Ok");
					}
				}
			}
		}

		if (GUILayout.Button ("Remove Mission Scene")) {
			if (IsMissionSceneValid ()) {
				EditorSceneManager.SaveScene (m_currentMissionScene);
				EditorSceneManager.CloseScene (m_currentMissionScene, true);
			}
		}
		GUILayout.Label ("Add Gameobject", EditorStyles.boldLabel);

		if (GUILayout.Button ("Interactable Object")) {
			if (IsMissionSceneValid ()) {
				GameObject interactiablesGroup = GameObject.Find (ConfigMission.Instance.Text_Interactable_Object_Group);
				if (interactiablesGroup == null) {
					interactiablesGroup = new GameObject (ConfigMission.Instance.Text_Interactable_Object_Group);
				}

				string path = ConfigMission.Instance.Path_To_InteractableObject;
				GameObject go = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject> (path)) as GameObject;

				go.transform.SetParent (interactiablesGroup.transform);
				go.AddComponent<InteractiveObject> ();
			}
		}

		if (GUILayout.Button ("Event Trigger")) {
			if (IsMissionSceneValid ()) {
				GameObject triggersGroup = GameObject.Find (ConfigMission.Instance.Text_Event_Trigger_Group);
				if (triggersGroup == null) {
					triggersGroup = new GameObject (ConfigMission.Instance.Text_Event_Trigger_Group);
				}

				string path = ConfigMission.Instance.Path_To_WorldTrigger;
				GameObject go = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject> (path)) as GameObject;

				//GameObject go = new GameObject ("New Event Trigger");
				go.transform.SetParent (triggersGroup.transform);
				go.AddComponent<WorldTrigger> ();
			}
		}

		if (GUILayout.Button ("Create Spawn Point")) {
			if (IsMissionSceneValid ()) {
				GameObject spawnPoint = GameObject.Find (ConfigMission.Instance.Text_Spawn_Point_Name);
				if (spawnPoint != null) {
					EditorUtility.DisplayDialog ("Error", "Already contain a spawn point in scene.", "Ok");
				} else {
					string path = ConfigMission.Instance.Path_To_SpawnPoint;
					GameObject go = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject> (path)) as GameObject;
					go.name = ConfigMission.Instance.Text_Spawn_Point_Name;
				}
			}
		}

		if (GUILayout.Button ("Create New Mission_DO NOT USE")) {
			if (IsMissionSceneValid ()) {

				GameObject missionGroup = GameObject.Find (ConfigMission.Instance.Text_Mission_Group);
				if (missionGroup == null) {
					missionGroup = new GameObject (ConfigMission.Instance.Text_Mission_Group);
				}
				string path = ConfigMission.Instance.Path_To_Mission;
				GameObject go = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject> (path)) as GameObject;
				go.transform.SetParent (missionGroup.transform);
				go.AddComponent<Mission> ();
			}
		}

		EditorGUILayout.EndVertical ();
	}
	private void CreateMissionFolder (string folder)
	{
		string pathToSceneFolder = MissionSceneManager.Instance.GenerateSceneFolderPath (folder);
		if (!AssetDatabase.IsValidFolder (pathToSceneFolder)) {
			AssetDatabase.CreateFolder (ConfigMission.Instance.Path_To_Folder_World, folder);
		}
	}

	private bool IsMissionSceneValid ()
	{
		if (m_currentMissionScene.name == null || m_currentMissionScene.name == "") {
			EditorUtility.DisplayDialog ("Not Valid Mission Scene", "Load or Create a valid mission scene", "Ok");
			return false;
		} else {
			return true;
		}
	}

	//private static string Text_Interactable_Object_Group = "Interactables";
	//private static string Text_Event_Trigger_Group = "Triggers";
	//private static string Text_Mission_Group = "Missions";
	//private static string Text_Spawn_Point_Name = "Spawn_Point";

	//private static string Path_To_InteractableObject = $"Assets/data/graphics/Interaction/Interaction_Interactable_Object.prefab";
	//private static string Path_To_WorldTrigger = $"Assets/data/graphics/Interaction/Interaction_World_Trigger.prefab";
	//private static string Path_To_SpawnPoint = $"Assets/data/graphics/Interaction/Interaction_Spawn_Point.prefab";
	//private static string Path_To_Mission = $"Assets/data/graphics/Interaction/Interaction_Missions.prefab";

	//private static string Prefix_Mission_Folder = "Mission_";
	//private static string Prefix_Mission_Scene = "World_Mission_";
	//private static string Path_To_Folder_World = "Assets/data/graphics/World";
}
