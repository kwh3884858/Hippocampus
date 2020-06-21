using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StarPlatinum.Manager
{
    public class SceneSlot : MonoBehaviour
    {
        public bool LoadScene(SceneLookupEnum requestSceneEnum)
        {
            if (SceneLookup.IsSceneExist(requestSceneEnum))
            {
                if (m_currentMissionSceneEnum != SceneLookupEnum.World_GameRoot)
                {
                    PrefabManager.Instance.UnloadScene(m_currentMissionSceneEnum);
                }
                SetCurrentSceneEnum(requestSceneEnum);
                PrefabManager.Instance.LoadScene(requestSceneEnum, LoadSceneMode.Additive);
                return true;
            }
            else
            {
                return false;
            }
        }
        public SceneLookupEnum GetCurrentSceneEnum()
        {
            return m_currentMissionSceneEnum;
        }
        public void SetCurrentSceneEnum(SceneLookupEnum sceneEnum)
        {
            m_currentMissionSceneEnum = sceneEnum;
        }

        private SceneLookupEnum m_currentMissionSceneEnum = SceneLookupEnum.World_GameRoot;

    }
}