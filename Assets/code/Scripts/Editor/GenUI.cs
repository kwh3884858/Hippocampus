using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UI;
using UI.Panels;
using UnityEngine.Serialization;

[Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;
 
    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() { return target; }
 
    public Serialization(Dictionary<TKey, TValue> target)
    {
        this.target = target;
    }
 
    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }
 
    public void OnAfterDeserialize()
    {
        var count = Math.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i)
        {
            target.Add(keys[i], values[i]);
        }
    }
}

[Serializable]
public class GenInfo
{
    /// <summary>
    /// 0->mediator,1->view,2->singleView,3->model,
    /// </summary>
    public int type;
    public string prefabPath;
    public string codePath;
    public string mediatorPath;
    public string modelPath;
    public string GUID;

    public GenInfo(int type,string prefabPath,string codePath,string mediatorPath = "",string modelPath = "")
    {
        this.type = type;
        this.prefabPath = prefabPath;
        this.codePath = codePath;
        this.mediatorPath = mediatorPath;
        this.modelPath = modelPath;
    }

    public GenInfo(){}
}

public class GenMVC : ScriptableObject
{

    private static string m_s_tempRoot = "Assets/code/Scripts/UI/Editor/";
    private static string m_s_GenCodeDir = Application.dataPath + "/code/Scripts/UI/";
    private static string m_s_CommonViewCodeDir = Application.dataPath + "/code/Scripts/UI/Element/View";
    private static string m_s_CommonLogicCodeDir = Application.dataPath + "/code/Scripts/UI/Element/Logic";
    private static string m_s_ViewCodeDir = Application.dataPath + "/code/Scripts/UI/Panels/AutoCreate/";

    private static string m_s_bindInfo = string.Empty;
    private static Dictionary<string, string> m_subViewDic = new Dictionary<string, string>();

