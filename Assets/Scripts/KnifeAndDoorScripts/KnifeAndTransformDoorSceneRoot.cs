using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Skylight;

public class KnifeAndTransformDoorSceneRoot : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
		UIManager.Instance ().ShowPanel<UIEquipedWeapon> ();
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
