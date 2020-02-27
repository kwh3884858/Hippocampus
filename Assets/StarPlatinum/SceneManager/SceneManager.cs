using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using StarPlatinum.Service;

//depend on EventManager
namespace StarPlatinum
{
	public enum SceneLoadMode
	{
		Single = UnityEngine.SceneManagement.LoadSceneMode.Single,
		Additive = UnityEngine.SceneManagement.LoadSceneMode.Additive

	}
	public class SceneManager : GameModule<SceneManager>
	{

		Dictionary<string, GameObject> m_allScenes = new Dictionary<string, GameObject> ();
		//private UnityEngine.SceneManagement.Scene mCurrentScene;
		//AsyncOperation asyncOperation = new AsyncOperation ();

		List<string> m_loadedScene = new List<string> ();

		public delegate void callback (SceneLoadedEvent e);
		List<callback> m_loadedSceneEvent = new List<callback> ();

		//private string m_currentSceneName;
		//public string CurrentSceneName =>  m_currentSceneName;

		private SceneLookupEnum m_currentScene = SceneLookupEnum.GameRoot;
		public SceneLookupEnum GetCurrentScene => m_currentScene;
		public SceneLookupEnum SetCurrentScene (SceneLookupEnum currentScene)
		{
			m_currentScene = currentScene;
			return m_currentScene;
		}

		//public UnityEngine.SceneManagement.Scene m_currentScene
		//{
		//    get; set;
		//}

		public override void SingletonInit ()
		{
			base.SingletonInit ();
			//EventManager.Instance.AddEventListener<SceneLoadedEvent>(SceneLoadedCallBack);

			AddSceneLoadedEvent (DisableAllUICanvas);
			AddSceneLoadedEvent (LoadSceneScript);
		}

		private void LoadSceneScript (SceneLoadedEvent e)
		{
			string path = AssetsManager.APPLICATION_PATH;
			path += "Scripts/Scenes/";

			if (!Directory.Exists (path)) {
				return;
			}

			//path += m_currentSceneName;

			if (!File.Exists (path)) {
				return;
			}


		}

		private void DisableAllUICanvas (SceneLoadedEvent e)
		{

			foreach (GraphicRaycaster go in GameObject.FindObjectsOfType<GraphicRaycaster> ()) {
				go.gameObject.SetActive (false);

				Debug.Log (go.gameObject.name + " disable, UI should be loaded by UIManager");
			}
		}

		//TODO:Find same scene name gameobject and find same name componennt and run it.
		void FindSceneGameObject ()
		{

		}


		public void LoadScene (SceneLookupEnum sceneName, SceneLoadMode loadMode, object sceneData = null)
		{
			m_currentScene = sceneName;
			LoadScene (SceneLookup.Get (sceneName), loadMode, sceneData);
			CameraService.Instance.UpdateCurrentCamera ();
		}
		private void LoadScene (string sceneName, SceneLoadMode loadMode, object sceneData = null)
		{
			//string sceneName = typeof (T).ToString ();


			//GameObject uiObject;
			//string perfbName = "Scenes/" + sceneName;
			if (m_loadedScene.Contains (sceneName)) {
				return;
			}
			//m_currentSceneName = sceneName;
			sceneName = sceneName.ToLower ();
			m_loadedScene.Add (sceneName);
			//Debug.Log ("Loaded Perfab : " + perfbName);
			if (loadMode == SceneLoadMode.Single) {
				AssetsManager.Instance ().LoadScene (
					sceneName,
					UnityEngine.SceneManagement.LoadSceneMode.Single);
			} else if (loadMode == SceneLoadMode.Additive) {
				AssetsManager.Instance ().LoadScene (
					sceneName,
					UnityEngine.SceneManagement.LoadSceneMode.Additive);
			}

			//UnityEngine.SceneManagement.LoadSceneMode.Single

			return;


		}
		private void FinishLoadScene<T> () where T : BaseScene
		{
			//UnityEngine.SceneManagement.Scene scene;
			//int i;
			//for (i = UnityEngine.SceneManagement.SceneManager.sceneCount - 1; i >= 0; i--) {

			//	scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt (i);
			//	if (!mAllScenes.Keys.Contains (scene.name)) {
			//		mAllScenes.Add (scene.name, scene);
			//		break;
			//	}

			//}
			//i = (i == -1) ? 0 : i;
			//scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt (i);


		}



