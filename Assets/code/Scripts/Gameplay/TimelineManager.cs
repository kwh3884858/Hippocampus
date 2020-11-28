﻿using System;
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
            m_timelineAssetDic.Add(TimelineEnum.Default, assets[0]!=null? assets[0]:null);
            m_timelineAssetDic.Add(TimelineEnum.StartScene, assets[1] != null ? assets[1] : null);
        }


        public void PlayTimeline(TimelineEnum TimelineType,Transform PlayerPosition = null)
        {
            Debug.Log("Play Timeline");
            m_timelineAssetDic.TryGetValue(TimelineType, out var timelineAsset);
            if (timelineAsset && TimelinePlayer)
            {
                TimelinePlayer.Play(timelineAsset);
            }
            Debug.Log("PlayableDirector named " + TimelinePlayer.name + " is now playing in " + TimelinePlayer.time+" left time is "+ (TimelinePlayer.duration - TimelinePlayer.time));
            //callback
            StartCoroutine(PlayedCallBack(TimelinePlayer.duration - TimelinePlayer.time, PlayerPosition));
        }

        IEnumerator PlayedCallBack(double time, Transform PlayerPosition)
        {
            yield return new WaitForSeconds((float)time);
            PlayerController.Instance().SetMoveEnable(true);
            if (PlayerPosition!=null)
            {
                PlayerController.Instance().SetPlayerPosition(PlayerPosition);
            }
            yield break;
        }
        // Update is called once per frame
        void Update()
        {
        
        }


    }
}
