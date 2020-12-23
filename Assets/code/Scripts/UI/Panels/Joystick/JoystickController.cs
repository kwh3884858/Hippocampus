using GamePlay.Player;
using StarPlatinum.Services;
using System.Collections;
using System.Collections.Generic;
using UI.Panels;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickController : UIPanel<UIDataProviderGameScene, TalkDataProvider>,IPointerDownHandler, IDragHandler, IPointerEnterHandler, IBeginDragHandler, IPointerUpHandler
{
    public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
    {
        base.Initialize(uiDataProvider, settings);
    }
    /// <summary>控制虚拟摇杆的位置</summary>
    public Transform joystick;
    /// <summary>摇杆的移动控制</summary>
    [SerializeField]
    private UnityEngine.InputSystem.OnScreen.OnScreenStick screenStick = null;
    /// <summary>自身位置</summary>
    private RectTransform myPostionRectTrans = null;

    private void Start()
    {
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        //if (player != null)
        //{
        //    joystick.axisX.directTransform = player.transform;
        //    joystick.axisY.directTransform = player.transform;
        //}
        if(joystick != null)
        {
            myPostionRectTrans = joystick as RectTransform;
        }
        //InputService.Instance.Input.PlayerControls.Move.canceled += Move_canceled;
        //Now use the new input system, all value should enter unity input system.
        //joystick.onMove.AddListener(PlayerController.Instance().JoystickMoveEvent);
        //joystick.onMoveEnd.AddListener(PlayerController.Instance().JoystickMoveEndEvent);

    }

    //private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    //{
    //    throw new System.NotImplementedException();
    //}

    #region UI Callback
    public void OnBeginDrag(PointerEventData eventData)
    {
        //((IBeginDragHandler)joystick).OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (screenStick != null)
        {
            screenStick.OnDrag(eventData);
        }
        //((IDragHandler)joystick).OnDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //myPostionRectTrans.anchoredPosition = eventData.position;
        //joystick.transform.position = eventData.position;
        //joystick.SetisNoReturnThumb();
        if(screenStick != null)
        {
            screenStick.OnPointerDown(eventData);
        }
        //joystick.OnPointerDown(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //((IPointerEnterHandler)joystick).OnPointerEnter(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (screenStick != null)
        {
            screenStick.OnPointerUp(eventData);
        }
        //((IPointerUpHandler)joystick).OnPointerUp(eventData);
    }
    #endregion
}
