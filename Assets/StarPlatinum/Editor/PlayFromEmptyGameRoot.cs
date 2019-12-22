using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Text;
using UnityEngine.UI;

public class PlayFromEmptyGameRoot : MonoBehaviour
{

	const string playFromSceneGameRoot = "Edit/Always Start From Scene GameRoot &p";

	static bool isPlayFromGameRoot {
		get { return EditorPrefs.HasKey (playFromSceneGameRoot) && EditorPrefs.GetBool (playFromSceneGameRoot); }
		set { EditorPrefs.SetBool (playFromSceneGameRoot, value); }
	}

	[MenuItem (playFromSceneGameRoot, false, 150)]
	static void PlayFromFirstSceneCheckMenu ()
	{
		isPlayFromGameRoot = !isPlayFromGameRoot;
		Menu.SetChecked (playFromSceneGameRoot, isPlayFromGameRoot);

		LogPrinter.ShowNotifyOrLog (isPlayFromGameRoot ? "Play from GameRoot scene" : "Play from current scene");
	}

	// The menu won't be gray out, we use this validate method for update check state
	[MenuItem (playFromSceneGameRoot, true)]
	static bool PlayFromFirstSceneCheckMenuValidate ()
	{
		Menu.SetChecked (playFromSceneGameRoot, isPlayFromGameRoot);
		return true;
	}

	[RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void ApplyPerfabAndSetAllSceneFalse ()
	{

	}

	// This method is called before any Awake. It's the perfect callback for this feature
	[RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.AfterSceneLoad)]
	static void LoadFirstSceneAtGameBegins ()
	{
		LogPrinter.ShowSceneInfo ();



		if (!isPlayFromGameRoot)
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
