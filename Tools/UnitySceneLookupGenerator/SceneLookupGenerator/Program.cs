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
        private static string Path_Assets = "\\Assets";
        //Scene Lookup
        private static string Path_Data_To_World = "\\data\\graphics\\World";
        private static string Path_Data_To_SceneLookupGenerator = "\\data\\tools\\SceneLookupGenerator";
        private static string Path_Code_To_File_SceneLookupTemplate = "\\data\\tools\\SceneLookupGenerator\\SceneLookupTemplate.txt";

        private static string Path_Code_To_File_RootConfig = "\\code\\Scripts\\Config\\ConfigRoot.cs";
        private static string Path_StarPlatinum_To_GameRootInspector = "\\code\\StarPlatinum\\Editor\\GameRootInspector.cs";
        private static string Scene_Lookup_OutputName = "\\SceneLookup.cs";

        //Length of "\\Assets"
        private static readonly int PATH_ASSETS_LENGTH = 7;

        private static string exePath
        {
            get
            {
                string exePath = "None";
                if (DEV_MODE == DevMode.Release)
                {
                    exePath = Environment.CurrentDirectory;
                }

                if (DEV_MODE == DevMode.Debug)
                {
                    if (Environment.OSVersion.Platform == PlatformID.MacOSX)
                    {
                        exePath = "/Users/cookie/Documents/UnityProjectFolder/Hippocampus/Assets/data/tools/SceneLookupGenerator";
                    }
                    else
                    {
                        exePath = "D:\\GithubRepository\\Hippocampus\\Assets\\data\\tools\\SceneLookupGenerator\\SceneLookupGenerator.exe";
                    }
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
        public static void Main(string[] args)
        {
            //Initialize a generator
            m_sceneLookupGenerator = new SceneLookupGenerator();
            m_sceneConfigGenerator = new SceneConfigGenerator();

            Console.WriteLine("Use -h for help info");

            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                Path_Assets = Path_Assets.Replace('\\', '/');

                Path_Data_To_World = Path_Data_To_World.Replace('\\', '/');
                Path_Data_To_SceneLookupGenerator = Path_Data_To_SceneLookupGenerator.Replace('\\', '/');
                Path_Code_To_File_SceneLookupTemplate = Path_Code_To_File_SceneLookupTemplate.Replace('\\', '/');

                Path_Code_To_File_RootConfig = Path_Code_To_File_RootConfig.Replace('\\', '/');
                Path_StarPlatinum_To_GameRootInspector = Path_StarPlatinum_To_GameRootInspector.Replace('\\', '/');
                Scene_Lookup_OutputName = Scene_Lookup_OutputName.Replace('\\', '/');

            }

            //Argument:
            //r: set scene root file path.
            //Generator will find the all scene file in the path.

            //o: set output file path
            //Generated scene lookup file will put in this path.		 

            ErrorType error = ErrorType.NoError;

            m_sceneLookupGenerator.SetSceneLookupOutputName(Scene_Lookup_OutputName);

            // Default argument
            if (args.Length == 0)
            {
                //string exePath = "None";
                //if (DEV_MODE == DevMode.Release) {
                //	exePath = Environment.CurrentDirectory;
                //}

                //if (DEV_MODE == DevMode.Debug) {
                //	exePath = "D:\\GithubRepository\\Hippocampus\\Assets\\Scenes";
                //}

                int pos = exePath.LastIndexOf(Path_Assets);
                if (pos < 0) return;
                //sub string to "../Assets"
                string configFath = exePath.Substring(0, pos + PATH_ASSETS_LENGTH);

                //scene root folder "Assets\\data\\graphics\\World"
                string worldRootPath = configFath + Path_Data_To_World;
                error = m_sceneLookupGenerator.SetWorldRootPath(worldRootPath);
                CheckError(error);

                //scene output folder "Assets\\data\\tools\\SceneLookupGenerator\\SceneLookup.cs"
                string lookupOutputPath = configFath + Path_Data_To_SceneLookupGenerator;
                error = m_sceneLookupGenerator.SetOutputPath(lookupOutputPath);
                CheckError(error);

                //lookup template folder "Assets\\data\\tools\\SceneLookupGenerator\\SceneLookupTemplate.txt"
                string templatePath = configFath + Path_Code_To_File_SceneLookupTemplate;
                error = m_sceneLookupGenerator.SetTemplateFile(templatePath);
                CheckError(error);

                //scene root folder "Assets\\data\\graphics\\World"
                error = m_sceneConfigGenerator.SetSceneRootPath(worldRootPath);
                CheckError(error);

                //Output Path "Assets\\code\\Scripts\\Config\\ConfigRoot.cs"
                string rootConfigOutputPath = configFath + Path_Code_To_File_RootConfig;
                error = m_sceneConfigGenerator.SetOutputPath(rootConfigOutputPath);
                CheckError(error);

                //inspector path "Assets\\code\\StarPlatinum\\Editor\\GameRootInspector.cs"
                string inspectorFath = configFath + Path_StarPlatinum_To_GameRootInspector;
                error = m_sceneConfigGenerator.SetInspectorFile(inspectorFath);
                CheckError(error);
            }

            // For argument contained
            for (int i = 0; i < args.Length; i += 2)
            {

                error = ErrorType.NoError;

                switch (args[i])
                {
                    case "-s":
                        string root = args[i + 1];
                        error = m_sceneLookupGenerator.SetWorldRootPath(root);
                        break;

                    case "-o":
                        string output = args[i + 1];
                        error = m_sceneLookupGenerator.SetOutputPath(output);
                        break;

                    case "-t":
                        string template = args[i + 1];
                        error = m_sceneLookupGenerator.SetTemplateFile(template);
                        break;

                    case "-h":
                        Console.Write(@"
Argument:
r: set scene root file path.
Generator will find the all scene file in the path.

o: set output file path
Generated scene lookup file will put in this path.	
					");
                        i--;
                        return;
                }

                switch (error)
                {
                    case ErrorType.NoError:
                        break;

                    case ErrorType.NoSceneRootPath:
                        Console.WriteLine("Scene root isn`t exist.");
                        return;

                    case ErrorType.NoOutputPath:
                        Console.WriteLine("Output path is not exist.");

                        return;
                }
            }

            error = m_sceneLookupGenerator.Execute();

            CheckError(error);

/*          Now the game does`t need the scene type.
            
            error = m_sceneConfigGenerator.Execute();

            CheckError(error);
*/
            Console.WriteLine("Generate Lookup Successful!");
        }

        static void CheckError(ErrorType error)
        {

            switch (error)
            {
                case ErrorType.NoError:
                    break;

                case ErrorType.NoTemplateFile:
                    Console.WriteLine("Template file is not exist.");
                    return;

                case ErrorType.NoSceneRootPath:
                    Console.WriteLine("Scene root is not exist.");
                    return;

                case ErrorType.NoOutputPath:
                    Console.WriteLine("Output path is not exist.");

                    return;
            }
        }
    }
}