using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;
using UI.Panels.Providers.DataProviders.StaticBoard;

namespace UI.Panels.GameScene.MainManu
{
    public class MainManuPanel : UIPanel<UIDataProviderGameScene, DataProvider>
    {
        public override void Initialize(UIDataProvider uiDataProvider, UIPanelSettings settings)
        {
            base.Initialize(uiDataProvider, settings);
        }


        public void ShowHud()
        {
            UiDataProvider.StaticBoard.ShowTalk("9", OnTalkEnd);
        }

        public void HidSelef()
        {
            base.InvokeHidePanel();
            // 测试
            //this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 点击开始游戏按钮，手动绑定
        /// </summary>
        public void OnClickStartBtn()
        {
            HidSelef();
            CallbackTime(1, ShowHud);
            StarPlatinum.PrefabManager.Instance.LoadScene(SceneLookupEnum.GoundTestScene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }

        private void OnTalkEnd()
        {
            Player.PlayerController.Instance().SetMoveEnable(true);
        }

        private int i = 0;
    }
}