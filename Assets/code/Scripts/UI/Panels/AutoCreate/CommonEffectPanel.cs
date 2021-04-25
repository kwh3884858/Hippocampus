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
    public partial class CommonEffectPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Animator m_pl_root_Animator;

		[HideInInspector] public RectTransform m_pl_flash;

         private CommonEffectModel m_model;

         private void UIFinder()
         {
			m_pl_root_Animator = FindUI<Animator>(transform ,"pl_root");

			m_pl_flash = FindUI<RectTransform>(transform ,"pl_root/pl_flash");

m_model = new CommonEffectModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}