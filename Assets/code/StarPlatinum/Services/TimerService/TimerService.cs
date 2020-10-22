using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum;
using StarPlatinum.Base;

namespace StarPlatinum
{
	public class TimerService : Singleton<TimerService>
	{
		GameObject m_timerMono;

		public class Timer
		{
			public TimerCallback m_timer;
			public float m_lastTime;
			public Timer (TimerCallback timer, float lastTimer)
			{
				m_timer = timer;
				m_lastTime = lastTimer;
			}
		}

		public delegate void TimerCallback ();

		public List<Timer> m_timers;

		public TimerService ()
		{
			Transform root = GameObject.Find ("GameRoot").transform;
			if (root == null) return;

			m_timerMono = new GameObject ("Timer");
			m_timerMono.transform.SetParent (root.transform);
			m_timerMono.AddComponent<TimerServiceMono> ().SetTimerCallback (FixedUpdate);

			m_timers = new List<Timer> ();
			m_readyForDelete = new List<Timer> ();

		}

		//   public override void SingletonInit ()
		//{
		//	base.SingletonInit ();


		//}
		List<Timer> m_readyForDelete;

		private void FixedUpdate ()
		{
			if (m_timers == null) {
				Debug.Log ("Timer is null");
				return;
			};
			int count = m_timers.Count;
			if (count != 0) {
				for (int i = 0; i < count; i++) {
					m_timers [i].m_lastTime -= Time.deltaTime;
					if (m_timers [i].m_lastTime <= 0) {
						m_readyForDelete.Add (m_timers [i]);
						if (m_timers [i].m_timer.Target.ToString () != "null") {
							m_timers [i].m_timer ();
						} else {
							Debug.Log ("Timer lose target before execute.");
						}
					}
				}
			}
			if (m_readyForDelete.Count != 0) {
				for (int i = 0; i < m_readyForDelete.Count; i++) {
					m_timers.Remove (m_readyForDelete [i]);
				}
			}

		}

		public void AddTimer (float time, TimerCallback timer)
		{
			m_timers.Add (new Timer (timer, time));
		}
	}
}