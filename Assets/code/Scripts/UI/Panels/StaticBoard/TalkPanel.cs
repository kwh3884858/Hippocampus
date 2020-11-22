using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using Config;
using Config.Data;
using Const;
using Controllers.Subsystems;
using Controllers.Subsystems.Story;
using DG.Tweening;
using Evidence;
using GamePlay.Stage;
using StarPlatinum;
using StarPlatinum.EventManager;
using TMPro;
using UI.Panels.Element;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Panels.StaticBoard
{
    public class TalkPanel : UIPanel<UIDataProviderGameScene,TalkDataProvider>
    {
        private enum ActionState
        {
            Waiting,
            Begin,
            Actioning,
            End,
        }
        private class TextHelp
        {
            private string m_color;
            private string m_fontSize;
            private string m_font;
            private bool m_bold;
            private float m_typewriterInterval = StoryConfig.Ins.ChineseContentSpeed;

            public float TypewriterInterval
            {
                get { return m_typewriterInterval; }
                set
                {
                    if (m_typewriterInterval.Equals(value))
                    {
                        m_typewriterInterval = Config.StoryConfig.Ins.ChineseContentSpeed;
                        return;
                    }
                    m_typewriterInterval = value;
                }
            }

            public void PushColor(string color)
            {
                if (m_color==color)
                {
                    m_color = null;
                    return;
                }
                m_color = color;
            }

            public void PushFontSize(string fontSize)
            {
                if (m_fontSize==fontSize)
                {
                    m_fontSize = null;
                    return;
                }
                m_fontSize = fontSize;
            }

            public void PushFont(string font)
            {
                if (m_font==font)
                {
                    m_font = null;
                    return;
                }
                m_font = font;
            }

            public void PushBold()
            {
                m_bold = !m_bold;
            }

            public void ClearData()
            {
                m_color = null;
                m_fontSize = null;
                m_font = null;
                m_bold = false;
            }

            public string GetContent(string content)
            {
                StringBuilder builder = new StringBuilder(content);
                if (m_color != null)
                {
                    builder.Insert(0, $"<#{m_color}>");
                    builder.Append("</color>");
                }

                if (m_font != null)
                {
                    builder.Insert(0, $"<font=\"{m_font}\">");
                    builder.Append("</font>");
                }

                if (m_fontSize != null)
                {
                    builder.Insert(0, $"<size={m_fontSize}>");
                    builder.Append("</size>");
                }

                if (m_bold)
                {
                    builder.Insert(0, $"<b>");
                    builder.Append("</b>");
                }
                

                return builder.ToString();
            }
        }
        
        private class TalkRecord
        {
            public TextHelp TextHelp;
            public StoryActionContainer StoryActionContainer;
            public ActionState ActionState ;
            public StoryActionType StoryActionType ;
            public StoryAction StoryAction;
            public Queue<string> NextIDS;
            public Dictionary<string, Vector2> PicturePos;
            public string BackGroundImg;
        }

        private StoryController StoryController => UiDataProvider.ControllerManager.StoryController;
        private LogController LogController => UiDataProvider.ControllerManager.LogController;
        private StoryConfig StoryConfig => UiDataProvider.ConfigProvider.StoryConfig;

        private float Width => UiDataProvider.Canvas.rectTransform().rect.width; 
        private float Height => UiDataProvider.Canvas.rectTransform().rect.height; 


        public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            base.Initialize(uiDataProvider, settings);
            m_textHelp = new TextHelp();
            
        }

        public override void Hide()
        {
            base.Hide();
            m_autoPlay = false;
            m_highSpeed = false;
            if (!UIManager.Instance().IsPanelShow(UIPanelType.UICommonCgscenePanel))
            {
                GamePlay.Player.PlayerController.Instance().SetMoveEnable(true);
            }
        }

        public override void ShowData(DataProvider data)
        {
            base.ShowData(data);
            CoreContainer.Instance.StopPlayerAnimation ();
            if (!UIManager.Instance().IsPanelShow(UIPanelType.UICommonCgscenePanel))
            {
                GamePlay.Player.PlayerController.Instance().SetMoveEnable(false);
            }

            if (Application.isEditor || GamePlay.Global.SingletonGlobalDataContainer.Instance.SHOW_SKIP)
            {
                m_skipButton.gameObject.SetActive(true);
            }
            else
            {
                m_skipButton.gameObject.SetActive(false);
            }
        }

        public override void UpdateData(DataProvider data)
        {
            base.UpdateData(data);
            Debug.Log($"开始对话 ID:{UIPanelDataProvider.ID}");
            m_backgroundImg.gameObject.SetActive(false);
            SetInfo(UIPanelDataProvider.ID);
        }

        public override void Tick()
        {
            base.Tick();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClickSkip();
            }
            if (string.IsNullOrEmpty(m_currentID)&&m_nextIDQueue.Count>0)
            {
                SetInfo(m_nextIDQueue.Dequeue());
            }

            if (m_isBeginNextAction)
            {
                m_isBeginNextAction = false;
                SetNextAction(m_actionContainer.GetNextAction());
            }
            
        }

        private void SetInfo(string talkID)
        {
            if (m_state != ActionState.Waiting)
            {
                Debug.LogWarning("当前对话未结束！！！！");
            }
            ClearData();
            m_skip = false;
            m_currentID = talkID;
            m_actionContainer = StoryController.GetStory(m_currentID);
            m_actionContainer.ProcessActionContainer();
            SetActionState(ActionState.Begin);
        }

        
        private void SetNextAction(StoryAction storyAction)
        {
            if (storyAction == null)
            {
                EventManager.Instance.SendEvent(new LabelEndEvent(){ LabelID = m_currentID});
                m_currentID = null;
                SetActionState(ActionState.Waiting);
                if (m_nextIDQueue.Count>0)
                {
                    return;
                }
                if (RecoverRecord())
                {
                    return;
                }
                InvokeHidePanel();
                UIPanelDataProvider.OnTalkEnd?.Invoke();
                return;
            }
            m_actionType = storyAction.Type;
            m_curAction = storyAction;
            SetActionState(ActionState.Actioning);
            Debug.LogWarning($"当前行为:{m_actionType}");

            switch (storyAction.Type)
            {
                case StoryActionType.WaitClick:
                    WaitClickEnd();
                    break;
                case StoryActionType.Name:
                    SetNameContent(storyAction.Content);
                    break;
                case StoryActionType.Content:
                    if (m_typewriterCoroutine != null)
                    {
                        StopCoroutine(m_typewriterCoroutine);
                    }
                    m_typewriterCoroutine = StartCoroutine(Typewriter(storyAction.Content));
                    break;
                case StoryActionType.Color:
                    m_textHelp.PushColor(storyAction.Content);
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.Font:
                    m_textHelp.PushFont(storyAction.Content);
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.Jump:
                    var jumpAction = storyAction as StoryJumpAction;
                    if (jumpAction == null)
                    {
                        Debug.LogError("JumpAction null!!!!!!!Please check");
                        SetActionState(ActionState.End);
                        return;
                    }
                    Debug.Log($"=== options:{jumpAction.Options}");
                    ShowJumpOption(jumpAction.Options);
                    break;
                case StoryActionType.Picture:
                    var pictureAction = storyAction as StoryShowPictureAction;
                    if (pictureAction == null)
                    {
                        Debug.LogError("pictureAction null!!!!!!!Please check");
                        SetActionState(ActionState.End);
                        return;
                    }

                    ShowPicture(pictureAction.Content, pictureAction.Pos);
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.Waiting:
                    SetActionState( ActionState.Actioning);
                    if (m_skip == false)
                    {
                        CallbackTime(float.Parse(storyAction.Content),()=>{SetActionState( ActionState.End );});
                    }
                    else
                    {
                        SetActionState( ActionState.End);
                    }
                    break;
                case StoryActionType.PictureMove:
                    var pictureMoveAction = storyAction as StoryPictureMoveAction;
                    MovePicture(pictureMoveAction.PicID,pictureMoveAction.StartX,pictureMoveAction.StartY,pictureMoveAction.EndX,pictureMoveAction.EndY,pictureMoveAction.Ease,pictureMoveAction.Duration);
                    break;
                case StoryActionType.FontSize:
                    m_textHelp.PushFontSize(storyAction.Content);
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.Bold:
                    m_textHelp.PushBold();
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.ChangeBGM:
                    SetBgm(storyAction.Content);
                    break;
                case StoryActionType.ChangeEffectMusic:
                    PlayEffectMusic(storyAction.Content);
                    break;
                case StoryActionType.TypewriterInterval:
                    m_textHelp.TypewriterInterval = float.Parse(storyAction.Content);
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.ShowEvidence:
                    InvokeShowPanel(UIPanelType.UICommonDetectiveNotesPanel, new EvidenceDataProvider()
                    {
                        OnShowEvidence = OnSelectEvidenceEnd,
                        IsOnEvidence = true,
                        CurState = CommonMapsTipsEvidencesPanel.ShowState.Evidences,
                        IsShowSelectBtn = true
                    });
                    return;
                case StoryActionType.LoadGameScene:
                    GameSceneManager.Instance.LoadScene(SceneLookup.GetEnum(m_curAction.Content, false));
                    bool result = MissionSceneManager.Instance.LoadCurrentMissionScene ();
                    if (result == false) {
                        Debug.LogError ("Current Game Scene: " + m_curAction.Content + " doesn`t contain Mission Scene " +
                                        MissionSceneManager.Instance.GetCurrentMissionEnum().ToString ());
                    }
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.LoadMission:
                    var action = m_curAction as StoryLoadMissionAction;
                    MissionSceneManager.Instance.SetCurrentMission(action.Mission);
                    MissionSceneManager.Instance.LoadCurrentMissionScene();
                    //if (MissionSceneManager.Instance.IsMissionSceneExist(action.Mission))
                    //{
                    //    MissionSceneManager.Instance.LoadMissionScene(action.Mission);
                    //}
                    //else
                    //{
                    //    MissionSceneManager.Instance.LoadMissionScene(MissionEnum.None);
                    //}
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.LoadCgScene:
                    UI.UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonCgscenePanel, new CGSceneDataProvider() { CGSceneID = m_curAction.Content });
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.CloseCgScene:
                    if (UIManager.Instance().IsPanelShow(UIPanelType.UICommonCgscenePanel))
                    {
                        StarPlatinum.TimerService.Instance.AddTimer(0.01f, () =>
                    {
                        EventManager.Instance.SendEvent(new CGSceneCloseEvent() { m_cgSceneId = m_curAction.Content });
                    });
                    }
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.TriggerEvent:
                    var triggerAction = m_curAction as StoryEventAction;
                    EventManager.Instance.SendEvent(triggerAction.Event);
                    SetActionState(ActionState.End);
                    return;
                case StoryActionType.PlayAnimation:
                    SetActionState(ActionState.End);
                    //TODO:播放动画
                    return;
                case StoryActionType.ChangeBackground:
                    ShowBG(storyAction.Content);
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.Wrap:
                    m_content.text += '\n';
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.ChangeSoundVolume:
                    int volume = int.Parse(m_curAction.Content);
                    SoundService.Instance.SetVolumePercentage(volume);
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.EnterControversy:
                    UIManager.Instance().ShowPanel(UIPanelType.UIJudgmentControversyPanel,new ControversyDataProvider(){ID = m_curAction.Content});
                    SetActionState(ActionState.End);
                    break;
                case StoryActionType.CutIn:
                    UIManager.Instance().ShowPanel(UIPanelType.UICommonBreaktheoryPanel,new BreakTheoryDataProvider(){Type = EnumBreakTheoryType.CutIn ,ImgKey = m_curAction.Content,CloseCallback=
                        () =>
                        {
                            SetActionState(ActionState.End);
                        }});
                    break;
                default:
                    Debug.LogError($"未处理对话行为:{storyAction.Type}");
                    break;
            }
        }

        private void SetActionState(ActionState state)
        {
            m_state = state;
            if ( state == ActionState.Begin||state == ActionState.End)
            {
                //记录
                if (state == ActionState.End &&m_curAction.Type != StoryActionType.Jump)
                {
                    UiDataProvider.ControllerManager.LogController.PushLog(m_curAction);
                }
                
                m_actionType = StoryActionType.Waiting;
                m_isBeginNextAction = true;
            }
        }

        private void ShowJumpOption(List<Option> options)
        {
            InvokeShowAsSubpanel(PanelType,UIPanelType.OptionsPanel,new OptionsDataProvider(){ Options = options,Callback = OptionCallback});
        }

        private void OptionCallback(string id)
        {
            m_skip = false;
            LogController.PushLog(m_curAction,id);
            m_nextIDQueue.Enqueue(id);
            SetActionState(ActionState.End);
        }

        private void WaitClickEnd()
        {
            if (m_skip)
            {
                EndCharacterTalk();
                return;
            }
            if (m_autoPlay)
            {
                CallbackTime(UiDataProvider.ConfigProvider.StoryConfig.AutoPlayWaitingTime,EndCharacterTalk);
            }
            else
            {
                SetTalkContentEnd(true);
            }
        }

        private void SetTalkContentEnd(bool isEnd)
        {
            m_characterTalkEnd = isEnd;
            m_contentEnd.SetActive(isEnd);
        }
        private void SetNameContent(string name)
        {
            m_nameTxt.gameObject.SetActive(false);
            m_curRoleInfo = RoleConfig.GetConfigByKey(name);
            string artNameKey = "";
            if (m_curRoleInfo != null)
            {
                artNameKey = m_curRoleInfo.artNameKey;
            }
            if (!string.IsNullOrEmpty(artNameKey))
            {
                m_name.enabled = true;
                PrefabManager.Instance.SetImage(m_name,artNameKey, () =>
                {
                    m_name.enabled = false;
//                    m_nameTxt.gameObject.SetActive(true);
//                    m_nameTxt.text = name;
                });
            }
            else
            {
                m_name.enabled = false;
            }
            ActiveTalkerTachie(name);
            ResetContentText();
            SetActionState(ActionState.End);
        }

        private void ActiveTalkerTachie(string talkerName)
        {
            foreach (var pictureItem in m_pictureItems)
            {
                var name = pictureItem.Value.GetMyCharactorName();
                if (string.IsNullOrEmpty(name) || name != talkerName)
                {
                    pictureItem.Value.SetTachieStatus(EnumTachieStatus.Darken);
                }
                else
                {
                    pictureItem.Value.SetTachieStatus(EnumTachieStatus.Talk);
                }
            }
        }

        private void SetBgm(string musicName)
        {
            m_uiDataProvider.SoundService.PlayBgm(musicName);
            SetActionState(ActionState.End);
        }

        private void PlayEffectMusic(string effectName)
        {
            m_uiDataProvider.SoundService.PlayEffect(effectName);
            SetActionState(ActionState.End);
        }

        private void ResetContentText()
        {
            m_content.text = "";
            m_content.pageToDisplay = 1;
        }

        IEnumerator Typewriter(string content)
        {
            for (int i = 0; i < content.Length; i++)
            {
                m_content.text += m_textHelp.GetContent(content[i].ToString());
                if (m_content.textInfo.pageCount > m_content.pageToDisplay)
                {
                    m_content.pageToDisplay++;
                }
                
                if (m_skip == false)
                {
                    PlayerTypewriterSound();
                    if (i + 1 < content.Length)
                    {
                        yield return new WaitForSeconds(m_highSpeed ? 0 : m_textHelp.TypewriterInterval);
                    }
                }
            }
            SetActionState(ActionState.End);
        }

        private void PlayerTypewriterSound()
        {
            if (m_highSpeed||m_skip)
            {
                return;
            }
            if (m_curRoleInfo == null||string.IsNullOrEmpty(m_curRoleInfo.typewriterSoundKey))
            {
                SoundService.Instance.PlayEffect(UiDataProvider.ConfigProvider.StoryConfig.TypewriterDefaultSound,false,0.1f,true);
                return;
            }
            SoundService.Instance.PlayEffect(m_curRoleInfo.typewriterSoundKey,false,0.1f,true);

        }

        private void ClearPicture()
        {
            foreach (var item in m_pictureItems)
            {
                UiDataProvider.RolePictureProvider.ReleasePictureItem(item.Value);
            }
            m_pictureItems.Clear();
            m_picPos.Clear();
        }

        private void ShowPicture(string picID, Vector2 pos,bool ignoreHidePos= false,Action callbackAfterShow=null)
        {
            if (m_pictureItems.ContainsKey(picID))
            {
                if (!ignoreHidePos&&(pos.x== 0 || pos.y ==0))
                {
                    UiDataProvider.RolePictureProvider.ReleasePictureItem(m_pictureItems[picID]);
                    m_pictureItems.Remove(picID);
                    m_picPos.Remove(picID);
                }
                else
                {
                    m_pictureItems[picID].rectTransform().anchoredPosition = new Vector2(Width/100*(pos.x - 50),Height/100*(pos.y - 50));
                    m_picPos[picID] = pos;
                }
                callbackAfterShow?.Invoke();
                return;
            }
            if (m_picPos.ContainsKey(picID))
            {
                m_picPos[picID] = pos;
                return;
            }
            else
            {
                m_picPos.Add(picID, pos);
            }

            UiDataProvider.RolePictureProvider.GetPictureItem(picID, (item) =>
            {
                m_pictureItems.Add(picID,item);
                item.transform.SetParent(m_pictureRoot);
                item.transform.localPosition= Vector3.zero;
                item.transform.localScale = Vector3.one;
                ShowPicture(picID,m_picPos[picID],ignoreHidePos,callbackAfterShow);
            }); 
            
        }

        private void MovePicture(string picID, int startX,int startY,int endX,int endY,Ease ease,float duration)
        {
            ShowPicture(picID,new Vector2(startX,startY),true, () =>
                {
                    DOTween.To(() => { return m_pictureItems[picID].rectTransform().anchoredPosition; },
                            v => { m_pictureItems[picID].rectTransform().anchoredPosition = v; },
                            new Vector2(Width / 100 * (endX - 50), Height / 100 * (endY - 50)), duration).SetEase(ease)
                        .OnComplete(() => { SetActionState(ActionState.End); });
                });
        }

        private void ShowBG(string bgKey)
        {
            if (string.IsNullOrEmpty(bgKey))
            {
                m_backgroundImg.gameObject.SetActive(false);
                return;
            }
            m_backgroundImg.gameObject.SetActive(true);
            PrefabManager.Instance.SetImage(m_backgroundImg,bgKey);
            m_storyBG = bgKey;
        }

        private void OnSelectEvidenceEnd(string exhibitID)
        {
            m_skip = false;
            var action = m_curAction as StoryEvidenceAction;
            if (exhibitID.Equals(action.evidenceID))
            {
                SetActionState(ActionState.End);
                ClickSkip();
                return;
            }

            var labelID = EvidenceDataManager.Instance.GetEvidenceWrongID(exhibitID, action.prefix);
            if (labelID == null)
            {
                SetNextAction(action);
                return;
            }
            SetTalkRecord();
            SetInfo(labelID);
        }
        
        
        private void EndCharacterTalk()
        {
            SetTalkContentEnd(false);
            m_highSpeed = false;
            SetActionState(ActionState.End);
        }

        private void ClearData()
        {
            if (m_typewriterCoroutine != null)
            {
                StopCoroutine(m_typewriterCoroutine);
            }
            m_textHelp.ClearData();
            m_actionContainer = null;
            ClearPicture();
            m_storyBG = null;
        }

        public void ClickSkip()
        {
            if (m_characterTalkEnd && !m_autoPlay)
            {
                EndCharacterTalk();
                return;
            }
            m_highSpeed = true;
        }

        public void AutoPlay()
        {
            m_autoPlay = !m_autoPlay;
            m_autoPlayImg.color = m_autoPlay? StoryConfig.AutoPlayButtonActiveColor: StoryConfig.AutoPlayButtonNormalColor;
            if (m_characterTalkEnd)
            {
                EndCharacterTalk();
            }
        }

        public void ClickShowLog()
        {
            InvokeShowAsSubpanel(PanelType,UIPanelType.UICommonLogPanel);
        }

        public void Skip()
        {
            if (m_characterTalkEnd)
            {
                EndCharacterTalk();
            }
            m_skip = true;
        }

