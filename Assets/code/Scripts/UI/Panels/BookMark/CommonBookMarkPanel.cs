using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using StarPlatinum.EventManager;
using StarPlatinum;
using System;

namespace UI.Panels
{
    public partial class CommonBookMarkPanel : UIPanel<UIDataProvider, DataProvider>
    {
        #region UI template method
        public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            base.Initialize(uiDataProvider, settings);
            m_model.Initialize(this);
            EventManager.Instance.AddEventListener<SettingStateEvent>(OnSettingStateChange);
            EventManager.Instance.AddEventListener<BookMarkEvent>(OnBookMarkChange);
        }

        public override void DeInitialize()
        {
            m_model.Deactivate();
            base.DeInitialize();
        }

        public override void Hide()
        {
            m_model.Hide();
            base.Hide();
            EventManager.Instance.RemoveEventListener<SettingStateEvent>(OnSettingStateChange);
            EventManager.Instance.RemoveEventListener<BookMarkEvent>(OnBookMarkChange);
        }

        public override void Deactivate()
        {
            m_model.Deactivate();
            base.Deactivate();
        }

        public override void ShowData(DataProvider data)
        {
            m_model.ShowData(data);
            base.ShowData(data);
        }

        public override void UpdateData(DataProvider data)
        {
            m_model.UpdateData(data);
            base.UpdateData(data);
        }

        public override void Tick()
        {
            m_model.Tick();
            base.Tick();
        }

        public override void LateTick()
        {
            m_model.LateTick();
            base.LateTick();
        }

        public override void SubpanelChanged(UIPanelType type, DataProvider data = null)
        {
            m_model.SubpanelChanged(type, data);
            base.SubpanelChanged(type, data);
        }

        public override void SubpanelDataChanged(UIPanelType type, DataProvider data)
        {
            m_model.SubpanelDataChanged(type, data);
            base.SubpanelDataChanged(type, data);
        }
        #endregion

        #region Member
        public void OnClickSave()
        {
            if (isStop)
            {
                return;
            }
            RefreshView();
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonLoadarchivePanel, new ArchiveDataProvider() { Type = ArchivePanelType.Save, OnClose = SetUnSelectState });
            showSaveFile = true;
        }

        public void OnClickAssistant()
        {
            if (isStop)
            {
                return;
            }
            RefreshView();
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonAssistantPanel, new AssistantDataProvider() { OnClose = SetUnSelectState });// 显示助手窗
            showAssistant = true;
        }

        public void OnClickDetectiveNotes()
        {
            if (isStop)
            {
                return;
            }
            RefreshView();
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonDetectiveNotesPanel, new DetectiveNotesDataProvider() { OnClose = SetUnSelectState });// 显示证物列表
            showDetectiveNotes = true;

        }

        public void OnClickSetting()
        {
            if (isStop)
            {
                return;
            }
            // TODO: show setting panel
            RefreshView();
            bookMarkControllers[0].SetSelectState();
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonSettingPanel);// 显示设置界面
            showDetectiveNotes = true;
        }

        private void SetUnSelectState()
        {
            for (int i = 0; i < bookMarkControllers.Count; i++)
            {
                bookMarkControllers[i].SetUnSelectState();
            }
        }

        private void RefreshView()
        {
            if (showAssistant)
            {
                UIManager.Instance().HideStaticPanel(UIPanelType.UICommonAssistantPanel);
                showAssistant = false;
            }
            if (showDetectiveNotes)
            {
                UIManager.Instance().HideStaticPanel(UIPanelType.UICommonDetectiveNotesPanel);
                showDetectiveNotes = false;
            }
            if (showSaveFile)
            {
                UIManager.Instance().HideStaticPanel(UIPanelType.UICommonLoadarchivePanel);
                showSaveFile = false;
            }
            if (showSetting)
            {
                UIManager.Instance().HideStaticPanel(UIPanelType.UICommonSettingPanel);
                showSetting = false;
            }
        }

        private void OnSettingStateChange(object sender, SettingStateEvent e)
        {
            if (e.IsShow)
            {
                OnClickSetting();
            }
            else
            {
                SetUnSelectState();
            }
        }

        private void OnBookMarkChange(object sender, BookMarkEvent e)
        {
            if (e != null)
            {
                ChangeBookMarkState(e.IsStop);
            }
        }

        private void ChangeBookMarkState(bool isStop)
        {
            this.isStop = isStop;
            if (this.isStop)
            {
                SetUnSelectState();
                SetStopState();
            }
        }

        private void SetStopState()
        {
            for (int i = 0; i < bookMarkControllers.Count; i++)
            {
                bookMarkControllers[i].SetStopState(isStop);
            }
        }

        [SerializeField]
        private List<SingleBookMarkController> bookMarkControllers = new List<SingleBookMarkController>();
        private bool showAssistant = false;
        private bool showDetectiveNotes = false;
        private bool showSaveFile = false;
        private bool showSetting = false;
        private bool isStop = false;
        #endregion
    }
}