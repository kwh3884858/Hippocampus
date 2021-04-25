using GamePlay.Stage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastDetection : MonoBehaviour
{


	void Update ()
	{
        if (!m_openDetection)
        {
			return;
        }


	}

	//是否开启射线检测
	public void SwitchDetection (bool isOpenDetection)
	{
		m_openDetection = isOpenDetection;
	}



	public bool m_openDetection = true;


}
