using StarPlatinum;
using StarPlatinum.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;

namespace GamePlay.Utility
{
    public class WorldDebug3DTextManager : Singleton<WorldDebug3DTextManager>
    {

        public WorldDebug3DTextManager()
        {
            Transform root = GameObject.Find("GameRoot").transform;
            Assert.IsTrue(root != null, "Game Root must always exist.");
            if (root == null) return;

            GameObject manager = new GameObject(typeof(WorldDebug3DTextManager).ToString());
            manager.transform.SetParent(root.transform);

            m_needText = new List<GameObject>();
        }

        public void AddTextToGameobject(string text, GameObject go)
        {
            PrefabManager.Instance.InstantiateAsync<WorldDebug3DText>(prefabName, (result) =>
            {
                WorldDebug3DText worldDebug = result.result as WorldDebug3DText;
                worldDebug.gameObject.name = go.name + " : 3D Text";
                //Move up
                worldDebug.transform.position += new Vector3(0,5,0); 
                worldDebug.SetText(text);
                m_needText.Add(worldDebug.gameObject);
            }, go.transform);
        }

        private string prefabName = "graphic_debug_3D_Text";
        public bool DISPALY_TEXT = true;
        private List<GameObject> m_needText;
    }
}