using System;
using System.Collections;
using System.Collections.Generic;
using Config;
using Const;
using Controllers.Subsystems.Story;
using Evidence;
using StarPlatinum;
using Tips;
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
    public partial class CommonDetectiveNotesPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_Btn_Close_Image;
		[HideInInspector] public Button m_Btn_Close_Button;

		[HideInInspector] public Image m_Btn_Maps_Image;
		[HideInInspector] public Button m_Btn_Maps_Button;

		[HideInInspector] public Image m_Btn_Tips_Image;
		[HideInInspector] public Button m_Btn_Tips_Button;

		[HideInInspector] public Image m_Btn_Evidences_Image;
		[HideInInspector] public Button m_Btn_Evidences_Button;

		[HideInInspector] public Image m_Image_Right_Image;
		[HideInInspector] public EvidenceIntroController m_Image_Right_EvidenceIntroController;

		[HideInInspector] public Image m_Image_SchematicDiagram_Image;

		[HideInInspector] public Text m_Text_Title_Text;

		[HideInInspector] public Text m_Text_Detail_Text;

		[HideInInspector] public Image m_Button_Select_Image;
		[HideInInspector] public Button m_Button_Select_Button;

		[HideInInspector] public Image m_Button_Detail_Image;
		[HideInInspector] public Button m_Button_Detail_Button;

		[HideInInspector] public Image m_Btn_Item_Image;
		[HideInInspector] public Button m_Btn_Item_Button;
		[HideInInspector] public SingleEvidenceController m_Btn_Item_SingleEvidenceController;

		[HideInInspector] public Image m_Image_Icon_Image;

		[HideInInspector] public Text m_Text_Intro_Text;

		[HideInInspector] public Image m_Image_Detial_Image;
		[HideInInspector] public TipDetailController m_Image_Detial_TipDetailController;

		[HideInInspector] public Text m_Text_Description_Text;

		[HideInInspector] public Image m_Image_Lock_Image;

		[HideInInspector] public Image m_btn_history_Image;
		[HideInInspector] public Button m_btn_history_Button;

		[HideInInspector] public RectTransform m_pl_subPanel;
		[HideInInspector] public Image m_img_unSelectedBG_Image;

		[HideInInspector] public Image m_img_SelectedBG_Image;

		[HideInInspector] public Image m_img_evidence_Image;
		[HideInInspector] public EvidenceIntroController m_img_evidence_EvidenceIntroController;

		[HideInInspector] public Image m_btn_detail_Image;
		[HideInInspector] public Button m_btn_detail_Button;

		[HideInInspector] public Image m_btn_selcet_Image;
		[HideInInspector] public Button m_btn_selcet_Button;

		[HideInInspector] public Image m_img_description_Image;

		[HideInInspector] public TextMeshProUGUI m_lbl_description_TextMeshProUGUI;


         private CommonDetectiveNotesModel m_model;

         private void UIFinder()
         {
			m_Btn_Close_Image = FindUI<Image>(transform ,"Btn_Close");
			m_Btn_Close_Button = FindUI<Button>(transform ,"Btn_Close");

			m_Btn_Maps_Image = FindUI<Image>(transform ,"Btn_Maps");
			m_Btn_Maps_Button = FindUI<Button>(transform ,"Btn_Maps");

			m_Btn_Tips_Image = FindUI<Image>(transform ,"Btn_Tips");
			m_Btn_Tips_Button = FindUI<Button>(transform ,"Btn_Tips");

			m_Btn_Evidences_Image = FindUI<Image>(transform ,"Btn_Evidences");
			m_Btn_Evidences_Button = FindUI<Button>(transform ,"Btn_Evidences");

			m_Image_Right_Image = FindUI<Image>(transform ,"Evidences/Image_Right");
			m_Image_Right_EvidenceIntroController = FindUI<EvidenceIntroController>(transform ,"Evidences/Image_Right");

			m_Image_SchematicDiagram_Image = FindUI<Image>(transform ,"Evidences/Image_Right/Image_SchematicDiagram");

			m_Text_Title_Text = FindUI<Text>(transform ,"Evidences/Image_Right/Text_Title");

			m_Text_Detail_Text = FindUI<Text>(transform ,"Evidences/Image_Right/Text_Detail");

			m_Button_Select_Image = FindUI<Image>(transform ,"Evidences/Image_Right/Button_Select");
			m_Button_Select_Button = FindUI<Button>(transform ,"Evidences/Image_Right/Button_Select");

			m_Button_Detail_Image = FindUI<Image>(transform ,"Evidences/Image_Right/Button_Detail");
			m_Button_Detail_Button = FindUI<Button>(transform ,"Evidences/Image_Right/Button_Detail");

			m_Btn_Item_Image = FindUI<Image>(transform ,"Evidences/Scroll View/Viewport/Content/Btn_Item");
			m_Btn_Item_Button = FindUI<Button>(transform ,"Evidences/Scroll View/Viewport/Content/Btn_Item");
			m_Btn_Item_SingleEvidenceController = FindUI<SingleEvidenceController>(transform ,"Evidences/Scroll View/Viewport/Content/Btn_Item");

			m_Image_Icon_Image = FindUI<Image>(transform ,"Evidences/Scroll View/Viewport/Content/Btn_Item/Image_Icon");

			m_Text_Intro_Text = FindUI<Text>(transform ,"Evidences/Scroll View/Viewport/Content/Btn_Item/Text_Intro");
			
			m_Image_Detial_Image = FindUI<Image>(transform ,"Tips/Image_Detial");
			m_Image_Detial_TipDetailController = FindUI<TipDetailController>(transform ,"Tips/Image_Detial");

			m_Text_Description_Text = FindUI<Text>(transform ,"Tips/Image_Detial/Text_Description");

			m_Image_Lock_Image = FindUI<Image>(transform ,"Tips/Image_Detial/Image_Lock");

			m_btn_history_Image = FindUI<Image>(transform ,"btn_history");
			m_btn_history_Button = FindUI<Button>(transform ,"btn_history");

			m_pl_subPanel = FindUI<RectTransform>(transform ,"pl_subPanel");
			m_img_unSelectedBG_Image = FindUI<Image>(transform ,"Evidences/Scroll View/Viewport/Content/Btn_Item/img_unSelectedBG");

			m_img_SelectedBG_Image = FindUI<Image>(transform ,"Evidences/Scroll View/Viewport/Content/Btn_Item/img_SelectedBG");

			m_img_evidence_Image = FindUI<Image>(transform ,"Evidences/left/img_evidence");
			m_img_evidence_EvidenceIntroController = FindUI<EvidenceIntroController>(transform ,"Evidences/left/img_evidence");

			m_btn_detail_Image = FindUI<Image>(transform ,"Evidences/left/img_evidence/btn_detail");
			m_btn_detail_Button = FindUI<Button>(transform ,"Evidences/left/img_evidence/btn_detail");

			m_btn_selcet_Image = FindUI<Image>(transform ,"Evidences/left/btn_selcet");
			m_btn_selcet_Button = FindUI<Button>(transform ,"Evidences/left/btn_selcet");

			m_img_description_Image = FindUI<Image>(transform ,"Evidences/left/img_description");

			m_lbl_description_TextMeshProUGUI = FindUI<TextMeshProUGUI>(transform ,"Evidences/left/img_description/lbl_description");


m_model = new CommonDetectiveNotesModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}