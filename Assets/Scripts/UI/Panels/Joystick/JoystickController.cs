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
    public ETCJoystick joystick;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            joystick.axisX.directTransform = player.transform;
            joystick.axisY.directTransform = player.transform;
        }

    }

    #region UI Callback
    public void OnBeginDrag(PointerEventData eventData)
    {
        ((IBeginDragHandler)joystick).OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ((IDragHandler)joystick).OnDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        joystick.transform.position = eventData.position;
        joystick.OnPointerDown(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ((IPointerEnterHandler)joystick).OnPointerEnter(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ((IPointerUpHandler)joystick).OnPointerUp(eventData);
    }
    #endregion
}
