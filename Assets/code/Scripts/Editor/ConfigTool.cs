using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace code.Scripts.Editor
{
    public class ConfigTool
    {
        [MenuItem("Tools/同步配置")]
        public static void GenConfig()
        {
            if (Directory.Exists(m_configPath)){  
                DirectoryInfo direction = new DirectoryInfo(m_configPath);  
                FileInfo[] files = direction.GetFiles("*",SearchOption.AllDirectories);  
  
                Debug.Log(files.Length);  
  
                for(int i=0;i<files.Length;i++){  
                    if (!files[i].Name.EndsWith(".csv")){  
                        continue;  
                    }
                    string json = CsvRead(files[i].FullName);
                    string jsonName = files[i].Name.Replace(".csv", ".json");
                    FileStream fs = new FileStream(m_jsonPath+jsonName, FileMode.Create);
                    //存储时时二进制,所以这里需要把我们的字符串转成二进制
                    byte[] bytes = new UTF8Encoding().GetBytes(json);
                    fs.Write(bytes, 0, bytes.Length);
                    //每次读取文件后都要记得关闭文件
                    fs.Close();
                }
            }  
        }

        /// <summary>  
        /// 将Csv读入DataTable  及返回json数据
        /// </summary>  
        /// <param name="filePath">csv文件路径</param>  
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>  
        /// <param name="k">可选参数表示最后K行不算记录默认0</param>  
        public static string CsvRead(string filePath) 
        {
            DataTable dt = new DataTable();
            String csvSplitBy = "(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)";
            StreamReader reader = new StreamReader(filePath, System.Text.Encoding.Default, false);
            List<Type> types = new List<Type>();
            int i = 0, m = 0;
            reader.Peek();
            while (reader.Peek() > 0)
            {
                ++m;
                string str = reader.ReadLine();
                if (m == 1) //如果是字段行，则自动加入字段。  
                {
                    MatchCollection mcs = Regex.Matches(str, csvSplitBy);
                    foreach (Match mc in mcs)
                    {
                        dt.Columns.Add(mc.Value); //增加列标题  
                    }
                }
                else if (m == 3)
                {
                    MatchCollection mcs = Regex.Matches(str, csvSplitBy);
                    foreach (Match mc in mcs)
                    {
                        types.Add(Type.GetType(GetType(mc.Value)));
                    }
                }
                else if(m>=5)
                {
                    MatchCollection mcs = Regex.Matches(str, "(?<=^|,)(\"(?:[^\"]|\"\")*\"|[^,]*)");
                    i = 0;
                    System.Data.DataRow dr = dt.NewRow();
                    foreach (Match mc in mcs)
                    {
                        dr[i] = mc.Value;
                        i++;
                    }

                    dt.Rows.Add(dr); //DataTable 增加一行       
                }
            }

            reader.Close();
            string excelJson = DataTableToJson(dt,types);
            Console.WriteLine(excelJson);
            return excelJson;
        }
        
        #region 帮助类Datatable转换为Json
        /// <summary>     
        /// Datatable转换为Json     
        /// </summary>    
        /// <param name="table">Datatable对象</param>     
        /// <returns>Json字符串</returns>     
        public static string DataTableToJson(DataTable dt,List<Type> types)
        {
            StringBuilder jsonString = new StringBuilder();
            jsonString.Append("{");
            DataRowCollection drc = dt.Rows;
            for (int i = 0; i < drc.Count; i++)
            {
                string strValue = drc[i][0].ToString();
                Type type = dt.Columns[0].DataType;
                strValue = StringFormat(strValue, type);
                jsonString.Append( strValue + ":{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    string strKey = dt.Columns[j].ColumnName;
                    strValue = drc[i][j].ToString();
                    type = dt.Columns[j].DataType;
                    jsonString.Append("\"" + strKey + "\":");
                    if (types[j] == typeof(List<string>)|| types[j] == typeof(List<int>))
                    {
                        var str = strValue.Split('|');
                        if (type.Name.Contains("string"))
                        {
                            type = typeof(string);
                        }
                        else
                        {
                            type = typeof(int);
                        }
                        jsonString.Append("[");
                        for (int m =0;m<str.Length;m++)
                        {
                            jsonString.Append(StringFormat(str[m], type));
                            if (m < str.Length - 1)
                            {
                                jsonString.Append(",");
                            }
                        }
                        jsonString.Append("]");
                    }
                    else
                    {
                        strValue = StringFormat(strValue, type);
                        jsonString.Append(strValue);
                    }
                    if (j < dt.Columns.Count - 1)
                    {
                        jsonString.Append(",");
                    }
                }
                jsonString.Append("},");
            }
            jsonString.Remove(jsonString.Length - 1, 1);
            jsonString.Append("}");
            return jsonString.ToString();
        } 
        #endregion
 
        #region 帮助类格式化字符型、日期型、布尔型StringFormat
        /// <summary>
        /// 格式化字符型、日期型、布尔型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static string StringFormat(string str, Type type)
        {
            if (type == typeof(string))
            {
                str = String2Json(str);
                str = "\"" + str + "\"";
            }
            else if (type == typeof(DateTime))
            {
                str = "\"" + str + "\"";
            }
            else if (type == typeof(bool))
            {
                str = str.ToLower();
            }
            else if (type != typeof(string) && string.IsNullOrEmpty(str))
            {
                str = "\"" + str + "\"";
            }
            return str;
        } 
        #endregion
        #region 过滤特殊字符String2Json(String s)
        
        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns>json字符串</returns>
        private static string String2Json(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }
        #endregion

        private static string GetType(string type)
        {
            if (type.Contains("List"))
            {
                if (type.Contains("String"))
                {
                    return "System.Collections.Generic.List`1[System.String]";
                }else if (type.Contains("Int32"))
                {
                    return "System.Collections.Generic.List`1[System.Int32]";
                }
                else
                {
                    Debug.LogError($"类型转换错误: {type}");
                }
            }
            return "System." + type;
        }
        
        private static string m_configPath = 
            System.Environment.CurrentDirectory+"/Config";
        private static string m_jsonPath = Application.dataPath + "/data/configData/";
    }
}