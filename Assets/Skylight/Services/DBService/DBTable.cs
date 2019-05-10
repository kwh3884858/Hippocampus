using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Skylight
{
    public class DBTable : MonoBehaviour
    {
        public int cols;
        public int rows;
        public string filePath;
        private DBRecord[] records;
        private Dictionary<string, int> keyidx = new Dictionary<string,int>();
        private string[][] fileds;
        private bool bIsLoaded = false;
        public bool autoLoad = false;
        void Start()
        {
            if(autoLoad)
                LoadFromFile(filePath);
        }

        public static string[][] Read(string fullname)
        {
            if (!File.Exists(fullname)) return new string[][] { };
            var lines = File.ReadAllLines(fullname);
            var list = new List<string[]>();
            var builder = new StringBuilder();
            for(int i = 2; i < lines.Length; i++)
            {
                var line = lines[i];
                builder.Remove(0, builder.Length);
                var comma = false;
                var array = line.ToCharArray();
                var values = new List<string>();
                var length = array.Length;
                var index = 0;
                while (index < length)
                {
                    var item = array[index++];
                    switch (item)
                    {
                        case ',':
                            if (comma)
                            {
                                builder.Append(item);
                            }
                            else
                            {
                                values.Add(builder.ToString());
                                builder.Remove(0, builder.Length);
                            }
                            break;
                        case '"':
                            if (comma && (index < length) && array[index] == '"')
                            {
                                builder.Append(item);
                                index++;
                                continue;
                            }
                            comma = !comma;
                            break;
                        default:
                            builder.Append(item);
                            break;
                    }
                }

                values.Add(builder.ToString());

                var count = values.Count;
                if (count == 0) continue;
                list.Add(values.ToArray());
            }
            return list.ToArray();
        }

        public bool LoadFromFile(string filePath)
        {
            if (bIsLoaded)
                return true;

            bIsLoaded = true;
            bool bRet = true;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            fileds = Read(filePath);
            rows = fileds.Length;
            cols = rows > 0 ? fileds[0].Length : 0;
            if(rows > 0 && cols > 0)
            {
                for (int i = 0; i < rows; i++)
                {
                    try
                    {
                        keyidx.Add(fileds[i][0], i);
                    }
                    catch (Exception)
                    {
                        UnityEngine.Debug.Log(fileds[i][0]);
                    }
                }
                records = new DBRecord[rows];
            }
            else
            {
                bRet = false;
            }
            sw.Stop();
            UnityEngine.Debug.Log(string.Format("total: {0} ms", sw.ElapsedMilliseconds));
            return bRet;
        }


        public T FindRecordByIndex<T>(int idx) where T:DBRecord, new ()
        {
            if (idx >= records.Length)
                return null;

            if (records[idx] == null)
            {
                records[idx] = new T();
                records[idx].Parse(fileds[idx]);
            }

            return (T)records[idx];
        }

        public T FindRecord<T>(int id) where T : DBRecord, new()
        {
            return FindRecord<T>(id.ToString());
        }

        public T FindRecord<T>(string key) where T : DBRecord, new()
        {
            int idx;
            if (keyidx.TryGetValue(key, out idx))
                return FindRecordByIndex<T>(idx);

            return null;
        }
    }
}
