using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skylight
{
	public class ButtonInput : BaseInput
	{
		public ButtonInput (string mappingKey) : base (mappingKey.ToLower (), InputType.Button)
		{

			//base (mappingKey, InputType.Button);
			//m_inputType = InputType.Button;
			//EventManager.Instance ().AddEventListener<ButtonDownEvent> (HandleEventHandler);
		}
		//from -1 to 1
		protected bool m_value = false;

		public virtual bool Value {
			get {
				return m_value || Input.GetKey (m_mappingKey);
			}
			set {
				m_value = value;
			}
		}

		//void HandleEventHandler (object sender, ButtonDownEvent e)
		//{
		//	if
		//}


	}

}
