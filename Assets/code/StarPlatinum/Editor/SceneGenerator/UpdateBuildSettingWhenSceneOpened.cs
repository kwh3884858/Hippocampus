
using System.IO;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Auto update build setting for scenes when user open a new scene.
/// </summary>
[InitializeOnLoad]
public class EdtorSceneAutomaticOperatioin : Editor
{

	private static readonly string SCENE_ROOT_PATH = "/data/graphics/World";
    private static readonly string MISSION_SCENE_NAME = "Mission";
	static EdtorSceneAutomaticOperatioin ()
	{
		UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += OnSceneOpened;
	}

	static public void UpdateSceneBuildSetting ()
	{

		string path = Application.dataPath + SCENE_ROOT_PATH;

		if (!Directory.Exists (path)) {
			Debug.LogWarning ("Can`t find Assets/Scene. Make sure already execute Assets/Framework/Initialize Framework Directory");
			Debug.LogWarning ("Cuureent setted path: " + path);
			return;
		}


		string [] files = Directory.GetFiles (path, "*.unity", SearchOption.AllDirectories);


		EditorBuildSettingsScene [] scenes = new EditorBuildSettingsScene [files.Length];

		for (int i = 0; i < files.Length; ++i) {
			string scenePath = files [i];

			scenePath = scenePath.Replace ("\\", "/");
			scenePath = scenePath.Substring (scenePath.IndexOf ("Assets" + SCENE_ROOT_PATH));

			scenes [i] = new EditorBuildSettingsScene (scenePath, true);
			scenes [i].enabled = false;

			if (files [i].Contains (SceneLookup.GetString (SceneLookupEnum.World_GameRoot))) // == GameRoot
			{
				scenes [i].enabled = true;
			}
		}

		EditorBuildSettings.scenes = scenes;
		if (scenes.Length == 0) {
			Debug.LogWarning ("Generated new build setting contain 0 scene, please check path of scene root");
		}
		Debug.Log ("Generated new build setting for scenes");
		for (int i = 0; i < scenes.Length; ++i) {
			Debug.Log ("No." + i + "-- Scene Path: " + scenes [i].path);
		}
	}

	static void OnSceneOpened (UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
	{
		CheckForSceneScript (scene, mode);
		UpdateSceneBuildSetting ();
	}

	static void CheckForSceneScript (UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
	{
		GameObject go = GameObject.Find (scene.name);
        if (!scene.name.Contains(MISSION_SCENE_NAME))
        {
            if (go == null)
            {
                new GameObject(scene.name);
            }
        }

        return;

	}

}