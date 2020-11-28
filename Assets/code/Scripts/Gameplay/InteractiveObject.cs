using Controllers.Subsystems.Story;
using GamePlay.Global;
using StarPlatinum;
using System.Collections;
using System.Collections.Generic;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;

namespace GamePlay
{
	public class InteractiveObject : MonoBehaviour
	{
		public static readonly string INTERACTABLE_TAG = "Interactive";

		public string m_newStoryFileName = "";
		public string m_objectName = "";

		public bool m_onlyTriggerOnce = false;

		[Header("UI")]
		public string m_UIDisplayName = "";
		
		[Header("Animation")]
		public GameObject m_needPlayAnimationObject = null;

		public void Start ()
		{
			if (tag != INTERACTABLE_TAG) {
				tag = INTERACTABLE_TAG;
			}

			if (m_objectName != null || m_objectName != "") {
				bool result = SingletonGlobalDataContainer.Instance.RegisterNewObject (m_objectName);
				//if (result == false) {
				//	Debug.LogError ("Global data container alraedy contain " + m_objectName);
				//}
			} else {
				Debug.LogError ("Interactive object Doesn`t have name");
			}
		}

		public void Interact ()
		{
			GameObject controller = GameObject.Find ("ControllerManager");
			if (controller == null) {
				return;
			}

			StoryController storyController = controller.GetComponent<StoryController> ();
			if (storyController == null) {
				return;
			}

			if (m_objectName == null || m_objectName == "") {
				return;
			}

            if (m_newStoryFileName == null || m_newStoryFileName == "")
            {
                Debug.LogWarning("New Story File Name is empty");
            }
            else
            {
                storyController.LoadStoryFileByName(m_newStoryFileName);
            }

            //bool result = storyController.LoadStoryByItem (m_objectName);
            if (!m_objectName.Contains ("_")) {
				int outCounterValue = -1;
				bool result = SingletonGlobalDataContainer.Instance.GetObjectCounter (m_objectName, out outCounterValue);
				if (result == false) {
					return;
				}
				if (outCounterValue == -1) {
					return;
				}
				string jumpLabel = m_objectName + "_" + outCounterValue;
				if (storyController.IsLabelExist (jumpLabel)) {
					UI.UIManager.Instance ().ShowStaticPanel (UIPanelType.TalkPanel, new TalkDataProvider () { ID = jumpLabel });
					SingletonGlobalDataContainer.Instance.ModifyCounterValue (m_objectName, 1);
				} else {
					SingletonGlobalDataContainer.Instance.ModifyCounterValue (m_objectName, -1);
					SingletonGlobalDataContainer.Instance.GetObjectCounter (m_objectName, out outCounterValue);

					jumpLabel = m_objectName + "_" + outCounterValue;
					UI.UIManager.Instance ().ShowStaticPanel (UIPanelType.TalkPanel, new TalkDataProvider () { ID = jumpLabel });
				}
			} else {
				UI.UIManager.Instance ().ShowStaticPanel (UIPanelType.TalkPanel, new TalkDataProvider () { ID = m_objectName });
			}
		}
	}
}
