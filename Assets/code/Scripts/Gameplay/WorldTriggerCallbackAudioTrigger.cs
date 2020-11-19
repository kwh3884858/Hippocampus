using System.Collections;
using System.Collections.Generic;
using GamePlay.EventTrigger;
using GamePlay.Global;
using GamePlay.Stage;
using GamePlay.Utility;
using UI.Panels.Providers.DataProviders;
using UnityEngine;
using VLB;

public class WorldTriggerCallbackAudioTrigger : WorldTriggerCallbackBase
{
    [SerializeField]
    public AudioSource m_audioSource;
    public bool m_debugSetting = true;

    [Header("AudioClip Setting")]
    public AudioClip m_audioClip;

    public float m_audioVolume = 1;
    public bool m_isLoop = false;
    public bool m_onlyTiriggerOnce;

    public void Awake()
    {
        m_audioSource = this.gameObject.AddComponent<AudioSource>();
        m_audioSource.playOnAwake = false;
    }
    protected override void AfterStart()
    {
#if UNITY_EDITOR
        //Debug 3D Text
        if (m_debugSetting)
        {
            WorldDebug3DTextManager.Instance.AddTextToGameobject("AudioTrigger", gameObject);
        }
#endif
    }
    protected override void Callback()
    {
        m_audioSource.PlayOneShot(m_audioClip,m_audioVolume);
        if (m_onlyTiriggerOnce)
        {
            StartCoroutine(DelayedDestory(m_audioClip.length));
        }
    }

    IEnumerator DelayedDestory(float t)
    {
        BoxCollider Box = GetComponent<BoxCollider>();
        Box.enabled = false;
        yield return new WaitForSeconds(t);
        Destroy(gameObject);
    }
}
