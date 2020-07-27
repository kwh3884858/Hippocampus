using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
//using AssetBundles;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StarPlatinum
{
	public class AssetsManager : GameModule<AssetsManager>
	{

		//AssetBundleLoad m_assetBundleLoader;
		//private AssetBundleManager abm;
		bool m_isAssetBundleManagerLoaded = false;

		public bool IsAssetBundleManagerLoaded {
			get {
				return m_isAssetBundleManagerLoaded;
			}
		}

		//AssetBundle打包路径/assets/StreamingAssets
		//public static string ASSETBUNDLE_PATH = Application.dataPath + "/StreamingAssets/AssetBundle/";

		//资源地址/assets
		//public static string APPLICATION_PATH = Application.dataPath + "/";

		//工程地址/
		//public static string PROJECT_PATH = APPLICATION_PATH.Substring (0, APPLICATION_PATH.Length - 7);

		//AssetBundle存放的文件夹名
		public static string ASSETBUNDLE_FILENAME = "AssetBundle";

		//AssetBundle打包的后缀名
		public static string SUFFIX = ".unity3d";

		// 已有的资源类型
		public const string UI = "ui.unity3d";
		public const string CSV = "csv.unity3d";
		public const string SCENE = "scene.unity3d";
		public const string SOUND = "sound.unity3d";
		public const string MODEL = "model.unity3d";
		//static bool isUseBundle = false;

		/// <summary>
		/// AssetBundle打包路径/assets/StreamingAssets/AssetBundle/
		/// </summary>
		/// <value>The assetbundle path.</value>
		public static string ASSETBUNDLE_PATH {
			get {
				string dataPath = Application.dataPath;
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					return dataPath + "/Raw/AssetBundle/";
				}

				if (Application.platform == RuntimePlatform.Android) {

					return dataPath + "!assets/AssetBundle/";
				}

				if (Application.platform == RuntimePlatform.OSXPlayer) {
					return dataPath + "/Resources/Data/StreamingAssets/AssetBundle/";
				}

				if (Application.platform == RuntimePlatform.WindowsPlayer) {
					return dataPath + "/StreamingAssets/AssetBundle/";
				}

				return dataPath + "/StreamingAssets/AssetBundle/";
			}
		}
		/// <summary>
		/// StreamingAssets路径 /assets/StreamingAssets/
		/// </summary>
		/// <value>The assetbundle path.</value>
		public static string STREAMING_PATH {
			get {
				string dataPath = Application.dataPath;
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					return dataPath + "/Raw/";
				}

				if (Application.platform == RuntimePlatform.Android) {

					return dataPath + "!assets/";
				}

				if (Application.platform == RuntimePlatform.OSXPlayer) {
					return dataPath + "/Resources/Data/StreamingAssets/";
				}

				if (Application.platform == RuntimePlatform.WindowsPlayer) {
					return dataPath + "/StreamingAssets/";
				}

				return dataPath + "/StreamingAssets/";
			}
		}

		/// <summary>
		/// 资源地址/assets
		/// </summary>
		/// <value>The application path.</value>
		public static string APPLICATION_PATH {
			get {
				string dataPath = Application.dataPath;
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					return dataPath + "/Raw/";
				}

				if (Application.platform == RuntimePlatform.Android) {
					return "jar:file//" + dataPath + "!/";
				}

				if (Application.platform == RuntimePlatform.OSXPlayer) {
					return dataPath + "/Resources/Data/";
				}

				if (Application.platform == RuntimePlatform.WindowsPlayer) {
					return dataPath + "/";
				}

				return dataPath + "/";
			}
		}

		/// <summary>
		/// 工程地址/
		/// </summary>
		/// <value>The project path.</value>
		public static string PROJECT_PATH {
			get {
				string dataPath = Application.dataPath.Substring (0, Application.dataPath.Length - 6);
				if (Application.platform == RuntimePlatform.IPhonePlayer) {
					return dataPath;
				}

				if (Application.platform == RuntimePlatform.Android) {
					return "jar:file//" + dataPath;
				}

				if (Application.platform == RuntimePlatform.OSXPlayer) {
					return dataPath;
				}

				if (Application.platform == RuntimePlatform.WindowsPlayer) {
					return dataPath;
				}

				return dataPath;
			}
		}

		public override void SingletonInit ()
		{
			base.SingletonInit ();

			//AssetBundleLoad.Initialize (ASSETBUNDLE_FILENAME + "/" + AssetsUtility.GetPlatformName () + "/" + AssetsUtility.GetPlatformName ());

			//AssetBundleLoad.overrideBaseDownloadingURL += OverrideBaseDownloadingURLWithPlatform;

//#if UNITY_EDITOR
			//abm = new AssetBundleManager();
			//abm.SetPrioritizationStrategy(AssetBundleManager.PrioritizationStrategy.PrioritizeStreamingAssets);
			//abm.UseSimulatedUri();
//#else

   //         abm = new AssetBundleManager ();
			//abm.SetPrioritizationStrategy (AssetBundleManager.PrioritizationStrategy.PrioritizeStreamingAssets);
			//abm.UseSimulatedUri ();
			//abm.Initialize (OnAssetBundleManagerInitialized);
//#endif
		}

		private void OnAssetBundleManagerInitialized (bool success)
		{
			if (success) {
				Debug.Log ("Assetbundle Manager loaded finish.");
				m_isAssetBundleManagerLoaded = true;
			} else
				Debug.Log ("Assetbundle Manager loaded failed");
			//Application.LoadLevelAdditiveAsync (levelName);
		}


		//public static string OverrideBaseDownloadingURLWithPlatform (string bundleName)
		//{
		//	return AssetsUtility.GetPlatformName () + "/" + bundleName;
		//}

		public void LoadPrefab (string path, Action<AssetBundle> OnAssetBundleDownloaded)
		{
//#if UNITY_EDITOR

			//if (abm != null) {
			//	abm.GetBundle (path, OnAssetBundleDownloaded);
			//} else {
			//	Debug.LogError ("Error initializing ABM.");
			//}

			//string strName = "Assets/" + path + ".prefab";
			//T go = AssetDatabase.LoadAssetAtPath<T> (strName);

			//return go;

			//Console.Instance ().Debug (path);
			//path = path.ToLower ();
			//T go = AssetBundleLoad.LoadGameObject (path) as T;
			//return go;
//#else

//				//Console.Instance ().Debug (path);
//			if (abm != null) {
//				abm.GetBundle (path, OnAssetBundleDownloaded);
//			} else {
//				Debug.LogError ("Error initializing ABM.");
//			}
//#endif
		}

		public T LoadPrefab<T> (string path) where T : UnityEngine.Object
		{
			string strName = "Assets/" + path + ".prefab";
			T go = PrefabManager.Instance.LoadPrefab (strName) as T;

			return go;
		}


		public void LoadScene (
			string sceneName,
			UnityEngine.SceneManagement.LoadSceneMode mode
			)
		{
//#if UNITY_EDITOR
			//string levelName = "scenes/scene1.unity3d";

			//if (abm != null) {
			//	abm.GetBundle (string.Format ("scenes/{0}.unity3d", sceneName), OnSceneAssetBundleDownloaded);
			//} else {
			//	Debug.LogError ("Error initializing ABM.");
			//}
			//UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);

			////old style
			//string strName = "Assets/scenes/" + sceneName + ".unity";
			StartCoroutine (LoadYourAsyncScene (sceneName));

			//AssetDatabase.LoadAssetAtPath<SceneAsset> (strName);
			//UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);

			//new style: load from scenemanger
			//AsyncOperation asyncLoad = new AsyncOperation ();

			//StartCoroutine (InitializeLevelAsync (sceneName, mode));


//#else
//			if (abm != null) {
//				abm.GetBundle (string.Format ("scenes/{0}.unity3d", sceneName), OnSceneAssetBundleDownloaded);
//			} else {
//				Debug.LogError ("Error initializing ABM.");
//			}
//			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);

//			//StartCoroutine (InitializeLevelAsync (sceneName, mode));
//			//Console.Instance ().Debug (path);
//			//path = path.ToLower ();
//			//T go = AssetBundleLoad.LoadGameObject (path) as T;
//            //return go;
//#endif
		}
		private void OnSceneAssetBundleDownloaded (AssetBundle bundle)
		{
			//string bundleName;
			//bundleName = bundle.name;
			//if (bundle != null) {
			//	// Do something with the bundle
			//	abm.UnloadBundle (bundle);
			//}
			//string sceneName = bundleName.Substring (
			//	bundleName.IndexOf ('/') + 1,
			//	bundleName.LastIndexOf ('.') - bundleName.IndexOf ('/') - 1
			//	);
			//Console.Instance ().Debug (sceneName);

			//abm.Dispose ();
			////Console.Instance().
			////TODO:Test whether bundle name is scene name? 
			//EventManager.Instance ().SendEvent<SceneLoadedEvent> (new SceneLoadedEvent (sceneName));
		}

		//IEnumerator InitializeLevelAsync (string sceneName, UnityEngine.SceneManagement.LoadSceneMode mode)
		//{
		//	yield return null;
		//	AssetBundleLoadOperation operation;
		//	System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder ("scenes/", 3);
		//	stringBuilder.Append (sceneName);
		//	stringBuilder.Append (".unity3d");
		//	switch (mode) {
		//	case UnityEngine.SceneManagement.LoadSceneMode.Single:

		//		operation = AssetBundleLoad.LoadLevelAsync (stringBuilder.ToString (), sceneName, false);
		//		if (operation == null)
		//			yield break;
		//		yield return StartCoroutine (operation);
		//		break;

		//	case UnityEngine.SceneManagement.LoadSceneMode.Additive:
		//		operation = AssetBundleLoad.LoadLevelAsync (stringBuilder.ToString (), sceneName, true);
		//		if (operation == null)
		//			yield break;
		//		yield return StartCoroutine (operation);
		//		break;


		//	}

		//string strName = "Assets/" + sceneName + ".unity";
		//AssetBundleCreateRequest bundleLoadRequest = AssetBundle.LoadFromFileAsync (Path.Combine (STREAMING_PATH, sceneName));
		//yield return bundleLoadRequest;

		//AssetBundle myLoadedAssetBundle = bundleLoadRequest.assetBundle;
		//if (myLoadedAssetBundle == null) {
		//	Debug.Log ("Failed to load AssetBundle!");
		//	yield break;
		//}

		//var assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject> ("MyObject");
		//yield return assetLoadRequest;

		//GameObject prefab = assetLoadRequest.asset as GameObject;
		//Instantiate (prefab);
		//myLoadedAssetBundle.Unload (false);

		//asyncOperation.allowSceneActivation = false;
		//while (!asyncOperation.isDone) {

		//	//Output the current progress
		//	Debug.Log ("Loading progress: " + (asyncOperation.progress * 100) + "%");

		//	// Check if the load has finished
		//	if (asyncOperation.progress >= 0.9f) {
		//		//Change the Text to show the Scene is ready
		//		Debug.Log ("Press the space bar to continue");
		//		//Wait to you press the space key to activate the Scene
		//		if (Input.GetKeyDown (KeyCode.Space))
		//			//Activate the Scene
		//			asyncOperation.allowSceneActivation = true;
		//	}

		//	yield return null;
		//}
		////if current scene is the now loaded scene, set it to variety
		//UnityEngine.SceneManagement.Scene scene =
		//UnityEngine.SceneManagement.SceneManager.GetActiveScene ();

		//if (scene.name == sceneName) {
		//	m_currentScene = scene;
		//}

		//}

		IEnumerator LoadYourAsyncScene (string sceneName)
		{
			// The Application loads the Scene in the background as the current Scene runs.
			// This is particularly good for creating loading screens.
			// You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
			// a sceneBuildIndex of 1 as shown in Build Settings.

			AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (sceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);


			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone) {
				yield return null;
			}

			//EventManager.Instance .SendEvent<SceneLoadedEvent> (new SceneLoadedEvent (sceneName));

		}
		//		public static GameObject LoadMaterialPrefabs (string path)
		//		{
		//#if UNITY_EDITOR
		//			//string strName = "Assets/Prefabs/" + path + ".prefab";
		//			//T go = AssetDatabase.LoadAssetAtPath<T> (strName);
		//			//			Debug.Log (path);
		//			path = path.ToLower ();
		//			GameObject go = AssetBundleLoad.LoadGameObject (path) as GameObject;
		//			//         string strName = "Assets/" + path + ".prefab";
		//			//T go = AssetDatabase.LoadAssetAtPath<T> (strName);
		//			go.GetComponent<Renderer> ().sharedMaterial.shader = Shader.Find (go.GetComponent<Renderer> ().sharedMaterial.shader.name);
		//			return go;
		//#else
		//			//string strName = ASSETBUNDLE_PATH + path;
		//            //Debug.Log(strName);
		//			//GameObject.Find ("Console").GetComponent <Text>().text += "\n" + strName;
		//			path = path.ToLower ();
		//			GameObject go = AssetBundleLoad.LoadGameObject (path) as GameObject;
		//            go.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find(go.GetComponent<Renderer>().sharedMaterial.shader.name);

		//            return go;
		//#endif
		//		}
		public static T LoadAnimationController<T> (string path) where T : UnityEngine.Object
		{
#if UNITY_EDITOR
			string strName = "Assets/Prefabs/" + path + ".prefab";
			T go = AssetDatabase.LoadAssetAtPath<T> (strName);
			return go;
#else
			string strName = ASSETBUNDLE_PATH + path;
            Debug.Log(strName);
            AssetBundle bundle = AssetBundle.LoadFromFile(strName);
            T go = bundle.LoadAllAssets<T>()[0];
            return go;
#endif
		}


		public static T LoadPrefabFromResources<T> (string path) where T : UnityEngine.Object
		{
#if UNITY_EDITOR
			//string strName = "Assets/" + path;
			//T go = AssetDatabase.LoadAssetAtPath<T> (strName);
			T go = Resources.Load<T> (path) as T;
			return go;
#else

			//string strName = ASSETBUNDLE_PATH + path;
			//Console.Log(path);
			T go = Resources.Load (path, typeof(T)) as T;
			//AssetBundle bundle = AssetBundle.LoadFromFile(strName);
			//T go = bundle.LoadAllAssets<T>()[0];
			return go;
#endif
		}


		//private IEnumerator Start ()
		//{
		//	AssetBundleManager.SetDevelopmentAssetBundleServer ();

		//	var request = AssetBundleManager.Initialize ();

		//	if (request != null)
		//		yield return StartCoroutine (request);

		//	//AssetBundleLoadAssetOperation loadRequest = AssetBundleManager.LoadAssetAsync("prefab", "Cube", typeof(GameObject));
		//	//if (loadRequest == null)
		//	//    yield break;

		//	//yield return StartCoroutine(loadRequest);

		//	//GameObject prefab = loadRequest.GetAsset();
		//	////如果讀取成功, 則創建實體
		//	//if (prefab != null)
		//	//    GameObject.Instantiate(prefab);

		//	//yield return new WaitForSeconds(5f);
		//	////釋放"prefab"這個bundle
		//	//AssetBundleManager.UnloadAssetBundle("prefab");
		//}
	}
}
