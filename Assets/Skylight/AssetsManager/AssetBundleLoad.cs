using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

namespace Skylight
{

	/*  The AssetBundle Manager provides a High-Level API for working with AssetBundles. 
    The AssetBundle Manager will take care of loading AssetBundles and their associated 
    Asset Dependencies.
        Initialize()
            Initializes the AssetBundle manifest object.
        LoadAssetAsync()
            Loads a given asset from a given AssetBundle and handles all the dependencies.
        LoadLevelAsync()
            Loads a given scene from a given AssetBundle and handles all the dependencies.
        LoadDependencies()
            Loads all the dependent AssetBundles for a given AssetBundle.
        BaseDownloadingURL
            Sets the base downloading url which is used for automatic downloading dependencies.
        SimulateAssetBundleInEditor
            Sets Simulation Mode in the Editor.
        Variants
            Sets the active variant.
        RemapVariantName()
            Resolves the correct AssetBundle according to the active variant.
*/

	public class AssetBundleLoad : MonoBehaviour
	{
		public enum LogMode { All, JustErrors };
		public enum LogType { Info, Warning, Error };


		static LogMode m_LogMode = LogMode.All;
		static string m_BaseDownloadingURL = "";
		static string [] m_ActiveVariants = { };
		static AssetBundleManifest m_AssetBundleManifest = null;


#if UNITY_EDITOR
		static int m_SimulateAssetBundleInEditor = -1;
		const string kSimulateAssetBundles = "SimulateAssetBundles";
#endif

		static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle> ();
		static Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string> ();
		static List<string> m_DownloadingBundles = new List<string> ();
		static List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation> ();
		static Dictionary<string, string []> m_Dependencies = new Dictionary<string, string []> ();

		private static void Log (LogType logType, string text)
		{
			if (logType == LogType.Error)
				Debug.LogError ("[AssetBundleManager] " + text);
			else if (m_LogMode == LogMode.All && logType == LogType.Warning)
				Debug.LogWarning ("[AssetBundleManager] " + text);
			else if (m_LogMode == LogMode.All)
				Debug.Log ("[AssetBundleManager] " + text);
		}


		/// <summary>
		/// The base downloading url which is used to generate the full
		/// downloading url with the assetBundle names.
		/// </summary>
		public static string BaseDownloadingURL {
			get { return m_BaseDownloadingURL; }
			set { m_BaseDownloadingURL = value; }
		}

		public delegate string OverrideBaseDownloadingURLDelegate (string bundleName);

		/// <summary>
		/// Implements per-bundle base downloading URL override.
		/// The subscribers must return null values for unknown bundle names;
		/// </summary>
		public static event OverrideBaseDownloadingURLDelegate overrideBaseDownloadingURL;


		public static string [] ActiveVariants {
			get { return m_ActiveVariants; }
			set { m_ActiveVariants = value; }
		}


		/// <summary>
		/// AssetBundleManifest object which can be used to load the dependecies
		/// and check suitable assetBundle variants.
		/// </summary>
		public static AssetBundleManifest AssetBundleManifestObject {
			set { m_AssetBundleManifest = value; }
		}


#if UNITY_EDITOR
		/// <summary>
		/// Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
		/// </summary>
		public static bool SimulateAssetBundleInEditor {
			get {
				if (m_SimulateAssetBundleInEditor == -1)
					m_SimulateAssetBundleInEditor = EditorPrefs.GetBool (kSimulateAssetBundles, true) ? 1 : 0;

				return m_SimulateAssetBundleInEditor != 0;
			}
			set {
				int newValue = value ? 1 : 0;
				if (newValue != m_SimulateAssetBundleInEditor) {
					m_SimulateAssetBundleInEditor = newValue;
					EditorPrefs.SetBool (kSimulateAssetBundles, value);
				}
			}
		}
#endif

		//old content, sync load assetbundle
		private static AssetBundleManifest manifest = null;

		private static Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle> ();

