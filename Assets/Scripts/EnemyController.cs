using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

	public float m_length = 2f;
	public float m_velocityHorizontal = 1;
	private float m_current = 0;
	// Use this for initialization
	void Start ()
	{
		m_current = m_length;
	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void FixedUpdate ()
	{
		if (m_current > 0) {
			m_current -= Time.fixedDeltaTime;
			transform.localPosition = new Vector3 (
			transform.localPosition.x + m_velocityHorizontal * Time.fixedDeltaTime,
			transform.localPosition.y,
			transform.localPosition.z);
			//transform.position = new Vector3(transform.position.x+  )
		} else {
			m_current = m_length;
			m_velocityHorizontal = -m_velocityHorizontal;
		}


	}
}
