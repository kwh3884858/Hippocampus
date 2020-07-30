using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UnityEngine.UI;

namespace UI.Panels
{
	public partial class MapCanvasPanel : UIPanel<UIDataProvider, DataProvider>
	{
		#region UI template method
		public override void Initialize (UIDataProvider uiDataProvider, UIPanelSettings settings)
		{
			base.Initialize (uiDataProvider, settings);
			m_model.Initialize(this);	
		}

		public override void DeInitialize()
		{
			m_model.Deactivate();
			base.DeInitialize();
		}

		public override void Hide()
		{
			m_model.Hide();
			base.Hide();
		}

		public override void Deactivate()
		{
			m_model.Deactivate();
			base.Deactivate();
		}

		public override void ShowData(DataProvider data)
		{
			m_model.ShowData(data);
			base.ShowData(data);
		}

		public override void UpdateData(DataProvider data)
		{
			m_model.UpdateData(data);
			base.UpdateData(data);
        }

		public override void Tick()
		{
			m_model.Tick();
			base.Tick();
        }

        public override void LateTick()
		{
			m_model.LateTick();
			base.LateTick();
		}

		public override void SubpanelChanged(UIPanelType type, DataProvider data = null)
		{
			m_model.SubpanelChanged(type, data);
			base.SubpanelChanged(type, data);
		}

		public override void SubpanelDataChanged(UIPanelType type, DataProvider data)
		{
			m_model.SubpanelDataChanged(type, data);
			base.SubpanelDataChanged(type, data);
		}
        #endregion

        #region Member

        public Image map2l;
        public int FloorNum;//地图显示楼层数
        public GameObject gameMap;//地图总组件

        public void Init()//初始化
        {
            gameMap.SetActive(false);
            FloorNum = 1;
        }

        public void ChangeMap()
        {
            if (FloorNum == 1)
            {
                map2l.color = new Color(255, 255, 255, 0);
            }
            if (FloorNum == 2)
            {
                map2l.color = new Color(255, 255, 255, 255);
            }
        }

        public void OnBtnClose()
        {
            InvokeHidePanel();
        }

        public void OnBtnFloor1()
        {
            FloorNum = 1;
            ChangeMap();
        }

        public void OnBtnFloor2()
        {
            FloorNum = 2;
            ChangeMap();
        }
        #endregion
    }
}