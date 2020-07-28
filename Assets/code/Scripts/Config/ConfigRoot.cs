
using UnityEngine;

using StarPlatinum.Service;
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

		[Header ("Scene Config")]

		//[Scene Config Auto Generated Code Begin]

[SerializeField]
private string World_GoundTestSceneName = "World_GoundTestScene"; 
 public CameraService.SceneCameraType World_GoundTestSceneCameraType;  

[SerializeField]
private string World_1F_South_CorriderName = "World_1F_South_Corrider"; 
 public CameraService.SceneCameraType World_1F_South_CorriderCameraType;  

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
private string World_2F_North_CorriderName = "World_2F_North_Corrider"; 
 public CameraService.SceneCameraType World_2F_North_CorriderCameraType;  

[SerializeField]
private string World_Mission_EP02_09_2F_East_Corrider_World_2F_West_CorriderName = "World_Mission_EP02_09_2F_East_Corrider_World_2F_West_Corrider"; 
 public CameraService.SceneCameraType World_Mission_EP02_09_2F_East_Corrider_World_2F_West_CorriderCameraType;  

[SerializeField]
private string World_1F_West_CorriderName = "World_1F_West_Corrider"; 
 public CameraService.SceneCameraType World_1F_West_CorriderCameraType;  

[SerializeField]
private string World_Episode3_HallName = "World_Episode3_Hall"; 
 public CameraService.SceneCameraType World_Episode3_HallCameraType;  

[SerializeField]
private string World_Mission_EP01_03_Poison_Island_PierName = "World_Mission_EP01_03_Poison_Island_Pier"; 
 public CameraService.SceneCameraType World_Mission_EP01_03_Poison_Island_PierCameraType;  

[SerializeField]
private string World_Kitchen_CorriderName = "World_Kitchen_Corrider"; 
 public CameraService.SceneCameraType World_Kitchen_CorriderCameraType;  

[SerializeField]
private string World_2F_West_CorriderName = "World_2F_West_Corrider"; 
 public CameraService.SceneCameraType World_2F_West_CorriderCameraType;  

[SerializeField]
private string World_2F_East_CorriderName = "World_2F_East_Corrider"; 
 public CameraService.SceneCameraType World_2F_East_CorriderCameraType;  

[SerializeField]
private string Poison_Island_PierName = "Poison_Island_Pier"; 
 public CameraService.SceneCameraType Poison_Island_PierCameraType;  

[SerializeField]
private string World_Mission_EP01_01_Poison_Island_PierName = "World_Mission_EP01_01_Poison_Island_Pier"; 
 public CameraService.SceneCameraType World_Mission_EP01_01_Poison_Island_PierCameraType;  

[SerializeField]
private string World_Mission_EP02_01_1F_South_Corrider_World_1F_South_CorriderName = "World_Mission_EP02_01_1F_South_Corrider_World_1F_South_Corrider"; 
 public CameraService.SceneCameraType World_Mission_EP02_01_1F_South_Corrider_World_1F_South_CorriderCameraType;  

[SerializeField]
private string World_Mission_EP01_02_Poison_Island_PierName = "World_Mission_EP01_02_Poison_Island_Pier"; 
 public CameraService.SceneCameraType World_Mission_EP01_02_Poison_Island_PierCameraType;  

[SerializeField]
private string World_2F_South_CorriderName = "World_2F_South_Corrider"; 
 public CameraService.SceneCameraType World_2F_South_CorriderCameraType;  

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
private string World_1F_Middle_CorriderName = "World_1F_Middle_Corrider"; 
 public CameraService.SceneCameraType World_1F_Middle_CorriderCameraType;  

[SerializeField]
private string World_Mission_EP02_11_1F_South_Corrider_World_1F_South_CorriderName = "World_Mission_EP02_11_1F_South_Corrider_World_1F_South_Corrider"; 
 public CameraService.SceneCameraType World_Mission_EP02_11_1F_South_Corrider_World_1F_South_CorriderCameraType;  

