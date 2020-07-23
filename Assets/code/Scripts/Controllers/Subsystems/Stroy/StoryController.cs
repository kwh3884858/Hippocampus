using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using Evidence;
using GamePlay.Global;
using GamePlay.Stage;
using StarPlatinum;
using StarPlatinum.Manager;
using StarPlatinum.StoryCompile;
using StarPlatinum.StoryReader;
using UI.Panels.StaticBoard;
using UnityEngine;

namespace Controllers.Subsystems.Story
{
	public enum StoryActionType
	{
		Name,
		Content,
		Color,
		FontSize,
		TypewriterInterval,
		Jump,
		Font,
		Waiting,
		Picture,
		PictureMove,
		Bold,
		ChangeBGM,
		ChangeEffectMusic,
		ShowEvidence,
	}

	public class StoryController : ControllerBase
	{
		public readonly string STORY_FOLDER = "Storys_Export/";
		public override void Initialize (IControllerProvider args)
		{
			base.Initialize (args);
			State = SubsystemState.Initialization;
			StartCoroutine (LoadStoryInfo ());
		}

		//public bool LoadStoryByItem (string itemId)
		//{
		//	if (m_storys == null) {
		//		return false;
		//	}
		//	bool result = true;
		//	result = m_storys.RequestLabel (itemId);
		//	if (!result) {
		//		return false;
		//	}

		//	result = IsCorrectChapter ();

		//	result = m_storys.JumpToWordAfterLabel (itemId);

		//	return result;
		//}



		private IEnumerator LoadStoryInfo ()
		{
			while (Data.ConfigProvider.StoryConfig == null) {
				yield return null;
			}
			m_config = Data.ConfigProvider.StoryConfig;
			LoadStoryFileByName (m_config.StoryPath);
			State = SubsystemState.Initialized;
		}

		public void LoadStoryFileByName (string storyFileName)
		{
			if (m_storyFileName == storyFileName) {
				return;
			}

			//Assets/Resources/STORY_FOLDER + storyFileName
			m_storys = new StoryReader (STORY_FOLDER + storyFileName);
			bool result = m_storys.GetLoadResult ();
			if (result == true) {
				m_storyFileName = storyFileName;
			}

		}

		public bool IsLabelExist (string label)
		{
			return m_storys.RequestLabel (label);
		}

		//private bool IsCorrectChapter ()
		//{
		//	bool result = true;
		//	result = SingletonGlobalDataContainer.Instance.Chapter == m_storys.Chapter;
		//	if (result == false) {
		//		return false;
		//	}

		//	result = SingletonGlobalDataContainer.Instance.Scene == m_storys.Scene;
		//	if (result == false) {
		//		return false;
		//	}

		//	return result;
		//}
		//private string GenerateStoryID (string itemId)
		//{
		//	return ChapterManager.Instance.GetCurrentSceneName () +
		//		GameSceneManager.Instance.GetCurrentSceneEnum ().ToString() +
		//		itemId;
		//}
		//Temp
		int left = 28;
		int right = 72;
		string hero = "Hero";
		string heroine = "Heroine";
		string jailerMan = "JailerMan";
		string jailerWoman = "JailerWoman";

		private void PushPicture (StoryActionContainer container, string leftName, string rightName)
		{
			container.PushPicture (hero, 0);
			container.PushPicture (heroine, 0);

			container.PushPicture (jailerMan, 0);
			container.PushPicture (jailerWoman, 0);


			if (leftName.Length != 0) {
				container.PushPicture (leftName, left);
			}
			if (rightName.Length != 0) {
				container.PushPicture (rightName, right);

			}
		}

		//Temp
		public StoryActionContainer GetStory (string labelId)
		{
			StoryActionContainer container = new StoryActionContainer ();

			StoryVirtualMachine.Instance.SetStoryActionContainer (container);

			//List<StoryBasicData> datas = m_storys.GetSotry();

			//for(int i = 0; i < datas.Count(); i++)
			//{
			//    if(datas[i].typename == StoryReader.NodeType.word.ToString())
			//    {
			//        StoryWordData data = datas[i] as StoryWordData;
			//        container.PushName(data.name);
			//        container.PushContent(data.content);
			//    }
			//    else if (datas[i].typename == StoryReader.NodeType.jump.ToString())
			//    {
			//        StoryJumpData data = datas[i] as StoryJumpData;
			//        container.PushJump(data.jump, );

			//    }
			//}
			if (m_storys == null) {
				Debug.LogError ("Story doesn`t exist.");
				return null;
			}

			if (!m_storys.RequestLabel (labelId)) {
				Debug.LogError ($"Label {labelId} doesn`t exist");
			} else {
				m_storys.JumpToWordAfterLabel (labelId);
			}

			//if (m_storys.Chapter == "Ep2" &&
			//	m_storys.Scene == "Pier") {
			//	PushPicture (container, hero, heroine);

			//}

			//if (labelId == "Ep2_Jeep_PoliceQuestion_0") {
			//	PushPicture (container, hero, "");
			//}

			//if (labelId == "Ep2_Jeep_PoliceQuestion_1") {
			//	PushPicture (container, hero, "");

			//}
			//if (labelId == "Ep2_Jeep_PoliceQuestion_2") {
			//	PushPicture (container, hero, "");

			//}
			//if (labelId == "Ep2_Jeep_PoliceQuestion_4") {
			//	PushPicture (container, hero, "");
			//}

			while (!m_storys.IsDone ()) {
				switch (m_storys.GetNodeType ()) {

				case StoryReader.NodeType.word:
					container.PushName (m_storys.GetName ());
					StoryVirtualMachine.Instance.Run (m_storys.GetContent ());
					container.PushWaiting (1f);
					m_storys.NextStory ();
					break;

				case StoryReader.NodeType.jump:
					container.PushJump (m_storys.GetJump ());
					//						m_storys.NextStory ();
					//Test
					return container;
					break;

				case StoryReader.NodeType.label:
					//m_storys.NextStory ();
					m_storys.NextStory ();
					break;
					break;

				case StoryReader.NodeType.end:
					m_storys.NextStory ();
					return container;
					break;

				case StoryReader.NodeType.exhibit:
					container.PushShowEvidence (m_storys.GetExhibit ());
					m_storys.NextStory ();
					//return container;
					break;

				case StoryReader.NodeType.raiseEvent:

					m_storys.NextStory ();
					break;

				default:
					Debug.LogError ("Unknown Node Type");
					break;
				}


			}


			return container;
		}

		private StoryReader m_storys;
		private StoryConfig m_config;
		private string m_storyFileName;

	}
}