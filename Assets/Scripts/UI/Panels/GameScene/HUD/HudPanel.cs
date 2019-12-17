using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;

namespace UI.Panels.GameScene.HUD
{
	public class HudPanel : UIPanel<UIDataProviderGameScene,HudDataProvider>
	{
		public override void UpdateData(DataProvider data)
		{
			base.UpdateData(data);
			Debug.Log(UIPanelDataProvider.Data);
			InvokeShowAsSubpanel(PanelType,UIPanelType.HudSubpanelPanel);
			CallbackTime(2,OpenSubPanel);
			CallbackTime(4,ClosePanel);
		}
		

		public void OpenSubPanel()
		{
			InvokeShowAsSubpanel(PanelType,UIPanelType.HudSubpanelTwoPanel);
		}

		public void ClosePanel()
		{
			InvokeHidePanel();
		}

		public void Refresh()
		{
			
		}
	}
}