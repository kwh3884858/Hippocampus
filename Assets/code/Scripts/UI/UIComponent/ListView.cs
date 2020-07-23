using System;
using System.Collections.Generic;
using StarPlatinum;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.UIComponent
{
    public class ListView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
    {
        public enum ListViewLayoutType
        {
            TopToBottom,
            BottomToTop,
            LeftToRight,
            RightToLeft,
        }

        public class ListItem
        {
            public GameObject go;

            public string tag;

            public string prefabName;

            public int index;

            public float startPos;

            public float endPos;

            public bool isInit;

            public object data;

            public ListItem()
            {
                this.go = null;
                this.prefabName = string.Empty;
                this.tag = string.Empty;
                this.index = 0;
                this.startPos = 0f;
                this.endPos = 0f;
                this.isInit = false;
                this.data = null;
            }

            public bool HasGameObject()
            {
                return this.go != null;
            }

            public void ExportItem()
            {
                if (!this.go)
                {
                    return;
                }
            }
        }

        public class FuncTab
        {
            public delegate float ReturnFloat(ListView.ListItem item);

            public delegate string ReturnString(ListView.ListItem item);

            public FuncTab.ReturnFloat GetItemSize;
            public FuncTab.ReturnString GetItemTag;
            public FuncTab.ReturnString GetItemPrefabName;
            public Action<ListView.ListItem> ItemEnter;
            public Action<ListView.ListItem> ItemRemove;
        }

        //�Զ����Ź�����Ч
        public bool autoPlaySound  = true;
        public string soundAssetName = "Sound_Ui_CommonScroll";

        public ListView.ListViewLayoutType layoutType;

        public List<string> ItemPrefabDataList = new List<string>();

        public RectTransform listContainer;

        public float offset;

        public float spacing;

        public float cacheSize;

        public float autoScrollTime = 0.2f;

        private bool isVertical;

        private Dictionary<string, float> prefabSizeMap = new Dictionary<string, float>();

        private string defaultPrefabName;

        private List<ListView.ListItem> itemList = new List<ListView.ListItem>();

        private float totalSize;

        private float viewSize;

        private float viewStartPos;

        private float viewEndPos;

        private float containerLastPos;

        private ScrollRect parentScrollRect;
        
        private ListView parentListView;

        private bool parentScrollEnable;

        private bool autoScroll;

        private Vector4 autoScrollParam;

        private static Vector2 TopToDownPreset = new Vector2(0.5f, 1);
        private static Vector2 DownToTopPreset = new Vector2(0.5f, 0);
        private static Vector2 LeftToRightPreset = new Vector2(0, 0.5f);
        private static Vector2 RightToLeftPreset = new Vector2(1, 0.5f);
        private static Vector2 Pivot = new Vector2(0.5f, 0.5f);
        private Dictionary<string, GameObject> assetObjectDic;
        private bool m_isPositive;
        private FuncTab m_funcTab;
        private Dictionary<string, List<GameObject>> mItemPoolDict = new Dictionary<string, List<GameObject>>();
        private Dictionary<GameObject, object> mItemObjectDict = new Dictionary<GameObject, object>();

        public Action onDrag;
        public Action<PointerEventData> onDragBegin;
        public Action<PointerEventData> onDragEnd;

        public float GetItemSizeByIndex(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return 0;
            }

            return Mathf.Abs(this.itemList[index].startPos - this.itemList[index].endPos);
        }

        private float GetItemSize(ListView.ListItem item)
        {
            if (m_funcTab != null && m_funcTab.GetItemSize != null)
            {
                return m_funcTab.GetItemSize(item);
            }

            if (item.startPos != 0 || item.endPos != 0)
            {
                return Mathf.Abs(item.startPos - item.endPos);
            }

            return this.prefabSizeMap[item.prefabName];
        }

        private string GetItemTag(ListView.ListItem item)
        {
            if (m_funcTab != null && m_funcTab.GetItemTag != null)
            {
                return m_funcTab.GetItemTag(item);
            }

            return string.Empty;
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            if (m_funcTab != null && m_funcTab.GetItemPrefabName != null)
            {
                return m_funcTab.GetItemPrefabName(item);
            }

            return this.defaultPrefabName;
        }

        private void SetContainerSize(float size)
        {
            this.listContainer.sizeDelta = ((!this.isVertical)
                ? new Vector3(size, this.listContainer.rect.height)
                : new Vector3(this.listContainer.rect.width, size));
        }

        private float GetContainerSize()
        {
            return (!this.isVertical) ? this.listContainer.rect.width : this.listContainer.rect.height;
        }

        public void SetContainerPos(float pos)
        {
            this.listContainer.anchoredPosition = ((!this.isVertical) ? new Vector2(pos, 0f) : new Vector2(0f, pos));
        }

        public float GetContainerPos()
        {
            return (!this.isVertical) ? this.listContainer.anchoredPosition.x : this.listContainer.anchoredPosition.y;
        }

        private void SetViewRect(float viewStart)
        {
            this.viewStartPos = this.m_isPositive ? (-viewStart - this.cacheSize) : (-viewStart + this.cacheSize);
            this.viewEndPos = this.m_isPositive
                ? (this.viewStartPos + this.viewSize + 2f * this.cacheSize)
                : (this.viewStartPos - this.viewSize - 2f * this.cacheSize);
        }

        private bool ItemVisible(ListView.ListItem item)
        {
            if (m_isPositive)
            {
                if (item.endPos < this.viewStartPos || item.startPos > this.viewEndPos)
                {
                    return false;
                }
            }
            else
            {
                if (item.endPos > this.viewStartPos || item.startPos < this.viewEndPos)
                {
                    return false;
                }
            }

            return true;
        }

        private bool ShowItem(ListView.ListItem item, bool force = false)
        {
            if (this.ItemVisible(item))
            {
                this.OnItemEnter(item, force);
                return true;
            }

            this.OnItemLeave(item);
            return false;
        }

        public void ResetPosition()
        {

        }
        public void SetInitData(Dictionary<string, GameObject> prefabDic, FuncTab funcObj)
        {
            this.isVertical = (this.layoutType == ListView.ListViewLayoutType.TopToBottom ||
                               this.layoutType == ListViewLayoutType.BottomToTop);
            if (this.layoutType == ListViewLayoutType.TopToBottom || this.layoutType == ListViewLayoutType.RightToLeft)
            {
                this.m_isPositive = false;
            }
            else
            {
                this.m_isPositive = true;
            }

            this.viewSize = ((!this.isVertical)
                ? base.GetComponent<RectTransform>().rect.width
                : base.GetComponent<RectTransform>().rect.height);
            string text = "";
            assetObjectDic = new Dictionary<string, GameObject>();

            for (int i = 0; i < ItemPrefabDataList.Count; i++)
            {
                if (prefabDic.ContainsKey(ItemPrefabDataList[i]))
                {
                    text = ItemPrefabDataList[i];
                    assetObjectDic[text] = prefabDic[ItemPrefabDataList[i]];
                    Rect rect = prefabDic[ItemPrefabDataList[i]].GetComponent<RectTransform>().rect;
                    this.prefabSizeMap[text] = (!this.isVertical) ? rect.width : rect.height;
                    this.mItemPoolDict[text] = new List<GameObject>();
                    this.defaultPrefabName = text;
                }
                else
                {
                    Debug.LogError(string.Format("List not find Item Prefab:{0}", ItemPrefabDataList[i]));
                    return;
                }
            }

            if (this.listContainer == null)
            {
                Debug.LogError("Please set Container");
                return;
            }

            m_funcTab = funcObj;
            ScrollRect component = base.GetComponent<ScrollRect>();
            if (component != null)
            {
                component.vertical = this.isVertical;
                component.horizontal = !this.isVertical;
                component.onValueChanged.RemoveAllListeners();
                component.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnValueChanged));
            }

            SetContainerLayout();
            SetParent(this.GetComponent<ScrollRect>());
            if(parentScrollRect!=null)
            {
                parentScrollRect.onValueChanged.AddListener(OnSoundPlay);
            }
        }

        private float m_lastSoundDelta;
        private float soundInterval = 1f;
        private void OnSoundPlay(Vector2 deltaV2)
        {
            if (autoPlaySound)
            {
                float delta = this.isVertical ? deltaV2.y : deltaV2.x;
                if(delta>0&&delta<1)
                {
                    if (Mathf.Abs(delta - m_lastSoundDelta) >= soundInterval/itemList.Count)
                    {
                        m_lastSoundDelta = delta;
                    }
                }
            }
        }

        public void SetContainerLayout()
        {
            Vector2 oldSize = listContainer.rect.size;
            switch (this.layoutType)
            {
                case ListViewLayoutType.BottomToTop:
                    listContainer.pivot = DownToTopPreset;
                    listContainer.anchorMax = DownToTopPreset;
                    listContainer.anchorMin = DownToTopPreset;
                    break;
                case ListViewLayoutType.TopToBottom:
                    listContainer.pivot = TopToDownPreset;
                    listContainer.anchorMax = TopToDownPreset;
                    listContainer.anchorMin = TopToDownPreset;
                    break;
                case ListViewLayoutType.LeftToRight:
                    listContainer.pivot = LeftToRightPreset;
                    listContainer.anchorMax = LeftToRightPreset;
                    listContainer.anchorMin = LeftToRightPreset;
                    break;
                case ListViewLayoutType.RightToLeft:
                    listContainer.pivot = RightToLeftPreset;
                    listContainer.anchorMax = RightToLeftPreset;
                    listContainer.anchorMin = RightToLeftPreset;
                    break;
                default: break;
            }
            listContainer.sizeDelta = oldSize;
        }

        public void SetParent(ScrollRect parent)
        {
            this.parentScrollRect = parent;
        }

        public void SetParentListView(ListView parent)
        {
            this.parentListView = parent;
        }
        

        public void RemoveAt(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }

            ListView.ListItem listItem = this.itemList[index];
            float num = Mathf.Abs(listItem.endPos - listItem.startPos) + this.spacing;
            this.totalSize -= num;
            this.SetContainerSize(this.totalSize);
            this.OnItemLeave(listItem);
            for (int i = index + 1; i < this.itemList.Count; i++)
            {
                ListView.ListItem listItem2 = this.itemList[i];
                listItem2.index--;
                listItem2.startPos = this.m_isPositive ? (listItem2.startPos - num) : (listItem2.startPos + num);
                listItem2.endPos = this.m_isPositive ? (listItem2.endPos - num) : (listItem2.endPos + num);
                if (listItem2.go != null)
                {
                    this.UpdateItemGameObjectPosition(listItem2);
                    this.ShowItem(listItem2);
                }
                else
                {
                    this.ShowItem(listItem2, true);
                }
            }

            this.itemList.RemoveAt(index);
        }

        public void Insert(int index)
        {
            if (index > this.itemList.Count || index < 0)
            {
                return;
            }

            ListView.ListItem listItem = new ListView.ListItem();
            listItem.index = index;
            listItem.prefabName = this.GetItemPrefabName(listItem);
            listItem.tag = this.GetItemTag(listItem);
            if (index != 0)
            {
                if (m_isPositive)
                {
                    listItem.startPos = (this.itemList[index - 1].endPos + this.spacing);
                }
                else
                {
                    listItem.startPos = (this.itemList[index - 1].endPos - this.spacing);
                }
            }
            else
            {
                if (m_isPositive)
                {
                    listItem.startPos = this.offset;
                }
                else
                {
                    listItem.startPos = -this.offset;
                }
            }

            float itemSize = this.GetItemSize(listItem);
            listItem.endPos = this.m_isPositive ? (listItem.startPos + itemSize) : (listItem.startPos - itemSize);
            this.itemList.Insert(index, listItem);
            this.ShowItem(listItem, false);
            this.totalSize += itemSize;
            this.SetContainerSize(this.totalSize);
            for (int i = index + 1; i < this.itemList.Count; i++)
            {
                listItem = this.itemList[i];
                listItem.index++;
                listItem.startPos = this.m_isPositive ? (listItem.startPos + itemSize) : (listItem.startPos - itemSize);
                listItem.endPos = this.m_isPositive ? (listItem.endPos + itemSize) : (listItem.endPos - itemSize);
                if (listItem.go != null)
                {
                    this.UpdateItemGameObjectPosition(listItem);
                }

                this.ShowItem(listItem, false);
            }
        }

        public void UpdateItemSize(int index, float size)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }

            ListView.ListItem listItem = this.itemList[index];
            this.ShowItem(listItem, false);
            float num = Mathf.Abs(listItem.endPos - listItem.startPos);
            listItem.endPos = this.m_isPositive ? (listItem.startPos + size) : (listItem.startPos - size);
            float num2 = size - num;
            this.totalSize += num2;
            this.SetContainerSize(this.totalSize);
            for (int i = index + 1; i < this.itemList.Count; i++)
            {
                ListView.ListItem listItem2 = this.itemList[i];
                listItem2.startPos = this.m_isPositive ? (listItem2.startPos + num2) : (listItem2.startPos - num2);
                listItem2.endPos = this.m_isPositive ? (listItem2.endPos + num2) : (listItem2.endPos - num2);

                if (listItem2.go != null)
                {
                    this.UpdateItemGameObjectPosition(listItem2);
                }

                this.ShowItem(listItem2, false);
            }
        }

        public void ForceRefresh()
        {
            for (int i = 0; i < this.itemList.Count; i++)
            {
                this.ShowItem(this.itemList[i], true);
            }
        }

        public void RefreshItem(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }

            ListView.ListItem listItem = this.itemList[index];
            this.ShowItem(listItem, true);
            float num = Mathf.Abs(listItem.endPos - listItem.startPos);
            float itemSize = this.GetItemSize(listItem);
            listItem.endPos = this.m_isPositive ? (listItem.startPos + itemSize) : (listItem.startPos - itemSize);
            float num2 = itemSize - num;
            this.totalSize += num2;
            this.SetContainerSize(this.totalSize);
            for (int i = index + 1; i < this.itemList.Count; i++)
            {
                ListView.ListItem listItem2 = this.itemList[i];
                listItem2.startPos = this.m_isPositive ? (listItem2.startPos + num2) : (listItem2.startPos - num2);
                listItem2.endPos = this.m_isPositive ? (listItem2.endPos + num2) : (listItem2.endPos - num2);
                if (listItem2.go != null)
                {
                    this.UpdateItemGameObjectPosition(listItem2);
                }

                this.ShowItem(listItem2, false);
            }
        }

        public void ClearPostion()
        {
            this.containerLastPos = 0f;
        }

        public void FillContent(int listLength)
        {
            int count = this.itemList.Count;
            this.totalSize = ((!this.isVertical) ? this.offset : (-this.offset));
            for (int i = 0; i < listLength; i++)
            {
                ListView.ListItem listItem;
                if (i < count)
                {
                    listItem = this.itemList[i];
                    this.OnItemLeave(listItem);
                }
                else
                {
                    listItem = new ListView.ListItem();
                    this.itemList.Add(listItem);
                }

                listItem.index = i;
                listItem.prefabName = this.GetItemPrefabName(listItem);
                listItem.tag = this.GetItemTag(listItem);
                float itemSize = this.GetItemSize(listItem);
                if (this.m_isPositive)
                {
                    listItem.startPos = this.totalSize;
                    listItem.endPos = this.totalSize + itemSize;
                    this.totalSize += (itemSize + this.spacing);
                }
                else
                {
                    listItem.startPos = this.totalSize;
                    listItem.endPos = this.totalSize - itemSize;
                    this.totalSize += (-(itemSize + this.spacing));
                }
            }

            for (int j = count - 1; j >= listLength; j--)
            {
                this.OnItemLeave(this.itemList[j]);
                this.itemList.RemoveAt(j);
            }

            this.totalSize = Mathf.Abs(this.totalSize);
            this.SetContainerSize(this.totalSize);
            this.SetContainerPos(this.containerLastPos);
            this.ShowContentAt(this.containerLastPos);
        }

        private void UpdateItemGameObjectPosition(ListView.ListItem item)
        {
            Vector3 v = (!this.isVertical) ? new Vector2(item.startPos, 0f) : new Vector2(0f, item.startPos);
            RectTransform component = item.go.GetComponent<RectTransform>();
            component.anchoredPosition = v;
            component.localScale = Vector3.one;
        }


        private void OnItemEnter(ListView.ListItem item, bool force = false)
        {
            if (item.go != null && !force)
            {
                return;
            }

            if (item.go == null)
            {
                item.go = GetRecycleItem(item.prefabName);
                if (item.go == null)
                {
                    item.go = GameObject.Instantiate(assetObjectDic[item.prefabName], this.listContainer);
                    item.isInit = false;
                    item.data = null;
                }
                else
                {
                    item.isInit = true;
                    this.mItemObjectDict.TryGetValue(item.go, out item.data);
                    //item.go.transform.SetParent(this.listContainer);
                }
                if (!item.go.activeSelf)
                {
                    item.go.SetActive(true);
                }

                RectTransform rect = item.go.GetComponent<RectTransform>();
                Vector2 size = rect.rect.size;
                switch (this.layoutType)
                {
                    case ListViewLayoutType.TopToBottom:
                        rect.pivot = TopToDownPreset;
                        rect.anchorMin = TopToDownPreset;
                        rect.anchorMax = TopToDownPreset;
                        break;
                    case ListViewLayoutType.BottomToTop:
                        rect.pivot = DownToTopPreset;
                        rect.anchorMin = DownToTopPreset;
                        rect.anchorMax = DownToTopPreset;
                        break;
                    case ListViewLayoutType.LeftToRight:
                        rect.pivot = LeftToRightPreset;
                        rect.anchorMin = LeftToRightPreset;
                        rect.anchorMax = LeftToRightPreset;
                        break;
                    case ListViewLayoutType.RightToLeft:
                        rect.pivot = RightToLeftPreset;
                        rect.anchorMin = RightToLeftPreset;
                        rect.anchorMax = RightToLeftPreset;
                        break;
                    default: break;
                }
                rect.sizeDelta = size;
            }

            this.UpdateItemGameObjectPosition(item);
            m_funcTab.ItemEnter(item);
        }

        private void OnItemLeave(ListView.ListItem item)
        {
            if (item.go == null)
            {
                return;
            }
            RecycleItem(item);
            //CoreUtils.assetService.Destroy(item.go);
            //item.go = null;
            //item.isInit = false;
        }

        public void RecycleItem(ListView.ListItem item)
        {
            if (!mItemObjectDict.ContainsKey(item.go))
            {
                mItemObjectDict[item.go] = item.data;
            }
            m_funcTab.ItemRemove?.Invoke(item);
            item.data = null;
            item.go.SetActive(false);
            this.mItemPoolDict[item.prefabName].Add(item.go);
            item.go = null;
        }

        public GameObject GetRecycleItem(string prefabName)
        {
            if (mItemPoolDict[prefabName].Count > 0)
            {
                int count = mItemPoolDict[prefabName].Count - 1;
                GameObject go = mItemPoolDict[prefabName][count];
                mItemPoolDict[prefabName].RemoveAt(count);
                go.transform.localScale = Vector3.zero;
                go.SetActive(true);
                return go;
            }
            return null;
        }

        public void ShowContentAt(float pos)
        {
            this.SetViewRect(pos);
            for (int i = 0; i < this.itemList.Count; i++)
            {
                ListView.ListItem item = this.itemList[i];
                this.ShowItem(item, false);
            }

            this.containerLastPos = pos;
        }

        public void OnValueChanged(Vector2 vec2)
        {
            this.ShowContentAt(this.GetContainerPos());
        }

        public void Clear()
        {
            for (int i = 0; i < this.itemList.Count; i++)
            {
                ListView.ListItem listItem = this.itemList[i];
                if (listItem.go != null)
                {
                    m_funcTab.ItemRemove?.Invoke(listItem);
                    PrefabManager.Instance.UnloadAsset(listItem.go);
                    listItem.go = null;
                }
            }

            this.prefabSizeMap.Clear();
            this.containerLastPos = 0f;
            this.itemList.Clear();
            this.totalSize = 0f;
            this.SetContainerSize(0f);
            ScrollRect component = base.GetComponent<ScrollRect>();
            if (component != null)
            {
                component.StopMovement();
            }
        }

        public void RefreshAndRestPos(int count = 0)
        {
            for (int i = 0; i < this.itemList.Count; i++)
            {
                ListView.ListItem listItem = this.itemList[i];
                if (listItem.go != null)
                {
                    RecycleItem(listItem);
                }
            }

            this.itemList.Clear();
            this.containerLastPos = 0f;
            this.totalSize = 0f;
            this.SetContainerSize(0f);
            ScrollRect component = base.GetComponent<ScrollRect>();
            if (component != null)
            {
                component.StopMovement();
            }

            FillContent(count);
        }

        private void OnDestroy()
        {
            this.Clear();
        }

        public ListView.ListItem GetItemByIndex(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return null;
            }

            return this.itemList[index];
        }

        public ListView.ListItem GetItemByTag(string tag)
        {
            for (int i = 0; i < this.itemList.Count; i++)
            {
                if (this.itemList[i] != null && this.itemList[i].tag == tag)
                {
                    return this.itemList[i];
                }
            }

            return null;
        }

        public void ScrollPanelToItemIndex(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }

            float num = -this.itemList[index].startPos;
            float containerPos = this.GetContainerPos();
            this.autoScroll = true;
            this.autoScrollParam = new Vector4((num - containerPos) / this.autoScrollTime, this.autoScrollTime, num,
                containerPos);
        }

        public void MovePanelToItemIndex(int index)
        {
            if (index < itemList.Count && index >= 0)
            {
                float num = 0f - itemList[index].startPos;
                SetContainerPos(num);
                ShowContentAt(num);
            }
        }

        public void ScrollList2IdxCenter(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }

            float num = -this.itemList[index].startPos;
            ListView.ListItem item = this.itemList[index];
            float itemSize = this.GetItemSize(item);
            float containerSize = this.GetContainerSize();
            if (m_isPositive)
            {
                if (this.itemList[index].startPos < this.viewSize / 2f)
                {
                    num = 0f;
                }
                else if (this.itemList[index].startPos > containerSize - this.viewSize / 2f)
                {
                    num = this.viewSize - containerSize;
                }
                else
                {
                    num = -this.itemList[index].startPos + this.viewSize / 2f - itemSize / 2f;
                }
            }
            else
            {
                if (this.itemList[index].startPos > -this.viewSize / 2f)
                {
                    num = 0f;
                }
                else if (this.itemList[index].startPos < -(containerSize - this.viewSize / 2f))
                {
                    num = -this.viewSize + containerSize;
                }
                else
                {
                    num = -this.itemList[index].startPos - this.viewSize / 2f + itemSize / 2f;
                }
            }

            this.SetContainerPos(num);
            this.ShowContentAt(num);
        }

        public void ScrollToPos(float dest)
        {
            float containerPos = this.GetContainerPos();
            this.autoScroll = true;
            this.autoScrollParam = new Vector4((dest - containerPos) / this.autoScrollTime, this.autoScrollTime, dest,
                containerPos);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onDragBegin?.Invoke(eventData);
            this.autoScroll = false;
            if ((!this.isVertical && Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y)) ||
                (this.isVertical && Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y)))
            {
                this.parentScrollEnable = false;
            }
            else
            {
                this.parentScrollEnable = true;
            }

            if (this.parentScrollRect != null && this.parentScrollEnable)
            {
                this.parentScrollRect.OnBeginDrag(eventData);
                if (parentListView)
                {
                    ExecuteEvents.Execute(parentListView.gameObject, eventData, ExecuteEvents.beginDragHandler);
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke();
            if (this.parentScrollRect != null && this.parentScrollEnable)
            {
                this.parentScrollRect.OnDrag(eventData);
                if (parentListView)
                {
                    ExecuteEvents.Execute(parentListView.gameObject, eventData, ExecuteEvents.dragHandler);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onDragEnd?.Invoke(eventData);
            if (this.parentScrollRect != null && this.parentScrollEnable)
            {
                this.parentScrollRect.OnEndDrag(eventData);
                if(parentListView)
                {
                    ExecuteEvents.Execute(parentListView.gameObject, eventData, ExecuteEvents.endDragHandler);
                }
            }
        }

        public void Update()
        {
            try
            {
                if (this.autoScroll)
                {
                    if (this.autoScrollParam.y <= 0f)
                    {
                        this.SetContainerPos(this.autoScrollParam.z);
                        this.ShowContentAt(this.autoScrollParam.z);
                        this.autoScroll = false;
                    }
                    else
                    {
                        float num = this.GetContainerPos();
                        num += this.autoScrollParam.x * Time.deltaTime;
                        this.autoScrollParam.y = this.autoScrollParam.y - Time.deltaTime;
                        if ((this.autoScrollParam.z > this.autoScrollParam.w && num > this.autoScrollParam.z) ||
                            (this.autoScrollParam.z < this.autoScrollParam.w && num < this.autoScrollParam.z))
                        {
                            num = autoScrollParam.z;
                            autoScroll = false;
                        }

                        this.SetContainerPos(num);
                        this.ShowContentAt(num);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

    }
}