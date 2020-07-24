using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StarPlatinum.EventManager
{
	public class RaiseEvent : EventArgs
	{
		public StoryReader.StoryReader.EventType m_eventType = StoryReader.StoryReader.EventType.none;
		public string m_eventName = "";

		public RaiseEvent (StoryReader.StoryReader.EventType eventType, string eventName)
		{
			m_eventType = eventType;
			m_eventName = eventName;
		}
	}

	abstract public class SceneEvent : EventArgs
	{

		private string m_sceneName;

		protected SceneEvent (string sceneName)
		{
			m_sceneName = sceneName;
		}

		public string GetSceneName ()
		{
			return m_sceneName;
		}
	}

	public class SceneLoadedEvent : SceneEvent
	{
		public SceneLoadedEvent (string sceneName) :
		 base (sceneName)
		{

		}
	}

	public class SceneEnterEvent : SceneEvent
	{
		public SceneEnterEvent (string sceneName) :
		 base (sceneName)
		{

		}
	}

	public class SceneLeaveEvent : SceneEvent
	{
		public SceneLeaveEvent (string sceneName) :
		 base (sceneName)
		{

		}
	}

	public class ButtonDownEvent : EventArgs
	{
		public string m_buttonName { get; set; }
		public ButtonDownEvent (string buttonName)
		{
			m_buttonName = buttonName;
		}
	}

	public class ButtonUpEvent : EventArgs
	{
		public string m_buttonName { get; set; }
		public ButtonUpEvent (string buttonName)
		{
			m_buttonName = buttonName;
		}
	}
}
