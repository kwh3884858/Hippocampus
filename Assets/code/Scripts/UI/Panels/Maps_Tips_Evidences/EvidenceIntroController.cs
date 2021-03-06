﻿using Config.Data;
using StarPlatinum;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class EvidenceIntroController : MonoBehaviour
{

    [SerializeField]
    private Image m_icon = null;
    [SerializeField]
    private TextMeshProUGUI m_intro = null;
    [SerializeField]
    private GameObject m_showBtn = null;
    [SerializeField]
    private TextMeshProUGUI m_title = null;
    [SerializeField]
    private GameObject m_detailBtn = null;

    public void Init(EvidencesController father, System.Action closeEvidenceUI, System.Action<string> onShowEvidence = null, bool isShowSelectBtn = false)
    {
        m_father = father;
        m_closeEvidenceUI = closeEvidenceUI;
        m_onShowEvidence = onShowEvidence;
        if (m_showBtn != null)
        {
            m_showBtn.SetActive(isShowSelectBtn);
        }

        m_intro.text = "";
    }

    public void RefreshView(Evidence.SingleEvidenceData data)
    {
        gameObject.SetActive(true);
        m_data = data;
        if (m_title != null)
        {
            m_title.text = m_data.exhibit;
        }
        if (m_data == null || (m_data != null && string.IsNullOrEmpty(m_data.exhibitImageName)))
        {
            if (m_detailBtn != null)
            {
                m_detailBtn.SetActive(false);
            }
        }
        else
        {
            if (m_detailBtn != null)
            {
                m_detailBtn.SetActive(true);
            }
        }
        ShowIntro(m_data.exhibitID, m_data.description);
    }

    public void ShowIntro(string iconPath, string intro)
    {
        m_intro.text = intro;
        var cfg = ConfigData.Instance.evidenceConfig.GetDetail(iconPath);// EvidenceConfig.GetConfigByKey(iconPath);
        PrefabManager.Instance.SetImage(m_icon, cfg.exhibitID);
    }

    public void ShowEvidenceEnable(bool enable)
    {
        if (m_showBtn != null)
        {
            m_showBtn.SetActive(enable);
        }
    }

    public void OnClickSelect()
    {
        //// 目前由证物系统对错误结果进行处理
        //if (Evidence.EvidenceDataManager.Instance.IsCorrectEvidence(m_data.exhibit))
        //{
        //    m_closeEvidenceUI?.Invoke();
        //    m_closeEvidenceUI = null;
        //    m_onShowEvidence?.Invoke();
        //    m_onShowEvidence = null;
        //}
        //else
        //{
        //    // TODO: 提示选择错误
        //    Debug.Log("select error");
        //}
        m_closeEvidenceUI?.Invoke();
        m_closeEvidenceUI = null;
        m_onShowEvidence?.Invoke(m_data.exhibitID);
        m_onShowEvidence = null;
    }

    public void OnClickDetail()
    {
        // TODO: show detail dialog
        if (m_father != null)
        {
            m_father.OnClickDetail(m_data.exhibitImageName);
        }
    }

    private System.Action<string> m_onShowEvidence = null;
    private System.Action m_closeEvidenceUI = null;
    /// <summary>证据数据</summary>
    private Evidence.SingleEvidenceData m_data = null;
    private EvidencesController m_father = null;
}
