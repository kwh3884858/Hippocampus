using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneLookupGenerator
{
    class GeneratorPathConfig
    {
        public string Execution_Path { get; set; }
        public string Path_Assets /*= "\\Assets"*/ { get; set; }
        //Scene Lookup
        public string Path_Data_To_World /*= "\\data\\graphics\\World";*/{ get; set; }
        public string Path_Data_To_SceneLookupGenerator /*= "\\data\\tools\\SceneLookupGenerator";*/{ get; set; }
        public string Path_Code_To_File_SceneLookupTemplate /*= "\\data\\tools\\SceneLookupGenerator\\SceneLookupTemplate.txt";*/{ get; set; }
        public string Scene_Lookup_OutputName/* = "\\SceneLookup.cs";*/{ get; set; }
        public string Path_To_Mission_Scene_Manager { get; set; }
        public List<string> Mission_Enum { get; set; }

        public void ForamtForUnixLikeSystem()
        {
            Execution_Path = Execution_Path.Replace('\\', '/');
            Path_Assets = Path_Assets.Replace('\\', '/');
            Path_Data_To_World = Path_Data_To_World.Replace('\\', '/');
            Path_Data_To_SceneLookupGenerator = Path_Data_To_SceneLookupGenerator.Replace('\\', '/');
            Path_Code_To_File_SceneLookupTemplate = Path_Code_To_File_SceneLookupTemplate.Replace('\\', '/');
            Scene_Lookup_OutputName = Scene_Lookup_OutputName.Replace('\\', '/');
            Path_To_Mission_Scene_Manager = Path_To_Mission_Scene_Manager.Replace('\\', '/');
        }
    }
}
