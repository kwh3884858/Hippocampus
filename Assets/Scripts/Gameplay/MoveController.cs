using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum;
using UI.Panels.Providers;
using Player;
using UI.Panels.Providers.DataProviders.StaticBoard;

public class MoveController : MonoBehaviour
{


	public float m_moveSpeed = 5f;
	public float m_jumpForce = 60f;
    public float m_rayDistance = 2f;

    [SerializeField]
    private bool m_isOldFaceRight = false;

    [SerializeField]
    private bool m_isFaceRight = false;

	//private Rigidbody2D m_rigidbody2D;
    [SerializeField]
	private BoxCollider2D m_boxCollider2D;
    //   public LayerMask m_layerMask;
    //   public LayerMask m_enemyLayerMask;

    private bool m_isMove = false;

    void Start ()
	{
        //m_rigidbody2D = transform.GetComponent<Rigidbody2D> ();
        m_boxCollider2D = transform.GetComponent<BoxCollider2D> ();
        PlayerController.Instance().m_moveCtrl = this;
        CameraService.Instance.SetTarget(gameObject);
	}

    private void OnDestroy()
    {
        if(PlayerController.Instance().m_moveCtrl == this)
        {
            PlayerController.Instance().m_moveCtrl = null;
        }
    }

	// Update is called once per frame
	void Update ()
	{
	


	}

    private void OnGUI()
    {
        if((GUI.Button(new Rect(0, 50, 200, 50), "Interact"))){
            RaycastHit hit;
            bool result = Physics.Raycast (transform.position, m_isFaceRight?Vector3.right:Vector3.left, out hit,Mathf.Infinity);
            if (result)
            {
                if(hit.collider.CompareTag("Interactable"))
                {
                    Debug.Log("Did Interactable");
                    // TODO:接触可交互物体，触发对话
                    UI.UIManager.Instance().ShowPanel(UIPanelType.TalkPanel,new TalkDataProvider(){ID = "1"});
                }
                Debug.DrawRay(transform.position, m_isFaceRight ? Vector3.right : Vector3.left * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, m_isFaceRight ? Vector3.right : Vector3.left * 1000, Color.white);

            }
        }
    }

    void FixedUpdate ()
	{
		//if (m_isLightAttack || m_isHeavyAttack) return;
        if(!m_isMove)// 不能进行移动
        {
            return;
        }
		float horizontalAxis = InputService.Instance .GetAxis (KeyMap.Horizontal);
		float verticalAxis = InputService.Instance .GetAxis (KeyMap.Vertical);
		if (horizontalAxis > 0.1f || horizontalAxis < -0.1f) {

            //m_rigidbody2D.velocity = new Vector2 (horizontalAxis * m_moveSpeed, m_rigidbody2D.velocity.y);

            transform.localPosition = new Vector3(
            transform.localPosition.x + horizontalAxis * m_moveSpeed * Time.fixedDeltaTime,
            transform.localPosition.y,
            transform.localPosition.z);

        }
        if (verticalAxis > 0.1f || verticalAxis < -0.1f)
        {

            //m_rigidbody2D.velocity = new Vector2 (horizontalAxis * m_moveSpeed, m_rigidbody2D.velocity.y);

            transform.localPosition = new Vector3(
            transform.localPosition.x ,
            transform.localPosition.y,
            transform.localPosition.z + verticalAxis * m_moveSpeed * Time.fixedDeltaTime);

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

    public void SetMoveEnable(bool isEnable)
    {
        m_isMove = isEnable;
    }
}
