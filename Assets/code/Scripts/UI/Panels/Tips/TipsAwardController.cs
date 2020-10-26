using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine.UI;
using UI.Panels.Providers.DataProviders;
using DG.Tweening;

namespace Tips
{

    public class TipsAwardController : UIPanel<UIDataProviderTip, TipDataProvider>
    {

        //[SerializeField]
        //private Text m_name = null;
        //[SerializeField]
        //private Text m_description = null;
        //[SerializeField]
        //private Image m_tipPanel = null;
        [SerializeField]
        private GameObject m_tipPrefab = null;
        [SerializeField]
        private float m_moveDistanceY = 100f;

        public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            base.Initialize(uiDataProvider, settings);
        }

        public override void ShowData(DataProvider data)
        {
            base.ShowData(data);
            //Init(UIPanelDataProvider.Data);
        }

        public override void UpdateData(DataProvider data)
        {
            base.UpdateData(data);
            m_showDataList.Add((data as TipDataProvider).Data);
            m_data = (data as TipDataProvider).Data;
            CreateTip();
        }

        public void Init(TipData data)
        {
            SetData(data);
            //Show();
            CreateTip();
        }

        private void SetData(TipData data)
        {
            m_data = data;
        }

        private void CreateTip()
        {
            if (m_tipPrefab != null)
            {
                GameObject tip = Instantiate(m_tipPrefab, transform);
                SingleTipGetCtrl tipGetCtrl = tip.GetComponent<SingleTipGetCtrl>();
                if (tipGetCtrl != null)
                {
                    tipGetCtrl.Init(this, m_singleTipGetCtrls.Count, m_showDataList[m_showDataList.Count - 1]);
                    m_singleTipGetCtrls.Add(tipGetCtrl);
                }
                int l = m_singleTipGetCtrls.Count;
                if (l > 3)
                {
                    m_singleTipGetCtrls[0].EndShow();
                }
                if (l > 1)
                {
                    for (int i = 0; i < l - 1; i++)
                    {
                        m_singleTipGetCtrls[i].MoveDown(m_moveDistanceY, 1f);
                    }
                }
            }
        }

        //public void Show()
        //{
        //    m_tipPanel.transform.DOLocalMoveX(460, 1f);
        //    if (m_data != null)
        //    {
        //        //m_name.text = $"Tips:{m_data.tip}";
        //        //m_description.text = $"Tips:{m_data.description}";
        //        m_name.text = m_data.tip;
        //        m_description.text = m_data.description;
        //    }
        //    StartCoroutine(Wait(5f, Close));
        //}

        //private void Close()
        //{
        //    m_tipPanel.transform.DOLocalMoveX(1460, 1f).OnComplete(OnTweeningComplete);
        //}

        //private void OnTweeningComplete()
        //{
        //    m_showDataList.Remove(m_data);
        //    if (m_showDataList.Count == 0)
        //    {
        //        base.InvokeHidePanel();
        //    }
        //    else
        //    {
        //        Init(m_showDataList[0]);
        //    }
        //}

        //private IEnumerator Wait(float _t, System.Action action)
        //{
        //    float vStart = Time.time;
        //    while (Time.time - vStart < _t)
        //    {
        //        yield return null;
        //    }
        //    action?.Invoke();
        //    yield return null;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="closeIndex">tip get index</param>
        public void OnTipGetClose(TipData tipData, SingleTipGetCtrl singleTipGetCtrl)
        {
            m_showDataList.Remove(tipData);
            m_singleTipGetCtrls.Remove(singleTipGetCtrl);
        }

        /// <summary>数据</summary>
        private TipData m_data = null;
        private List<TipData> m_showDataList = new List<TipData>();
        /// <summary>单条提示的控制显示</summary>
        private List<SingleTipGetCtrl> m_singleTipGetCtrls = new List<SingleTipGetCtrl>();
    }
}
