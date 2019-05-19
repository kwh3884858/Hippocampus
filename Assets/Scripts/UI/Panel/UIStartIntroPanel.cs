using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;

public class UIStartIntroPanel : UIPanel
{

	public override void PanelInit ()
	{
		base.PanelInit ();
		AddButtonClick ("Apply", HandleUnityAction);
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
		UIManager.Instance ().ClosePanel<UIStartIntroPanel> ();
	}


}
