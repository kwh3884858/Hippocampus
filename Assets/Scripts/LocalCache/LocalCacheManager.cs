using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LocalCache
{
    public class LocalCacheManager
    {
        //Path on window : C:\Users\User\AppData\LocalLow\
        private static string Path = "";
        private string m_saveID;
        private Dictionary<string, LocalCacheBase> m_data;
        private DictionaryCache m_dicData;

        public LocalCacheManager(string saveID)
        {
            m_saveID = saveID;
            m_data = new Dictionary<string, LocalCacheBase>();
            Load();
            m_dicData = GetData<DictionaryCache>();
            if (m_dicData == null)
            {
                m_dicData=new DictionaryCache();
            }
        }
        
        public int GetInt(string key)
        {
            if (m_dicData != null && m_dicData.IntDic.ContainsKey(key))
            {
                return m_dicData.IntDic[key];
            }
            else
            {
                return 0;
            }
        }
        
        public void SaveInt(string key,int value)
        {
            if (m_dicData.IntDic.ContainsKey(key))
            {
                m_dicData.IntDic[key] = value;
            }
            else
            {
                m_dicData.IntDic.Add(key, value);
            }
            SetData(m_dicData);
        }
        public string GetString(string key)
        {
            if (m_dicData != null && m_dicData.StringDic.ContainsKey(key))
            {
                return m_dicData.StringDic[key];
            }
            else
            {
                return null;
            }
        }
        
        public void SaveString(string key,string value)
        {
            if (m_dicData.StringDic.ContainsKey(key))
            {
                m_dicData.StringDic[key] = value;
            }
            else
            {
                m_dicData.StringDic.Add(key, value);
            }
            SetData(m_dicData);
        }
        public float GetFloat(string key)
        {
            if (m_dicData != null && m_dicData.FloatDic.ContainsKey(key))
            {
                return m_dicData.FloatDic[key];
            }
            else
            {
                return 0;
            }
        }
        
        public void SaveFloat(string key,float value)
        {
            if (m_dicData.FloatDic.ContainsKey(key))
            {
                m_dicData.FloatDic[key] = value;
            }
            else
            {
                m_dicData.FloatDic.Add(key, value);
            }
            SetData(m_dicData);
        }
        
        public bool DeleteData<T>() where T : LocalCacheBase
        {
            string key = typeof(T).FullName;
            if (m_data.ContainsKey(key))
            {
                m_data.Remove(key);
                Save();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetData(LocalCacheBase value)
        {
            string key = value.GetTypeName();
            if (m_data.ContainsKey(key))
            {
                m_data[key] = value;
            }
            else
            {
                m_data.Add(key, value);
            }
            Save();
        }

        public T GetData<T>() where T : LocalCacheBase
        {
            if (m_data.ContainsKey(typeof(T).FullName))
            {
                return m_data[typeof(T).FullName] as T;
            }
            else
            {
                return null;
            }
        }

        private void Save()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(m_saveID);
            foreach(var data in m_data)
            {
                stringBuilder.AppendLine(data.Key);
                stringBuilder.AppendLine(data.Value.ToJson());
            }
            PersistentStorage.Save(Path + m_saveID, stringBuilder.ToString());
        }

        private void Load()
        {

            string str = PersistentStorage.Load(Path + m_saveID);
            if (str == null)
            {
                return;
            }
            List<string> split = new List<string>();
#if (UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR_OSX) && !UNITY_EDITOR_WIN && !UNITY_STANDALONE_WIN
            split.Add("\n");
#endif
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            split.Add("\r\n");
#endif
            string[] strings = str.Split(split.ToArray(), StringSplitOptions.None);
            if (m_saveID.ToString() != strings[0])
            {
                return;
            }
            for(int i = 1; i < strings.Length-1; i++)
            {
                Type type =Type.GetType(strings[i++]);
                LocalCacheBase obj = LocalCacheJsonUtil.Deserialize(type,strings[i])as LocalCacheBase;
                if (obj == null)
                {
                    continue;
                }
                m_data.Add(strings[i - 1], obj);
            }
        }
    }
}
