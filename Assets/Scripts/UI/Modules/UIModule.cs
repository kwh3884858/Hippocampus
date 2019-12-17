
#define DEBUG_UI_MODULE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Extentions;
using UI.Panels;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;

namespace UI.Modules
{
    public abstract class UIModule : MonoBehaviour
    {
        private class UIPanelOperation
        {
            public enum OperationType
            {
                ShowPanel,
                HidePanel,
                ShowAsSubpanel,
                ChangePanel,
                UpdatePanel,
            }

            public OperationType Type { get; protected set; }
            public UIPanelType PanelType { get; protected set; }
            public DataProvider Data { get; protected set; } = null;

            public UIPanelOperation(OperationType type, UIPanelType panel, DataProvider data = null)
            {
                m_id = s_id++;
                Type = type;
                PanelType = panel;
                Data = data;
            }

            public override string ToString()
            {
                //{base.ToString()} 
                return $"({m_id}. {Type} {PanelType})";
            }

            private static int s_id = 0;
            protected readonly int m_id = 0;
        }

        private class UIPanelOperationWithOwner : UIPanelOperation
        {
            public UIPanelType OwnerPanelType { get; }

            public UIPanelOperationWithOwner(OperationType type, UIPanelType owner, UIPanelType panel, DataProvider data = null) : base(type, panel, data)
            {
                Type = type;
                OwnerPanelType = owner;
                PanelType = panel;
                Data = data;
            }

            public override string ToString()
            {
                //{base.ToString()} 
                return $"({m_id}. {Type} {OwnerPanelType} => {PanelType})";
            }
        }

        private class UIPanelHideOperation : UIPanelOperation
        {
            public bool ForceDeactivate { get; }

            public UIPanelHideOperation(OperationType type, bool forceDeactivate, UIPanelType panel, DataProvider data = null) : base(type, panel, data)
            {
                Type = type;
                ForceDeactivate = forceDeactivate;
                PanelType = panel;
                Data = data;
            }
        }
        
        public event Action<UIPanel> OnPanelShowed = null;
        public event Action<UIPanel> OnPanelHide = null;

        [Conditional("DEBUG_UI_MODULE")]
        [DebuggerStepThrough]
        private void Log(string msg)
        {
            Debug.Log( $"{GetType()} ({GetHashCode()}). {msg}" + GetAdditionalDebugInfo(), this);
        }

        public UIDataProvider UiDataProvider
        {
            get { return m_uiDataProvider; }
            private set { m_uiDataProvider = value; }
        }

        private bool IsQueue
        {
            get { return m_isQueue; }
            set
            {
                m_isQueue = value;
            }
        }

        public virtual void Initialize(UIDataProvider uiDataProvider, Skylight.PrefabManager content)
        {
            Log("Initialize");
            UiDataProvider = uiDataProvider;
            m_content = content;

            GameObject root = new GameObject(GetType().Name);
            m_container = root.AddComponent<RectTransform>();
            m_container.SetParent(transform, false);
            m_container.SetSiblingIndex(0);
            m_container.anchorMin = Vector2.zero;
            m_container.anchorMax = Vector2.one;
            m_container.offsetMin = Vector2.zero;
            m_container.offsetMax = Vector2.zero;
        }

        public virtual void DeInitialize()
        {
            StopAllCoroutines();
            foreach (var uiPanel in m_panels)
            {
                uiPanel.Value.ChangePanelEvent -= OnChangePanelHandler;
                uiPanel.Value.HidePanelEvent -= OnHidePanelHandler;
                uiPanel.Value.ShowPanelEvent -= OnShowPanelHandler;
                uiPanel.Value.UpdatePanelEvent -= OnUpdatePanelHandler;
                uiPanel.Value.ShowAsSubpanelEvent -= OnShowAsSubpanelHandler;
                uiPanel.Value.PanelDataChangedEvent -= OnPanelDataChangedHandler;

                uiPanel.Value.DeInitialize();
                Destroy(uiPanel.Value.gameObject);
            }

            m_panels.Clear();
            m_subpanels.Clear();
            m_loadingPanels.Clear();
            m_deactivatedPanels.Clear();

            if (m_operationCoroutine != null)
            {
                StopCoroutine(m_operationCoroutine);
                m_operationCoroutine = null;
            }
            if (m_queueCoroutine != null)
            {
                StopCoroutine(m_queueCoroutine);
                m_queueCoroutine = null;
            }
            m_currentOperation = null;
            m_operationQueue.Clear();
            if (IsQueue)
            {
                IsQueue = false;
            }
            Destroy(m_container.gameObject);
            Log("DeInitialize");
        }

