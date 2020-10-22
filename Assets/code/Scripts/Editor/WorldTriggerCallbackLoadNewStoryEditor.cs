using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GamePlay.EventTrigger
{
    [CustomEditor(typeof(WorldTriggerCallbackLoadNewStory))]
    public class WorldTriggerCallbackLoadNewStoryEditor : Editor
    {
        void OnSceneGUI()
        {
            WorldTriggerCallbackLoadNewStory teleport = (WorldTriggerCallbackLoadNewStory)target;
            Vector3 pos = teleport.transform.position;
            Vector3 relativePos = teleport.m_lengthBetweenTriggerAndSpwanPoint * WorldTriggerCallbackTeleportPlayer.DirecitonMapping[teleport.m_spawnDirection];

            Handles.color = Color.red;
            Handles.DrawLine(pos, pos + relativePos);
            Handles.color = Color.green;
            Handles.DrawLine(pos + relativePos, pos + relativePos + Vector3.up);
        }
    }
}