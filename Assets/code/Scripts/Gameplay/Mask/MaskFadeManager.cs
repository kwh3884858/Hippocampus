using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Base;
using UI.Panels.Providers.DataProviders;

public class MaskFadeManager : Singleton<MaskFadeManager>
{
    /// <summary>
    /// 显示遮罩
    /// </summary>
    /// <param name="maskType">遮罩类型</param>
    /// <param name="inTime">渐入时间</param>
    /// <param name="stayTime">停留时间</param>
    /// <param name="outTime">渐出时间</param>
    public void ShowMask(MaskType maskType, float inTime = 0.3f, float stayTime = 0.3f, float outTime = 0.3f)
    {
        UI.UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonMaskPanel, new MaskDataProvider()
        {
            MyType = maskType,
            InTime = inTime,
            StayTime = stayTime,
            OutTime = outTime
        });
    }
}

/// <summary>
/// 遮罩类型
/// </summary>
public enum MaskType
{
    /// <summary>fade in and fade out mask</summary>
    Normal = 0,
    /// <summary>time shape fade in and fade out mask</summary>
    TimeShape,
    /// <summary>diffusion shape fade in and fade out mask</summary>
    DiffusionShape,
    /// <summary>fence shape fade in and fade out mask</summary>
    FenceShape,
    /// <summary>shuttle shape fade in and fade out mask</summary>
    ShuttleShape
}