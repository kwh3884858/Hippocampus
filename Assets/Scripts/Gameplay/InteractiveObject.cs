using Controllers.Subsystems.Story;
using StarPlatinum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    public class InteractiveObject : MonoBehaviour
    {
        public string m_objectName = "";

        public float m_interactiveRadius = 0;

        public void Interact()
        {
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



            storyController.LoadStoryByID(GenerateStoryID());
        }

        private string GenerateStoryID()
        {
            return ChapterManager.Instance.GetCurrentSceneName() +
                SceneManager.Instance().CurrentScene +
                m_objectName;
        }
    }
}
