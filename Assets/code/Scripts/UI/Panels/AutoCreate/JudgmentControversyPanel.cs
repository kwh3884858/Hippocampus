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
    public partial class JudgmentControversyPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_img_bg_Image;

		[HideInInspector] public RectTransform m_go_bg;
		[HideInInspector] public RectTransform m_go_hero;
		[HideInInspector] public UI_Judgment_ControversyCharactor_Item_SubView m_UI_Judgment_ControversyCharactor_Item;
		[HideInInspector] public RectTransform m_go_enemy;
		[HideInInspector] public Image m_img_enemy_Image;

		[HideInInspector] public RectTransform m_pl_line;
		[HideInInspector] public RectTransform m_go_cordon;
		[HideInInspector] public Image m_img_cordon_Image;

		[HideInInspector] public VerticalLayoutGroup m_go_barrage_VerticalLayoutGroup;

		[HideInInspector] public RectTransform m_go_beginLine1;
		[HideInInspector] public RectTransform m_go_beginLine2;
		[HideInInspector] public RectTransform m_go_beginLine3;
		[HideInInspector] public RectTransform m_go_beginLine4;
		[HideInInspector] public RectTransform m_pl_screen;
		[HideInInspector] public Image m_img_screenLeft_Image;

		[HideInInspector] public Image m_img_screenRight_Image;

		[HideInInspector] public Image m_img_controversyText_Image;

		[HideInInspector] public Image m_img_heavyAttackCounter_Image;

		[HideInInspector] public TextMeshProUGUI m_lbl_heavyAttackCounter_TextMeshProUGUI;


         private JudgmentControversyModel m_model;

         private void UIFinder()
         {
			m_img_bg_Image = FindUI<Image>(transform ,"img_bg");

			m_go_bg = FindUI<RectTransform>(transform ,"go_bg");
			m_go_hero = FindUI<RectTransform>(transform ,"go_hero");
			m_UI_Judgment_ControversyCharactor_Item = FindUI<UI_Judgment_ControversyCharactor_Item_SubView>(transform ,"go_hero/UI_Judgment_ControversyCharactor_Item");
			m_UI_Judgment_ControversyCharactor_Item.Init(FindUI<RectTransform>(transform ,"go_hero/UI_Judgment_ControversyCharactor_Item"));
			m_go_enemy = FindUI<RectTransform>(transform ,"go_enemy");
			m_img_enemy_Image = FindUI<Image>(transform ,"go_enemy/img_enemy");

			m_pl_line = FindUI<RectTransform>(transform ,"pl_line");
			m_go_cordon = FindUI<RectTransform>(transform ,"pl_line/go_cordon");
			m_img_cordon_Image = FindUI<Image>(transform ,"pl_line/go_cordon/img_cordon");

			m_go_barrage_VerticalLayoutGroup = FindUI<VerticalLayoutGroup>(transform ,"pl_line/go_barrage");

			m_go_beginLine1 = FindUI<RectTransform>(transform ,"pl_line/go_barrage/go_beginLine1");
			m_go_beginLine2 = FindUI<RectTransform>(transform ,"pl_line/go_barrage/go_beginLine2");
			m_go_beginLine3 = FindUI<RectTransform>(transform ,"pl_line/go_barrage/go_beginLine3");
			m_go_beginLine4 = FindUI<RectTransform>(transform ,"pl_line/go_barrage/go_beginLine4");
			m_pl_screen = FindUI<RectTransform>(transform ,"pl_screen");
			m_img_screenLeft_Image = FindUI<Image>(transform ,"pl_screen/img_screenLeft");

			m_img_screenRight_Image = FindUI<Image>(transform ,"pl_screen/img_screenRight");

			m_img_controversyText_Image = FindUI<Image>(transform ,"pl_screen/img_controversyText");

			m_img_heavyAttackCounter_Image = FindUI<Image>(transform ,"img_heavyAttackCounter");

			m_lbl_heavyAttackCounter_TextMeshProUGUI = FindUI<TextMeshProUGUI>(transform ,"img_heavyAttackCounter/lbl_heavyAttackCounter");


m_model = new JudgmentControversyModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}