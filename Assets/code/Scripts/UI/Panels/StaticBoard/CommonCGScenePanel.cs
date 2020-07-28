using System.Collections;
using System.Collections.Generic;
using Config.Data;
using Controllers.Subsystems;
using DG.Tweening;
using StarPlatinum;
using StarPlatinum.EventManager;
using UI.Panels.Element;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.StaticBoard;

namespace UI.Panels
{
	public partial class CommonCGScenePanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);	
			
			m_btn_back_Button.onClick.AddListener(()=>{InvokeHidePanel();});
			EventManager.Instance.AddEventListener<CGScenePointInfoChangeEvent>(OnCGScenePointInfoChange);
		}

		public override void DeInitialize()
		{
			m_model.Deactivate();
			base.DeInitialize();
			EventManager.Instance.RemoveEventListener<CGScenePointInfoChangeEvent>(OnCGScenePointInfoChange);
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
			m_btn_back_Button.gameObject.SetActive(CgSceneController.CheckCGIsClear(m_model.SceneID));
			ClearPointItem();
			var pointInfos = m_model.GetPointInfos();
			for (int i = 0; i < pointInfos.Count; i++)
			{
				var pointInfo = pointInfos[i];
				if (m_items.Count <= i)
				{
					PrefabManager.Instance.InstantiateAsync<CGScenePointItem>("UI_Common_CGScenePoint_IItem", (result) =>
					{
						if (result.status != RequestStatus.SUCCESS)
						{
							return;
						}

						var item = result.result as CGScenePointItem;
						m_items.Add(item);
						item.ClickCallback += OnClickPoint;
						item.SetPointInfo(pointInfo);
					},m_go_interactionPoints);
					continue;
				}
				m_items[i].SetPointInfo(pointInfo);
				m_items[i].gameObject.SetActive(true);
			}
		}

		private void OnCGScenePointInfoChange(object sender, CGScenePointInfoChangeEvent e)
		{
			RefreshPointInfos();
		}

		private void ClearPointItem()
		{
			foreach (var pointItem in m_items)
			{
				pointItem.gameObject.SetActive(false);
			}
		}

		private void OnClickPoint(int pointID,CGScenePointTouchConfig config)
		{
			
			var storyID = CgSceneController.TouchPointAndGetStoryID(pointID);
			InvokeShowPanel(UIPanelType.TalkPanel,new TalkDataProvider(){ID = storyID});
		}

		#region Member

		public CGSceneController CgSceneController => UiDataProvider.ControllerManager.CGSceneController;
		private List<CGScenePointItem> m_items = new List<CGScenePointItem>();

		#endregion
	}
}