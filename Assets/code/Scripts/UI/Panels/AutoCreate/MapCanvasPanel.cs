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
    public partial class MapCanvasPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 
		[HideInInspector] public Image m_Map_1L_Image;

		[HideInInspector] public Image m_Map_2L_Image;


         private MapCanvasModel m_model;

         private void UIFinder()
         {
			m_Map_1L_Image = FindUI<Image>(transform ,"MapBox/Map_1L");

			m_Map_2L_Image = FindUI<Image>(transform ,"MapBox/Map_2L");


m_model = new MapCanvasModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}