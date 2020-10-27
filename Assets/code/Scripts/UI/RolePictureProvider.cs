using System;
using System.Collections.Generic;
using Const;
using StarPlatinum;
using UI.Panels.Element;
using UnityEngine;

namespace UI
{
    public class RolePictureProvider
    {
        public class PictureRequest
        {
            public string ID;
            public Action<PictureItem> Callback;

            public PictureRequest(string id,Action<PictureItem> callback)
            {
                ID = id;
                Callback = callback;
            }
        }
        private const string m_pictureItemKey = "picture_item";
        private const int m_idNum = 10;

        public RolePictureProvider()
        {
            m_pictureItems = new Queue<PictureItem>();
            m_request=new Queue<PictureRequest>();
        }

        public void GetPictureItem(string id,Action<PictureItem> callback)
        {
            var item = GetItem();
            if (item != null)
            {
                SetItemInfo(item,id);
                callback.Invoke(item);
            }
            else
            {
                m_request.Enqueue(new PictureRequest(id,callback));
            }
        }

        public void ReleasePictureItem(PictureItem item)
        {
            item.Release();
            m_pictureItems.Enqueue(item);
        }

        private void SetItemInfo(PictureItem item, string id)
        {
            if (item == null&&id.Length!=m_idNum)
            {
                Debug.LogWarning("立绘图片获取失败，数据错误");
                return;
            }

            string roleID = id;

            item.Initialize(id);
            
            PrefabManager.Instance.LoadAssetsAsync<Sprite>(new List<string>(){roleID}, (RequestResult) =>
            {
                if (RequestResult.status == RequestStatus.FAIL)
                {
                    Debug.LogWarning("立绘图片获取失败，加载失败");
                    return;
                }
                item.SetBody(RequestResult.result as Sprite);
            },null );


        }

        private PictureItem GetItem()
        {
            if (m_pictureItems.Count > 0)
            {
                return m_pictureItems.Dequeue();
            }
            else
            {
                PrefabManager.Instance.InstantiateComponentAsync<PictureItem>(m_pictureItemKey, (result) =>
                    {
                        if (result.status == RequestStatus.FAIL)
                        {
                            Debug.LogError("立绘预设加载失败,请检查！");
                            return;
                        }
                        m_pictureItems.Enqueue(result.result as PictureItem);
                        GetNewItem();
                    });
                return null;
            }
        }

        private void GetNewItem()
        {
            var request = m_request.Count > 0?m_request?.Dequeue():null;
            if (request != null)
            {
                var item = m_pictureItems.Dequeue();
                if (item != null)
                {
                    SetItemInfo(item,request.ID);
                    request.Callback?.Invoke(item);
                }
            }
        }
        
        private Queue<PictureRequest> m_request;
        private Queue<PictureItem> m_pictureItems;
    }
}