using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于存储框架中静态类型的区域，
/// 请注意不要再外部申请同一种类型的枚举，
/// 以免和现有的枚举实际int值冲突
/// Used to store statically typed regions in the framework, 
/// please be careful not to apply for the same type of enumeration externally, 
/// so as not to conflict with the existing enumeration actual int value.
/// </summary>
public class SkylightStaticData : MonoBehaviour
{

	public enum PollerType
	{
		Reloading,

		Input,
	}

}
