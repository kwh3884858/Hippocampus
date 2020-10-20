using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace code.Scripts.Editor
{
    public class BuildTool
    {
        [MenuItem("Tools/Build/Windows")]
        public static void BuildWindows()
        {
            BuildPlayerOptions ops = new BuildPlayerOptions
            {
                locationPathName = GetBuildPath()+"Windows/Windows.exe",
                scenes = GetBuildScenes(),
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };
            AddressableAssetSettings.BuildPlayerContent();
            BuildPipeline.BuildPlayer(ops);
        }
        
        [MenuItem("Tools/Build/Android")]
        public static void BuildAndroid()
        {
            BuildPlayerOptions ops = new BuildPlayerOptions
            {
                locationPathName = GetBuildPath()+"Android/Android.apk",
                scenes = GetBuildScenes(),
                target = BuildTarget.Android,
                options = BuildOptions.None
            };
            AddressableAssetSettings.BuildPlayerContent();
            BuildPipeline.BuildPlayer(ops);
        }
        
        private static string[] GetBuildScenes()
        {
            List<string> names = new List<string>();

            foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
            {
                if (e == null)
                    continue;

                if (e.enabled)
                    names.Add(e.path);
            }
            return names.ToArray();
        }
        
        
        private static string GetBuildPath()
        {
#if UNITY_IOS
        string dirPath = Application.dataPath + "/../Build/iPhone";
#elif UNITY_STANDALONE || UNITY_ANDROID
            string dirPath = Application.dataPath + "/../Build/";
#endif
            if (!System.IO.Directory.Exists(dirPath))
            {
                System.IO.Directory.CreateDirectory(dirPath);
            }
            dirPath = Path.GetFullPath(dirPath);
            return dirPath;
        }
    }
}