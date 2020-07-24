using System;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

namespace LocalCache
{
    public class PersistentStorage
    {
        public static void Save(string path,string file)
        {
            StreamWriter sw;
            FileInfo fileInfo = new FileInfo(m_path+ "//" + path);
            sw = fileInfo.CreateText();
            sw.WriteLine(file);
            sw.Close();
        }

        public static void Delete(string path)
        {
            FileInfo fi = new FileInfo(m_path+ "//" + path);
            try
            {
                fi.Delete();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }        
        }

        public static string Load(string path)
        {
            string txt="";
            StreamReader sr;
            try
            {
                sr = File.OpenText(m_path + "//" + path);
            }
            catch (Exception e)
            {
                Debug.LogError($"未找到文件夹:{path}");
                return  null;
            }
            txt = sr.ReadToEnd();
            sr.Close();
            return txt;
        }

        private static string m_path = Application.persistentDataPath;
    }
}