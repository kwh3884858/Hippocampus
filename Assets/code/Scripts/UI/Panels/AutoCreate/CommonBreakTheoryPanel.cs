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
		[HideInInspector] public RectTransform m_pl_breakTheory;
		[HideInInspector] public Image m_go_bg_Image;

		[HideInInspector] public Animation m_go_breakTheory_Animation;

		[HideInInspector] public Image m_img_theoryBreak_Image;

		[HideInInspector] public Animation m_go_theory_Animation;

		[HideInInspector] public Image m_img_theory_Image;

		[HideInInspector] public Image m_img_bg_Image;


         private CommonBreakTheoryModel m_model;

         private void UIFinder()
         {
			m_pl_breakTheory = FindUI<RectTransform>(transform ,"pl_breakTheory");
			m_go_bg_Image = FindUI<Image>(transform ,"pl_breakTheory/go_bg");

			m_go_breakTheory_Animation = FindUI<Animation>(transform ,"pl_breakTheory/go_breakTheory");

			m_img_theoryBreak_Image = FindUI<Image>(transform ,"pl_breakTheory/go_breakTheory/img_theoryBreak");

			m_go_theory_Animation = FindUI<Animation>(transform ,"pl_breakTheory/go_theory");

			m_img_theory_Image = FindUI<Image>(transform ,"pl_breakTheory/go_theory/img_theory");

			m_img_bg_Image = FindUI<Image>(transform ,"img_bg");


m_model = new CommonBreakTheoryModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}