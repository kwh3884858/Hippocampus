using Config.GameRoot;
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

        CameraService.SceneCameraType m_chapter2ScenePierCameraType;

        override public void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

            EditorGUILayout.BeginVertical ();
			EditorGUILayout.LabelField ("Scene Editor");
			EditorGUILayout.LabelField ("编辑游戏中的场景后，确保GameRoot为BuildSetting中序列为0的场景");
			EditorGUILayout.LabelField ("运行根目录下的Tool目录中的SceneGenerator,这会自动生成Lookup");
			EditorGUILayout.LabelField ("再回到GameRoot中添加所需场景");


            m_gameRoot = Selection.activeGameObject.gameObject.GetComponent<GameRoot>();
            if (m_gameRoot == null) return;

            m_enumStartScene = RootConfig.Instance.StartScene;
            m_enumStartScene = (SceneLookupEnum)EditorGUILayout.EnumPopup("Start Scene:", m_enumStartScene);

            if (m_enumStartScene != m_gameRoot.m_startScene)
            {
                m_gameRoot.m_startScene = m_enumStartScene;
                RootConfig.Instance.StartScene = m_enumStartScene;
                Debug.Log($"Set {m_enumStartScene.ToString()} as Start Scene");
            }

            m_chapter2ScenePierCameraType = RootConfig.Instance.Chapter2ScenePierCameraType;
            m_chapter2ScenePierCameraType = (CameraService.SceneCameraType)EditorGUILayout.EnumPopup("Scene Pier Camera Type:", m_chapter2ScenePierCameraType);

            if (m_chapter2ScenePierCameraType != RootConfig.Instance.Chapter2ScenePierCameraType)
            {
                RootConfig.Instance.Chapter2ScenePierCameraType = m_chapter2ScenePierCameraType;
            }

            EditorGUILayout.EndVertical ();
		}
	}
}