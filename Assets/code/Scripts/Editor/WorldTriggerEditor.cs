using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GamePlay;

namespace GamePlay
{
	// draw a red circle around the scene cube

	[CustomEditor (typeof (WorldTrigger))]
	public class WorldTriggerEditor : Editor
	{
		void OnSceneGUI ()
		{
			WorldTrigger moveController = (WorldTrigger)target;
			Collider collider = moveController.GetComponent<Collider> ();
			if (collider == null) {
				return;
			}
			//Gizmos.DrawCube (collider.bounds.center, collider.bounds.size);

			//Handles.color = Color.red;
			//Handles.DrawWireDisc (moveController.transform.position, new Vector3 (0, 1, 0), moveController.m_showInteractiveUIRadius);
			//Handles.color = Color.green;
			//Handles.DrawWireDisc (moveController.transform.position, new Vector3 (0, 1, 0), moveController.m_interactableRadius);

		}
	}
}

