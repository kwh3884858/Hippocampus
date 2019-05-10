using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

namespace Skylight
{
	public enum LocalizationType
	{
		CN,
		EN,
	}
	public class Localization : GameModule<Localization>
	{

		override public void SingletonInit ()
		{
			string localLangId = System.Globalization.CultureInfo.InstalledUICulture.Name;
			if (localLangId == "zh-CN") {
				//SetLanguage (LocalizationType.CN);
			}
			// else if more...
			else {
				//SetLanguage (LocalizationType.EN);
			}
		}

		public string [] m_dictLangAbbr = {
			"中文",
			"English",
		};

		public LocalizationType m_curLanguage;
		Dictionary<int, string> m_dictTextWithId = new Dictionary<int, string> ();

		Dictionary<string, string> m_dictTextWithAlias = new Dictionary<string, string> ();


		public void SetLanguage (LocalizationType languageType)
		{
			m_curLanguage = languageType;
			// 根据语言设置重载本地化表
			//	string dbPath = Application.persistentDataPath + "/"; // 语言包文件路径;
			m_dictTextWithId.Clear ();
			m_dictTextWithAlias.Clear ();
			// 从dbPath读取数据，加入两个dict
			//	Console.Instance ().Debug (dbPath);
			SQLiteHelper helper = new SQLiteHelper ("Localization.db");
			SqliteDataReader reader = helper.ExecuteQuery ("SELECT * FROM Localization");
			int idOrdinal = reader.GetOrdinal ("ID");
			int aliasOrdinal = reader.GetOrdinal ("Alias");
			int textOridinal = reader.GetOrdinal (m_curLanguage.ToString ());

			while (reader.Read ()) {
				int nId = reader.GetInt32 (idOrdinal);
				string strAlias = reader.GetString (aliasOrdinal);
				string strText = reader.GetString (textOridinal);

				m_dictTextWithId [nId] = strText;
				m_dictTextWithAlias [strAlias] = strText;
			}
			reader.Close ();
			helper.CloseConnection ();
			Debug.LogFormat ("加载了{0}条本地化记录", m_dictTextWithId.Count);
		}

		// 根据ID获取字符串
		public string GetString (int id)
		{
			if (m_dictTextWithId.ContainsKey (id)) {
				return m_dictTextWithId [id];
			}
			Debug.LogWarningFormat ("[本地化]未知的ID:{0}", id);
			return string.Format ("{{{0}_{1}}}", m_dictLangAbbr [(int)m_curLanguage], id);
		}
		// 根据别名获取字符串
		public string GetString (string alias)
		{
			if (m_dictTextWithAlias.ContainsKey (alias)) {
				return m_dictTextWithAlias [alias];
			}
			Debug.LogWarningFormat ("[本地化]未知的别名:{0}", alias);
			return string.Format ("{{{0}_{1}}}", m_dictLangAbbr [(int)m_curLanguage], alias);
		}
	}



}
