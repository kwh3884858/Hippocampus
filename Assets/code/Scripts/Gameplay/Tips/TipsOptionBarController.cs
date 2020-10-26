using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tips
{
    /// <summary>
    /// tips界面选项控制
    /// </summary>
    public class TipsOptionBarController : MonoBehaviour
    {
        [SerializeField]
        private Text m_brief = null;
        [SerializeField]
        private Button m_btn = null;

        public void SetFather(TipsController vFather)
        {
            m_father = vFather;
        }

        public void SetFather(DetectiveNotes.TipsController tipsController)
        {
            m_mother = tipsController;
        }

        public void Init(TipData vData)
        {
            SetData(vData);
            Show();
        }

        public void SetData(TipData vData)
        {
            m_data = vData;
        }

        public void Show()
        {
            SetIsOrNotUnLock(m_data.isUnlock);
            if (m_data.isUnlock)
            {
                SetIsAlreadyClick(m_data.isAlreadyClick);
            }
            SetDetailActive(true);
        }

        public void OnClick()
        {
            if (m_data.isUnlock)
            {
                // TODO: 设置已经点击
                TipsManager.Instance.ClickTip(m_data.tip);// 修改本地数据
                m_data.isAlreadyClick = true;// 修改自身数据
                SetIsAlreadyClick(m_data.isAlreadyClick);// 修改显示
                // TODO: 显示单个tip详情
                if (m_father != null)
                {
                    m_father.ShowTipDetail(m_data);
                }
                if(m_mother != null)
                {
                    m_mother.ShowTipDetail(m_data);
                }
            }
            else
            {
                if (m_father != null)
                {
                    m_father.ShowTipDetail(null);
                }
                if (m_mother != null)
                {
                    m_mother.ShowTipDetail(null);
                }
            }

        }

        private void SetDetailActive(bool vActive)
        {
            if (this.gameObject.activeSelf != vActive)
            {
                this.gameObject.SetActive(vActive);
            }
        }

        /// <summary>
        /// 是否已经解锁
        /// </summary>
        /// <param name="vUnlock"></param>
        private void SetIsOrNotUnLock(bool vUnlock)
        {
            //if (m_btn != null)
            //{
            //    m_btn.interactable = vUnlock;
            //}
            if (vUnlock)
            {
                if (m_brief != null)
                {
                    m_brief.text = m_data.tip;
                }
            }
            else
            {
                if (m_brief != null)
                {
                    m_brief.text = "******";
                }
            }
        }

        /// <summary>
        /// 设置是否已经点击
        /// </summary>
        /// <param name="vIsClick"></param>
        private void SetIsAlreadyClick(bool vIsClick)
        {
            if (m_brief != null)
            {
                if (vIsClick)
                {
                    m_brief.fontStyle = FontStyle.Normal;
                }
                else
                {
                    m_brief.fontStyle = FontStyle.Bold;
                }
            }
        }

        /// <summary>数据</summary>
        private TipData m_data = null;
        /// <summary>tips 管理器</summary>
        private TipsController m_father = null;
        private DetectiveNotes.TipsController m_mother = null;
    }
}

