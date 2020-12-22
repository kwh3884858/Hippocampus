using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgePlatformCtrl
{

    /// <summary>
    /// 是否是移动平台（安卓、ios）
    /// </summary>
    public bool IsMobile
    {
        get
        {
            return (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer ||
                GamePlay.Global.SingletonGlobalDataContainer.Instance.MOBILE_MODE);
        }
    }

}
