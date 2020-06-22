using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour
{

	public float m_xMargin = 1f;
	public float m_yMargin = 1f;
	public float m_xSmooth = 3f;
	public float m_ySmooth = 3f;
	public Vector2 m_maxXAndY = new Vector2 (1000, 1000);
	public Vector2 m_minXAndY = new Vector2 (-1000, -1000);
	public float m_yOffset = 0;

	[SerializeField]
	private Transform m_target;

	public void Refresh ()
	{
        //m_xMargin = xMargin;
        //m_yMargin = yMargin;

        //m_xSmooth = xSmooth;
        //m_ySmooth = ySmooth;

        //m_maxXAndY = new Vector2 (maxX, maxY);
        //m_minXAndY = new Vector2 (minX, minY);

        //m_yOffset = yOffset;

        if (m_target != null)
        {
            return;
        }

		GameObject player = GameObject.Find ("Hero");
		if (player == null) {
			m_target = null;
			return;

		}

		m_target = player.transform;
	}


	void Start ()
	{
		Refresh ();


	}

	//public  void SetMainCamera(GameObject cameraGameobject)
	//  {
	//      transform.SetParent(cameraGameobject.transform);
	//  }

	public void ChangeTarget (Transform nextTarget)
	{
		m_target = nextTarget;
	}

	public void BackToPlayer ()
	{
		m_target = GameObject.Find ("Hero").transform;
	}

	bool CheckXMargin ()
	{
		return Mathf.Abs (transform.position.x - m_target.position.x) > m_xMargin;
		//return true;
	}

	bool CheckYMargin ()
	{
		return Mathf.Abs (transform.position.y - m_target.position.y) > m_yMargin;

	}

	void FixedUpdate ()
	{
		if (m_target == null) return;
		//Debug.Log (player.transform.position);
		TrackPlayer ();
	}

	void TrackPlayer ()
	{
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		if (CheckXMargin ()) {
			targetX = Mathf.Lerp (transform.position.x, m_target.position.x, Time.deltaTime * m_xSmooth);
		}

		if (CheckYMargin ()) {
			targetY = Mathf.Lerp (transform.position.y, m_target.position.y + m_yOffset, Time.deltaTime * m_ySmooth);
		}

		targetX = Mathf.Clamp (targetX, m_minXAndY.x, m_maxXAndY.x);
		targetY = Mathf.Clamp (targetY, m_minXAndY.y, m_maxXAndY.y);

		transform.position = new Vector3 (targetX, targetY, transform.position.z);
	}

	internal void SetTarget (GameObject target)
    {
        if (target == null) return;
        if (m_target != null)
        {
            Debug.LogError("Camera Controller is not null, you will replace the original target");
        }
		m_target = target.transform;
	}
}