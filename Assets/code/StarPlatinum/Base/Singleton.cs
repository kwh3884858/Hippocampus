
using System;

namespace StarPlatinum.Base
{
	public abstract class Singleton<T> where T : class, new()
	{
		public Singleton () { }

		private static readonly Lazy<T> m_instance = new Lazy<T> (() => new T ());

		public static T Instance { get { return m_instance.Value; } }
	}
}


