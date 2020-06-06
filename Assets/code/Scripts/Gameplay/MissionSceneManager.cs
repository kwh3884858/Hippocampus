using System.Collections;
using System.Collections.Generic;
using Config.GameRoot;
using StarPlatinum.Base;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionSceneManager : Singleton<MissionSceneManager>
{
	public string GenerateFolderName (MissionEnum missionEnum)
	{
		return ConfigMission.Instance.Prefix_Mission_Folder + missionEnum.ToString () + "_" + SceneManager.GetActiveScene ().name;
	}

	public string GenerateSceneName (MissionEnum missionEnum)
	{
		return ConfigMission.Instance.Prefix_Mission_Scene + missionEnum.ToString () + "_" + SceneManager.GetActiveScene ().name;
	}

	public bool IsMissionSceneExist (string folder, string sceneName)
	{
		string pathToScene = GenerateSceneFolderPath (folder);
		string [] missionAssets = AssetDatabase.FindAssets (sceneName, new string [] { pathToScene });
		if (missionAssets.Length < 1) {
			Debug.Log ("Cant Find Scene Assets in: " + pathToScene);
			return false;
		}
		if (missionAssets.Length > 1) {
			Debug.LogError ("Scene Assets more than one: " + pathToScene);
			foreach (var sceneFile in missionAssets) {
				Debug.LogError (sceneFile);
			}
			return false;
		}
		return true;
	}


	public string GenerateScenePath (string folderName, string sceneName)
	{
		return ConfigMission.Instance.Path_To_Folder_World + "/" + folderName + "/" + sceneName + ".unity";
	}

	public string GenerateSceneFolderPath (string folderName)
	{
		return ConfigMission.Instance.Path_To_Folder_World + "/" + folderName;
	}

}
