using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{

	bool m_pass = false;



	public bool GetIsPassed ()
	{
		return m_pass;
	}

	// Use this for initialization
	void Start ()
	{
		m_pass = false;
	}
	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (!m_pass) {
			m_pass = true;
		}
	}
}
