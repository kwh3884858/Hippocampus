using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisionToTriggerWallMask : MonoBehaviour
{

    private void Start()
    {

        m_activeWallLayer = LayerMask.NameToLayer("WallDissolved");
        m_inactiveWallLayer = LayerMask.NameToLayer("Wall");
    }

    private void OnTriggerEnter(Collider other)
    {
        //collision.gameObject.layer = m_activeWallLayer;
        if (m_inactiveWallLayer == other.gameObject.layer)
        {
            SetLayerRecursively(other.gameObject, m_activeWallLayer);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //collision.gameObject.layer = m_inactiveWallLayer;
        if (m_activeWallLayer == other.gameObject.layer)
        {
            SetLayerRecursively(other.gameObject, m_inactiveWallLayer);
        }
    }


    private void SetLayerRecursively(GameObject obj, LayerMask layer)
    {
        if (obj == null)
        {
            return;
        }
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, layer);
        }
    }


    [Header("private, Mask")]
    private LayerMask m_inactiveWallLayer;
    private LayerMask m_activeWallLayer;

}
