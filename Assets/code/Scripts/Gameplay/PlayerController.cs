using System.Collections;
using System.Collections.Generic;
using Cinemachine.Examples;
using UnityEngine;
using StarPlatinum;
using StarPlatinum.Base;

namespace GamePlay.Player
{
	public class PlayerController : MonoSingleton<PlayerController>
	{

		/// <summary>移动控制</summary>
		private MonoMoveController m_moveCtrl = null;

		public override void SingletonInit ()
		{

		}

		public void SetMonoMoveController (MonoMoveController moveController)
		{
			m_moveCtrl = moveController;
		}

        public void SetPlayerPosition(Transform playerPos)
        {
            m_moveCtrl.transform.position = playerPos.position;
        }
        public void SetMoveEnable (bool isMove)
		{
			if (m_moveCtrl != null) {
                //MG Version disable move
                CharacterMovement m_move = m_moveCtrl.gameObject.GetComponent<CharacterMovement>();
                if (m_move)
                {
                    m_move.SetCharacterMoveable(isMove);
                }
                //m_moveCtrl.SetMoveEnable (isMove);
            }
		}

		public void JoystickMoveEvent (Vector2 vec)
		{
			//Debug.Log(vec);
			InputService.Instance.SetAxis (KeyMap.Horizontal, vec.x);
			InputService.Instance.SetAxis (KeyMap.Vertical, vec.y);
		}

		public void JoystickMoveEndEvent ()
		{
			InputService.Instance.SetAxis (KeyMap.Horizontal, 0f);
			InputService.Instance.SetAxis (KeyMap.Vertical, 0f);

		}

		private void OnDestroy ()
		{
			if (m_moveCtrl) {
				m_moveCtrl = null;
			}
		}
	}
}

