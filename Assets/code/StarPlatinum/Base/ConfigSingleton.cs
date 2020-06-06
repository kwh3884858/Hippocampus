#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;


namespace StarPlatinum.Base
{

	public abstract class ConfigSingleton<T> : ScriptableObject where T : ScriptableObject
	{
		private static readonly string LOADPATH = $"Assets/code/Config/{typeof (T).Name}.asset";

		public static ref T Instance {
			get {
				if (m_instance == null) {
					m_instance = LoadConfig ();
				}

				return ref m_instance;
			}
		}

		private static T LoadConfig ()
		{
			m_instance = null;

			if (Application.isPlaying) {
				PrefabManager.Instance.InstantiateConfigAsync (typeof (T).Name, (result) => {
					Debug.Log ($"===========Aas:{result.key}加载完成,");
					m_instance = result.result as T;
				});

				return m_instance;
			} else {
#if UNITY_EDITOR
				//var path = $"Assets/code/StarPlatinum/Config/{typeof(T).Name}.asset";

				m_instance = AssetDatabase.LoadAssetAtPath<T> (LOADPATH);
				if (m_instance == null) { Debug.LogError ($"{LOADPATH} doesn`t exist {typeof (T).Name}"); return null; }

				Debug.Log ($"======Resource加载完成name:{typeof (T).Name}, path:{LOADPATH}, config:{m_instance}=====");

				return m_instance;
#endif
			}
			return null;
		}

		private static T m_instance = null;
	}

}