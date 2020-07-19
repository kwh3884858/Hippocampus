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
    public abstract class UIModel : IUiModel
    {
        public UIDataProvider UiDataProvider { get; private set; }
        public IUiPanel Panel { get; private set; }
        public virtual void Initialize(IUiPanel uiPanel )
        {
            Panel = uiPanel;
            UiDataProvider = uiPanel.UiDataProvider;
        }

        public virtual void DeInitialize()
        {
        }

        public virtual void Hide()
        {
        }

        public virtual void Deactivate()
        {
        }

        public virtual void ShowData(DataProvider data)
        {
        }

        public virtual void UpdateData(DataProvider data)
        {
        }
        
        public virtual void Tick()
        {
        }

        public virtual void LateTick()
        {
        }

        public virtual void SubpanelChanged(UIPanelType type, DataProvider data = null)
        {
        }

        public virtual void SubpanelDataChanged(UIPanelType type, DataProvider data)
        {
        }
    }
}