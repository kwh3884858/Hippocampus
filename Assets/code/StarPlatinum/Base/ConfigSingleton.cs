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

		public static void Preload ()
		{
			LoadConfig ();
		}

		private static T LoadConfig ()
		{
			m_instance = null;

			//Is loading
			if (m_isLoading) {
				return null;
			}

			if (Application.isPlaying) {
				m_isLoading = true;
				PrefabManager.Instance.InstantiateConfigAsync (typeof (T).Name, (result) => {
					if (result.status == RequestStatus.FAIL) {
						return;
					}
					Debug.Log ($"===========Aas:{result.key}加载完成,");
					m_instance = result.result as T;
				});
				m_isLoading = false;
				return m_instance;
			} else
			{
#if UNITY_EDITOR
				m_isLoading = true;
				m_instance = AssetDatabase.LoadAssetAtPath<T> (LOADPATH);
				if (m_instance == null) { Debug.LogError ($"{LOADPATH} doesn`t exist {typeof (T).Name}"); return null; }

				Debug.Log ($"======Resource加载完成name:{typeof (T).Name}, path:{LOADPATH}, config:{m_instance}=====");
				m_isLoading = false;
				return m_instance;
#endif
			}
			return null;
		}

		private static T m_instance = null;
		private static bool m_isLoading = false;
	}

}