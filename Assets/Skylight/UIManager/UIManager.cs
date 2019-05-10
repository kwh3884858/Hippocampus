using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEngine.UI;

namespace Skylight
{
	public class UIManager : GameModule<UIManager>
	{
		private GameObject m_dialog;
		private GameObject m_panel;
		private GameObject m_overlay;
		private GameObject m_box;

		private Stack<GameObject> m_dialogs = new Stack<GameObject> ();
		private UIDialog m_currentDialog = null;
		private UIBox m_currentBox = null;

		public delegate void Callback ();

		public override void SingletonInit ()
		{
			transform.SetParent (GameRoot.Instance ().transform);
			m_box = AddGameObject ("box");
			m_panel = AddGameObject ("panel");
			m_dialog = AddGameObject ("dialog");
			m_overlay = AddGameObject ("overlay");

		}

		public T ShowDialog<T> (Dictionary<string, object> varList = null) where T : UIDialog
		{
			if (m_currentDialog != null) {
				m_currentDialog.gameObject.SetActive (false);
				m_currentDialog = null;
			}

			string name = typeof (T).ToString ();
			GameObject uiObject;

			Transform dialogTran = m_panel.transform.Find (name);

			if (!dialogTran) {
				string perfbName = "UI/Dialog/" + typeof (T).ToString ();
				Debug.Log (perfbName);
				GameObject perfb = null;
				if (perfb == null) {
					Debug.Log ("UIDialog Can`t Find Perfab");
				}
				uiObject = GameObject.Instantiate (perfb);
				uiObject.name = name;
				T t = uiObject.AddComponent<T> ();
				uiObject.transform.SetParent (m_dialog.transform);

				t.PanelInit ();

			} else {
				uiObject = dialogTran.gameObject;
			}

			if (uiObject) {
				T panel = uiObject.GetComponent<T> ();
				panel.PanelOpen ();
				if (varList != null)
					panel.m_userData = varList;

				m_currentDialog = panel;

				m_dialogs.Push (uiObject);

				uiObject.SetActive (true);

			}
			return uiObject.GetComponent<T> ();
		}

