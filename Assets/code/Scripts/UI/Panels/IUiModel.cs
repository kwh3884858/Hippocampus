using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;

namespace UI.Panels
{
    public interface IUiModel
    {
        UIDataProvider UiDataProvider { get; }
        IUiPanel Panel { get; }

        void Initialize(IUiPanel panel);

        void DeInitialize();

        void Hide();

        void Deactivate();

        void ShowData(DataProvider data);

        void Tick();

        void LateTick();

        void SubpanelChanged(UIPanelType type, DataProvider data = null);

        void SubpanelDataChanged(UIPanelType type, DataProvider data);
    }
}