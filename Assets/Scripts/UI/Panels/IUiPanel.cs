using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;

namespace UI.Panels
{
    public interface IUiPanel
    {
        UIDataProvider UiDataProvider { get; }
        UIPanelType PanelType { get; }

        void InvokePanelDataChanged(UIPanelType type, DataProvider data);
        void InvokeHidePanel(bool forceDeactivate = false);
        void InvokeHidePanel(UIPanelType type, bool forceDeactivate = false);
        void InvokeShowPanel(UIPanelType type, DataProvider data = null);
        void InvokeUpdatePanel(UIPanelType type, DataProvider data);
        void InvokeShowAsSubpanel(UIPanelType ownerType, UIPanelType type, DataProvider data = null);
        void InvokeChangePanel(UIPanelType type, DataProvider data = null);
        void InvokeChangePanel(UIPanelType changedType, UIPanelType type, DataProvider data = null);
    }
}
