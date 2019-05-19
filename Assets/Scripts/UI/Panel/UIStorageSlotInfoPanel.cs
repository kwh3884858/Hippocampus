using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
using UnityEngine.UI;
public class UIStorageSlotInfoPanel : UIPanel
{

	Text m_title;
	Text m_content;
	SlotInfoSystem m_slotInfoSystem;
	public override void PanelInit ()
	{
		base.PanelInit ();
		m_title = transform.Find ("Title").GetComponent<Text> ();
		m_content = transform.Find ("Content").GetComponent<Text> ();

		AddButtonClick ("CLose", HandleUnityAction);
		m_slotInfoSystem = GameObject.Find ("SceneRoot").GetComponent<SlotInfoSystem> ();
	}

	public override void PanelOpen ()
	{
		base.PanelOpen ();

	}

	public override void PanelClose ()
	{
		base.PanelClose ();

	}

	public void ShowSlotInfo (int index)
	{
		m_title.text = m_slotInfoSystem.GetTitle (index);
		m_content.text = m_slotInfoSystem.GetContent (index);
	}

	void HandleUnityAction ()
	{
		UIManager.Instance ().ClosePanel<UIStorageSlotInfoPanel> ();
	}

}
