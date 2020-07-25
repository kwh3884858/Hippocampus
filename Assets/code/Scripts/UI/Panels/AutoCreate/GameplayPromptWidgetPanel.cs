using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using Const;
using Controllers.Subsystems.Story;
using StarPlatinum;
using TMPro;
using UI.Panels.Element;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UI.Panels.Providers.DataProviders.GameScene;
using UI.Panels.Providers.DataProviders.StaticBoard;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
    public partial class GameplayPromptWidgetPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_PromptWidget_Image_Image;


         private GameplayPromptWidgetModel m_model;

         private void UIFinder()
         {
			m_PromptWidget_Image_Image = FindUI<Image>(transform ,"PromptWidget_Image");


m_model = new GameplayPromptWidgetModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}