        public void Activate()
        {
            Log("Activate");
            if (m_deactivatedPanels.Count > 0)
            {
                foreach (var deactivatedPanel in m_deactivatedPanels)
                {
                    ShowPanel(deactivatedPanel);
                }
                m_deactivatedPanels.Clear();
            }
            else
            {
                foreach (var settings in m_panelsSettings)
                {
                    if (settings.ShowOnStart)
                    {
                        Assert.IsTrue(m_panelsSettings.Find(c => c.ShowOnStart) != null, "Start panel " + settings.PanelType + " not found in Panels Settings");
                        StartCoroutine(ShowPanelAsync(UIPanelType.None, settings.PanelType));
                    }
                }
            }
        }

        public void Deactivate()
        {
            Log("Deactivate");
            List<UIPanelType> types = new List<UIPanelType>();
            foreach (var loadingPanel in m_panels)
            {
                if (m_subpanels.ContainsValue(loadingPanel.Key))
                {
                    continue;
                }
                if (loadingPanel.Value.PanelState == UIPanelState.Show)
                {
                    types.Add(loadingPanel.Key);
                }
            }
            foreach (var type in types)
            {
                m_deactivatedPanels.Add(type);
                HidePanel(type);
            }
        }

        public virtual void Tick()
        {
            foreach (var uiPanel in m_panels)
            {
                if (!uiPanel.Value.PanelState.IsShow())
                {
                    continue;
                }

                uiPanel.Value.Tick();
            }
        }

        public virtual void LateTick()
        {
            foreach (var uiPanel in m_panels)
            {
                if (!uiPanel.Value.PanelState.IsShow())
                {
                    continue;
                }

                uiPanel.Value.LateTick();
            }
        }

        public T GetPanel<T>() where T : UIPanel
        {
            foreach (var uiPanel in m_panels)
            {
                var panel = uiPanel.Value as T;
                if (panel != null)
                {
                    return panel;
                }
            }

            throw new NullReferenceException();
        }

        private void OnPanelDataChangedHandler(UIPanelType type, DataProvider data)
        {
            foreach (var subpanel in m_subpanels)
            {
                if (subpanel.Value == type)
                {
                    m_panels[subpanel.Key].SubpanelDataChanged(type, data);
                }
            }
        }

        private void OnShowPanelHandler(UIPanelType type, DataProvider data = null)
        {
            ShowPanel(type, data);
        }

        private void OnUpdatePanelHandler(UIPanelType type, DataProvider data)
        {
            UpdatePanel(type, data);
        }

        private void OnHidePanelHandler(UIPanelType type, bool forceDeactivate = false)
        {
            HidePanel(type, forceDeactivate);
        }

        private void OnShowAsSubpanelHandler(UIPanelType ownerType, UIPanelType type, DataProvider data = null)
        {
            ShowAsSubpanel(ownerType, type, data);
        }

        private void OnChangePanelHandler(UIPanelType hideType, UIPanelType openType, DataProvider data = null)
        {
            ChangePanel(hideType, openType, data);
        }

        private string GetPathByAttribute(UIPanelType panelType)
        {
            var atrs = panelType.GetType().GetMember(panelType.ToString());
            var attributes = atrs[0].GetCustomAttributes(typeof(AssetPathAttribute), false);
            var atribute = (AssetPathAttribute)attributes[0];
            return atribute.Path;
        }

        protected IEnumerator LoadPanelAsync(UIPanelSettings settings)
        {
            Log("LoadPanelAsync " + settings.PanelType);
            m_loadingPanels.Add(settings.PanelType);
            var assetKey = GetPathByAttribute(settings.PanelType);
            UIPanel panel = null; 
            m_content.InstantiateAsync<UIPanel>(assetKey, (newPanel) => { panel = newPanel;},m_container);
            yield return new WaitUntil(()=>panel != null);
            Assert.IsNotNull(panel, $"LoadPanelAsync failed for {settings.PanelType}: key={assetKey}");

            InstantiatePanel(panel, settings);
            m_loadingPanels.Remove(settings.PanelType);
            Log("End LoadPanelAsync " + settings.PanelType);
        }

