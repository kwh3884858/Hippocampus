using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GamePlay.EventTrigger
{
    [CustomEditor(typeof(WorldTriggerCallbackTeleportPlayer))]
    public class WorldTriggerCallbackTeleportPlayerEditor : Editor
    {

        void OnSceneGUI()
        {
            WorldTriggerCallbackTeleportPlayer teleport = (WorldTriggerCallbackTeleportPlayer)target;
            Vector3 pos = teleport.transform.position;
            Vector3 relativePos = teleport.m_lengthBetweenTriggerAndSpwanPoint* WorldTriggerCallbackTeleportPlayer.DirecitonMapping[teleport.m_spawnDirection];

            Handles.color = Color.red;
            Handles.DrawLine(pos, pos + relativePos);
            Handles.color = Color.green;
            Handles.DrawLine(pos + relativePos, pos + relativePos + Vector3.up);
        }
    }
}