using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Config.Data;
using Controllers.Subsystems;
using StarPlatinum;
using StarPlatinum.EventManager;
using UnityEngine;

namespace UI.Panels.Element
{
    public partial class UI_Common_CGscene_Item_SubView : UIElementBase
    {
        private List<CGScenePointItem> m_items = new List<CGScenePointItem>();
        public override void BindEvent()
        {
            base.BindEvent();
            m_items = m_go_interactionPoints.transform.GetComponentsInChildren<CGScenePointItem>().ToList();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void SetInfo(CGSceneConfig config)
        {
            gameObject.SetActive(true);
        }
        
        public void RefreshPointInfos(List<CGScenePointInfo> pointInfos,Action<int,CGScenePointTouchConfig> callback)
        {
            EventManager.Instance.SendEvent(new ChangeCursorEvent());
//            ClearPointItem();
            for (int i = 0; i < pointInfos.Count; i++)
            {
                var pointInfo = pointInfos[i];
                if (m_items.Count <= i)
                {
                    Debug.LogError($"CG式场景预制错误！！{gameObject.name}");
                    continue;
                }
                m_items[i].ClickCallback = callback;
                m_items[i].SetPointInfo(pointInfo);
//                m_items[i].gameObject.SetActive(true);
            }
        }
        
//        private void ClearPointItem()
//        {
//            foreach (var pointItem in m_items)
//            {
//                pointItem.gameObject.SetActive(false);
//            }
//        }
    }
}