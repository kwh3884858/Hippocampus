using System.Collections;
using System.Collections.Generic;
using Controllers;
using Controllers.Subsystems.Story;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
	static public ControllerManager GetControllerManager () => m_controllerManager;
	static public StoryController GetStoryController () => m_storyController;

	private void Start ()
	{
		m_controllerManager = GetComponent<ControllerManager> ();
		m_storyController = GetComponent<StoryController> ();
	}

	static ControllerManager m_controllerManager;
	static StoryController m_storyController;
}
