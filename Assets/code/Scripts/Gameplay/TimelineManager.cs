using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.Player;
using StarPlatinum.Base;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Object = UnityEngine.Object;

namespace Assets.code.Scripts.Gameplay
{
    [System.Serializable]
    public enum TimelineEnum
    {
        Default,
        StartScene,

    };
    public class TimelineManager : MonoSingleton<TimelineManager>
    {
        PlayableDirector TimelinePlayer;
        Dictionary<TimelineEnum, TimelineAsset> m_timelineAssetDic = new Dictionary<TimelineEnum, TimelineAsset>();
        [SerializeField]
        TimelineAsset[] assets;//实现在Inspector面板的显示

        public override void SingletonInit()
        {
            TimelinePlayer = Object.FindObjectOfType<PlayableDirector>();
            m_timelineAssetDic.Add(TimelineEnum.Default, assets[0]);
            m_timelineAssetDic.Add(TimelineEnum.StartScene, assets[1]);
        }


        public void PlayTimeline(TimelineEnum TimelineType)
        {
            Debug.Log("Play Timeline");
            m_timelineAssetDic.TryGetValue(TimelineType, out var timelineAsset);
            if (timelineAsset && TimelinePlayer)
            {
                TimelinePlayer.Play(timelineAsset);
            }
            Debug.Log("PlayableDirector named " + TimelinePlayer.name + " is now playing in " + TimelinePlayer.time+" left time is "+ (TimelinePlayer.duration - TimelinePlayer.time));
            StartCoroutine(PlayedCallBack(TimelinePlayer.duration - TimelinePlayer.time));
        }

        IEnumerator PlayedCallBack(double time)
        {
            yield return new WaitForSeconds((float)time);
            PlayerController.Instance().SetMoveEnable(true);
            yield break;
        }
        // Update is called once per frame
        void Update()
        {
        
        }


    }
}
