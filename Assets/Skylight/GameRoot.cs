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
#if UNITY_EDITOR


#else
              //After Assetmanager is loaded, loading other modole
            while (!AssetsManager.Instance ().IsAssetBundleManagerLoaded) {
				yield return null;
			}
#endif

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

			//Add ECS system
			//AddEntitas ();

			SceneManager.Instance ().AddSceneLoadedEvent (Handlecallback);

			SceneManager.Instance ().LoadScene (SceneLookupEnum.Porto_2DARPG, SceneLoadMode.Additive);


            yield return null;
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


	}
}
