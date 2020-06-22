using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StarPlatinum.Manager
{
    public class SceneSlot : MonoBehaviour
    {
        public delegate void SceneLoadedCallback();
        public bool LoadScene(SceneLookupEnum requestSceneEnum)
        {
            if (SceneLookup.IsSceneExist(requestSceneEnum))
            {
                if (m_currentSceneEnum != SceneLookupEnum.World_GameRoot)
                {
                    PrefabManager.Instance.UnloadScene(m_currentSceneEnum);
                }
                SetCurrentSceneEnum(requestSceneEnum);
                PrefabManager.Instance.LoadScene(requestSceneEnum, LoadSceneMode.Additive);
                StartCoroutine(CheckSceneIsLoaded());
                return true;
            }
            else
            {
                return false;
            }
        }

        public SceneLookupEnum GetCurrentSceneEnum()
        {
            return m_currentSceneEnum;
        }
        public SceneLookupEnum GetLastSceneEnum()
        {
            return m_lastSceneEnum;
        }
        public void SetCurrentSceneEnum(SceneLookupEnum sceneEnum)
        {
            m_lastSceneEnum = m_currentSceneEnum;
            m_currentSceneEnum = sceneEnum;
        }
        public void AddCallbackAfterLoaded(SceneLoadedCallback newCallback)
        {
            m_callback += newCallback;
        }

        private IEnumerator CheckSceneIsLoaded()
        {
            while (!PrefabManager.Instance.IsSceneLoaded(m_currentSceneEnum))
            {
                yield return null;
            }

            m_callback?.Invoke();
        }
        private SceneLookupEnum m_currentSceneEnum = SceneLookupEnum.World_GameRoot;
        private SceneLookupEnum m_lastSceneEnum = SceneLookupEnum.World_GameRoot;
        private SceneLoadedCallback m_callback = null;
    }
}