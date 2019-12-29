using System;
using System.Collections.Generic;
using Const;
using Extentions;
using StarPlatinum;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI.Panels
{
    public class UIPanel : MonoBehaviour, IUiPanel
    {
        public event Action<UIPanelType, bool> HidePanelEvent = null;
        
        public event Action<UIPanelType, DataProvider> ShowPanelEvent = null;
        
        public event Action<UIPanelType, DataProvider> UpdatePanelEvent = null;
        
        public event Action<UIPanelType, UIPanelType, DataProvider> ShowAsSubpanelEvent = null;
        
        public event Action<UIPanelType, UIPanelType, DataProvider> ChangePanelEvent = null;
        
        public event Action<UIPanelType, DataProvider> PanelDataChangedEvent = null;

        protected UIPanelType CurrentSubPanel { get; private set; }
        protected UIPanelType PrevSubPanel { get; private set; }

        protected IconProvider IconProvider => UiDataProvider.IconProvider;


        protected virtual void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public virtual void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            m_uiDataProvider = uiDataProvider;
            m_uiPanelSettings = settings;
            PanelType = m_uiPanelSettings.PanelType;


            // Debug.Log(GetType());
        }

        public virtual void DeInitialize()
        {
            if (PanelState.IsShow())
            {
                Hide();
            }
        }

        public virtual void Tick()
        {
        }

        public virtual void LateTick()
        {
        }

        public virtual void ShowData(DataProvider data)
        {
            Assert.IsTrue(PanelState != UIPanelState.WaitData && PanelState != UIPanelState.Show, "Panel " + GetType() + " already show");
            m_dataProvider = data;
            PanelState = UIPanelState.Show;

        }

        public virtual void UpdateData(DataProvider data)
        {
            m_dataProvider = data;
        }

        public virtual void Hide()
        {
            Assert.IsTrue(PanelState != UIPanelState.Hide, "Panel " + GetType() + " already hide");
            PanelState = UIPanelState.Hide;

        }

        public virtual void Deactivate()
        {
            PanelState = UIPanelState.Deactivate;
            gameObject.SetActive(false);
        }

        public virtual void SubpanelChanged(UIPanelType type, DataProvider data = null)
        {
            PrevSubPanel = CurrentSubPanel;
            CurrentSubPanel = type;
        }

        public virtual void SubpanelDataChanged(UIPanelType type, DataProvider data)
        {
        }

        protected void InvokePanelDataChanged(UIPanelType type, DataProvider data)
        {
            PanelDataChangedEvent?.Invoke(type, data);
        }

        public void InvokeHidePanel(bool forceDeactivate)
        {
            HidePanelEvent?.Invoke(PanelType, forceDeactivate);
        }
        
        public void InvokeHidePanel()
        {
            HidePanelEvent?.Invoke(PanelType, false);
        }

        protected void InvokeHidePanel(UIPanelType type, bool forceDeactivate = false)
        {
            HidePanelEvent?.Invoke(type, forceDeactivate);
        }

        protected void InvokeShowPanel(UIPanelType type, DataProvider data = null)
        {
            ShowPanelEvent?.Invoke(type, data);
        }

        protected void InvokeUpdatePanel(UIPanelType type, DataProvider data)
        {
            UpdatePanelEvent?.Invoke(type, data);
        }

        protected void InvokeShowAsSubpanel(UIPanelType ownerType, UIPanelType type, DataProvider data = null)
        {
            ShowAsSubpanelEvent?.Invoke(ownerType, type, data);
        }

        protected void InvokeChangePanel(UIPanelType type, DataProvider data = null)
        {
            ChangePanelEvent?.Invoke(PanelType, type, data);
        }

        protected void InvokeChangePanel(UIPanelType changedType, UIPanelType type, DataProvider data = null)
        {
            ChangePanelEvent?.Invoke(changedType, type, data);
        }

        protected void CallbackTime(float delayTime ,Action callback)
        {
            TimerService.Instance().AddTimer(delayTime,()=>
            {
                callback?.Invoke();
            });
        }

        public void ClickSound(int num)
        {
            UiDataProvider.SoundService.PlayEffect(SoundNameConst.UIClickName+num);
        }


        #region IUiPanel interface implementation
        UIDataProvider IUiPanel.UiDataProvider => UiDataProvider;
        void IUiPanel.InvokePanelDataChanged(UIPanelType type, DataProvider data)
        {
            InvokePanelDataChanged(type, data);
        }
        void IUiPanel.InvokeHidePanel(UIPanelType type, bool forceDeactivate)
        {
            InvokeHidePanel(type, forceDeactivate);
        }
        void IUiPanel.InvokeShowPanel(UIPanelType type, DataProvider data)
        {
            InvokeShowPanel(type, data);
        }
        void IUiPanel.InvokeUpdatePanel(UIPanelType type, DataProvider data)
        {
            InvokeUpdatePanel(type, data);
        }
        void IUiPanel.InvokeShowAsSubpanel(UIPanelType ownerType, UIPanelType type, DataProvider data)
        {
            InvokeShowAsSubpanel(ownerType, type, data);
        }
        void IUiPanel.InvokeChangePanel(UIPanelType type, DataProvider data)
        {
            InvokeChangePanel(type, data);
        }
        void IUiPanel.InvokeChangePanel(UIPanelType changedType, UIPanelType type, DataProvider data)
        {
            InvokeChangePanel(changedType, type, data);
        }
        #endregion

        public UIPanelType PanelType { get; protected set; }

        public UIPanelState PanelState
        {
            get { return m_panelState; }
            internal set { m_panelState = value; }
        }

        protected virtual RectTransform Rect
        {
            get { return GetComponent<RectTransform>(); }
        }

        protected UIDataProvider UiDataProvider
        {
            get { return m_uiDataProvider; }
        }

        public DataProvider UIPanelDataProvider
        {
            get { return m_dataProvider; }
        }

        public UIPanelSettings PanelSettings
        {
            get { return m_uiPanelSettings; }
        }

        public Animator Animator
        {
            get { return m_animator; }
        }



        public Transform SubpanelHolder
        {
            get { return m_subpanelHolder; }
        }

        public bool PlaySoundOnShowHide => m_playSoundOnShowHide;

        [SerializeField]
        protected Transform m_subpanelHolder = null;
        [SerializeField]
        protected float m_moveHidingSpeed = 0.25f;
        [SerializeField]
        private bool m_missClickHide = false;
        [SerializeField]
        private bool m_playSoundOnShowHide = true;


        protected UIDataProvider m_uiDataProvider = null;
        protected DataProvider m_dataProvider = null;
        protected UIPanelSettings m_uiPanelSettings = null;

        private Animator m_animator;
        private UIPanelState m_panelState = UIPanelState.Initialize;
    }
    
    public abstract class UIPanel<T1, T2> : UIPanel
        where T1 : UIDataProvider
        where T2 : DataProvider
    {
        protected new T1 UiDataProvider
        {
            get; private set;
        }

        public new T2 UIPanelDataProvider
        {
            get; private set;
        }

        public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            base.Initialize(uiDataProvider, settings);

            m_uiDataProvider = uiDataProvider;
            UiDataProvider = m_uiDataProvider as T1;
            m_uiPanelSettings = settings;
            PanelType = PanelSettings.PanelType;
        }

        public override void ShowData(DataProvider data)
        {
            base.ShowData(data);
            UIPanelDataProvider = data as T2;
        }

        public override void UpdateData(DataProvider data)
        {
            base.UpdateData(data);
            UIPanelDataProvider = data as T2;
        }
    }
}
