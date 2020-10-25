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
using UI.UIComponent;

namespace UI.Panels
{
    public partial class CommonBreakTheoryPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_img_bg_Image;


         private CommonBreakTheoryModel m_model;

         private void UIFinder()
         {
			m_img_bg_Image = FindUI<Image>(transform ,"img_bg");


m_model = new CommonBreakTheoryModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}