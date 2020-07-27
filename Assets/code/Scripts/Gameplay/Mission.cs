using System.Collections;
using System.Collections.Generic;
using StarPlatinum.Manager;
using GamePlay.Stage;
using StarPlatinum.Utility;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;

public class Mission : MonoBehaviour
{
    public MissionEnum m_missionId;
    //public string m_missionId;
    public MissionEnum GetMssionId => m_missionId;

    public bool m_isStartFromStory;
    public bool IsStartFromStory => m_isStartFromStory;

    [ConditionalField("m_isStartFromStory")]
    public string m_storyFileName;
    public string m_storyLabelId;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartMission()
    {
        if (m_isStartFromStory)
        {
            if (m_storyFileName != "")
            {
                GlobalManager.GetStoryController().LoadStoryFileByName(m_storyFileName);
            }
            if (m_storyLabelId != "")
            {
                UI.UIManager.Instance().ShowStaticPanel(UIPanelType.TalkPanel, new TalkDataProvider() { ID = m_storyLabelId });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