        protected async void LoadPanelSync(UIPanelSettings settings)
        {
            Log("LoadPanelSync " + settings.PanelType);
            var assetKey = GetPathByAttribute(settings.PanelType);
            var panel = await m_content.InstantiateAsyncAwait<UIPanel>(assetKey,m_container);
            Assert.IsNotNull(panel, $"LoadPanelSync failed for {settings.PanelType}: guid={assetKey}");
            InstantiatePanel(panel, settings);
        }

        protected void InstantiatePanel(UIPanel panel, UIPanelSettings settings)
        {
            Log("InstantiatePanel " + settings.PanelType);
            m_panels.Add(settings.PanelType, panel);
            panel.ChangePanelEvent += OnChangePanelHandler;
            panel.HidePanelEvent += OnHidePanelHandler;
            panel.ShowPanelEvent += OnShowPanelHandler;
            panel.UpdatePanelEvent += OnUpdatePanelHandler;
            panel.ShowAsSubpanelEvent += OnShowAsSubpanelHandler;
            panel.PanelDataChangedEvent += OnPanelDataChangedHandler;

            panel.Initialize(UiDataProvider, settings);
            panel.gameObject.SetActive(false);
            panel.gameObject.transform.SetSiblingIndex(m_panelsSettings.IndexOf(settings));
        }

        protected void UnloadPanel(UIPanelType type)
        {
            Log("UnloadPanel " + type);
            var uiPanel = m_panels[type];

            uiPanel.ChangePanelEvent -= OnChangePanelHandler;
            uiPanel.HidePanelEvent -= OnHidePanelHandler;
            uiPanel.ShowPanelEvent -= OnShowPanelHandler;
            uiPanel.UpdatePanelEvent -= OnUpdatePanelHandler;
            uiPanel.ShowAsSubpanelEvent -= OnShowAsSubpanelHandler;
            uiPanel.PanelDataChangedEvent -= OnPanelDataChangedHandler;

            uiPanel.DeInitialize();
            Destroy(uiPanel.gameObject);
            m_panels.Remove(type);
        }

        public IEnumerator ShowPanelAsync(UIPanelType ownerType, UIPanelType type, DataProvider data = null)
        {
            Log("ShowPanelAsync " + type);

            yield return StartCoroutine(LoadPanelAsync(type));

            m_operationQueue.Add(ownerType != UIPanelType.None
                ? new UIPanelOperationWithOwner(UIPanelOperation.OperationType.ShowAsSubpanel, ownerType, type, data)
                : new UIPanelOperation(UIPanelOperation.OperationType.ShowPanel, type, data));
            Log("Add queue " + m_operationQueue.Last());
            if (!IsQueue)
            {
                m_queueCoroutine = StartCoroutine(StartPanelOperation());
            }
        }

        private IEnumerator InstantShowSubpanelAsync(UIPanelType ownerType, UIPanelType type, DataProvider data = null)
        {
            Log("ShowSubpanelAsync " + type);

            yield return StartCoroutine(LoadPanelAsync(type));

            ShowPanelData(type, data);

            var panel = m_panels[type];
            SetSubPanel(m_panels[ownerType], panel);

            Log("End ShowSubpanelAsync " + type);
        }

        private void SetSubPanel(UIPanel uiPanel, UIPanel subPanel)
        {
            subPanel.transform.SetParent(uiPanel.SubpanelHolder);
            subPanel.transform.localScale = Vector3.one;

        }

        private IEnumerator LoadPanelAsync(UIPanelType type)
        {
            if (m_loadingPanels.Contains(type))
            {
                yield break;
            }
            if (!m_panels.ContainsKey(type))
            {
                yield return StartCoroutine(LoadPanelAsync(m_panelsSettings.Single(p => p.PanelType == type)));
            }
        }

        public virtual void ShowPanel(UIPanelType type, DataProvider data = null)
        {
            m_operationQueue.Add(new UIPanelOperation(UIPanelOperation.OperationType.ShowPanel, type, data));
            Log("Add queue " + m_operationQueue.Last());
            if (!IsQueue)
            {
                m_queueCoroutine = StartCoroutine(StartPanelOperation());
            }
        }

