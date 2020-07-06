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
    public partial class CommonCGScenePanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_img_cg_Image;

		[HideInInspector] public RectTransform m_go_interactionPoints;

         private CommonCGSceneModel m_model;

         private void UIFinder()
         {
			m_img_cg_Image = FindUI<Image>(transform ,"img_cg");

			m_go_interactionPoints = FindUI<RectTransform>(transform ,"go_interactionPoints");

m_model = new CommonCGSceneModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}