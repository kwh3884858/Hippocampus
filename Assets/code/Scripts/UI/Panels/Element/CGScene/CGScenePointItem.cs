﻿using System;
using Config.Data;
using Controllers.Subsystems;
using StarPlatinum;
using StarPlatinum.Services.EffectService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Panels.Element
{
    public class CGScenePointItem: UIElementBase,IPointerEnterHandler,IPointerExitHandler
    {
        private void Start()
        {
            m_button.onClick.AddListener(OnClick);
            m_mousePos = transform.position;
        }

        private Vector3 m_mousePos;
        private void Update()
        {
            if (m_isMouseEnter)
            {
                m_mousePos.x = Input.mousePosition.x;
                m_mousePos.y = Input.mousePosition.y;
                m_effectObj.position = Camera.main.ScreenToWorldPoint(m_mousePos);
            }
        }

        public void SetPointInfo(CGScenePointInfo pointInfo)
        {
            m_pointInfo = pointInfo;
            RefreshPointInfo();
        }

        private void RefreshPointInfo()
        {
            var pointConfig = CGScenePointConfig.GetConfigByKey(m_pointInfo.ID);
            transform.localPosition = new Vector3(pointConfig.posX,pointConfig.posY,transform.localPosition.z);
            RefreshTouchInfo();
        }

        private void RefreshTouchInfo()
        {
            var config = CGScenePointTouchConfig.GetConfigByKey(int.Parse(string.Format("{0}{1:D2}",m_pointInfo.ID,m_pointInfo.touchNum)));
            PrefabManager.Instance.InstantiateAsync<GameObject>(config.mouseEffectKey, (result) =>
            {
                if (m_mouseEffectObj != null)
                {
                    Destroy(m_mouseEffectObj);
                }
                m_mouseEffectObj = result.result as GameObject;
            },m_effectObj);
        }

        private void OnClick()
        {
            if (m_pointInfo != null)
            {
                Debug.LogError("未初始化数据");
                return;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            m_isMouseEnter = true;
            m_effectObj.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            m_isMouseEnter = false;
            m_effectObj.gameObject.SetActive(false);
        }
        
        [SerializeField] private Transform m_effectObj;
        [SerializeField] private Button m_button;

        private GameObject m_mouseEffectObj;
        
        private CGScenePointInfo m_pointInfo;
        private bool m_isMouseEnter = false;
    }
}