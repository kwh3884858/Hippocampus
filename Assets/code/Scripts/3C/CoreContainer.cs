using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Base;
using StarPlatinum.Manager;
using UnityEngine.SceneManagement;
using Config.GameRoot;
using StarPlatinum.Service;
using UnityEngine.Assertions;
using GamePlay.EventTrigger;

namespace GamePlay.Stage
{
	public class CoreContainer : Singleton<CoreContainer>
	{
		public CoreContainer ()
		{
			m_player = null;

			Transform root = GameObject.Find ("GameRoot").transform;
			Assert.IsTrue (root != null, "Game Root must always exist.");
			if (root == null) return;
			GameObject manager = new GameObject (typeof (CoreContainer).ToString ());
			manager.transform.SetParent (root.transform);
			m_containerScene = manager.AddComponent<SceneSlot> ();
		}

		public void Initialize ()
		{
			m_containerScene.LoadScene (SceneLookupEnum.World_CoreContainer);
		}

		//After scene loaded, it will call this function
		public void SetSceneLoaded (
			GameObject player,
			GameObject camera,
			bool loaded)
		{
			m_player = player;
			m_isSceneLoaded = loaded;

			SetPlayerDisable ();
			CameraService.Instance.SetMainCamera (camera);
		}

		public void SetPlayerDisable ()
		{
			m_player.SetActive (false);
		}

		public void SetPlayerPosition (Vector3 pos)
		{
			m_player.transform.position = pos;
		}

		public void SpawnPlayer ()
		{
			SceneLookupEnum sceneEnum = MissionSceneManager.Instance.GetCurrentMissionScene ();
			Scene missionScene = SceneManager.GetSceneByName (sceneEnum.ToString ());
			GameObject [] gameObjects = missionScene.GetRootGameObjects ();

			//Teleport from another scene
			m_player.transform.position = Vector3.zero;

			SceneLookupEnum lastGameScene = GameSceneManager.Instance.GetLastSceneEnum ();

			//Direct find spawn point
			foreach (GameObject go in gameObjects) {
				if (go.name == ConfigMission.Instance.Text_Spawn_Point_Name) {
					if (m_player != null) {
						m_player.transform.position =
							new Vector3 (go.transform.position.x, go.transform.position.y + 0.5f, go.transform.position.z);
						m_player.SetActive (true);
						return;
					}
				}
			}
			if (lastGameScene != SceneLookupEnum.World_GameRoot)
			{

				WorldTriggerCallbackTeleportPlayer[] teleports = GameObject.FindObjectsOfType<WorldTriggerCallbackTeleportPlayer>();
				foreach (WorldTriggerCallbackTeleportPlayer teleport in teleports)
				{
					if (teleport.GetTeleportScene() == lastGameScene)
					{
						Vector3 direction = WorldTriggerCallbackTeleportPlayer.DirecitonMapping[teleport.m_spawnDirection];
						direction *= teleport.m_lengthBetweenTriggerAndSpwanPoint;
						m_player.transform.position = teleport.transform.position + direction + new Vector3(0.0f, 0.5f, 0.0f);
						m_player.SetActive(true);
						return;
					}
				}

			}
			Debug.LogError ("Can not find spawn point!");
		}

		public void StopPlayerAnimation ()
		{
			m_player.GetComponent<MonoMoveController> ().StopPlayerAnimation ();
		}

		private bool IsValid ()
		{
			return m_isSceneLoaded == true;
		}

		private SceneSlot m_containerScene;

		private GameObject m_player;
		bool m_isSceneLoaded = false;


	}
}