		public void UnloadScene (string sceneName)
		{
			if (m_loadedScene.Contains (sceneName.ToLower ())) {

				StartCoroutine (UnloadSceneEnumerator (sceneName));
			} else {
				Debug.Log (sceneName + " doesn`t exist.");
			}
		}

		IEnumerator UnloadSceneEnumerator (string sceneName)
		{
			AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync (sceneName);
			while (operation.isDone != true) {
				yield return null;
			}


		}
		/*
        public GameObject SetActiveScene<T>(object sceneData = null) where T : BaseScene
        {
            string sceneName = typeof(T).ToString();
            if (m_currentScene.IsValid() == true && m_currentScene.name != sceneName)
            {
                EventManager.Instance.SendEvent<SceneLeaveEvent>(new SceneLeaveEvent(sceneName));
                Debug.Log("Leave Scene" + sceneName);
                //mCurrentScene = null;
            }
            //Find target scene, and set it as current scene
            UnityEngine.SceneManagement.Scene scene =
            UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);

            UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
            m_currentScene = scene;

            //m_currentScene = scene;

            GameObject go;
            T t;
            if (!m_allScenes.TryGetValue(sceneName, out go))
            {
                go = GameObject.Instantiate(new GameObject(scene.name));
                t = go.AddComponent<T>();
                t.SceneInit(scene.name);

                m_allScenes.Add(scene.name, go);
            }
            else
            {
                t = go.GetComponent<T>();
            }
            EventManager.Instance.SendEvent<SceneEnterEvent>(new SceneEnterEvent(sceneName));

            //sceneGo.transform.SetParent (transform);

            //FinishLoadScene ();
            //callback ();

            //Active scene not current scene

            //GameObject go = m_allScenes [sceneName];
            //m_currentScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName (sceneName);
            //UnityEngine.SceneManagement.SceneManager.SetActiveScene (m_currentScene);
            return go;
        }
        */
		public void AddSceneLoadedEvent (callback func)
		{
			m_loadedSceneEvent.Add (func);
		}

		public void RemoveSceneLoadedEvent (callback func)
		{
			m_loadedSceneEvent.Remove (func);
		}

		public void SceneLoadedCallBack (object sender, SceneLoadedEvent showSceneEvent)
		{
			if (m_loadedSceneEvent != null) {
				foreach (callback call in m_loadedSceneEvent) {
					call?.Invoke (showSceneEvent);
				}

			}
		}
		/*
		public void ReloadScene<T> () where T : BaseScene
		{
			if (!mCurrentScene) {
				Debug.Log ("Dont have current scene!");
				return;
			}

			string sceneName = typeof (T).ToString ();

			if (sceneName == mCurrentScene.name) {
				Debug.Log ("Current scene is ready reload");
				mCurrentScene = null;

			}
			BaseScene destoryedScene = mAllScenes [sceneName].GetComponent<BaseScene> ();
			destoryedScene.SceneDestory ();
			if (!mAllScenes.Remove (sceneName)) {
				Debug.Log ("Current scene doesn`t exist in all scenes cache !");
			}

			Destroy (destoryedScene.gameObject);

			LoadScene<T> ();

			//ShowScene<>
		}
		*/
		//public void LoadScene

		/*
	public void CloseScene ()
	{
		if (mCurrentScene) {
			mCurrentScene.GetComponent<BaseScene> ().SceneClose ();
			mCurrentScene.gameObject.SetActive (false);
			mCurrentScene = null;
		}
	}

	public void UnloadAllScene ()
	{

		Dictionary<string, GameObject>.Enumerator etor = mAllScenes.GetEnumerator ();
		while (etor.MoveNext ()) {
			Destroy (etor.Current.Value);
		}
		mAllScenes.Clear ();

	}
	*/
	}
}