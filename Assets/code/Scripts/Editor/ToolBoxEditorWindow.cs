using System;
using System.Collections;
using System.Collections.Generic;
using Config.GameRoot;
using GamePlay;
using GamePlay.EventTrigger;
using GamePlay.Stage;
using StarPlatinum.Manager;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolBoxEditorWindow : EditorWindow
{
	private MissionEnum m_currentMissionEnum = MissionEnum.None;
	private Scene m_currentMissionScene = new Scene ();
	private Scene m_currentGameScene = new Scene ();
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
		if (Application.isPlaying) {
			return;
		}
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
		if (m_currentGameScene.name == null || m_currentGameScene.name == "") {
			m_currentGameScene = SceneManager.GetActiveScene ();
		}
		EditorGUILayout.LabelField ("Current Active Game Sccene:  " + m_currentGameScene.name);
		EditorGUILayout.LabelField ("Current Active Mission Scene:  " + m_currentMissionScene.name);

		m_currentMissionEnum = (MissionEnum)EditorGUILayout.EnumPopup ("Current Mission", m_currentMissionEnum);
		if (GUILayout.Button ("Create Mission Scene")) {
			if (m_currentMissionEnum != MissionEnum.None) {
				RemoveCurrentMissionSceneInternal ();
				CreateMissionSceneInternal (m_currentMissionEnum);
			} else {
				EditorUtility.DisplayDialog ("Select A Mission", "Select a valid mission for edit", "Ok");
			}
		}

		if (GUILayout.Button ("Load Mission Scene")) {
			if (m_currentMissionEnum != MissionEnum.None) {
				RemoveCurrentMissionSceneInternal ();
				LoadMissionSceneInternal (m_currentMissionEnum);
			} else {
				EditorUtility.DisplayDialog ("Select A Mission", "Select a valid mission for edit", "Ok");
			}
		}

		if (GUILayout.Button ("Remove Mission Scene")) {
			RemoveCurrentMissionSceneInternal ();
		}
		GUILayout.Label ("Add Gameobject", EditorStyles.boldLabel);

		if (GUILayout.Button ("Create Interactable Object")) {
			if (IsMissionSceneValid ()) {
				GameObject interactiablesGroup = GameObject.Find (ConfigMission.Instance.Text_Interactable_Object_Group);
				if (interactiablesGroup == null) {
					interactiablesGroup = new GameObject (ConfigMission.Instance.Text_Interactable_Object_Group);
				}

				string path = ConfigMission.Instance.Path_To_InteractableObject;
				GameObject go = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject> (path)) as GameObject;

				go.transform.SetParent (interactiablesGroup.transform);
				go.AddComponent<InteractiveObject> ();
			} else {
				EditorUtility.DisplayDialog ("Not Valid Mission Scene", "Load or Create a valid mission scene", "Ok");
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
			} else {
				EditorUtility.DisplayDialog ("Not Valid Mission Scene", "Load or Create a valid mission scene", "Ok");
			}
		}

		if (GUILayout.Button ("Create Event Trigger")) {
			if (IsMissionSceneValid ()) {
				CreateEventTrigger (m_currentMissionScene);
			} else {
				EditorUtility.DisplayDialog ("Not Valid Mission Scene", "Load or Create a valid mission scene", "Ok");
			}
		}
		if (GUILayout.Button ("Create Event Trigger With [Prefab: Load New Story]")) {
			if (IsMissionSceneValid ()) {
				GameObject loadNewStory = CreateEventTrigger (m_currentMissionScene);
				loadNewStory.name = "Load_New_Story";
				loadNewStory.AddComponent<WorldTriggerCallbackLoadNewStory> ();
			} else {
				EditorUtility.DisplayDialog ("Not Valid Mission Scene", "Load or Create a valid mission scene", "Ok");
			}
		}
		if (GUILayout.Button ("Create Event Trigger With [Prefab: Create New Teleport Point]")) {
			if (IsGameSceneValid ()) {
				GameObject loadNewStory = CreateEventTrigger (m_currentGameScene);
				loadNewStory.name = "Teleport_Point";
				loadNewStory.AddComponent<WorldTriggerCallbackTeleportPlayer> ();
			} else {
				EditorUtility.DisplayDialog ("Not Valid Mission Scene", "Load or Create a valid mission scene", "Ok");
			}
		}

		EditorGUILayout.EndVertical ();
	}

	private GameObject CreateEventTrigger (Scene activeScene)
	{
		Scene currentActiveScene = SceneManager.GetActiveScene ();
		SceneManager.SetActiveScene (activeScene);

		GameObject triggersGroup = null;
		GameObject [] rootGameObjects = activeScene.GetRootGameObjects ();
		foreach (GameObject rootObject in rootGameObjects) {
			if (rootObject.name == ConfigMission.Instance.Text_Event_Trigger_Group) {
				triggersGroup = rootObject;
			}
		}
		if (triggersGroup == null) {
			triggersGroup = new GameObject (ConfigMission.Instance.Text_Event_Trigger_Group);
		}

		string path = ConfigMission.Instance.Path_To_WorldTrigger;
		GameObject go = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject> (path)) as GameObject;

		go.name = "Event Trigger";
		go.transform.SetParent (triggersGroup.transform);
		go.AddComponent<WorldTrigger> ();
		go.AddComponent<EventRegister> ();

		SceneManager.SetActiveScene (currentActiveScene);
		return go;
	}

	private void LoadMissionSceneInternal (MissionEnum missionEnum)
	{
		string folderName = ConfigMission.Instance.Prefix_Mission_Folder + missionEnum.ToString () + "_" + SceneManager.GetActiveScene ().name;
		string sceneName = ConfigMission.Instance.Prefix_Mission_Scene + missionEnum.ToString () + "_" + SceneManager.GetActiveScene ().name;

		if (m_currentMissionScene.name != sceneName) {
			string pathToSceneFolder = MissionSceneManager.Instance.GenerateFullSceneFolderPath (folderName);
			if (!AssetDatabase.IsValidFolder (pathToSceneFolder)) {
				EditorUtility.DisplayDialog ("Scene Folder Not Exist", "Click [Create Mission Scene] to create new mission scene", "Ok");
			}
			bool isExistMissionScene = MissionSceneManager.Instance.IsFileMissionSceneExistInAssets (folderName, sceneName);
			if (isExistMissionScene) {
				string pathToScene = MissionSceneManager.Instance.GenerateFullScenePath (folderName, sceneName);
				m_currentMissionScene = EditorSceneManager.OpenScene (pathToScene, OpenSceneMode.Additive);
				EditorSceneManager.SetActiveScene (m_currentMissionScene);
			} else {
				EditorUtility.DisplayDialog ("Scene Not Exist", "Click [Create Mission Scene] to create new mission scene", "Ok");
			}
		}
	}

	private void RemoveCurrentMissionSceneInternal ()
	{
		if (IsMissionSceneValid ()) {
			EditorSceneManager.SaveScene (m_currentMissionScene);
			EditorSceneManager.CloseScene (m_currentMissionScene, true);
			SetCurrentScene (new Scene ());
		}
	}

	private void CreateMissionSceneInternal (MissionEnum missionEnum)
	{
		string folderName = MissionSceneManager.Instance.GenerateFolderName (missionEnum);
		string sceneName = MissionSceneManager.Instance.GenerateSceneName (missionEnum);
		bool isConfirm = EditorUtility.DisplayDialog ("Create New Scene", "New scene file will be create.\nScene Name: " + sceneName + "\nLocation: data/graphics/World/" + folderName, "Ok, create it", "Cancel");
		if (isConfirm) {
			CreateMissionFolder (folderName);

			bool isExistMissionScene = MissionSceneManager.Instance.IsFileMissionSceneExistInAssets (folderName, sceneName);
			if (!isExistMissionScene) {
				Scene newScene = EditorSceneManager.NewScene (NewSceneSetup.EmptyScene, NewSceneMode.Additive);
				newScene.name = sceneName;
				string fullScenePath = MissionSceneManager.Instance.GenerateFullScenePath (folderName, sceneName);

				bool saveOK = EditorSceneManager.SaveScene (newScene, fullScenePath);
				UnityEditor.AddressableAssets.Settings.AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.GetSettings (false);

				//Make a gameobject an addressable
				AddressableAssetGroup missionSceneGroup = settings.FindGroup ("MissionScene");
				string guid = AssetDatabase.AssetPathToGUID (fullScenePath);

				//This is the function that actually makes the object addressable
				var entry = settings.CreateOrMoveEntry (guid, missionSceneGroup ? missionSceneGroup : settings.DefaultGroup);
				entry.address = sceneName;

				//You'll need these to run to save the changes!
				settings.SetDirty (AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
				AssetDatabase.SaveAssets ();

				if (!saveOK) {
					Debug.LogError ("Save is faild. Scene:" + sceneName);
				} else {
					SetCurrentScene (newScene);
					EdtorSceneAutomaticOperatioin.UpdateSceneBuildSetting ();
				}
			} else {
				EditorUtility.DisplayDialog ("Scene Conflict", "Scene file Already Exist, Please load this scene.\nScene Name: " + sceneName + "\nLocation: data/graphics/World" + folderName, "Ok");
			}
		}
	}

	private void CreateMissionFolder (string folder)
	{
		string pathToSceneFolder = MissionSceneManager.Instance.GenerateFullSceneFolderPath (folder);
		if (!AssetDatabase.IsValidFolder (pathToSceneFolder)) {
			AssetDatabase.CreateFolder (ConfigMission.Instance.Path_To_Folder_World, folder);
		}
	}

	private bool IsMissionSceneValid ()
	{
		if (m_currentMissionScene == null || m_currentMissionScene.name == null || m_currentMissionScene.name == "") {
			return false;
		} else {
			return true;
		}
	}

	private bool IsGameSceneValid ()
	{
		if (m_currentGameScene == null || m_currentGameScene.name == null || m_currentGameScene.name == "") {
			return false;
		} else {
			return true;
		}
	}

	private void SetCurrentScene (Scene scene)
	{
		m_currentMissionScene = scene;
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
