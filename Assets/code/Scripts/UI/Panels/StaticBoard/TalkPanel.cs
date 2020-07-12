using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using Config.Data;
using Const;
using Controllers.Subsystems.Story;
using StarPlatinum;
using TMPro;
using UI.Panels.Element;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;
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
                if (m_color != null)
                {
                    content = $"<#{m_color}>{content}</color>";
                }

                if (m_font != null)
                {
                    content = $"<font=\"{m_font}\">{content}</font>";
                }

                if (m_fontSize != null)
                {
                    content = $"<size={m_fontSize}>{content}</size>";
                }

                if (m_bold)
                {
                    content = $"<b>{content}</b>";
                }

                return content;
            }
        }
        
        private StoryController StoryController => UiDataProvider.ControllerManager.StoryController;
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
        }
        public override void UpdateData(DataProvider data)
        {
            base.UpdateData(data);
            Debug.Log($"开始对话 ID:{UIPanelDataProvider.ID}");
            SetInfo(UIPanelDataProvider.ID);
        }

        private void SetInfo(string talkID)
        {
            if (m_state != ActionState.Waiting)
            {
                Debug.LogWarning("当前对话未结束！！！！");
            }
            ClearData();
            m_isFirstTalk = true;
            m_skip = false;
            m_currentID = talkID;
            m_actionContainer = StoryController.GetStory(m_currentID);
            SetActionState(ActionState.Begin);
        }

        
        private void SetNextAction(StoryAction storyAction)
        {
            if (storyAction == null)
            {
                SetActionState(ActionState.Waiting);
                m_characterTalkEnd = true;
                if (m_autoPlay||m_skip)
                {
                    EndCharacterTalk();
                }
                return;
            }
            m_actionType = storyAction.Type;
            SetActionState(ActionState.Actioning);
            switch (storyAction.Type)
            {
                case StoryActionType.Name:
                    AddNewTalker(storyAction.Content);
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
                m_actionType = StoryActionType.Waiting;
                SetNextAction(m_actionContainer.GetNextAction());
            }
        }

        private void ShowJumpOption(List<Option> options)
        {
            InvokeShowAsSubpanel(PanelType,UIPanelType.OptionsPanel,new OptionsDataProvider(){ Options = options,Callback = OptionCallback});
        }

        private void OptionCallback(string id)
        {
            m_skip = false;
            SetActionState(ActionState.End);
            SetInfo(id);
        }

        private void AddNewTalker(string name)
        {
            m_currentRoleName = name;
            if (m_skip)
            {
                EndCharacterTalk();
                return;
            }
            if (m_isFirstTalk)
            {
                m_isFirstTalk = false;
                EndCharacterTalk();
                return;
            }
            if (m_autoPlay)
            {
                CallbackTime(UiDataProvider.ConfigProvider.StoryConfig.AutoPlayWaitingTime,EndCharacterTalk);
            }
            else
            {
                m_characterTalkEnd = true;
            }
        }
        
        private void SetNameContent(string name)
        {
            m_name.enabled = true;
            m_nameTxt.gameObject.SetActive(false);
            m_curRoleInfo = RoleConfig.GetConfigByKey(name);
            string artNameKey = "";
            if (m_curRoleInfo != null)
            {
                artNameKey = m_curRoleInfo.artNameKey;
            }
            PrefabManager.Instance.SetImage(m_name,artNameKey, () =>
                {
                    m_name.enabled = false;
                    m_nameTxt.gameObject.SetActive(true);
                    m_nameTxt.text = name;
                });
            ResetContentText();
            SetActionState(ActionState.End);
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
            foreach (var txt in content)
            {
                m_content.text += m_textHelp.GetContent(txt.ToString());
                if (m_content.textInfo.pageCount > m_content.pageToDisplay)
                {
                    m_content.pageToDisplay++;
                }
                
                if (m_skip == false)
                {
                    PlayerTypewriterSound();
                    yield return new WaitForSeconds(m_highSpeed ? 0 : StoryController.GetContentSpeed());
                }
            }
            SetActionState(ActionState.End);
        }

        private void PlayerTypewriterSound()
        {
            if (m_curRoleInfo == null||string.IsNullOrEmpty(m_curRoleInfo.typewriterSoundKey))
            {
                SoundService.Instance.PlayEffect(UiDataProvider.ConfigProvider.StoryConfig.TypewriterDefaultSound);
                return;
            }
            SoundService.Instance.PlayEffect(m_curRoleInfo.typewriterSoundKey);

        }

        private void ShowPicture(string picID, Vector2 pos)
        {
            if (m_pictureItems.ContainsKey(picID))
            {
                if (pos.x== 0 || pos.y ==0)
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
                SetActionState(ActionState.End);
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
                ShowPicture(picID,m_picPos[picID]);
            }); 
            
        }

        private void MovePicture(int oldPosID, int targetPosID)
        {
        }

        private void PictureItem(PictureItem item)
        {
            
        }
        
        private void EndCharacterTalk()
        {
            m_characterTalkEnd = false;
            m_highSpeed = false;
            if (m_actionType == StoryActionType.Name)
            {
                SetNameContent(m_currentRoleName);
                return;
            }
            else
            {
                InvokeHidePanel();
                UIPanelDataProvider.OnTalkEnd?.Invoke();
            }
        }

        private void ClearData()
        {
            if (m_typewriterCoroutine != null)
            {
                StopCoroutine(m_typewriterCoroutine);
            }
            m_textHelp.ClearData();
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
            m_autoPlayButtonText.color = m_autoPlay? StoryConfig.AutoPlayButtonActiveColor: StoryConfig.AutoPlayButtonNormalColor;
            if (m_characterTalkEnd)
            {
                EndCharacterTalk();
            }
        }

        public void Skip()
        {
            m_skip = true;
        }

        [SerializeField] private Image m_name;
        [SerializeField] private TMP_Text m_nameTxt;
        [SerializeField] private TMP_Text m_content;
        [SerializeField] private TMP_Text m_autoPlayButtonText;
        [SerializeField] private Transform m_pictureRoot;
        [SerializeField] private GameObject m_talkObj;

        private Dictionary<string, PictureItem> m_pictureItems = new Dictionary<string, PictureItem>();
        private Dictionary<string,Vector2> m_picPos = new Dictionary<string, Vector2>();
        
        private bool m_highSpeed = false;
        private bool m_characterTalkEnd = false;
        private bool m_autoPlay = false;
        private bool m_isFirstTalk = false;
        private bool m_skip = false;
        private string m_currentID;
        private string m_currentRoleName;
        private RoleConfig m_curRoleInfo;
        private StoryActionContainer m_actionContainer;
        private TextHelp m_textHelp;
        private ActionState m_state = ActionState.Waiting;
        private StoryActionType m_actionType = StoryActionType.Waiting;
        private Coroutine m_typewriterCoroutine = null;
//        private bool isReset;
    }
}