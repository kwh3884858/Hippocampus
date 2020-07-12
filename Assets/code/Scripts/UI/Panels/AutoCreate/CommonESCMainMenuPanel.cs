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
    public partial class CommonESCMainMenuPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_Btn_LookMap_Image;
		[HideInInspector] public Button m_Btn_LookMap_Button;

		[HideInInspector] public Image m_Btn_ShowEvidence_Image;
		[HideInInspector] public Button m_Btn_ShowEvidence_Button;

		[HideInInspector] public Image m_Btn_ShowTips_Image;
		[HideInInspector] public Button m_Btn_ShowTips_Button;

		[HideInInspector] public Image m_Btn_Save_Image;
		[HideInInspector] public Button m_Btn_Save_Button;

		[HideInInspector] public Image m_Btn_Load_Image;
		[HideInInspector] public Button m_Btn_Load_Button;

		[HideInInspector] public Image m_Btn_Setting_Image;
		[HideInInspector] public Button m_Btn_Setting_Button;

		[HideInInspector] public Image m_Btn_Exit_Image;
		[HideInInspector] public Button m_Btn_Exit_Button;


         private CommonESCMainMenuModel m_model;

         private void UIFinder()
         {
			m_Btn_LookMap_Image = FindUI<Image>(transform ,"Btn_LookMap");
			m_Btn_LookMap_Button = FindUI<Button>(transform ,"Btn_LookMap");

			m_Btn_ShowEvidence_Image = FindUI<Image>(transform ,"Btn_ShowEvidence");
			m_Btn_ShowEvidence_Button = FindUI<Button>(transform ,"Btn_ShowEvidence");

			m_Btn_ShowTips_Image = FindUI<Image>(transform ,"Btn_ShowTips");
			m_Btn_ShowTips_Button = FindUI<Button>(transform ,"Btn_ShowTips");

			m_Btn_Save_Image = FindUI<Image>(transform ,"Btn_Save");
			m_Btn_Save_Button = FindUI<Button>(transform ,"Btn_Save");

			m_Btn_Load_Image = FindUI<Image>(transform ,"Btn_Load");
			m_Btn_Load_Button = FindUI<Button>(transform ,"Btn_Load");

			m_Btn_Setting_Image = FindUI<Image>(transform ,"Btn_Setting");
			m_Btn_Setting_Button = FindUI<Button>(transform ,"Btn_Setting");

			m_Btn_Exit_Image = FindUI<Image>(transform ,"Btn_Exit");
			m_Btn_Exit_Button = FindUI<Button>(transform ,"Btn_Exit");


m_model = new CommonESCMainMenuModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}