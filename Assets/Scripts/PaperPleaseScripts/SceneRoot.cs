using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
public class SceneRoot : MonoBehaviour
{
	DataSystem m_dataSystem;
	SlotInfoSystem m_slotInfoSystem;
	// Use this for initialization
	void Start ()
	{
		UIManager.Instance ().ShowPanel<UIControlPanel> ();
		UIManager.Instance ().ShowPanel<UINewsPanel> ();
		UIManager.Instance ().ShowPanel<UIStartIntroPanel> ();

		m_dataSystem = new DataSystem ();
	}

	public DataSystem GetDataSystem ()
	{
		return m_dataSystem;

	}

	public int GetData ()
	{
		return m_dataSystem.GetDay ();
	}

	public void NextDay ()
	{
		m_dataSystem.NextDay ();
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
