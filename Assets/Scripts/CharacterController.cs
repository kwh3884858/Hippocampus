using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
public class CharacterController : MonoBehaviour
{
	enum WeaponState
	{
		None,
		Knife,
		SetDoor,

	}

	enum DoorState
	{
		None,
		One,
		Two,

	}

	public float m_moveSpeed = 5f;
	public float m_jumpForce = 60f;
	public LayerMask m_layerMask;

	private WeaponState m_state;

	private bool m_isOldFaceRight = false;
	private bool m_isFaceRight = false;

	private Rigidbody2D m_rigidbody2D;
	private BoxCollider2D m_boxCollider2D;

	private float m_rayDistance;


	bool m_isLightAttack;
	bool m_isHeavyAttack;

	DoorState m_doorState = DoorState.None;
	//bool isDoorExist = false;
	// Use this for initialization
	void Start ()
	{
		m_rigidbody2D = transform.GetComponent<Rigidbody2D> ();
		m_boxCollider2D = transform.GetComponent<BoxCollider2D> ();

		m_rayDistance = m_boxCollider2D.bounds.extents.y + 0.1f;

		m_state = WeaponState.None;

	}

	// Update is called once per frame
	void Update ()
	{
		if (InputService.Instance ().GetInput (KeyMap.Knife) == true) {
			m_state = WeaponState.Knife;
		}


		if (InputService.Instance ().GetInput (KeyMap.Door) == true) {
			m_state = WeaponState.SetDoor;
		}

		//Whether enter a door




		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			if (m_state == WeaponState.Knife) {
				Debug.Log ("Shoot knife");


				Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

				Vector2 dir = new Vector2 (mousePos.x - transform.position.x, mousePos.y - transform.position.y);
				dir.Normalize ();
				GameObject.Find ("Knife").GetComponent<KnifeController> ().Shoot (transform.position, dir);
			}


			if (m_state == WeaponState.SetDoor) {
				if (m_doorState == DoorState.None) {
					Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

					Vector2 dir = new Vector2 (mousePos.x - transform.position.x, mousePos.y - transform.position.y);
					dir.Normalize ();
					bool result = GameObject.Find ("Door1").GetComponent<DoorController> ().Shoot (new Vector3 (transform.position.x + dir.x, transform.position.y + dir.y, transform.position.z), dir);

					if (result) {
						m_doorState = DoorState.One;
						return;
					}
				}
				if (m_doorState == DoorState.One) {
					Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

					Vector2 dir = new Vector2 (mousePos.x - transform.position.x, mousePos.y - transform.position.y);
					dir.Normalize ();
					bool result = GameObject.Find ("Door2").GetComponent<DoorController> ().Shoot (new Vector3 (transform.position.x + dir.x, transform.position.y + dir.y, transform.position.z), dir);

					if (result) {
						m_doorState = DoorState.Two;
						return;
					}
				}

				if (m_doorState == DoorState.Two) {

					//bool result = false;
					//result = GameObject.Find ("Door2").GetComponent<DoorController> ().GetIsDoorStay ();
					//result = GameObject.Find ("Door1").GetComponent<DoorController> ().GetIsDoorStay ();

					//if (result) {
					GameObject.Find ("Door1").GetComponent<DoorController> ().Disappear ();
					GameObject.Find ("Door2").GetComponent<DoorController> ().Disappear ();
					m_doorState = DoorState.None;
					return;
					//}
				}

			}
		}


	}


	void FixedUpdate ()
	{
		//if (m_isLightAttack || m_isHeavyAttack) return;

		float horizontalAxis = InputService.Instance ().GetAxis (KeyMap.Horizontal);
		float verticalAxis = InputService.Instance ().GetAxis (KeyMap.Vertical);
		if (horizontalAxis > 0.1f || horizontalAxis < -0.1f) {
			Debug.Log (InputService.Instance ().GetAxis (KeyMap.Horizontal));
			m_rigidbody2D.velocity = new Vector2 (horizontalAxis * m_moveSpeed, m_rigidbody2D.velocity.y);
			//transform.localPosition = new Vector3 (
			//transform.localPosition.x + horizontalAxis * m_moveSpeed * Time.fixedDeltaTime,
			//transform.localPosition.y,
			//transform.localPosition.z);

		}
		if (verticalAxis > 0.1f) {
			Debug.Log (InputService.Instance ().GetAxis (KeyMap.Horizontal));


			RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, m_rayDistance, m_layerMask);

			if (hit.collider != null) {
				Debug.Log ("Jump!");

				m_rigidbody2D.AddForce (new Vector2 (0, m_jumpForce));
			}
		}

		if (horizontalAxis < -0.1f) {
			m_isFaceRight = false;
		}
		if (horizontalAxis > 0.1f) {
			m_isFaceRight = true;
		}

		if (m_isFaceRight != m_isOldFaceRight) {

			transform.localScale = new Vector3 (
			 -transform.localScale.x,
			 transform.localScale.y,
			 transform.localScale.z);
			m_isOldFaceRight = m_isFaceRight;

		}
	}
}
