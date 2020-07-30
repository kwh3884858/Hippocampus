using System;
using System.Collections;
using System.Collections.Generic;
using Controllers.Subsystems.Story;
using TMPro;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UI.Panels.StaticBoard.Element;
using UnityEngine;

namespace UI.Panels.StaticBoard
{
	public class Option
	{
		public string ID { get; set; }
		public string Content { get; set; }

		public Option(string id, string content)
		{
			ID = id;
			Content = content;
		}
	}
	public class OptionsPanel : UIPanel<UIDataProvider,OptionsDataProvider>
	{
		public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize(uiDataProvider, settings);
			m_optionItems = new List<OptionItem>();
		}

		public override void UpdateData(DataProvider data)
		{
			base.UpdateData(data);
			SetInfo();
		}

		private void SetInfo()
		{
			m_options = UIPanelDataProvider.Options;
			m_currentCallback = UIPanelDataProvider.Callback;
			if (m_options == null || m_options.Count <= 0)
			{
				return;
			}
			if (m_optionItems.Count < m_options.Count)
			{
				CreateOption(m_options.Count-m_optionItems.Count);
			}

			for (int i = 0; i < m_options.Count; i++)
			{
				//符合条件直接跳转
				if (m_options[i].Content == "")
				{
					CallbackTime(0.1f, () => { Callback(m_options[i].ID);});
					break;
				}
				m_optionItems[i].gameObject.SetActive(true);
				m_optionItems[i].Init(m_options[i].ID,m_options[i].Content);
			}
		}

		private void Callback(string id)
		{
			m_currentCallback?.Invoke(id);
			foreach (var item in m_optionItems)
			{
				item.gameObject.SetActive(false);
			}
			InvokeHidePanel();
		}

		private void CreateOption(int num)
		{
			for (int i = 0; i < num; i++)
			{
				var option = Instantiate(m_optionItem, m_content);
				option.OnClickItem += Callback;
				m_optionItems.Add(option);
			}
		}

		[SerializeField] private OptionItem m_optionItem;
		[SerializeField] private Transform m_content;

		private Action<string> m_currentCallback;
		private List<Option> m_options;
		private List<OptionItem> m_optionItems;
	}
}