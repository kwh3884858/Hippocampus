using UnityEditor;
using UnityEngine;

namespace Config.Editor.Windows
{
    [CustomEditor(typeof(StoryConfig))]
    public class CustomGameSettingEditor : UnityEditor.Editor
    {
        private StoryConfig data;

        private void OnEnable()
        {
            data = target as StoryConfig;
        }

        public override void OnInspectorGUI()
        {
            if (data == null)
            {
                return;
            }

            EditorGUILayout.BeginVertical();
            base.OnInspectorGUI();
            if (GUILayout.Button("更新参数", GUILayout.Width(100)))
            {
//                GameSettingService.Ins.SetGameSetting(data);
            }

            EditorGUILayout.EndVertical();
        }
    }
}