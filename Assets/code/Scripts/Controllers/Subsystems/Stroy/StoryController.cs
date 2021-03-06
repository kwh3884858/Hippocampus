using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config;
using Evidence;
using GamePlay.Global;
using GamePlay.Stage;
using StarPlatinum;
using StarPlatinum.EventManager;
using StarPlatinum.Manager;
using StarPlatinum.StoryCompile;
using StarPlatinum.StoryReader;
using UI.Panels;
using UI.Panels.StaticBoard;
using UnityEngine;

namespace Controllers.Subsystems.Story {
    public enum StoryActionType {
        WaitClick,
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
        LoadGameScene,
        LoadMission,
        LoadCgScene,
        CloseCgScene,
        TriggerEvent,
        PlayInteractionAnimation,
        PlayAnimation,
        ChangeBackground,
        LoadSkybox,
        Wrap,
        ChangeSoundVolume,
        EnterControversy,
        CutIn,
        BreakTheory,
        ChangeTalkPanelType,
        ChangeFrontImg,
        RemoveAllExhibit,
        TimeLine,
        AddEvidence,
        AddTip,
        RemoveEvidence,
        Position,
        Rotation,
        GameEvent,
    }

    public class StoryController : ControllerBase {
        public readonly string STORY_FOLDER = "Storys_Export/";
        public override void Initialize (IControllerProvider args) {
            base.Initialize (args);
            State = SubsystemState.Initialization;
            StartCoroutine (LoadStoryInfo ());
            
            EventManager.Instance.AddEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.AddEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
        }

