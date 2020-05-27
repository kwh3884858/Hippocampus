using System.Collections;
using System.Collections.Generic;
using GamePlay;
using UnityEditor;
using UnityEngine;

public class ToolBoxEditorWindow : EditorWindow
{
	private string myString = "Hello World";
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
		GUILayout.Label ("Group Name Constant", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField ("Interactable Group Name", Text_Interactable_Object_Group);
		myString = EditorGUILayout.TextField ("Trigger Group Name", Text_Event_Trigger_Group);

		GUILayout.Label ("Prefab Object Name", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField ("Spawn Point Name", Text_Spawn_Point_Name);

		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
		myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);

		GUILayout.Label ("Path to Prefab", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField ("Path to Interactable Object", Path_To_InteractableObject);
		myString = EditorGUILayout.TextField ("Path to World Trigger", Path_To_WorldTrigger);
		myString = EditorGUILayout.TextField ("Path to Spawn Point", Path_To_SpawnPoint);

		EditorGUILayout.EndToggleGroup ();

		GUILayout.Label ("Add Gameobject", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical ();


		if (GUILayout.Button ("Interactable Object")) {
			GameObject interactiablesGroup = GameObject.Find (Text_Interactable_Object_Group);
			if (interactiablesGroup == null) {
				interactiablesGroup = new GameObject (Text_Interactable_Object_Group);
			}

			string path = Path_To_InteractableObject;
			GameObject go = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject> (path)) as GameObject;
			//PrefabUtility.UnpackPrefabInstance (go, PrefabUnpackMode.Completely, InteractionMode.UserAction);

			//GameObject go = new GameObject ("New Interactable Object");
			go.transform.SetParent (interactiablesGroup.transform);
			go.AddComponent<InteractiveObject> ();
		}

		if (GUILayout.Button ("Event Trigger")) {
			GameObject triggersGroup = GameObject.Find (Text_Event_Trigger_Group);
			if (triggersGroup == null) {
				triggersGroup = new GameObject (Text_Event_Trigger_Group);
			}

			string path = Path_To_WorldTrigger;
			GameObject go = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject> (path)) as GameObject;

			//GameObject go = new GameObject ("New Event Trigger");
			go.transform.SetParent (triggersGroup.transform);
			go.AddComponent<WorldTrigger> ();

		}

		if (GUILayout.Button ("Create Spawn Point")) {
			GameObject spawnPoint = GameObject.Find (Text_Spawn_Point_Name);
			if (spawnPoint != null) {
				EditorUtility.DisplayDialog ("Error", "Already contain a spawn point in scene.", "Ok");
			} else {
				string path = Path_To_SpawnPoint;
				GameObject go = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject> (path)) as GameObject;
				go.name = Text_Spawn_Point_Name;

			}


		}

		EditorGUILayout.EndVertical ();
	}


	private string Text_Interactable_Object_Group = "Interactables";
	private string Text_Event_Trigger_Group = "Triggers";
	private string Text_Spawn_Point_Name = "Spawn_Point";

	private string Path_To_InteractableObject = $"Assets/data/graphics/Interaction/Interaction_Interactable_Object.prefab";
	private string Path_To_WorldTrigger = $"Assets/data/graphics/Interaction/Interaction_World_Trigger.prefab";
	private string Path_To_SpawnPoint = $"Assets/data/graphics/Interaction/Interaction_Spawn_Point.prefab";

}
