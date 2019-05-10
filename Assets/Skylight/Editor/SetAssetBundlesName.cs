using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
namespace Skylight
{


	public class SetAssetBundlesName : Editor
	{
		//此处添加需要命名的资源后缀名,注意大小写。
		static readonly string [] Filtersuffix = { ".mat", ".dds", ".prefab", ".unity" };

		private static void SetAssetBundleName (string fullPath, string [] suffixs)
		{


			if (Directory.Exists (fullPath)) {
				DirectoryInfo dir = new DirectoryInfo (fullPath);

				FileInfo [] files = dir.GetFiles ("*", SearchOption.AllDirectories);
				for (var i = 0; i < files.Length; ++i) {
					FileInfo fileInfo = files [i];
					EditorUtility.DisplayProgressBar ("设置AssetName名称", "正在设置AssetName名称中...", 1f * i / files.Length);
					foreach (string suffix in suffixs) {

						if (fileInfo.Name.EndsWith (suffix)) {
							string path = fileInfo.FullName.Replace ('\\', '/').Substring (AssetsManager.PROJECT_PATH.Length);
							AssetImporter importer = AssetImporter.GetAtPath (path);
							if (importer) {
								string name = path.Substring (7);
								importer.assetBundleName = name.Substring (0, name.LastIndexOf ('.')) + AssetsManager.SUFFIX;
							}
						}
					}
				}
				AssetDatabase.RemoveUnusedAssetBundleNames ();
			}
			EditorUtility.ClearProgressBar ();
			AssetDatabase.Refresh ();
		}

		[MenuItem ("Assets/AssetBundles/SetSelectedAssetBundleName")]
		static void SetSelectedAssetBundleName ()
		{
			//获取选中文件
			UnityEngine.Object [] SelectedAsset = Selection.GetFiltered (typeof (Object),
								SelectionMode.Assets | SelectionMode.ExcludePrefab);

			if (SelectedAsset.Length != 1) return;
			string fullPath = AssetsManager.PROJECT_PATH + AssetDatabase.GetAssetPath (SelectedAsset [0]);

			SetAssetBundleName (fullPath, Filtersuffix);

		}

		[MenuItem ("Assets/AssetBundles/SetAllAssetsBundleName")]
		static void SetAllAssetsBundleName ()
		{
			//此处添加需要统一打包的文件夹
			string [] bundlesDirectory = { "UI", "Prefabs", "Scenes", "Models" };


			for (int i = 0; i < bundlesDirectory.Length; i++) {
				string fullpath = AssetsManager.APPLICATION_PATH + "/" + bundlesDirectory [i];

				SetAssetBundleName (fullpath, Filtersuffix);


			}
		}



		[MenuItem ("Assets/AssetBundles/ShowAllAssetBundleName")]
		static void ShowAllAssetBundleName ()
		{

			string [] names = AssetDatabase.GetAllAssetBundleNames ();

			foreach (var name in names) {
				Debug.Log (name);
			}

		}

		[MenuItem ("Assets/AssetBundles/ClearAssetBundleName")]
		static void ClearAssetBundleName ()
		{
			UnityEngine.Object [] SelectedAsset = Selection.GetFiltered (typeof (Object),
									SelectionMode.Assets | SelectionMode.ExcludePrefab);
			//此处添加需要命名的资源后缀名,注意大小写。
			//string [] Filtersuffix = new string [] { ".prefab", ".mat", ".dds" };

			if (!(SelectedAsset.Length == 1)) return;

			string fullPath = AssetsManager.PROJECT_PATH + AssetDatabase.GetAssetPath (SelectedAsset [0]);

			if (Directory.Exists (fullPath)) {
				DirectoryInfo dir = new DirectoryInfo (fullPath);

				var files = dir.GetFiles ("*", SearchOption.AllDirectories);
				;
				for (var i = 0; i < files.Length; ++i) {
					var fileInfo = files [i];
					EditorUtility.DisplayProgressBar ("清除AssetName名称",
					"正在清除AssetName名称中...", 1f * i / files.Length);
					foreach (string suffix in Filtersuffix) {
						if (fileInfo.Name.EndsWith (suffix)) {
							string path = fileInfo.FullName.Replace ('\\', '/').Substring (AssetsManager.PROJECT_PATH.Length);
							var importer = AssetImporter.GetAtPath (path);
							if (importer) {
								importer.assetBundleName = null;
							}
						}
					}
				}
			}
			EditorUtility.ClearProgressBar ();
			AssetDatabase.RemoveUnusedAssetBundleNames ();
		}


	}
}