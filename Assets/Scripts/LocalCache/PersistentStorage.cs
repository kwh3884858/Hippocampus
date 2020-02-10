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
            if (!fileInfo.Exists)
            {
                sw = fileInfo.CreateText();
            }
            else
            {
                sw = fileInfo.AppendText();
            }
            sw.WriteLine(file);
            sw.Close();
            sw.Dispose();
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
            return txt;
        }

        private static string m_path = Application.persistentDataPath;
    }
}