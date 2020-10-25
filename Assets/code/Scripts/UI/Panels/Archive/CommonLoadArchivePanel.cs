using System.Collections;
using System.Collections.Generic;
using Controllers.Subsystems.Role;
using LocalCache;
using StarPlatinum;
using StarPlatinum.EventManager;
using UI.Panels.Element;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using System;

namespace UI.Panels
{
	public enum ArchivePanelType
	{
		Load,
		Save
	}
	public partial class CommonLoadArchivePanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);	
			
			m_btn_createNewArchive_Button.onClick.AddListener(OnClickCreateNewArchive);
			m_btn_back_Button.onClick.AddListener(OnClosePanel);
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
			EventManager.Instance.RemoveEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
			EventManager.Instance.RemoveEventListener<PlayerSaveArchiveEvent>(OnPlayerSaveArchive);
			EventManager.Instance.RemoveEventListener<PlayerDeleteArchiveEvent>(OnPlayerDeleteArchive);

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
			EventManager.Instance.AddEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
			EventManager.Instance.AddEventListener<PlayerSaveArchiveEvent>(OnPlayerSaveArchive);
			EventManager.Instance.AddEventListener<PlayerDeleteArchiveEvent>(OnPlayerDeleteArchive);

   //         Only show back button when it is save archive.
   //         m_data = data as ArchiveDataProvider;
			//if (m_data != null) {
   //             if (m_data.Type == ArchivePanelType.Load)
   //             {
   //                 m_btn_back_Button.gameObject.SetActive(false);
   //             }
   //             else
   //             {
   //                 m_btn_back_Button.gameObject.SetActive(true);
   //             }
   //         }
               
		}

		public override void UpdateData(DataProvider data)
		{
			m_model.UpdateData(data);
			base.UpdateData(data);
			m_data = data as ArchiveDataProvider;
			if (m_data == null)
			{
				m_data = new ArchiveDataProvider(){Type = ArchivePanelType.Load};
			}
			RefreshPanel();
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

		private void RefreshPanel()
		{
			m_pl_createNewArchive.gameObject.SetActive(false);

			if (m_data.Type == ArchivePanelType.Load)
			{
				LoadArchiveInfo();
				m_lbl_title_TextMeshProUGUI.text = "载入存档";
			}
			else
			{
				m_lbl_title_TextMeshProUGUI.text = "储存进度";
				LoadArchiveInfo();
			}
		}
		
		private void LoadArchiveInfo()
		{
			var previewData = PlayerArchiveController.ArchivePreviewData;
			if (m_data.Type == ArchivePanelType.Save && previewData.ArchivePreviewData.Count < 3)
			{
				m_pl_createNewArchive.gameObject.SetActive(true);
			}

			foreach (var item in m_items)
			{
				item.gameObject.SetActive(false);
			}

			int i = 0;
			foreach (var data in previewData.ArchivePreviewData)
			{
				if (m_items.Count > i)
				{
					m_items[i].SetInfo(i, data, m_data.Type);
					m_items[i].gameObject.SetActive(true);
				}
				else
				{
					PrefabManager.Instance.InstantiateComponentAsync<UI_Common_LoadArchive_Item_SubView>(
						"UI_Common_LoadArchive_Item",
						(result) =>
						{
							if (result.status != RequestStatus.SUCCESS)
							{
								return;
							}

							var itemView = result.result as UI_Common_LoadArchive_Item_SubView;
							itemView.Init(itemView.GetComponent<RectTransform>());
							m_items.Add(itemView);
							itemView.SetInfo(m_items.Count-1, data, m_data.Type);
						}, m_pl_mid_Image.transform);
				}
				i++;
			}
		}

		private void OnPlayerLoadArchive(object sender, PlayerLoadArchiveEvent e)
		{
			InvokeHidePanel();
		}
		private void OnPlayerSaveArchive(object sender, PlayerSaveArchiveEvent e)
		{
			RefreshPanel();
		}
        private void OnPlayerDeleteArchive(object sender, PlayerDeleteArchiveEvent e)
        {
            RefreshPanel();
        }

        private void OnClickCreateNewArchive()
		{
			PlayerArchiveController.SaveData(PlayerArchiveController.ArchivePreviewData.ArchivePreviewData.Count);
		}

		private void OnClosePanel()
		{
			InvokeHidePanel();
		}

		#region Member

		public PlayerArchiveController PlayerArchiveController =>
			UiDataProvider.ControllerManager.PlayerArchiveController;
		private List<UI_Common_LoadArchive_Item_SubView> m_items = new List<UI_Common_LoadArchive_Item_SubView>();
		private ArchiveDataProvider m_data;

		#endregion
	}
}