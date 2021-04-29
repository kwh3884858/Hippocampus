using Config.GameRoot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Base;
using StarPlatinum.Manager;
using GamePlay.Stage;
using UnityEngine.Assertions;
using UnityEngine.Rendering.Universal;
using System;

namespace StarPlatinum.Services
{
    /// <summary>
    /// 如果没有摄像机，那么实例化一个摄像机并把固定脚本挂上去
    /// 如果有摄像机，没有固定脚本，那么把脚本挂上去
    /// 如果有摄像机，也有固定脚本，那么什么都不做
    /// 如果有多台摄像机
    /// </summary>
    public class CameraService : Singleton<CameraService>
    {
        public void SetMainCamera(GameObject mainCamera)
        {
            if (m_mainCamera != null)
            {
                GameObject.Destroy(m_mainCamera);
            }
            m_mainCamera = mainCamera;
            SetUICamera();
            SetRealCamera();

        }
        public GameObject GetMainCamera()
        {
            if (m_mainCamera == null || m_mainCamera.activeInHierarchy == false)
            {
                Debug.LogError("Camera is expired");
            }

            bool result = UpdateCurrentCamera();
            if (!result)
            {
                return null;
            }

            return m_mainCamera;
        }

        public Camera GetMainCameraComponent()
        {
            var mainCamera = GetMainCamera();
            if (mainCamera == null)
            {
                return null;
            }

            return mainCamera.GetComponent<Camera>();
        }

        public void InitializeRaycast()
        {
            if (m_mainCamera)
            {
                m_rayCastDetection = m_mainCamera.GetComponent<RayCastDetection>();
            }
            else
            {
                Debug.LogError("Raycast detection is not initialized");
            }
        }

        public void SwitchRaycastState(bool state)
        {
            m_rayCastDetection.SwitchDetection(state);
        }

        public bool UpdateCurrentCamera()
        {
            m_cameraController = null;

            CloseAllCamera();


            //SceneLookupEnum sceneEnum = GameSceneManager.Instance.GetCurrentSceneEnum ();
            //SceneCameraType cameraType = ConfigRoot.Instance.GetCameraTypeBySceneName (sceneEnum.ToString ());

            m_cameraController = m_mainCamera.GetComponent<CameraController>();
            if (m_cameraController == null)
            {
                m_cameraController = m_mainCamera.AddComponent<CameraController>();

            }

            m_cameraController.Refresh();


            return true;
        }

        public Camera GetUICamera() { return m_uiCamera; }

        public void SetTarget(GameObject target)
        {
            if (m_cameraController != null)
            {
                m_cameraController.SetTarget(target);
            }
        }

        public SceneCameraType GetSceneCameraType() { return m_sceneCameraType; }

        private void CloseAllCamera()
        {
            Camera[] cameras = Camera.allCameras;

            Assert.IsTrue(cameras.Length > 0, "Camera length is wrong");

            for (int i = 0; i < cameras.Length; i++)
            {
                if (cameras[i].gameObject != m_mainCamera && cameras[i] != m_uiCamera && cameras[i] != m_realCamera)
                {
                    cameras[i].gameObject.SetActive(false);
                }
            }
        }

        //private GameObject GetCamera ()
        //{

        //	Camera [] cameras = Camera.allCameras;
        //	Debug.Log ("Camera Length:" + cameras.Length);

        //	if (Camera.allCamerasCount == 0) {
        //		Debug.Log ("This scene does`t contain a camera!");

        //		return null;
        //	}

        //	if (cameras.Length > 1) {
        //		for (int i = 1; i < cameras.Length; i++) {
        //			cameras [i].gameObject.SetActive (false);
        //			GameObject.Destroy (cameras [i].gameObject);
        //		}
        //	}

        //	return cameras [0].gameObject;
        //}

        //private GameObject CreateCamera ()
        //{
        //	GameObject root = GameObject.Find ("GameRoot");
        //	if (root == null) return null;

        //	PrefabManager.Instance.InstantiateAsync<GameObject> ("MainCamera", (result) => {
        //		GameObject cameraGameObject = result.result as GameObject;

        //		Camera camera = cameraGameObject.GetComponent<Camera> ();
        //		if (camera == null) {
        //			cameraGameObject.AddComponent<Camera> ();
        //			cameraGameObject.transform.SetParent (root.transform);
        //		}
        //	});
        //	return camera;
        //}

        private void SetUICamera()
        {
            GameObject uiCamera = GameObject.Find("UICamera");

            if (uiCamera == null)
            {
                return;
            }

            var camera = uiCamera.GetComponent<Camera>();
            var mainCamera = m_mainCamera.GetComponent<Camera>();
            var cameraData = mainCamera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(camera);
            m_uiCamera = camera;

        }

        private void SetRealCamera()
        {
            GameObject realCamera = GameObject.Find("RealCamera");
            if (realCamera != null)
            {
                m_realCamera = realCamera.GetComponent<Camera>();
            }
        }

        public enum SceneCameraType
        {
            FirstPerson,
            ThirdPerson
        }

        [SerializeField]
        private GameObject m_mainCamera;
        private Camera m_uiCamera;
        private Camera m_realCamera;
        [SerializeField]
        private CameraController m_cameraController;

        private SceneCameraType m_sceneCameraType = SceneCameraType.ThirdPerson;

        private GameObject camera;
        private RayCastDetection m_rayCastDetection;
    }
}