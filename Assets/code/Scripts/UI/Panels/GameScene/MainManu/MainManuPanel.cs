using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;
using UI.Panels.Providers.DataProviders.StaticBoard;
using StarPlatinum.Manager;
using GamePlay.Stage;
using StarPlatinum;
using StarPlatinum.EventManager;

namespace UI.Panels.GameScene.MainManu
{
	public class MainManuPanel : UIPanel<UIDataProvider, DataProvider>
	{
		public override void ShowData(DataProvider data)
		{
			base.ShowData(data);
			EventManager.Instance.AddEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);
		}

		public override void Hide()
		{
			base.Hide();
			EventManager.Instance.RemoveEventListener<PlayerLoadArchiveEvent>(OnPlayerLoadArchive);

		}

		public void ShowHud ()
		{
            //			UiDataProvider.StaticBoard.ShowTalk ("9", OnTalkEnd);
            GamePlay.Player.PlayerController.Instance().SetMoveEnable(true);
            UIManager.Instance().ShowPanel(UIPanelType.JoystickPanel);// 显示UI，wywtsest
        }

        public void HidSelef ()
		{
			base.InvokeHidePanel ();
			// 测试
			//this.gameObject.SetActive(false);
		}

		/// <summary>
		/// 点击开始游戏按钮，手动绑定
		/// </summary>
		public void OnClickStartBtn ()
		{
			//未读取存档
			if (UiDataProvider.ControllerManager.PlayerArchiveController.CurrentSaveIndex == 0)
			{
				UiDataProvider.ControllerManager.PlayerArchiveController.LoadData(0);
			}

			HidSelef ();
            //Now we don`t load scene in UI button, we use mission to manager state, and input system UI like HUD should display when player is controllable.
            GameSceneManager.Instance.LoadScene(SceneLookupEnum.World_Episode2_Pier);
            MissionSceneManager.Instance.LoadMissionScene(MissionEnum.DockByPier);
            CallbackTime (1, ShowHud);
			UIManager.Instance ().ActivatState (GameState.Battle);// 设置当前状态,wywtsest
			//StarPlatinum.PrefabManager.Instance.LoadScene (SceneLookupEnum.World_Episode2_Pier, UnityEngine.SceneManagement.LoadSceneMode.Additive);
		}

		/// <summary>
		/// 点击载入游戏按钮
		/// </summary>
		public void OnClickLoadBtn ()
		{
//			UIManager.Instance ().ShowPanel (UIPanelType.LoadGamePanel);
			UIManager.Instance().ShowStaticPanel(UIPanelType.UICommonLoadarchivePanel, new ArchiveDataProvider(){ Type = ArchivePanelType.Load});
		}

		/// <summary>
		/// 点击游戏设置按钮
		/// </summary>
		public void OnClickSettingBtn ()
		{

		}

		/// <summary>
		/// 点击制作人员列表按钮
		/// </summary>
		public void OnClickProductionStaffBtn ()
		{

		}

		/// <summary>
		/// 点击退出游戏按钮
		/// </summary>
		public void OnClickExitBtn ()
		{
			Application.Quit ();// 退出游戏
		}

		private void OnTalkEnd ()
		{
			GamePlay.Player.PlayerController.Instance ().SetMoveEnable (true);
		}

		private void OnPlayerLoadArchive(object sender, PlayerLoadArchiveEvent e)
		{
			OnClickStartBtn();
		}
	}
}