
using UnityEngine;

using StarPlatinum.Service;
using StarPlatinum.Base;
using System.Collections.Generic;

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

		[Header ("Scene Config")]

		//[Scene Config Auto Generated Code Begin]

[SerializeField]
private string World_GoundTestSceneName = "World_GoundTestScene"; 
 public CameraService.SceneCameraType World_GoundTestSceneCameraType;  

[SerializeField]
private string World_Mission_DockByPier_World_Episode2_PierName = "World_Mission_DockByPier_World_Episode2_Pier"; 
 public CameraService.SceneCameraType World_Mission_DockByPier_World_Episode2_PierCameraType;  

[SerializeField]
private string World_CoreContainerName = "World_CoreContainer"; 
 public CameraService.SceneCameraType World_CoreContainerCameraType;  

[SerializeField]
private string World_Mission_DockByPier_World_Episode3_HallName = "World_Mission_DockByPier_World_Episode3_Hall"; 
 public CameraService.SceneCameraType World_Mission_DockByPier_World_Episode3_HallCameraType;  

[SerializeField]
private string World_Mission_DockByPier_World_Episode4_DoctorRoomName = "World_Mission_DockByPier_World_Episode4_DoctorRoom"; 
 public CameraService.SceneCameraType World_Mission_DockByPier_World_Episode4_DoctorRoomCameraType;  

[SerializeField]
private string World_Episode3_HallName = "World_Episode3_Hall"; 
 public CameraService.SceneCameraType World_Episode3_HallCameraType;  

[SerializeField]
private string World_Episode2_PierName = "World_Episode2_Pier"; 
 public CameraService.SceneCameraType World_Episode2_PierCameraType;  

[SerializeField]
private string World_Episode4_DoctorRoomName = "World_Episode4_DoctorRoom"; 
 public CameraService.SceneCameraType World_Episode4_DoctorRoomCameraType;  

[SerializeField]
private string World_GameRootName = "World_GameRoot"; 
 public CameraService.SceneCameraType World_GameRootCameraType;  

[SerializeField]
private string World_Mission_EnterIsland_World_Episode2_PierName = "World_Mission_EnterIsland_World_Episode2_Pier"; 
 public CameraService.SceneCameraType World_Mission_EnterIsland_World_Episode2_PierCameraType;  

[SerializeField]
private string World_UITestSceneName = "World_UITestScene"; 
 public CameraService.SceneCameraType World_UITestSceneCameraType;  

[SerializeField]
private string World_Mission_EnterIsland_World_Episode3_HallName = "World_Mission_EnterIsland_World_Episode3_Hall"; 
 public CameraService.SceneCameraType World_Mission_EnterIsland_World_Episode3_HallCameraType;  

//[Scene Config Auto Generated Code End]

		[Header ("Chapter Config, Please Modify Them In Code")]

		public List<string> m_chapterName = new List<string> {
			"Episode1",
			"Pier"
		};

		public string GetChapterName (int index) => m_chapterName [index];

		public bool IsExistChapterName (string name) => m_chapterName.Contains (name);

		public CameraService.SceneCameraType GetCameraTypeBySceneName (string name)
		{
			switch (name) {
			//[Switch Case Auto Generated Code Begin]

case "World_GoundTestScene" :  
   return World_GoundTestSceneCameraType; 

case "World_Mission_DockByPier_World_Episode2_Pier" :  
   return World_Mission_DockByPier_World_Episode2_PierCameraType; 

case "World_CoreContainer" :  
   return World_CoreContainerCameraType; 

case "World_Mission_DockByPier_World_Episode3_Hall" :  
   return World_Mission_DockByPier_World_Episode3_HallCameraType; 

case "World_Mission_DockByPier_World_Episode4_DoctorRoom" :  
   return World_Mission_DockByPier_World_Episode4_DoctorRoomCameraType; 

case "World_Episode3_Hall" :  
   return World_Episode3_HallCameraType; 

case "World_Episode2_Pier" :  
   return World_Episode2_PierCameraType; 

case "World_Episode4_DoctorRoom" :  
   return World_Episode4_DoctorRoomCameraType; 

case "World_GameRoot" :  
   return World_GameRootCameraType; 

case "World_Mission_EnterIsland_World_Episode2_Pier" :  
   return World_Mission_EnterIsland_World_Episode2_PierCameraType; 

case "World_UITestScene" :  
   return World_UITestSceneCameraType; 

case "World_Mission_EnterIsland_World_Episode3_Hall" :  
   return World_Mission_EnterIsland_World_Episode3_HallCameraType; 

//[Switch Case Auto Generated Code End]    
			default:
				return CameraService.SceneCameraType.None;
			}
		}
	}
}