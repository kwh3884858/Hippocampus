using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;

public class UINewsPanel : UIPanel
{

	NewsSystem newsSystem;
	public override void PanelInit ()
	{
		base.PanelInit ();
		newsSystem = new NewsSystem ();
		AddButtonClick ("Close", HandleUnityAction);
	}

	public override void PanelOpen ()
	{
		base.PanelOpen ();

	}

	public override void PanelClose ()
	{
		base.PanelClose ();

	}

	void HandleUnityAction ()
	{
		UIManager.Instance ().ClosePanel<UINewsPanel> ();
	}

}
