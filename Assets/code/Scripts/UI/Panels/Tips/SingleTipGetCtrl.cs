using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Tips;

public class SingleTipGetCtrl : MonoBehaviour
{

    [SerializeField]
    private Text m_name = null;
    [SerializeField]
    private Text m_description = null;

    public void Init(TipsAwardController father, int index, string name, string description)
    {
        m_father = father;
        m_index = index;
        if (m_name != null)
        {
            m_name.text = name;
        }
        if (m_description != null)
        {
            m_description.text = description;
        }
        m_moveTween = transform.DOLocalMoveX(460, 1f);
        StartCoroutine(Wait(5f, Close));
    }

    public void MoveDown(float distance, float moveTime)
    {
        if (m_moveTween != null)
        {
            m_moveTween.Kill(true);
        }
        if (m_moveDownTween != null)
        {
            m_moveDownTween.Kill();
            m_targetY = m_targetY - distance;
        }
        else
        {
            m_targetY = transform.localPosition.y - distance;
        }
        m_moveDownTween = transform.DOLocalMoveY(m_targetY, moveTime);
    }

    public void EndShow()
    {
        StopAllCoroutines();
        Close();
    }

    private void Close()
    {
        if (m_father != null)
        {
            m_father.OnTipGetClose(m_index);
        }
        m_moveTween = transform.DOLocalMoveX(1460, 1f).OnComplete(OnTweeningComplete);
    }

    private void OnTweeningComplete()
    {
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Wait(float _t, System.Action action)
    {
        float vStart = Time.time;
        while (Time.time - vStart < _t)
        {
            yield return null;
        }
        action?.Invoke();
        yield return null;
    }

    private void OnDestroy()
    {
        if (m_moveTween != null)
        {
            m_moveTween.Kill();
        }
        if (m_moveDownTween != null)
        {
            m_moveDownTween.Kill();
        }
    }

    /// <summary>移动缓动</summary>
    private Tween m_moveTween = null;
    /// <summary>向下移动的缓动</summary>
    private Tween m_moveDownTween = null;
    private TipsAwardController m_father = null;
    private int m_index = 0;
    private float m_targetY = 0f;
}
