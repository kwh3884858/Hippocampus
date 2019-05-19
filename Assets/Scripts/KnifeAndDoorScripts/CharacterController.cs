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
	public LayerMask m_enemyLayerMask;
	public float m_effectRange = 2f;
	//public float m_stayWithKnifeTime = 3;

	private WeaponState m_state;

	private bool m_isOldFaceRight = false;
	private bool m_isFaceRight = false;

	private Rigidbody2D m_rigidbody2D;
	private BoxCollider2D m_boxCollider2D;

	private float m_rayDistance;


	bool m_isLightAttack;
	bool m_isHeavyAttack;


	KnifeController knifeController;
	DoorState m_doorState = DoorState.None;
	//bool isDoorExist = false;
	// Use this for initialization
	void Start ()
	{
		m_rigidbody2D = transform.GetComponent<Rigidbody2D> ();
		m_boxCollider2D = transform.GetComponent<BoxCollider2D> ();

		m_rayDistance = m_boxCollider2D.bounds.extents.y + 0.1f;

		m_state = WeaponState.None;

		knifeController = GameObject.Find ("Knife").GetComponent<KnifeController> ();

	}

	// Update is called once per frame
	void Update ()
	{
		if (InputService.Instance ().GetInput (KeyMap.Knife) == true) {

			if (knifeController.GetIsStaying () && knifeController.GetIsPowered ()) {
				transform.position = knifeController.transform.position;

				m_rigidbody2D.simulated = false;

				StartCoroutine (IsStayWithKnife (knifeController.m_stayTime));
			} else if (m_rigidbody2D.simulated == false) {
				m_rigidbody2D.simulated = true;
			} else {

				m_state = WeaponState.Knife;

			}

		}


		if (InputService.Instance ().GetInput (KeyMap.Door) == true) {
			m_state = WeaponState.SetDoor;
		}

		//Whether enter a door




		if (Input.GetKeyDown (KeyCode.Mouse0)) {
			if (m_state == WeaponState.Knife && m_rigidbody2D.simulated == true) {
				Debug.Log ("Shoot knife");

				Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

				Vector2 dir = new Vector2 (mousePos.x - transform.position.x, mousePos.y - transform.position.y);
				dir.Normalize ();
				knifeController.Shoot (transform.position, dir);
			}

			//if (m_state == WeaponState.Knife && m_rigidbody2D.simulated == false) {
			//	Collider2D [] others = Physics2D.OverlapCircleAll (this.transform.position, m_effectRange, m_enemyLayerMask);   //获取指定范围所有碰撞体
			//	foreach (var other in others) {
			//		if (other.name == "Enemy") {
			//			transform.position = other.transform.position;

			//			Destroy (other.gameObject);

			//		}

			//	}
			//}


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

	IEnumerator IsStayWithKnife (float time)
	{
		while (time >= 0) {
			time -= Time.fixedDeltaTime;
			yield return null;
		}

		m_rigidbody2D.simulated = true;
	}

	void FixedUpdate ()
	{
		//if (m_isLightAttack || m_isHeavyAttack) return;

		float horizontalAxis = InputService.Instance ().GetAxis (KeyMap.Horizontal);
		float verticalAxis = InputService.Instance ().GetAxis (KeyMap.Vertical);
		if (horizontalAxis > 0.1f || horizontalAxis < -0.1f) {
			//Debug.Log (InputService.Instance ().GetAxis (KeyMap.Horizontal));
			m_rigidbody2D.velocity = new Vector2 (horizontalAxis * m_moveSpeed, m_rigidbody2D.velocity.y);
			//transform.localPosition = new Vector3 (
			//transform.localPosition.x + horizontalAxis * m_moveSpeed * Time.fixedDeltaTime,
			//transform.localPosition.y,
			//transform.localPosition.z);

		}
		if (verticalAxis > 0.1f) {
			//Debug.Log (InputService.Instance ().GetAxis (KeyMap.Horizontal));


			RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, m_rayDistance, m_layerMask);

			if (hit.collider != null) {
				//Debug.Log ("Jump!");

				m_rigidbody2D.AddForce (new Vector2 (0, m_jumpForce));
			}
		}
		if (verticalAxis < -0.1f && m_rigidbody2D.simulated == false) {
			m_rigidbody2D.simulated = true;
			knifeController.Disappear ();
		}
		if (knifeController.GetIsPowered () == false &&
		knifeController.GetIsFlying () == false &&
			knifeController.GetIsStaying () == false &&
		m_rigidbody2D.simulated == true) {

			RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, m_rayDistance, m_layerMask);

			if (hit.collider != null) {
				//Debug.Log ("Jump!");

				knifeController.Recharge ();
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
