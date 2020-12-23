using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Base;
using StarPlatinum.Manager;
using UnityEngine.SceneManagement;
using Config.GameRoot;
using StarPlatinum.Services;
using UnityEngine.Assertions;
using GamePlay.EventTrigger;
using UnityEngine.AI;
using StarPlatinum;

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
            //TODO
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
            m_monoMoveController = m_player.GetComponent<MonoMoveController>();

            SetPlayerDisable();
			CameraService.Instance.SetMainCamera (camera);
            CameraService.Instance.InitializeRaycast();
		}

		public void SetPlayerDisable ()
		{
			m_player.SetActive (false);
		}

        public void SetCharacterSpeed(float speedArgument)
        {
            m_monoMoveController.SetCharacterSpeed(speedArgument);
        }

        public void InteractWith(Collider collider)
        {
            m_monoMoveController.InteractWith(collider);
        }

        public void SpawnPlayer ()
		{
            if (m_needToSpawnOnLastPositionFromArchive)
            {
                m_needToSpawnOnLastPositionFromArchive = false;
                m_player.transform.position = m_playerPosition;
                m_player.SetActive(true);
                return;
            }

			SceneLookupEnum currentMissionSceneEnum = MissionSceneManager.Instance.GetCurrentMissionSceneEnum();
			SceneLookupEnum currentGameSceneEnum = GameSceneManager.Instance.GetCurrentSceneEnum();
			Scene currentMissionScene = SceneManager.GetSceneByName (currentMissionSceneEnum.ToString ());
			Scene currentGameScene = SceneManager.GetSceneByName(currentGameSceneEnum.ToString());
			GameObject [] missionSceneGameobjects = currentMissionScene.GetRootGameObjects ();
			GameObject[] gameSceneGameobjects = currentGameScene.GetRootGameObjects();
			string specificTeleportName = GameSceneManager.Instance.GetSpecificTeleportName();

			SceneLookupEnum lastGameSceneEnum = GameSceneManager.Instance.GetLastSceneEnum ();
            SceneLookupEnum lastMissionSceneEnum = MissionSceneManager.Instance.GetLastMissionScecneEnum();

            MissionSceneManager.LoadMissionBy loadMissionBy = MissionSceneManager.Instance.GetLoadMissionBy();

            //Fisrt load game
            if (lastGameSceneEnum == SceneLookupEnum.World_GameRoot && lastMissionSceneEnum == SceneLookupEnum.World_GameRoot)
            {
                //Direct find spawn point
                foreach (GameObject go in missionSceneGameobjects)
                {
                    if (go.name == ConfigMission.Instance.Text_Spawn_Point_Name)
                    {
                        if (m_player != null)
                        {
                            m_player.transform.position =
                                new Vector3(go.transform.position.x, go.transform.position.y + 0.5f, go.transform.position.z);
                            m_player.SetActive(true);
                            return;
                        }
                    }
                }

                //Find spawn point in game scene
                foreach (GameObject go in gameSceneGameobjects)
                {
                    if (go.name == ConfigMission.Instance.Text_Spawn_Point_Name)
                    {
                        if (m_player != null)
                        {
                            m_player.transform.position =
                                new Vector3(go.transform.position.x, go.transform.position.y + 0.5f, go.transform.position.z);
                            m_player.SetActive(true);
                            return;
                        }
                    }
                }
            }
            else
            {
                //Not first time

                if (loadMissionBy != MissionSceneManager.LoadMissionBy.Teleport)
                {
                    //Load mission by story, mission enum will different. But game scene are the same.
                    //PS: Load anonymous will not change mission enum, but will change mission scene enum

                    //Do Nothing
                    m_player.SetActive(true);
                    return;
                }
                else
                {
                    //teleport
                    WorldTriggerCallbackTeleportPlayer[] teleports = GameObject.FindObjectsOfType<WorldTriggerCallbackTeleportPlayer>();
                    foreach (WorldTriggerCallbackTeleportPlayer teleport in teleports)
                    {
                        if (teleport.IsCGScene() == true)
                        {
                            continue;
                        }

                        if (teleport.GetTeleportScene() == lastGameSceneEnum)
                        {
                            if (specificTeleportName != "" && specificTeleportName != teleport.gameObject.name)
                            {
                                continue;
                            }
                            Vector3 direction = WorldTriggerCallbackTeleportPlayer.DirecitonMapping[teleport.m_spawnDirection];
                            direction *= teleport.m_lengthBetweenTriggerAndSpwanPoint;
                            m_player.transform.position = teleport.transform.position + direction + new Vector3(0.0f, 0.5f, 0.0f);
                            m_player.SetActive(true);
                            return;
                        }
                    }

                    //No suitable spawn point
                    Debug.LogError("Can not find spawn point!");
                    m_player.transform.position = new Vector3(0, 0.5f, 0);
                    m_player.SetActive(true);
                    return;
                }
            }
		}

		public void StopPlayerAnimation ()
		{
            m_monoMoveController.StopPlayerAnimation ();
		}

		public void EnablePlayerInteractability ()
		{
            m_monoMoveController.SetInteract();
		}

		private bool IsValid ()
		{
			return m_isSceneLoaded == true;
		}

        public void SetSpawnPositionFromArchive(Vector3 playerPos)
        {
            m_needToSpawnOnLastPositionFromArchive = true;
            m_playerPosition = playerPos;
        }

        public Vector3 GetPlayerPosition()
        {
            return m_player.transform.position;
        }

        public void SetPlayerPosition(Vector3 pos)
        {
            m_player.transform.position = pos;
        }

        private SceneSlot m_containerScene;

		private GameObject m_player;
        private MonoMoveController m_monoMoveController;
		bool m_isSceneLoaded = false;

        bool m_needToSpawnOnLastPositionFromArchive = false;
        Vector3 m_playerPosition = Vector3.zero;

	}
}