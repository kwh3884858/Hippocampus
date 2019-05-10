using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Skylight
{
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