		public void CloseAllDialogs ()
		{
			while (m_dialogs.Count != 0) {
				CloseCurrentDialog ();
			}
		}
		public void CloseCurrentDialog ()
		{
			m_currentDialog.GetComponent<UIDialog> ().PanelClose ();

			m_currentDialog.gameObject.SetActive (false);
			m_currentDialog = null;
			if (m_dialogs.Count != 0) {
				GameObject uiDialog = m_dialogs.Pop ();
				uiDialog.SetActive (true);
				m_currentDialog = uiDialog.GetComponent<UIDialog> ();

			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="text">显示在屏幕上的文字</param>
		/// <param name="rectPos">显示在屏幕上的位置,左下角是（0,0)</param>
		/// <param name="offset">UI在屏幕上的偏移，UI的位置会被限制在(x-offseX,y-offsetY)之中</param>
		/// <param name="WaitingTime">等待消失的时间</param>
		/// <param name="isAutoFade">是否需要自动消失，false会自动屏蔽等待时间</param>
		/// <param name="varList">变量列表</param>
		/// <returns></returns>
		public T ShowOverlay<T> (string text,
								 Vector3 rectPos = default (Vector3),
								 Vector2 offset = default (Vector2),
								 float WaitingTime = 1.5f,
								 bool isAutoFade = true,
								 Dictionary<string, object> varList = null
								 ) where T : UIOverlay
		{
			rectPos = new Vector3 (380, 700, 0);
			offset = new Vector2 (200, 100);
			string name = typeof (T).ToString ();
			Debug.Log (name);
			var panelTran = m_panel.transform.Find (name);
			GameObject uiObject;
			T panel = null;
			if (panelTran == null) {
				string perfbName = "UI/Overlay/" + typeof (T).ToString ();
				GameObject perfb = null;
				if (perfb == null) {
					Debug.Log ("UIOverlay Can`t Find Perfab");
				}
				uiObject = GameObject.Instantiate (perfb);
				uiObject.name = name;
				T t = uiObject.AddComponent<T> ();
				uiObject.transform.SetParent (m_overlay.transform);
				float maxWidth = Screen.width - offset.x;
				float maxHeight = Screen.height - offset.y;
				if (rectPos.x > maxWidth) {
					rectPos.x = maxWidth;
				}
				if (rectPos.x < offset.x) {
					rectPos.x = offset.x;
				}
				if (rectPos.y > maxHeight) {
					rectPos.y = maxHeight;
				}
				if (rectPos.y < offset.y) {
					rectPos.y = offset.y;
				}
				//Debug.Log("(uiObject.transform " + uiObject.transform.GetChild(0).transform.position);
				uiObject.transform.GetChild (0).transform.position = rectPos;
				//Debug.Log("After calculate, uiObject RectTransform " + uiObject.GetComponent<RectTransform>().position);

				t.PanelInit ();
			} else {
				uiObject = panelTran.gameObject;
			}
			if (uiObject) {
				panel = uiObject.GetComponent<T> ();
				panel.ShowMsg (text);
				panel.PanelOpen ();
				if (varList != null)
					panel.m_userData = varList;

				uiObject.SetActive (true);
				if (isAutoFade) {
					StartCoroutine (DelayToInvokeDo (() => {
						AnimationService.Instance ().UIToFade (uiObject.transform);
					}, WaitingTime));

					StartCoroutine (DelayToInvokeDo (() => {
						panel.Callback ();
					}, 1.5f + WaitingTime));
				}
			}
			return panel;
		}


		IEnumerator DelayToInvokeDo (Action action, float delaySeconds)
		{
			yield return new WaitForSeconds (delaySeconds);
			action ();
		}

		/// <summary>
		/// Shows the panel.
		/// 
		/// 显示一个panel，panel是最为简单的UI元素，只是单纯显示在画面中，
		/// 所有的处理都交给Panel本身解决，建议在复杂频繁的UI切换中中不要使用panel
		/// 或者把一系列Panel置于一个父对象上统一管理开关
		/// </summary>
		/// <returns>The panel.</returns>
		/// <param name="isFramework">这个UI是否为框架使用的。设为<c>true</c>是用于框架级的UI预设。通常都是false。</param>
		/// <param name="varList">Variable list.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void ShowPanel<T> (bool isFramework = false,
			Callback callback = null,
		 Dictionary<string, object> varList = null) where T : UIPanel
		{
			string panelName = typeof (T).ToString ();
			//Debug.Log (name);
			Transform panelTran = m_panel.transform.Find (panelName);
			GameObject uiObject;
			T panel = null;
			if (panelTran == null) {
				string perfbName;
				if (!isFramework) {
					perfbName = string.Format ("UI/Panel/{0}.unity3d", typeof (T));
				} else {
					perfbName = string.Format ("Skylight/UI/Panel/{0}.unity3d", typeof (T));
				}
				AssetsManager.Instance ().LoadPrefab (perfbName, (AssetBundle obj) => {
					GameObject perfb;
					perfb = obj.LoadAsset<GameObject> (panelName);
					//perfb = obj.mainAsset as GameObject;
					if (perfb == null) {
						Debug.Log ("UIPanel Can`t Find Perfab");
						return;
					}
					uiObject = GameObject.Instantiate (perfb);
					uiObject.name = panelName;
					T t = uiObject.AddComponent<T> ();
					uiObject.transform.SetParent (m_panel.transform);

					t.PanelInit ();

					OpenUIPanel<T> (uiObject);
					if (callback != null) {
						callback ();

					}
				});
				//if (perfb == null) {
				//	Debug.Log ("UIPanel Can`t Find Perfab");
				//}
				//uiObject = GameObject.Instantiate (perfb);
				//uiObject.name = name;
				//T t = uiObject.AddComponent<T> ();
				//uiObject.transform.SetParent (m_panel.transform);

				//t.PanelInit ();

			} else {
				uiObject = panelTran.gameObject;
				OpenUIPanel<T> (uiObject);
				callback ();
			}

			return;
		}

		void OpenUIPanel<T> (GameObject uiObject, Dictionary<string, object> varList = null) where T : UIPanel
		{
			if (uiObject) {
				T panel = uiObject.GetComponent<T> ();
				panel.PanelOpen ();
				if (varList != null)
					panel.m_userData = varList;

				uiObject.SetActive (true);
			}

		}


		public void ClosePanel<T> () where T : UIPanel
		{
			string name = typeof (T).ToString ();
			Transform panelTran = m_panel.transform.Find (name);

			if (panelTran != null) {
				if (panelTran.gameObject.activeSelf == false) {
					return;
				}
				panelTran.GetComponent<T> ().PanelClose ();
				panelTran.gameObject.SetActive (false);
			}

		}

		public T GetPanel<T> () where T : UIPanel
		{

			return m_panel.transform.Find (typeof (T).ToString ()).GetComponent<T> ();
		}

		public void UnloadPanel<T> (float time = 0) where T : UIPanel
		{
			string name = typeof (T).ToString ();
			Transform panelTran = m_panel.transform.Find (name);

			if (panelTran != null) {
				// panelTran.GetComponent<T>().PanelClose();
				Destroy (panelTran.gameObject, time);
			}

		}

		public void ShowBox<T> (Dictionary<string, object> varList = null) where T : UIBox
		{
			string name = typeof (T).ToString ();

			var panelTran = m_box.transform.Find (name);
			GameObject uiObject;
			if (panelTran == null) {
				string perfbName = "UI/Box/" + typeof (T).ToString ();

				//GameObject perfb = AssetsManager.LoadPrefab<GameObject> (perfbName);
				AssetsManager.Instance ().LoadPrefab (perfbName, (AssetBundle obj) => {
					GameObject perfb;
					if (obj == null) {
						Debug.Log ("UIPanel Can`t Find Perfab");
					}
					perfb = obj.mainAsset as GameObject;

					uiObject = GameObject.Instantiate (perfb);
					uiObject.name = obj.name.Substring (0, obj.name.IndexOf ('.'));
					T t = uiObject.AddComponent<T> ();
					uiObject.transform.SetParent (m_box.transform);

					t.PanelInit ();

					OpenUIBoxOpen<T> (uiObject);
				});
				//uiObject = GameObject.Instantiate (perfb);
				//uiObject.name = name;
				//T t = uiObject.AddComponent<T> ();
				//uiObject.transform.SetParent (m_box.transform);
				//t.PanelInit ();
			} else {
				uiObject = panelTran.gameObject;
				OpenUIBoxOpen<T> (uiObject);
			}
			//if (uiObject) {
			//	T box = uiObject.GetComponent<T> ();
			//	box.PanelOpen ();
			//	if (varList != null)
			//		box.m_userData = varList;

			//	if (m_currentBox)
			//		m_currentBox.gameObject.SetActive (false);

			//	uiObject.SetActive (true);
			//	m_currentBox = box;
			//}
		}

		private void OpenUIBoxOpen<T> (GameObject uiObject, Dictionary<string, object> varList = null) where T : UIBox
		{
			if (uiObject) {
				T box = uiObject.GetComponent<T> ();
				box.PanelOpen ();
				if (varList != null)
					box.m_userData = varList;

				if (m_currentBox)
					m_currentBox.gameObject.SetActive (false);

				uiObject.SetActive (true);
				m_currentBox = box;
			}
		}

		public void CloseBox ()
		{
			if (m_currentBox)
				m_currentBox.gameObject.SetActive (false);

			m_currentBox = null;
		}
	}
}