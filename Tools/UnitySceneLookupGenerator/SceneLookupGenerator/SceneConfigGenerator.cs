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
        public SceneConfigGenerator()
        {
            m_sceneRootPath = "";
            m_outputPath = "";
            m_templatePath = "";

        }

        string m_sceneRootPath;
        string m_outputPath;
        string m_templatePath;

        public string GetSceneRootPath()
        {
            return m_sceneRootPath;
        }

        public ErrorType SetSceneRootPath(string projectRoot)
        {
            m_sceneRootPath = projectRoot;
            return CheckDirectoryPath(m_sceneRootPath, ErrorType.NoSceneRootPath);
        }

        public ErrorType SetOutputPath(string output)
        {
            m_outputPath = output;

            return CheckDirectoryPath(m_outputPath, ErrorType.NoOutputPath);

        }

        public ErrorType SetTemplatePath(string templatePath)
        {
            m_templatePath = templatePath;

            return CheckFilePath(m_templatePath, ErrorType.NoTemplateFile);
        }

        private ErrorType CheckDirectoryPath(string path, ErrorType pathError)
        {
            if (!Directory.Exists(path))
            {
                return pathError;
            }

            return ErrorType.NoError;
        }

        private ErrorType CheckFilePath(string path, ErrorType pathError)
        {
            if (!File.Exists(path))
            {
                return pathError;
            }

            return ErrorType.NoError;
        }


    }
}
