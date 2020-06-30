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

public class MonoMoveController : MonoBehaviour
{

    [Header("Public, Physics Property")]
    public float m_moveSpeed = 5f;
    public float m_jumpForce = 60f;
    public float m_rayDistance = 2f;

    [Header("Private, Physics Data")]
    [SerializeField]
    private bool m_isOldFaceRight = false;

    [SerializeField]
    private bool m_isFaceRight = false;

    //private Rigidbody2D m_rigidbody2D;
    [SerializeField]
    private BoxCollider2D m_boxCollider2D;


    [Header("Public, Interactive Property")]
    public float m_showInteractiveUIRadius = 1.0f;

    public float m_interactableRadius = 0.5f;

    public float m_interactableRaycastAngle = 90;

    public float m_interactableRaycastAngleInterval = 10;

    //   public LayerMask m_layerMask;
    //   public LayerMask m_enemyLayerMask;

    private bool m_isMove = false;
    private int count = 0;// 测试计数 Delete in future

    void Start()
    {
        //m_rigidbody2D = transform.GetComponent<Rigidbody2D> ();
        m_boxCollider2D = transform.GetComponent<BoxCollider2D>();
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

    }

    private void OnGUI()
    {
        if ((GUI.Button(new Rect(0, 50, 200, 50), "Interact")))
        {
            RaycastHit hit;
            bool result = Physics.Raycast(transform.position, m_isFaceRight ? Vector3.right : Vector3.left, out hit, Mathf.Infinity);
            if (result)
            {
                if (hit.collider.CompareTag(InteractiveObject.INTERACTABLE_TAG))
                {
                    Debug.Log("Did Interactive");
                    // TODO:接触可交互物体，触发对话
                    count++;
                    //UI.UIManager.Instance ().ShowPanel (UIPanelType.TalkPanel, new TalkDataProvider () { ID = count.ToString () });

                    hit.collider.GetComponent<InteractiveObject>().Interact();
                }
                Debug.DrawRay(transform.position, m_isFaceRight ? Vector3.right : Vector3.left * hit.distance, Color.yellow);
                //Debug.Log("Did Hit");
            }
            else
            {
                Debug.DrawRay(transform.position, m_isFaceRight ? Vector3.right : Vector3.left * 1000, Color.white);

            }
        }
        if (GUI.Button(new Rect(0, 100, 200, 50), "Evidence"))
        {
            UIManager.Instance().ShowPanel(UIPanelType.Evidencepanel);// 显示UI
        }

        if (GUI.Button(new Rect(0, 150, 200, 50), "Tips"))
        {
            UIManager.Instance().ShowStaticPanel(UIPanelType.Tipspanel);// 显示UI
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
        if (horizontalAxis > 0.1f || horizontalAxis < -0.1f)
        {
            transform.localPosition = new Vector3(
            transform.localPosition.x + horizontalAxis * m_moveSpeed * Time.fixedDeltaTime,
            transform.localPosition.y,
            transform.localPosition.z);
        }
        if (verticalAxis > 0.1f || verticalAxis < -0.1f)
        {
            transform.localPosition = new Vector3(
            transform.localPosition.x,
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
            transform.localScale = new Vector3(
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