    public static void CreatePrCSByObj(GameObject obj,string outSavePath = "",bool isIL = false)
    {
        m_subViewDic.Clear();
        GameObject prefab = obj;

        string allPath = AssetDatabase.GetAssetPath(obj);
        string[] folders = allPath.Split('/');

        UIPanel vb = prefab.GetComponent<UIPanel>();
        if (vb == null)
        {
            vb = prefab.AddComponent<UIPanel>();
        }
        string bindInfo = folders[folders.Length - 2];

        StringBuilder members = new StringBuilder();
        StringBuilder finds = new StringBuilder();

        Dictionary<string, bool> nameDic = new Dictionary<string, bool>();

        foreach (RectTransform trans in prefab.GetComponentsInChildren<RectTransform>(true))
        {
            var prefabRef = PrefabUtility.GetNearestPrefabInstanceRoot(trans.gameObject);
            var prefabRefPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(trans.gameObject);
            // 是预制件内的预制件
            bool bIsPrefab = false;
            if(prefabRef != obj && prefabRef != null)
            {
                if(!m_subViewDic.ContainsKey(prefabRefPath))
                {
                    var subViewName = Path.GetFileNameWithoutExtension(prefabRefPath);
                    m_subViewDic.Add(prefabRefPath, subViewName);

                    var subViewObjc = AssetDatabase.LoadAssetAtPath<GameObject>(prefabRefPath);
                    //var subViewObjc = PrefabUtility.LoadPrefabContents(prefabRefPath);
                    CreateSubViewCSByObj(subViewObjc, m_s_CommonViewCodeDir);
                }
                var prefabRef2 = PrefabUtility.GetNearestPrefabInstanceRoot(trans.parent.gameObject);
                if (prefabRef2 == obj || prefabRef2 == null)
                {
                    // 首个预制件需要生成 View 成员变量
                    bIsPrefab = true;
                }
                else
                {
                    // 其他预制件内的所有东西都不生成， 因为会单独未这些预制件生成绑定代码
                    continue;
                }
            }
            if (bIsPrefab || (trans.name.IndexOf("_")>-1 && trans.parent))
            {
                Transform pr = trans;
                string path = pr.name;
                while(pr && pr.parent && pr.parent.parent)
                {
                    pr = pr.parent;
                    path = pr.name + '/'+path;
                }
                Behaviour[] coms = trans.gameObject.GetComponents<Behaviour>();
                string classType = "";

                bool isAppendLine = false;
                if(bIsPrefab)
                {
                    classType = Path.GetFileNameWithoutExtension(prefabRefPath);
                    string member_name = trans.name;
                    bool isTrue = false;
                    if (Regex.IsMatch(member_name, @"^[A-Za-z0-9_]*$"))
                    {
                        isTrue = true;
                    }
                    else
                    {
                        Debug.Log("非法字符：" + member_name);
                    }
                    if (!nameDic.ContainsKey(member_name) && isTrue)
                    {
                        nameDic.Add(member_name, true);
                        string member = string.Format("\t\t[HideInInspector] public {0}_SubView m_{1};", classType, member_name);
                        string find = string.Format("\t\t\tm_{0}.Init(FindUI<RectTransform>(transform ,\"{2}\"));", member_name, classType, path);
                        members.AppendLine(member);
                        finds.AppendLine(find);
                    }
                    else
                    {
                        Debug.Log("异常名称：" + member_name);
                    }
                }
                else if(coms.Length>0){
                    for (int i = 0; i < coms.Length; i++)
                    {
                        classType = coms[i].GetType().ToString();
                        var simpleName = classType.Substring(classType.LastIndexOf(".")+1);

                        string member_name = trans.name + "_" + simpleName;

                        bool isTrue = false;
                        if (Regex.IsMatch(member_name, @"^[A-Za-z0-9_]*$"))
                        {
                            isTrue = true;
                        }
                        else
                        {
                            Debug.Log("非法字符："+ member_name);
                        }

                        if (!nameDic.ContainsKey(member_name) && isTrue)
                        {
                            isAppendLine = true;
                            nameDic.Add(member_name, true);
                            string member = string.Format("\t\t[HideInInspector] public {0} m_{1};", simpleName, member_name);
                            string find = string.Format("\t\t\tm_{0} = FindUI<{1}>(transform ,\"{2}\");", member_name, simpleName, path);
                            members.AppendLine(member);
                            finds.AppendLine(find);
                        }
                        else
                        {
                            Debug.Log("异常名称："+ member_name);
                        }
                    }
                    if (isAppendLine)
                    {
                        members.AppendLine();
                        finds.AppendLine();
                    }
                }
                else
                {
                    classType = "RectTransform";
                    string member_name = trans.name;
                    bool isTrue = false;
                    if (Regex.IsMatch(member_name, @"^[A-Za-z0-9_]*$"))
                    {
                        isTrue = true;
                    }
                    else
                    {
                        Debug.Log("非法字符：" + member_name);
                    }
                    if (!nameDic.ContainsKey(member_name) && isTrue)
                    {
                        nameDic.Add(member_name, true);
                        string member = string.Format("\t\t[HideInInspector] public {0} m_{1};", classType, member_name);
                        string find = string.Format("\t\t\tm_{0} = FindUI<{1}>(transform ,\"{2}\");", member_name, classType, path);
                        members.AppendLine(member);
                        finds.AppendLine(find);
                    }
                    else
                    {
                        Debug.Log("异常名称：" + member_name);
                    }
                }
            }
        }

        string csName = prefab.name;//.Substring(3);
        csName = csName.Replace("_", "");
        csName = csName.Replace("UI", "");
        csName += ".cs";
        string savepath =  string.IsNullOrEmpty(outSavePath) ? EditorUtility.SaveFilePanel(
            "Save "+prefab.name+".cs",
            m_s_GenCodeDir + "Panels/",
            csName,
            "cs") : outSavePath;
        if(savepath!=null && savepath.Length>0)
        {
            GenInfo info = CreateTemplate(obj,m_s_ViewCodeDir + csName, m_s_tempRoot + "ViewTemplate.txt", members.ToString(), finds.ToString(),bindInfo);
            UIPostProcessor.AddType(info.prefabPath);
            info.codePath = savepath.Substring(Application.dataPath.Replace("Assets", "").Length);

            string mdPath = savepath;
            if (File.Exists(mdPath) == false)
            {
                CreateTemplate(obj,mdPath, m_s_tempRoot + "MediatorTemplate.txt", members.ToString(), finds.ToString(),bindInfo);
                AssetDatabase.ImportAsset(mdPath);
                mdPath = savepath.Replace("Panel.cs", "Model.cs");
                CreateTemplate(obj,mdPath, m_s_tempRoot + "ModelTemplate.txt", members.ToString(), finds.ToString(),bindInfo);
                AssetDatabase.ImportAsset(mdPath);
            }
            else
            {
                //修复关系
                mdPath = mdPath.Substring(Application.dataPath.Replace("Assets", "").Length);
                info.mediatorPath = mdPath;
                mdPath = savepath.Replace("Panel.cs", "Model.cs");
                mdPath = mdPath.Substring(Application.dataPath.Replace("Assets", "").Length);
                info.modelPath = mdPath;
                SaveGenInfo();
            }
        }        
    }

