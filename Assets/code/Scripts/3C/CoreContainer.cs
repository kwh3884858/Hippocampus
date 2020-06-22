using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Base;
using StarPlatinum.Manager;
using UnityEngine.SceneManagement;
using Config.GameRoot;
using StarPlatinum.Service;
using UnityEngine.Assertions;

namespace GamePlay.Stage
{
    public class CoreContainer : Singleton<CoreContainer>
    {
        public CoreContainer()
        {
            m_player = null;

            Transform root = GameObject.Find("GameRoot").transform;
            Assert.IsTrue(root != null, "Game Root must always exist.");
            if (root == null) return;
            GameObject manager = new GameObject(typeof(CoreContainer).ToString());
            manager.transform.SetParent(root.transform);
            m_containerScene = manager.AddComponent<SceneSlot>();
        }

        public void Initialize()
        {
            m_containerScene.LoadScene(SceneLookupEnum.World_CoreContainer);
        }

        //After scene loaded, it will call this function
        public void SetSceneLoaded(
            GameObject player,
            GameObject camera,
            bool loaded)
        {
            m_player = player;
            m_isSceneLoaded = loaded;

            m_player.SetActive(false);
            CameraService.Instance.SetMainCamera(camera);
        }

        public void SpawnPlayer()
        {
            SceneLookupEnum sceneEnum = MissionSceneManager.Instance.GetCurrentMissionScene();
            Scene missionScene = SceneManager.GetSceneByName(sceneEnum.ToString());

            GameObject[] gameObjects = missionScene.GetRootGameObjects();
            foreach (GameObject go in gameObjects)
            {
                if (go.name == ConfigMission.Instance.Text_Spawn_Point_Name)
                {
                    if (m_player != null)
                    {
                        m_player.transform.position =
                            new Vector3(go.transform.position.x, go.transform.position.y + 0.5f, go.transform.position.z);
                        m_player.SetActive(true);
                    }
                }
            }
        }

        private bool IsValid()
        {
            return m_isSceneLoaded == true;
        }

        SceneSlot m_containerScene;

        GameObject m_player;
        bool m_isSceneLoaded = false;
    }
}