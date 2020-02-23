using System;

namespace SceneLookupGenerator
{
	class MainClass
	{

		/// <summary>
		/// Config
		/// </summary>

		//Debug version will use a simulation path, 
		//Release will use path that executable file exists to be root path
		enum DevMode
		{
			Debug,
			Release
		}

		private static DevMode DEV_MODE = DevMode.Release;
		private static string Path_Scene_Lookup_Template = "\\SceneLookupTemplate.txt";
		private static string Path_Assets = "\\Assets";
		private static string Path_Scripts_To_RootConfig = "\\Scripts\\Config\\RootConfig.cs";
		private static string Path_StarPlatinum_To_GameRootInspector = "\\StarPlatinum\\Editor\\GameRootInspector.cs";
		private static string Scene_Lookup_OutputName = "\\SceneLookup.cs";

		private static string exePath {
			get {
				string exePath = "None";
				if (DEV_MODE == DevMode.Release) {
					exePath = Environment.CurrentDirectory;
				}

				if (DEV_MODE == DevMode.Debug) {
					exePath = "D:\\GithubRepository\\Hippocampus\\Assets\\Scenes";
				}
				return exePath;
			}
		}
		/// <summary>
		/// Program
		/// </summary>

		static SceneLookupGenerator m_sceneLookupGenerator;
		static SceneConfigGenerator m_sceneConfigGenerator;

		//Example: -s /Users/cookie/Documents/UnityProjectFolder/BladeSlayer/Assets/Scenes -o /Users/cookie/Documents/UnityProjectFolder/BladeSlayer/Assets/Scenes
		public static void Main (string [] args)
		{
			//Initialize a generator
			m_sceneLookupGenerator = new SceneLookupGenerator ();
			m_sceneConfigGenerator = new SceneConfigGenerator ();

			Console.WriteLine ("Use -h for help info");

			if (Environment.OSVersion.Platform == PlatformID.Unix ||
				Environment.OSVersion.Platform == PlatformID.MacOSX) {
				Path_Scene_Lookup_Template = Path_Scene_Lookup_Template.Replace ('\\', '/');
				Path_Assets = Path_Assets.Replace ('\\', '/');
				Path_Scripts_To_RootConfig = Path_Scripts_To_RootConfig.Replace ('\\', '/');
				Path_StarPlatinum_To_GameRootInspector = Path_StarPlatinum_To_GameRootInspector.Replace ('\\', '/');
				Scene_Lookup_OutputName = Scene_Lookup_OutputName.Replace ('\\', '/');

			}

			//Argument:
			//r: set scene root file path.
			//Generator will find the all scene file in the path.

			//o: set output file path
			//Generated scene lookup file will put in this path.		 

			ErrorType error = ErrorType.NoError;

			m_sceneLookupGenerator.SetSceneLookupOutputName (Scene_Lookup_OutputName);

			// Default argument
			if (args.Length == 0) {
				//string exePath = "None";
				//if (DEV_MODE == DevMode.Release) {
				//	exePath = Environment.CurrentDirectory;
				//}

				//if (DEV_MODE == DevMode.Debug) {
				//	exePath = "D:\\GithubRepository\\Hippocampus\\Assets\\Scenes";
				//}

				error = m_sceneLookupGenerator.SetSceneRootPath (exePath);
				CheckError (error);
				error = m_sceneLookupGenerator.SetOutputPath (exePath);
				CheckError (error);
				error = m_sceneLookupGenerator.SetTemplateFile (exePath + Path_Scene_Lookup_Template);
				CheckError (error);

				error = m_sceneConfigGenerator.SetSceneRootPath (exePath);
				CheckError (error);

				//Find Scene Folder "Assets\\Scripts\\Config\\RootConfig.cs"
				int pos = exePath.LastIndexOf (Path_Assets);
				if (pos < 0) return;
				//sub string to "../Assets"
				string configFath = exePath.Substring (0, pos + 7);
				configFath += Path_Scripts_To_RootConfig;
				error = m_sceneConfigGenerator.SetOutputPath (configFath);
				CheckError (error);

				string inspectorFath = exePath.Substring (0, pos + 7);
				if (pos < 0) return;
				inspectorFath += Path_StarPlatinum_To_GameRootInspector;
				error = m_sceneConfigGenerator.SetInspectorFile (inspectorFath);
				CheckError (error);
			}

			// For argument contained
			for (int i = 0; i < args.Length; i += 2) {

				error = ErrorType.NoError;

				switch (args [i]) {
				case "-s":
					string root = args [i + 1];
					error = m_sceneLookupGenerator.SetSceneRootPath (root);
					break;

				case "-o":
					string output = args [i + 1];
					error = m_sceneLookupGenerator.SetOutputPath (output);
					break;

				case "-t":
					string template = args [i + 1];
					error = m_sceneLookupGenerator.SetTemplateFile (template);
					break;

				case "-h":
					Console.Write (@"
Argument:
r: set scene root file path.
Generator will find the all scene file in the path.

o: set output file path
Generated scene lookup file will put in this path.	
					");
					i--;
					return;
				}

				switch (error) {
				case ErrorType.NoError:
					break;

				case ErrorType.NoSceneRootPath:
					Console.WriteLine ("Scene root isn`t exist.");
					return;

				case ErrorType.NoOutputPath:
					Console.WriteLine ("Output path is not exist.");

					return;
				}
			}

			error = m_sceneLookupGenerator.Execute ();

			CheckError (error);

			error = m_sceneConfigGenerator.Execute ();

			CheckError (error);

			Console.WriteLine ("Generate Lookup Successful!");
		}

		static void CheckError (ErrorType error)
		{

			switch (error) {
			case ErrorType.NoError:
				break;

			case ErrorType.NoTemplateFile:
				Console.WriteLine ("Template file is not exist.");
				return;

			case ErrorType.NoSceneRootPath:
				Console.WriteLine ("Scene root is not exist.");
				return;

			case ErrorType.NoOutputPath:
				Console.WriteLine ("Output path is not exist.");

				return;
			}
		}
	}
}