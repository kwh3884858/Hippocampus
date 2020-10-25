using Config.GameRoot;
using GamePlay.Global;
using GamePlay.Stage;
using StarPlatinum.Base;
using StarPlatinum.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StarPlatinum
{
	public class GameRoot : MonoSingleton<GameRoot>
	{

		public  SceneLookupEnum m_startScene { get; set; }
		public  MissionEnum m_startMission { get; set; }

		public ConfigRoot m_configRoot;
#if DevMode
		public static string m_consolePrefabName = "Debug_Console";
#endif
		VirtualMachineInterface virtualMachineInterface;

		public override void SingletonInit () { }

		// Use this for initialization
		void Start ()
		{
			PreloadData ();
			//No More Productivity! 555 >_<
			//Productivity ();
			ConfigData data = new ConfigData ();// 测试

			CoreContainer.Instance.Initialize();
			SingletonGlobalDataContainer.Instance.Initialize ();
			Evidence.EvidenceDataManager.Instance.Initialize ();
            Tips.TipsManager.Instance.Initialize();
			SoundService.Instance.Initialize();
        }

        void PreloadData ()
		{
			//ConfigRoot configRoot = ConfigRoot.Instance;
			//allData.Add (configRoot);
			StartCoroutine (CheckDataLoaded (allData, AfterLoadData));
		}

		IEnumerator CheckDataLoaded (List<System.Object> allData, Action callback)
		{
            //Load Config Root and Config Mission
            bool isLoadRoot = false;
            bool isLoadMission = false;
			ConfigRoot rootConfig = null;
            ConfigMission missionConfig = null;

            while (rootConfig == null && isLoadRoot == false) {
				rootConfig = ConfigRoot.Instance;
                isLoadRoot = true;
				yield return null;
			}

            while (missionConfig == null && isLoadMission == false)
            {
                missionConfig = ConfigMission.Instance;
                isLoadMission = true;
                yield return null;

            }
#if DevMode
			//add console
			GameObject console = GameObject.Find (m_consolePrefabName);
			if (console == null) {
				PrefabManager.Instance.InstantiateAsync<Transform> (m_consolePrefabName, (result) => {
					Debug.Log ($"===========aas:{result.key}加载完成.");
					Debug.Log ($"===========aas:{result.key}加载完成.");

					Transform tf = result.result as Transform;
					tf.gameObject.name = m_consolePrefabName;
				});
			}
#endif
			callback ();
		}

		void AfterLoadData ()
		{
		}

        public void OnGUI()
        {
            //if (GUI.Button(new Rect(0, 0, 200, 50), "Productivity"))
            //{
            //    Productivity();
            //}
        }


		// Update is called once per frame
		void Update ()
		{
			input = InputService.Instance.GetAxis (KeyMap.Horizontal);
			if (input != lastInput) {
				lastInput = input;
			}
		}

		void OnDestroy ()
		{
            SingletonGlobalDataContainer.Instance.Shutdown ();
            Evidence.EvidenceDataManager.Instance.Shutdown ();
            Tips.TipsManager.Instance.Shutdown ();
			SoundService.Instance.Shutdown();
        }

		private void Productivity ()
		{
			System.Random random = new System.Random ();
			int ran = random.Next (0, 10);

			if (ran < 3) SoundService.Instance.PlayBgm ("U", false);
			else if (ran < 6) SoundService.Instance.PlayBgm ("Ka", false);
			else if (ran < 11) SoundService.Instance.PlayBgm ("He", false);
		}
		float input = 0;
		float lastInput = 0;
		private List<System.Object> allData = new List<System.Object> ();
	}
}
