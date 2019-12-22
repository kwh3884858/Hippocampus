using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
namespace Skylight
{
	public class CreateScriptForUI : MonoBehaviour
	{


		[MenuItem ("Assets/Framework/CreateUIPanelScript %m")]
		static void CreateUIPanelScript ()
		{
			string selectName = Selection.activeGameObject.name;

			ProjectWindowUtil.StartNameEditingIfProjectWindowExists (0,
			ScriptableObject.CreateInstance<CreateAssetAction> (),
			 "Assets/Scripts/UI/Panel/" + selectName + ".cs",
			EditorGUIUtility.IconContent ("cs Script Icon").image as Texture2D,
			"Assets/Skylight/Editor/Template/NewUIPanelScript.cs.txt");
		}
		[MenuItem ("Assets/Framework/CreateUIPanelScript %m", true)]
		static bool ValidateSelectedPrefab ()
		{
			if (Selection.activeGameObject == null) { return false; }
			return PrefabUtility.GetPrefabType (Selection.activeGameObject) == PrefabType.Prefab;
		}


		[MenuItem ("Assets/Framework/AddTextLocalization %l")]
		static void AddTextLocalization ()
		{
			Text [] texts = Selection.activeGameObject.GetComponentsInChildren<Text> ();
			foreach (Text text in texts) {
				if (!text.gameObject.GetComponent<UITextLocalization> ()) {
					text.gameObject.AddComponent<UITextLocalization> ();

				}

			}
		}
		[MenuItem ("Assets/Framework/AddTextLocalization %l", true)]
		static bool ValidateSelectedIsPrefab ()
		{
			if (Selection.activeGameObject == null) { return false; }

			return PrefabUtility.GetPrefabType (Selection.activeGameObject) == PrefabType.Prefab;
		}


		private static string GetSelectedPath ()
		{
			//默认路径为Assets
			string selectedPath = "Assets";

			//获取选中的资源
			Object [] selection = Selection.GetFiltered (typeof (Object), SelectionMode.Assets);

			//遍历选中的资源以返回路径
			foreach (Object obj in selection) {
				selectedPath = AssetDatabase.GetAssetPath (obj);
				if (!string.IsNullOrEmpty (selectedPath) && File.Exists (selectedPath)) {
					selectedPath = Path.GetDirectoryName (selectedPath);
					break;
				}
			}

			return selectedPath;
		}

	}

}
