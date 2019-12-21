using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Skylight
{
	public enum RequestStatus
	{
		SUCCESS,
		FAIL
	}
	public struct RequestResult
	{
		public Object result;
		public string key;
		public RequestStatus status;

		public RequestResult(Object result, string key, RequestStatus status)
		{
			this.key = key;
			this.result = result;
			this.status = status;
		}
	}
	public class PrefabManager : GameModule<PrefabManager>
	{

		Dictionary<string, GameObject> m_allPrefab = new Dictionary<string, GameObject> ();

		public GameObject LoadPrefab (string prefabName)
		{
			string name = "Prefabs/" + prefabName;
			if (m_allPrefab.ContainsKey (prefabName)) {
				return m_allPrefab [prefabName];
			} else {
				GameObject perfb = null;
				if (perfb == null) {
					Debug.Log (prefabName + "can`t find in Prefabs/" + prefabName);
					return null;
				}
				m_allPrefab.Add (prefabName, perfb);
				return perfb;
			}
		}

		public async Task<GameObject> LoadAssetSync(string key)
		{
			if (m_allPrefab.ContainsKey(key))
			{
				return m_allPrefab[key];
			}
			var obj = await Addressables.LoadAsset<GameObject>(key);
			if (obj!=null)
			{
				m_allPrefab.Add(key,obj);
				return obj;
			}
			return null;
		}

		public async Task<T> InstantiateAsyncAwait<T>(string key,Transform parent =null) where T : MonoBehaviour
		{
			var objP = await Addressables.LoadAsset<GameObject>(key);
			var obj = GameObject.Instantiate<T>(objP.GetComponent<T>(), parent);
			if (obj == null)
			{
				Debug.Log(string.Format("找不到key为 {key} 的预设"));
			}
			return obj;
		}
		public async Task<GameObject> InstantiateAsyncAwait(string key,Transform parent =null)
		{
			var obj = await Addressables.Instantiate<GameObject>(key,parent);
			if (obj == null)
			{
				Debug.Log(string.Format("找不到key为 {key} 的预设"));
			}
			return obj;
		}
		
		public void InstantiateAsync<T>(string key,Action<RequestResult> callBack,Transform parent =null) where T: UnityEngine.Object
		{
			Addressables.LoadAsset<T>(key).Completed+= operation =>
			{
				RequestResult result = new RequestResult();
				result.key = key;
				if (operation.Result == null)
				{
					result.status = RequestStatus.FAIL;
					callBack?.Invoke(result);
					return;
				}
				T obj = Instantiate(operation.Result, parent);
				result.result = obj;
				result.status = RequestStatus.SUCCESS;
				callBack?.Invoke(result);
			};
		}

		public void LoadScene(string key, LoadSceneMode loadSceneMode)
		{
			Addressables.LoadScene(key,loadSceneMode);
		}


		public void UploadPrefab (string name)
		{
			GameObject temp = m_allPrefab [name];
			m_allPrefab.Remove (name);
			Destroy (temp);
		}

		public void UploadAllPrefab ()
		{

			Dictionary<string, GameObject>.Enumerator etor = m_allPrefab.GetEnumerator ();
			while (etor.MoveNext ()) {

				Destroy (etor.Current.Value);

			}
			m_allPrefab.Clear ();
		}
	}
}