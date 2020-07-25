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
    public partial class CommonLoadArchivePanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_img_cg_Image;

		[HideInInspector] public VerticalLayoutGroup m_pl_mid_VerticalLayoutGroup;
		[HideInInspector] public Image m_pl_mid_Image;

		[HideInInspector] public RectTransform m_pl_createNewArchive;
		[HideInInspector] public Image m_btn_createNewArchive_Image;
		[HideInInspector] public HorizontalLayoutGroup m_btn_createNewArchive_HorizontalLayoutGroup;
		[HideInInspector] public Button m_btn_createNewArchive_Button;

		[HideInInspector] public TextMeshProUGUI m_lbl_title_TextMeshProUGUI;

		[HideInInspector] public Image m_btn_back_Image;
		[HideInInspector] public Button m_btn_back_Button;


         private CommonLoadArchiveModel m_model;

         private void UIFinder()
         {
			m_img_cg_Image = FindUI<Image>(transform ,"img_cg");

			m_pl_mid_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(transform ,"pl_mid");
			m_pl_mid_Image = FindUI<Image>(transform ,"pl_mid");

			m_pl_createNewArchive = FindUI<RectTransform>(transform ,"pl_mid/pl_createNewArchive");
			m_btn_createNewArchive_Image = FindUI<Image>(transform ,"pl_mid/pl_createNewArchive/btn_createNewArchive");
			m_btn_createNewArchive_HorizontalLayoutGroup = FindUI<HorizontalLayoutGroup>(transform ,"pl_mid/pl_createNewArchive/btn_createNewArchive");
			m_btn_createNewArchive_Button = FindUI<Button>(transform ,"pl_mid/pl_createNewArchive/btn_createNewArchive");

			m_lbl_title_TextMeshProUGUI = FindUI<TextMeshProUGUI>(transform ,"lbl_title");

			m_btn_back_Image = FindUI<Image>(transform ,"btn_back");
			m_btn_back_Button = FindUI<Button>(transform ,"btn_back");


m_model = new CommonLoadArchiveModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}