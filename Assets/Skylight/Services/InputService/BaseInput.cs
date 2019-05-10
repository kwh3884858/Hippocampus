using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skylight
{
	public class BaseInput
	{
		//public string DeivceType = "";

		protected string m_mappingKey;

		protected InputType m_inputType = InputType.UnKnown;

		//protected DeviceType m_deviceType = DeviceType.Unknown;



		public BaseInput (string mappingKey, InputType input)
		{
			m_inputType = input;
			//m_deviceType = device;
			m_mappingKey = mappingKey;
		}


		//public virtual DeviceType GetDeviceType ()
		//{
		//	return m_deviceType;
		//}

		public virtual InputType GetInputeType ()
		{
			return m_inputType;
		}
	}
}

