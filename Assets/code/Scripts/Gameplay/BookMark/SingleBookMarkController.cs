using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SingleBookMarkController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    /// <summary>书签</summary>
    [SerializeField]
    private Image bookMark = null;
    /// <summary>选中的显示对象</summary>
    [SerializeField]
    private GameObject selectObj = null;
    void Start()
    {
        if (bookMark != null)
        {
            bookMark.alphaHitTestMinimumThreshold = 0.1f;
        }
        if (selectObj != null)
        {
            selectObj.SetActive(false);
        }
        startMoveY = transform.localPosition.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelect)
        {
            return;
        }
        if (moveDownTween != null)
        {
            moveDownTween.Kill();
        }
        if (moveUpTween != null)
        {
            moveUpTween.Kill();
        }
        moveDownTween = transform.DOLocalMoveY(startMoveY - moveDistanceY, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelect)
        {
            return;
        }
        if (moveUpTween != null)
        {
            moveUpTween.Kill();
        }
        if (moveDownTween != null)
        {
            moveDownTween.Kill();
        }
        moveUpTween = transform.DOLocalMoveY(startMoveY, 0.5f);
    }

    private void OnDestroy()
    {
        if (moveUpTween != null)
        {
            moveUpTween.Kill();
        }
        if (moveDownTween != null)
        {
            moveDownTween.Kill();
        }
    }

    public void SetSelectState()
    {
        if (moveUpTween != null)
        {
            moveUpTween.Kill();
        }
        if (moveDownTween != null)
        {
            moveDownTween.Kill();
        }
        Vector3 localPos = transform.localPosition;
        transform.localPosition = new Vector3(localPos.x, startMoveY - moveDistanceY, localPos.z);
        isSelect = true;
        if(selectObj != null)
        {
            selectObj.SetActive(true);
        }
    }

    public void SetUnSelectState()
    {
        if (!isSelect)
        {
            return;
        }
        if (moveUpTween != null)
        {
            moveUpTween.Kill();
        }
        if (moveDownTween != null)
        {
            moveDownTween.Kill();
        }
        moveUpTween = transform.DOLocalMoveY(startMoveY, 0.5f);
        isSelect = false;
        if (selectObj != null)
        {
            selectObj.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetSelectState();
    }

    private Tween moveDownTween = null;
    private Tween moveUpTween = null;
    private float startMoveY = 0f;
    private float moveDistanceY = 82f;
    private bool isSelect = false;
}