[SerializeField]
private string World_Mission_EnterIsland_World_Episode3_HallName = "World_Mission_EnterIsland_World_Episode3_Hall"; 
 public CameraService.SceneCameraType World_Mission_EnterIsland_World_Episode3_HallCameraType;  

//[Scene Config Auto Generated Code End]

		//[Header ("Chapter Config, Please Modify Them In Code")]

		//public List<string> m_chapterName = new List<string> {
		//	"Episode1",
		//	"Pier"
		//};

		//public string GetChapterName (int index) => m_chapterName [index];

		//public bool IsExistChapterName (string name) => m_chapterName.Contains (name);

		public CameraService.SceneCameraType GetCameraTypeBySceneName (string name)
		{
			switch (name) {
			//[Switch Case Auto Generated Code Begin]

case "World_GoundTestScene" :  
   return World_GoundTestSceneCameraType; 

case "World_1F_South_Corrider" :  
   return World_1F_South_CorriderCameraType; 

case "World_Mission_DockByPier_World_Episode2_Pier" :  
   return World_Mission_DockByPier_World_Episode2_PierCameraType; 

case "World_CoreContainer" :  
   return World_CoreContainerCameraType; 

case "World_Mission_DockByPier_World_Episode3_Hall" :  
   return World_Mission_DockByPier_World_Episode3_HallCameraType; 

case "World_Mission_DockByPier_World_Episode4_DoctorRoom" :  
   return World_Mission_DockByPier_World_Episode4_DoctorRoomCameraType; 

case "World_2F_North_Corrider" :  
   return World_2F_North_CorriderCameraType; 

case "World_Mission_EP02_09_2F_East_Corrider_World_2F_West_Corrider" :  
   return World_Mission_EP02_09_2F_East_Corrider_World_2F_West_CorriderCameraType; 

case "World_1F_West_Corrider" :  
   return World_1F_West_CorriderCameraType; 

case "World_Episode3_Hall" :  
   return World_Episode3_HallCameraType; 

case "World_Mission_EP01_03_Poison_Island_Pier" :  
   return World_Mission_EP01_03_Poison_Island_PierCameraType; 

case "World_Kitchen_Corrider" :  
   return World_Kitchen_CorriderCameraType; 

case "World_2F_West_Corrider" :  
   return World_2F_West_CorriderCameraType; 

case "World_2F_East_Corrider" :  
   return World_2F_East_CorriderCameraType; 

case "Poison_Island_Pier" :  
   return Poison_Island_PierCameraType; 

case "World_Mission_EP01_01_Poison_Island_Pier" :  
   return World_Mission_EP01_01_Poison_Island_PierCameraType; 

case "World_Mission_EP02_01_1F_South_Corrider_World_1F_South_Corrider" :  
   return World_Mission_EP02_01_1F_South_Corrider_World_1F_South_CorriderCameraType; 

case "World_Mission_EP01_02_Poison_Island_Pier" :  
   return World_Mission_EP01_02_Poison_Island_PierCameraType; 

case "World_2F_South_Corrider" :  
   return World_2F_South_CorriderCameraType; 

case "World_Episode4_DoctorRoom" :  
   return World_Episode4_DoctorRoomCameraType; 

case "World_GameRoot" :  
   return World_GameRootCameraType; 

case "World_Mission_EnterIsland_World_Episode2_Pier" :  
   return World_Mission_EnterIsland_World_Episode2_PierCameraType; 

case "World_UITestScene" :  
   return World_UITestSceneCameraType; 

case "World_1F_Middle_Corrider" :  
   return World_1F_Middle_CorriderCameraType; 

case "World_Mission_EP02_11_1F_South_Corrider_World_1F_South_Corrider" :  
   return World_Mission_EP02_11_1F_South_Corrider_World_1F_South_CorriderCameraType; 

case "World_Mission_EnterIsland_World_Episode3_Hall" :  
   return World_Mission_EnterIsland_World_Episode3_HallCameraType; 

//[Switch Case Auto Generated Code End]    
			default:
				return CameraService.SceneCameraType.None;
			}
		}
	}
}