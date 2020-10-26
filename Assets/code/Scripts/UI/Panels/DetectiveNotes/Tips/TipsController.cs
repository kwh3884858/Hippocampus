using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tips;
using UnityEngine;
namespace DetectiveNotes
{

    public class TipsController : MonoBehaviour
    {
        [SerializeField]
        private TipsOptionBarController m_tip = null;
        [SerializeField]
        private Transform m_content = null;
        [SerializeField]
        private TipDetailController m_tipDetail = null;

        public void Init()
        {
            SetData();
            Show();
        }

        private void SetData()
        {
            //m_dataManager = TipsManager.Instance.tipsData;
            m_tipDataList = TipsManager.Instance.tipsData.MyTipsDic.Values.ToList();
            SortTips();
            //m_tipList.Clear ();
        }

        public void Show()
        {
            gameObject.SetActive(true);
            CloseTipDetail();
            CreateTips();
        }

        /// <summary>
        /// 显示详情
        /// </summary>
        /// <param name="vData">数据</param>
        public void ShowTipDetail(TipData vData)
        {
            if (m_tipDetail != null)
            {
                m_tipDetail.Init(vData);
            }
        }

        /// <summary>
        /// 关闭详情显示
        /// </summary>
        public void CloseTipDetail()
        {
            if (m_tipDetail != null)
            {
                m_tipDetail.Close();
            }
        }

        public int Compare(TipData x, TipData y)
        {
            if (x.isUnlock && !y.isUnlock)
            {
                return -1;//不变
            }
            else if (!x.isUnlock && y.isUnlock)
            {
                return 1;// 交换
            }
            else if (x.isUnlock && y.isUnlock)
            {
                // 都是解锁，按照解锁时间比较
                return y.time.CompareTo(x.time);// 降序
            }
            else
            {
                // 未解锁
                return x.tip.CompareTo(y.tip);// 升序
            }
        }

        /// <summary>
        /// 创建证据显示，暂时使用gameobject创建
        /// </summary>
        private void CreateTips()
        {
            ClearTipList();
            if (m_tip != null && m_content != null)
            {
                TipsOptionBarController vTmpCtrl = null;
                foreach (var data in m_tipDataList)
                {
                    vTmpCtrl = GameObject.Instantiate(m_tip, m_content);
                    vTmpCtrl.SetFather(this);
                    vTmpCtrl.Init(data);
                    //vTmpCtrl.OnClick();
                    m_tipList.Add(vTmpCtrl);
                }
            }
        }

        private void ClearTipList()
        {
            int vL = m_tipList.Count;
            for (int i = 0; i < vL; i++)
            {
                Destroy(m_tipList[i].gameObject);
            }
            m_tipList.Clear();
        }

        private void SortTips()
        {
            if (m_tipDataList != null)
            {
                m_tipDataList.Sort(Compare);
            }
        }
        public void Close()
        {
            gameObject.SetActive(false);
            for (int i = 0; i < m_tipList.Count; i++)
            {
                Destroy(m_tipList[i].gameObject);
            }
            m_tipList.Clear();
        }

        //private TipsDataManager m_dataManager = null;
        /// <summary>保存的tip列表</summary>
        private List<TipData> m_tipDataList = null;
        private List<TipsOptionBarController> m_tipList = new List<TipsOptionBarController>();
    }

}
