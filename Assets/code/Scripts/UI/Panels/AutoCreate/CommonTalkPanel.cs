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
    public partial class CommonTalkPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_img_bg_Image;

		[HideInInspector] public Image m_img_talkBG_Image;

		[HideInInspector] public RectTransform m_pl_charactor;
		[HideInInspector] public Image m_skip_button_Image;
		[HideInInspector] public Button m_skip_button_Button;

		[HideInInspector] public Image m_img_content_Image;

		[HideInInspector] public TextMeshProUGUI m_txtPro_content_TextMeshProUGUI;

		[HideInInspector] public Image m_btn_auto_Image;
		[HideInInspector] public Button m_btn_auto_Button;

		[HideInInspector] public Image m_btn_skip_Image;
		[HideInInspector] public Button m_btn_skip_Button;

		[HideInInspector] public Image m_btn_history_Image;
		[HideInInspector] public Button m_btn_history_Button;

		[HideInInspector] public Image m_img_nameBG_Image;

		[HideInInspector] public TextMeshProUGUI m_lbl_name_TextMeshProUGUI;

		[HideInInspector] public Image m_img_artName_Image;

		[HideInInspector] public Image m_img_contentEnd_Image;
		[HideInInspector] public Animation m_img_contentEnd_Animation;

		[HideInInspector] public Image m_img_forntBG_Image;


         private CommonTalkModel m_model;

         private void UIFinder()
         {
			m_img_bg_Image = FindUI<Image>(transform ,"img_bg");

			m_img_talkBG_Image = FindUI<Image>(transform ,"img_talkBG");

			m_pl_charactor = FindUI<RectTransform>(transform ,"pl_charactor");
			m_skip_button_Image = FindUI<Image>(transform ,"button/skip_button");
			m_skip_button_Button = FindUI<Button>(transform ,"button/skip_button");

			m_img_content_Image = FindUI<Image>(transform ,"img_content");

			m_txtPro_content_TextMeshProUGUI = FindUI<TextMeshProUGUI>(transform ,"img_content/txtPro_content");

			m_btn_auto_Image = FindUI<Image>(transform ,"img_content/btn_auto");
			m_btn_auto_Button = FindUI<Button>(transform ,"img_content/btn_auto");

			m_btn_skip_Image = FindUI<Image>(transform ,"img_content/btn_skip");
			m_btn_skip_Button = FindUI<Button>(transform ,"img_content/btn_skip");

			m_btn_history_Image = FindUI<Image>(transform ,"img_content/btn_history");
			m_btn_history_Button = FindUI<Button>(transform ,"img_content/btn_history");

			m_img_nameBG_Image = FindUI<Image>(transform ,"img_content/img_nameBG");

			m_lbl_name_TextMeshProUGUI = FindUI<TextMeshProUGUI>(transform ,"img_content/img_nameBG/lbl_name");

			m_img_artName_Image = FindUI<Image>(transform ,"img_content/img_artName");

			m_img_contentEnd_Image = FindUI<Image>(transform ,"img_content/img_contentEnd");
			m_img_contentEnd_Animation = FindUI<Animation>(transform ,"img_content/img_contentEnd");

			m_img_forntBG_Image = FindUI<Image>(transform ,"img_forntBG");


m_model = new CommonTalkModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}