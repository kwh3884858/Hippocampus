using Evidence;

namespace UI.Panels.Providers.DataProviders
{
    public class EvidenceDataProvider : DataProvider
    {
        public SingleEvidenceData Data { get; set; }

        public System.Action CloseEvidenceUI { get; set; }
        public System.Action OnShowEvidence { get; set; }

        public bool IsOnEvidence { get; set; }
        public CommonMapsTipsEvidencesPanel.ShowState CurState { get; set; }
    }
}
