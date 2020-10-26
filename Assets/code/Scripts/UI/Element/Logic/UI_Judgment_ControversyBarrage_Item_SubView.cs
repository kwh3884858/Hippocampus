using System.Collections;
using System.Collections.Generic;
using Config.Data;
using StarPlatinum;
using StarPlatinum.EventManager;
using StarPlatinum.Service;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public partial class UI_Judgment_ControversyBarrage_Item_SubView : UIElementBase
    {
        private List<UI_Judgment_BarrageText_Item_SubView> m_subViews =new List<UI_Judgment_BarrageText_Item_SubView>();
        
        private bool m_isSpecial = false;
        private bool m_isSlashed = false;
        private float m_speed = 0;
        private float m_moveTime = 0;
        public float MovingTime = 0;
        public BarrageItem Info;
        public override void BindEvent()
        {
            base.BindEvent();
        }

        public void SetInfo(BarrageItem info ,float distance)
        {
            m_isSlashed = false;
            MovingTime = 0;
            m_moveTime = 0;
            gameObject.SetActive(true);
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
                            LayoutRebuilder.ForceRebuildLayoutImmediate(m_go_container_HorizontalLayoutGroup.rectTransform());
                            LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_barrage_HorizontalLayoutGroup.rectTransform());
                            if (data.Index == info.CorrectIndex)
                            {
                                var speed = CommonConfig.Data.ControversyBarrageMoveSpeed;
                                m_speed = (distance + subView.transform.position.x - transform.position.x)/speed;
                                m_moveTime = speed;
                            }
                        }),m_go_container_ContentSizeFitter.transform);
                    continue;
                }
                m_subViews[i].gameObject.SetActive(true);
                m_subViews[i].SetInfo(data,info.IsSpecial);
                LayoutRebuilder.ForceRebuildLayoutImmediate(m_go_container_HorizontalLayoutGroup.rectTransform());
                LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_barrage_HorizontalLayoutGroup.rectTransform());
            }

            if (m_isSpecial != info.IsSpecial)
            {
                SetBG();
            }
        }

        public void Move()
        {
            transform.Translate(m_speed * Time.deltaTime*Vector3.left,Space.World);
            m_lbl_test_TextMeshProUGUI.text = m_moveTime.ToString();
            m_moveTime -= Time.deltaTime;
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

            return pos.x > m_img_behind_Image.transform.position.x;
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