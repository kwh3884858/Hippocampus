using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum;
using StarPlatinum.Base;

namespace Assistant
{

    /// <summary>
    /// 助手系统管理器
    /// </summary>
    public class AssistantSystemManager : Singleton<AssistantSystemManager>
    {
        /// <summary>
        /// 显示助手系统界面
        /// </summary>
        public void ShowView()
        {
            // TODO: show assistant view
            UI.UIManager.Instance().ShowPanel(UIPanelType.Evidencepanel);// 显示UI
        }

        /// <summary>
        /// 隐藏助手系统界面
        /// </summary>
        public void HideView()
        {
            // TODO: hide assistant view
        }

        /// <summary>
        /// 设置当前的场景
        /// </summary>
        /// <param name="sceneType">场景类型</param>
        public void SetCurScene(GameSceneType sceneType)
        {
            // 确定玩家目前处于房间（CG式场景）还是过道（行走图场景）
            if(m_promptManager != null)
            {
                m_promptManager.SetCurScene(sceneType);
            }
        }

        /// <summary>
        /// 进行提示
        /// </summary>
        /// <param name="type">选择类型</param>
        public void DoPrompt(AssistantSelectType type)
        {
            // TODO: 执行行动提示对话、剧情提示对话
            switch (type)
            {
                case AssistantSelectType.None:
                    break;
                case AssistantSelectType.ActionPrompt:
                    DoActionPrompt();// 行为提示
                    break;
                case AssistantSelectType.PlotPrompt:
                    DoPlotPrompt();// 剧情提示
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void Exit()
        {
            // TODO: 退出助手系统
        }

        /// <summary>
        /// 执行行动提示
        /// </summary>
        private void DoActionPrompt()
        {
            if(m_promptManager != null)
            {
                m_promptManager.DoPrompt();
            }
        }

        /// <summary>
        /// 执行剧情提示
        /// </summary>
        private void DoPlotPrompt()
        {
            if (m_promptManager != null)
            {
                m_promptManager.DoPlotPrompt();
            }
        }

        /// <summary>
        /// 界面显示控制
        /// </summary>
        private AssistantSystemPanel m_view = null;

        /// <summary>
        /// 助手系统 提示管理器
        /// </summary>
        private AssistantPromptManager m_promptManager = new AssistantPromptManager();
    }
}