    [MenuItem("Assets/GenUI/Gen Prefab Code", false, 78)]
    public static void CreatePrCS()
    {
        AssetDatabase.StartAssetEditing();
        GameObject prefab = Selection.activeGameObject as GameObject;
        GenInfo info = getPrefabInfo(AssetDatabase.GetAssetPath(prefab));
        string savePath = "";
        if (!string.IsNullOrEmpty(info.codePath))
        {
            savePath = Application.dataPath.Replace("Assets", "") + info.codePath;
        }
        CreatePrCSByObj(prefab,savePath);
        AssetDatabase.StopAssetEditing();
        AssetDatabase.Refresh();
    }


    [MenuItem("Assets/GenUI/Gen SubView Code", false, 78)]
    public static void CreateSubViewCS()
    {
        GameObject prefab = Selection.activeGameObject as GameObject;
        CreateSubViewCSByObj(prefab, m_s_CommonViewCodeDir);
    }

    public static void CreateSubViewCSByObj(GameObject obj, string outSavePath)
    {
        GameObject prefab = obj;
        if(PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Variant)
        {
            var parentObj = PrefabUtility.GetCorrespondingObjectFromSource(obj);
            var prefabRefPath = AssetDatabase.GetAssetPath(parentObj);
            if (!m_subViewDic.ContainsKey(prefabRefPath))
            {
                var subViewName = Path.GetFileNameWithoutExtension(prefabRefPath);
                m_subViewDic.Add(prefabRefPath, subViewName);
                var subViewObjc = AssetDatabase.LoadAssetAtPath<GameObject>(prefabRefPath);
                //var subViewObjc = PrefabUtility.LoadPrefabContents(prefabRefPath);
                CreateSubViewCSByObj(subViewObjc, m_s_CommonViewCodeDir);
            }
        }

        StringBuilder members = new StringBuilder();
        StringBuilder finds = new StringBuilder();

        Dictionary<string, bool> nameDic = new Dictionary<string, bool>();

        foreach (RectTransform trans in prefab.GetComponentsInChildren<RectTransform>(true))
        {
            var prefabRef = PrefabUtility.GetNearestPrefabInstanceRoot(trans.gameObject);
            var prefabRefPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(trans.gameObject);
            // 是预制件内的预制件
            bool bIsPrefab = false;
            if (prefabRef != obj && prefabRef != null)
            {
                if (!m_subViewDic.ContainsKey(prefabRefPath))
                {
                    var subViewName = Path.GetFileNameWithoutExtension(prefabRefPath);
                    m_subViewDic.Add(prefabRefPath, subViewName);
                    var subViewObjc = AssetDatabase.LoadAssetAtPath<GameObject>(prefabRefPath);
                        //PrefabUtility.LoadPrefabContents(prefabRefPath);
                    CreateSubViewCSByObj(subViewObjc, m_s_CommonViewCodeDir);
                }
                if (trans.parent == null)
                    continue;
                var prefabRef2 = PrefabUtility.GetNearestPrefabInstanceRoot(trans.parent.gameObject);
                if (prefabRef2 == obj || prefabRef2 == null)
                {
                    // 首个预制件需要生成 View 成员变量
                    bIsPrefab = true;
                }
                else
                {
                    // 其他预制件内的所有东西都不生成， 因为会单独未这些预制件生成绑定代码
                    continue;
                }
            }
            // 根节点也要生成控件脚本
            if (bIsPrefab || (trans.name.IndexOf("_") > -1/* && trans.parent*/))
            {
                Transform pr = trans;
                string path = pr.name;
                while (pr && pr.parent && pr.parent.parent)
                {
                    pr = pr.parent;
                    path = pr.name + '/' + path;
                }

                //Debug.Log(path);

                Behaviour[] coms = trans.gameObject.GetComponents<Behaviour>();
                string classType = "";

                bool isAppendLine = false;
                if (bIsPrefab)
                {
                    classType = Path.GetFileNameWithoutExtension(prefabRefPath);
                    string member_name = trans.name;
                    bool isTrue = false;
                    if (Regex.IsMatch(member_name, @"^[A-Za-z0-9_]*$"))
                    {
                        isTrue = true;
                    }
                    else
                    {
                        Debug.Log("非法字符：" + member_name);
                    }
                    if (!nameDic.ContainsKey(member_name) && isTrue)
                    {
                        nameDic.Add(member_name, true);
                        string member = string.Format("\t\t[HideInInspector] public {0}_SubView m_{1};", classType, member_name);
                        string find = string.Format("\t\t\tm_{0}.Init(FindUI<RectTransform>(gameObject.transform ,\"{2}\"));", member_name, classType, path);
                        members.AppendLine(member);
                        finds.AppendLine(find);
                    }
                    else
                    {
                        Debug.Log("异常名称：" + member_name);
                    }
                }
                else if (coms.Length > 0)
                {
                    for (int i = 0; i < coms.Length; i++)
                    {
                        classType = coms[i].GetType().ToString();
                        var simpleName = classType.Substring(classType.LastIndexOf(".") + 1);

                        string member_name = trans.name + "_" + simpleName;

                        bool isTrue = false;
                        if (Regex.IsMatch(member_name, @"^[A-Za-z0-9_]*$"))
                        {
                            isTrue = true;
                        }
                        else
                        {
                            Debug.Log("非法字符：" + member_name);
                        }

                        if (!nameDic.ContainsKey(member_name) && isTrue)
                        {
                            isAppendLine = true;
                            nameDic.Add(member_name, true);
                            string member = string.Format("\t\t[HideInInspector] public {0} m_{1};", simpleName, member_name);
                            string find = string.Format("\t\t\tm_{0} = FindUI<{1}>(gameObject.transform ,\"{2}\");", member_name, simpleName, path);
                            if (trans.parent == null)
                            {
                                find = string.Format("\t\t\tm_{0} = gameObject.GetComponent<{1}>();", member_name, simpleName);
                            }
                            members.AppendLine(member);
                            finds.AppendLine(find);
                        }
                        else
                        {
                            Debug.Log("控件名称重复：" + member_name+" path :"+path);
                        }
                    }
                    if (isAppendLine)
                    {
                        members.AppendLine();
                        finds.AppendLine();
                    }
                }
                else
                {
                    classType = "RectTransform";
                    string member_name = trans.name;
                    bool isTrue = false;
                    if (Regex.IsMatch(member_name, @"^[A-Za-z0-9_]*$"))
                    {
                        isTrue = true;
                    }
                    else
                    {
                        Debug.Log("非法字符：" + member_name);
                    }
                    if (!nameDic.ContainsKey(member_name) && isTrue)
                    {
                        nameDic.Add(member_name, true);
                        string member = string.Format("\t\t[HideInInspector] public {0} m_{1};", classType, member_name);
                        string find = string.Format("\t\t\tm_{0} = FindUI<{1}>(gameObject.transform ,\"{2}\");", member_name, classType, path);
                        if (trans.parent == null)
                        {
                            find = string.Format("\t\t\tm_{0} = gameObject.GetComponent<{1}>();", member_name, classType);
                        }
                        members.AppendLine(member);
                        finds.AppendLine(find);
                    }
                    else
                    {
                        Debug.Log("异常名称：" + member_name);
                    }
                }

            }
        }

        string csName = prefab.name;//.Substring(3);

        var prefabType = PrefabUtility.GetPrefabAssetType(obj);
        string savepath = Path.Combine(outSavePath, csName + "_SubView.cs");
        if (savepath != null && savepath.Length > 0)
        {
            if (!Directory.Exists(outSavePath))
            {
                Directory.CreateDirectory(outSavePath);
            }
            if(prefabType == PrefabAssetType.Variant)
            {
                CreateTemplate(obj, savepath, m_s_tempRoot + "SubViewVariant.txt", members.ToString(), finds.ToString(), "");
            }
            else
            {
                CreateTemplate(obj, savepath, m_s_tempRoot + "SubView.txt", members.ToString(), finds.ToString(), "");
            }
        }
        string savepath2 = Path.Combine(m_s_CommonLogicCodeDir, csName + "_SubView.cs");
        if (File.Exists(savepath2) == false)
        {
            if(!Directory.Exists(m_s_CommonLogicCodeDir))
            {
                Directory.CreateDirectory(m_s_CommonLogicCodeDir);
            }

            if (prefabType == PrefabAssetType.Variant)
            {
                CreateTemplate(obj, savepath2, m_s_tempRoot + "SubViewVariantLogic.txt", members.ToString(), finds.ToString(), "");
            }
            else
            {
                CreateTemplate(obj, savepath2, m_s_tempRoot + "SubViewLogic.txt", members.ToString(), finds.ToString(), "");
            }
        }
    }

