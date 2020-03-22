using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;
using UI.Panels.Providers.DataProviders.StaticBoard;

namespace UI.Panels.GameScene.LoadGame
{
    /// <summary>
    /// @author wyw
    /// @since 2020-3-22
    /// 载入游戏界面
    /// </summary>
    public class LoadGamePanel : UIPanel<UIDataProviderGameScene, DataProvider>
    {
        public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            base.Initialize(uiDataProvider, settings);
        }

        /// <summary>
        /// 显示时调用
        /// </summary>
        public override void ShowData(DataProvider data)
        {
            base.ShowData(data);
        }

        public void HidSelef()
        {
            base.InvokeHidePanel();
        }

        /// <summary>
        /// 点击返回按钮
        /// </summary>
        public void OnClickReturnBtn()
        {
            // TODO:目前返回主界面
            //UIManager.Instance().ActivatState(GameState.MainManu);
            UIManager.Instance().ShowPanel(UIPanelType.MainManuPanel);
            HidSelef();
        }

    }
}