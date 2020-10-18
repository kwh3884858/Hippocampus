using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assistant
{
    /// <summary>
    /// 助手系统 提示接口
    /// </summary>
    public interface IAssistantPrompt
    {
        /// <summary>
        /// 进行提示
        /// </summary>
        void DoPrmpt();
    }
}
