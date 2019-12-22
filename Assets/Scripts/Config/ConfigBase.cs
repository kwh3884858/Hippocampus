#if UNITY_EDITOR
using UnityEditor;
#endif
using StarPlatinum;
using UnityEngine;


namespace Config
{
    public abstract class BaseConfig<T> : ScriptableObject where T : ScriptableObject
    {
        public static bool isPreloaded = false;
        public static void PreloadInGameConfig()
        {
            if (isPreloaded)
            {
                return;
            }

            isPreloaded = true;
            StoryConfig.Preload();
        }


        private static T _instance = null;

        public static T Ins
        {
            get
            {
                return _instance;
            }
        }

        public static void Preload()
        {
            if (Application.isPlaying)
            {
                PrefabManager.Instance.InstantiateAsync<ScriptableObject>(typeof(T).Name, (result)=>
                {
                    Debug.Log($"===========aas:{result.key}加载完成,");
                    _instance = result.result as T;
                });
            }
            else//Temp For AAS Not Init Success In Edit Mode
            {
#if UNITY_EDITOR
                var path = $"Assets/Config/{typeof(T).Name}.asset";
                _instance = AssetDatabase.LoadAssetAtPath<T>(path);
                Debug.Log($"===========resource加载完成name:{typeof(T).Name},path:{path},config:{_instance}");
#endif
            }
            
        }
    }
}
