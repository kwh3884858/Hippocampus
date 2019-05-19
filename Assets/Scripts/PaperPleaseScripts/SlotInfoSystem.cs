using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInfoSystem
{

	string [] m_titles;
	string [] m_contents;
	// Use this for initialization
	public SlotInfoSystem ()
	{
		m_titles = new string [] { "", "", "" };
		m_contents = new string [] { "", "", "" };
	}


	public string GetTitle (int index)
	{
		return m_titles [index];
	}

	public string GetContent (int index)
	{
		return m_contents [index];
	}
}
