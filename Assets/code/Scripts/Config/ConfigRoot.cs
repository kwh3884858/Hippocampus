
using UnityEngine;

using StarPlatinum.Services;
using StarPlatinum.Base;
using System.Collections.Generic;
using GamePlay.Stage;

namespace Config.GameRoot
{
	/// <summary>
	/// Only for editor
	/// </summary>
	[CreateAssetMenu (fileName = "ConfigRoot", menuName = "Config/SpawnConfigRoot", order = 1)]
	public class ConfigRoot : ConfigSingleton<ConfigRoot>
	{
		//public class SceneConfig
		//{
		//	[SerializeField]
		//	private string m_sceneName = "";
		//	public CameraType m_cameraType;
		//}

		[Header ("Start Scene")]
		public SceneLookupEnum StartScene;

		[Header ("Start Mission")]
		public MissionEnum StartMission;

		//[Header ("Scene Config")]

//[Scene Config Auto Generated Code Begin]


//[Scene Config Auto Generated Code End]

		//[Header ("Chapter Config, Please Modify Them In Code")]

		//public List<string> m_chapterName = new List<string> {
		//	"Episode1",
		//	"Pier"
		//};

		//public string GetChapterName (int index) => m_chapterName [index];

		//public bool IsExistChapterName (string name) => m_chapterName.Contains (name);

		//public CameraService.SceneCameraType GetCameraTypeBySceneName (string name)
		//{
		//	switch (name) {
			//[Switch Case Auto Generated Code Begin]


//[Switch Case Auto Generated Code End]    
			//default:
			//	return CameraService.SceneCameraType.None;
			//}
		//}
	}
}