using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StarPlatinum
{
	public class AssetsUtility
	{
		public const string AssetBundlesOutputPath = "AssetBundle";

		public static string GetPlatformName ()
		{
#if UNITY_EDITOR
			return GetPlatformForAssetBundles (EditorUserBuildSettings.activeBuildTarget);
#else
            return GetPlatformForAssetBundles(Application.platform);
#endif
		}

#if UNITY_EDITOR
		private static string GetPlatformForAssetBundles (BuildTarget target)
		{
			switch (target) {
			case BuildTarget.Android:
				return "Android";
#if UNITY_TVOS
                case BuildTarget.tvOS:
                    return "tvOS";
#endif
			case BuildTarget.iOS:
				return "iOS";
			case BuildTarget.WebGL:
				return "WebGL";
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return "StandaloneWindows";
			case BuildTarget.StandaloneOSX:

				return "OSX";
			// Add more build targets for your own.
			// If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
			default:
				return null;
			}
		}
#endif

		private static string GetPlatformForAssetBundles (RuntimePlatform platform)
		{
			switch (platform) {
			case RuntimePlatform.Android:
				return "Android";
			case RuntimePlatform.IPhonePlayer:
				return "iOS";
#if UNITY_TVOS
                case RuntimePlatform.tvOS:
                    return "tvOS";
#endif
			case RuntimePlatform.WebGLPlayer:
				return "WebGL";

			case RuntimePlatform.WindowsPlayer:
				return "StandaloneWindows";
			case RuntimePlatform.OSXPlayer:
				return "OSX";
			// Add more build targets for your own.
			// If you add more targets, don't forget to add the same platforms to GetPlatformForAssetBundles(RuntimePlatform) function.
			default:
				return null;
			}
		}
	}
}