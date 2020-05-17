using Config.GameRoot;
using StarPlatinum.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StarPlatinum
{
	public class GameRoot : MonoSingleton<GameRoot>
	{

		public SceneLookupEnum m_startScene { get; set; }

		public override void SingletonInit () { }
		VirtualMachineInterface virtualMachineInterface;

		public void OnGUI ()
		{
			if (GUI.Button (new Rect (0, 0, 200, 50), "Productivity")) {

				Productivity ();
			}
		}

		void Productivity ()
		{
			System.Random random = new System.Random ();
			int ran = random.Next (0, 10);


			if (ran < 3) SoundService.Instance.PlayBgm ("U", false);
			else if (ran < 6) SoundService.Instance.PlayBgm ("Ka", false);
			else if (ran < 11) SoundService.Instance.PlayBgm ("He", false);

		}
		// Use this for initialization
		void Start ()
		{
			PreloadData ();
			//SoundService.Instance.PlayBgm("kendo_girls");
			//Console.Instance.Print("Hello, World!");
			Productivity ();
			ConfigData data = new ConfigData ();// 测试

			//ReadStorys readStorys = new ReadStorys("Storys/StoryTest");
			//			//DONT CHANGE ORDER
			//			//不要修改顺序，有相互依赖关系
			//			AddGameObject<EventManager> ();
			//
			//
			//			AddGameObject<PollerService> ();
			//
			//			//依赖Poller
			//			//AddGameObject<Localization> ();
			//
			//			//Initialize asset bundle loader and assetmanager
			//			AddGameObject<AssetsManager> ();
			//
			//			StartCoroutine (AfterInitialize ());
			//			SceneManager.Instance ().LoadScene (SceneLookupEnum.UITestScene, SceneLoadMode.Additive);
			//			AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (SceneLookup.Get(SceneLookupEnum.UITestScene), UnityEngine.SceneManagement.LoadSceneMode.Additive); 



			//PrefabManager.Instance.LoadScene(m_startScene, LoadSceneMode.Additive);
			//PrefabManager.Instance.LoadScene(SceneLookupEnum.GoundTestScene,LoadSceneMode.Additive);
		}

		void PreloadData ()
		{


			RootConfig rootConfig = RootConfig.Instance;
			allData.Add (rootConfig);


			StartCoroutine (CheckDataLoaded (allData, AfterLoadData));
		}

		IEnumerator CheckDataLoaded (List<System.Object> allData, Action callback)
		{
			RootConfig rootConfig = null;


			while (rootConfig == null) {
				rootConfig = RootConfig.Instance;

				yield return null;
			}


			callback ();
		}

		void AfterLoadData ()
		{
			CameraService.Instance.UpdateCurrentCamera ();

		}

		IEnumerator AfterInitialize ()
		{
#if UNITY_EDITOR


#else
              //After Assetmanager is loaded, loading other modole
            while (!AssetsManager.Instance ().IsAssetBundleManagerLoaded) {
				yield return null;
			}
#endif

			AddGameObject<SceneManager> ();
			//AddGameObject<SoundService> ();

			AddGameObject<UIManager> ();

			//AddGameObject<PrefabManager> ();
			//EventManager.Instance.LogicStart ();
			//   AddGameObject<CameraService>();
			//AddGameObject<InputService> ();
			//AddGameObject<TimerService> ();
			//AddGameObject<Console> ();
			AddGameObject<Localization> ();

			//Add ECS system
			//AddEntitas ();

			//SceneManager.Instance ().AddSceneLoadedEvent (Handlecallback);

			SceneManager.Instance ().LoadScene (SceneLookupEnum.World_UITestScene, SceneLoadMode.Additive);


			yield return null;
		}
		float input = 0;
		float lastInput = 0;
		// Update is called once per frame
		void Update ()
		{
			//if (virtualMachineInterface != null) {
			//	virtualMachineInterface.Update ();

			//}


			input = InputService.Instance.GetAxis (KeyMap.Horizontal);

			if (input != lastInput) {
				//Debug.Log(input );
				lastInput = input;
			}
		}

		void Handlecallback (SceneLoadedEvent loadedEvent)
		{
			//Show UI, if you have
			//if (loadedEvent.GetSceneName () == SceneLookupEnum.PaperPleasePrototype.ToString ().ToLower ()) {
			//	//UIManager.Instance ().ShowPanel<UIDirectionButtonPanel> ();
			//	UIManager.Instance ().ShowPanel<UIStartIntroPanel> ();
			//}

			//virtualMachineInterface = new VirtualMachineInterface ();
			//virtualMachineInterface.Start ();
			//SceneManager.Instance ().RemoveSceneLoadedEvent (Handlecallback);
		}


		public void AddSystemManger ()
		{


		}
		private void AddEntitas ()
		{
			GameObject entitas = new GameObject ("Entitas.GameControllerBehaviour");
			entitas.transform.parent = transform;

			//entitas.AddComponent<GameControllerBehaviour> ();

		}

		private List<System.Object> allData = new List<System.Object> ();
	}
}
