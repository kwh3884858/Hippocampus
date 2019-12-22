using System;
using TMPro;
using UnityEngine;

namespace UI.Panels.StaticBoard.Element
{
	public class OptionItem : MonoBehaviour
	{
		public Action<string> OnClickItem;

		public void Init(string id,string content)
		{
			m_content.text = content;
			m_id = id;
		}

		public void OnClick()
		{
			OnClickItem?.Invoke(m_id);
		}

		[SerializeField]
		private TMP_Text m_content;

		private string m_id;
	}
}