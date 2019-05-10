using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Skylight
{
	public static class CreateAssetBundles
	{

#if UNITY_WEBPLAYER
         static   BuildTarget target = BuildTarget.WebPlayer;
#elif UNITY_STANDALONE_WIN && UNITY_EDITOR
        static    BuildTarget target = BuildTarget.StandaloneWindows;
#elif UNITY_ANDROID
        static BuildTarget target = BuildTarget.Android;
#elif UNITY_IPHONE
          static  BuildTarget target = BuildTarget.iPhone;
#elif UNITY_STANDALONE_OSX
		static BuildTarget target = BuildTarget.StandaloneOSX;
#endif


		[MenuItem ("Assets/AssetBundles/BuildAssetBundles")]
		public static void BuildAllAssetBundles ()
		{
			string assetBundleDirectory = "Assets/StreamingAssets/AssetBundle/" + AssetsUtility.GetPlatformName () + "/";

			//	BuildPipeline.BuildAssetBundles (assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);

			if (!Directory.Exists (assetBundleDirectory)) {
				Directory.CreateDirectory (assetBundleDirectory);
			}


			//第一个参数获取的是AssetBundle存放的相对地址。
			BuildPipeline.BuildAssetBundles (
				assetBundleDirectory,
				BuildAssetBundleOptions.ChunkBasedCompression |
				BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle,
				 target);

			AssetDatabase.Refresh ();
			//BuildAssetBundleOptions.ForceRebuildAssetBundle

		}
	}
}
