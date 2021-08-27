using System;
using System.IO;
using System.Reflection;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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

        private static DevMode DEV_MODE = DevMode.Debug;
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
                    if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
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

        private static string configPath
        {
            get
            {
                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                if (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                {
                    path += "/GeneratorConfig.yaml";
                }
                else
                {
                    path += "\\GeneratorConfig.yaml";
                }

                return path;
            }
        }
        /// <summary>
        /// Program
        /// </summary>

        static SceneLookupGenerator m_sceneLookupGenerator;

        //Example: -s /Users/cookie/Documents/UnityProjectFolder/BladeSlayer/Assets/Scenes -o /Users/cookie/Documents/UnityProjectFolder/BladeSlayer/Assets/Scenes
        public static void Main(string[] args)
        {
            //Initialize a generator
            m_sceneLookupGenerator = new SceneLookupGenerator();

            StringBuilder content = new StringBuilder();
            //Read template
            using (FileStream fs = File.OpenRead(configPath))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {

                    content.Append( temp.GetString(b));
                    Array.Clear(b, 0, b.Length);
                }
            }
            IDeserializer deserializer = new DeserializerBuilder()
                .Build();

            GeneratorPathConfig config = deserializer.Deserialize<GeneratorPathConfig>(content.ToString());

            Console.WriteLine("Use -h for help info");

            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
            {
                config.ForamtForUnixLikeSystem();
            }

            //Argument:
            //r: set scene root file path.
            //Generator will find the all scene file in the path.

            //o: set output file path
            //Generated scene lookup file will put in this path.		 

            ErrorType error = ErrorType.NoError;

            m_sceneLookupGenerator.SetSceneLookupOutputName(config.Scene_Lookup_OutputName);

            // Default argument
            if (args.Length == 0)
            {
                int pos = exePath.LastIndexOf(config.Path_Assets);
                if (pos < 0) return;
                //sub string to "../Assets"
                string configFath = exePath.Substring(0, pos + PATH_ASSETS_LENGTH);

                //scene root folder "Assets\\data\\graphics\\World"
                string worldRootPath = configFath + config.Path_Data_To_World;
                error = m_sceneLookupGenerator.SetWorldRootPath(worldRootPath);
                CheckError(error);

                //scene output folder "Assets\\data\\tools\\SceneLookupGenerator\\SceneLookup.cs"
                string lookupOutputPath = configFath + config.Path_Data_To_SceneLookupGenerator;
                error = m_sceneLookupGenerator.SetOutputPath(lookupOutputPath);
                CheckError(error);

                //lookup template folder "Assets\\data\\tools\\SceneLookupGenerator\\SceneLookupTemplate.txt"
                string templatePath = configFath + config.Path_Code_To_File_SceneLookupTemplate;
                error = m_sceneLookupGenerator.SetTemplateFile(templatePath);
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