        public virtual void UpdatePanel(UIPanelType type, DataProvider data)
        {
            if (IsQueue)
            {
                return;
            }

            m_operationQueue.Add(new UIPanelOperation(UIPanelOperation.OperationType.UpdatePanel, type, data));
            Log("Add queue " + m_operationQueue.Last());
            m_queueCoroutine = StartCoroutine(StartPanelOperation());
        }

        public virtual void HidePanel(UIPanelType type, bool forceDeactivate = false)
        {
            if (m_currentOperation != null && m_currentOperation.PanelType == type)
            {
                if (m_currentOperation.Type == UIPanelOperation.OperationType.ShowPanel)
                {
                    ReStartOperation(new UIPanelHideOperation(UIPanelOperation.OperationType.HidePanel, forceDeactivate, type));
                }
            }
            else
            {
                m_operationQueue.Add(new UIPanelHideOperation(UIPanelOperation.OperationType.HidePanel, forceDeactivate, type));
                Log("Add queue " + m_operationQueue.Last());
                if (!IsQueue)
                {
                    m_queueCoroutine = StartCoroutine(StartPanelOperation());
                }
            }
        }

        public virtual void ShowAsSubpanel(UIPanelType ownerType, UIPanelType openType, DataProvider data = null)
        {
            var operation = m_operationQueue.FirstOrDefault(x => x.PanelType == ownerType);
            if (!m_panels.ContainsKey(ownerType) || operation != null)
            {
                throw new Exception("cannot show " + openType + " as subpanel, becouse parent panel " + ownerType + " already closed!");
            }

            if (m_subpanels.ContainsKey(ownerType) && m_subpanels[ownerType] != openType)
            {
                HidePreviousSubpanel(m_subpanels[ownerType]);
            }

            if (m_currentOperation?.PanelType == ownerType &&
                (m_currentOperation?.Type == UIPanelOperation.OperationType.ShowPanel ||
                m_currentOperation?.Type == UIPanelOperation.OperationType.ShowAsSubpanel))
            {
                if (ownerType != UIPanelType.None && m_panels[ownerType].SubpanelHolder != null)
                {
                    m_subpanels[ownerType] = openType;
                    ChangeSubpanel(openType, openType, data);

                    if (m_panels.ContainsKey(openType))
                    {
                        ShowPanelData(openType, data);

                        var panel = m_panels[openType];
                        SetSubPanel(m_panels[ownerType], panel);
                    }
                    else
                    {
                        StartCoroutine(InstantShowSubpanelAsync(ownerType, openType, data));
                    }
                    return;
                }
            }

            m_operationQueue.Add(new UIPanelOperationWithOwner(UIPanelOperation.OperationType.ShowAsSubpanel, ownerType, openType, data));
            Log("Add queue " + m_operationQueue.Last());
            if (!IsQueue)
            {
                m_queueCoroutine = StartCoroutine(StartPanelOperation());
            }
        }

        private void HidePreviousSubpanel(UIPanelType subpanel)
        {
            if (m_operationQueue.FirstOrDefault(x =>
                    x.Type == UIPanelOperation.OperationType.HidePanel && x.PanelType == subpanel) != null)
            {
                return;
            }
            if (m_currentOperation != null && m_currentOperation.Type == UIPanelOperation.OperationType.HidePanel &&
                m_currentOperation.PanelType == subpanel)
            {
                return;
            }

            m_operationQueue.Add(new UIPanelHideOperation(UIPanelOperation.OperationType.HidePanel, false, subpanel));
            Log("Add queue " + m_operationQueue.Last());
        }

        public virtual void ChangePanel(UIPanelType hideType, UIPanelType openType, DataProvider data = null)
        {
            var ownerPanel = m_subpanels.FirstOrDefault(x => x.Value == hideType);
            m_operationQueue.Add(new UIPanelOperationWithOwner(UIPanelOperation.OperationType.ChangePanel, hideType, openType, data));
            Log("Add queue " + m_operationQueue.Last());

            if (ownerPanel.Key != UIPanelType.None && m_panels[ownerPanel.Key].SubpanelHolder != null)
            {
                m_operationQueue.Add(new UIPanelOperationWithOwner(UIPanelOperation.OperationType.ShowAsSubpanel,
                    ownerPanel.Key, openType, data));
                Log("Add queue " + m_operationQueue.Last());
            }
            else
            {
                m_operationQueue.Add(new UIPanelOperation(UIPanelOperation.OperationType.ShowPanel, openType, data));
                Log("Add queue " + m_operationQueue.Last());
            }

            if (!IsQueue)
            {
                m_queueCoroutine = StartCoroutine(StartPanelOperation());
            }
        }

