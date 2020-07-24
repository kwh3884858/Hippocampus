using System.Collections;
using System.Collections.Generic;
using Config.GameRoot;
using StarPlatinum;
using StarPlatinum.Base;
using StarPlatinum.Manager;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace GamePlay.Stage
{
    public enum MissionEnum
    {
        None,
        DockByPier,
        EnterIsland
    };

    public class MissionSceneManager : Singleton<MissionSceneManager>
    {
        public MissionSceneManager()
        {
            if (Application.isPlaying)
            {
                Transform root = GameObject.Find("GameRoot").transform;
                Assert.IsTrue(root != null, "Game Root must always exist.");
                if (root == null) return;

                GameObject manager = new GameObject(typeof(MissionSceneManager).ToString());
                manager.transform.SetParent(root.transform);
                m_currentMissionScene = manager.AddComponent<SceneSlot>();

                m_currentMissionScene.AddCallbackAfterLoaded(delegate ()
                {
                    CoreContainer.Instance.SpawnPlayer();
                });
            }
        }
        public bool LoadCurrentMissionScene()
        {
            return LoadMissionScene(m_currentMission);
        }
        public bool LoadMissionScene(MissionEnum requestMission)
        {
            if (IsMissionSceneExist(requestMission))
            {
                CoreContainer.Instance.SetPlayerDisable();
                string sceneName = GenerateSceneName(requestMission);
                SceneLookupEnum sceneEnum = SceneLookup.GetEnum(sceneName, false);
                SetCurrentMission(requestMission);

                if (!m_currentMissionScene.LoadScene(sceneEnum))
                {
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        public MissionEnum[] GetAllMission()
        {
            return ALL_MISSION;
        }
        public MissionEnum GetCurrentMission()
        {
            return m_currentMission;
        }
        public SceneLookupEnum GetCurrentMissionScene()
        {
            return m_currentMissionScene.GetCurrentSceneEnum();
        }

        private void SetCurrentMission(MissionEnum missionEnum)
        {
            m_currentMission = missionEnum;
        }

        public MissionEnum GetMissionEnumBy(string mission, bool isMatchCase = false)
        {
            foreach (MissionEnum item in ALL_MISSION)
            {
                if (isMatchCase)
                {
                    if (item.ToString() == mission)
                    {
                        return item;
                    }
                }
                else
                {
                    if (item.ToString().ToLower() == mission.ToLower())
                    {
                        return item;
                    }
                }

            }

            return MissionEnum.None;
        }

        public bool IsMissionExist(MissionEnum mission)
        {
            foreach (var item in ALL_MISSION)
            {
                if (item == mission)
                {
                    return true;
                }
            }
            return false;
        }

        public string GenerateFolderName(MissionEnum missionEnum)
        {
            return ConfigMission.Instance.Prefix_Mission_Folder + missionEnum.ToString() + "_" + SceneManager.GetActiveScene().name;
        }

        //Default get current game scene name
        public string GenerateSceneName(MissionEnum missionEnum)
        {
            if (Application.isPlaying)
            {
                Assert.IsTrue(GameSceneManager.Instance.GetCurrentSceneEnum() != SceneLookupEnum.World_GameRoot, "Current game scene is invalid, please do not try to load a mission scene when game scene is valid.");
                //return ConfigMission.Instance.Prefix_Mission_Scene + missionEnum.ToString() + "_" + GameSceneManager.Instance.GetCurrentSceneEnum().ToString();
                return GenerateSceneName(missionEnum, GameSceneManager.Instance.GetCurrentSceneEnum());
            }
            return ConfigMission.Instance.Prefix_Mission_Scene + missionEnum.ToString() + "_" + SceneManager.GetActiveScene().name;
        }

        private string GenerateSceneName(MissionEnum missionEnum, SceneLookupEnum scene)
        {
            return ConfigMission.Instance.Prefix_Mission_Scene + missionEnum.ToString() + "_" + scene.ToString();

        }

        //For runtime
        public bool IsMissionSceneExist(MissionEnum mission)
        {
            if (GameSceneManager.Instance.GetCurrentSceneEnum() == SceneLookupEnum.World_GameRoot)
            {
                Debug.LogError("Current game scene is invalid");
                return false;
            }
            string missionSceneName = GenerateSceneName(mission);
            if (SceneLookup.IsSceneExistNoMatchCase(missionSceneName))
            {
                return true;
            }
            else
            {
                Debug.LogError("Request mission scene is not exist: " + mission);
                return false;
            }
        }
        public bool IsGameSceneExistCurrentMission(SceneLookupEnum gameSceneEnum)
        {
            if (gameSceneEnum == SceneLookupEnum.World_GameRoot)
            {
                Debug.LogError("Game scene enum is invalid");
                return false;
            }
            string missionSceneName = GenerateSceneName(m_currentMission, gameSceneEnum);
            if (SceneLookup.IsSceneExistNoMatchCase(missionSceneName))
            {
                return true;
            }
            else
            {
                Debug.LogError("Request scene is not exist: " + missionSceneName);
                return false;
            }
        }

        //For editor mode
        public bool IsFileMissionSceneExistInAssets(string folder, string sceneName)
        {
            string pathToScene = GenerateFullSceneFolderPath(folder);
            string[] missionAssets = AssetDatabase.FindAssets(sceneName, new string[] { pathToScene });
            if (missionAssets.Length < 1)
            {
                Debug.Log("Cant Find Scene Assets in: " + pathToScene);
                return false;
            }
            if (missionAssets.Length > 1)
            {
                Debug.LogError("Scene Assets more than one: " + pathToScene);
                foreach (var sceneFile in missionAssets)
                {
                    Debug.LogError(sceneFile);
                }
                return false;
            }
            return true;
        }

        public string GenerateFullScenePath(string folderName, string sceneName)
        {
            return GenerateFullSceneFolderPath(folderName) + "/" + sceneName + ".unity";
        }

        public string GenerateFullSceneFolderPath(string folderName)
        {
            return ConfigMission.Instance.Path_To_Folder_World + "/" + folderName;
        }

        private SceneSlot m_currentMissionScene;
        private MissionEnum m_currentMission = MissionEnum.None;
        private MissionEnum[] ALL_MISSION = {
        MissionEnum.None,
        MissionEnum.DockByPier,
        MissionEnum.EnterIsland};

    }
}