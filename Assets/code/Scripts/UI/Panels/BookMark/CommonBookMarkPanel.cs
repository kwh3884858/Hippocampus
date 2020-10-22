using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;

namespace UI.Panels
{
    public partial class CommonBookMarkPanel : UIPanel<UIDataProvider, DataProvider>
    {
        #region UI template method
        public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            base.Initialize(uiDataProvider, settings);
            m_model.Initialize(this);
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
            // TODO:show save panel
        }

        public void OnClickAssistant()
        {
            if (showDetectiveNotes)
            {
                UIManager.Instance().HideStaticPanel(UIPanelType.UICommonMapsTipsEvidencesPanel);
                showDetectiveNotes = false;
            }
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonAssistantPanel, new AssistantDataProvider() { OnClose = SetUnSelectState });// 显示助手窗
            showAssistant = true;
        }

        public void OnClickDetectiveNotes()
        {
            // TODO: show detective notes panel
            if (showAssistant)
            {
                UIManager.Instance().HideStaticPanel(UIPanelType.UICommonAssistantPanel);
                showAssistant = false;
            }
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonMapsTipsEvidencesPanel, new DetectiveNotesDataProvider() { OnClose = SetUnSelectState });// 显示证物列表
            showDetectiveNotes = true;

        }

        public void OnClickSetting()
        {
            // TODO: show setting panel
        }

        private void SetUnSelectState()
        {
            for (int i = 0; i < bookMarkControllers.Count; i++)
            {
                bookMarkControllers[i].SetUnSelectState();
            }
        }

        [SerializeField]
        private List<SingleBookMarkController> bookMarkControllers = new List<SingleBookMarkController>();
        private bool showAssistant = false;
        private bool showDetectiveNotes = false;
        #endregion
    }
}