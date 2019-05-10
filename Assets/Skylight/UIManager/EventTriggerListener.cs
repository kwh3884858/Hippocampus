using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;


public class EventTriggerListener : UnityEngine.EventSystems.EventTrigger
{
	public delegate void VoidDelegate ();
	public delegate void BoolDelegate (bool state);
	public delegate void FloatDelegate (float delta);
	public delegate void VectorDelegate (Vector2 delta);
	public delegate void ObjectDelegate (GameObject obj);
	public delegate void KeyCodeDelegate (KeyCode key);

	public VoidDelegate onClick;
	public VoidDelegate onDown;
	public VoidDelegate onEnter;
	public VoidDelegate onExit;
	public VoidDelegate onUp;
	public VoidDelegate onSelect;
	public VoidDelegate onUpdateSelect;


	static public EventTriggerListener Get (Transform transform)
	{
		EventTriggerListener listener = transform.GetComponent<EventTriggerListener> ();
		if (listener == null)
			listener = transform.gameObject.AddComponent<EventTriggerListener> ();
		return listener;
	}

	public override void OnPointerClick (PointerEventData eventData)
	{
		if (onClick != null)
			onClick ();
	}

	public override void OnPointerDown (PointerEventData eventData)
	{
		if (onDown != null)
			onDown ();
	}

	public override void OnPointerEnter (PointerEventData eventData)
	{
		if (onEnter != null)
			onEnter ();
	}

	public override void OnPointerExit (PointerEventData eventData)
	{
		if (onExit != null)
			onExit ();
	}

	public override void OnPointerUp (PointerEventData eventData)
	{
		if (onUp != null)
			onUp ();
	}

	public override void OnSelect (BaseEventData eventData)
	{
		if (onSelect != null)
			onSelect ();
	}

	public override void OnUpdateSelected (BaseEventData eventData)
	{
		if (onUpdateSelect != null)
			onUpdateSelect ();
	}

	public void OnHover (VoidDelegate enter, VoidDelegate exit)
	{
		onEnter += enter;
		onExit += exit;
	}

	public void OnPreHover (float coefficient)
	{
		OnHover (delegate {
			//GetComponent<RectTransform> ().localScale = new Vector3 (coefficient, coefficient, 0);
			Skylight.SoundService.Instance ().PlayEffect ("button04", false, 0.5f, true);
		}, delegate {
			//GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 0);
			Skylight.SoundService.Instance ().PlayEffect ("button04", false, 0.5f, true);
		});
	}

	public void OnClick (VoidDelegate down, VoidDelegate up)
	{
		onDown += down;
		onUp += up;
	}

	public void OnPreClick (string effectname = "ButtonConfirm", float scaleSize = 0.2f)
	{
		OnClick (delegate {
			GetComponent<RectTransform> ().localScale = new Vector3 (1 - scaleSize, 1 - scaleSize, 0);
			Skylight.SoundService.Instance ().PlayEffect (effectname);
		},
		delegate {
			GetComponent<RectTransform> ().localScale = new Vector3 (1 - scaleSize, 1 - scaleSize, 0);
			Skylight.SoundService.Instance ().PlayEffect (effectname);

		});

	}

	//public void RemoveClick(VoidDelegate down, VoidDelegate up)
	//{
	//    onDown -= down;
	//    onUp -= up;
	//}

}