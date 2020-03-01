using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Subsystems.Story;
using StarPlatinum;
using StarPlatinum.Utility;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;

namespace GamePlay
{
	public class WorldTriggerCallbackLoadNewStory : MonoBehaviour
	{
		[SerializeField]
		private WorldTrigger m_worldTrigger;

		public string m_newStoryFileName;

		public bool m_showStoryPanel = false;

		[ConditionalField ("m_showStoryPanel")]
		public string m_jumpToLabel;

		// Start is called before the first frame update
		void Start ()
		{
			m_showStoryPanel = false;

			if (m_worldTrigger == null) {

				m_worldTrigger = GetComponent<WorldTrigger> ();
				if (m_worldTrigger == null) {
					gameObject.AddComponent<WorldTrigger> ();
				}

			}

			m_worldTrigger.Callback = LoadNewStory;
		}

		private void LoadNewStory ()
		{
			GameObject controller = GameObject.Find ("ControllerManager");
			if (controller == null) {
				return;
			}

			StoryController storyController = controller.GetComponent<StoryController> ();
			if (storyController == null) {
				return;
			}

			storyController.LoadStoryFileByName (m_newStoryFileName);

			if (m_showStoryPanel) {
				UI.UIManager.Instance ().ShowPanel (UIPanelType.TalkPanel, new TalkDataProvider () { ID = m_jumpToLabel });

			}
		}
	}
}