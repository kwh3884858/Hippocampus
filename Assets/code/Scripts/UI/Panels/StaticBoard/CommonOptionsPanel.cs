using System;
using System.Collections;
using System.Collections.Generic;
using Config.Data;
using Controllers.Subsystems.Story;
using StarPlatinum;
using TMPro;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UI.Panels.StaticBoard.Element;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
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
	public partial class CommonOptionsPanel : UIPanel<UIDataProvider,DataProvider>
	{
		public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize(uiDataProvider, settings);
			m_optionItems = new List<OptionItem>();
		}

		public override void UpdateData(DataProvider data)
		{
			base.UpdateData(data);
			UIPanelDataProvider = data as OptionsDataProvider;
			SetInfo();
		}

		private void SetInfo()
		{
			SetTalkPanelType();
			m_options = UIPanelDataProvider.Options;
			m_currentCallback = UIPanelDataProvider.Callback;
			if (m_options == null || m_options.Count <= 0)
			{
				m_sv_list_Image.gameObject.SetActive(false);
				return;
			}

			var option = m_options.Find(x => x.Content == "");
			if (option !=null)
			{
				CallbackTime(0.1f, () => { Callback(option.ID);});
				m_sv_list_Image.gameObject.SetActive(false);
				return;
			}
			m_sv_list_Image.gameObject.SetActive(true);
			if (m_optionItems.Count < m_options.Count)
			{
				CreateOption(m_options.Count-m_optionItems.Count);
			}

			for (int i = 0; i < m_options.Count; i++)
			{
				m_optionItems[i].gameObject.SetActive(true);
				m_optionItems[i].Init(m_options[i].ID,m_options[i].Content,m_uiType);
			}
		}

		private void SetTalkPanelType()
		{
			var type = UiDataProvider.ControllerManager.StoryController.GetTalkPanelType();
			if (m_uiType == type)
			{
				return;
			}
			
			m_uiType = type;
			var config = TalkPanelConfig.GetConfigByKey(type);
			if (config == null)
			{
				return;
			}
			PrefabManager.Instance.SetImage(m_btn_history_Image,config.historyBtnBG);
		}

		private void Callback(string id)
		{
			InvokeHidePanel();
			m_currentCallback?.Invoke(id);
			foreach (var item in m_optionItems)
			{
				item.gameObject.SetActive(false);
			}
		}

		private void CreateOption(int num)
		{
			for (int i = 0; i < num; i++)
			{
				var option = Instantiate(m_optionItem, m_pl_content_VerticalLayoutGroup.transform);
				option.OnClickItem += Callback;
				m_optionItems.Add(option);
			}
		}

		public void ShowLog()
		{
			InvokeShowAsSubpanel(PanelType,UIPanelType.UICommonLogPanel);
		}

		[SerializeField] private OptionItem m_optionItem;

		private OptionsDataProvider UIPanelDataProvider;

		private Action<string> m_currentCallback;
		private List<Option> m_options;
		private List<OptionItem> m_optionItems;
		private int m_uiType;
	}
}