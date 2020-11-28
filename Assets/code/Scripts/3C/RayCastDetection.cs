using GamePlay.Stage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastDetection : MonoBehaviour
{
	public LayerMask m_interactableMask;
	public LayerMask m_collideableMask;

	public float m_maxDistance = 2;

	private string m_oldName;
	private bool m_openDetection = true;

	void Update ()
	{
        if (!m_openDetection)
        {
			return;
        }

		// 以摄像机所在位置为起点，创建一条向下发射的射线  
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, m_maxDistance, m_collideableMask.value)) {

            if (hit.collider)
            {
				if (hit.collider.gameObject)
				{
					if (((1 << hit.collider.gameObject.layer) | m_interactableMask) != 0)
                    {
                        CoreContainer.Instance.InteractWith(hit.collider);
                    }
                }
            }

            // 如果射线与平面碰撞，打印碰撞物体信息  
            Debug.Log("碰撞对象: " + hit.transform.parent.name);
            Debug.Log("碰撞距离：" + hit.distance);
            // 在场景视图中绘制射线  
            Debug.DrawLine (ray.origin, hit.point, Color.red);
		} else {
			CoreContainer.Instance.InteractWith(null);
		}
	}

	//是否开启射线检测
	public void SwitchDetection (bool isOpenDetection)
	{
		m_openDetection = isOpenDetection;
	}

}
