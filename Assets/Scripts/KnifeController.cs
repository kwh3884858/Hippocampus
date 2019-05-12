using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
public class KnifeController : MonoBehaviour
{
	public float m_speed = 2f;
	public LayerMask m_layerMask;
	public float m_flyingExistTime = 2f;

	public float m_stayTime = 5f;

	private float m_flyingTime = 4f;

	private Vector3 m_dir;


	bool m_isFlying = false;

	bool m_isPowered = true;

	bool m_isStay = false;

	public Vector3 GetDir ()
	{
		return m_dir;
	}

	public float GetSpeed ()
	{
		return m_speed;
	}
	public float GetStayTime ()
	{
		return m_stayTime;
	}

	public bool GetIsFlying ()
	{
		return m_isFlying;
	}


	public bool GetIsStaying ()
	{
		return m_isStay;
	}
	// Use this for initialization
	void Start ()
	{
		m_isFlying = false;
		m_isPowered = true;
	}

	public void Shoot (Vector3 pos, Vector3 dir)
	{
		if (m_isFlying || !m_isPowered) return;

		transform.position = pos;

		m_dir = dir;

		m_isFlying = true;
		StopAllCoroutines ();
		StartCoroutine (FlyingTimer (m_flyingExistTime));
	}

	public void Recharge ()
	{
		m_isPowered = true;
	}

	public bool GetIsPowered ()
	{
		return m_isPowered;
	}
	// Update is called once per frame
	void Update ()
	{
		if (m_isFlying && m_isPowered) {
			if (InputService.Instance ().GetInput (KeyMap.Knife) == true) {
				GameObject go = GameObject.Find ("Hero");

				go.transform.position = transform.position;
				go.GetComponent<Rigidbody2D> ().velocity = m_dir * m_speed * 50f;

				m_isPowered = false;
			}
		}
	}

	private void FixedUpdate ()
	{
		if (m_isFlying) {
			transform.position += m_dir * m_speed * m_flyingExistTime;

		}
	}




	IEnumerator FlyingTimer (float time)
	{
		m_flyingExistTime = 1f;

		while (time > 0) {
			time -= Time.fixedDeltaTime;
			m_flyingExistTime += Time.fixedDeltaTime;
			yield return null;
		}

		if (m_isFlying) {
			Disappear ();
		}
	}



	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.name == "Barrier") {
			m_isFlying = false;
			Disappear ();
		}

		if (collision.gameObject.name == "Wall") {
			m_isFlying = false;
			StartCoroutine (Disappear (m_stayTime));
		}
	}

	IEnumerator Disappear (float time)
	{
		m_isStay = true;
		while (time > 0) {
			time -= Time.fixedDeltaTime;
			yield return null;

		}
		if (!m_isFlying) {
			transform.position = new Vector3 (-100, -100, 0);

			m_isStay = false;

		}
	}
	public void Disappear ()
	{

		transform.position = new Vector3 (-100, -100, 0);
		m_isStay = false;
		m_isFlying = false;

	}


}
