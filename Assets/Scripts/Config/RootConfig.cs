
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
private string Episode2_PierName = "Episode2_Pier"; 
 public CameraService.SceneCameraType Episode2_PierCameraType;  

[SerializeField]
private string GameRootName = "GameRoot"; 
 public CameraService.SceneCameraType GameRootCameraType;  

[SerializeField]
private string GoundTestSceneName = "GoundTestScene"; 
 public CameraService.SceneCameraType GoundTestSceneCameraType;  

[SerializeField]
private string UITestSceneName = "UITestScene"; 
 public CameraService.SceneCameraType UITestSceneCameraType;

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

case "Episode2_Pier" :  
   return Episode2_PierCameraType; 

case "GameRoot" :  
   return GameRootCameraType; 

case "GoundTestScene" :  
   return GoundTestSceneCameraType; 

case "UITestScene" :  
   return UITestSceneCameraType; 

//[Switch Case Auto Generated Code End]    
                default:
                    return CameraService.SceneCameraType.None;
            }
        }
    }
}