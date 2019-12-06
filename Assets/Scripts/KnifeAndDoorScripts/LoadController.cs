using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadController : MonoBehaviour
{


	public void Reload (GameObject go)
	{
		for (int i = transform.childCount - 1; i >= 0; i--) {

			Transform tran = transform.GetChild (i);
			SaveController save = tran.GetComponent<SaveController> ();

			bool isPassed = save.GetIsPassed ();

			if (isPassed) {

				go.transform.position = tran.position;
				go.GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
				return;
			}
		}
	}
}
