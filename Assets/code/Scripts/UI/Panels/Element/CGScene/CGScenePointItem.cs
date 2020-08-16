using System;
using Config.Data;
using Controllers.Subsystems;
using StarPlatinum;
using StarPlatinum.EventManager;
using StarPlatinum.Services.EffectService;
using UI.UIComponent;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public enum CGScenePointTouchType
    {
        DeathBody=1,
        Item=2,
        NPC=3,
    }
    public class CGScenePointItem: UIElementBase,IPointerEnterHandler,IPointerExitHandler
    {
        public Action<int,CGScenePointTouchConfig> ClickCallback;
        private void Start()
        {
            m_button.onClick.AddListener(OnClick);
            m_mousePos = transform.position;
            m_flashing.enabled = false;
        }

        private void OnDisable()
        {
            m_isMouseEnter = false;
        }

        private Vector3 m_mousePos;
        private void Update()
        {
//            if (m_isMouseEnter)
//            {
//                m_mousePos.x = Input.mousePosition.x;
//                m_mousePos.y = Input.mousePosition.y;
//                m_effectObj.position = Camera.main.ScreenToWorldPoint(m_mousePos);
//            }
        }
        public void SetPointInfo(CGScenePointInfo pointInfo)
        {
            m_pointInfo = pointInfo;
            RefreshPointInfo();
        }

        private void RefreshPointInfo()
        {
//            var pointConfig = CGScenePointConfig.GetConfigByKey(m_pointInfo.ID);
//            transform.localPosition = new Vector3(pointConfig.posX,pointConfig.posY,transform.localPosition.z);
//            m_root_RectTransform.sizeDelta = new Vector2(pointConfig.width,pointConfig.height);
            RefreshTouchInfo();
        }

        private void RefreshTouchInfo()
        {
            m_config = GlobalManager.GetControllerManager().CGSceneController.GetTouchConfigByPointID(m_pointInfo.ID);
            if (m_isMouseEnter)
            {
                EventManager.Instance.SendEvent(new ChangeCursorEvent(){cursorKey = GetMouseKeyByTouchType(m_config.touchType)});
            }
//            PrefabManager.Instance.InstantiateAsync<GameObject>(m_config.mouseEffectKey, (result) =>
//            {
//                if (m_mouseEffectObj != null)
//                {
//                    Destroy(m_mouseEffectObj);
//                }
//                m_mouseEffectObj = result.result as GameObject;
//            },m_effectObj);
        }

        private void OnClick()
        {
            SoundService.Instance.PlayEffect("UI_Click0");
            if (m_pointInfo == null)
            {
                Debug.LogError("未初始化数据");
                return;
            }
            ClickCallback?.Invoke(m_pointInfo.ID,m_config);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_isMouseEnter = true;
            EventManager.Instance.SendEvent(new ChangeCursorEvent(){cursorKey = GetMouseKeyByTouchType(m_config.touchType)});
            m_effectObj.gameObject.SetActive(true);
            m_flashing.enabled = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_isMouseEnter = false;
            EventManager.Instance.SendEvent(new ChangeCursorEvent(){cursorKey = null});
            m_effectObj.gameObject.SetActive(false);
            m_flashing.enabled = false;
        }

        private string GetMouseKeyByTouchType(int touchType)
        {
            switch ((CGScenePointTouchType)touchType)
            {
                case CGScenePointTouchType.DeathBody:
                    return "UI_Cursor2";
                case CGScenePointTouchType.Item:
                    return "UI_Cursor1";
                case CGScenePointTouchType.NPC:
                    return "UI_Cursor2";
                default:
                    return null;
            }
        }
        
        [SerializeField] private Transform m_effectObj;
        [SerializeField] private Button m_button;
        [SerializeField] private Flashing m_flashing;

        private GameObject m_mouseEffectObj;
        private CGScenePointTouchConfig m_config;
        
        private CGScenePointInfo m_pointInfo;
        private bool m_isMouseEnter = false;
    }
}