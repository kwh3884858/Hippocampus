using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using StarPlatinum.StoryReader;
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
    }

    public class StoryController: ControllerBase
    {
        public override void Initialize(IControllerProvider args)
        {
            base.Initialize(args);
            State = SubsystemState.Initialization;
            StartCoroutine(LoadStoryInfo());
        }

        private IEnumerator LoadStoryInfo()
        {
            while (Data.ConfigProvider.StoryConfig==null)
            {
                yield return null;
            }
            m_config = Data.ConfigProvider.StoryConfig;
            LoadStory();
            State = SubsystemState.Initialized;
        }

        private void LoadStory()
        {
            m_storys = new StoryReader(m_config.StoryPath);
        }

        public StoryActionContainer GetStory(string ID)
        {
            StoryActionContainer container =new StoryActionContainer();

            //TODO: Get info from Story Parsing
            List<StoryBasicData> datas = m_storys.GetSotry();

            for(int i = 0; i < datas.Count(); i++)
            {
                if(datas[i].typename == StoryReader.NodeType.word.ToString())
                {
                    StoryWordData data = datas[i] as StoryWordData;
                    container.PushName(data.name);
                    container.PushContent(data.content);
                }
            }

            container.PushName("迪奥");
            container.PushContent("jojo，人的能力是有极限的");
            container.PushName("迪奥");
            container.PushContent("我从短暂的人生当中学到一件事......");
            container.PushName("迪奥");
            container.PushContent("越是玩弄计谋，就越会发现人类的能力是有极限的......");
            container.PushName("迪奥");
            container.PushContent("除非超越人类。");
            container.PushName("乔纳森");
            container.PushBold();
            container.PushFontSize("48");
            container.PushColor("#000000");
            container.PushContent("你到底想说什么？");
            container.PushColor("#000000");
            container.PushFontSize("48");
            container.PushBold();
            container.PushWaiting(1.2f);
            container.PushName("迪奥");
            container.PushFontSize("48");
            container.PushColor("#FF0000");
            container.PushContent("我不做人了！");
            container.PushWaiting(1.2f);
            container.PushFontName("SourceHanSansCN2");
            container.PushContent("乔乔！");
            container.PushFontName("SourceHanSansCN2");
            container.PushColor("#FF0000");
            container.PushFontSize("48");
            return container;
        }

        public float GetContentSpeed()
        {
            return m_config.ChineseContentSpeed;
        }

        private StoryReader m_storys;
        private StoryConfig m_config;
    }
}