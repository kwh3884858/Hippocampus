using StarPlatinum;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using StarPlatinum.Base;

namespace Tips
{
	public class TipsManager : Singleton<TipsManager>
	{
		public TipsDataManager tipsData { get; private set; } = null;

		public TipsManager ()
		{

		}

        public void Initialize()
        {
            if (tipsData == null)
            {
                tipsData = new TipsDataManager();
            }
            tipsData.Initialize();
        }

        public void Shutdown()
        {
            if (tipsData == null)
            {
                tipsData = new TipsDataManager();
            }
            tipsData.Shutdown();
        }

        public void AddTip (string vTip)
		{
			if (tipsData != null) {
				tipsData.AddTip (vTip);
			}
#if UNITY_EDITOR
			else {
				Debug.LogWarning ("tipsData is null!");
			}
#endif
		}

		public void RemoveTip (string vTip)
		{
			if (tipsData != null) {
				tipsData.RemoveTip (vTip);
			}
#if UNITY_EDITOR
			else {
				Debug.LogWarning ("tipsData is null!");
			}
#endif
		}

		/// <summary>
		/// 获取指定名称的tip信息
		/// </summary>
		/// <param name="vTip">tip名称</param>
		/// <returns></returns>
		public TipData GetTipDetail (string vTip)
		{
			if (tipsData != null) {
				return tipsData.GetTipDetail (vTip);
			}
#if UNITY_EDITOR
			else {
				Debug.LogWarning ("tipsData is null!");
			}
#endif
			return null;
		}

		/// <summary>
		/// 解锁tip
		/// </summary>
		/// <param name="vTip"></param>
		public void UnlockTip (string vTip, long vTime)
		{
			if (tipsData != null) {
				tipsData.UnlockTip (vTip, vTime);
			}
#if UNITY_EDITOR
			else {
				Debug.LogWarning ("tipsData is null!");
			}
#endif
		}

		/// <summary>
		/// 点击tip
		/// </summary>
		/// <param name="vTip"></param>
		public void ClickTip (string vTip)
		{
			if (tipsData != null) {
				tipsData.ClickTip (vTip);
			}
#if UNITY_EDITOR
			else {
				Debug.LogWarning ("tipsData is null!");
			}
#endif
		}
		/// <summary>
		/// DateTime 转 long
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public static long ConvertDateTimeToLong (DateTime dt)
		{
			DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime (new DateTime (1970, 1, 1));
			TimeSpan toNow = dt.Subtract (dtStart);
			long timeStamp = toNow.Ticks;
			timeStamp = long.Parse (timeStamp.ToString ().Substring (0, timeStamp.ToString ().Length - 4));
			return timeStamp;
		}
	}
}

