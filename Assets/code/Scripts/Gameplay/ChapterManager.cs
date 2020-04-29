using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum;
using Config.GameRoot;

public class ChapterManager : Singleton<ChapterManager>
{
    [SerializeField]
    private string m_sceneName;

    public ChapterManager()
    {
        m_sceneName = RootConfig.Instance.GetChapterName(0);
    }

    public string GetCurrentSceneName() => m_sceneName;

    public void GoToChapter(string chapterName)
    {
        bool result = RootConfig.Instance.IsExistChapterName(chapterName);

        if (result)
        {
            m_sceneName = chapterName;
        }
    }

}
