using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
public class DoorController : MonoBehaviour
{
	public float m_speed = 0.2f;
	public LayerMask m_layerMask;
	public int m_detectCount = 10;

	private Vector3 m_dir;
	private Collider2D m_collider2D;
	private float m_radius;

	private Transform m_sibline;

	bool isFlying = false;
	bool m_isStay = false;

	float interval;

	// Use this for initialization
	void Start ()
	{

		interval = 360f / m_detectCount;
		m_collider2D = transform.GetComponent<Collider2D> ();
		m_radius = m_collider2D.bounds.extents.y + 0.1f;

		if (gameObject.name == "Door1") {
			m_sibline = GameObject.Find ("Door2").transform;
		} else {
			m_sibline = GameObject.Find ("Door1").transform;

		}
	}

	// Update is called once per frame
	void Update ()
	{

	}

	private void FixedUpdate ()
	{
		if (isFlying) {
			transform.position += m_dir * m_speed;

		}
	}

	public bool Shoot (Vector3 pos, Vector3 dir)
	{
		if (m_isStay) return false;
		transform.position = pos;
		if (dir.x <= 0.001f && dir.x >= -0.001f) {
			if (dir.y >= 0f) {
				transform.localRotation = Quaternion.Euler (0, 0, 0);

			} else {
				transform.localRotation = Quaternion.Euler (0, 0, 180);

			}
		}
		float atan = dir.y / dir.x;
		atan = Mathf.Atan (atan) * 57.2957f;
		if (dir.y >= 0) {
			transform.localRotation = Quaternion.Euler (0, 0, atan + 90);

		} else {
			transform.localRotation = Quaternion.Euler (0, 0, atan - 90);

		}


		m_dir = dir;

		isFlying = true;

		return true;
	}

	private void OnTriggerEnter2D (Collider2D collision)
	{
		if (collision.gameObject.name == "Wall") {
			isFlying = false;
			List<int> touched = new List<int> ();
			for (int i = 0; i < m_detectCount; i++) {

				Vector3 rotateQuaternion = Quaternion.AngleAxis (interval * i, Vector3.forward) * Vector3.up;



				RaycastHit2D raycastHit2D = Physics2D.Raycast (transform.position, rotateQuaternion, m_radius, m_layerMask);
				if (raycastHit2D.collider != null) {
					Debug.DrawLine (transform.position, raycastHit2D.point, Color.green, 20f);

					touched.Add (i);
					Debug.Log ("Angle:" + i);
				}
			}
			if (touched.Count == 0) {
				m_isStay = true;
				Disappear ();
				return;
			}
			float rotate = FindLeftestLine (touched) * interval - (touched.Count - 1) * interval / 2;

			if (rotate > 360) rotate -= 360;
			rotate -= 90;
			transform.localRotation = Quaternion.Euler (0, 0, rotate);

			Debug.DrawRay (transform.position, transform.InverseTransformDirection (Vector3.left), Color.red, 20f);


			m_isStay = true;
			//StartCoroutine (Disappear (5));
		}

		if (m_isStay) {
			if (collision.gameObject.name == "Hero") {
				Transform hero = collision.gameObject.transform;
				if (hero.GetComponent<Rigidbody2D> ().velocity.y <= 0.3f) {
					float dotResult = Vector3.Dot (hero.InverseTransformDirection (Vector3.up), transform.InverseTransformDirection (Vector3.left));
					if (dotResult >= 0.5f) {
						bool isSiblingStay = m_sibline.GetComponent<DoorController> ().GetIsDoorStay ();
						if (isSiblingStay) {
							hero.position = m_sibline.position;
						}
					}
				}


			}

			if (collision.gameObject.name == "Knife") {
				Transform knife = collision.gameObject.transform;
				KnifeController knifeController = knife.GetComponent<KnifeController> ();

				if (knifeController.GetIsFlying ()) {

					bool isSiblingStay = m_sibline.GetComponent<DoorController> ().GetIsDoorStay ();
					if (isSiblingStay) {
						Vector3 dir = knifeController.GetDir ();
						knife.position = m_sibline.position + dir * m_radius;
					}
				}
			}

		}

	}
	private void OnDrawGizmos ()
	{

		//for (int i = 0; i < m_detectCount; i++) {
		//	Vector3 rotateQuaternion = Quaternion.AngleAxis (interval * i, Vector3.forward) * Vector3.up;
		//	Debug.DrawLine (transform.position, transform.position + (rotateQuaternion * m_radius), Color.green);
		//	//Debug.DrawRay (transform.position, rotateQuaternion, Color.green,);
		//}
	}

	int FindLeftestLine (List<int> lines)
	{
		bool isZero = false;
		foreach (int i in lines) {
			if (i == 0) {
				isZero = true;
			}
		}

		if (isZero) {
			if (lines.Count == 1) return lines [lines.Count - 1];
			int index = 0;
			for (int i = 0; i < lines.Count - 1; i++) {
				if (lines [i + 1] - lines [i] == 1) {
					index = i + 1;
				}
			}
			return lines [index];
		} else {
			return lines [lines.Count - 1];
		}
	}

	public void Disappear ()
	{
		if (m_isStay) {
			transform.position = new Vector3 (-100, -100, 0);

		}

		m_isStay = false;

	}

	public bool GetIsDoorStay ()
	{
		return m_isStay;
	}
}
