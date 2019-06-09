using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
public class SceneRoot : MonoBehaviour
{
	DataSystem m_dataSystem;
	SlotInfoSystem m_slotInfoSystem;
	NewsSystem m_newsSystem;


	// Use this for initialization
	void Start ()
	{
		UIManager.Instance ().ShowPanel<UIControlPanel> ();
		UIManager.Instance ().ShowPanel<UINewsPanel> ();
		UIManager.Instance ().ShowPanel<UIStartIntroPanel> ();

		m_dataSystem = new DataSystem ();
		m_slotInfoSystem = new SlotInfoSystem ();
		m_newsSystem = new NewsSystem ();

	}

	public DataSystem GetDataSystem ()
	{
		return m_dataSystem;

	}

	public int GetData ()
	{
		return m_dataSystem.GetDay ();
	}

	public int NextDay ()
	{
		return m_dataSystem.NextDay ();
	}

	public string GetTitile (int data)
	{
		return m_newsSystem.GetTitle (data);
	}

	public string GetContent (int data)
	{
		return m_newsSystem.GetContent (data);
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
