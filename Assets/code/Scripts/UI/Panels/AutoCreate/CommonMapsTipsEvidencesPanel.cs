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
    public partial class CommonMapsTipsEvidencesPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_Btn_Close_Image;
		[HideInInspector] public Button m_Btn_Close_Button;


         private CommonMapsTipsEvidencesModel m_model;

         private void UIFinder()
         {
			m_Btn_Close_Image = FindUI<Image>(transform ,"Btn_Close");
			m_Btn_Close_Button = FindUI<Button>(transform ,"Btn_Close");


m_model = new CommonMapsTipsEvidencesModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}