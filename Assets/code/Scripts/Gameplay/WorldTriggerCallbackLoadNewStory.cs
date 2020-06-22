using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Subsystems.Story;
using StarPlatinum;
using StarPlatinum.Utility;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;

namespace GamePlay.EventTrigger
{
	public class WorldTriggerCallbackLoadNewStory : WorldTriggerCallbackBase
    {
		public string m_newStoryFileName;
		public bool m_showStoryPanel = true;
		[ConditionalField ("m_showStoryPanel")]
		public string m_jumpToLabel;
		public bool m_destoryAfterTrigger = false;
        
        protected override void Callback()
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
				UI.UIManager.Instance ().ShowStaticPanel (UIPanelType.TalkPanel, new TalkDataProvider () { ID = m_jumpToLabel });
			}

			if (m_destoryAfterTrigger) {
				Destroy (gameObject);
			}
		}
    }
}