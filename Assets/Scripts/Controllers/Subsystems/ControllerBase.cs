using System;
using UnityEngine;

namespace Controllers.Subsystems
{
    public class ControllerBase : MonoBehaviour
    {
        [Serializable]
        public enum SubsystemState
        {
            /// <summary>
            /// Initialization in progress and submodule still need some time
            /// </summary>
            Initialization,
            /// <summary>
            /// Initialization is failed and game can't be run anymore
            /// </summary>
            Failed,
            /// <summary>
            /// Submodule is ready to work
            /// </summary>
            Initialized,
            /// <summary>
            /// Submodule finished working and deinitialized
            /// </summary>
            Terminated,
        }
        
        public bool Initialized => State == SubsystemState.Initialized;
        public IControllerProvider Args { get; private set; }

        public GameRunTimeData Data => Args.Data;
        public SubsystemState State
        {
            get { return m_state; }
            protected set
            {
                if (m_state == value)
                {
                    return;
                }
                Debug.Log(this + ". State " + m_state + " => " + value);
                m_state = value;
            }
        }
        
        public virtual void Initialize(IControllerProvider args)
        {
            Args = args;
            State = SubsystemState.Initialized;
        }


        
        public virtual void Tick()
        {
            
        }
        public virtual void Terminate()
        {
            State = SubsystemState.Terminated;
        }
        
        private SubsystemState m_state = SubsystemState.Terminated;
    }
}