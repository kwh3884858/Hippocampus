using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using GamePlay.Global;
using StarPlatinum;
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
		Jump,
		Font,
		Waiting,
		Picture,
		PictureMove,
		Bold,
		ChangeBGM,
		ChangeEffectMusic,
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
			m_storys = new StoryReader (STORY_FOLDER + storyFileName);
		}




		private bool IsCorrectChapter ()
		{
			bool result = true;
			result = SingletonGlobalDataContainer.Instance.Chapter == m_storys.Chapter;
			if (result == false) {
				return false;
			}

			result = SingletonGlobalDataContainer.Instance.Scene == m_storys.Scene;
			if (result == false) {
				return false;
			}

			return result;
		}
		private string GenerateStoryID (string itemId)
		{
			return ChapterManager.Instance.GetCurrentSceneName () +
				SceneManager.Instance ().GetCurrentScene +
				itemId;
		}
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

			if (m_storys.Chapter == "Ep2" &&
				m_storys.Scene == "Pier") {
				PushPicture (container, hero, heroine);

			}

			if (labelId == "Ep2_Jeep_PoliceQuestion_0") {
				PushPicture (container, hero, "");
			}

			if (labelId == "Ep2_Jeep_PoliceQuestion_1") {
				PushPicture (container, hero, "");

			}
			if (labelId == "Ep2_Jeep_PoliceQuestion_2") {
				PushPicture (container, hero,"");

			}
			if (labelId == "Ep2_Jeep_PoliceQuestion_4") {
				PushPicture (container, hero, "");
			}

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

				case StoryReader.NodeType.label:
					//m_storys.NextStory ();
					m_storys.NextStory ();
					break;

				default:
					break;
				}


			}

			//while (!m_storys.IsDone())
			//{
			//    switch (m_storys.GetNodeType())
			//    {
			//        case StoryReader.NodeType.word:
			//            container.PushName(m_storys.GetName());
			//            container.PushContent(m_storys.GetContent());
			//            container.PushWaiting(1f);
			//            break;

			//        case StoryReader.NodeType.jump:
			//            break;

			//        default:
			//            break;
			//    }

			//    m_storys.Next();
			//}

			//            container.PushName("迪奥");
			//            container.PushContent("jojo，人的能力是有极限的");
			//            container.PushName("迪奥");
			//            container.PushContent("我从短暂的人生当中学到一件事......");
			//            container.PushName("迪奥");
			//            container.PushContent("越是玩弄计谋，就越会发现人类的能力是有极限的......");
			//            container.PushName("迪奥");
			//            container.PushContent("除非超越人类。");
			//            container.PushName("乔纳森");
			//            container.PushBold();
			//            container.PushFontSize("48");
			//            container.PushColor("#000000");
			//            container.PushContent("你到底想说什么？");
			//            container.PushColor("#000000");
			//            container.PushFontSize("48");
			//            container.PushBold();
			//            container.PushWaiting(1.2f);
			//            container.PushName("迪奥");
			//            container.PushFontSize("48");
			//            container.PushColor("#FF0000");
			//            container.PushContent("我不做人了！");
			//            container.PushWaiting(1.2f);
			//            container.PushFontName("SourceHanSansCN2");
			//            container.PushContent("乔乔！");
			//            container.PushFontName("SourceHanSansCN2");
			//            container.PushColor("#FF0000");
			//            container.PushFontSize("48");
			//if (ID == "9")
			//{
			//    container.PushName("迪奥");
			//    container.PushContent("jojo，人的能力是有极限的");
			//    container.PushJump(
			//        new List<Option>() {new Option("1", "1"), new Option("2", "2"), new Option("3", "3")});
			//}else if (ID == "1")
			//{
			//    container.PushName("乔纳森");
			//    container.PushContent("你到底想说什么？");
			//}else if (ID == "2")
			//{
			//    container.PushName("迪奥");
			//    container.PushContent("我不做人了！");
			//}else if (ID == "3")
			//{
			//    container.PushName("乔纳森");
			//    container.PushContent("乔乔！");
			//}
			return container;
		}

		public float GetContentSpeed ()
		{
			return m_config.ChineseContentSpeed;
		}

		private StoryReader m_storys;
		private StoryConfig m_config;
	}
}