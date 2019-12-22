using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSystem
{

	private int m_day;

	// Use this for initialization
	public DataSystem ()
	{
		m_day = 0;

	}


	public int NextDay ()
	{
		m_day++;
		return m_day;
	}

	public int GetDay ()
	{
		return m_day;
	}

}
