using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using UnityEditor.SceneManagement;

namespace Skylight
{
	public class InitFramework : Editor
	{

		/// <summary>
		/// 初始化框架文件夹
		/// </summary>
		[MenuItem ("Assets/Framework/InitFrameworkDirectory")]
		static void InitFrameworkDirectory ()
		{

			string [] frameworkDir = {
				"UI/Panel",             //存放UI的预设体
				"UI/Dialog",
				"UI/Overlay",
				"UI/Box",
				"Skylight/UI/Panel",	//框架中的UI预设体
				"Skylight/UI/Dialog",
				"Skylight/UI/Overlay",
				"Skylight/UI/Box",
				"Effects",              //视觉效果
				"Prefabs",              //存放预设
				"Plugins",				//插件
				"Scenes",               //存放场景和场景预设
				"Models",               //存放模型
                "Materials",            //材质文件
                "Shaders",              //shder文件
				"Images",               //存放图片
                "Images/Character",
				"Images/Item",
				"Images/Map",
				"Images/Weapon",
				"Animations",
				"Animations/Character",
				"Librarys",				//所依赖的库，例如本地数据库
				"Resources/Sound/Music",
				"Resources/Sound/Effect",
				"Resources/Sound",      //音乐系统存放音乐素材
                "Resources/Images",     //对话编辑器图片资源
                "Scripts",
				"Scripts/UI/Panel",     //UI系统附带脚本
                "Scripts/UI/Dialog",
				"Scripts/UI/Overlay",
				"Scripts/UI/Box",
				"Scripts/Logic",
				"Scripts/Scenes",
				"Sources",				//保存生成代码，以及不需要手动编写的代码
				"Tools",				//工具
			};


			for (int i = 0; i < frameworkDir.Length; i++) {

				string path = Application.dataPath + "/" + frameworkDir [i];

				if (!Directory.Exists (path)) {
					Directory.CreateDirectory (path);
				}
			}
			AssetDatabase.Refresh ();

		}

		[MenuItem ("Assets/Framework/CreateGameRootScene")]
		static void InitSceneBuildSetting ()
		{
			UnityEngine.SceneManagement.Scene currentScene;

			const string GameRootScenePath = "Scenes";
			const string GameRootSceneName = "GameRoot.unity";
			StringBuilder builder = new StringBuilder ();
			builder.Append (Application.dataPath);
			builder.Append ("/");
			builder.Append (GameRootScenePath);

			string path = builder.ToString ();
			if (!Directory.Exists (path)) {
				EditorGUILayout.HelpBox ("Now auto generate directory: " +
				 GameRootScenePath, MessageType.Info, false);
				Directory.CreateDirectory (path);
			}

			builder.Append ("/");
			builder.Append (GameRootSceneName);
			string gameRoot = builder.ToString ();
			if (!File.Exists (gameRoot)) {
				bool isAutoGenerate = EditorUtility.DisplayDialog ("Auto Generation",
				"You don`t have GameRoot.unity scene." +
				"Do you need to auto generate?" +
				"Current scene will be save as GameRoot in Scenes fold",
					"OK", "No, thanks");

				if (isAutoGenerate) {
					currentScene = EditorSceneManager.GetActiveScene ();
					builder.Remove (0, Application.dataPath.Length - 6);
					if (!EditorSceneManager.SaveScene (currentScene, builder.ToString (), false)) {
						Debug.LogError ("Scene save error!");
						return;
					} else {
						Debug.Log ("GameRoot scene generate in " + builder);
					}

				} else {
					return;
				}

			}
			currentScene = EditorSceneManager.GetActiveScene ();

			Debug.Log ("Current Scene name is " + currentScene.name);

		}

		[MenuItem ("Assets/Framework/SetGameRootAsFirstScene")]
		static void SetGameRootAsFirstScene ()
		{
			UnityEngine.SceneManagement.Scene scene = EditorSceneManager.GetActiveScene ();
			SetBuildSettingFirstScene (scene);
			SetPlayModeStartScene (scene.path);
		}

		[MenuItem ("Assets/Framework/SetGameRootAsFirstScene", true)]
		static bool SetGameRootAsFirstSceneCheckValidate ()
		{
			return EditorSceneManager.GetActiveScene ().path == "Assets/Scenes/GameRoot.unity";
		}

		static void SetBuildSettingFirstScene (UnityEngine.SceneManagement.Scene scene)
		{
			List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene> ();
			EditorBuildSettingsScene settingScene = new EditorBuildSettingsScene (scene.path, true);
			scenes.Add (settingScene);
			EditorBuildSettings.scenes = scenes.ToArray ();
		}

		static void SetPlayModeStartScene (string scenePath)
		{
			SceneAsset myWantedStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset> (scenePath);
			if (myWantedStartScene != null)
				EditorSceneManager.playModeStartScene = myWantedStartScene;
			else
				Debug.Log ("Could not find Scene " + scenePath);
		}


		[MenuItem ("Assets/Framework/TestButton %t")]
		static void TestButton ()
		{

		}



	}
}
