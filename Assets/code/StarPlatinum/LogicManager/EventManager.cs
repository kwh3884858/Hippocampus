using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using StarPlatinum.Base;

namespace StarPlatinum.EventManager
{
	/// <summary>
	/// 逻辑类型，用于EventManager
	/// </summary>
	public enum LogicType
	{
		//TODO
	}

	public class EventManager : Singleton<EventManager>
	{
		//LogicBase m_currentLogic;
		private Dictionary<Type, Delegate> m_allEvent = new Dictionary<Type, Delegate> ();
		private Dictionary<EventKey, Action<object>> m_allStrEvent = new Dictionary<EventKey, Action<object>>();
		
		public void AddEventListener<T> (EventHandler<T> handler) where T : EventArgs
		{
			Delegate d;
			if (m_allEvent.TryGetValue (typeof (T), out d)) {
				m_allEvent [typeof (T)] = Delegate.Combine (d, handler);
			} else {
				m_allEvent [typeof (T)] = handler;
			}
		}
		
		public void AddEventListener(EventKey eventKey,Action<object> handler)
		{
			if (m_allStrEvent.ContainsKey(eventKey))
			{
				m_allStrEvent[eventKey] +=  handler;
			}
			else
			{
				m_allStrEvent[eventKey] = handler;
			}

		}
		
		public void RemoveEventListener<T> (EventHandler<T> handler) where T : EventArgs
		{
			Delegate d;
			if (m_allEvent.TryGetValue (typeof (T), out d)) {
				Delegate currentDel = Delegate.Remove (d, handler);

				if (currentDel == null) {
					m_allEvent.Remove (typeof (T));
				} else {
					m_allEvent [typeof (T)] = currentDel;
				}
			}
		}
		
		public void RemoveEventListener(EventKey eventKey,Action<object> handler)
		{
			if (m_allStrEvent.ContainsKey(eventKey)&&Array.IndexOf(m_allStrEvent[eventKey].GetInvocationList(),handler)>=0)
			{
				// ReSharper disable once DelegateSubtraction
				m_allStrEvent[eventKey] -= handler;
			}
		}

		//Examlpe:
		//EventManager.Instance.SendEvent<SceneLoadedEvent>(new SceneLoadedEvent (sceneName));

		public void SendEvent<T> (T message) where T : EventArgs
		{
			if (message == null) {
				throw new ArgumentNullException ("e");
			}
			Delegate d;
			if (m_allEvent.TryGetValue (typeof (T), out d)) {
				(d as EventHandler<T>)?.Invoke (this, message);
			}
		}
		
		public void SendEvent (EventKey eventKey,object data =null)
		{
			Action<object> action;
			if (m_allStrEvent.TryGetValue (eventKey, out action)) {
				action?.Invoke(data);
			}
		}
	}

}