		public static AssetBundle LoadAB (string abPath)
		{
			if (abDic.ContainsKey (abPath) == true)
				return abDic [abPath];
			if (manifest == null) {
				string ane = AssetsManager.ASSETBUNDLE_PATH + AssetsUtility.GetPlatformName () + "/" + AssetsUtility.GetPlatformName ();
				AssetBundle manifestBundle = AssetBundle.LoadFromFile (AssetsManager.ASSETBUNDLE_PATH +
																		  AssetsUtility.GetPlatformName () + "/" + AssetsUtility.GetPlatformName ());
				manifest = (AssetBundleManifest)manifestBundle.LoadAsset ("AssetBundleManifest");
			}
			if (manifest != null) {
				// 2.获取依赖文件列表  
				string [] cubedepends = manifest.GetAllDependencies (abPath);

				for (int index = 0; index < cubedepends.Length; index++) {
					//Debug.Log(cubedepends[index]);
					// 3.加载所有的依赖资源
					LoadAB (cubedepends [index]);
				}

				// 4.加载资源
				abDic [abPath] = AssetBundle.LoadFromFile (AssetsManager.ASSETBUNDLE_PATH + AssetsUtility.GetPlatformName () + "/" + abPath);

				return abDic [abPath];
			}
			return null;
		}

		public static Object LoadGameObject (string abName)
		{
			string abPath = abName + AssetsManager.SUFFIX;
			int index = abName.LastIndexOf ('/');
			if (index == -1) {
				index = abName.Length;
			}
			string realName = abName.Substring (index + 1, abName.Length - index - 1);

			LoadAB (abPath);

			if (abDic.ContainsKey (abPath) && abDic [abPath] != null) {
				return abDic [abPath].LoadAsset (realName);
			}
			return null;
		}
		//old content finish

		public static void LoadGameObjectAsync (string abName)
		{
			string abPath = abName + AssetsManager.SUFFIX;
			int index = abName.LastIndexOf ('/');
			if (index == -1) {
				index = abName.Length;
			}
			string realName = abName.Substring (index + 1, abName.Length - index - 1);
		}

		private static Object LoadABAsync (string abPath)
		{
			if (abDic.ContainsKey (abPath) == true)
				return abDic [abPath];


			return null;
		}


		/// <summary>
		/// Retrieves an asset bundle that has previously been requested via LoadAssetBundle.
		/// Returns null if the asset bundle or one of its dependencies have not been downloaded yet.
		/// </summary>
		static public LoadedAssetBundle GetLoadedAssetBundle (string assetBundleName, out string error)
		{
			if (m_DownloadingErrors.TryGetValue (assetBundleName, out error))
				return null;

			LoadedAssetBundle bundle = null;
			m_LoadedAssetBundles.TryGetValue (assetBundleName, out bundle);
			if (bundle == null)
				return null;

			// No dependencies are recorded, only the bundle itself is required.
			string [] dependencies = null;
			if (!m_Dependencies.TryGetValue (assetBundleName, out dependencies))
				return bundle;

			// Make sure all dependencies are loaded
			foreach (var dependency in dependencies) {
				if (m_DownloadingErrors.TryGetValue (dependency, out error))
					return null;

				// Wait all the dependent assetBundles being loaded.
				LoadedAssetBundle dependentBundle;
				m_LoadedAssetBundles.TryGetValue (dependency, out dependentBundle);
				if (dependentBundle == null)
					return null;
			}

			return bundle;
		}


		/// <summary>
		/// Initializes asset bundle namager and starts download of manifest asset bundle.
		/// Returns the manifest asset bundle downolad operation object.
		/// </summary>
		static public AssetBundleLoadManifestOperation Initialize ()
		{
#if UNITY_EDITOR
			SimulateAssetBundleInEditor = false;
#endif
			return Initialize (AssetsUtility.GetPlatformName ());
		}