        private void ChangeSubpanel(UIPanelType type, UIPanelType changeType, DataProvider data = null)
        {
            var subpanel = m_subpanels.FirstOrDefault(x => x.Value == type);
            if (subpanel.Key != UIPanelType.None)
            {
                if (changeType == UIPanelType.None)
                {
                    m_subpanels.Remove(subpanel.Key);
                }
                else
                {
                    m_subpanels[subpanel.Key] = changeType;
                }

                m_panels[subpanel.Key].SubpanelChanged(changeType, data);
            }
        }

        protected virtual void InstantShowPanel(UIPanelType type, DataProvider data = null)
        {
            Log("InstantShowPanel " + type);

            if (m_panels.ContainsKey(type))
            {
                var panel = m_panels[type];
                panel.gameObject.SetActive(true);
                if (m_deactivatedPanels.Contains(type))
                {
                    m_deactivatedPanels.Remove(type);
                }
                if (panel.PanelState == UIPanelState.Deactivate || panel.PanelState == UIPanelState.Show)
                {
                    if (panel.PanelState == UIPanelState.Deactivate)
                    {
                        panel.PanelState = UIPanelState.Show;
                    }
                    panel.UpdateData(data);
                    Log("Just UpdateData " + panel.PanelType);
                }
                else
                {
                    panel.ShowData(data);
                    panel.UpdateData(data);
                }
                OnPanelShowed?.Invoke(panel);
                Log("OnPanelShowed " + panel.PanelType);
            }
            else
            {
                StartCoroutine(InstantShowPanelAsync(type, data));
            }
        }

        private IEnumerator InstantShowPanelAsync(UIPanelType type, DataProvider data = null)
        {
            Log("InstantShowPanelAsync " + type);

            if (m_loadingPanels.Contains(type))
            {
                yield break;
            }
            if (!m_panels.ContainsKey(type))
            {
                yield return StartCoroutine(LoadPanelAsync(m_panelsSettings.Single(p => p.PanelType == type)));
            }

            InstantShowPanel(type, data);
        }

        private void ReStartOperation(UIPanelOperation operation)
        {
            m_operationQueue.Insert(0, operation);
            Log("RestartQueue op " + m_operationQueue.FirstOrDefault());

            if (m_operationCoroutine != null)
            {
                StopCoroutine(m_operationCoroutine);
                m_operationCoroutine = null;
            }
        }