#region 记录
        private Queue<TalkRecord> m_talkRecord=new Queue<TalkRecord>();

        private bool RecoverRecord()
        {
            if (m_talkRecord.Count <= 0)
            {
                return false;
            }
            var record = m_talkRecord.Dequeue();
            m_textHelp = record.TextHelp;
            m_actionContainer = record.StoryActionContainer;
            m_state = record.ActionState;
            m_actionType = record.StoryActionType;
            m_curAction = record.StoryAction;
            m_nextIDQueue = record.NextIDS;
            m_storyBG = record.BackGroundImg;
            
            m_picPos.Clear();
            foreach (var picPos in record.PicturePos)
            {
                ShowPicture(picPos.Key, picPos.Value);
            }

            ShowBG(m_storyBG);
            CallbackTime(0.02f,()=>
            {
                SetNextAction(m_curAction);
            });
            return true;
        }

        private void SetTalkRecord()
        {
            var record = new TalkRecord();
            record.ActionState = m_state;
            record.TextHelp = m_textHelp;
            record.StoryActionContainer = m_actionContainer;
            record.StoryAction = m_curAction;
            record.StoryActionType = m_actionType;
            record.NextIDS = m_nextIDQueue;
            record.BackGroundImg = m_storyBG;
            record.PicturePos = m_picPos;
            m_talkRecord.Enqueue(record);
        }
