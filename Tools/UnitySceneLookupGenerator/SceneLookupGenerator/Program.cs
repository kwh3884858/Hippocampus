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

        static DevMode m_devMode = DevMode.Release;

        /// <summary>
        /// Program
        /// </summary>


        static SceneLookupGenerator m_sceneLookupGenerator;

        //Example: -s /Users/cookie/Documents/UnityProjectFolder/BladeSlayer/Assets/Scenes -o /Users/cookie/Documents/UnityProjectFolder/BladeSlayer/Assets/Scenes
        public static void Main (string [] args)
		{
			//Initialize a generator
			m_sceneLookupGenerator = new SceneLookupGenerator ();
			Console.WriteLine ("Use -h for help info");

			//Argument:
			//r: set scene root file path.
			//Generator will find the all scene file in the path.

			//o: set output file path
			//Generated scene lookup file will put in this path.		 

			ErrorType error = ErrorType.NoError;

            if(args.Length == 0)
            {
                string exePath= "None";
                if (m_devMode == DevMode.Release)
                {
                    exePath = Environment.CurrentDirectory;
                }
            
                if(m_devMode == DevMode.Debug)
                {
                    exePath = "D:\\GithubProjects\\Hippocampus\\Assets\\Scenes";
                }

                error = m_sceneLookupGenerator.SetSceneRootPath(exePath);
                CheckError(error);
                error = m_sceneLookupGenerator.SetOutputPath(exePath);
                CheckError(error);
                error = m_sceneLookupGenerator.SetTemplatePath(exePath + "\\SceneLookupTemplate.txt");
                CheckError(error);
            }

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
					error = m_sceneLookupGenerator.SetTemplatePath (template);
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
