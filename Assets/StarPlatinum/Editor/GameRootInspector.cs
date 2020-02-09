using Config.GameRoot;
using StarPlatinum.Service;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace StarPlatinum
{ 
	[CustomEditor (typeof (GameRoot))]
	public class GameRootInspector : Editor
	{
        GameRoot m_gameRoot;
        SceneLookupEnum m_enumStartScene;

        //[Camera Type Variable Auto Generated Code Begin]

CameraService.SceneCameraType m_Episode2_PierCameraType; 

 CameraService.SceneCameraType m_GameRootCameraType; 

 CameraService.SceneCameraType m_GoundTestSceneCameraType; 

 CameraService.SceneCameraType m_UITestSceneCameraType; 

 //[Camera Type Variable Auto Generated Code End]
        override public void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

            EditorGUILayout.BeginVertical ();
			EditorGUILayout.LabelField ("Scene Editor");
			EditorGUILayout.LabelField ("编辑游戏中的场景后，确保GameRoot为BuildSetting中序列为0的场景");
			EditorGUILayout.LabelField ("运行根目录下的Tool目录中���SceneGenerator,这会自动生成Lookup");
			EditorGUILayout.LabelField ("再回到GameRoot中添加所需场景");


            m_gameRoot = Selection.activeGameObject.gameObject.GetComponent<GameRoot>();
            if (m_gameRoot == null) return;

            if (RootConfig.Instance == null)
            {
                return ;
            }

            m_enumStartScene = RootConfig.Instance.StartScene;
            m_enumStartScene = (SceneLookupEnum)EditorGUILayout.EnumPopup("Start Scene:", m_enumStartScene);

            if (m_enumStartScene != m_gameRoot.m_startScene)
            {
                m_gameRoot.m_startScene = m_enumStartScene;
                RootConfig.Instance.StartScene = m_enumStartScene;
                Debug.Log($"Set {m_enumStartScene.ToString()} as Start Scene");
            }

            //[Inspector Popup Auto Generated Code Begin]

m_Episode2_PierCameraType = RootConfig.Instance.Episode2_PierCameraType;  
m_Episode2_PierCameraType = (CameraService.SceneCameraType)EditorGUILayout.EnumPopup("Episode2_Pier Camera Type: ", m_Episode2_PierCameraType); 
    if (m_Episode2_PierCameraType != RootConfig.Instance.Episode2_PierCameraType) 
{ 
RootConfig.Instance.Episode2_PierCameraType = m_Episode2_PierCameraType; 
} 

m_GameRootCameraType = RootConfig.Instance.GameRootCameraType;  
m_GameRootCameraType = (CameraService.SceneCameraType)EditorGUILayout.EnumPopup("GameRoot Camera Type: ", m_GameRootCameraType); 
    if (m_GameRootCameraType != RootConfig.Instance.GameRootCameraType) 
{ 
RootConfig.Instance.GameRootCameraType = m_GameRootCameraType; 
} 

m_GoundTestSceneCameraType = RootConfig.Instance.GoundTestSceneCameraType;  
m_GoundTestSceneCameraType = (CameraService.SceneCameraType)EditorGUILayout.EnumPopup("GoundTestScene Camera Type: ", m_GoundTestSceneCameraType); 
    if (m_GoundTestSceneCameraType != RootConfig.Instance.GoundTestSceneCameraType) 
{ 
RootConfig.Instance.GoundTestSceneCameraType = m_GoundTestSceneCameraType; 
} 

m_UITestSceneCameraType = RootConfig.Instance.UITestSceneCameraType;  
m_UITestSceneCameraType = (CameraService.SceneCameraType)EditorGUILayout.EnumPopup("UITestScene Camera Type: ", m_UITestSceneCameraType); 
    if (m_UITestSceneCameraType != RootConfig.Instance.UITestSceneCameraType) 
{ 
RootConfig.Instance.UITestSceneCameraType = m_UITestSceneCameraType; 
} 

//[Inspector Popup Auto Generated Code End]



            EditorGUILayout.EndVertical ();
		}
	}
}