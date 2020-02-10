using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalData
{
    /// <summary>本地存储数据字符串标识</summary>
    private static string keyMark = "Hippocampus_{0}";

    /// <summary>
    /// 读取字符型信息
    /// </summary>
    /// <returns>The string.</returns>
    /// <param name="vParamName">V parameter name.</param>
    /// <param name="vDefaultStr">V default string.</param>
    public static string ReadStr(string vParamName, string vDefaultStr)
    {
        return PlayerPrefs.GetString(string.Format(keyMark, vParamName), vDefaultStr);
    }

    /// <summary>
    /// 写入字符型信息
    /// </summary>
    /// <returns>The string.</returns>
    /// <param name="vParamName">V parameter name.</param>
    /// <param name="vContent">V content.</param>
    public static void WriteStr(string vParamName, string vContent)
    {
        if (!string.IsNullOrEmpty(vParamName))
        {
            PlayerPrefs.SetString(string.Format(keyMark, vParamName), vContent);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 读取int型数据
    /// </summary>
    /// <param name="vKey"></param>
    /// <returns></returns>
    public static int ReadInt(string vKey)
    {
        return PlayerPrefs.GetInt(string.Format(keyMark, vKey));
    }

    /// <summary>
    /// 读取int型数据
    /// </summary>
    /// <param name="vKey"></param>
    /// <param name="vDefault"></param>
    /// <returns></returns>
    public static int ReadInt(string vKey, int vDefault)
    {
        return PlayerPrefs.GetInt(string.Format(keyMark, vKey), vDefault);
    }

    /// <summary>
    /// 写入int型数据
    /// </summary>
    /// <param name="vKey"></param>
    /// <param name="vValue"></param>
    public static void WriteInt(string vKey, int vValue)
    {
        if (!string.IsNullOrEmpty(vKey))
        {
            PlayerPrefs.SetInt(string.Format(keyMark, vKey), vValue);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 根据key判断本地是否有存储数据
    /// </summary>
    /// <param name="vParamName">key</param>
    /// <returns></returns>
    public static bool HasKey(string vParamName)
    {
        return PlayerPrefs.HasKey(string.Format(keyMark, vParamName));
    }
}
