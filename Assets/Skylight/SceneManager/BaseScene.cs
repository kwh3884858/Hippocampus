using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

namespace Skylight
{
	public class BaseScene : MonoBehaviour
	{
		public string m_sceneName;

		// Use this for initialization
		public object m_sceneData = null;


		//public event EventHandler<ShowSceneEvent> m_sceneEvents;

		void Start ()
		{
		}

		public void AddGameObject<T> () where T : MonoBehaviour
		{
			GameObject go = new GameObject ();
			go.name = typeof (T).ToString ();
			go.AddComponent<T> ();
			go.transform.SetParent (transform);
		}

		public virtual void SceneInit (string sceneName)
		{
			m_sceneName = sceneName;
			//Regist Event
			EventManager.Instance ().AddEventListener<SceneLoadedEvent> (SceneLoaded);
			EventManager.Instance ().AddEventListener<SceneEnterEvent> (SceneEnter);
			EventManager.Instance ().AddEventListener<SceneLeaveEvent> (SceneLeave);

			//EventManager.Instance ().Notify ((iwwqnt)LogicType.SceneInit);
		}
		public virtual void SceneLoaded (object sender, SceneLoadedEvent showSceneEvent)
		{

		}
		public virtual void SceneEnter (object sender, SceneEnterEvent showSceneEvent)
		{

		}

		public virtual void SceneLeave (object sender, SceneLeaveEvent showSceneEvent)
		{

		}
		public virtual void SceneClose ()
		{
			//EventManager.Instance ().Notify ((int)LogicType.SceneClose);

		}
		public virtual void SceneDestory ()
		{

		}

		//protected virtual void OnShowScene (ShowSceneEvent e)
		//{
		//	EventHandler<ShowSceneEvent> temp = m_sceneEvents;
		//	if (temp != null) temp (this, e);
		//}
	}
}
