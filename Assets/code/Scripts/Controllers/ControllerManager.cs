using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Controllers.Subsystems;
using Controllers.Subsystems.Role;
using Controllers.Subsystems.Story;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

namespace Controllers
{
    public enum ControllerinitializedStep
    {
        Success,
        CheckInitializedTerminated,
        CheckInitializedInited,
    }
    public class ControllerManager: MonoBehaviour,IControllerProvider
    {
        public GameRunTimeData Data { get; private set; }
        public PlayerArchiveController PlayerArchiveController { get; private set; }
        public StoryController StoryController { get; private set; }
        public CGSceneController CGSceneController { get; private set; }

        public Action OnInitialized;
        public void Awake()
        {
            PlayerArchiveController = RegisterController<PlayerArchiveController>();
            StoryController = RegisterController<StoryController>();
            CGSceneController = RegisterController<CGSceneController>();
        }

        public async Task<ControllerinitializedStep> InitializedControllers(GameRunTimeData data)
        {
            Data = data;
            foreach (var controller in m_controllers)
            {
                controller.Initialize(this);
            }
            while (m_controllers.Any(c => c.State == ControllerBase.SubsystemState.Initialization))
            {
                await Task.Delay(m_controllerInitializeWaitingTime);
            }
            if (m_controllers.Any(c => c.State == ControllerBase.SubsystemState.Terminated))
            {
                TerminateEverything("controller initialized terminated");
                return ControllerinitializedStep.CheckInitializedTerminated;
            }
            if (m_controllers.Any(c => c.State != ControllerBase.SubsystemState.Initialized))
            {
                string error = "";
                foreach (var subsystem in m_controllers)
                {
                    error += $"{subsystem.GetType().Name}: {subsystem.State}\n";
                }
                Debug.LogError($"Some controller initialization failed\n{error}");
                TerminateEverything("controller initialization failed. See message above");
                return ControllerinitializedStep.CheckInitializedInited;
            }
            OnInitialized?.Invoke();
            return ControllerinitializedStep.Success;
        }

        public void Tick()
        {
            foreach (var controller in m_controllers)
            {
                controller.Tick();
            }
        }
        
        public void TerminateEverything(string reason, bool error = false)
        {
            if (error)
            {
                Log($"Network terminated with reason: {reason}");
            }
            foreach (var subsystem in m_controllers)
            {
                subsystem.Terminate();
            }
        }
        
        private void OnDestroy()
        {
            m_controllers.Clear();
        }

        private T RegisterController<T>()
            where T : ControllerBase
        {
            T controller = GetComponent<T>();
            Assert.IsNotNull(controller, typeof(T) + " not found on ControllerManager");
            m_controllers.Add(controller);
            return controller;
        }
        
        protected void Log(string msg)
        {
            Debug.Log($"NetworkManager:{msg}");
        }

        [SerializeField] private int m_controllerInitializeWaitingTime;
        
        private List<ControllerBase> m_controllers = new List<ControllerBase>();

    }
}