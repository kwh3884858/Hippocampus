using UnityEngine;

namespace Skylight
{
	public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
	{
		private static T m_Instance = null;

		public static T Instance ()
		{
			if (m_Instance == null) {
				m_Instance = GameObject.FindObjectOfType (typeof (T)) as T;
				if (m_Instance == null) {
					m_Instance = new GameObject (typeof (T).ToString (), typeof (T)).GetComponent<T> ();
					m_Instance.SingletonInit ();
				}
			}
			return m_Instance;
		}

		private void Awake ()
		{
			if (m_Instance == null) {
				m_Instance = this as T;
				SingletonInit ();
			}
		}

		public virtual void SingletonInit () { }

		public void AddGameObject<T2> () where T2 : MonoBehaviour
		{
			var go = new GameObject ();
			go.name = typeof (T2).ToString ();
			go.AddComponent<T2> ();
			go.transform.SetParent (transform);

		}
		public GameObject AddGameObject (string name)
		{
			var go = new GameObject ();
			go.name = name;
			go.transform.SetParent (transform);
			return go;
		}

		private void OnApplicationQuit ()
		{
			m_Instance = null;
		}
	}
}