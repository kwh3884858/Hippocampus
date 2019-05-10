using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skylight;
using UnityEngine.UI;
using System;

public class UITextLocalization : MonoBehaviour
{
	public string m_alias;
	void Start ()
	{
		ShowText ();
		EventManager.Instance ().RegisterCallback ((int)LogicType.Changelanguage, Changelanguage);

	}

	private bool Changelanguage (System.Object vars)
	{
		Text m_textCom = GetComponent<Text> ();
		m_textCom.text = Localization.Instance ().GetString (m_alias).Replace ("\\n", "\n");
		return true;
	}

	public void ShowText ()
	{
		Text m_textCom = GetComponent<Text> ();
		if (m_textCom != null &&
			m_textCom.text.StartsWith ("{") &&
			m_textCom.text.EndsWith ("}")) {
			// Text组件中的文本是{xxx}形式的，那就认为中间的xxx是一个别名
			m_alias = m_textCom.text.Substring (1, m_textCom.text.Length - 2);
			m_textCom.text = Localization.Instance ().GetString (m_alias).Replace ("\\n", "\n");
		}


	}
}
