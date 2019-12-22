using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadComponent : MonoBehaviour
{

	Collider2D m_collider2D;
	// Use this for initialization
	void Start ()
	{
		m_collider2D = GetComponent<Collider2D> ();

	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.name == "Hero") {
			LoadController load = GameObject.Find ("SavaPoint").GetComponent<LoadController> ();
			load.Reload (collision.gameObject);
		}
	}
}
