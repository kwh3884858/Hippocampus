using System.Collections;
using Config;

namespace Controllers.Subsystems.Stroy
{
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
            ReadStorys storys = new ReadStorys(m_config.StoryPath);

        }

        private StoryConfig m_config;
    }
}