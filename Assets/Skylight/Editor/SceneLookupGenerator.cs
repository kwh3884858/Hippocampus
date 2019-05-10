using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class SceneLookupGenerator
{

	static readonly string m_templatePath = "Assets/Skylight/Editor/Template/SceneLookupTemplate.txt";
	static readonly string m_outputPath = "Assets/Scenes/SceneLookup.cs";

	static readonly string m_macroConstant = "#CONSTANT#";
	static readonly string m_macroTotal = "#TOTAL#";
	static readonly string m_macroSceneName = "#SCENENAME#";
	static readonly string m_macroSceneEnum = "#SCENEENUM#";

	static string m_content;

	public static void CreateSceneLookup (List<string> scenes)
	{

		//Find template
		if (!File.Exists (m_templatePath)) {
			Debug.Log ("Can`t find template file " + m_templatePath);
			return;
		}


		//Read template
		using (FileStream fs = File.OpenRead (m_templatePath)) {
			byte [] b = new byte [1024];
			UTF8Encoding temp = new UTF8Encoding (true);
			while (fs.Read (b, 0, b.Length) > 0) {

				m_content = temp.GetString (b);

			}
		}


		//Replace macro constant int
		StringBuilder content = new StringBuilder ();

		for (int i = 0; i < scenes.Count; i++) {
			content.Append ("public const int ");
			content.Append (scenes [i]);
			content.Append (" = ");
			content.Append (i);
			content.Append (";\n");

		}
		m_content = m_content.Replace (m_macroConstant, content.ToString ());
		//Replace total count
		m_content = m_content.Replace (m_macroTotal, scenes.Count.ToString ());


		content.Clear ();

		for (int i = 0; i < scenes.Count; i++) {
			content.Append ("\"");
			content.Append (scenes [i]);
			content.Append ("\",\n");


		}
		//Remove last comma
		content.Remove (content.Length - 2, 2);
		m_content = m_content.Replace (m_macroSceneName, content.ToString ());

		//Repalce macro enum scene name
		content.Clear ();
		for (int i = 0; i < scenes.Count; i++) {
			content.Append (scenes [i]);
			content.Append (" = ");
			content.Append (i);
			content.Append (",\n");
		}

		content.Remove (content.Length - 2, 2);

		m_content = m_content.Replace (m_macroSceneEnum, content.ToString ());

		//Write scene lookup
		if (File.Exists (m_outputPath)) {
			File.Delete (m_outputPath);
			return;
		}
		// Write the byte array to the other FileStream.

		using (FileStream fsNew = new FileStream (m_outputPath,
				 FileMode.Create, FileAccess.Write)) {
			fsNew.Write (Encoding.UTF8.GetBytes (m_content), 0, m_content.Length);
		}

		AssetDatabase.Refresh ();

	}

}