#endregion

        [SerializeField] private Image m_name;
        [SerializeField] private TMP_Text m_nameTxt;
        [SerializeField] private TMP_Text m_content;
        [SerializeField] private Image m_autoPlayImg;
        [SerializeField] private Transform m_pictureRoot;
        [SerializeField] private Image m_backgroundImg;
        [SerializeField] private GameObject m_contentEnd;
        [SerializeField] private Button m_skipButton;

        private Dictionary<string, PictureItem> m_pictureItems = new Dictionary<string, PictureItem>();
        private Dictionary<string,Vector2> m_picPos = new Dictionary<string, Vector2>();
        
        private bool m_highSpeed = false;//当前文本快进
        private bool m_characterTalkEnd = false;
        private bool m_autoPlay = false;
        private bool m_skip = false;//剧情快进
        private bool m_waitingEnd = false;
        private string m_currentID;
        private Queue<string> m_nextIDQueue = new Queue<string>();
        private RoleConfig m_curRoleInfo;
        private StoryActionContainer m_actionContainer;
        private TextHelp m_textHelp;
        private ActionState m_state = ActionState.Waiting;
        private StoryActionType m_actionType = StoryActionType.Waiting;
        private Coroutine m_typewriterCoroutine = null;
        private StoryAction m_curAction;
        private string m_storyBG = null;
        private bool m_isBeginNextAction = false;
//        private bool isReset;
    }
}