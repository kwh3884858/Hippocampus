using System.Collections;
using System.Collections.Generic;
using StarPlatinum;
using UI.Panels.Element;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.UIComponent;

namespace UI.Panels
{
	public partial class CommonLogPanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);
			InitLogItemList();
			
			m_btn_back_Button.onClick.AddListener(OnClose);
		}

		public override void DeInitialize()
		{
			m_model.Deactivate();
			base.DeInitialize();
		}

		public override void Hide()
		{
			m_model.Hide();
			base.Hide();
		}

		public override void Deactivate()
		{
			m_model.Deactivate();
			base.Deactivate();
		}

		public override void ShowData(DataProvider data)
		{
			m_model.ShowData(data);
			base.ShowData(data);
		}

		public override void UpdateData(DataProvider data)
		{
			m_model.UpdateData(data);
			base.UpdateData(data);
			RefreshInfo();
		}

		public override void Tick()
		{
			m_model.Tick();
			base.Tick();
		}

		public override void LateTick()
		{
			m_model.LateTick();
			base.LateTick();
		}

		public override void SubpanelChanged(UIPanelType type, DataProvider data = null)
		{
			m_model.SubpanelChanged(type, data);
			base.SubpanelChanged(type, data);
		}

		public override void SubpanelDataChanged(UIPanelType type, DataProvider data)
		{
			m_model.SubpanelDataChanged(type, data);
			base.SubpanelDataChanged(type, data);
		}
		#endregion

		private void InitLogItemList()
		{
			PrefabManager.Instance.LoadAssetsAsync<GameObject>(m_sv_list_ListView.ItemPrefabDataList, (result) =>
			{
				m_listAssets = result;
				ListView.FuncTab tab =new ListView.FuncTab();
				tab.ItemEnter += InitLogItemInfo;
				m_sv_list_ListView.SetInitData(result,tab);
				m_sv_list_ListView.FillContent(m_model.GetLogNum());
			});
		}

		private void InitLogItemInfo(ListView.ListItem item)
		{
			UI_Common_Log_Item_SubView itemView = null;
			if (item.data == null)
			{
				itemView = item.go.GetComponent<UI_Common_Log_Item_SubView>();
				itemView.Init(item.go.GetComponent<RectTransform>());
				item.data = itemView;
			}
			else
			{
				itemView = item.data as UI_Common_Log_Item_SubView;
			}
			itemView.SetInfo(m_model.GetLogByIndex(item.index));
		}
		
		private void RefreshInfo()
		{
			if (m_listAssets == null)
			{
				return;
			}
			m_sv_list_ListView.FillContent(m_model.GetLogNum());
			m_sv_list_ListView.ScrollPanelToItemIndex(0);
		}

		private void OnClose()
		{
			InvokeHidePanel();
		}
		#region Member

		private Dictionary<string, GameObject> m_listAssets;

		#endregion
	}
}