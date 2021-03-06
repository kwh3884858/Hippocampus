﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using StarPlatinum;

using UI.Panels.Providers.DataProviders.StaticBoard;

using GamePlay.Player;
using StarPlatinum.Services;
using GamePlay;
using UI.Panels.GameScene.MainManu;
using UI;
using UI.Panels;
using UI.Panels.Providers.DataProviders;
using StarPlatinum.EventManager;

public class MonoMoveController : MonoBehaviour
{
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
    public void InteractWith(Collider collider)
    {
        InteractWithCollider(collider);
    }

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

        m_findColliderDetection = new FindColliderDetection();
    }
    // Update is called once per frame
    void Update()
    {
        if (!m_isEnable)
        {
            return;
        }

        if (InputService.Instance.Input.PlayerControls.TogglePause.triggered)
        {
            //UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonESCMainMenuPanel);// 显示UI
            StarPlatinum.EventManager.EventManager.Instance.SendEvent(new StarPlatinum.SettingStateEvent() { IsShow = true });
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonSettingPanel);// 显示UI
        }

        switch (CameraService.Instance.GetSceneCameraType())
        {
            case CameraService.SceneCameraType.ThirdPerson:
                {
                    Collider interactCollider = m_findColliderDetection.OverlapSphereDetectionFindNearestCollider(transform, m_interactableRadius, m_interactableMask);
                    InteractWithCollider(interactCollider);
                }
                break;

            case CameraService.SceneCameraType.FirstPerson:
                {
                    //TODO
                }
                break;
        }
     
    }



    private void InteractWithCollider(Collider collider)
    {
        if (collider != null)
        {
            // Show Interaction Button
            if (!UIManager.Instance().IsPanelShow(UIPanelType.UIGameplayPromptwidgetPanel))
            {
                UIManager.Instance().ShowPanel(UIPanelType.UIGameplayPromptwidgetPanel, new PromptWidgetDataProvider { m_interactiableObject = collider.gameObject });

                InteractiveObject interativeObject = collider.GetComponent<InteractiveObject>();
                string itemName = Error_Interactive_Object_Name;
                if (interativeObject)
                {
                    itemName = interativeObject.GetUIDisplayName();
                    if (itemName == "")
                    {
                        itemName = Error_Interactive_Object_Name;
                    }
                }
                EventManager.Instance.SendEvent(new CommonGamePlayPanelUpdateDataEvent { m_interactButtonShouldVisiable = true, m_itemName = itemName }) ;
            }

            // Do Interaction
            if (!UIManager.Instance().IsPanelShow(UIPanelType.UICommonTalkPanel))
            {
                if (InputService.Instance.Input.PlayerControls.Interact.triggered || m_isInteractByUI )
                {
                    m_isInteractByUI = false;
                    Debug.Log("Did Interactive: " + collider.gameObject);
                    collider.GetComponent<InteractiveObject>().Interact();

                    Debug.DrawRay(collider.transform.position, Vector3.up * 3, Color.yellow, 5.0f);
                }
            }
        }
        else
        {
            if (m_isInteractByUI)
            {
                if (UIManager.Instance().IsPanelShow(UIPanelType.UIGameplayPromptwidgetPanel))
                {
                    UIManager.Instance().HidePanel(UIPanelType.UIGameplayPromptwidgetPanel);
                }
                // Common gameplay may need to show others icon, which is only should make interact button invisiable. 
                if (UIManager.Instance().IsPanelShow(UIPanelType.UICommonGameplayPanel))
                {
                    EventManager.Instance.SendEvent(new CommonGamePlayPanelUpdateDataEvent { m_interactButtonShouldVisiable = false});
                }
            }
            m_isInteractByUI = false;
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
        if (!m_isEnable)
        {
            return;
        }

        if (!m_isMove)// 不能进行移动
        {
            return;
        }

        Vector2 moveAxis = StarPlatinum.Services.InputService.Instance.Input.PlayerControls.Move.ReadValue<Vector2>();
        float horizontalStepLength = 0;
        float verticalStepLength = 0;

        if (moveAxis.x > 0.1f || moveAxis.x < -0.1f)
        {
            horizontalStepLength = moveAxis.x * m_moveSpeed;
            transform.localPosition = new Vector3(
            transform.localPosition.x + horizontalStepLength * Time.fixedDeltaTime,
            transform.localPosition.y,
            transform.localPosition.z);
        }
        if (moveAxis.y > 0.1f || moveAxis.y < -0.1f)
        {
            verticalStepLength = moveAxis.y * m_moveSpeed;
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


        if (moveAxis.x < -0.1f)
        {
            m_isFaceRight = false;
        }
        if (moveAxis.y > 0.1f)
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

    private FindColliderDetection m_findColliderDetection;

    private const string Error_Interactive_Object_Name = "!!!No Item Name!!!";
    private const float Default_Max_Distance = 10000;

    [Header("Third Person Controller")]
    public bool m_isEnable = true;

    [Header("Public, Third Person Physics Property")]
    public float m_moveSpeed = 5f;
    public float m_jumpForce = 60f;
    public float m_rayDistance = 2f;

    [Header("Public, Third Person Interactive Property")]
    public float m_interactableRadius = 0.5f;
    public float m_showInteractiveUIRadius = 1.0f;
    public float m_interactableRaycastAngle = 90;
    public float m_interactableRaycastAngleInterval = 10;
    public LayerMask m_interactableMask;

    [Header("Public, First Person Interactive Property")]
    public float raycastLength = 2.0f;
    public LayerMask m_raycastMask;

    [Header("Private, Third Person Physics Data")]
    [SerializeField]
    private bool m_isOldFaceRight = false;
    [SerializeField]
    private bool m_isFaceRight = false;
    [SerializeField]
    private Animator m_animator;
    [SerializeField]
    private SpriteRenderer m_spriteRender;
    [SerializeField]
    private CapsuleCollider m_capsuleCollider;

    public bool m_isMove = true;
    private bool m_isInteractByUI = false;

    private int count = 0;// 测试计数 Delete in future

}
