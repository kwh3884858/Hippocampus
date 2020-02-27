using System.Collections;
using System.Collections.Generic;
using GamePlay;
using UnityEngine;
using StarPlatinum.Service;

public class WorldTrigger : MonoBehaviour
{
	public delegate void WorldCallback ();

	private WorldCallback m_callback;
	public WorldCallback Callback { get => m_callback; set => m_callback = value; }



	private void Start ()
	{
		Callback = DefacultCallback;
		m_boxCollider = GetComponent<BoxCollider> ();
		if (m_boxCollider == null) {
			m_boxCollider = gameObject.AddComponent<BoxCollider> ();
		}
	}

	//private void OnDrawGizmos ()
	//{
	//	Gizmos.DrawWireSphere (m_boxCollider.center, 1);
	//}

	private void OnTriggerEnter (Collider collision)
	{
		Debug.Log ("Trigger");

		Debug.DrawLine (transform.position, Vector3.up, Color.red, 4.0f);
		//Debug.DrawLine (collision.point, collision.normal, Color.white);
		m_callback?.Invoke ();

		//foreach (ContactPoint contact in collision.contacts) {
		//	Gizmos.DrawWireSphere (contact.point, 1);
		//	//Debug.DrawLine (contact.point, contact.normal, Color.white);
		//}
	}

	private void DefacultCallback ()
	{
		Transform cameraTransform = CameraService.Instance.GetMainCamera ().transform;
		CameraMoveController.Instance.SetMoveTransform (cameraTransform);
		CameraMoveController.Instance.Move (cameraTransform.position + new Vector3 (5, 0, 0), 3, 1);
	}

	private BoxCollider m_boxCollider;
}
