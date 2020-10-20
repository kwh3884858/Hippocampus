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
    public partial class CommonBookMarkPanel : UIPanel<UIDataProvider, DataProvider>
    {
         #region gen ui code 

         private CommonBookMarkModel m_model;

         private void UIFinder()
         {

m_model = new CommonBookMarkModel ();

         }
         #endregion
   	
         public void Start () {
               UIFinder();
         }
    }
}