    public static void CreateSingleViewCSByObj(GameObject obj,string outSavePath = "")
    {
        m_subViewDic.Clear();
        GameObject prefab= obj;
        UIPanel vb = prefab.GetComponent<UIPanel>();
        if (vb == null)
        {
            vb = prefab.AddComponent<UIPanel>();
        }        
        
        string allPath = AssetDatabase.GetAssetPath(obj);
        string[] folders = allPath.Split('/');
        string bindInfos = folders[folders.Length - 2];

        StringBuilder members = new StringBuilder();
        StringBuilder finds = new StringBuilder();

        Dictionary<string, bool> nameDic = new Dictionary<string, bool>();

        foreach (RectTransform trans in prefab.GetComponentsInChildren<RectTransform>(true))
        {
            var prefabRef = PrefabUtility.GetNearestPrefabInstanceRoot(trans.gameObject);
            var prefabRefPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(trans.gameObject);
            // 是预制件内的预制件
            bool bIsPrefab = false;
            if (prefabRef != obj && prefabRef != null)
            {
                if (!m_subViewDic.ContainsKey(prefabRefPath))
                {
                    var subViewName = Path.GetFileNameWithoutExtension(prefabRefPath);
                    m_subViewDic.Add(prefabRefPath, subViewName);

                    var subViewObjc = AssetDatabase.LoadAssetAtPath<GameObject>(prefabRefPath);
                    //var subViewObjc = PrefabUtility.LoadPrefabContents(prefabRefPath);
                    CreateSubViewCSByObj(subViewObjc, m_s_CommonViewCodeDir);
                }
                var prefabRef2 = PrefabUtility.GetNearestPrefabInstanceRoot(trans.parent.gameObject);
                if (prefabRef2 == obj || prefabRef2 == null)
                {
                    // 首个预制件需要生成 View 成员变量
                    bIsPrefab = true;
                }
                else
                {
                    // 其他预制件内的所有东西都不生成， 因为会单独未这些预制件生成绑定代码
                    continue;
                }
            }
            if (bIsPrefab || (trans.name.IndexOf("_") > -1 && trans.parent))
            {
                Transform pr = trans;
                string path = pr.name;
                while (pr && pr.parent && pr.parent.parent)
                {
                    pr = pr.parent;
                    path = pr.name + '/' + path;
                }

                //Debug.Log(path);

                Behaviour[] coms = trans.gameObject.GetComponents<Behaviour>();
                string classType = "";

                bool isAppendLine = false;
                if (bIsPrefab)
                {
                    classType = Path.GetFileNameWithoutExtension(prefabRefPath);
                    string member_name = trans.name;
                    bool isTrue = false;
                    if (Regex.IsMatch(member_name, @"^[A-Za-z0-9_]*$"))
                    {
                        isTrue = true;
                    }
                    else
                    {
                        Debug.Log("非法字符：" + member_name);
                    }
                    if (!nameDic.ContainsKey(member_name) && isTrue)
                    {
                        nameDic.Add(member_name, true);
                        string member = string.Format("\t\t[HideInInspector] public {0}_SubView m_{1};", classType, member_name);
                        string find = string.Format("\t\t\tm_{0}.Init(FindUI<RectTransform>(transform ,\"{2}\"));", member_name, classType, path);
                        members.AppendLine(member);
                        finds.AppendLine(find);
                    }
                    else
                    {
                        Debug.Log("异常名称：" + member_name);
                    }
                }
                else if (coms.Length > 0)
                {
                    for (int i = 0; i < coms.Length; i++)
                    {
                        classType = coms[i].GetType().ToString();
                        var simpleName = classType.Substring(classType.LastIndexOf(".") + 1);

                        string member_name = trans.name + "_" + simpleName;

                        bool isTrue = false;
                        if (Regex.IsMatch(member_name, @"^[A-Za-z0-9_]*$"))
                        {
                            isTrue = true;
                        }
                        else
                        {
                            Debug.Log("非法字符：" + member_name);
                        }

                        if (!nameDic.ContainsKey(member_name) && isTrue)
                        {
                            isAppendLine = true;
                            nameDic.Add(member_name, true);
                            string member = string.Format("\t\t[HideInInspector] public {0} m_{1};", simpleName, member_name);
                            string find = string.Format("\t\t\tm_{0} = FindUI<{1}>(transform ,\"{2}\");", member_name, simpleName, path);
                            members.AppendLine(member);
                            finds.AppendLine(find);
                        }
                        else
                        {
                            Debug.Log("异常名称：" + member_name);
                        }
                    }
                    if (isAppendLine)
                    {
                        members.AppendLine();
                        finds.AppendLine();
                    }
                }
                else
                {
                    classType = "RectTransform";
                    string member_name = trans.name;
                    bool isTrue = false;
                    if (Regex.IsMatch(member_name, @"^[A-Za-z0-9_]*$"))
                    {
                        isTrue = true;
                    }
                    else
                    {
                        Debug.Log("非法字符：" + member_name);
                    }
                    if (!nameDic.ContainsKey(member_name) && isTrue)
                    {
                        nameDic.Add(member_name, true);
                        string member = string.Format("\t\t[HideInInspector] public {0} m_{1};", classType, member_name);
                        string find = string.Format("\t\t\tm_{0} = FindUI<{1}>(transform ,\"{2}\");", member_name, classType, path);
                        members.AppendLine(member);
                        finds.AppendLine(find);
                    }
                    else
                    {
                        Debug.Log("异常名称：" + member_name);
                    }
                }

            }
        }

        string csName = prefab.name;//.Substring(3);

        string savepath = string.IsNullOrEmpty(outSavePath) ? EditorUtility.SaveFilePanel(
            "Save " + prefab.name + ".cs",
            m_s_GenCodeDir  + "MVC/View_Mediator/",
            csName + "View.cs",
            "cs") : outSavePath;
        if (savepath != null && savepath.Length > 0)
        {
            CreateTemplate(obj,savepath, m_s_tempRoot + "SingleView.txt", members.ToString(), finds.ToString(),bindInfos);
        }
    }

