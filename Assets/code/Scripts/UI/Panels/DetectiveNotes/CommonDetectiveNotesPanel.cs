using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UnityEngine.UI;
using StarPlatinum.EventManager;
using StarPlatinum;

namespace UI.Panels
{
    public partial class CommonDetectiveNotesPanel : UIPanel<UIDataProvider, DataProvider>
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
            EventManager.Instance.SendEvent(new BookMarkEvent() { IsStop = false });
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
            m_data = data as EvidenceDataProvider;
            if (m_data == null)
            {
                m_data = new EvidenceDataProvider();
            }
            RefreshPanel();
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

        public void OnClickClose()
        {
            InvokeHidePanel();
        }

        public void OnClickMaps()
        {
            if (m_data.IsOnEvidence)
            {
                return;
            }
            SwitchType(ShowState.Maps);
        }

        public void OnClickEvidence()
        {
            SwitchType(ShowState.Evidences);
        }

        public void OnClickTips()
        {
            if (m_data.IsOnEvidence)
            {
                return;
            }
            SwitchType(ShowState.Tips);
        }

        private void SwitchType(ShowState curState)
        {
            if (m_curState == curState)
            {
                return;
            }
            m_curState = curState;
            switch (curState)
            {
                case ShowState.Evidences:
                    if (m_mapsCtrl != null)
                    {
                        m_mapsCtrl.Close();
                    }
                    if (m_tipsCtrl != null)
                    {
                        m_tipsCtrl.Close();
                    }
                    FindUI<Button>(transform, "Btn_Maps").interactable = true;
                    FindUI<Button>(transform, "Btn_Tips").interactable = true;
                    FindUI<Button>(transform, "Btn_Evidences").interactable = false;
                    m_evidencesCtrl.Init(InvokeHidePanel, m_data.OnShowEvidence, m_data.IsShowSelectBtn);
                    break;
                case ShowState.Tips:
                    m_evidencesCtrl.HideSelf();
                    if (m_mapsCtrl != null)
                    {
                        m_mapsCtrl.Close();
                    }
                    FindUI<Button>(transform, "Btn_Maps").interactable = true;
                    FindUI<Button>(transform, "Btn_Tips").interactable = false;
                    FindUI<Button>(transform, "Btn_Evidences").interactable = true;
                    //InvokeHidePanel();
                    if (m_tipsCtrl != null)
                    {
                        m_tipsCtrl.Init();
                    }
                    //UIManager.Instance().ShowStaticPanel(UIPanelType.Tipspanel);// 显示tips列表
                    break;
                case ShowState.Maps:
                    m_evidencesCtrl.HideSelf();
                    FindUI<Button>(transform, "Btn_Maps").interactable = false;
                    FindUI<Button>(transform, "Btn_Tips").interactable = true;
                    FindUI<Button>(transform, "Btn_Evidences").interactable = true;
                    //InvokeHidePanel();
                    if (m_mapsCtrl != null)
                    {
                        m_mapsCtrl.Show();
                    }
                    if (m_tipsCtrl != null)
                    {
                        m_tipsCtrl.Close();
                    }
                    //UIManager.Instance().ShowStaticPanel(UIPanelType.UIMapcanvasPanel);// 显示地图
                    break;
                default:
                    m_evidencesCtrl.HideSelf();
                    break;
            }
        }

        private void RefreshPanel()
        {
            EventManager.Instance.SendEvent(new BookMarkEvent() { IsStop = m_data.IsOnEvidence });
            if (m_data.IsOnEvidence)
            {
                FindUI<Button>(transform, "Btn_Maps").interactable = false;
                FindUI<Button>(transform, "Btn_Tips").interactable = false;
                FindUI<Button>(transform, "Btn_Evidences").interactable = false;
                FindUI<Button>(transform, "Btn_Close").interactable = false;
            }
            m_evidencesCtrl.ShowEvidenceEnable(m_data.IsOnEvidence);
            SwitchType(ShowState.Evidences);
        }

        [SerializeField]
        private EvidencesController m_evidencesCtrl = null;
        [SerializeField]
        private MapsController m_mapsCtrl = null;
        [SerializeField]
        private DetectiveNotes.TipsController m_tipsCtrl = null;

        private EvidenceDataProvider m_data;
        private ShowState m_curState = ShowState.None;

        public enum ShowState
        {
            None = 0,
            Evidences = 1,
            Tips = 2,
            Maps = 3
        }

        #endregion
    }
}