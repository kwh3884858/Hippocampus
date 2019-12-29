using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace StarPlatinum
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
    public class PrefabManager : Singleton<PrefabManager>
    {

        Dictionary<string, GameObject> m_allPrefab = new Dictionary<string, GameObject> ();
        public GameObject LoadPrefab (string prefabName)
        {
            //string name = "Prefabs/" + prefabName;
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
        
        public void InstantiateAsync<T>(string key,Action<RequestResult> callBack,Transform parent =null) where T: UnityEngine.Object
        {
            Addressables.LoadAsset<T>(key).Completed+= operation =>
            {
                var result = GetResult(key,operation);
                if (result.status == RequestStatus.FAIL)
                {
                    callBack?.Invoke(result);
                    return;
                }
                result.result = Object.Instantiate(operation.Result, parent);
                callBack?.Invoke(result);
            };
        }

        public void InstantiateComponentAsync<T>(string key,Action<RequestResult> callBack,Transform parent =null) where T: UnityEngine.Object
        {
            Addressables.LoadAsset<GameObject>(key).Completed+= operation =>
            {
                var result = GetResult(key,operation);
                if (result.status == RequestStatus.FAIL)
                {
                    callBack?.Invoke(result);
                    return;
                }
                var obj = Object.Instantiate(operation.Result, parent);
                result.result = obj.GetComponent<T>();
                callBack?.Invoke(result);
            };
        }
        
        public void InstantiateConfigAsync(string key,Action<RequestResult> callBack,Transform parent =null)
        {
            Addressables.LoadAsset<ScriptableObject>(key).Completed+= operation =>
            {
                var result = GetResult(key,operation);
                if (result.status == RequestStatus.FAIL)
                {
                    callBack?.Invoke(result);
                    return;
                }
                result.result = Object.Instantiate(operation.Result, parent);
                callBack?.Invoke(result);
            };
        }

        public void LoadAssetsAsync<T>(List<string> keys, Action<RequestResult> callBack, Transform parent = null)where T: UnityEngine.Object
        {
            foreach (var key in keys)
            {
                Addressables.LoadAsset<T>(key).Completed += operation =>
                {
                    var result = GetResult(key, operation);
                    callBack?.Invoke(result);
                };
            }
        }

        public void LoadScene(SceneLookupEnum key, LoadSceneMode loadSceneMode)
        {
            Addressables.LoadScene(SceneLookup.Get(key),loadSceneMode);
        }

        private RequestResult GetResult<T>(string key,AsyncOperationHandle<T> operation)where T: UnityEngine.Object
        {
            RequestResult result = new RequestResult();
            result.key = key;
            if (operation.Result == null)
            {
                result.status = RequestStatus.FAIL;
                return result;
            }
            result.result = operation.Result;
            result.status = RequestStatus.SUCCESS;
            return result;
        }


        public void UploadPrefab (string name)
        {
            GameObject temp = m_allPrefab [name];
            m_allPrefab.Remove (name);
            Object.Destroy (temp);
        }

        public void UploadAllPrefab ()
        {

            Dictionary<string, GameObject>.Enumerator etor = m_allPrefab.GetEnumerator ();
            while (etor.MoveNext ()) {

                GameObject.Destroy(etor.Current.Value);

            }
            m_allPrefab.Clear ();
        }
    }
}