    [MenuItem("Assets/GenUI/Gen Prefab Code Single View", false, 79)]
    public static void CreateSingleViewCS()
    {
        GameObject prefab = Selection.activeGameObject as GameObject;
        GenInfo info = getPrefabInfo(AssetDatabase.GetAssetPath(prefab));
        string savePath = "";
        if (!string.IsNullOrEmpty(info.codePath))
        {
            savePath = Application.dataPath.Replace("Assets", "") + info.codePath;
        }
        CreateSingleViewCSByObj(prefab, savePath);
    }

    private static Dictionary<string, GenInfo> s_genInfoDic;

    private static Dictionary<string, GenInfo>  getGenInfos(){

        s_genInfoDic = null;
        if (s_genInfoDic == null)
        {
            s_genInfoDic = new Dictionary<string, GenInfo>();
            string infoPath = Application.dataPath.Replace("Assets", "") + "genInfo.json";
            if (File.Exists(infoPath))
            {
                s_genInfoDic = JsonConvert.DeserializeObject<Dictionary<string, GenInfo>>(File.ReadAllText(infoPath));
            }
        }

        return s_genInfoDic;
    }

    private static void SaveGenInfo(){
        string infoPath = Application.dataPath.Replace("Assets", "") + "genInfo.json";

        if (s_genInfoDic!=null && s_genInfoDic.Count>0)
        {           
            File.WriteAllText(infoPath,JsonTree(JsonConvert.SerializeObject(s_genInfoDic)));
        }
    }
    
