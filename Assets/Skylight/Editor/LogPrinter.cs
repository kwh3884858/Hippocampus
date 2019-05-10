using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum LogType
{
    Debug,
    SceneView,

}
public class LogPrinter : MonoBehaviour {


    static public void ShowNotifyOrLog(string msg,LogType logType = LogType.Debug)
    {
        switch (logType)
        {
            case LogType.SceneView:
                if (Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
                    EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
                else
                    Debug.Log(msg); // When there's no scene view opened, we just print a log

                break;

            case LogType.Debug:
                Debug.Log(msg); // When there's no scene view opened, we just print a log

                break;
        }

    }

    /// <summary>
    /// Shows the scene info.
    /// Show All Infomation in a single time.
    /// </summary>
    static public void ShowSceneInfo()
    {
        Scene scene = SceneManager.GetActiveScene();
        StringBuilder builder = new StringBuilder();
        builder.Append("Current Scene Name; ");
        builder.Append(scene.name);
        builder.Append("\nScene Index In BuilderSettings: ");
        builder.Append(scene.buildIndex);
        builder.Append("\nCurrent Scene Path: ");
        builder.Append(scene.path);
        ShowNotifyOrLog(builder.ToString(),LogType.SceneView);
    }
}
