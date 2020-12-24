using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LogoAnimCtrl : MonoBehaviour
{

    /// <summary>logo图片</summary>
    [SerializeField]
    private Image logoImg = null;
    /// <summary>黑色背景</summary>
    [SerializeField]
    private GameObject blackBgObj = null;
    /// <summary>渐显时间</summary>
    [SerializeField]
    private float fadeInTime = 0.5f;
    /// <summary>渐隐时间</summary>
    [SerializeField]
    private float fadeOutTime = 0.5f;

    /// <summary>渐变序列</summary>
    private Sequence fadeSequence = null;
    private System.Action onAnimEnd = null;

    // Start is called before the first frame update
    void Start()
    {
        if (logoImg != null)
        {
            if (fadeSequence != null)
            {
                fadeSequence.Kill();
            }
            Color tmpColor = logoImg.color;
            tmpColor.a = 0;
            logoImg.color = tmpColor;
            fadeSequence = DOTween.Sequence();
            fadeSequence.Append(logoImg.DOFade(1f, 0.5f));
            fadeSequence.Append(logoImg.DOFade(0f, 0.5f));
            fadeSequence.OnComplete(() =>
            {
                if (blackBgObj != null)
                {
                    blackBgObj.SetActive(false);
                }
                onAnimEnd?.Invoke();
                onAnimEnd = null;
            });
        }
    }

    public void Show(System.Action onAnimEnd)
    {
        this.onAnimEnd = onAnimEnd;
        if(gameObject != null && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        if (fadeSequence != null)
        {
            fadeSequence.Kill();
        }
    }

}