        public override void Terminate()
        {
            base.Terminate();
            EventManager.Instance.RemoveEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.RemoveEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
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

        private IEnumerator LoadStoryInfo () {
            while (Data.ConfigProvider.StoryConfig == null) {
                yield return null;
            }
            m_config = Data.ConfigProvider.StoryConfig;
            State = SubsystemState.Initialized;
        }

        public void LoadStoryFileByName (string storyFileName) {
            if (m_storyFileName == storyFileName) {
                return;
            }

            //Assets/Resources/STORY_FOLDER + storyFileName

            StoryReader storyReader = new StoryReader (STORY_FOLDER + storyFileName);
            if (storyReader != null) {
                m_storys = storyReader;
            }
            bool result = m_storys.GetLoadResult ();
            if (result == true) {
                m_storyFileName = storyFileName;
                if (!m_readLabel.ContainsKey(m_storyFileName))
                {
                    m_readLabel.Add(m_storyFileName,new List<string>());
                }
            }

        }

        public bool IsLabelExist (string label) {
            return m_storys.RequestLabel (label);
        }

        public bool IsLabelRead(string labelId)
        {
            return m_readLabel[m_storyFileName].Contains(labelId);
        }

        private void AddReadLabel(string labelId)
        {
            m_readLabel[m_storyFileName].Add(labelId);
        }

        public StoryActionContainer GetStory (string labelId) {
            StoryActionContainer container = new StoryActionContainer ();

            StoryVirtualMachine.Instance.SetStoryActionContainer (container);
            if (m_storys == null) {
                Debug.LogError ("Story doesn`t exist.");
                return null;
            }
            if (!m_storys.RequestLabel (labelId)) {
                Debug.LogError ($"Label {labelId} doesn`t exist");
            } else {
                m_storys.JumpToWordAfterLabel (labelId);
            }

            if (!IsLabelRead(labelId))
            {
                AddReadLabel(labelId);
            }

            //            container.PushChangePanelType(1);
            //            container.PushJump(new List<Option>(){new Option("1","这是一个不知道什么内容的选项"),new Option("2","这是一个不知道什么内容的选项")});
            //            container.PushFrontImg("MG_TalkType1_TalkBGVignetee");
            while (!m_storys.IsDone ()) {
                switch (m_storys.GetNodeType ()) {

                    case StoryReader.NodeType.word:
                        container.PushName (m_storys.GetName ());
                        StoryVirtualMachine.Instance.Run (m_storys.GetContent ());
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

                    case StoryReader.NodeType.end:
                        m_storys.NextStory ();
                        return container;
                        break;

                    case StoryReader.NodeType.exhibit:
                        container.PushShowEvidence (m_storys.GetExhibit (), m_storys.GetExhibitPrefix ());
                        m_storys.NextStory ();
                        //return container;
                        break;

                    case StoryReader.NodeType.raiseEvent:
                        string eventName = m_storys.GetEventName ();

                        switch (m_storys.GetEventType ()) {
                            case StoryReader.EventType.loadScene:
                                container.PushLoadGameScene (eventName);
                                break;

                            case StoryReader.EventType.loadMission:
                                {
                                    MissionEnum needLoadMission = MissionSceneManager.Instance.GetMissionEnumBy (eventName, false);
                                    if (needLoadMission == MissionEnum.None) {
                                        Debug.LogError (eventName + " is not exist.");
                                    }
                                    container.LoadMission (needLoadMission);
                                    //m_storys.NextStory();
                                    //return container;
                                }
                                break;
                            case StoryReader.EventType.LoadCgScene:
                                container.LoadCGScene (eventName);
                                break;
                            case StoryReader.EventType.CloseCgScene:
                                container.CloseCGScene (eventName);
                                break;
                            case StoryReader.EventType.LoadControversy:
                                container.PushEnterControversy (eventName);
                                break;
                            case StoryReader.EventType.PlayCutIn:
                                container.PushCutIn (eventName);
                                break;
                            case StoryReader.EventType.PlayInteractionAnimation:
                                string cleanItemName = eventName;
                                if (cleanItemName.Contains ("_")) {
                                    cleanItemName = eventName.Substring (0, eventName.IndexOf ('_'));
                                }
                                container.PlayInteractionAnimation (cleanItemName);
                                break;
                            case StoryReader.EventType.invokeEvent:
                                container.TriggerEvent (new StarPlatinum.EventManager.RaiseEvent (
                                    StoryReader.EventType.invokeEvent,
                                    eventName));
                                break;

                            case StoryReader.EventType.playAnimation:
                                container.PlayAnimation (eventName);
                                break;

                            case StoryReader.EventType.PlayTimeline:
                                container.PushTimeLine (eventName);
                                break;

                            case StoryReader.EventType.LoadFrontground:
                                container.PushFrontImg (eventName);
                                break;

                            case StoryReader.EventType.LoadBackground:
                                container.ChangeBackground (eventName);
                                break;
                            case StoryReader.EventType.LoadSkybox:
                                SkyboxEnum skyboxEnum = (SkyboxEnum) Enum.Parse (typeof (SkyboxEnum), eventName);
                                container.LoadSkybox (skyboxEnum);
                                break;
                            case StoryReader.EventType.SwitchTalkUIType:
                                int UIPanelType = int.Parse (eventName);
                                container.PushChangePanelType (UIPanelType);
                                break;
                            case StoryReader.EventType.RemoveSpecificExhibit:
                                container.PushRemoveEvidence (eventName);
                                break;
                            case StoryReader.EventType.RemoveAllExhibit:
                                container.RemoveAllExhibit ();
                                break;
                            case StoryReader.EventType.GameOver:
                                container.PushGameEvent ("GameEnd");
                                break;

                            default:
                                break;
                        }
                        m_storys.NextStory ();
                        break;

                    default:
                        Debug.LogError ("Unknown Node Type");
                        break;
                }

            }
            return container;
        }

        public void SetTalkPanelType (int type) {
            m_talkPanelType = type;
        }

        public int GetTalkPanelType () {
            return m_talkPanelType;
        }

        #region 存档相关
        
        private void OnPlayerPreSaveArchive(object sender, PlayerPreSaveArchiveEvent e)
        {
            GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.StoryArchiveData.ReadLabels = m_readLabel;
        }

        private void OnPlayerLoadArchive(object sender, PlayerLoadArchiveEvent e)
        {
            var data = GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData
                .StoryArchiveData;
            m_readLabel = data.ReadLabels;
            if (m_readLabel == null)
            {
                m_readLabel = new Dictionary<string, List<string>>();
            }
        }


        #endregion
        
        private StoryReader m_storys;
        private StoryConfig m_config;
        private string m_storyFileName;
        private int m_talkPanelType = 2;

        private Dictionary<string, List<string>> m_readLabel;
    }
}