using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skylight
{
	public class PollerService : GameModule<PollerService>
	{

		public delegate bool Poller ();
		public float m_interval;

		private List<int> m_allowList;
		private bool m_isDoEvent;

		private float m_recorder;

		Dictionary<int, List<Poller>> m_pollers;
		//List<Poller> m_readyForDelete;

		public override void SingletonInit ()
		{
			base.SingletonInit ();
			m_pollers = new Dictionary<int, List<Poller>> ();
			//m_readyForDelete = new List<Poller>();
			m_allowList = new List<int> ();
			m_recorder = 0;
			m_interval = 0.1f;
			m_isDoEvent = true;
		}
		//每过固定时间调用一次
		private void FixedUpdate ()
		{
			if (!m_isDoEvent) return;

			m_recorder += Time.fixedDeltaTime;
			if (m_recorder > m_interval) {

				DoEvent ();

				m_recorder = 0;
			}
		}
		/// <summary>
		/// 开启一个轮询列表。
		/// 轮询器中可能存在不同的轮询列表用于存放不同的轮询任务，
		///	这样即使其中一种类型轮询关闭了，也不会影响到其他的轮询
		/// Open a polling list. 
		/// There may be different polling lists in the poller for different polling tasks,
		/// so that even if one of the types of polling lists is closed,
		/// it will not affect other polling lists.
		/// </summary>
		/// <param name="openId">Open specific polling list.</param>
		public void OpenPollerList (int openId)
		{
			Debug.Log ("Open " + (SkylightStaticData.PollerType)openId + " Poller List");
			if (m_allowList.Contains (openId)) {
				return;
			}
			m_allowList.Add (openId);
		}

		public void ClosePollerList (int closeId)
		{
			Debug.Log ("Close " + (SkylightStaticData.PollerType)closeId + " Poller List");

			if (!m_allowList.Contains (closeId)) {
				return;
			}
			m_allowList.Remove (closeId);
		}

		public void RegisterPoller (int pollerId, Poller poller)
		{
			if (!m_pollers.ContainsKey (pollerId)) {
				m_pollers.Add (pollerId, new List<Poller> ());

			}


			if (!m_pollers [pollerId].Contains (poller)) {
				m_pollers [pollerId].Add (poller);
			} else {
				Debug.Log ("Already Exist This Callback Poller");
			}
		}

		public bool Contain (int pollerId, Poller poller)
		{
			if (!m_pollers.ContainsKey (pollerId)) {
				return false;

			}


			if (!m_pollers [pollerId].Contains (poller)) {
				return false;
			} else {
				return true;
			}

		}

		public void UnsignPoller (int pollerId, Poller poller)
		{

			if (m_pollers.ContainsKey (pollerId)) {
				for (int i = 0; i < m_pollers [pollerId].Count; i++) {
					if (m_pollers [pollerId] [i] == poller) {
						m_pollers [pollerId].RemoveAt (i);
					}
				}
			}
			if (m_pollers [pollerId].Count == 0) {
				m_pollers.Remove (pollerId);
			}
		}

		private void DoEvent ()
		{
			for (int i = 0; i < m_allowList.Count; i++) {

				if (!m_pollers.ContainsKey (m_allowList [i])) {
					break;
				}

				for (int j = 0; j < m_pollers [m_allowList [i]].Count; j++) {
					if (m_pollers [m_allowList [i]] [j].Target.Equals (null)) {
						Debug.LogWarning (m_pollers [m_allowList [i]] [j] + "is null");
						return;
					}

					if (!m_pollers [m_allowList [i]] [j] ()) {
						return;
					}

					//整个键值对都被删掉了，退出这个键值对
					if (!m_pollers.ContainsKey (m_allowList [i])) {
						break;
					}

					//其中的一个数值被删除了。跳过，继续执行
					if (m_pollers [m_allowList [i]] == null) {
						continue;
					}
				}

			}

		}

		public void StopDoEvent ()
		{
			m_isDoEvent = false;
		}

		public void StartDoEvent ()
		{
			m_isDoEvent = true;
		}
	}

}
