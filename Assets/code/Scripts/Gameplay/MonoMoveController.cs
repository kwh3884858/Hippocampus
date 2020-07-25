using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using StarPlatinum;

using UI.Panels.Providers.DataProviders.StaticBoard;

using GamePlay.Player;
using StarPlatinum.Service;
using GamePlay;
using UI.Panels.GameScene.MainManu;
using UI;
using UI.Panels;
using UI.Panels.Providers.DataProviders;

public class MonoMoveController : MonoBehaviour
{

    //   public LayerMask m_layerMask;
    //   public LayerMask m_enemyLayerMask;


    void Start()
    {
        //m_rigidbody2D = transform.GetComponent<Rigidbody2D> ();
        m_boxCollider2D = transform.GetComponent<BoxCollider2D>();
		if (m_boxCollider2D == null) {
            Debug.LogError ("Player Collision Is Lost.");
		}
        m_animator = transform.GetComponent<Animator> ();
		if (m_animator == null) {
            Debug.LogError ("Player Animator Is Lost.");
		} else {
            m_animator.SetBool ("IsFaceLeft", !m_isFaceRight);
		}
        m_spriteRender = transform.GetComponent<SpriteRenderer> ();
		if (m_spriteRender == null) {
            Debug.LogError ("Player Sprite Render Is Lost");
		}
        PlayerController.Instance().SetMonoMoveController(this);
        CameraService.Instance.SetTarget(gameObject);
        count = 0;

    }
    // Update is called once per frame
    void Update()
    {
        //// tips库测试
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    // 显示tips库
        //UIManager.Instance().ShowPanel(UIPanelType.Tipspanel);// 显示UI
        //UIManager.Instance().ShowStaticPanel(UIPanelType.Tipspanel);// 显示UI
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Tips.TipsManager.Instance.UnlockTip("Assassin's Creed ", Tips.TipsManager.ConvertDateTimeToLong(System.DateTime.Now));// 添加tip 数据
        //}
        //if(Input.GetKeyDown(KeyCode.W))
        //{
        //    var data = new Tips.TipData("测试", "www");
        //    UIManager.Instance().ShowPanel(UIPanelType.Tipgetpanel, new UI.Panels.Providers.DataProviders.TipDataProvider() { Data = data });// 显示UI
        //}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonESCMainMenuPanel);// 显示UI
        }
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 50, 200, 50), "Interact"))
        {

            int maxColliders = 10;
            Collider [] hitColliders = new Collider [maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc (transform.position, m_interactableRadius , hitColliders);

            for (int i = 0; i < numColliders; i++) {
				if (hitColliders[i].CompareTag(InteractiveObject.INTERACTABLE_TAG)) {
                    Collider hitCollider = hitColliders [i];
                    Debug.Log ("Did Interactive: " + hitCollider.gameObject);
                    hitCollider.GetComponent<InteractiveObject> ().Interact ();

                    Debug.DrawRay (hitCollider.transform.position, Vector3.up * 3, Color.yellow);
                    break;
                }
            }
            
            //RaycastHit hit;
            //if (result)
            //{
            //    if (hit.collider.CompareTag(InteractiveObject.INTERACTABLE_TAG))
            //    {
            //        Debug.Log("Did Interactive");
            //        // TODO:接触可交互物体，触发对话
            //        count++;
            //        //UI.UIManager.Instance ().ShowPanel (UIPanelType.TalkPanel, new TalkDataProvider () { ID = count.ToString () });

            //        hit.collider.GetComponent<InteractiveObject>().Interact();
            //    }
            //    Debug.DrawRay(transform.position, m_isFaceRight ? Vector3.right : Vector3.left * hit.distance, Color.yellow);
            //    //Debug.Log("Did Hit");
            //}
            //else
            //{
            //    Debug.DrawRay(transform.position, m_isFaceRight ? Vector3.right : Vector3.left * 1000, Color.white);

            //}
        }
        if (GUI.Button(new Rect(0, 100, 200, 50), "Evidence"))
        {
            //UIManager.Instance().ShowPanel(UIPanelType.Evidencepanel);// 显示UI
            UIManager.Instance().ShowStaticPanel(UIPanelType.Evidencepanel);// 显示UI
        }

        if (GUI.Button(new Rect(0, 150, 200, 50), "Tips"))
        {
            UIManager.Instance().ShowStaticPanel(UIPanelType.Tipspanel);// 显示UI
        }
        
        if (GUI.Button(new Rect(0, 200, 200, 50), "Save"))
        {
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonLoadarchivePanel,new ArchiveDataProvider(){Type = ArchivePanelType.Save});// 显示UI
        }

    }

    void FixedUpdate()
    {
        if (!m_isMove)// 不能进行移动
        {
            return;
        }
        float horizontalAxis = StarPlatinum.InputService.Instance.GetAxis(StarPlatinum.KeyMap.Horizontal);
        float verticalAxis = StarPlatinum.InputService.Instance.GetAxis(StarPlatinum.KeyMap.Vertical);
        float horizontalStepLength = 0;
        float verticalStepLength = 0;

        if (horizontalAxis > 0.1f || horizontalAxis < -0.1f)
        {
            horizontalStepLength = horizontalAxis * m_moveSpeed ;
            transform.localPosition = new Vector3(
            transform.localPosition.x + horizontalStepLength * Time.fixedDeltaTime,
            transform.localPosition.y,
            transform.localPosition.z);
        }
        if (verticalAxis > 0.1f || verticalAxis < -0.1f)
        {
            verticalStepLength = verticalAxis * m_moveSpeed;
            transform.localPosition = new Vector3(
            transform.localPosition.x,
            transform.localPosition.y,
            transform.localPosition.z + verticalStepLength * Time.fixedDeltaTime);
        }
        m_animator.SetFloat ("Speed", horizontalStepLength * horizontalStepLength + verticalStepLength * verticalStepLength);

        //No Jump
        //if (verticalAxis > 0.1f) {
        //	//Debug.Log (InputService.Instance ().GetAxis (KeyMap.Horizontal));


        //	RaycastHit2D hit = Physics2D.Raycast (transform.position, Vector2.down, m_rayDistance, m_layerMask);

        //	if (hit.collider != null) {
        //		//Debug.Log ("Jump!");

        //		m_rigidbody2D.AddForce (new Vector2 (0, m_jumpForce));
        //	}
        //}


        if (horizontalAxis < -0.1f)
        {
            m_isFaceRight = false;
        }
        if (horizontalAxis > 0.1f)
        {
            m_isFaceRight = true;
        }

        if (m_isFaceRight != m_isOldFaceRight)
        {
            m_spriteRender.flipX = m_isFaceRight;
            m_isOldFaceRight = m_isFaceRight;
        }
        Debug.Log ("IsFaceLeft: " + m_animator.GetBool ("IsFaceLeft"));
    }

    public void SetMoveEnable(bool isEnable)
    {
        m_isMove = isEnable;
    }

    [Header ("Public, Physics Property")]
    public float m_moveSpeed = 5f;
    public float m_jumpForce = 60f;
    public float m_rayDistance = 2f;


    [Header ("Public, Interactive Property")]
    public float m_showInteractiveUIRadius = 1.0f;
    public float m_interactableRadius = 0.5f;
    public float m_interactableRaycastAngle = 90;
    public float m_interactableRaycastAngleInterval = 10;


    [Header ("Private, Physics Data")]
    [SerializeField]
    private bool m_isOldFaceRight = false;
    [SerializeField]
    private bool m_isFaceRight = false;
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private SpriteRenderer m_spriteRender;
    //private Rigidbody2D m_rigidbody2D;
    [SerializeField]
    private BoxCollider2D m_boxCollider2D;

    private bool m_isMove = false;
    private int count = 0;// 测试计数 Delete in future

}
