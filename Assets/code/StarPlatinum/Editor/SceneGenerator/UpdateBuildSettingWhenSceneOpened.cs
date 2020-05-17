
using System.IO;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Auto update build setting for scenes when user open a new scene.
/// </summary>
[InitializeOnLoad]
public class UpdateBuildSettingWhenSceneOpened : Editor
{

	private static readonly string SCENE_ROOT_PATH = "/data/graphics/World";

	static UpdateBuildSettingWhenSceneOpened ()
	{

		UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += OnSceneOpened;

	}


	static void OnSceneOpened (UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
	{
		CheckForSceneScript (scene, mode);

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

			if (files [i].Contains (SceneLookup.Get (SceneLookupEnum.World_GameRoot))) // == GameRoot
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

	static void CheckForSceneScript (UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
	{
		//GameObject[] gameObjects= scene.GetRootGameObjects();
		//bool isExistSceneRootGameobject = false;
		//foreach (GameObject go in gameObjects)
		//{
		//    if(go.name.IndexOf( scene.name) != -1)
		//    {
		//        isExistSceneRootGameobject = true;
		//    }
		//}

		GameObject go = GameObject.Find (scene.name);

		if (go == null) {
			new GameObject (scene.name);

		}


		return;

	}

}