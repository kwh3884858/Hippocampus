using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Text;

public class PlayFromEmptyGameRoot : MonoBehaviour
{
	static readonly string [] openList = { "Main Camera", "GameRoot" };

	const string playFromFirstMenuStr = "Edit/Always Start From Scene GameRoot &p";

	static bool playFromFirstScene {
		get { return EditorPrefs.HasKey (playFromFirstMenuStr) && EditorPrefs.GetBool (playFromFirstMenuStr); }
		set { EditorPrefs.SetBool (playFromFirstMenuStr, value); }
	}

	[MenuItem (playFromFirstMenuStr, false, 150)]
	static void PlayFromFirstSceneCheckMenu ()
	{
		playFromFirstScene = !playFromFirstScene;
		Menu.SetChecked (playFromFirstMenuStr, playFromFirstScene);

        LogPrinter.ShowNotifyOrLog(playFromFirstScene ? "Play from GameRoot scene" : "Play from current scene");
	}

	// The menu won't be gray out, we use this validate method for update check state
	[MenuItem (playFromFirstMenuStr, true)]
	static bool PlayFromFirstSceneCheckMenuValidate ()
	{
		Menu.SetChecked (playFromFirstMenuStr, playFromFirstScene);
		return true;
	}

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ApplyPerfabAndSetAllSceneFalse()
    {

    }

    // This method is called before any Awake. It's the perfect callback for this feature
    [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.AfterSceneLoad)]
	static void LoadFirstSceneAtGameBegins ()
	{
        LogPrinter.ShowSceneInfo();

        if (SceneManager.GetActiveScene ().buildIndex == 0) {
			foreach (GameObject go in SceneManager.GetActiveScene ().GetRootGameObjects ()) {
				go.SetActive (false);
				foreach (string name in openList) {
					if (go.name == name) {
						go.SetActive (true);
						break;
					}
				}
			}
		} else {
			foreach (GameObject go in Object.FindObjectsOfType<GameObject> ())
				go.SetActive (false);
		}


		if (!playFromFirstScene)
			return;

		if (EditorBuildSettings.scenes.Length == 0) {
			Debug.LogWarning ("The scene build list is empty. Can't play from first scene.");
			return;
		}



		SceneManager.LoadScene (0);


	}


	static IEnumerator AfterLoad ()
	{
		yield return null;
		Debug.Log ("sceneCountInBuildSettings: " + SceneManager.sceneCountInBuildSettings);

		GameObject goes = new GameObject ("Custom Game Object");
		// Ensure it gets reparented if this was a context click (otherwise does nothing)

		GameObjectUtility.SetParentAndAlign (goes, FindObjectOfType<GameObject> ());
		// Register the creation in the undo system
		Undo.RegisterCreatedObjectUndo (goes, "Create " + goes.name);
		Selection.activeObject = goes;
	}

   
}
