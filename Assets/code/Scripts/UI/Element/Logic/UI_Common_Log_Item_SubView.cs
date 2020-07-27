using System.Collections;
using System.Collections.Generic;
using Config.Data;
using Controllers.Subsystems;
using StarPlatinum;
using UnityEngine;
using UnityEngine.UI;
using LogType = Controllers.Subsystems.LogType;

namespace UI.Panels.Element
{
    public partial class UI_Common_Log_Item_SubView : UIElementBase
    {
        private float m_itemSize = 0;
        private RectTransform m_pl_oneLine;
        private RectTransform m_pl_talk;

        public override void BindEvent()
        {
            base.BindEvent();
            m_pl_talk = m_pl_talk_ContentSizeFitter.GetComponent<RectTransform>();
            m_pl_oneLine = m_pl_oneLine_ContentSizeFitter.GetComponent<RectTransform>();

        }

        public void SetInfo(LogInfo logInfo)
        {
            switch (logInfo.LogType)
            {
                case LogType.Talk:
                    SetTalkInfo(logInfo);
                    break;
                case LogType.Jump:
                    SetJumpInfo(logInfo);
                    break;
                case LogType.ShowEvidence:
                    SetShowEvidenceInfo(logInfo);
                    break;
            }
        }

        public float GetItemSize()
        {
            return m_itemSize;
        }

        private void SetTalkInfo(LogInfo logInfo)
        {
            m_pl_oneLine.gameObject.SetActive(false);
            m_pl_talk.gameObject.SetActive(true);
            
            m_img_name_Image.enabled = true;
            m_lbl_name_TextMeshProUGUI.gameObject.SetActive(false);
            string artNameKey = RoleConfig.GetConfigByKey(logInfo.Name)?.artNameKey;
            if (artNameKey == null)
            {
                artNameKey = logInfo.Name;
            }
            PrefabManager.Instance.SetImage(m_img_name_Image,artNameKey, () =>
            {
                m_img_name_Image.enabled = false;
//                m_lbl_name_TextMeshProUGUI.gameObject.SetActive(true);
//                m_lbl_name_TextMeshProUGUI.text = artNameKey;
            });

            m_lbl_name_TextMeshProUGUI.text = logInfo.Name;
            m_lbl_content_TextMeshProUGUI.text = logInfo.Content;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_talk);
            m_itemSize = m_pl_talk.rect.height;
        }

        private void SetJumpInfo(LogInfo logInfo)
        {
            m_pl_oneLine.gameObject.SetActive(true);
            m_pl_talk.gameObject.SetActive(false);
            m_lbl_oneLine_TextMeshProUGUI.text = logInfo.Content;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_oneLine);
            m_itemSize = m_pl_oneLine.rect.height;
        }
        
        private void SetShowEvidenceInfo(LogInfo logInfo)
        {
            m_pl_oneLine.gameObject.SetActive(true);
            m_pl_talk.gameObject.SetActive(false);
            m_lbl_oneLine_TextMeshProUGUI.text = logInfo.Content;
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_oneLine);
            m_itemSize = m_pl_oneLine.rect.height;
        }
    }
}