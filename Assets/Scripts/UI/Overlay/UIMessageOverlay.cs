using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum;
using UnityEngine.UI;

public class UIMessageOverlay : UIOverlay
{
	Text m_text;
	public override void PanelInit ()
	{
		base.PanelInit ();
		m_text = transform.Find ("Text").GetComponent<Text> ();
	}

	public override void PanelOpen ()
	{
		base.PanelOpen ();

	}

	public override void PanelClose ()
	{
		base.PanelClose ();

	}

	public override void Callback ()
	{
		//throw new System.NotImplementedException ();
	}

	public override void ShowMsg (string msg)
	{
		//throw new System.NotImplementedException ();
		m_text.text = msg;
	}
}
