using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Subsystems.Story;
using GamePlay.Global;
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
        [ConditionalField("m_showStoryPanel")]
        public string m_jumpToLabel = "";
        public bool m_onlyTiriggerOnce = true;

        protected override void AfterStart()
        {
            if (m_onlyTiriggerOnce)
            {
                CheckJumpLabel();
                if (SingletonGlobalDataContainer.Instance.IsStoryTriggered(m_jumpToLabel))
                {
                    Destroy(gameObject);
                }
            }
        }

        protected override void Callback()
        {
            if (m_newStoryFileName == "")
            {
                Debug.LogError("New Story File Name is empty");
                return;
            }
            GameObject controller = GameObject.Find("ControllerManager");
            if (controller == null)
            {
                return;
            }

            StoryController storyController = controller.GetComponent<StoryController>();
            if (storyController == null)
            {
                return;
            }

            storyController.LoadStoryFileByName(m_newStoryFileName);

            if (m_showStoryPanel)
            {
                CheckJumpLabel();
                UI.UIManager.Instance().ShowStaticPanel(UIPanelType.TalkPanel, new TalkDataProvider() { ID = m_jumpToLabel });
            }

            if (m_onlyTiriggerOnce)
            {
                SingletonGlobalDataContainer.Instance.AddtTriggeredStory(m_jumpToLabel);
                Destroy(gameObject);
            }
        }

        private void CheckJumpLabel()
        {
            if (m_jumpToLabel == "")
            {
                m_jumpToLabel = m_newStoryFileName;
            }
        }
    }
}