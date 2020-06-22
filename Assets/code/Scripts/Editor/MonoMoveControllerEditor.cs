using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GamePlay
{
    // draw a red circle around the scene cube

    [CustomEditor(typeof(MonoMoveController))]
    public class MonoMoveControllerEditor : Editor
    {
        void OnSceneGUI()
        {
            MonoMoveController moveController = (MonoMoveController)target;

            Handles.color = Color.red;
            Handles.DrawWireDisc(moveController.transform.position, new Vector3(0, 1, 0), moveController.m_showInteractiveUIRadius);
            Handles.color = Color.green;
            Handles.DrawWireDisc(moveController.transform.position, new Vector3(0, 1, 0), moveController.m_interactableRadius);

        }
    }
}