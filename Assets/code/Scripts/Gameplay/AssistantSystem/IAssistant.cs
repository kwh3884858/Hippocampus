using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assistant
{
    /// <summary>
    /// 助手系统
    /// </summary>
    public interface IAssistant
    {
        /// <summary>
        /// 进行提示
        /// </summary>
        void DoPrompt(AssistantSelectType type);

        /// <summary>
        /// 退出
        /// </summary>
        void Exit();
    }
}
