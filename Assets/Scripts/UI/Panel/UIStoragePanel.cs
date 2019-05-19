using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;

public class UIStoragePanel : UIPanel
{

	private Slot [] m_slots;
	private int [] m_slotId;
	private int m_currentSlot = -1;
	public override void PanelInit ()
	{
		base.PanelInit ();
		AddButtonClick ("Slot1", HandleUnityAction1);
		AddButtonClick ("Slot2", HandleUnityAction2);
		AddButtonClick ("Slot3", HandleUnityAction3);
		AddButtonClick ("Slot4", HandleUnityAction4);

		m_slots = new Slot [4];
	}



	public override void PanelOpen ()
	{
		base.PanelOpen ();

	}

	public override void PanelClose ()
	{
		base.PanelClose ();

	}

	public void SaveItemToSlot (int slotIndex, int id)
	{
		if (m_slots [slotIndex].m_name != "None") {

		} else {

		}
	}

	void HandleUnityAction1 ()
	{
		CheckSlot (0);
	}


	void HandleUnityAction2 ()
	{
		CheckSlot (1);

	}
	void HandleUnityAction3 ()
	{
		CheckSlot (2);

	}
	void HandleUnityAction4 ()
	{
		CheckSlot (3);

	}


	void CheckSlot (int slotIndex)
	{
		if (m_slots [slotIndex].m_name != "None") {
			UIManager.Instance ().ShowPanel<UIStorageSlotInfoPanel> (false, HandleCallback);
			m_currentSlot = slotIndex;

		} else {
			UIManager.Instance ().ShowOverlay<UIMessageOverlay> ("This is empty in slot" + (slotIndex + 1));
		}
	}
	void StorageItem ()
	{

	}

	void HandleCallback ()
	{
		UIStorageSlotInfoPanel panel = UIManager.Instance ().GetPanel<UIStorageSlotInfoPanel> ();
		if (m_currentSlot == -1) { Debug.Log ("Error index of slot."); return; }
		panel.ShowSlotInfo (m_currentSlot);
		m_currentSlot = -1;
	}

}
