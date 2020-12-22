using System.Collections;
using System.Collections.Generic;
using Assets.code.StarPlatinum.Services.CameraService;
using StarPlatinum.Base;
using UnityEngine;

public class VMCameraManager : MonoSingleton<VMCameraManager>
{
    public GameObject m_ActivatedVMCamera;

    private BaseCamera m_VMCameraController;

    public override void SingletonInit()
    {
        if (m_ActivatedVMCamera)
        {
            m_ActivatedVMCamera.SetActive(true);
            m_VMCameraController = m_ActivatedVMCamera.GetComponent<BaseCamera>();
        }
        else
        {
            Debug.LogError("VMCamera has not been set, please assign the value in VMCameraManager");
        }

    }

    public void SetLookAtRotation(float x, float y, float z)
    {
        m_VMCameraController.SetLookRotation(x,y,z);
    }

    public void SetCameraRotate(bool brotate)
    {
        m_VMCameraController.SetCameraRotation(brotate);
    }
}
