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
    public partial class CommonEndPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_btn_close_Image;
		[HideInInspector] public Button m_btn_close_Button;


         private CommonEndModel m_model;

         private void UIFinder()
         {
			m_btn_close_Image = FindUI<Image>(transform ,"bg/btn_close");
			m_btn_close_Button = FindUI<Button>(transform ,"bg/btn_close");


m_model = new CommonEndModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}