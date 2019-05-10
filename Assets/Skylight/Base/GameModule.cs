using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Skylight
{
	public class GameModule<T> : MonoSingleton<T> where T : GameModule<T>
	{
		public override void SingletonInit ()
		{
			transform.SetParent (GameRoot.Instance ().transform);
		}
	}
}
