
using UnityEngine;

using StarPlatinum.Service;
using StarPlatinum.Base;
using System.Collections.Generic;

namespace Config.GameRoot
{
    /// <summary>
    /// Only for editor
    /// </summary>
    [CreateAssetMenu(fileName = "RootConfig", menuName = "Config/SpawnRootConfig", order = 1)]
    public class RootConfig : ConfigSingleton<RootConfig>
    {
        public class SceneConfig
        {
            [SerializeField]
            private string m_sceneName = "";
            public CameraType m_cameraType;
        }

        [Header("Start Scene")]
        public SceneLookupEnum StartScene;

        [Header("Scene Config")]

        //[Scene Config Auto Generated Code Begin]

[SerializeField]
private string World_GoundTestSceneName = "World_GoundTestScene"; 
 public CameraService.SceneCameraType World_GoundTestSceneCameraType;  

[SerializeField]
private string World_Episode2_PierName = "World_Episode2_Pier"; 
 public CameraService.SceneCameraType World_Episode2_PierCameraType;  

[SerializeField]
private string World_GameRootName = "World_GameRoot"; 
 public CameraService.SceneCameraType World_GameRootCameraType;  

[SerializeField]
private string World_UITestSceneName = "World_UITestScene"; 
 public CameraService.SceneCameraType World_UITestSceneCameraType;  

//[Scene Config Auto Generated Code End]

        [Header("Chapter Config, Please Modify Them In Code")]
        
        public List<string> m_chapterName = new List<string> {
            "Episode1",
            "Pier"
        };

        public string GetChapterName(int index) => m_chapterName[index];

        public bool IsExistChapterName(string name) => m_chapterName.Contains(name);

        public CameraService.SceneCameraType GetCameraTypeBySceneName(string name)
        {
            switch (name)
            {
                //[Switch Case Auto Generated Code Begin]

case "World_GoundTestScene" :  
   return World_GoundTestSceneCameraType; 

case "World_Episode2_Pier" :  
   return World_Episode2_PierCameraType; 

case "World_GameRoot" :  
   return World_GameRootCameraType; 

case "World_UITestScene" :  
   return World_UITestSceneCameraType; 

//[Switch Case Auto Generated Code End]    
                default:
                    return CameraService.SceneCameraType.None;
            }
        }
    }
}