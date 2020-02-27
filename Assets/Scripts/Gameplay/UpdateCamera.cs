using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarPlatinum.Service;
public class UpdateCamera : MonoBehaviour
{
	// Start is called before the first frame update
	void Start ()
	{
		CameraService.Instance.UpdateCurrentCamera ();
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
