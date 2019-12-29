using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum;

namespace Player
{
    public class PlayerController : MonoSingleton<PlayerController>
    {

        /// <summary>移动控制</summary>
        public MoveController m_moveCtrl = null;

        public override void SingletonInit()
        {

        }

        public void SetMoveEnable(bool isMove)
        {
            if (m_moveCtrl != null)
            {
                m_moveCtrl.SetMoveEnable(isMove);
            }
        }
    }
}

