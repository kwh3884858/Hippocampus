
using System.IO;
using UnityEditor;
using UnityEngine;


/// <summary>
/// Auto update build setting for scenes when user open a new scene.
/// </summary>
[InitializeOnLoad]
public class SceneLookupGenerator : Editor
{

    private static readonly string scenePath = "/Scenes";

    static SceneLookupGenerator()
    {

        UnityEditor.SceneManagement.EditorSceneManager.sceneOpened += SceneOpened;

    }


    static void SceneOpened(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
    {
        CheckForSceneScript(scene, mode);

        string path = Application.dataPath + scenePath;

        if (!Directory.Exists(path))
        {
            Debug.Log("Can`t find Assets/Scene. Make sure already execute Assets/Framework/Initialize Framework Directory");
            return;
        }


        string[] files = Directory.GetFiles(path, "*.unity", SearchOption.AllDirectories);


        EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[files.Length];

        for (int i = 0; i < files.Length; ++i)
        {
            string scenePath = files[i];

            scenePath = scenePath.Replace("\\", "/");
            scenePath = scenePath.Substring(scenePath.IndexOf("Assets/Scenes/"));

            scenes[i] = new EditorBuildSettingsScene(scenePath, true);
        }

        EditorBuildSettings.scenes = scenes;

        Debug.Log("Generated new build setting for scenes");
    }

    static void CheckForSceneScript(UnityEngine.SceneManagement.Scene scene, UnityEditor.SceneManagement.OpenSceneMode mode)
    {
        //GameObject[] gameObjects= scene.GetRootGameObjects();
        //bool isExistSceneRootGameobject = false;
        //foreach (GameObject go in gameObjects)
        //{
        //    if(go.name.IndexOf( scene.name) != -1)
        //    {
        //        isExistSceneRootGameobject = true;
        //    }
        //}

        GameObject go = GameObject.Find(scene.name);

        if (go == null)
        {
            new GameObject(scene.name);

        }


        return;

    }

}