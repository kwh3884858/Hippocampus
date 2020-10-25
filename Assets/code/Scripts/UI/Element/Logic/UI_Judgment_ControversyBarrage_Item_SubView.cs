using System.Collections;
using System.Collections.Generic;
using StarPlatinum;
using StarPlatinum.EventManager;
using UnityEngine;

namespace UI.Panels.Element
{
    public partial class UI_Judgment_ControversyBarrage_Item_SubView : UIElementBase
    {
        private List<UI_Judgment_BarrageText_Item_SubView> m_subViews =new List<UI_Judgment_BarrageText_Item_SubView>();
        
        private bool m_isSpecial = false;
        private bool m_isSlashed = false;
        public float MovingTime = 0;
        public BarrageItem Info;
        public override void BindEvent()
        {
            base.BindEvent();
        }

        public void SetInfo(BarrageItem info)
        {
            gameObject.SetActive(false);
            Info = info;
            m_pl_barrage_MakeChildTransparent.Transparent(false);
            HideSubView();
            for (int i = 0; i < info.Items.Count; i++)
            {
                var data = info.Items[i];
                if (i >= m_subViews.Count)
                {
                    PrefabManager.Instance.InstantiateAsync<UI_Judgment_BarrageText_Item_SubView>("UI_Judgment_BarrageText_Item",(
                        result =>
                        {
                            if (result.status == RequestStatus.FAIL)
                            {
                                return;
                            }

                            var subView = result.result as UI_Judgment_BarrageText_Item_SubView;
                            subView.Init(subView.GetComponent<RectTransform>());
                            m_subViews.Add(subView);
                            subView.SetInfo(data,info.IsSpecial);
                        }),m_go_container_ContentSizeFitter.transform);
                    return;
                }
                m_subViews[i].gameObject.SetActive(true);
                m_subViews[i].SetInfo(data,info.IsSpecial);
            }

            if (m_isSpecial != info.IsSpecial)
            {
                SetBG();
            }
        }

        public void Slash()
        {
            if (m_isSlashed)
            {
                return;
            }

            m_isSlashed = true;
            
            m_pl_barrage_MakeChildTransparent.Transparent(true);
        }
        
        public bool IsPassed(Vector3 pos)
        {
            
            
            Vector3 myPos= Camera.main.WorldToScreenPoint(m_img_behind_Image.transform.position);
            Vector3 otherPos = Camera.main.WorldToScreenPoint(pos);

            return myPos.x < otherPos.x;
        }

        private void HideSubView()
        {
            foreach (var subView in m_subViews)
            {
                subView.gameObject.SetActive(false);
            }
        }
        
        private void SetBG()
        {
            if (m_isSpecial)
            {
                PrefabManager.Instance.SetImage(m_img_front_Image, UIRes.SpecialBarrageFront);
                PrefabManager.Instance.SetImage(m_img_behind_Image, UIRes.SpecialBarrageBehind);
            }
            else
            {
                PrefabManager.Instance.SetImage(m_img_front_Image, UIRes.NormalBarrageFront);
                PrefabManager.Instance.SetImage(m_img_behind_Image, UIRes.NormalBarrageBehind);
            }
        }
    }
}