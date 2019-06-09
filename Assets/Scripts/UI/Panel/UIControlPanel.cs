using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
using UnityEngine.UI;

public class UIControlPanel : UIPanel
{

	private Text m_data;
	private SceneRoot m_sceneRoot;
	public override void PanelInit ()
	{
		base.PanelInit ();
		AddButtonClick ("News", HandleUnityAction);
		AddButtonClick ("Manual", HandleUnityAction1);

		//Get UI elements
		m_data = transform.Find ("Data").GetComponent<Text> ();
		m_sceneRoot = GameObject.Find ("SceneRoot").GetComponent<SceneRoot> ();

	}

	public override void PanelOpen ()
	{
		base.PanelOpen ();
		ShowNewDay ();
	}

	public override void PanelClose ()
	{
		base.PanelClose ();

	}

	public void ShowNewDay ()
	{
		m_sceneRoot.NextDay ();

		m_data.text = "Day " + m_sceneRoot.GetData ();
	}

	void HandleUnityAction ()
	{
		UIManager.Instance ().ShowPanel<UINewsPanel> ();
	}

	void HandleUnityAction1 ()
	{

	}



}
