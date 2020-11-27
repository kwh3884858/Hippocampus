﻿using System.Collections;
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
        m_capsuleCollider = transform.GetComponent<CapsuleCollider>();
        if (m_capsuleCollider == null)
        {
            Debug.LogError("Player Collision Is Lost.");
        }
        m_animator = transform.GetComponent<Animator>();
        if (m_animator == null)
        {
            Debug.LogError("Player Animator Is Lost.");
        }
        else
        {
            //m_animator.SetBool ("IsFaceLeft", !m_isFaceRight);
        }
        m_spriteRender = transform.GetComponent<SpriteRenderer>();
        if (m_spriteRender == null)
        {
            Debug.LogWarning("Player Sprite Render Is Lost");
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
            //UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonESCMainMenuPanel);// 显示UI
            StarPlatinum.EventManager.EventManager.Instance.SendEvent(new StarPlatinum.SettingStateEvent() { IsShow = true });
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonSettingPanel);// 显示UI
        }

        int maxColliders = 10;
        Collider[] hitColliders = new Collider[maxColliders];
        Dictionary<Collider, float> colliders = new Dictionary<Collider, float>();

        Collider interactCollider = null;
        float smallestLength = 10000;

        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, m_interactableRadius, hitColliders, m_interactableLayer.value);
        //Debug.Log ("Num of Collisions: " + numColliders);
        for (int i = 0; i < numColliders; i++)
        {
            if (hitColliders[i].CompareTag(InteractiveObject.INTERACTABLE_TAG))
            {
                colliders.Add(hitColliders[i], Vector3.SqrMagnitude(hitColliders[i].transform.position - transform.position));
            }
        }
        foreach (var pair in colliders)
        {
            if (pair.Value < smallestLength)
            {
                smallestLength = pair.Value;
                interactCollider = pair.Key;
            }
        }

        if (interactCollider != null)
        {
            if (!UIManager.Instance().IsPanelShow(UIPanelType.UIGameplayPromptwidgetPanel))
            {
                UIManager.Instance().ShowPanel(UIPanelType.UIGameplayPromptwidgetPanel, new PromptWidgetDataProvider { m_interactiableObject = interactCollider.gameObject });
                UIManager.Instance().UpdateData(UIPanelType.UICommonGameplayPanel, new CommonGamePlayDataProvider { m_interactButtonShouldVisiable = true });
            }
            if (!UIManager.Instance().IsPanelShow(UIPanelType.TalkPanel))
            {
                if (Input.GetKeyDown(KeyCode.Return) || m_isInteractByUI)
                {
                    m_isInteractByUI = false;
                    Debug.Log("Did Interactive: " + interactCollider.gameObject);
                    interactCollider.GetComponent<InteractiveObject>().Interact();

                    Debug.DrawRay(interactCollider.transform.position, Vector3.up * 3, Color.yellow, 5.0f);
                }
            }
        }
        else
        {
            m_isInteractByUI = false;
            if (UIManager.Instance().IsPanelShow(UIPanelType.UIGameplayPromptwidgetPanel))
            {
                UIManager.Instance().HidePanel(UIPanelType.UIGameplayPromptwidgetPanel);
                UIManager.Instance().UpdateData(UIPanelType.UICommonGameplayPanel, new CommonGamePlayDataProvider { m_interactButtonShouldVisiable = false });
            }
        }
    }

    private void OnGUI()
    {

        //if (GUI.Button(new Rect(0, 100, 200, 50), "Evidence"))
        //{
        //    //UIManager.Instance().ShowPanel(UIPanelType.Evidencepanel);// 显示UI
        //    UIManager.Instance().ShowStaticPanel(UIPanelType.Evidencepanel);// 显示UI
        //}

        //if (GUI.Button(new Rect(0, 150, 200, 50), "Tips"))
        //{
        //    UIManager.Instance().ShowStaticPanel(UIPanelType.Tipspanel);// 显示UI
        //}

        //if (GUI.Button(new Rect(0, 200, 200, 50), "Save"))
        //{
        //    UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonLoadarchivePanel,new ArchiveDataProvider(){Type = ArchivePanelType.Save});// 显示UI
        //}
        //if (GUI.Button(new Rect(0, 250, 200, 50), "ShowCG"))
        //{
        //    UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonCgscenePanel,new CGSceneDataProvider(){CGSceneID = "EP04-01"});// 显示UI
        //}
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
            horizontalStepLength = horizontalAxis * m_moveSpeed;
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
        //m_animator.SetFloat ("Speed", horizontalStepLength * horizontalStepLength + verticalStepLength * verticalStepLength);

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
            transform.localScale = new Vector3(m_isFaceRight ? 1 : -1, transform.localScale.y, transform.localScale.z);
            //m_spriteRender.flipX = m_isFaceRight;
            m_isOldFaceRight = m_isFaceRight;
        }
    }

    public void SetMoveEnable(bool isEnable)
    {
        m_isMove = isEnable;
    }

    public void StopPlayerAnimation()
    {
        m_animator.SetFloat("Speed", 0.0f);
    }

    public void SetCharacterSpeed(float speedArgument)
    {
        m_moveSpeed = speedArgument;
    }

    public void SetInteract()
    {
        m_isInteractByUI = true;
    }

    [Header("Public, Physics Property")]
    public float m_moveSpeed = 5f;
    public float m_jumpForce = 60f;
    public float m_rayDistance = 2f;


    [Header("Public, Interactive Property")]
    public float m_showInteractiveUIRadius = 1.0f;
    public float m_interactableRadius = 0.5f;
    public float m_interactableRaycastAngle = 90;
    public float m_interactableRaycastAngleInterval = 10;
    public LayerMask m_interactableLayer;


    [Header("Private, Physics Data")]
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
    private CapsuleCollider m_capsuleCollider;


    public bool m_isMove = true;
    private bool m_isInteractByUI = false;

    private int count = 0;// 测试计数 Delete in future

}
