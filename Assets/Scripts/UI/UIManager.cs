using System;
using System.Collections.Generic;
using System.Linq;
using Extentions;
using UI.Modules;
using UI.Panels.Providers;
using UI.Panels.Providers.DataProviders;
using UnityEngine;
using UnityEngine.Assertions;
using StarPlatinum;

namespace UI
{

    public class UIManager : MonoSingleton<UIManager>
    {

        public override void SingletonInit()
        {
        }
        public event Action OnActiveModuleActivated = null;
        public event Action OnActiveModuleDeactivate = null;

        public GameRunTimeData Data
        {
            get; private set;
        }


        public UIModule ActiveModule => m_activeModule;

        public Canvas Canvas => m_canvas;

        private void Awake()
        {
        }

        public void Initialize(GameRunTimeData data)
        {
            Data = data;
            ActivateModule<UIDataProviderBattle>(m_uiModuleStaticBoard);
            ShowModule(GameState.MainManu);
            ShowModule(GameState.Battle);
        }

        public void DeInitialize()
        {
            UnloadActiveModule();
        }

        public void Tick()
        {

            if (m_activeModule != null)
            {
                m_activeModule.Tick();
            }
            m_uiModuleStaticBoard.Tick();
        }

        public void LateTick()
        {

            if (m_activeModule != null)
            {
                m_activeModule.LateTick();
            }
            m_uiModuleStaticBoard.LateTick();
        }

        public void ShowStaticPanel(UIPanelType type,DataProvider dataProvider = null)
        {
            m_uiModuleStaticBoard.ShowPanel(type,dataProvider);
        }

        public void ShowPanel(UIPanelType type, DataProvider dataProvider = null)
        {
            m_activeModule.ShowPanel(type,dataProvider);
        }

        public void UnloadActiveModule()
        {
            OnActiveModuleDeactivate?.Invoke();
            foreach (var uiModule in m_uiModules)
            {
                if (uiModule.Value != null)
                {
                    uiModule.Value.DeInitialize();
                }
            }

            if (m_uiModuleStaticBoard != null)
            {
                m_uiModuleStaticBoard.DeInitialize();
            }

            m_uiModules.Clear();
            m_activeModule = null;
        }
        
        public void ActivatState(GameState state)
        {
            if (!m_uiModules.TryGetValue(state, out m_activeModule)||m_activeModule == null)
            {
                Debug.Log($"Can't find state : {state} in UIModules!!!");
                return;
            }
            m_activeModule.Activate();
            OnActiveModuleActivated?.Invoke();
        }

        public void OnDeactivated(GameState state)
        {
            if (m_activeModule == m_uiModules[state])
            {
                m_activeModule = null;
            }
            m_uiModules[state].Deactivate();
        }

        public bool IsPanelShow(UIPanelType type)
        {
            return m_activeModule != null && m_activeModule.IsPanelShow(type);
        }

        public void ShowModule(GameState state)
        {
            UIModule uiModule = null;
            switch (state)
            {
                case GameState.MainManu:
                    uiModule = ActivateModule<UIDataProviderMainMenu>(m_uiModuleMainMenu);
                    break;
                case GameState.Battle:
                    uiModule = ActivateModule<UIDataProviderBattle>(m_uiModuleBattle);
                    break;
            }

            m_uiModules.Add(state, uiModule);

            //TODO:Active module after state data initialized
        }

        private UIModule ActivateModule<T1>( UIModule uiModule)
    where T1 : UIDataProviderGameScene, new()
        {
            T1 dataProvider = new T1
            {
                Data = Data,
                IconProvider = LoadIconProvider(),
                SoundService = StarPlatinum.SoundService.Instance,
                StaticBoard = m_uiModuleStaticBoard,
                RolePictureProvider = new RolePictureProvider(),
                Canvas = m_canvas
            };

            uiModule.Initialize(dataProvider, StarPlatinum.PrefabManager.Instance);
            return uiModule;
        }

        public IconProvider LoadIconProvider()
        {
            Assert.IsNotNull(m_iconProvider, "IconProvider not found");
            return m_iconProvider.GetComponent<IconProvider>();
        }

        [SerializeField]
        private Canvas m_canvas;

        [SerializeField]
        private UIModuleStaticBoard m_uiModuleStaticBoard;
        [SerializeField]
        private UIModuleMainMenu m_uiModuleMainMenu;
        [SerializeField]
        private UIModuleBattle m_uiModuleBattle;

        [SerializeField]
        private GameObject m_iconProvider;

        private Dictionary<GameState,UIModule> m_uiModules = new Dictionary<GameState, UIModule>();
        private UIModule m_activeModule;
    }
}
