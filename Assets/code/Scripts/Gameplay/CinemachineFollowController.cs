using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineFollowController : MonoBehaviour
{

    private void Awake()
    {
        m_confiner = transform.GetComponent<CinemachineConfiner>();
        CinemachineManager.Instance.SetFollowCtrl(this);
        LoadCameraRange();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetBoundingVolume(Collider collider)
    {
        if (m_confiner != null)
        {
            m_confiner.m_BoundingVolume = collider;
        }
    }

    public void SetBoundingVolumeByName(string name)
    {
        if (m_confiner != null)
        {
            bool result = m_moveRangeDic.TryGetValue(name, out Collider collider);
            if (result)
            {
                m_confiner.m_BoundingVolume = collider;
            }
        }
    }

    private void LoadCameraRange()
    {
        string[] sceneList = SceneLookup.GetAllSceneString();
        string scene = "";
        GameObject cameraRange;
        Collider collider;
        for (int i = 0; i < sceneList.Length; i++)
        {
            scene = sceneList[i];
            cameraRange = GameObject.Find(scene);
            if (cameraRange != null)
            {
                collider = cameraRange.GetComponent<Collider>();
                m_moveRangeDic.Add(scene, collider);
            }
            else
            {
                m_moveRangeDic.Add(scene, null);
            }
        }
    }

    private CinemachineConfiner m_confiner = null;
    private Dictionary<string, Collider> m_moveRangeDic = new Dictionary<string, Collider>();
}
