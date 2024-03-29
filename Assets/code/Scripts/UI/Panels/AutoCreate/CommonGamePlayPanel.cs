﻿using System;
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
    public partial class CommonGamePlayPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_Btn_Map_Image;
		[HideInInspector] public Button m_Btn_Map_Button;

		[HideInInspector] public Image m_Btn_AssistantInteraction_Image;
		[HideInInspector] public Button m_Btn_AssistantInteraction_Button;

		[HideInInspector] public Image m_Btn_Evidence_Image;
		[HideInInspector] public Button m_Btn_Evidence_Button;

		[HideInInspector] public Image m_Btn_Tips_Image;
		[HideInInspector] public Button m_Btn_Tips_Button;

		[HideInInspector] public Image m_Btn_Interact_Image;
		[HideInInspector] public Button m_Btn_Interact_Button;

		[HideInInspector] public Image m_Img_ItemName_Image;

		[HideInInspector] public Text m_Text_ItemName_Text;

		[HideInInspector] public Image m_Btn_Search_Image;
		[HideInInspector] public Button m_Btn_Search_Button;

		[HideInInspector] public Image m_Img_Interactable_Tag_Image;

		[HideInInspector] public Image m_Img_Dialog_Tag_Image;


         private CommonGamePlayModel m_model;

         private void UIFinder()
         {
			m_Btn_Map_Image = FindUI<Image>(transform ,"Btn_Map");
			m_Btn_Map_Button = FindUI<Button>(transform ,"Btn_Map");

			m_Btn_AssistantInteraction_Image = FindUI<Image>(transform ,"Btn_AssistantInteraction");
			m_Btn_AssistantInteraction_Button = FindUI<Button>(transform ,"Btn_AssistantInteraction");

			m_Btn_Evidence_Image = FindUI<Image>(transform ,"Btn_Evidence");
			m_Btn_Evidence_Button = FindUI<Button>(transform ,"Btn_Evidence");

			m_Btn_Tips_Image = FindUI<Image>(transform ,"Btn_Tips");
			m_Btn_Tips_Button = FindUI<Button>(transform ,"Btn_Tips");

			m_Btn_Interact_Image = FindUI<Image>(transform ,"Btn_Interact");
			m_Btn_Interact_Button = FindUI<Button>(transform ,"Btn_Interact");

			m_Img_ItemName_Image = FindUI<Image>(transform ,"Img_ItemName");

			m_Text_ItemName_Text = FindUI<Text>(transform ,"Img_ItemName/Text_ItemName");

			m_Btn_Search_Image = FindUI<Image>(transform ,"Btn_Search");
			m_Btn_Search_Button = FindUI<Button>(transform ,"Btn_Search");

			m_Img_Interactable_Tag_Image = FindUI<Image>(transform ,"Img_Interactable_Tag");

			m_Img_Dialog_Tag_Image = FindUI<Image>(transform ,"Img_Dialog_Tag");


m_model = new CommonGamePlayModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}