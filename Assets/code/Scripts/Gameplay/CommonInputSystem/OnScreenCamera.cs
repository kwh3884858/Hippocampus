using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.InputSystem.Layouts;

namespace MyInputSystem
{
    /// <summary>
    /// A stick control displayed on screen and moved around by touch or other pointer
    /// input.
    /// </summary>
    [AddComponentMenu("Input/On-Screen Camera")]
    public class OnScreenCamera : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {

        private void LateUpdate()
        {
            if (m_IsDrag)
            {
                if(m_curPos == m_LastPos)
                {
                    SendValueToControl(Vector2.zero);
                }
                else
                {
                    m_curPos = m_LastPos;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData == null)
                throw new System.ArgumentNullException(nameof(eventData));
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out m_LastPos);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData == null)
                throw new System.ArgumentNullException(nameof(eventData));
            m_IsDrag = true;
            //var position = eventData.position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
            Vector2 delta = position - m_LastPos;
            m_LastPos = position;

            //var delta = position - m_PointerDownPos;

            delta = Vector2.ClampMagnitude(delta, movementRange);
            //((RectTransform)transform).anchoredPosition = m_StartPos + (Vector3)delta;

            var newPos = new Vector2(delta.x / movementRange, delta.y / movementRange);
            SendValueToControl(newPos);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_IsFirst = true;
            m_IsDrag = false;
            SendValueToControl(Vector2.zero);
        }

        public float movementRange
        {
            get => m_MovementRange;
            set => m_MovementRange = value;
        }

        [FormerlySerializedAs("movementRange")]
        [SerializeField]
        private float m_MovementRange = 50;

        [InputControl(layout = "Vector2")]
        [SerializeField]
        private string m_ControlPath;

        private Vector2 m_PointerDownPos;
        /// <summary>上一次计算detal的位置</summary>
        private Vector2 m_LastPos;
        private bool m_IsFirst = true;
        private bool m_IsDrag = false;
        private Vector2 m_curPos;

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }
    }
}
