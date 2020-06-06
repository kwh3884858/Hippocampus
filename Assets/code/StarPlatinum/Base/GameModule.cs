using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using StarPlatinum.Base;

namespace StarPlatinum
{
	public class GameModule<T> : MonoSingleton<T> where T : GameModule<T>
	{
		public override void SingletonInit ()
		{
			transform.SetParent (GameRoot.Instance ().transform);
		}
	}
}