    public static string JsonTree(string json)
    {
        int level = 0;
        var jsonArr = json.ToArray(); 
        string jsonTree = string.Empty;
        for (int i = 0; i < json.Length; i++)
        {
            char c = jsonArr[i];
            if (level > 0 && '\n' == jsonTree.ToArray()[jsonTree.Length - 1])
            {
                jsonTree += TreeLevel(level);
            }
            switch (c)
            {
                case '{':
                    jsonTree += c + "\n";
                    level++;

                    break;
                case ',':
                    jsonTree += c + "\n";
                    break;
                case '}':
                    jsonTree += "\n";
                    level--;
                    jsonTree += TreeLevel(level);
                    jsonTree += c;
                    break;
                default:
                    jsonTree += c;
                    break;
            }
        }
        return jsonTree;
    }

    private static string TreeLevel(int level)
    {
        string leaf = string.Empty;
        for (int t = 0; t < level; t++)
        {
            leaf += "\t";
        }
        return leaf;
    }

    public static GenInfo getPrefabInfo(string prefabPath){
        Dictionary<string, GenInfo> genInfoDic = getGenInfos();
        string guid = AssetDatabase.AssetPathToGUID(prefabPath);
        if (genInfoDic.ContainsKey(guid))
        {
            return genInfoDic[guid];
        }
        
        var info = new GenInfo(0, prefabPath, "");
        info.GUID = guid;
        genInfoDic[guid] = info;
       
        return info;
    }

