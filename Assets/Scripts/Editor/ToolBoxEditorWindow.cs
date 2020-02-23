using System.Collections;
using System.Collections.Generic;
using GamePlay;
using UnityEditor;
using UnityEngine;

public class ToolBoxEditorWindow : EditorWindow
{
	string myString = "Hello World";
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
		myString = EditorGUILayout.TextField ("Interactable Group Name", TEXT_INTERACTABLE_OBJECT_Group);
		myString = EditorGUILayout.TextField ("Trigger Group Name", TEXT_EVENT_TRIIGGER_GROUP);

		groupEnabled = EditorGUILayout.BeginToggleGroup ("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle ("Toggle", myBool);
		myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
		EditorGUILayout.EndToggleGroup ();

		GUILayout.Label ("Add Gameobject", EditorStyles.boldLabel);

		EditorGUILayout.BeginHorizontal ();


		if (GUILayout.Button ("Interactable Object")) {
			GameObject interactiablesGroup = GameObject.Find (TEXT_INTERACTABLE_OBJECT_Group);
			if (interactiablesGroup == null) {
				interactiablesGroup = new GameObject (TEXT_INTERACTABLE_OBJECT_Group);
			}

			string path = $"Assets/Prefabs/InteractableObject.prefab";
			GameObject go = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject> (path)) as GameObject;
			//PrefabUtility.UnpackPrefabInstance (go, PrefabUnpackMode.Completely, InteractionMode.UserAction);

			//GameObject go = new GameObject ("New Interactable Object");
			go.transform.SetParent (interactiablesGroup.transform);
			go.AddComponent<InteractiveObject> ();
		}

		if (GUILayout.Button ("Event Trigger")) {
			GameObject triggersGroup = GameObject.Find (TEXT_EVENT_TRIIGGER_GROUP);
			if (triggersGroup == null) {
				triggersGroup = new GameObject (TEXT_EVENT_TRIIGGER_GROUP);
			}

			string path = $"Assets/Prefabs/WorldTrigger.prefab";
			GameObject go = Instantiate (AssetDatabase.LoadAssetAtPath<GameObject> (path)) as GameObject;

			//GameObject go = new GameObject ("New Event Trigger");
			go.transform.SetParent (triggersGroup.transform);
			go.AddComponent<WorldTrigger> ();

		}

		EditorGUILayout.EndHorizontal ();
	}


	private string TEXT_INTERACTABLE_OBJECT_Group = "Interactables";
	private string TEXT_EVENT_TRIIGGER_GROUP = "Triggers";

}
