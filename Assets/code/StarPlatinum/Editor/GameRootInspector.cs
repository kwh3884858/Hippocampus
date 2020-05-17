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

CameraService.SceneCameraType m_World_GoundTestSceneCameraType; 

 CameraService.SceneCameraType m_World_Episode2_PierCameraType; 

 CameraService.SceneCameraType m_World_GameRootCameraType; 

 CameraService.SceneCameraType m_World_UITestSceneCameraType; 

 //[Camera Type Variable Auto Generated Code End]
		override public void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			EditorGUILayout.BeginVertical ();
			EditorGUILayout.LabelField ("Scene Editor");
			EditorGUILayout.LabelField ("编辑游戏中的场景后，确保GameRoot为BuildSetting中序列为0的场景");
			EditorGUILayout.LabelField ("运行根目录下的Tool目录中的SceneGenerator,会自动生成Scene Lookup");
			EditorGUILayout.LabelField ("以及生成root config和该Inspector中的摄像机选择项");
			EditorGUILayout.LabelField ("再回到GameRoot中添加所需场景");


			m_gameRoot = Selection.activeGameObject.gameObject.GetComponent<GameRoot> ();
			if (m_gameRoot == null) return;

			if (RootConfig.Instance == null) {
				return;
			}

			m_enumStartScene = RootConfig.Instance.StartScene;
			m_enumStartScene = (SceneLookupEnum)EditorGUILayout.EnumPopup ("Start Scene:", m_enumStartScene);

			if (m_enumStartScene != m_gameRoot.m_startScene) {
				m_gameRoot.m_startScene = m_enumStartScene;
				RootConfig.Instance.StartScene = m_enumStartScene;
				Debug.Log ($"Set {m_enumStartScene.ToString ()} as Start Scene");
			}

			//[Inspector Popup Auto Generated Code Begin]

m_World_GoundTestSceneCameraType = RootConfig.Instance.World_GoundTestSceneCameraType;  
m_World_GoundTestSceneCameraType = (CameraService.SceneCameraType)EditorGUILayout.EnumPopup("World_GoundTestScene Camera Type: ", m_World_GoundTestSceneCameraType); 
    if (m_World_GoundTestSceneCameraType != RootConfig.Instance.World_GoundTestSceneCameraType) 
{ 
RootConfig.Instance.World_GoundTestSceneCameraType = m_World_GoundTestSceneCameraType; 
} 

m_World_Episode2_PierCameraType = RootConfig.Instance.World_Episode2_PierCameraType;  
m_World_Episode2_PierCameraType = (CameraService.SceneCameraType)EditorGUILayout.EnumPopup("World_Episode2_Pier Camera Type: ", m_World_Episode2_PierCameraType); 
    if (m_World_Episode2_PierCameraType != RootConfig.Instance.World_Episode2_PierCameraType) 
{ 
RootConfig.Instance.World_Episode2_PierCameraType = m_World_Episode2_PierCameraType; 
} 

m_World_GameRootCameraType = RootConfig.Instance.World_GameRootCameraType;  
m_World_GameRootCameraType = (CameraService.SceneCameraType)EditorGUILayout.EnumPopup("World_GameRoot Camera Type: ", m_World_GameRootCameraType); 
    if (m_World_GameRootCameraType != RootConfig.Instance.World_GameRootCameraType) 
{ 
RootConfig.Instance.World_GameRootCameraType = m_World_GameRootCameraType; 
} 

m_World_UITestSceneCameraType = RootConfig.Instance.World_UITestSceneCameraType;  
m_World_UITestSceneCameraType = (CameraService.SceneCameraType)EditorGUILayout.EnumPopup("World_UITestScene Camera Type: ", m_World_UITestSceneCameraType); 
    if (m_World_UITestSceneCameraType != RootConfig.Instance.World_UITestSceneCameraType) 
{ 
RootConfig.Instance.World_UITestSceneCameraType = m_World_UITestSceneCameraType; 
} 

//[Inspector Popup Auto Generated Code End]



			EditorGUILayout.EndVertical ();
		}
	}
}