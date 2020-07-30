using System.Collections;
using System.Collections.Generic;
//using StarPlatinum;
using UnityEngine;
using Newtonsoft.Json;
using StarPlatinum.EventManager;
using StarPlatinum;

namespace Tips
{
    public class TipsDataManager
    {

        /// <summary>已经获得的tips</summary>
        public Dictionary<string, TipData> MyTipsDic { private set; get; } = null;

        public TipsDataManager()
        {
            Initialize();
        }

        public void Initialize()
        {
            LoadData();
            EventManager.Instance.AddEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.AddEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
        }

        public void Shutdown()
        {
            EventManager.Instance.RemoveEventListener<PlayerPreSaveArchiveEvent>(OnPlayerPreSaveArchive);
            EventManager.Instance.RemoveEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
        }

        /// <summary>
        /// 加载本地数据
        /// </summary>
        private void LoadData()
        {
            //MyTipsDic = new Dictionary<string, TipData>();
            //if (LocalData.HasKey(tipsName))
            //{
            //    m_data = JsonConvert.DeserializeObject<TipsData>(LocalData.ReadStr(tipsName, ""));
            //}
            //else
            //{
            //    m_data = new TipsData();
            //}

            m_tipsConfig = ConfigData.Instance.tipsConfig.GetDicDetails();
            //if (m_tipsConfig != null)
            //{
            //    TipData tipData;
            //    foreach (var data in m_tipsConfig)
            //    {
            //        if (m_data.tipsList.ContainsKey(data.Key))
            //        {
            //            tipData = m_data.tipsList[data.Key];
            //            MyTipsDic.Add(data.Value.tip, new TipData(data.Value.tip, data.Value.description, tipData.isUnlock, tipData.time, tipData.isAlreadyClick));
            //        }
            //        else
            //        {
            //            MyTipsDic.Add(data.Value.tip, new TipData(data.Value.tip, data.Value.description, false));
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 添加tip
        /// </summary>
        /// <param name="vTip"></param>
        public void AddTip(string vTip)
        {
            if (MyTipsDic.ContainsKey(vTip))// 已经存在
            {
#if UNITY_EDITOR
                Debug.LogWarning("tip already contain");
#endif
                return;
            }
            if (m_tipsConfig.ContainsKey(vTip))
            {
                TipsConfig.Detail data = m_tipsConfig[vTip];
                MyTipsDic.Add(vTip, new TipData(data.tip, data.description));
                //SaveData();
            }
#if UNITY_EDITOR
            else
            {
                Debug.Log("tip name not contain in tip Table!");
            }
#endif
        }

        /// <summary>
        /// 移除tip
        /// </summary>
        /// <param name="vTip"></param>
        public void RemoveTip(string vTip)
        {
            if (m_tipsConfig.ContainsKey(vTip))
            {
                MyTipsDic.Remove(vTip);
                //SaveData();
            }
#if UNITY_EDITOR
            else
            {
                Debug.Log("tip name not contain in tip Table!");
            }
#endif
        }

        /// <summary>
        /// 获取指定名称的tip信息
        /// </summary>
        /// <param name="vTip">tip名称</param>
        /// <returns></returns>
        public TipData GetTipDetail(string vTip)
        {
            if (m_tipsConfig.ContainsKey(vTip))
            {
                TipsConfig.Detail detail = m_tipsConfig[vTip];
                return new TipData(detail.tip, detail.description);
            }
            return null;
        }

        /// <summary>
        /// 解锁tip
        /// </summary>
        /// <param name="vTip">tip</param>
        public void UnlockTip(string vTip, long vTime)
        {
            if (MyTipsDic.ContainsKey(vTip))
            {
                if (!MyTipsDic[vTip].isUnlock)
                {
                    MyTipsDic[vTip].isUnlock = true;
                    MyTipsDic[vTip].time = vTime;
                    //SaveData();
                    //UIManager.Instance().ShowPanel(UIPanelType.Tipgetpanel, new UI.Panels.Providers.DataProviders.TipDataProvider() { Data = MyTipsDic[vTip] });// 显示UI
                    TipData data = MyTipsDic[vTip];
                    data.tip = "Tips:" + data.tip;
                    data.description = "Tips:" + data.description;
                    if (UI.UIManager.Instance().IsPanelShow(UIPanelType.Tipgetpanel))
                    {
                        UI.UIManager.Instance().UpdateData(UIPanelType.Tipgetpanel, new UI.Panels.Providers.DataProviders.TipDataProvider() { Data = data });
                    }
                    else
                    {
                        UI.UIManager.Instance().ShowStaticPanel(UIPanelType.Tipgetpanel, new UI.Panels.Providers.DataProviders.TipDataProvider() { Data = data });// 显示UI
                    }
                }
#if UNITY_EDITOR
                else
                {
                    Debug.LogWarning("tip already unlock");
                }
#endif
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning("tip name not contain");
            }
#endif
        }

        /// <summary>
        /// 设置点击
        /// </summary>
        /// <param name="vTip"></param>
        public void ClickTip(string vTip)
        {
            if (MyTipsDic.ContainsKey(vTip))
            {

                if (!MyTipsDic[vTip].isAlreadyClick)
                {
                    MyTipsDic[vTip].isAlreadyClick = true;
                    //SaveData();
                }
#if UNITY_EDITOR
                else
                {
                    Debug.LogWarning("tip already click");
                }
#endif
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogWarning("tip name not contain");
            }
#endif
        }

        /// <summary>
        /// 保存信息到本地
        /// </summary>
        public void SaveData()
        {
            //m_data.tipsList = MyTipsDic;
            //LocalData.WriteStr(tipsName, JsonConvert.SerializeObject(m_data));
        }

        private void OnPlayerPreSaveArchive(object sender, PlayerPreSaveArchiveEvent e)
        {
            GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.TipsArchiveData.TipsList = MyTipsDic;
        }

        private void OnPlayerLoadArchive(object sender, PlayerLoadArchiveEvent e)
        {
            MyTipsDic = GlobalManager.GetControllerManager().PlayerArchiveController.CurrentArchiveData.TipsArchiveData.TipsList;
            if (MyTipsDic == null)
            {
                MyTipsDic = new Dictionary<string, TipData>();
                if (m_tipsConfig != null)
                {
                    TipData tipData;
                    foreach (var data in m_tipsConfig)
                    {
                        MyTipsDic.Add(data.Value.tip, new TipData(data.Value.tip, data.Value.description, false));
                    }
                }
            }
        }

        /// <summary>保存本地的名称</summary>
        //private static readonly string tipsName = "Tips";

        /// <summary>读取的本地配置文件信息</summary>
        private Dictionary<string, TipsConfig.Detail> m_tipsConfig = null;
        /// <summary>本地保存的数据</summary>
        //private TipsData m_data = new TipsData();
    }
}