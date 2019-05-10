using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Skylight
{
    /// <summary>
    /// 如果没有摄像机，那么实例化一个摄像机并把固定脚本挂上去
    /// 如果有摄像机，没有固定脚本，那么把脚本挂上去
    /// 如果有摄像机，也有固定脚本，那么什么都不做
    /// 如果有多台摄像机
    /// </summary>
	public class CameraService : GameModule<CameraService>
	{
		private GameObject m_mainCamera;

		private GameObject m_cachedCamera;

		public override void SingletonInit ()
		{
			base.SingletonInit ();
            EventManager.Instance().RegisterCallback((int)LogicType.MainMenuOpen, SetCamera);
            EventManager.Instance().RegisterCallback((int)LogicType.MainMenuClose, CameraClose);
            EventManager.Instance().RegisterCallback((int)LogicType.SceneOpen, SetCamera);
            EventManager.Instance().RegisterCallback((int)LogicType.SceneClose, CameraClose);

        }

		public GameObject GetCamera ()
		{

			Camera [] cameras = Camera.allCameras;
			Debug.Log ("Camera Length:" + cameras.Length);
			for (int i = 0; i < cameras.Length; i++) {
				Debug.Log ("Camera" + i + " name: " + cameras [i].name);
				if (cameras [i].name != "AutoCamera") {
					return cameras [i].gameObject;
				}
			}
			Debug.Log ("This scene doean`t contain a camera!");
			return null;

		}

		private void SetCachedCamera (bool flag)
		{
			if (m_cachedCamera == null) {
                m_cachedCamera = PrefabManager.Instance().LoadPrefab("");
				m_cachedCamera = Instantiate (m_cachedCamera);
				m_cachedCamera.transform.SetParent (transform);
			}

			m_cachedCamera.SetActive (flag);
		}

		public bool SetCamera (System.Object vars = null)
		{

			m_mainCamera = GetCamera ();
			if (m_mainCamera == null) {
				SetCachedCamera (true);

			}
			return true;
		}

		public bool CameraClose (System.Object vars = null)
		{
			Camera [] cameras = Camera.allCameras;
			Debug.Log ("Camera Length:" + cameras.Length);
			for (int i = 0; i < cameras.Length; i++) {
				Debug.Log ("Camera" + i + " name: " + cameras [i].name);

				cameras [i].gameObject.SetActive (false);
			}

			m_mainCamera = null;

			return true;
		}

	}
}