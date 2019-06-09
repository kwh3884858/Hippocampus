using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotSystem : MonoBehaviour
{

	private Slot [] m_slots;

	// Use this for initialization
	void Start ()
	{
		m_slots = new Slot [4];
		m_slots [0] = new Slot ();
		m_slots [0].m_name = "克托卡里斯列夫斯基";

	}

	// Update is called once per frame
	void Update ()
	{

	}
}
