using System.Collections;
using System.Collections.Generic;
using Config.Data;
using StarPlatinum;
using UI.Panels.Element;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;

namespace UI.Panels
{
	public partial class CommonCGScenePanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);	
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

		private void RefreshInfo()
		{
			var config = m_model.SceneInfo;
			PrefabManager.Instance.SetImage(m_img_cg_Image,config.CGKey);
			RefreshPointInfos();
		}

		private void RefreshPointInfos()
		{
			ClearPointItem();
			var pointInfos = m_model.GetPointInfos();
			for (int i = 0; i < pointInfos.Count; i++)
			{
				if (m_items.Count >= i)
				{
					PrefabManager.Instance.InstantiateAsync<CGScenePointItem>("CGScenePointItem", (result) =>
					{
						if (result.status != RequestStatus.SUCCESS)
						{
							return;
						}

						var item = result.result as CGScenePointItem;
						m_items.Add(item);
						item.SetPointInfo(pointInfos[i]);
					},m_go_interactionPoints);
					continue;
				}
				m_items[i].SetPointInfo(pointInfos[i]);
				m_items[i].gameObject.SetActive(true);
			}
		}
		
		

		private void ClearPointItem()
		{
			foreach (var pointItem in m_items)
			{
				pointItem.gameObject.SetActive(false);
			}
		}

		#region Member

		private List<CGScenePointItem> m_items = new List<CGScenePointItem>();

		#endregion
	}
}