    private static GenInfo CreateTemplate(GameObject obj,string createNewFilePath, string tempFilePath, string member, string memberfind, string binderInfo)
    {
        if (string.IsNullOrEmpty(createNewFilePath))
        {
            return null;
        }
        //获取要创建资源的绝对路径
        string fullPath = Path.GetFullPath(createNewFilePath);
        //读取本地的模板文件
        StreamReader streamReader = new StreamReader(tempFilePath);
        string tempText = streamReader.ReadToEnd();
        streamReader.Close();
        //获取文件名，不含扩展名
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(createNewFilePath);
        //tempText = Regex.Replace(tempText,"#ABName#",binderInfo);
        if (obj != null)
        {
            tempText = Regex.Replace(tempText,"#ViewName#",obj.name);
        }

        //将模板类中的类名替换成你创建的文件名
        tempText = Regex.Replace(tempText, "#ClassName#", fileNameWithoutExtension);

        if (obj!=null && PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Variant)
        {
            var baseName = PrefabUtility.GetCorrespondingObjectFromSource(obj).name;
            tempText = Regex.Replace(tempText, "#BaseClassName#", baseName);
        }

        tempText = Regex.Replace(tempText, "#DateTime#", System.DateTime.Now.ToLongDateString());

        //处理视图模板

        if(fileNameWithoutExtension.Substring(fileNameWithoutExtension.Length - 5, 5) == "Panel")
        //if (fileNameWithoutExtension.IndexOf("View") > -1)
        {
            string PreName = fileNameWithoutExtension.Remove(fileNameWithoutExtension.Length - 5, 5);
            string MediatorName = PreName + "Mediator";
            string ModelName = PreName + "Model";
            //string MediatorName = fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.IndexOf("View")) + "Mediator";
            tempText = Regex.Replace(tempText, "#MediatorName#", MediatorName);
            tempText = Regex.Replace(tempText, "#ModelName#", ModelName);
        }
        if (member != null && member.Length > 0)
        {
            tempText = Regex.Replace(tempText, "#MemberUI#", member);
        }
        else
        {
            tempText = Regex.Replace(tempText, "#MemberUI#", "");
        }

        if (memberfind != null && memberfind.Length > 0)
        {
            tempText = Regex.Replace(tempText, "#MemberFinder#", memberfind);
        }        
        else
        {
            tempText = Regex.Replace(tempText, "#MemberFinder#", "");
        }

        if (fileNameWithoutExtension.IndexOf("Mediator") > -1)
        {
            string ViewName = fileNameWithoutExtension.Substring(0, fileNameWithoutExtension.IndexOf("Mediator")) + "View";
            tempText = Regex.Replace(tempText, "#ViewClass#", ViewName);
        }
        
        


        bool encoderShouldEmitUTF8Identifier = true; //参数指定是否提供 Unicode 字节顺序标记
        bool throwOnInvalidBytes = false;//是否在检测到无效的编码时引发异常
        UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier, throwOnInvalidBytes);
        bool append = false;
        // 写入文件
        StreamWriter streamWriter = new StreamWriter(fullPath, append, encoding);
        streamWriter.Write(tempText);
        streamWriter.Close();
        // SubView 是固定路径的，不保存gen
        if (tempFilePath == m_s_tempRoot + "SubView.txt")
        {
            return null;
        }
        if (tempFilePath == m_s_tempRoot + "SubViewLogic.txt")
        {
            return null;
        }
        if (obj != null && PrefabUtility.GetPrefabAssetType(obj) == PrefabAssetType.Variant)
        {
            return null;
        }
        Dictionary<string,GenInfo> genInfoDic = getGenInfos();
        
        string pfPath = AssetDatabase.GetAssetPath(obj);
        string GUID = AssetDatabase.AssetPathToGUID(pfPath);

        GenInfo info = getPrefabInfo(pfPath);
      
        createNewFilePath = createNewFilePath.Substring(Application.dataPath.Replace("Assets", "").Length);
        // 开始写入配置
        if (tempFilePath == m_s_tempRoot + "MediatorTemplate.txt")
        {
            // mediator
            info.mediatorPath = createNewFilePath;
            
        }else if (tempFilePath == m_s_tempRoot + "ViewTemplate.txt")
        {
            // view
            info.codePath = createNewFilePath;
            info.type = 1;
        }else if (tempFilePath == m_s_tempRoot + "SingleView.txt")
        {
            // singleView
            info.codePath = createNewFilePath;
            info.type = 2;
        }else if (tempFilePath == m_s_tempRoot + "ModelTemplate.txt")
        {
            info.modelPath = createNewFilePath;
            info.type = 3;
        }

        SaveGenInfo();
        return info;
    }


}