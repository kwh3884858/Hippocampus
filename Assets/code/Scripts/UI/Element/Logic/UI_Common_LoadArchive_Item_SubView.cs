using System;
using System.Collections;
using System.Collections.Generic;
using LocalCache;
using StarPlatinum;
using StarPlatinum.EventManager;
using UI.Utils;
using UnityEngine;

namespace UI.Panels.Element
{
    public partial class UI_Common_LoadArchive_Item_SubView : UIElementBase
    {
        public override void BindEvent()
        {
            base.BindEvent();
            m_btn_selectArchive_Button.onClick.AddListener(OnSelectArchive);
            m_btn_deleteArchive_Button.onClick.AddListener(OnDeleteSelect);
        }

        public void SetInfo(int index, ArchivePreviewData data, ArchivePanelType type)
        {
            m_index = index;
            DateTime lastPlayTime = new DateTime(data.SaveTime);
            m_lbl_playTime_TextMeshProUGUI.text = UIHelper.GetHMCounterDown(data.TotalPlayTime);
            m_lbl_lastPlayTime_TextMeshProUGUI.text = lastPlayTime.ToString("yyyy/MM/dd\r\nHH:mm:ss");
            m_lbl_epName_TextMeshProUGUI.text = data.EPName;
            PrefabManager.Instance.SetImage(m_img_cg_Image, data.Img);
            m_type = type;
        }

        private void OnSelectArchive()
        {
            if (m_type == ArchivePanelType.Load)
            {
                GameRunTimeData.Instance.ControllerManager.PlayerArchiveController.LoadData(m_index + 1);
            }
            else
            {
                GameRunTimeData.Instance.ControllerManager.PlayerArchiveController.SaveData(m_index);
            }
        }

        public void OnDeleteSelect()
        {
            GameRunTimeData.Instance.ControllerManager.PlayerArchiveController.DeleteArchiveData(m_index);
            EventManager.Instance.SendEvent(new PlayerDeleteArchiveEvent());
        }

        private int m_index;
        private ArchivePanelType m_type;
    }
}