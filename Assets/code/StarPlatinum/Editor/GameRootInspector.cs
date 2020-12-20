using Config.GameRoot;
using GamePlay.Stage;
using StarPlatinum.Services;
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

		[SerializeField]
		SceneLookupEnum m_enumStartScene;
		SceneLookupEnum m_enumStartSceneInConfig;

		[SerializeField]
		MissionEnum m_enumStartMission;
		MissionEnum m_enumStartMissionInConfig;

		//[Camera Type Variable Auto Generated Code Begin]



 //[Camera Type Variable Auto Generated Code End]
		override public void OnInspectorGUI ()
		{
			base.DrawDefaultInspector ();

			EditorGUILayout.BeginVertical ();
			EditorGUILayout.LabelField ("Scene Editor");
			EditorGUILayout.LabelField ("编辑游戏中的场景后，确保GameRoot为BuildSetting中序列为0的场景");
			EditorGUILayout.LabelField ("运行根目录下的Tool目录中的SceneGenerator,会自动生成Scene Lookup");
			EditorGUILayout.LabelField ("以及生成root config和该Inspector中的摄像机选择项");
			EditorGUILayout.LabelField ("再回到GameRoot中添加所需场景");

			m_gameRoot = Selection.activeGameObject.gameObject.GetComponent<GameRoot> ();
			if (m_gameRoot == null) return;

			if (ConfigRoot.Instance == null) {
				return;
			}

            GUILayout.Label("Start From This Scene", EditorStyles.boldLabel);

            m_enumStartSceneInConfig = ConfigRoot.Instance.StartScene;
			m_enumStartScene = (SceneLookupEnum)EditorGUILayout.EnumPopup ("Start Scene:", m_enumStartSceneInConfig);
			if (m_enumStartScene != m_enumStartSceneInConfig) {
				ConfigRoot.Instance.StartScene = m_enumStartScene;
				Debug.Log ($"Set {m_enumStartScene.ToString ()} as Start Scene");
            }

			GUILayout.Label ("Start From This Scene", EditorStyles.boldLabel);

			m_enumStartMissionInConfig = ConfigRoot.Instance.StartMission;
			m_enumStartMission = (MissionEnum)EditorGUILayout.EnumPopup ("Start Mission:", m_enumStartMissionInConfig);
			if (m_enumStartMission != m_enumStartMissionInConfig) {
				ConfigRoot.Instance.StartMission = m_enumStartMission;
				Debug.Log ($"Set {m_enumStartMission.ToString ()} as Start Mission");
			}


			GUILayout.Label("Camera Setting", EditorStyles.boldLabel);

            //[Inspector Popup Auto Generated Code Begin]

//[Inspector Popup Auto Generated Code End]



			EditorGUILayout.EndVertical ();
		}
	}
}