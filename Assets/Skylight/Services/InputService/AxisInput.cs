using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skylight
{
	public class AxisInput : BaseInput
	{
		public AxisInput (string mappingKey) : base (mappingKey, InputType.Axis)
		{
			m_inputType = InputType.Axis;
			m_value = 0;
		}
		//from -1 to 1
		protected float m_value;

		public virtual float Value {
			get {
				return Mathf.Clamp (m_value + Input.GetAxisRaw (m_mappingKey), -1f, 1f);
			}
			set {
				m_value = value;
			}
		}



	}

}
