using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
public class KnifeController : MonoBehaviour
{
	public float m_speed = 2f;
	public LayerMask m_layerMask;

	private Vector3 m_dir;

	bool m_isFlying = false;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{
		if (m_isFlying) {
			if (InputService.Instance ().GetInput (KeyMap.Knife) == true) {
				GameObject.Find ("Hero").transform.position = transform.position;
			}
		}
	}

	private void FixedUpdate ()
	{
		if (m_isFlying) {
			transform.position += m_dir * m_speed;

		}
	}

	public void Shoot (Vector3 pos, Vector3 dir)
	{
		transform.position = pos;

		m_dir = dir;

		m_isFlying = true;

	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.name == "Wall") {
			m_isFlying = false;
			StartCoroutine (Disappear (5));
		}
	}

	IEnumerator Disappear (float time)
	{

		while (time > 0) {
			time -= Time.fixedDeltaTime;
			yield return null;

		}
		if (!m_isFlying) {
			transform.position = new Vector3 (-100, -100, 0);

		}
	}


	public Vector3 GetDir ()
	{
		return m_dir;
	}

	public bool GetIsFlying ()
	{
		return m_isFlying;
	}
}
