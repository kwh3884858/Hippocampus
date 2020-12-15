using GamePlay.Global;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace GamePlay.EventTrigger
{
    public class WorldTriggerCallbackTimelinePlayer : WorldTriggerCallbackBase
    {

        protected override void AfterStart()
        {
            AddSceneDebugTextForGameobject($"Play Timeline:{m_timeline}");

            if (SingletonGlobalDataContainer.Instance.IsStoryTriggered(m_timeline.name))
            {
                Destroy(gameObject);
            }
        }

        protected override void Callback()
        {
            SingletonGlobalDataContainer.Instance.AddtTriggeredStory(m_timeline.name);

            Gameplay.Manager.TimelineManager.Instance().PlayTimeline(m_timeline);
            Destroy(gameObject);
        }

        void OnGui()
        {
            if (GUI.Button(new Rect(0,0,500,500), "Skip Timeline"))
            {
                Gameplay.Manager.TimelineManager.Instance().AbortTimeline();
            }
        }

        public TimelineAsset m_timeline;
    }
}