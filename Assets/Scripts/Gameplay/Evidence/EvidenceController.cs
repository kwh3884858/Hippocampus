using System.Collections;
using System.Collections.Generic;
using UI.Panels;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;

namespace Evidence
{
	public class EvidenceController : UIPanel<UIDataProviderGameScene, TalkDataProvider>
	{

		[SerializeField]
		private SingleEvidenceController m_evidence = null;
		[SerializeField]
		private Transform m_content = null;

		private EvidenceDataManager m_dataManager = null;
		private List<SingleEvidenceController> m_evidenceList = new List<SingleEvidenceController> ();

		public void Init ()
		{
			SetData ();
			Show ();
		}

		private void SetData ()
		{
			m_dataManager = EvidenceDataManager.Instance;
			m_evidenceList.Clear ();
		}

		public void Show ()
		{
			CreateEvidences ();
		}

		public void OnClickCloseButton ()
		{
			// TODO:关闭显示
			base.InvokeHidePanel ();
		}

		/// <summary>
		/// 创建证据显示，暂时使用gameobject创建
		/// </summary>
		private void CreateEvidences ()
		{
			ClearEvidenceList ();
			if (m_evidence != null && m_content != null) {
				SingleEvidenceController vTmpCtrl = null;
				foreach (var data in m_dataManager.myEvidenceDic) {
					vTmpCtrl = GameObject.Instantiate (m_evidence, m_content);
					vTmpCtrl.Init (data.Value);
					m_evidenceList.Add (vTmpCtrl);
				}
			}
		}

		private void ClearEvidenceList ()
		{
			int vL = m_evidenceList.Count;
			for (int i = 0; i < vL; i++) {
				Destroy (m_evidenceList [i].gameObject);
			}
			m_evidenceList.Clear ();
		}
	}
}