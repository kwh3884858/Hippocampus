using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace StarPlatinum
{
    public class EditorCreateUIScript : EditorWindow
    {
        static readonly string TEMPLATEPATH = "Assets/StarPlatinum/Editor/Template/TemplateUIPanelScript.cs.txt";
        static  string prefabPathPrefix = "Assets/UI/Panels/GameScene/";
        static  string scriptPathPrefix = "Assets/Scripts/UI/Panels/GameScene/";

        
        [MenuItem("GameObject/CreateUIPanelScript %&m", false, -10)]
        static void CreateUIPanelScript(MenuCommand menuCommand)
        {
            GameObject context = menuCommand.context as GameObject;
             string monogameobjectName;
             string namespaceName;
            if (context == null) return;
            string name = context.name;

            int namespaceLength = name.LastIndexOf("_");
            if (namespaceLength == name.Length || namespaceLength < 0) return;

            namespaceName = name.Substring(0, namespaceLength );
            monogameobjectName = name.Substring(namespaceLength + 1);

            //Create Script
            string perfabDir = prefabPathPrefix + namespaceName;
            string scriptDir = scriptPathPrefix + namespaceName;

            string scriptPath = scriptDir + "/" + monogameobjectName + ".cs";

            if (!Directory.Exists(perfabDir))
                Directory.CreateDirectory(perfabDir);

            if (!Directory.Exists(scriptDir))
                Directory.CreateDirectory(scriptDir);

            File.Copy(TEMPLATEPATH, scriptPath, true);

            int index = Application.dataPath.LastIndexOf("Assets");
            string scriptFullPath = Application.dataPath.Substring(0, index) + scriptPath;


            string fileContent = System.IO.File.ReadAllText(scriptFullPath);

            fileContent = fileContent.Replace("#NAMESPACE#", namespaceName);
            fileContent = fileContent.Replace("#CLASSNAME#", monogameobjectName);

            System.IO.File.WriteAllText(scriptFullPath, fileContent);
            AssetDatabase.Refresh();

            //Create Prefab, Add Component to Game Object
            string prefabPath = perfabDir + "/" + name + ".prefab";

            // Make sure the file name is unique, in case an existing Prefab has the same name.
            prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabPath);

            GameObject savedGameobject = PrefabUtility.SaveAsPrefabAssetAndConnect(context, prefabPath, InteractionMode.UserAction);
            ProjectWindowUtil.ShowCreatedAsset(savedGameobject);

        }

        [MenuItem("GameObject/CreateUIPanelScript %&m", true)]
        static bool ValidateCreatePrefab()
        {
            return Selection.activeGameObject != null && !EditorUtility.IsPersistent(Selection.activeGameObject);
        }

        [MenuItem("Assets/Custom Create UI Script Editor", false, 800)]
        public static void ShowWindow()
        {
            
            GetWindow<EditorCreateUIScript>("Create UI Script Editor").minSize = new Vector2(500, 200);
        }

        void OnGUI()
        {

            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(450));
            EditorGUILayout.LabelField("Path Editor", EditorStyles.boldLabel);
            prefabPathPrefix = EditorGUILayout.TextField("Prefab Path:", prefabPathPrefix);
            scriptPathPrefix = EditorGUILayout.TextField("Script Path", scriptPathPrefix);
            EditorGUILayout.EndVertical();

        }
            //void OnGUI()
            //{
            //    GUILayout.Space(10);
            //    EditorGUILayout.LabelField("Input Script Name And NameSpace.", EditorStyles.wordWrappedLabel);
            //    GUILayout.Space(10);
            //    m_monogameobjectName = EditorGUILayout.TextField("Gameobject and Script Name: ", m_monogameobjectName);
            //    GUILayout.Space(10);
            //    m_namespaceName = EditorGUILayout.TextField("Namespace Name: ", m_namespaceName);

            //    GUILayout.Space(60);
            //    EditorGUILayout.BeginHorizontal();
            //    if (GUILayout.Button("Create"))
            //    {
            //        if (m_monogameobjectName.Length == 0 ||
            //            m_namespaceName.Length == 0)
            //        {
            //            Close();
            //        }
            //        string folder = "Assets/UI/Panels/GameScene/" + m_namespaceName;
            //        string filePath = folder + "/" + m_monogameobjectName + ".cs";

            //        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            //        if (!File.Exists(filePath)) File.Create(filePath);

            //        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
            //           ScriptableObject.CreateInstance<CreateAssetAction>(),
            //           "NewInterface.cs",
            //           EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
            //           TEMPLATEPATH
            //          );

            //        GameObject go = CreateCustomUIPanel(m_monogameobjectName);
            //        ReplaceContent(filePath);
            //        GameObjectUtility.SetParentAndAlign(go, m_context);
            //        Close();
            //    }
            //    if (GUILayout.Button("Cancel")) Close();
            //    EditorGUILayout.EndHorizontal();
            //}

            //public static string GetSelectedPathOrFallback()
            //{
            //    string path = "Assets";
            //    foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            //    {
            //        path = AssetDatabase.GetAssetPath(obj);
            //        if (!string.IsNullOrEmpty(path) && File.Exists(path))
            //        {
            //            path = Path.GetDirectoryName(path);
            //            break;
            //        }
            //    }
            //    return path;
            //}

            //static GameObject CreateCustomUIPanel(string goName)
            //{
            //    GameObject go = new GameObject(goName);
            //    // Ensure it gets reparented if this was a context click (otherwise does nothing)

            //    // Register the creation in the undo system
            //    Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

            //    Selection.activeObject = go;

            //    return go;
            //}

            //void ReplaceContent(string fullPath)
            //{
            //    StreamReader reader = new StreamReader(fullPath);
            //    string content = reader.ReadToEnd();
            //    reader.Close();

            //    //替换默认的文件名
            //    content = content.Replace("#NAMESPACENAME#", m_namespaceName);

            //    StreamWriter writer = new StreamWriter(fullPath, false, System.Text.Encoding.UTF8);
            //    writer.Write(content);
            //    writer.Close();

            //    AssetDatabase.Refresh();
            //}

        }
    }