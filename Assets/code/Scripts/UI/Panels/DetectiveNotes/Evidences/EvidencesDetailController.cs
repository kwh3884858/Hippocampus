using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using StarPlatinum;

public class EvidencesDetailController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image m_detailImage = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        CloseSelf();
    }

    private void CloseSelf()
    {
        gameObject.SetActive(false);
    }

    private void ShowSelf()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    public void Init(string detailImagePath)
    {
        if (m_detailImage != null && !string.IsNullOrEmpty(detailImagePath))
        {
            ShowSelf();
            PrefabManager.Instance.SetImage(m_detailImage, detailImagePath);
        }
    }
}
