
using StarPlatinum;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Base;

namespace GamePlay.Global
{
	public class SingletonGlobalDataContainer : Singleton<SingletonGlobalDataContainer>
	{
		//public string Scene { get; private set; }
		//public string Chapter { get; private set; }
		public List<string> m_isStoryTriggered = new List<string> ();

		public Dictionary<string, int> m_objectTriggeredCounter = new Dictionary<string, int> ();

		//public void SetScene (string scene)
		//{
		//	Scene = scene;
		//}
		//public void SetChapter (string chapter)
		//{
		//	Chapter = chapter;
		//}
		//public void ClearItem
		public bool RegisterNewObject (string item)
		{
			int tmp;
			if (m_objectTriggeredCounter.TryGetValue (item, out tmp)) {
				return false;
			}
			m_objectTriggeredCounter.Add (item, 0);
			return true;
		}

		public bool GetObjectCounter (string gameObjectName, out int outValue)
		{
			return m_objectTriggeredCounter.TryGetValue (gameObjectName, out outValue);
		}

		public bool ModifyCounterValue (string gameObjectName, int modifiedValue)
		{
			int tmp;
			if (m_objectTriggeredCounter.TryGetValue (gameObjectName, out tmp)) {
				m_objectTriggeredCounter [gameObjectName] += modifiedValue;
				if (m_objectTriggeredCounter [gameObjectName] < 0) {
					m_objectTriggeredCounter [gameObjectName] = 0;
				}
				return true;
			} else {
				return false;
			}
		}

		//public bool TriggeredItemGenerateID (string item, out string ID)
		//{
		//	int counter;
		//	if (!m_itemTriggeredCounter.TryGetValue (item, out counter)) {
		//		ID = "ERROR, NOT REGISTERED ITEM";
		//		return false;
		//	}

		//	ID = Chapter + "_" + Scene + "_" + item + "_" + counter;
		//	return true;
		//}

		public bool IsStoryTriggered (string storyId)
		{
			return m_isStoryTriggered.Contains (storyId);
		}

		public void AddtTriggeredStory (string storyId)
		{
			m_isStoryTriggered.Add (storyId);
		}
	}
}