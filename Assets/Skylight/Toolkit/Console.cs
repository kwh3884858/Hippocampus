using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Skylight
{
	public class Console : GameModule<Console>
	{
		//GameObject
		SkylightConsole m_skylightConsole;

		public override void SingletonInit ()
		{
			base.SingletonInit ();
			UIManager.Instance ().ShowPanel<SkylightConsole> (true, () => {
				m_skylightConsole = UIManager.Instance ().GetPanel<SkylightConsole> ();

			});
			//m_skylightConsole.Show ("11");
		}
		public void Debug (GameObject go)
		{
			m_skylightConsole.Show (go + "\n");
		}

		public void Debug (string text)
		{
			//if (i < limit) {
			//	i++;
			//	return;
			//}
			m_skylightConsole.Show (text + "\n");
		}
	}

}
