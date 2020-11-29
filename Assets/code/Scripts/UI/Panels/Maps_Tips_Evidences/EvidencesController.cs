using Evidence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidencesController : MonoBehaviour
{
    [SerializeField]
    private SingleEvidenceController m_evidence = null;
    [SerializeField]
    private Transform m_content = null;
    [SerializeField]
    private EvidenceIntroController m_introCtrl = null;
    [SerializeField]
    private EvidencesDetailController m_detailCtrl = null;

    private EvidenceDataManager m_dataManager = null;
    private List<SingleEvidenceController> m_evidenceList = new List<SingleEvidenceController>();

    public void Init(System.Action closeUI, System.Action<string> onShowEvidence, bool isShowSelectBtn)
    {
        SetData();
        m_CloseUI = closeUI;
        m_onShowEvidence = onShowEvidence;
        m_isShowSelectBtn = isShowSelectBtn;
        if (m_introCtrl != null)
        {
            m_introCtrl.Init(this, m_CloseUI, m_onShowEvidence, m_isShowSelectBtn);
        }
        Show();
    }

    private void SetData()
    {
        m_dataManager = EvidenceDataManager.Instance;
        //m_evidenceList.Clear ();
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        CreateEvidences();
    }

    public void OnClickCloseButton()
    {
        // TODO:关闭显示
    }

    public void HideSelf()
    {
        this.gameObject.SetActive(false);
    }

    //public void ShowIntro(string iconPath, string intro)
    //{
    //    if (m_introCtrl != null)
    //    {
    //        m_introCtrl.ShowIntro(iconPath, intro);
    //    }
    //}

    public void RefreshIntroView(SingleEvidenceData data,SingleEvidenceController view)
    {
        if (m_introCtrl != null)
        {
            m_introCtrl.gameObject.SetActive(true);
            m_introCtrl.RefreshView(data);
        }

        if (m_curSelectItem != null)
        {
            m_curSelectItem.SetSelectedStatus(false);
        }

        m_curSelectItem = view;
    }

    public void ShowEvidenceEnable(bool enable)
    {
        if (m_introCtrl != null)
        {
            m_introCtrl.ShowEvidenceEnable(enable);
        }
    }

    /// <summary>
    /// 创建证据显示，暂时使用gameobject创建
    /// </summary>
    private void CreateEvidences()
    {
        ClearEvidenceList();
        if (m_evidence != null && m_content != null)
        {
            SingleEvidenceController vTmpCtrl = null;
            bool isFirst = true;
            foreach (var data in m_dataManager.MyEvidenceDic)
            {
                vTmpCtrl = GameObject.Instantiate(m_evidence, m_content);
                vTmpCtrl.Init(this, data.Value, m_CloseUI, m_onShowEvidence);
                if (isFirst)
                {
                    isFirst = false;
                    vTmpCtrl.OnClick();
                }
                m_evidenceList.Add(vTmpCtrl);
            }

            if (isFirst)
            {
                m_introCtrl.gameObject.SetActive(false);
            }
        }
    }

    private void ClearEvidenceList()
    {
        int vL = m_evidenceList.Count;
        for (int i = 0; i < vL; i++)
        {
            Destroy(m_evidenceList[i].gameObject);
        }
        m_evidenceList.Clear();
    }

    public void OnClickDetail(string detailImagePath)
    {
        if (m_detailCtrl != null)
        {
            m_detailCtrl.Init(detailImagePath);
        }
    }


    private System.Action<string> m_onShowEvidence = null;
    private System.Action m_CloseUI = null;
    /// <summary>是否显示举证按钮</summary>
    private bool m_isShowSelectBtn = false;

    private SingleEvidenceController m_curSelectItem;
}
