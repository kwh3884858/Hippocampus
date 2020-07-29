using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GamePlay.EventTrigger
{
	[CustomEditor (typeof (WorldTriggerCallbackTeleportPlayer))]
	public class WorldTriggerCallbackTeleportPlayerEditor : Editor
	{
		void OnSceneGUI ()
		{
			WorldTriggerCallbackTeleportPlayer teleport = (WorldTriggerCallbackTeleportPlayer)target;
			Vector3 pos = teleport.transform.position;
			Vector3 relativePos = teleport.m_lengthBetweenTriggerAndSpwanPoint * WorldTriggerCallbackTeleportPlayer.DirecitonMapping [teleport.m_spawnDirection];

			Handles.color = Color.red;
			Handles.DrawLine (pos, pos + relativePos);
			Handles.color = Color.green;
			Handles.DrawLine (pos + relativePos, pos + relativePos + Vector3.up);
		}

		override public void OnInspectorGUI ()
		{
			WorldTriggerCallbackTeleportPlayer trigger = (WorldTriggerCallbackTeleportPlayer)target;

			if (trigger.m_isCGScene == false) {
				GUI.backgroundColor = Color.green;
				m_sceneLookupEnum = (SceneLookupEnum)EditorGUILayout.EnumPopup ("Needed Mission:", m_sceneLookupEnum);
				if (GUILayout.Button ("Update Teleported Game Scene")) {
					trigger.SetTeleportedScene (m_sceneLookupEnum);
				}
				GUI.backgroundColor = Color.white;
			}
			base.DrawDefaultInspector ();
			if (GUI.changed) {
				EditorUtility.SetDirty (target);
			}

		}
		public SceneLookupEnum m_sceneLookupEnum;
	}
}