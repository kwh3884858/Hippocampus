using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assistant
{
    /// <summary>
    /// 游戏的场景类型
    /// </summary>
    public enum GameSceneType
    {
        None = 0,
        /// <summary>
        /// 房间（CG式场景）
        /// </summary>
        Room,
        /// <summary>
        /// 过道（行走图场景）
        /// </summary>
        Aisle
    }

    /// <summary>
    /// 提示选择类型
    /// </summary>
    public enum AssistantSelectType
    {
        None = 0,
        /// <summary>
        /// 行为提示
        /// </summary>
        ActionPrompt,
        /// <summary>
        /// 剧情提示
        /// </summary>
        PlotPrompt
    }
}
