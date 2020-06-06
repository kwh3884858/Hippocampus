using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLookupGenerator
{
	class SceneConfigGenerator
	{
		private static readonly string SCENE_CONFIG_CODE_START = "//[Scene Config Auto Generated Code Begin]";
		private static readonly string SCENE_CONFIG_CODE_END = "//[Scene Config Auto Generated Code End]";

		private static readonly string GETTER_CASE_CODE_START = "//[Switch Case Auto Generated Code Begin]";
		private static readonly string GETTER_CASE_CODE_END = "//[Switch Case Auto Generated Code End]";

		private static readonly string TYPE_VARIABLE_CODE_START = "//[Camera Type Variable Auto Generated Code Begin]";
		private static readonly string TYPE_VARIABLE_CODE_END = "//[Camera Type Variable Auto Generated Code End]";

		private static readonly string POPUP_CODE_START = "//[Inspector Popup Auto Generated Code Begin]";
		private static readonly string POPUP_CODE_END = "//[Inspector Popup Auto Generated Code End]";

		public SceneConfigGenerator ()
		{
			m_worldRootPath = "";
			m_configPath = "";
			m_InspectorPath = "";

		}

		string m_worldRootPath;
		string m_configPath;
		string m_InspectorPath;

		public ErrorType Execute ()
		{
			List<string> scenes;

			string allContent = "";

			//Find template
			if (!Directory.Exists (m_worldRootPath)) {
				return ErrorType.NoSceneRootPath;
			}


			DirectoryInfo folder = new DirectoryInfo (m_worldRootPath);
			FileInfo [] SceneInfos = folder.GetFiles ("*.unity", SearchOption.AllDirectories);
			//FileInfo [] sceneMetaInfos = folder.GetFiles("*.unity.meta");

			scenes = new List<string> ();
			//SceneIds = new List<string>();


			foreach (FileInfo info in SceneInfos) {
				scenes.Add (info.Name.Remove (info.Name.LastIndexOf (".", StringComparison.Ordinal)));
			}

			//Check config Path error
			ErrorType error = CheckFilePath (m_configPath, ErrorType.NoTemplateFile);
			if (error != ErrorType.NoError) {
				return ErrorType.NoTemplateFile;
			}

			//Read template
			using (FileStream fs = File.OpenRead (m_configPath)) {
				byte [] buffer = new byte [1024];
				UTF8Encoding temp = new UTF8Encoding (true);
				while (fs.Read (buffer, 0, buffer.Length) > 0) {

					allContent += temp.GetString (buffer);
					Array.Clear (buffer, 0, buffer.Length);

				}
			}

			//Generator code for GameRoot config
			StringBuilder inputContent = new StringBuilder ();

			inputContent.Append (SCENE_CONFIG_CODE_START);
			inputContent.Append ("\n\n");
			for (int i = 0; i < scenes.Count; i++) {
				inputContent.Append ($"[SerializeField]\n");
				inputContent.Append ($"private string {scenes [i]}Name = \"{ scenes [i]}\"; \n ");
				inputContent.Append ($"public CameraService.SceneCameraType {scenes [i]}CameraType;  \n\n");
			}

			int startOfConfig = allContent.IndexOf (SCENE_CONFIG_CODE_START);
			int endOfConfig = allContent.IndexOf (SCENE_CONFIG_CODE_END);

			if (startOfConfig < 0 || endOfConfig < 0) {
				return ErrorType.NotExistInsertFlag;
			}

			allContent = allContent.Remove (startOfConfig, endOfConfig - startOfConfig);
			allContent = allContent.Insert (startOfConfig, inputContent.ToString ());
			inputContent.Clear ();


			// Getter Case Auto Generator
			inputContent.Append (GETTER_CASE_CODE_START);
			inputContent.Append ("\n\n");
			for (int i = 0; i < scenes.Count; i++) {
				inputContent.Append ($"case \"{scenes [i]}\" :  \n");
				inputContent.Append ($"   return {scenes [i]}CameraType; \n\n");
			}

			int startOfCase = allContent.IndexOf (GETTER_CASE_CODE_START);
			int endOfCase = allContent.IndexOf (GETTER_CASE_CODE_END);

			if (startOfCase < 0 || endOfCase < 0) {
				return ErrorType.NotExistInsertFlag;
			}

			allContent = allContent.Remove (startOfCase, endOfCase - startOfCase);
			allContent = allContent.Insert (startOfCase, inputContent.ToString ());
			inputContent.Clear ();

			allContent = allContent.Substring (0, allContent.IndexOf ('\0'));

			//Write scene lookup
			if (File.Exists (m_configPath)) {
				File.Delete (m_configPath);
			}
			// Write the byte array to the other FileStream.
			using (FileStream fsNew = new FileStream (m_configPath,
					 FileMode.Create, FileAccess.Write)) {
				Byte [] bs = Encoding.UTF8.GetBytes (allContent);
				fsNew.Write (bs, 0, bs.Length);
			}


			//Auto Generated Code For Game Root Inspector
			allContent = "";
			using (FileStream fs = File.OpenRead (m_InspectorPath)) {
				byte [] buffer = new byte [1024];
				UTF8Encoding temp = new UTF8Encoding (true);
				while (fs.Read (buffer, 0, buffer.Length) > 0) {

					allContent += temp.GetString (buffer);
					Array.Clear (buffer, 0, buffer.Length);

				}
			}

			// Type Variable
			inputContent.Append (TYPE_VARIABLE_CODE_START);
			inputContent.Append ("\n");
			inputContent.Append ("\n");
			for (int i = 0; i < scenes.Count; i++) {

				inputContent.Append ($"CameraService.SceneCameraType m_{scenes [i]}CameraType; \n\n ");

			}

			int startOfTypeVar = allContent.IndexOf (TYPE_VARIABLE_CODE_START);
			int endOfTypeVar = allContent.IndexOf (TYPE_VARIABLE_CODE_END);

			if (startOfTypeVar < 0 || endOfTypeVar < 0) {
				return ErrorType.NotExistInsertFlag;
			}

			allContent = allContent.Remove (startOfTypeVar, endOfTypeVar - startOfTypeVar);
			allContent = allContent.Insert (startOfTypeVar, inputContent.ToString ());
			inputContent.Clear ();

			//POPUP
			inputContent.Append (POPUP_CODE_START);
			inputContent.Append ("\n\n");
			for (int i = 0; i < scenes.Count; i++) {
				inputContent.Append ($"m_{scenes [i]}CameraType = ConfigRoot.Instance.{scenes [i]}CameraType;  \n");
				inputContent.Append ($"m_{scenes [i]}CameraType = (CameraService.SceneCameraType)EditorGUILayout.EnumPopup(\"{scenes [i]} Camera Type: \", m_{scenes [i]}CameraType); \n");
				inputContent.Append ($"    if (m_{scenes [i]}CameraType != ConfigRoot.Instance.{scenes [i]}CameraType) \n");
				inputContent.Append ("{ \n");
				inputContent.Append ($"ConfigRoot.Instance.{scenes [i]}CameraType = m_{scenes [i]}CameraType; \n");
				inputContent.Append ("} \n\n");

			}

			int startOfPopup = allContent.IndexOf (POPUP_CODE_START);
			int endOfPopup = allContent.IndexOf (POPUP_CODE_END);

			if (startOfPopup < 0 || endOfPopup < 0) {
				return ErrorType.NotExistInsertFlag;
			}

			allContent = allContent.Remove (startOfPopup, endOfPopup - startOfPopup);
			allContent = allContent.Insert (startOfPopup, inputContent.ToString ());
			inputContent.Clear ();

			allContent = allContent.Substring (0, allContent.IndexOf ('\0'));


			//Write scene lookup
			if (File.Exists (m_InspectorPath)) {
				File.Delete (m_InspectorPath);
			}
			// Write the byte array to the other FileStream.
			using (FileStream fsNew = new FileStream (m_InspectorPath,
					 FileMode.Create, FileAccess.Write)) {
				Byte [] bs = Encoding.UTF8.GetBytes (allContent);
				fsNew.Write (bs, 0, bs.Length);
			}


			return ErrorType.NoError;

		}

		public string GetSceneRootPath ()
		{
			return m_worldRootPath;
		}

		public ErrorType SetSceneRootPath (string projectRoot)
		{
			m_worldRootPath = projectRoot;
			return CheckDirectoryPath (m_worldRootPath, ErrorType.NoSceneRootPath);
		}

		public ErrorType SetOutputPath (string output)
		{
			m_configPath = output;

			return CheckDirectoryPath (m_configPath, ErrorType.NoOutputPath);

		}

		public ErrorType SetInspectorFile (string templatePath)
		{
			m_InspectorPath = templatePath;

			return CheckFilePath (m_InspectorPath, ErrorType.NoTemplateFile);
		}

		private ErrorType CheckDirectoryPath (string path, ErrorType pathError)
		{
			if (!Directory.Exists (path)) {
				return pathError;
			}

			return ErrorType.NoError;
		}

		private ErrorType CheckFilePath (string path, ErrorType pathError)
		{
			if (!File.Exists (path)) {
				return pathError;
			}

			return ErrorType.NoError;
		}


	}
}
