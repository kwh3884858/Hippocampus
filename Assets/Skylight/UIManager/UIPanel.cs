using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Skylight
{
	public abstract class UIPanel : MonoBehaviour
	{

		public Dictionary<string, object> m_userData = null;


		/// <summary>
		/// only call once when initialization
		/// 在panel初始化的时候执行，只执行一次
		/// 建议做一些初始化的动作，例如绑定UI元素
		/// 不需要手动调用，管理器会自动调用
		/// 只需要手动重写即可
		/// </summary>
		public virtual void PanelInit ()
		{
			EventManager.Instance ().Notify ((int)LogicType.PanelInit);
		}


		//every open will call
		/// <summary>
		/// every open will call
		/// 每次打开这个panel的时候都会调用一次
		/// 建议做一些需要根据场景的不同而动态改变的逻辑处理
		/// 不需要手动调用，管理器会自动调用
		/// 只需要手动重写即可
		/// </summary>
		public virtual void PanelOpen ()
		{
			EventManager.Instance ().Notify ((int)LogicType.PanelOpen);

		}


		//every close will call
		public virtual void PanelClose ()
		{
			EventManager.Instance ().Notify ((int)LogicType.PanelClose);

		}

		public T GetControl<T> (string name)
		{
			var tran = transform.Find (name);
			if (tran) {
				return tran.GetComponent<T> ();
			}

			return default (T);
		}

		public void AddButtonClick (string name, UnityAction callback)
		{
			Button button = GetControl<Button> (name);
			if (button) {
				button.onClick.AddListener (callback);
			} else {
				Debug.LogWarning (name + " Not Found");
			}
		}

		public void AddClickEvent (string name, System.Action<BaseEventData> callback)
		{
			EventTrigger trigger = GetControl<EventTrigger> (name);
			if (trigger == null) {
				Transform tran = transform.Find (name);
				if (tran)
					trigger = tran.gameObject.AddComponent<EventTrigger> ();
			}

			EventTrigger.Entry entry = new EventTrigger.Entry ();
			entry.eventID = EventTriggerType.PointerUp;
			entry.callback = new EventTrigger.TriggerEvent ();
			entry.callback.AddListener (new UnityEngine.Events.UnityAction<BaseEventData> (callback));
			trigger.triggers.Add (entry);
		}

		public void AddEventTriggerListener (GameObject go, EventTriggerType eventType, System.Action<BaseEventData> callback)
		{
			EventTrigger trigger = go.GetComponent<EventTrigger> ();
			if (trigger == null)
				trigger = go.AddComponent<EventTrigger> ();

			EventTrigger.Entry entry = new EventTrigger.Entry ();
			entry.eventID = eventType;
			entry.callback = new EventTrigger.TriggerEvent ();
			entry.callback.AddListener (new UnityEngine.Events.UnityAction<BaseEventData> (callback));
			trigger.triggers.Add (entry);
		}

		/// <summary>
		/// 删除所有的自对象
		/// </summary>
		/// <param name="go">Go.</param>
		public void DeleteAllChild (GameObject go)
		{
			if (go == null || go.transform.childCount == 0) {
				return;
			}
			int count = go.transform.childCount;
			for (int i = 0; i < count; i++) {
				Destroy (go.transform.GetChild (i).gameObject);
			}

		}

		/// <summary>
		/// 一次性设置所有的子对象的active
		/// </summary>
		/// <param name="go">Go.</param>
		/// <param name="isActive">If set to <c>true</c> is active.</param>
		public void SetActiveAllChild (GameObject go, bool isActive)
		{
			if (go == null || go.transform.childCount == 0) {
				return;
			}
			int count = go.transform.childCount;
			for (int i = 0; i < count; i++) {
				go.transform.GetChild (i).gameObject.SetActive (isActive);
			}
		}
	}
}
