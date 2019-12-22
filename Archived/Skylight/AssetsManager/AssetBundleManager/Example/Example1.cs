using UnityEngine;

namespace Skylight
{
	public class Example1 : MonoBehaviour
	{
		private AssetBundleManager abm;

		private void Start ()
		{
			abm = new AssetBundleManager ();
			abm.SetPrioritizationStrategy (AssetBundleManager.PrioritizationStrategy.PrioritizeStreamingAssets);
			abm.UseSimulatedUri ();
			abm.Initialize (OnAssetBundleManagerInitialized);
		}

		private void OnAssetBundleManagerInitialized (bool success)
		{
			string levelName = "scenes/scene1.unity3d";
			if (success) {
				abm.GetBundle (levelName, OnAssetBundleDownloaded);
			} else {
				Debug.LogError ("Error initializing ABM.");
			}
			UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ("scene1", UnityEngine.SceneManagement.LoadSceneMode.Additive);
			//Application.LoadLevelAdditiveAsync (levelName);
		}

		private void OnAssetBundleDownloaded (AssetBundle bundle)
		{
			if (bundle != null) {
				// Do something with the bundle
				abm.UnloadBundle (bundle);
			}

			abm.Dispose ();
		}
	}
}