		/// <summary>
		/// Initializes asset bundle namager and starts download of manifest asset bundle.
		/// Returns the manifest asset bundle downolad operation object.
		/// </summary>
		static public AssetBundleLoadManifestOperation Initialize (string manifestAssetBundleName)
		{
#if UNITY_EDITOR
			Log (LogType.Info, "Simulation Mode: " + (SimulateAssetBundleInEditor ? "Enabled" : "Disabled"));
#endif

			var go = new GameObject ("AssetBundleManager", typeof (AssetBundleLoad));
			DontDestroyOnLoad (go);

#if UNITY_EDITOR
			// If we're in Editor simulation mode, we don't need the manifest assetBundle.
			if (SimulateAssetBundleInEditor)
				return null;
#endif

			LoadAssetBundle (manifestAssetBundleName, true);
			var operation = new AssetBundleLoadManifestOperation (manifestAssetBundleName, "AssetBundleManifest", typeof (AssetBundleManifest));
			m_InProgressOperations.Add (operation);
			return operation;
		}


		// Temporarily work around a il2cpp bug
		static protected void LoadAssetBundle (string assetBundleName)
		{
			LoadAssetBundle (assetBundleName, false);
		}

		// Starts the download of the asset bundle identified by the given name, and asset bundles
		// that this asset bundle depends on.
		static protected void LoadAssetBundle (string assetBundleName, bool isLoadingAssetBundleManifest)
		{
			Log (LogType.Info, "Loading Asset Bundle " + (isLoadingAssetBundleManifest ? "Manifest: " : ": ") + assetBundleName);

#if UNITY_EDITOR
			// If we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.
			if (SimulateAssetBundleInEditor)
				return;
#endif

			if (!isLoadingAssetBundleManifest) {
				if (m_AssetBundleManifest == null) {
					Log (LogType.Error, "Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
					return;
				}
			}

			// Check if the assetBundle has already been processed.
			bool isAlreadyProcessed = LoadAssetBundleInternal (assetBundleName, isLoadingAssetBundleManifest);

			// Load dependencies.
			if (!isAlreadyProcessed && !isLoadingAssetBundleManifest)
				LoadDependencies (assetBundleName);
		}
		/// <summary>
		/// Starts a load operation for a level from the given asset bundle.
		/// </summary>
		static public AssetBundleLoadOperation LoadLevelAsync (string assetBundleName, string levelName, bool isAdditive)
		{
			Log (LogType.Info, "Loading " + levelName + " from " + assetBundleName + " bundle");

			AssetBundleLoadOperation operation = null;

#if UNITY_EDITOR
			if (SimulateAssetBundleInEditor) {
				operation = new AssetBundleLoadLevelSimulationOperation (assetBundleName, levelName, isAdditive);
			} else
#endif
			{
				assetBundleName = RemapVariantName (assetBundleName);
				LoadAssetBundle (assetBundleName);
				operation = new AssetBundleLoadLevelOperation (assetBundleName, levelName, isAdditive);

				m_InProgressOperations.Add (operation);
			}

			return operation;
		}

		// Returns base downloading URL for the given asset bundle.
		// This URL may be overridden on per-bundle basis via overrideBaseDownloadingURL event.
		protected static string GetAssetBundleBaseDownloadingURL (string bundleName)
		{
			if (overrideBaseDownloadingURL != null) {
				foreach (OverrideBaseDownloadingURLDelegate method in overrideBaseDownloadingURL.GetInvocationList ()) {
					string res = method (bundleName);
					if (res != null)
						return res;
				}
			}
			return m_BaseDownloadingURL;
		}


		// Checks who is responsible for determination of the correct asset bundle variant
		// that should be loaded on this platform. 
		//
		// On most platforms, this is done by the AssetBundleManager itself. However, on
		// certain platforms (iOS at the moment) it's possible that an external asset bundle
		// variant resolution mechanism is used. In these cases, we use base asset bundle 
		// name (without the variant tag) as the bundle identifier. The platform-specific 
		// code is responsible for correctly loading the bundle.
		static protected bool UsesExternalBundleVariantResolutionMechanism (string baseAssetBundleName)
		{
#if ENABLE_IOS_APP_SLICING
            var url = GetAssetBundleBaseDownloadingURL(baseAssetBundleName);
            if (url.ToLower().StartsWith("res://") ||
                url.ToLower().StartsWith("odr://"))
                return true;
#endif
			return false;
		}


		// Remaps the asset bundle name to the best fitting asset bundle variant.
		static protected string RemapVariantName (string assetBundleName)
		{
			string [] bundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant ();

			// Get base bundle name
			string baseName = assetBundleName.Split ('.') [0];

			if (UsesExternalBundleVariantResolutionMechanism (baseName))
				return baseName;

			int bestFit = int.MaxValue;
			int bestFitIndex = -1;
			// Loop all the assetBundles with variant to find the best fit variant assetBundle.
			for (int i = 0; i < bundlesWithVariant.Length; i++) {
				string [] curSplit = bundlesWithVariant [i].Split ('.');
				string curBaseName = curSplit [0];
				string curVariant = curSplit [1];

				if (curBaseName != baseName)
					continue;

				int found = System.Array.IndexOf (m_ActiveVariants, curVariant);

				// If there is no active variant found. We still want to use the first
				if (found == -1)
					found = int.MaxValue - 1;

				if (found < bestFit) {
					bestFit = found;
					bestFitIndex = i;
				}
			}

			if (bestFit == int.MaxValue - 1) {
				Log (LogType.Warning, "Ambigious asset bundle variant chosen because there was no matching active variant: " + bundlesWithVariant [bestFitIndex]);
			}

			if (bestFitIndex != -1) {
				return bundlesWithVariant [bestFitIndex];
			} else {
				return assetBundleName;
			}
		}


		// Sets up download operation for the given asset bundle if it's not downloaded already.
		static protected bool LoadAssetBundleInternal (string assetBundleName, bool isLoadingAssetBundleManifest)
		{
			// Already loaded.
			LoadedAssetBundle bundle = null;
			m_LoadedAssetBundles.TryGetValue (assetBundleName, out bundle);
			if (bundle != null) {
				bundle.m_ReferencedCount++;
				return true;
			}

			// @TODO: Do we need to consider the referenced count of WWWs?
			// In the demo, we never have duplicate WWWs as we wait LoadAssetAsync()/LoadLevelAsync() to be finished before calling another LoadAssetAsync()/LoadLevelAsync().
			// But in the real case, users can call LoadAssetAsync()/LoadLevelAsync() several times then wait them to be finished which might have duplicate WWWs.
			if (m_DownloadingBundles.Contains (assetBundleName))
				return true;

			string bundleBaseDownloadingURL = GetAssetBundleBaseDownloadingURL (assetBundleName);

			if (bundleBaseDownloadingURL.ToLower ().StartsWith ("odr://", System.StringComparison.Ordinal)) {
#if ENABLE_IOS_ON_DEMAND_RESOURCES
                Log(LogType.Info, "Requesting bundle " + assetBundleName + " through ODR");
                m_InProgressOperations.Add(new AssetBundleDownloadFromODROperation(assetBundleName));
#else
				new System.ApplicationException ("Can't load bundle " + assetBundleName + " through ODR: this Unity version or build target doesn't support it.");
#endif
			} else if (bundleBaseDownloadingURL.ToLower ().StartsWith ("res://")) {
#if ENABLE_IOS_APP_SLICING
                Log(LogType.Info, "Requesting bundle " + assetBundleName + " through asset catalog");
                m_InProgressOperations.Add(new AssetBundleOpenFromAssetCatalogOperation(assetBundleName));
#else
				new System.ApplicationException ("Can't load bundle " + assetBundleName + " through asset catalog: this Unity version or build target doesn't support it.");
#endif
			} else {
				WWW download = null;

				if (!bundleBaseDownloadingURL.EndsWith ("/")) {
					bundleBaseDownloadingURL += "/";
				}

				string url = bundleBaseDownloadingURL + assetBundleName;

				// For manifest assetbundle, always download it as we don't have hash for it.
				if (isLoadingAssetBundleManifest)
					download = new WWW (url);
				else
					download = WWW.LoadFromCacheOrDownload (url, m_AssetBundleManifest.GetAssetBundleHash (assetBundleName), 0);

				m_InProgressOperations.Add (new AssetBundleDownloadFromWebOperation (assetBundleName, download));
			}
			m_DownloadingBundles.Add (assetBundleName);

			return false;
		}



		void Update ()
		{
			// Update all in progress operations
			for (int i = 0; i < m_InProgressOperations.Count;) {
				var operation = m_InProgressOperations [i];
				if (operation.Update ()) {
					i++;
				} else {
					m_InProgressOperations.RemoveAt (i);
					ProcessFinishedOperation (operation);
				}
			}
		}

		void ProcessFinishedOperation (AssetBundleLoadOperation operation)
		{
			AssetBundleDownloadOperation download = operation as AssetBundleDownloadOperation;
			if (download == null)
				return;

			if (string.IsNullOrEmpty (download.error))
				m_LoadedAssetBundles.Add (download.assetBundleName, download.assetBundle);
			else {
				string msg = string.Format ("Failed downloading bundle {0} from {1}: {2}",
						download.assetBundleName, download.GetSourceURL (), download.error);
				m_DownloadingErrors.Add (download.assetBundleName, msg);
			}

			m_DownloadingBundles.Remove (download.assetBundleName);
		}


		// Where we get all the dependencies and load them all.
		static protected void LoadDependencies (string assetBundleName)
		{
			if (m_AssetBundleManifest == null) {
				Log (LogType.Error, "Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
				return;
			}

			// Get dependecies from the AssetBundleManifest object..
			string [] dependencies = m_AssetBundleManifest.GetAllDependencies (assetBundleName);
			if (dependencies.Length == 0)
				return;

			for (int i = 0; i < dependencies.Length; i++)
				dependencies [i] = RemapVariantName (dependencies [i]);

			// Record and load all dependencies.
			m_Dependencies.Add (assetBundleName, dependencies);
			for (int i = 0; i < dependencies.Length; i++)
				LoadAssetBundleInternal (dependencies [i], false);
		}


		/// <summary>
		/// Unloads assetbundle and its dependencies.
		/// </summary>
		static public void UnloadAssetBundle (string assetBundleName)
		{
#if UNITY_EDITOR
			// If we're in Editor simulation mode, we don't have to load the manifest assetBundle.
			if (SimulateAssetBundleInEditor)
				return;
#endif
			assetBundleName = RemapVariantName (assetBundleName);

			UnloadAssetBundleInternal (assetBundleName);
			UnloadDependencies (assetBundleName);
		}

		static protected void UnloadDependencies (string assetBundleName)
		{
			string [] dependencies = null;
			if (!m_Dependencies.TryGetValue (assetBundleName, out dependencies))
				return;

			// Loop dependencies.
			foreach (var dependency in dependencies) {
				UnloadAssetBundleInternal (dependency);
			}

			m_Dependencies.Remove (assetBundleName);
		}

		static protected void UnloadAssetBundleInternal (string assetBundleName)
		{
			string error;
			LoadedAssetBundle bundle = GetLoadedAssetBundle (assetBundleName, out error);
			if (bundle == null)
				return;

			if (--bundle.m_ReferencedCount == 0) {
				bundle.OnUnload ();
				m_LoadedAssetBundles.Remove (assetBundleName);

				Log (LogType.Info, assetBundleName + " has been unloaded successfully");
			}
		}

	}

	//	/// <summary>
	//	/// Loaded assetBundle contains the references count which can be used to
	//	/// unload dependent assetBundles automatically.
	//	/// </summary>
	//	public class LoadedAssetBundle
	//	{
	//		public AssetBundle m_AssetBundle;
	//		public int m_ReferencedCount;

	//		internal event System.Action unload;

	//		internal void OnUnload ()
	//		{
	//			m_AssetBundle.Unload (false);
	//			if (unload != null)
	//				unload ();
	//		}

	//		public LoadedAssetBundle (AssetBundle assetBundle)
	//		{
	//			m_AssetBundle = assetBundle;
	//			m_ReferencedCount = 1;
	//		}
	//	}


	//	public abstract class AssetBundleLoadOperation : IEnumerator
	//	{
	//		public object Current {
	//			get {
	//				return null;
	//			}
	//		}

	//		public bool MoveNext ()
	//		{
	//			return !IsDone ();
	//		}

	//		public void Reset ()
	//		{
	//		}

	//		abstract public bool Update ();

	//		abstract public bool IsDone ();
	//	}

	//	public abstract class AssetBundleDownloadOperation : AssetBundleLoadOperation
	//	{
	//		bool done;

	//		public string assetBundleName { get; private set; }
	//		public LoadedAssetBundle assetBundle { get; protected set; }
	//		public string error { get; protected set; }

	//		protected abstract bool downloadIsDone { get; }
	//		protected abstract void FinishDownload ();

	//		public override bool Update ()
	//		{
	//			if (!done && downloadIsDone) {
	//				FinishDownload ();
	//				done = true;
	//			}

	//			return !done;
	//		}

	//		public override bool IsDone ()
	//		{
	//			return done;
	//		}

	//		public abstract string GetSourceURL ();

	//		public AssetBundleDownloadOperation (string assetBundleName)
	//		{
	//			this.assetBundleName = assetBundleName;
	//		}
	//	}


	//	public class AssetBundleDownloadFromWebOperation : AssetBundleDownloadOperation
	//	{
	//		WWW m_WWW;
	//		string m_Url;

	//		public AssetBundleDownloadFromWebOperation (string assetBundleName, WWW www)
	//			: base (assetBundleName)
	//		{
	//			if (www == null)
	//				throw new System.ArgumentNullException ("www");
	//			m_Url = www.url;
	//			this.m_WWW = www;
	//		}

	//		protected override bool downloadIsDone { get { return (m_WWW == null) || m_WWW.isDone; } }

	//		protected override void FinishDownload ()
	//		{
	//			error = m_WWW.error;
	//			if (!string.IsNullOrEmpty (error))
	//				return;

	//			AssetBundle bundle = m_WWW.assetBundle;
	//			if (bundle == null)
	//				error = string.Format ("{0} is not a valid asset bundle.", assetBundleName);
	//			else
	//				assetBundle = new LoadedAssetBundle (m_WWW.assetBundle);

	//			m_WWW.Dispose ();
	//			m_WWW = null;
	//		}

	//		public override string GetSourceURL ()
	//		{
	//			return m_Url;
	//		}
	//	}

	//#if UNITY_EDITOR
	//	public class AssetBundleLoadLevelSimulationOperation : AssetBundleLoadOperation
	//	{
	//		AsyncOperation m_Operation = null;

	//		public AssetBundleLoadLevelSimulationOperation (string assetBundleName, string levelName, bool isAdditive)
	//		{
	//			string [] levelPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName (assetBundleName, levelName);
	//			if (levelPaths.Length == 0) {
	//				///@TODO: The error needs to differentiate that an asset bundle name doesn't exist
	//				//        from that there right scene does not exist in the asset bundle...

	//				Debug.LogError ("There is no scene with name \"" + levelName + "\" in " + assetBundleName);
	//				return;
	//			}

	//			if (isAdditive)
	//				m_Operation = UnityEditor.EditorApplication.LoadLevelAdditiveAsyncInPlayMode (levelPaths [0]);
	//			else
	//				m_Operation = UnityEditor.EditorApplication.LoadLevelAsyncInPlayMode (levelPaths [0]);
	//		}

	//		public override bool Update ()
	//		{
	//			return false;
	//		}

	//		public override bool IsDone ()
	//		{
	//			return m_Operation == null || m_Operation.isDone;
	//		}
	//	}
	//#endif

	//	public class AssetBundleLoadLevelOperation : AssetBundleLoadOperation
	//	{
	//		protected string m_AssetBundleName;
	//		protected string m_LevelName;
	//		protected bool m_IsAdditive;
	//		protected string m_DownloadingError;
	//		protected AsyncOperation m_Request;

	//		public AssetBundleLoadLevelOperation (string assetbundleName, string levelName, bool isAdditive)
	//		{
	//			m_AssetBundleName = assetbundleName;
	//			m_LevelName = levelName;
	//			m_IsAdditive = isAdditive;
	//		}

	//		public override bool Update ()
	//		{
	//			if (m_Request != null)
	//				return false;

	//			LoadedAssetBundle bundle = AssetBundleLoad.GetLoadedAssetBundle (m_AssetBundleName, out m_DownloadingError);
	//			if (bundle != null) {
	//#if UNITY_5_3 || UNITY_5_4
	//                m_Request = SceneManager.LoadSceneAsync(m_LevelName, m_IsAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
	//#else
	//				if (m_IsAdditive) {
	//					m_Request = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (m_LevelName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
	//					//m_Request = Application.LoadLevelAdditiveAsync (m_LevelName);

	//				} else {
	//					//m_Request = Application.LoadLevelAsync (m_LevelName);
	//					m_Request = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (m_LevelName, UnityEngine.SceneManagement.LoadSceneMode.Single);

	//				}
	//#endif
	//			return false;
	//		} else
	//			return true;
	//	}

	//	public override bool IsDone ()
	//	{
	//		// Return if meeting downloading error.
	//		// m_DownloadingError might come from the dependency downloading.
	//		if (m_Request == null && m_DownloadingError != null) {
	//			Debug.LogError (m_DownloadingError);
	//			return true;
	//		}

	//		return m_Request != null && m_Request.isDone;
	//	}
	//}
	//public abstract class AssetBundleLoadAssetOperation : AssetBundleLoadOperation
	//{
	//	public abstract T GetAsset<T> () where T : UnityEngine.Object;
	//}

	//public class AssetBundleLoadAssetOperationSimulation : AssetBundleLoadAssetOperation
	//{
	//	Object m_SimulatedObject;

	//	public AssetBundleLoadAssetOperationSimulation (Object simulatedObject)
	//	{
	//		m_SimulatedObject = simulatedObject;
	//	}

	//	public override T GetAsset<T> ()
	//	{
	//		return m_SimulatedObject as T;
	//	}

	//	public override bool Update ()
	//	{
	//		return false;
	//	}

	//	public override bool IsDone ()
	//	{
	//		return true;
	//	}
	//}
	//public class AssetBundleLoadAssetOperationFull : AssetBundleLoadAssetOperation
	//{
	//	protected string m_AssetBundleName;
	//	protected string m_AssetName;
	//	protected string m_DownloadingError;
	//	protected System.Type m_Type;
	//	protected AssetBundleRequest m_Request = null;

	//	public AssetBundleLoadAssetOperationFull (string bundleName, string assetName, System.Type type)
	//	{
	//		m_AssetBundleName = bundleName;
	//		m_AssetName = assetName;
	//		m_Type = type;
	//	}

	//	public override T GetAsset<T> ()
	//	{
	//		if (m_Request != null && m_Request.isDone)
	//			return m_Request.asset as T;
	//		else
	//			return null;
	//	}

	//	// Returns true if more Update calls are required.
	//	public override bool Update ()
	//	{
	//		if (m_Request != null)
	//			return false;

	//		LoadedAssetBundle bundle = AssetBundleLoad.GetLoadedAssetBundle (m_AssetBundleName, out m_DownloadingError);
	//		if (bundle != null) {
	//			///@TODO: When asset bundle download fails this throws an exception...
	//			m_Request = bundle.m_AssetBundle.LoadAssetAsync (m_AssetName, m_Type);
	//			return false;
	//		} else {
	//			return true;
	//		}
	//	}

	//	public override bool IsDone ()
	//	{
	//		// Return if meeting downloading error.
	//		// m_DownloadingError might come from the dependency downloading.
	//		if (m_Request == null && m_DownloadingError != null) {
	//			Debug.LogError (m_DownloadingError);
	//			return true;
	//		}

	//		return m_Request != null && m_Request.isDone;
	//	}
	//}
	//public class AssetBundleLoadManifestOperation : AssetBundleLoadAssetOperationFull
	//{
	//	public AssetBundleLoadManifestOperation (string bundleName, string assetName, System.Type type)
	//		: base (bundleName, assetName, type)
	//	{
	//	}

	//	public override bool Update ()
	//	{
	//		base.Update ();

	//		if (m_Request != null && m_Request.isDone) {
	//			AssetBundleLoad.AssetBundleManifestObject = GetAsset<AssetBundleManifest> ();
	//			return false;
	//		} else
	//			return true;
	//	}
	//}



}
