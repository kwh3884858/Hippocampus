using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLookupGenerator
{
    class MissionEnumGenerator
    {
        private static readonly string MISSION_ENUM_START = "//[Mission Enum Auto Generated Code Begin]";
        private static readonly string MISSION_ENUM_END = "//[Mission Enum Auto Generated Code End]";

        private static readonly string ALL_MISSION_START = "//[All Mission Enum Variable Auto Generated Code Begin]";
        private static readonly string ALL_MISSION_END = "//[All Mission Enum Variable Auto Generated Code End]";

        string m_content;

        string m_missionEnumPath;
        List<string> m_allMission;
        public MissionEnumGenerator(string missionEnumPath, List<string> allMission)
        {
            m_missionEnumPath = missionEnumPath;
            m_allMission = allMission;
        }


        public ErrorType Execute()
        {
            //Read MissionSceneManager
            using (FileStream fs = File.OpenRead(m_missionEnumPath))
            {
                byte[] b = new byte[1024];
                UTF8Encoding temp = new UTF8Encoding(true);
                while (fs.Read(b, 0, b.Length) > 0)
                {
                    m_content += temp.GetString(b);
                    Array.Clear(b, 0, b.Length);
                }
            }


            //Generator code for Mission Enum
            StringBuilder inputContent = new StringBuilder();

            inputContent.Append(MISSION_ENUM_START);
            inputContent.Append("\n");
            for (int i = 0; i < m_allMission.Count; i++)
            {
                inputContent.Append($"{m_allMission[i]},  \n");
            }

            int startOfConfig = m_content.IndexOf(MISSION_ENUM_START);
            int endOfConfig = m_content.IndexOf(MISSION_ENUM_END);

            if (startOfConfig < 0 || endOfConfig < 0)
            {
                return ErrorType.NotExistInsertFlag;
            }

            m_content = m_content.Remove(startOfConfig, endOfConfig - startOfConfig);
            m_content = m_content.Insert(startOfConfig, inputContent.ToString());
            inputContent.Clear();

            //Generator code for All mission variable
            inputContent.Append(ALL_MISSION_START);
            inputContent.Append("\n");
            for (int i = 0; i < m_allMission.Count; i++)
            {
                inputContent.Append($"MissionEnum.{m_allMission[i]},  \n");
            }

            startOfConfig = m_content.IndexOf(ALL_MISSION_START);
            endOfConfig = m_content.IndexOf(ALL_MISSION_END);

            if (startOfConfig < 0 || endOfConfig < 0)
            {
                return ErrorType.NotExistInsertFlag;
            }

            m_content = m_content.Remove(startOfConfig, endOfConfig - startOfConfig);
            m_content = m_content.Insert(startOfConfig, inputContent.ToString());
            inputContent.Clear();

            m_content = m_content.Substring(0, m_content.IndexOf('\0'));

            // Write the byte array to the other FileStream.
            using (FileStream fsNew = new FileStream(m_missionEnumPath,
                     FileMode.Create, FileAccess.Write))
            {
                Byte[] bs = Encoding.UTF8.GetBytes(m_content);
                fsNew.Write(bs, 0, bs.Length);
            }

            return ErrorType.NoError;
        }

    }
}
