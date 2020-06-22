using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace GamePlay.Stage
{
    public class CoreControllerInitialize : MonoBehaviour
    {
        public GameObject m_player;
        public GameObject m_camera;
        // Start is called before the first frame update
        void Start()
        {
            Assert.IsTrue(m_player != null, "Player is not set.");
            Assert.IsTrue(m_camera != null, "Camera is not set.");

            CoreContainer.Instance.SetSceneLoaded(m_player,m_camera, true);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}