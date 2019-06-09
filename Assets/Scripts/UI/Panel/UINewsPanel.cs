using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
using UnityEngine.UI;

public class UINewsPanel : UIPanel
{

	SceneRoot m_sceneRoot;

	Text m_title;
	Text m_content;
	public override void PanelInit ()
	{
		base.PanelInit ();

		AddButtonClick ("Close", HandleUnityAction);

		m_sceneRoot = GameObject.Find ("SceneRoot").GetComponent<SceneRoot> ();

		m_content = transform.Find ("Content").GetComponent<Text> ();
		m_title = transform.Find ("Titile").GetComponent<Text> ();
	}

	public override void PanelOpen ()
	{
		base.PanelOpen ();

		m_content.text = "";

		int data = m_sceneRoot.GetData ();
		m_content.text = m_sceneRoot.GetContent (data);
		m_title.text = m_sceneRoot.GetTitile (data);
	}

	public override void PanelClose ()
	{
		base.PanelClose ();

		m_content.text = "";

	}

	void HandleUnityAction ()
	{
		UIManager.Instance ().ClosePanel<UINewsPanel> ();
	}

}
