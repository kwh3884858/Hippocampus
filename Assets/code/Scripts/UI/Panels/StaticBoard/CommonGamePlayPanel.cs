using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using GamePlay.Stage;
using UnityEngine.UI;
using StarPlatinum.EventManager;
using static UI.Utils.UIHelper;
using System;
using Config.GameRoot;
using StarPlatinum.Services;

namespace UI.Panels
{
	public class CommonGamePlayPanelUpdateDataEvent : EventArgs
	{
		public bool m_interactButtonShouldVisiable = false;
		public string m_itemName = null;
	}

	public partial class CommonGamePlayPanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);
			EventManager.Instance.AddEventListener<CursorEvent>(OnCursorEvent);
			EventManager.Instance.AddEventListener<CommonGamePlayPanelUpdateDataEvent>(OnDataUpdateEvent);
		}

        public override void DeInitialize()
		{
			m_model.Deactivate();
			base.DeInitialize();
			EventManager.Instance.RemoveEventListener<CursorEvent>(OnCursorEvent);
		}

        private void OnCursorEvent(object sender, CursorEvent e)
        {
			m_model.m_isInteractButtonVisiable = true;
        }

		private void OnDataUpdateEvent(object sender, CommonGamePlayPanelUpdateDataEvent e)
		{
			m_model.m_isInteractButtonVisiable = e.m_interactButtonShouldVisiable;
			m_model.m_itemName = e.m_itemName;
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
		}

		public override void Tick()
		{
			m_model.Tick();
			base.Tick();

			UpdateUIInteractButtonVisiable();
		}

        private void UpdateUIInteractButtonVisiable()
        {
			if (m_model.m_isNowInteractButtonIsVisiableCache != m_model.m_isInteractButtonVisiable)
			{
				UpdateButtonVisiable(m_model.m_isInteractButtonVisiable);
				m_model.m_isNowInteractButtonIsVisiableCache = m_model.m_isInteractButtonVisiable;
				m_Text_ItemName_Text.text = m_model.m_itemName;
			}
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

		#region Member
        public void OnClickMap()
        {
            UIManager.Instance().ShowStaticPanel(UIPanelType.UIMapcanvasPanel);// 显示地图
        }

        public void OnClickAssistantInteraction()
        {
            // TODO: 助手互动
        }

        public void OnClickEvidence()
        {
            UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonDetectiveNotesPanel);// 显示证物列表
        }

        public void OnClickTips()
        {
            UIManager.Instance().ShowStaticPanel(UIPanelType.Tipspanel);// 显示tips列表
        }

		public void OnClickInteract ()
		{
			//Use new input system
			//CoreContainer.Instance.EnablePlayerInteractability ();
		}

		public void OnSearch() 
		{
			
			List<Collider > interactions = m_model.m_findColliderDetection.OverlapSphereDetectionFindCollider(
				CoreContainer.Instance.GetPlayerPosition(), 
				ConfigPlayer.Instance.interactableRadius, 
				ConfigPlayer.Instance.interactableLayer, 
				ConfigPlayer.Instance.maxColliders);
            foreach (var item in interactions)
            {
				Vector3 screenPos = CameraService.Instance.GetMainCamera().GetComponent<Camera>().WorldToScreenPoint(item.transform.position);
				GameObject tag = Instantiate(m_Img_Dialog_Tag_Image.gameObject);
				tag.transform.position = screenPos;
				tag.transform.SetParent(gameObject.transform);
				StartCoroutine(CloseTagAfterTime(5.0f, tag));
			}
		}

		IEnumerator CloseTagAfterTime(float deltaTime, GameObject tag)
        {
			if (deltaTime > 0)
			{
				yield return new WaitForSeconds(deltaTime);
			}
			Destroy(tag);
        }

        public void UpdateButtonVisiable(bool isVisiable)
        {
            if (GamePlay.Global.SingletonGlobalDataContainer.Instance.PlatformCtrl.IsMobile)
            {
				m_Btn_Interact_Image.gameObject.SetActive(isVisiable);
			}
			m_Img_ItemName_Image.gameObject.SetActive(isVisiable);
        }
		#endregion
	}
}