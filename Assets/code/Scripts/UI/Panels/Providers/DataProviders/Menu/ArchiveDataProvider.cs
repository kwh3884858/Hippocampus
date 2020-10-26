namespace UI.Panels.Providers.DataProviders
{
    public class ArchiveDataProvider: DataProvider
    {
        public ArchivePanelType Type { get; set; }
        public System.Action OnClose { get; set; }
    }
}