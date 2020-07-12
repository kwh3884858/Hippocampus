using UnityEditor;
using UnityEngine;

namespace UI
{
    public class CGSceneEditor:Editor
    {
        [MenuItem("Tools/CG场景编辑")]
        public static void EditorCGScene()
        {
            var uimanager = GetPrefabs(uimanagerPath);
            var cgScenePanel = GetPrefabs(CGScenePanelPath,uimanager.transform);
            
        }

        public static GameObject GetPrefabs(string path,Transform parent = null)
        {
            GameObject obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
            var newPrefab = PrefabUtility.InstantiatePrefab(obj,parent) as GameObject;
            return newPrefab;
        }
        
        private static string uimanagerPath = "Assets/data/graphics/UI/Root/Prefabs/UI_Root_UI_Manager.prefab";
        private static string CGScenePanelPath ="Assets/data/graphics/UI/Common/CGScene/Prefabs/UI_Common_CGScene_Panel.prefab";
    }
}