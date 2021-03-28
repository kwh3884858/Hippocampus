using System;
using System.Collections.Generic;
using Controllers.Subsystems.Story;
using StarPlatinum;
using StarPlatinum.EventManager;

namespace Controllers.Subsystems
{
    public enum LogType
    {
        Talk,
        Jump,
        ShowEvidence,
    }
    public class LogInfo
    {
        public LogType LogType;
        public string Name;
        public string Content;
    }
    
    public class LogController: ControllerBase
    {
        public override void Initialize(IControllerProvider args)
        {
            base.Initialize(args);
            EventManager.Instance.AddEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.AddEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
        }

        public override void Terminate()
        {
            base.Terminate();
            EventManager.Instance.RemoveEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.RemoveEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
        }


        public void PushLog(StoryAction action,string selectID =null)
        {
            if (m_logInfos.Count >= MAXLogNum && action.Type!= StoryActionType.Content)
            {
                m_logInfos.RemoveAt(0);
            }
            switch (action.Type)
            {
                case StoryActionType.Name:
                    SetName(action);
                    break;
                case StoryActionType.Content:
                    SetContent(action);
                    break;
                case StoryActionType.Jump:
                    SetJump(action,selectID);
                    break;
                case StoryActionType.ShowEvidence:
                    SetShowEvidence(action);
                    break;
            }
        }

        public void LogEnd()
        {
            m_logInfos.Clear();
        }

        public List<LogInfo> GetLogInfos()
        {
            if (m_logInfos.Count <= 0)
            {
                return m_logInfos;
            }

            var lastLog = m_logInfos[m_logInfos.Count - 1];
            if (lastLog.LogType == LogType.Talk && string.IsNullOrEmpty(lastLog.Content))
            {
                return m_logInfos.GetRange(0, m_logInfos.Count - 1);
            }
            return m_logInfos;
        }

        private void SetName(StoryAction action)
        {
             m_logInfos.Add(new LogInfo(){LogType =  LogType.Talk, Name = action.Content});
        }

        private void SetContent(StoryAction action)
        {
            var lastLog = m_logInfos[m_logInfos.Count - 1];
            if (string.IsNullOrEmpty(lastLog.Content))
            {
                lastLog.Content = "";
            }
            lastLog.Content += action.Content;
        }

        private void SetJump(StoryAction action,string selectID)
        {
            var jumpAction = action as StoryJumpAction;
            m_logInfos.Add(new LogInfo(){LogType = LogType.Jump,Content = jumpAction.Options.Find(x=>x.ID.Equals(selectID)).Content});
        }

        private void SetShowEvidence(StoryAction action)
        {
            m_logInfos.Add(new LogInfo(){LogType = LogType.ShowEvidence,Content = action.Content});

        }

        public void TalkEnd()
        {
            
        }
        
        
        #region 存档相关
        
        private void OnPlayerPreSaveArchive(object sender, PlayerPreSaveArchiveEvent e)
        {
            GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.StoryArchiveData.LogInfos = m_logInfos;
        }

        private void OnPlayerLoadArchive(object sender, PlayerLoadArchiveEvent e)
        {
            var data = GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData
                .StoryArchiveData;
            m_logInfos = data.LogInfos;
            if (m_logInfos == null)
            {
                m_logInfos = new List<LogInfo>();
            }
        }


        #endregion

        private int MAXLogNum = 300;
        private List<LogInfo> m_logInfos = new List<LogInfo>();
    }
}