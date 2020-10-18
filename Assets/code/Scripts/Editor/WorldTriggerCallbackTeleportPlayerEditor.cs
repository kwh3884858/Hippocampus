using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Config.GameRoot;
using GamePlay.Stage;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using DesperateDevs.Unity.Editor;

namespace GamePlay.EventTrigger
{
	[CustomEditor (typeof (WorldTriggerCallbackTeleportPlayer))]
	public class WorldTriggerCallbackTeleportPlayerEditor : Editor
	{
		void OnSceneGUI ()
		{
			WorldTriggerCallbackTeleportPlayer teleport = (WorldTriggerCallbackTeleportPlayer)target;
			Vector3 pos = teleport.transform.position;
			Vector3 relativePos = teleport.m_lengthBetweenTriggerAndSpwanPoint * WorldTriggerCallbackTeleportPlayer.DirecitonMapping [teleport.m_spawnDirection];

			Handles.color = Color.red;
			Handles.DrawLine (pos, pos + relativePos);
			Handles.color = Color.green;
			Handles.DrawLine (pos + relativePos, pos + relativePos + Vector3.up);
		}

		override public void OnInspectorGUI ()
		{
			WorldTriggerCallbackTeleportPlayer trigger = (WorldTriggerCallbackTeleportPlayer)target;

			base.DrawDefaultInspector ();

            if (trigger.m_isCGScene == false) {
                foldoutType = EditorGUILayout.Foldout(foldoutType, "Auto Input Teleport Scene");
                if (trigger.GetTeleportScene() == SceneLookupEnum.World_Invalid)
                {
                    GUI.backgroundColor = Color.red;
                    EditorGUILayout.TextArea("Teleport scene name is illegal");
                    GUI.backgroundColor = Color.white;
                    foldoutType = true;
                }

                if (foldoutType)
				{
					GUI.backgroundColor = Color.green;
					m_sceneLookupEnum = (SceneLookupEnum)EditorGUILayout.EnumPopup("Needed Mission:", m_sceneLookupEnum);
					if (GUILayout.Button("Update Teleported Game Scene"))
					{
						trigger.SetTeleportedScene(m_sceneLookupEnum);
						foldoutType = false;
					}
					GUI.backgroundColor = Color.white;
				}

				if (GUILayout.Button("Open teleport scene"))
				{
                    SceneLookupEnum sceneLookupEnum = trigger.GetTeleportScene();

                    if (sceneLookupEnum != SceneLookupEnum.World_Invalid)
                    {
                        string sceneName = sceneLookupEnum.ToString();
                        string folderName = sceneName.Substring(sceneName.IndexOf('_') + 1);

                        bool isExistMissionScene = MissionSceneManager.Instance.IsFileMissionSceneExistInAssets(folderName, sceneName);
                        if (isExistMissionScene)
                        {
                            string pathToScene = MissionSceneManager.Instance.GenerateFullScenePath(folderName, sceneName);
                            Scene currentMissionScene = EditorSceneManager.OpenScene(pathToScene, OpenSceneMode.Single);
                            EditorSceneManager.SetActiveScene(currentMissionScene);
                        }
                        else
                        {
                            EditorUtility.DisplayDialog("Scene Not Exist", "Make sure scene enum is valid. You should use SceneGenerator to re-generate the scene lookup file", "Ok");
                        }
                    }
                }
			}

			if (GUI.changed) {
				EditorUtility.SetDirty (target);
			}

		}
		public SceneLookupEnum m_sceneLookupEnum;
        bool foldoutType = false;

    }
}