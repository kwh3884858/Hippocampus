using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Skylight
{
	public class GameRoot : MonoSingleton<GameRoot>
	{
		VirtualMachineInterface virtualMachineInterface;

		// Use this for initialization
		void Start ()
		{
			//AddGameObject<NetService>();
			//DONT CHANGE ORDER
			//不要修改顺序，有相互依赖关系
			AddGameObject<EventManager> ();


			AddGameObject<PollerService> ();

			//依赖Poller
			//AddGameObject<Localization> ();
			//Initialize asset bundle loader and assetmanager
			AddGameObject<AssetsManager> ();

			StartCoroutine (AfterInitialize ());
		}

		IEnumerator AfterInitialize ()
		{
			//After Assetmanager is loaded, loading other modole
			while (!AssetsManager.Instance ().IsAssetBundleManagerLoaded) {
				yield return null;
			}
			AddGameObject<SceneManager> ();
			AddGameObject<SoundService> ();

			AddGameObject<UIManager> ();

			AddGameObject<PrefabManager> ();
			EventManager.Instance ().LogicStart ();
			//   AddGameObject<CameraService>();
			AddGameObject<InputService> ();
			AddGameObject<TimerService> ();
			AddGameObject<Console> ();
			AddGameObject<Localization> ();
			//UIManager.Instance().ShowPanel<UIButtonPanel>();
			//SceneManager.Instance().LoadScene()
			// UIManager.Instance().ShowPanel<UIButtonPanel>();
			//SceneManager.Instance ().ShowScene<SceneCave> ();
			//UIManager.Instance ().ShowPanel<UIMainMenu> ();
			//UIManager.Instance().ShowPanel<UIMainMenuPanel>();

			//AddEntitas ();

			SceneManager.Instance ().AddSceneLoadedEvent (Handlecallback);

			//yield return null;
			SceneManager.Instance ().LoadScene (SceneLookupEnum.SceneKnifeAndTransformDoor, SceneLoadMode.Additive);
			//SceneManager.Instance ().LoadScene ("scene2", SceneLoadMode.Additive);



		}
		// Update is called once per frame
		void Update ()
		{
			//if (virtualMachineInterface != null) {
			//	virtualMachineInterface.Update ();

			//}
		}

		void Handlecallback (SceneLoadedEvent loadedEvent)
		{

			//if (loadedEvent.GetSceneName () == SceneLookupEnum.PrototypeA.ToString ().ToLower ()) {
			//	UIManager.Instance ().ShowPanel<UIDirectionButtonPanel> ();
			//	UIManager.Instance ().ShowPanel<UIControllerPanel> ();
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


	}
}
