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
    public partial class CommonGamePlayTransitionPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_img_Mask_Image;
		[HideInInspector] public Animation m_img_Mask_Animation;


         private CommonGamePlayTransitionModel m_model;

         private void UIFinder()
         {
			m_img_Mask_Image = FindUI<Image>(transform ,"img_Mask");
			m_img_Mask_Animation = FindUI<Animation>(transform ,"img_Mask");


m_model = new CommonGamePlayTransitionModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}