        private IEnumerator StartPanelOperation()
        {
            IsQueue = true;
            while (m_operationQueue.Count > 0)
            {
                m_currentOperation = m_operationQueue[0];
                m_operationQueue.RemoveAt(0);
                Log("Run from queue " + m_currentOperation);

                while (m_loadingPanels.Count > 0)
                {
                    yield return null;
                }

                switch (m_currentOperation.Type)
                {
                    case UIPanelOperation.OperationType.ShowPanel:
                        Log("ShowPanel " + m_currentOperation);
                        Assert.IsTrue(m_panelsSettings.SingleOrDefault(p => p.PanelType == m_currentOperation.PanelType) != null, "UIModule " + GetType() + " does not have panel " + m_currentOperation.PanelType);
                        m_operationCoroutine = StartCoroutine(ShowPanelOperation(m_currentOperation.PanelType, m_currentOperation.Data));
                        while (m_operationCoroutine != null) yield return null;
                        Log("End ShowPanel " + m_currentOperation);
                        break;
                    case UIPanelOperation.OperationType.UpdatePanel:
                        Log("UpdatePanel " + m_currentOperation);
                        Assert.IsTrue(m_panelsSettings.SingleOrDefault(p => p.PanelType == m_currentOperation.PanelType) != null, "UIModule " + GetType() + " does not have panel " + m_currentOperation.PanelType);
                        yield return m_operationCoroutine =
                            StartCoroutine(UpdatePanelOperation(m_currentOperation.PanelType, m_currentOperation.Data));
                        Log("End UpdatePanel " + m_currentOperation);
                        break;
                    case UIPanelOperation.OperationType.HidePanel:
                        {
                            var operation = (UIPanelHideOperation)m_currentOperation;
                            Log("HidePanel " + m_currentOperation);
                            Assert.IsTrue(m_panels.ContainsKey(operation.PanelType),
                                "HidePanel not  found " + operation.PanelType);
                            if (!operation.ForceDeactivate)
                            {
                                ChangeSubpanel(operation.PanelType, UIPanelType.None);
                            }
                            yield return m_operationCoroutine =
                                StartCoroutine(HidePanelOperation(operation.PanelType, operation.ForceDeactivate));
                            Log("End HidePanel " + m_currentOperation);
                        }
                        break;
                    case UIPanelOperation.OperationType.ShowAsSubpanel:
                        {
                            var operation = (UIPanelOperationWithOwner)m_currentOperation;
                            Log("ShowSubpanel " + m_currentOperation);
                            Assert.IsTrue(m_panelsSettings.SingleOrDefault(p => p.PanelType == operation.PanelType) != null,
                                "UIModule " + GetType() + " does not have panel " + m_currentOperation.PanelType);
                            m_subpanels[operation.OwnerPanelType] = operation.PanelType;
                            ChangeSubpanel(operation.PanelType, operation.PanelType, operation.Data);
                            var ownerPanel = m_panels[operation.OwnerPanelType];
                            yield return m_operationCoroutine = ownerPanel.SubpanelHolder != null
                                ? StartCoroutine(ShowSubpanelOperation(operation.OwnerPanelType, operation.PanelType, operation.Data))
                                : StartCoroutine(ShowPanelOperation(operation.PanelType, operation.Data));
                            Log("End ShowSubpanel " + m_currentOperation);
                        }
                        break;
                    case UIPanelOperation.OperationType.ChangePanel:
                        {
                            var operation = (UIPanelOperationWithOwner)m_currentOperation;
                            Log("ChangePanel " + m_currentOperation);
                            Assert.IsTrue(m_panels.ContainsKey(operation.OwnerPanelType), "ChangedPanel not found " + operation.OwnerPanelType);
                            ChangeSubpanel(operation.OwnerPanelType, operation.PanelType, operation.Data);
                            yield return m_operationCoroutine = StartCoroutine(HidePanelOperation(operation.OwnerPanelType));
                            Log("End ChangePanel " + m_currentOperation);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            m_currentOperation = null;
            m_queueCoroutine = null;

            IsQueue = false;
        }

        private IEnumerator ShowPanelOperation(UIPanelType type, DataProvider data = null)
        {
            if (m_operationCoroutine != null)
            {
                StopCoroutine(m_operationCoroutine);
            }

            if (m_panels.ContainsKey(type))
            {
                ShowPanelData(type, data);

                var panel = m_panels[type];

                OnPanelShowed?.Invoke(panel);
                Log("OnPanelShowed " + panel.PanelType);
            }
            else
            {
                yield return StartCoroutine(ShowPanelAsync(UIPanelType.None, type, data));
            }

            m_operationCoroutine = null;
        }

        private IEnumerator ShowSubpanelOperation(UIPanelType ownerType, UIPanelType type, DataProvider data = null)
        {
            if (m_operationCoroutine != null)
            {
                StopCoroutine(m_operationCoroutine);
            }

            if (m_panels.ContainsKey(type))
            {
                ShowPanelData(type, data);

                var panel = m_panels[type];
                if (m_panels[ownerType].SubpanelHolder != null)
                {
                    SetSubPanel(m_panels[ownerType], panel);
                }



                OnPanelShowed?.Invoke(panel);
                Log("OnPanelShowed " + panel.PanelType);
            }
            else
            {
                yield return StartCoroutine(ShowPanelAsync(ownerType, type, data));
            }

            m_operationCoroutine = null;
        }

        private void ShowPanelData(UIPanelType type, DataProvider data = null)
        {
            var panel = m_panels[type];
            panel.gameObject.SetActive(true);
            if (m_deactivatedPanels.Contains(type))
            {
                m_deactivatedPanels.Remove(type);
            }
            
            if (panel.PanelState == UIPanelState.Deactivate ||
                panel.PanelState == UIPanelState.Show)
            {
                if (panel.PanelState == UIPanelState.Deactivate)
                {
                    panel.PanelState = UIPanelState.Show;
                }
                panel.UpdateData(data);
                Log("Just UpdateData " + panel.PanelType);
            }
            else
            {
                panel.ShowData(data);
                panel.UpdateData(data);
            }
            UiDataProvider.SoundService.PlayMusic("ShowPanel");
        }

        private IEnumerator UpdatePanelOperation(UIPanelType type, DataProvider data = null)
        {
            var panel = m_panels[type];
            panel.UpdateData(data);
            yield return null;
        }

        private IEnumerator HidePanelOperation(UIPanelType type, bool forceDeactivate = false)
        {
            var panel = m_panels[type];
            // Hack for multiple hides for one panel. May be best lock input on this situations
            if (panel.PanelState != UIPanelState.Show)
            {
                yield break;
            }

            UiDataProvider.SoundService.PlayMusic("InvokePanel");
            var subpanel = m_subpanels.FirstOrDefault(x => x.Value == type);


            if (forceDeactivate)
            {
                panel.Deactivate();
                yield break;
            }

            HidePanelData(type);
        }

        private void HidePanelData(UIPanelType type)
        {
            var panel = m_panels[type];
            OnPanelHide?.Invoke(panel);
            panel.Hide();

            if (m_deactivatedPanels.Contains(type))
            {
                m_deactivatedPanels.Remove(type);
            }
            if (m_subpanels.ContainsKey(type))
            {
                var subpanel = m_subpanels[type];
                m_subpanels.Remove(type);
                if (m_panels.ContainsKey(subpanel))
                {
                    HidePanelData(subpanel);
                }
            }

            if (panel.PanelSettings.UnLoadMode == UIPanelUnLoadMode.Destroy)
            {
                UnloadPanel(type);
            }
            else
            {
                panel.gameObject.SetActive(false);
            }
        }

        public virtual bool IsPanelShow(UIPanelType type)
        {
            if (m_panels.ContainsKey(type))
            {
                return m_panels[type].gameObject.activeSelf;
            }
            return false;
        }

        public T GetShowedPanel<T>(UIPanelType type) where T : UIPanel
        {
            return GetShowedPanel(type) as T;
        }

        public UIPanel GetShowedPanel(UIPanelType type)
        {
            UIPanel panel;
            if (!m_panels.TryGetValue(type, out panel))
            {
                return null;
            }

            if (panel.PanelState != UIPanelState.Show)
            {
                return null;
            }
            return panel;
        }

        [ContextMenu("ShowPanelStates")]
        public void ShowPanelStates()
        {
            string res = "PanelStates:";
            foreach (var uiPanel in m_panels)
            {
                res += "\r\n";
                res += GetSubPanelTypes(uiPanel.Key);
            }
            Debug.Log(res);
        }

        private string GetAdditionalDebugInfo()
        {
            return string.Empty;
        }

        private string GetSubPanelTypes(UIPanelType type)
        {
            if (m_subpanels.ContainsKey(type))
            {
                return type.ToString() + ": subpanel is " + m_subpanels[type];
            }

            return type.ToString() + ": no subpanels";
        }

        [SerializeField]
        private List<UIPanelSettings> m_panelsSettings = new List<UIPanelSettings>();

        protected Dictionary<UIPanelType, UIPanel> m_panels = new Dictionary<UIPanelType, UIPanel>();
        protected List<UIPanelType> m_deactivatedPanels = new List<UIPanelType>();

        protected Dictionary<UIPanelType, UIPanelType> m_subpanels = new Dictionary<UIPanelType, UIPanelType>();

        private List<UIPanelType> m_loadingPanels = new List<UIPanelType>();
        private UIDataProvider m_uiDataProvider = null;
        private Skylight.PrefabManager m_content = null;
        private RectTransform m_container = null;

        private List<UIPanelOperation> m_operationQueue = new List<UIPanelOperation>();
        private UIPanelOperation m_currentOperation;
        private Coroutine m_operationCoroutine;
        private Coroutine m_queueCoroutine;

        private bool m_isQueue;
    }

    public abstract class UIModule<T> : UIModule where T : UIDataProvider
    {
        protected new T UiDataProvider
        {
            get; private set;
        }

        public override void Initialize(UIDataProvider uiDataProvider,Skylight.PrefabManager  content)
        {
            base.Initialize(uiDataProvider, content);
            UiDataProvider = base.UiDataProvider as T;
        }
    }
}
