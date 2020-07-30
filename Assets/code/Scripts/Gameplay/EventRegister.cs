using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using StarPlatinum.EventManager;
using UnityEngine;

public class EventRegister : MonoBehaviour
{
    public enum OperationAfterReceiveEvent
	{
        None,
        DisableGameobject,
        EnableGameobject
	}
    // Start is called before the first frame update
    void Start()
    {
		if (m_isInitialied == false) {
            m_boxCollider = GetComponent<BoxCollider> ();
            if (m_boxCollider != null) {
                EventManager.Instance.AddEventListener<RaiseEvent> (EventHandle);
                if (m_operationAfterReceiveEvent == OperationAfterReceiveEvent.EnableGameobject) {
                    m_boxCollider.enabled = false;
                }
            } else {
                Debug.LogError (gameObject.name + " + " + m_eventName + " is null");
            }
            m_isInitialied = true;
        }

    }

	private void EventHandle (object sender, RaiseEvent e)
	{
		if (e.m_eventType != StarPlatinum.StoryReader.StoryReader.EventType.invokeEvent) {
            return;
		}
		if (e.m_eventName != m_eventName) {
            return;
		}

		switch (m_operationAfterReceiveEvent) {
        case OperationAfterReceiveEvent.EnableGameobject:
			if (m_boxCollider == null) {
                Debug.LogError (m_eventName + " is triggered, but collision is null.");
                break;
			}
            m_boxCollider.enabled = true;
            break;

        case OperationAfterReceiveEvent.DisableGameobject:
            if (m_boxCollider == null) {
                Debug.LogError (m_eventName + " is triggered, but collision is null.");
                break;
            }
            m_boxCollider.enabled = false;
            break;

        default:
			break;
		}
	}

    public OperationAfterReceiveEvent m_operationAfterReceiveEvent = OperationAfterReceiveEvent.None;
    public string m_eventName = "";
    private BoxCollider m_boxCollider = null;
    private bool m_isInitialied = false;
}
