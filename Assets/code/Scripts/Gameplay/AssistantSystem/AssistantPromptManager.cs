using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assistant
{

    /// <summary>
    /// 助手系统 提示管理
    /// </summary>
    public class AssistantPromptManager
    {

        /// <summary>
        /// 进行提示
        /// </summary>
        public void DoPrompt()
        {
            switch (m_sceneType)
            {
                case GameSceneType.None:
                    break;
                case GameSceneType.Room:
                    m_curPrompt = new AssistantRoomActionPrompt();
                    break;
                case GameSceneType.Aisle:
                    m_curPrompt = new AssistantAisleActionPrompt();
                    break;
                default:
                    break;
            }
            DoCurPrompt();
        }

        /// <summary>
        /// 设置当前的场景
        /// </summary>
        /// <param name="sceneType">场景类型</param>
        public void SetCurScene(GameSceneType sceneType)
        {
            m_sceneType = sceneType;
        }

        /// <summary>
        /// 进行剧情提示
        /// </summary>
        public void DoPlotPrompt()
        {
            m_curPrompt = new AssistantPlotPrompt();
            DoCurPrompt();
        }

        /// <summary>
        /// 进行当前的提示
        /// </summary>
        private void DoCurPrompt()
        {
            if (m_curPrompt != null)
            {
                m_curPrompt.DoPrmpt();
            }
        }

        /// <summary>
        /// 游戏场景类型
        /// </summary>
        private GameSceneType m_sceneType = GameSceneType.None;

        /// <summary>
        /// 提示类型
        /// </summary>
        private IAssistantPrompt m_curPrompt = null;
    }

}
