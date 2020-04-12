using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using Const;
using Controllers.Subsystems.Story;
using StarPlatinum;
using TMPro;
using UI.Panels.Element;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UI.Panels.StaticBoard.Element;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UI.Panels.StaticBoard
{
    public class RecordPanel : UIPanel<UIDataProviderGameScene,RecordDataProvider>
    {
        public override void UpdateData(DataProvider data)
        {
            base.UpdateData(data);
            SetData(UIPanelDataProvider.Records);
        }

        public override void Hide()
        {
            base.Hide();
            ClearItemList();
        }

        public void SetData(List<RecordData> dataList)
        {
            ClearItemList();
            if (dataList ==null || dataList.Count ==0)
            {
                EmptyRecord();
                return;
            }
            foreach (var data in dataList)
            {
                GetUIElement<RecordItem>(UIElementType.TalkRecordItem, (item) =>
                {
                    item.SetData(data);
                    m_itemList.Add(item);
                    item.transform.SetParent(m_svContent);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(m_svContent.rectTransform());
                });
            }
            
            m_scrollView.verticalScrollbar.value = 1;  //让它处于初始拉动状态
        }

        private void ClearItemList()
        {
            foreach (var recordItem in m_itemList)
            {
                ReleaseUIElement(UIElementType.TalkRecordItem,recordItem);
            }
            m_itemList.Clear();
        }

        public void EmptyRecord()
        {
            Debug.LogError("历史记录页面传入数据错误！！！！");
        }

        public void OnClickClose()
        {
            InvokeHidePanel();
        }
        private List<RecordItem> m_itemList = new List<RecordItem>();

        [SerializeField] private Transform m_svContent;
        [SerializeField] private ScrollRect m_scrollView;
    }
}