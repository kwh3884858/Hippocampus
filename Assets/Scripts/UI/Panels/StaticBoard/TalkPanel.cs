using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Subsystems.Story;
using TMPro;
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

            public string GetContent(string content)
            {
                if (m_color != null)
                {
                    content = $"<{m_color}>{content}</color>";
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

        public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            base.Initialize(uiDataProvider, settings);
            m_textHelp = new TextHelp();
        }

        public override void UpdateData(DataProvider data)
        {
            base.UpdateData(data);
            SetInfo(UIPanelDataProvider.ID);
            
        }

        private void SetInfo(string talkID)
        {
            m_currentID = talkID;
            m_actionContainer = StoryController.GetStory(m_currentID);
            SetActionState(ActionState.Begin);
        }

        private void SetNextAction(StoryAction storyAction)
        {
            if (storyAction == null)
            {
                SetActionState(ActionState.Waiting);
                return;
            }
            switch (storyAction.Type)
            {
                case StoryActionType.Name:
                    SetNameContent(storyAction.Content);
                    break;
                case StoryActionType.Content:
                    StartCoroutine(Typewriter(storyAction.Content));
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
                    break;
                case StoryActionType.Picture:
                    break;
                case StoryActionType.Waiting:
                    SetActionState( ActionState.Actioning);
                    CallbackTime(float.Parse(storyAction.Content),()=>{SetActionState( ActionState.End );});
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
            }
        }

        private void SetActionState(ActionState state)
        {
            m_state = state;
            if ( state == ActionState.Begin||state == ActionState.End)
            {
                SetNextAction(m_actionContainer.GetNextAction());
            }
        }

        private void SetNameContent(string name)
        {
            m_name.text = m_textHelp.GetContent(name);
            ResetContentText();
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
                yield return null;
                if (m_content.textInfo.pageCount > m_content.pageToDisplay)
                {
                    m_content.pageToDisplay++;
                }
                yield return new WaitForSeconds(StoryController.GetContentSpeed());
            }
            SetActionState(ActionState.End);
        }
        
        [SerializeField] private TMP_Text m_name;
        [SerializeField] private TMP_Text m_content;

        private string m_currentID;
        private StoryActionContainer m_actionContainer;
        private TextHelp m_textHelp;
        private ActionState m_state = ActionState.Waiting;
//        private bool isReset;
    }
}