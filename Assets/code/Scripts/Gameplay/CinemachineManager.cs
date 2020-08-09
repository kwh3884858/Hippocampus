using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Base;

public class CinemachineManager : Singleton<CinemachineManager>
{

    public void SetFollowCtrl(CinemachineFollowController followController)
    {
        m_followController = followController;
    }

    public void SetBoundingVolume(Collider collider)
    {
        if (m_followController != null)
        {
            m_followController.SetBoundingVolume(collider);
        }
    }
    public void SetBoundingVolumeByName(string name)
    {
        if (m_followController != null)
        {
            m_followController.SetBoundingVolumeByName(name);
        }
    }

    private CinemachineFollowController m_followController = null;
}
