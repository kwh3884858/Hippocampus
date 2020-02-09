using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GamePlay
{
    // draw a red circle around the scene cube

    [CustomEditor(typeof(InteractiveObject))]
    public class CubeEditor : Editor
    {
        void OnSceneGUI()
        {
            InteractiveObject interactiveObject = (InteractiveObject)target;

            Handles.color = Color.red;
            Handles.DrawWireDisc(interactiveObject.transform.position, new Vector3(0, 1, 0), interactiveObject.m_interactiveRadius);
        }
    }
}