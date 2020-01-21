using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 摄像机移动控制
/// </summary>
public class CameraMoveController : MonoBehaviour
{

    //private Tween m_moveTween = null;
    private Sequence m_moveSequence = null;
    private Vector3 m_originalPos = Vector3.zero;
    private Transform m_myTransform = null;

    private void OnDestroy()
    {
        //if (m_moveTween != null)
        //{
        //    m_moveTween.Kill();
        //}
        if (m_moveSequence != null)
        {
            m_moveSequence.Kill();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_myTransform = this.transform;
    }

    /// <summary>
    /// 摄像机移动缓动
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="moveTime"></param>
    /// <param name="returnTime"></param>
    /// <param name="moveMethods"></param>
    /// <param name="onComplete"></param>
    public void Move(Vector3 targetPos, float moveTime, float returnTime, Ease moveMethods = Ease.InOutQuad, System.Action onComplete = null)
    {

        if (m_myTransform != null)
        {
            m_originalPos = m_myTransform.position;
            if (m_moveSequence != null)
            {
                m_moveSequence.Kill();
            }
            //if (m_moveTween != null)
            //{
            //    m_moveTween.Kill();
            //}
            m_moveSequence = DOTween.Sequence();
            m_moveSequence.Append(m_myTransform.DOMove(targetPos, moveTime).SetEase(moveMethods));
            m_moveSequence.Append(m_myTransform.DOMove(m_originalPos, returnTime).SetEase(moveMethods));
            //m_moveTween = m_myTransform.DOMove(targetPos, moveTime).SetEase(Ease.InOutQuad);
            if (onComplete != null)
            {
                m_moveSequence.OnComplete(() => { onComplete?.Invoke(); });
                //m_moveTween.OnComplete(() => { onComplete?.Invoke(); });
            }
        }
    }
}
