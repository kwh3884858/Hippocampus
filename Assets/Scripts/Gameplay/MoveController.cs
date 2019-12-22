using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum;
public class MoveController : MonoBehaviour
{


	public float m_moveSpeed = 5f;
	public float m_jumpForce = 60f;



	private bool m_isOldFaceRight = false;
	private bool m_isFaceRight = false;

	//private Rigidbody2D m_rigidbody2D;
	//private BoxCollider2D m_boxCollider2D;
 //   public LayerMask m_layerMask;
 //   public LayerMask m_enemyLayerMask;

    void Start ()
	{
        //m_rigidbody2D = transform.GetComponent<Rigidbody2D> ();
        //m_boxCollider2D = transform.GetComponent<BoxCollider2D> ();

        CameraService.Instance.SetTarget(gameObject);
	}

	// Update is called once per frame
	void Update ()
	{
	


	}

	void FixedUpdate ()
	{
		//if (m_isLightAttack || m_isHeavyAttack) return;

		float horizontalAxis = InputService.Instance .GetAxis (KeyMap.Horizontal);
		float verticalAxis = InputService.Instance .GetAxis (KeyMap.Vertical);
		if (horizontalAxis > 0.1f || horizontalAxis < -0.1f) {

            //m_rigidbody2D.velocity = new Vector2 (horizontalAxis * m_moveSpeed, m_rigidbody2D.velocity.y);

            transform.localPosition = new Vector3(
            transform.localPosition.x + horizontalAxis * m_moveSpeed * Time.fixedDeltaTime,
            transform.localPosition.y,
            transform.localPosition.z);

        }

        //No Jump
        //if (verticalAxis > 0.1f) {
        //	//Debug.Log (InputService.Instance ().GetAxis (KeyMap.Horizontal));


        //	RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, m_rayDistance, m_layerMask);

        //	if (hit.collider != null) {
        //		//Debug.Log ("Jump!");

        //		m_rigidbody2D.AddForce (new Vector2 (0, m_jumpForce));
        //	}
        //}


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
