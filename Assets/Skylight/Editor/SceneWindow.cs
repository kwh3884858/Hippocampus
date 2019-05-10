using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Skylight
{
	public class SceneEditorWindow : EditorWindow
	{
		List<SceneAsset> m_SceneAssets = new List<SceneAsset> ();

		// Add menu item named "Example Window" to the Window menu
		[MenuItem ("Window/SceneEditorWindow")]
		public static void ShowWindow ()
		{
			//Show existing window instance. If one doesn't exist, make one.
			EditorWindow.GetWindow (typeof (SceneEditorWindow));
		}



		void OnGUI ()
		{

			GUILayout.Label ("Scenes to include in build:", EditorStyles.boldLabel);
			for (int i = 0; i < m_SceneAssets.Count; ++i) {
				m_SceneAssets [i] = (SceneAsset)EditorGUILayout.ObjectField (m_SceneAssets [i], typeof (SceneAsset), false);
			}
			if (GUILayout.Button ("Add")) {
				m_SceneAssets.Add (null);
			}

			GUILayout.Space (8);

			if (GUILayout.Button ("Apply To Build Settings")) {
				SetEditorBuildSettingsScenes ();
			}
		}

		public void SetEditorBuildSettingsScenes ()
		{
			// Find valid Scene paths and make a list of EditorBuildSettingsScene
			List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene> ();

			//Scene name list for generate lookup
			List<string> sceneName = new List<string> ();

			foreach (var sceneAsset in m_SceneAssets) {
				if (sceneAsset == null) break;
				string scenePath = AssetDatabase.GetAssetPath (sceneAsset);
				sceneName.Add (sceneAsset.name);
				if (!string.IsNullOrEmpty (scenePath))
					editorBuildSettingsScenes.Add (new EditorBuildSettingsScene (scenePath, true));
			}

			//Generate scene lookup
			//SceneLookupGenerator.CreateSceneLookup (sceneName);

			// Set the Build Settings window Scene list
			EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray ();


		}
	}
}
