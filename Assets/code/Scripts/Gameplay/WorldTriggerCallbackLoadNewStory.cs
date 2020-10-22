using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Subsystems.Story;
using GamePlay.Global;
using GamePlay.Stage;
using StarPlatinum;
using StarPlatinum.Utility;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;

namespace GamePlay.EventTrigger
{
	public class WorldTriggerCallbackLoadNewStory : WorldTriggerCallbackBase
	{
		public string m_newStoryFileName = "";
		public bool m_showStoryPanel = true;
		[ConditionalField ("m_showStoryPanel")]
		public string m_jumpToLabel = "";
		public bool m_onlyTiriggerOnce = true;

		[ConditionalField("m_onlyTiriggerOnce", true)]
        [Header("When the player triggered a reapted story, he should be re-located to a place away from the trigger.")]
        public WorldTriggerCallbackTeleportPlayer.Direction m_spawnDirection = WorldTriggerCallbackTeleportPlayer.Direction.Right;
		[ConditionalField("m_onlyTiriggerOnce", true)]
		public float m_lengthBetweenTriggerAndSpwanPoint = 6.0f;

        protected override void AfterStart ()
		{
			if (m_onlyTiriggerOnce) {
				CheckJumpLabel ();
				if (SingletonGlobalDataContainer.Instance.IsStoryTriggered (m_jumpToLabel)) {
					Destroy (gameObject);
				}
			}
		}

		protected override void Callback ()
		{

			GameObject controller = GameObject.Find ("ControllerManager");
			if (controller == null) {
				return;
			}
			StoryController storyController = controller.GetComponent<StoryController> ();
			if (storyController == null) {
				return;
			}
			if (m_newStoryFileName == "") {
				Debug.LogWarning ("New Story File Name is empty");
			} else {
				storyController.LoadStoryFileByName (m_newStoryFileName);
			}
			if (m_showStoryPanel) {
				CheckJumpLabel ();
				if (!m_onlyTiriggerOnce && !m_jumpToLabel.Contains ("_")) {
					//Need auto generate story
					string jumpLabel = m_jumpToLabel + "_" + m_triggerCounter;
					if (storyController.IsLabelExist (jumpLabel)) {
						UI.UIManager.Instance ().ShowStaticPanel (UIPanelType.TalkPanel, new TalkDataProvider () { ID = jumpLabel });
						m_triggerCounter++;
					} else {
						m_triggerCounter = --m_triggerCounter < 0 ? 0 : m_triggerCounter;
						jumpLabel = m_jumpToLabel + "_" + m_triggerCounter;
						UI.UIManager.Instance ().ShowStaticPanel (UIPanelType.TalkPanel, new TalkDataProvider () { ID = jumpLabel });
					}
				} else {
					UI.UIManager.Instance ().ShowStaticPanel (UIPanelType.TalkPanel, new TalkDataProvider () { ID = m_jumpToLabel });
				}
			}

			if (m_onlyTiriggerOnce) {
				SingletonGlobalDataContainer.Instance.AddtTriggeredStory (m_jumpToLabel);
				Destroy (gameObject);
            }
            else
            {
				CoreContainer.Instance.SetPlayerPosition(transform.position + WorldTriggerCallbackTeleportPlayer.DirecitonMapping[m_spawnDirection] * m_lengthBetweenTriggerAndSpwanPoint + new Vector3(0.0f, 0.5f, 0.0f));
			}
		}

		private void CheckJumpLabel ()
		{
			if (m_jumpToLabel == "") {
				m_jumpToLabel = m_newStoryFileName;
			}
		}

		private int m_triggerCounter = 0;
	}
}