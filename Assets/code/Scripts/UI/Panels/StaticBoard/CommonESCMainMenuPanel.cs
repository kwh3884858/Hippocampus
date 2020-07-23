using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;

namespace UI.Panels
{
    /// <summary>
    /// 游戏ESC主菜单的控制
    /// </summary>
	public partial class CommonESCMainMenuPanel : UIPanel<UIDataProvider, DataProvider>
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

		#region Member
        public void OnClickLookMap()
        {
            InvokeHidePanel();// 隐藏界面显示
            // TODO: 查看地图
        }

        /// <summary>
        /// 显示证物列表
        /// </summary>
        public void OnClickShowEvidence()
        {
            InvokeHidePanel();// 隐藏界面显示
            UIManager.Instance().ShowStaticPanel(UIPanelType.Evidencepanel);// 显示证物列表
        }

        /// <summary>
        /// 显示tips列表
        /// </summary>
        public void OnClickShowTips()
        {
            InvokeHidePanel();// 隐藏界面显示
            UIManager.Instance().ShowStaticPanel(UIPanelType.Tipspanel);// 显示tips列表
        }

        public void OnClickContinue()
        {
            InvokeHidePanel();// 隐藏界面显示
        }

        public void OnClickSave()
        {
            InvokeHidePanel();// 隐藏界面显示
            // TODO: 保存游戏
        }

        public void OnClickLoad()
        {
            // TODO: 加载游戏
        }

        public void OnClickSetting()
        {
            InvokeHidePanel();// 隐藏界面显示
            // TODO: 设置界面
        }

        /// <summary>
        /// 点击退出游戏按钮
        /// </summary>
        public void OnClickExit()
        {
            Application.Quit();// 退出游戏
        }

		#endregion
	}
}