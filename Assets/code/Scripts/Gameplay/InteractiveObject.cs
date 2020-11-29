using Controllers.Subsystems.Story;
using GamePlay.Global;
using StarPlatinum;
using System.Collections;
using System.Collections.Generic;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;
using StarPlatinum.EventManager;
using System;
using StarPlatinum.Utility;
using GamePlay.EventTrigger;
using GamePlay.Stage;
using Evidence;

namespace GamePlay
{
    public class InteractiveObject : MonoBehaviour
    {
        public static readonly string INTERACTABLE_TAG = "Interactive";

        [Header("Triggered Story and Label")]
        public string m_newStoryFileName = "";
        public string m_objectName = "";

        [ConditionalField("m_addtionalExhibitTest", true)]
        public bool m_onlyTriggerOnce = false;

        [Header("UI")]
        public string m_UIDisplayName = "";

        [Header("Exhibit Test")]
        public bool m_addtionalExhibitTest = false;

        [ConditionalField("m_addtionalExhibitTest", false)]
        [Header("Only For OnlyTriggerOnce")]
        public List<string> m_mustHaveExhibit = new List<string>();

        [ConditionalField("m_addtionalExhibitTest", false)]
        public string m_testFailureStoryFile = "";

        [ConditionalField("m_addtionalExhibitTest", false)]
        public string m_testFailureStoryLabel = "";

        public void Start()
        {
            if (tag != INTERACTABLE_TAG) {
                tag = INTERACTABLE_TAG;
            }

            if (m_objectName != null || m_objectName != "") {
                bool result = SingletonGlobalDataContainer.Instance.RegisterNewObject(m_objectName);
                //if (result == false) {
                //	Debug.LogError ("Global data container alraedy contain " + m_objectName);
                //}
            } else {
                Debug.LogError("Interactive object Doesn`t have name");
            }

            if (m_addtionalExhibitTest)
            {
                m_onlyTriggerOnce = false;
            }

            if (m_onlyTriggerOnce)
            {
                if (SingletonGlobalDataContainer.Instance.IsStoryTriggered(m_objectName))
                {
                    Destroy(gameObject);
                }
            }

            EventManager.Instance.AddEventListener<PlayInteractionAnimationEvent>(EventHandle);
        }

        private void EventHandle(object sender, PlayInteractionAnimationEvent e)
        {
            string cleanItemName = m_objectName;
            if (cleanItemName.Contains("_"))
            {
                cleanItemName = cleanItemName.Substring(0, cleanItemName.IndexOf('_'));
            }

            if (e.m_itemName == cleanItemName)
            {
                Animation animation = GetComponent<Animation>();
                if (animation != null)
                {
                    animation.Play();
                }
            }
        }

        public string GetUIDisplayName() => m_UIDisplayName;

        public void Interact()
        {
            GameObject controller = GameObject.Find("ControllerManager");
            if (controller == null) {
                return;
            }

            StoryController storyController = controller.GetComponent<StoryController>();
            if (storyController == null) {
                return;
            }

            if (m_objectName == null || m_objectName == "") {
                Debug.LogWarning("Object Name is empty");
                return;
            }

            if (m_addtionalExhibitTest && m_testFailureStoryLabel == "")
            {
                Debug.LogWarning("Failure Story Label is empty");
                return;
            }

            if (m_addtionalExhibitTest)
            {
                if (m_testFailureStoryFile == null || m_testFailureStoryFile == "")
                {
                    Debug.LogWarning("Failure Story File Name is empty");
                }
                else
                {
                    storyController.LoadStoryFileByName(m_testFailureStoryFile);
                }
            }

            if (m_newStoryFileName == null || m_newStoryFileName == "")
            {
                Debug.LogWarning("New Story File Name is empty");
            }
            else
            {
                storyController.LoadStoryFileByName(m_newStoryFileName);
            }

            bool isPassExhibitTest = true;
            if (m_addtionalExhibitTest)
            {

                foreach (string item in m_mustHaveExhibit)
                {
                    if (!EvidenceDataManager.Instance.IsEvidenceExist(item))
                    {
                        isPassExhibitTest = false;
                    }
                }

                if (!isPassExhibitTest)
                {
                    // Test fail
                    if (storyController.IsLabelExist(m_testFailureStoryLabel))
                    {
                        UI.UIManager.Instance().ShowStaticPanel(UIPanelType.TalkPanel, new TalkDataProvider() { ID = m_testFailureStoryLabel });
                    }
                }
            }

            if ((m_addtionalExhibitTest && isPassExhibitTest) || !m_addtionalExhibitTest)
            {
                //bool result = storyController.LoadStoryByItem (m_objectName);
                if (!m_objectName.Contains("_"))
                {
                    int outCounterValue = -1;
                    bool result = SingletonGlobalDataContainer.Instance.GetObjectCounter(m_objectName, out outCounterValue);
                    if (result == false)
                    {
                        return;
                    }
                    if (outCounterValue == -1)
                    {
                        return;
                    }
                    string jumpLabel = m_objectName + "_" + outCounterValue;
                    if (storyController.IsLabelExist(jumpLabel))
                    {
                        UI.UIManager.Instance().ShowStaticPanel(UIPanelType.TalkPanel, new TalkDataProvider() { ID = jumpLabel });
                        SingletonGlobalDataContainer.Instance.ModifyCounterValue(m_objectName, 1);
                    }
                    else
                    {
                        SingletonGlobalDataContainer.Instance.ModifyCounterValue(m_objectName, -1);
                        SingletonGlobalDataContainer.Instance.GetObjectCounter(m_objectName, out outCounterValue);

                        jumpLabel = m_objectName + "_" + outCounterValue;
                        UI.UIManager.Instance().ShowStaticPanel(UIPanelType.TalkPanel, new TalkDataProvider() { ID = jumpLabel });
                    }
                }
                else
                {
                    UI.UIManager.Instance().ShowStaticPanel(UIPanelType.TalkPanel, new TalkDataProvider() { ID = m_objectName });
                }
            }


			if (m_onlyTriggerOnce || (m_addtionalExhibitTest && isPassExhibitTest))
            {
                SingletonGlobalDataContainer.Instance.AddtTriggeredStory(m_objectName);
                Destroy(gameObject);
            }

        }
	}
}
